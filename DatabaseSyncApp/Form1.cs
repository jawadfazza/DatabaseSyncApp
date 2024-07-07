using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DatabaseSyncApp
{
    public partial class Form1 : Form
    {
        private const string ConfigFilePath = "connectionStrings.xml";
        private string currentUserRole;

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center the main form
            // Show login form
            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    currentUserRole = loginForm.CurrentUserRole;
                }
                else
                {
                    Application.Exit();
                }
            }

            LoadConnections();
        }

        private void LoadConnections()
        {

            List<(string, string)> connections = LoadConnectionStrings("Source");

            sourceComboBox.Items.Clear();
            sourceComboBox.Items.Add("");
            foreach ((string name, string connection) in connections)
            {
                sourceComboBox.Items.Add(name);
            }

            if (connections.Count > 0)
            {
                sourceComboBox.SelectedIndex = 0;
            }
            //Destination
            connections.Clear();
            connections = LoadConnectionStrings("Destination");

            destinationComboBox.Items.Clear();
            destinationComboBox.Items.Add("");
            foreach ((string name, string connection) in connections)
            {
                destinationComboBox.Items.Add(name);
            }

            if (connections.Count > 0)
            {
                destinationComboBox.SelectedIndex = 0;
            }
        }
        private List<(string, string)> LoadConnectionStrings(string Type)
        {
            List<(string, string)> connections = new List<(string, string)>();

            if (!File.Exists(ConfigFilePath))
            {
                CreateConfigFile();
            }

            XElement config = XElement.Load(ConfigFilePath);

            foreach (XElement connection in config.Elements("Connection"))
            {
                string name = connection.Element("Name")?.Value;
                string connectionString = connection.Element("ConnectionString")?.Value;
                string connectionType = connection.Element("Type")?.Value;
                if (connectionType == Type)
                {
                    connections.Add((name, connectionString));
                }
            }

            return connections;
        }

        private void TransferButton_Click(object sender, EventArgs e)
        {

            transferButton.Enabled = false;
            if (sourceLabel.Text != "" && destinationLabel.Text != "")
            {
                string sourceConnectionString = sourceLabel.Text;
                string destinationConnectionString = destinationLabel.Text;

                int count = 1;
                List<string> tableNames = GetTableNames(sourceConnectionString);
                tableProgressBar.Minimum = 0;
                tableProgressBar.Maximum = tableNames.Count;
                tableProgressBar.Value = 0;
                foreach (string tableName in tableNames)
                {
                    TransferTableData(sourceConnectionString, destinationConnectionString, tableName, progressBar);
                    tableProgressBar.Value = count++;
                    this.Invoke(new Action(() => messageLabel.Text = $"Data transfer for table - {tableName}."));
                }
                MessageBox.Show("Database Transferd successfully.");
            }
            else
            {
                MessageBox.Show("Choose the Database Source and Destination.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


            transferButton.Enabled = true;
        }

        private void TransferTableData(string sourceConnectionString, string destinationConnectionString, string tableName, ProgressBar progressBar)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Load data from the source table
                using (SqlConnection sourceConnection = new SqlConnection(sourceConnectionString))
                {
                    sourceConnection.Open();

                    // Check if the UniqueIdentifier column exists in the source table
                    bool sourceHasUniqueIdentifier = ColumnExists(sourceConnection, tableName, "UniqueIdentifier");



                    // Load data from the source table into DataTable
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", sourceConnection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from source table: {ex.Message}");
                return;
            }

            // Proceed only if there is data to transfer
            if (dataTable.Rows.Count > 0)
            {
                string stagingTableName = $"{tableName}_Staging";

                try
                {
                    using (SqlConnection destinationConnection = new SqlConnection(destinationConnectionString))
                    {
                        destinationConnection.Open();

                        // Get the primary key column name
                        string primaryKeyColumn = GetPrimaryKeyColumn(destinationConnection, tableName);

                        if (primaryKeyColumn != null)
                        {
                            // Check if the destination table exists
                            bool tableExists = TableExists(destinationConnection, tableName);

                            if (!tableExists)
                            {
                                MessageBox.Show($"Table {tableName} does not exist in the destination database.");
                                return;
                            }

                            // Ensure the destination table has the UniqueIdentifier column
                            bool destinationHasUniqueIdentifier = ColumnExists(destinationConnection, tableName, "UniqueIdentifier");

                            // Create the staging table
                            CreateStagingTable(destinationConnection, tableName, stagingTableName);

                            // Transfer data to the staging table using SqlBulkCopy
                            TransferDataToStagingTable(destinationConnection, dataTable, stagingTableName, progressBar);

                            // Merge data from the staging table to the destination table
                            MergeDataFromStagingToDestination(destinationConnection, tableName, stagingTableName, GetPrimaryKeyColumns(destinationConnection, tableName));

                            // Drop the staging table
                            DropStagingTable(destinationConnection, stagingTableName);

                            messageLabel.Text = $"Data transferred successfully for table {tableName}.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error transferring data to destination table: {ex.Message}");
                }
            }
        }


        private string GetPrimaryKeyColumn(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand getPrimaryKeyCmd = new SqlCommand(
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                    $"WHERE TABLE_NAME = @TableName AND CONSTRAINT_NAME LIKE 'PK_%';", connection))
                {
                    getPrimaryKeyCmd.Parameters.AddWithValue("@TableName", tableName);
                    using (SqlDataReader reader = getPrimaryKeyCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving primary key column: {ex.Message}");
            }
            return null;
        }


        private void MergeDataFromStagingToDestination(SqlConnection connection, string tableName, string stagingTableName, List<string> primaryKeyColumns)
        {
            try
            {

                // Get the column names and identify identity columns
                List<string> columnNames = new List<string>();
                List<string> insertColumnNames = new List<string>();
                string identityColumn = GetIdentityColumn(connection, tableName);

                using (SqlCommand getColumnsCmd = new SqlCommand(
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = 'dbo'", connection))
                {
                    using (SqlDataReader reader = getColumnsCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string columnName = reader.GetString(0);
                            if (columnName != identityColumn)
                            {
                                insertColumnNames.Add(columnName);
                                columnNames.Add(columnName);
                            }
                        }
                    }
                }

                string insertColumnsList = string.Join(", ", insertColumnNames);
                string insertSourceColumnList = string.Join(", ", insertColumnNames.Select(col => $"Source.{col}"));

                // Ensure source rows are unique by using ROW_NUMBER and common table expressions (CTE)
                string cteQuery =
                    $"WITH SourceWithRowNumber AS " +
                    $"( " +
                    $"  SELECT *, ROW_NUMBER() OVER (PARTITION BY {string.Join(", ", primaryKeyColumns)} ORDER BY {string.Join(", ", primaryKeyColumns)}) AS RowNum " +
                    $"  FROM {stagingTableName} " +
                    $") ";

                // Build the ON clause using the primary key columns
                string onClause = string.Join(" AND ", primaryKeyColumns.Select(col => $"Target.{col} = Source.{col}"));

                // Merge data from the staging table to the destination table
                using (SqlCommand mergeCmd = new SqlCommand(
                    $"{cteQuery} " +
                    $"MERGE INTO {tableName} AS Target " +
                    $"USING (SELECT * FROM SourceWithRowNumber WHERE RowNum = 1) AS Source " +
                    $"ON {onClause} " +
                    $"WHEN MATCHED THEN " +
                    $"UPDATE SET {string.Join(", ", columnNames.Select(col => $"Target.{col} = Source.{col}"))} " +
                    $"WHEN NOT MATCHED BY TARGET THEN " +
                    $"INSERT ({insertColumnsList}) " +
                    $"VALUES ({insertSourceColumnList}) " +
                    $"WHEN NOT MATCHED BY SOURCE THEN " +
                    $"DELETE " +
                    $";", connection))
                {
                    int count = mergeCmd.ExecuteNonQuery();
                    progressBar.Value = count;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error merging data from staging table to destination table: {ex.Message}");
            }
        }
        private string GetIdentityColumn(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand getIdentityColumnsCmd = new SqlCommand(
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                    $"WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = 'dbo' AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1", connection))
                {
                    using (SqlDataReader reader = getIdentityColumnsCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error identifying identity column: {ex.Message}");
            }
            return null;
        }
        private bool ColumnExists(SqlConnection connection, string tableName, string columnName)
        {
            try
            {
                using (SqlCommand checkColumnCmd = new SqlCommand(
                    $"SELECT 1 FROM sys.columns WHERE Name = N'{columnName}' AND Object_ID = Object_ID(N'{tableName}')", connection))
                {
                    return checkColumnCmd.ExecuteScalar() != null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking column existence: {ex.Message}");
                return false;
            }
        }
        private bool TableExists(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand checkTableCmd = new SqlCommand(
                    $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0;", connection))
                {
                    return (int)checkTableCmd.ExecuteScalar() == 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking table existence: {ex.Message}");
                return false;
            }
        }

        private void CreateStagingTable(SqlConnection connection, string tableName, string stagingTableName)
        {
            try
            {
                using (SqlCommand createStagingTableCmd = new SqlCommand(
                    $"IF OBJECT_ID('{stagingTableName}', 'U') IS NOT NULL DROP TABLE {stagingTableName}; " +
                    $"SELECT * INTO {stagingTableName} FROM {tableName} WHERE 1 = 0;", connection))
                {
                    createStagingTableCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error creating staging table: {ex.Message}");
            }
        }

        private void TransferDataToStagingTable(SqlConnection connection, DataTable dataTable, string stagingTableName, ProgressBar progressBar)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = stagingTableName;

                    // Set up the progress bar
                    progressBar.Minimum = 0;
                    progressBar.Maximum = dataTable.Rows.Count;
                    progressBar.Value = 0;

                    bulkCopy.SqlRowsCopied += (sender, args) =>
                    {
                        progressBar.Value = (int)args.RowsCopied;
                    };
                    bulkCopy.NotifyAfter = 100;

                    // Explicitly map the columns
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.WriteToServer(dataTable);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error transferring data to staging table: {ex.Message}");
            }
        }


        private void DropStagingTable(SqlConnection connection, string stagingTableName)
        {
            try
            {
                using (SqlCommand dropStagingTableCmd = new SqlCommand(
                    $"DROP TABLE {stagingTableName};", connection))
                {
                    dropStagingTableCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error dropping staging table: {ex.Message}");
            }
        }

        private List<string> GetPrimaryKeyColumns(SqlConnection connection, string tableName)
        {
            List<string> primaryKeyColumns = new List<string>();
            try
            {
                using (SqlCommand getPrimaryKeysCmd = new SqlCommand(
                    $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                    $"WHERE TABLE_NAME = @TableName AND CONSTRAINT_NAME LIKE 'PK_%';", connection))
                {
                    getPrimaryKeysCmd.Parameters.AddWithValue("@TableName", tableName);
                    using (SqlDataReader reader = getPrimaryKeysCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            primaryKeyColumns.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving primary key columns: {ex.Message}");
            }
            return primaryKeyColumns;
        }








        private void CreateConfigFile()
        {
            XElement config = new XElement("Config");
            config.Save(ConfigFilePath);
        }




        private void LoadDatabases(string connectionString, ComboBox comboBox)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable databases = connection.GetSchema("Databases");
                    comboBox.Items.Clear();

                    foreach (DataRow database in databases.Rows)
                    {
                        comboBox.Items.Add(database.Field<string>("database_name"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading databases: " + ex.Message);
            }
        }

        private List<string> GetTableNames(string connectionString)
        {
            List<string> tableNames = new List<string>();

            using (SqlConnection connection = new SqlConnection($"{connectionString}"))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");

                foreach (DataRow row in schema.Rows)
                {
                    string tableName = row[2].ToString();
                    tableNames.Add(tableName);
                }
            }

            return tableNames;
        }

        private void TransferTableData(string sourceConnectionString, string destinationConnectionString, string sourceDatabase, string destinationDatabase, string tableName)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection sourceConnection = new SqlConnection($"{sourceConnectionString};Initial Catalog={sourceDatabase}"))
            {
                sourceConnection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", sourceConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            using (SqlConnection destinationConnection = new SqlConnection($"{destinationConnectionString};Initial Catalog={destinationDatabase}"))
            {
                destinationConnection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName = tableName;

                    try
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error occurred while transferring data for table {tableName}: " + ex.Message);
                    }
                }
            }
        }

        private void sourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            sourceLabel.Text = sourceComboBox.SelectedItem != null ? GetConnectionStringByName(name: sourceComboBox.SelectedItem.ToString(), "Source") : "";
        }

        private void DestinationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            destinationLabel.Text = destinationComboBox.SelectedItem != null ? GetConnectionStringByName(name: destinationComboBox.SelectedItem.ToString(), "Destination") : "";
        }

        private string GetConnectionStringByName(string name, string ConnectionType)
        {
            List<(string, string)> connections = LoadConnectionStrings(ConnectionType);

            foreach ((string connectionName, string connectionString) in connections)
            {
                if (connectionName == name)
                {
                    return TestConnection(connectionString) ? connectionString : "";
                }
            }

            return ""; // Connection string not found
        }

        private bool TestConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
                return false;
            }
        }



        private void BtnRefreshSource_Click(object sender, EventArgs e)
        {
            // Implement the logic to refresh the source connections
            LoadConnections();
        }

        private void DeleteConnectionStringByName(string name)
        {
            if (!File.Exists(ConfigFilePath))
            {
                MessageBox.Show("Configuration file not found.");
                return;
            }

            XElement config = XElement.Load(ConfigFilePath);
            XElement connectionToDelete = config.Elements("Connection").FirstOrDefault(el => el.Element("Name")?.Value == name);

            if (connectionToDelete != null)
            {
                connectionToDelete.Remove();
                config.Save(ConfigFilePath);
                MessageBox.Show($"Connection string '{name}' deleted successfully.");
            }
            else
            {
                MessageBox.Show($"Connection string '{name}' not found.");
            }
        }

        private async void cancelTransferButton_Click(object sender, EventArgs e)
        {

            transferButton.Enabled = false;
            if (sourceLabel.Text != "" && destinationLabel.Text != "")
            {
                string sourceConnectionString = sourceLabel.Text;
                string destinationConnectionString = destinationLabel.Text;


                int count = 1;
                List<string> tableNames = GetTableNames(sourceConnectionString);
                tableProgressBar.Minimum = 0;
                tableProgressBar.Maximum = tableNames.Count;
                tableProgressBar.Value = 0;
                foreach (string tableName in tableNames)
                {
                    await CanecelTransferTableData(sourceConnectionString, destinationConnectionString, tableName, progressBar);
                    tableProgressBar.Value = count++;
                }
            }

            transferButton.Enabled = true;
        }

        private async Task CanecelTransferTableData(string sourceConnectionString, string destinationConnectionString, string tableName, ProgressBar progressBar)
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Load data from the source table
                using (SqlConnection sourceConnection = new SqlConnection(sourceConnectionString))
                {
                    await sourceConnection.OpenAsync();

                    // Load data from the source table into DataTable
                    using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", sourceConnection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from source table: {ex.Message}");
                return;
            }

            // Proceed only if there is data to transfer
            if (dataTable.Rows.Count > 0)
            {
                string stagingTableName = $"{tableName}";

                using (SqlConnection destinationConnection = new SqlConnection(destinationConnectionString))
                {
                    try
                    {
                        await destinationConnection.OpenAsync();

                        // Check if the destination table exists
                        bool tableExists = await TableExistsAsync(destinationConnection, tableName);

                        if (!tableExists)
                        {
                            //Invoke(new Action(() => MessageBox.Show($"Table {tableName} does not exist in the destination database.")));
                            return;
                        }

                        // Delete records in the destination table that exist in the source table
                        await DeleteRecordsInDestinationAsync(destinationConnection, dataTable, tableName);

                        this.Invoke(new Action(() => messageLabel.Text = $"Data transfer canceled successfully for table {tableName}."));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => messageLabel.Text = $"Error transferring data to destination table: {ex.Message}"));
                    }
                }
            }
        }



        private async Task<bool> TableExistsAsync(SqlConnection connection, string tableName)
        {
            try
            {
                using (SqlCommand checkTableCmd = new SqlCommand(
                    $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0;", connection))
                {
                    return (int)await checkTableCmd.ExecuteScalarAsync() == 1;
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => MessageBox.Show($"Error checking table existence: {ex.Message}")));
                return false;
            }
        }

        private async Task DeleteRecordsInDestinationAsync(SqlConnection connection, DataTable sourceData, string tableName)
        {
            try
            {
                int count = 1;

                progressBar.Minimum = 0;
                progressBar.Maximum = sourceData.Rows.Count;
                progressBar.Value = 0;
                foreach (DataRow row in sourceData.Rows)
                {
                    string uniqueIdentifier = row["UniqueIdentifier"].ToString();
                    using (SqlCommand deleteCmd = new SqlCommand(
                        $"DELETE FROM {tableName} WHERE UniqueIdentifier = @UniqueIdentifier", connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@UniqueIdentifier", uniqueIdentifier);
                        await deleteCmd.ExecuteNonQueryAsync();
                        progressBar.Value = count++;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => MessageBox.Show($"Error deleting records from destination table: {ex.Message}")));
            }
        }




        private void btnRefreshDestination_Click(object sender, EventArgs e)
        {
            // Implement the logic to refresh the source connections
            LoadConnections();
        }

        private bool HasPermission(string permission)
        {
            XDocument doc = XDocument.Load("roles.xml");

            var role = doc.Descendants("Role")
                .FirstOrDefault(r => r.Attribute("name").Value == currentUserRole);

            if (role != null)
            {
                var permissions = role.Descendants("Permission")
                    .Select(p => p.Value)
                    .ToList();

                return permissions.Contains(permission);
            }

            return false;
        }

        private void BtnDeleteSource_Click(object sender, EventArgs e)
        {
            if (HasPermission("DeleteSource"))
            {
                // Confirm delete
                DialogResult result = MessageBox.Show("Are you sure you want to delete this source connection?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteConnectionStringByName(sourceComboBox.SelectedItem.ToString());
                    LoadConnections();
                    MessageBox.Show("Source connection string deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("You do not have permission to perform this action.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteDestination_Click(object sender, EventArgs e)
        {
            if (HasPermission("DeleteDestination"))
            {
                // Confirm delete
                DialogResult result = MessageBox.Show("Are you sure you want to delete this destination connection?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteConnectionStringByName(destinationComboBox.SelectedItem.ToString());
                    LoadConnections();
                    MessageBox.Show("Destination connection string deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("You do not have permission to perform this action.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddConnectionStringButton_Click(object sender, EventArgs e)
        {
            if (HasPermission("AddConnection"))
            {
                // Open the form to add a new connection string
                ConnectionStringConfigForm configForm = new ConnectionStringConfigForm();
                configForm.ShowDialog();
                LoadConnections();
            }
            else
            {
                MessageBox.Show("You do not have permission to perform this action.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
