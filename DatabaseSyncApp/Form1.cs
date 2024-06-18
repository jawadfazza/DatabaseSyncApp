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

        public Form1()
        {
            InitializeComponent();

            List<(string, string)> connections = LoadConnectionStrings();
            sourceComboBox.Items.Add("");
            destinationComboBox.Items.Add("");
            foreach ((string name, string connection) in connections)
            {
                sourceComboBox.Items.Add(name);
                destinationComboBox.Items.Add(name);
            }

            if (connections.Count > 0)
            {
                sourceComboBox.SelectedIndex = 0;
                destinationComboBox.SelectedIndex = 0;
            }
        }


        private void TransferButton_Click(object sender, EventArgs e)
        {
          

            if (sourceLabel.Text != "" && destinationLabel.Text != "")
            {
                string sourceConnectionString = sourceLabel.Text;
                string destinationConnectionString = destinationLabel.Text;
               

                List<string> tableNames = GetTableNames(sourceConnectionString);

                foreach (string tableName in tableNames)
                {
                    TransferTableData(sourceConnectionString, destinationConnectionString,  tableName, progressBar);
                }
            }
        }


        private void TransferTableData(string sourceConnectionString, string destinationConnectionString, string tableName, ProgressBar progressBar)
        {
            DataTable dataTable = new DataTable();

            // Load data from the source table
            using (SqlConnection sourceConnection = new SqlConnection($"{sourceConnectionString}"))
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
            if (dataTable.Rows.Count > 0)
            {


                // Optional: Create staging table to avoid duplication
                string stagingTableName = tableName + "_Staging";

                using (SqlConnection destinationConnection = new SqlConnection(destinationConnectionString))
                {
                    destinationConnection.Open();

                    // Check if the destination table exists
                    bool tableExists = false;
                    using (SqlCommand checkTableCmd = new SqlCommand(
                        $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL SELECT 1 ELSE SELECT 0;", destinationConnection))
                    {
                        tableExists = (int)checkTableCmd.ExecuteScalar() == 1;
                    }

                    if (!tableExists)
                    {
                        //MessageBox.Show($"Table {tableName} does not exist in the destination database.");
                        return;
                    }

                    // Create the staging table
                    using (SqlCommand createStagingTableCmd = new SqlCommand(
                        $"IF OBJECT_ID('{stagingTableName}', 'U') IS NOT NULL DROP TABLE {stagingTableName}; " +
                        $"SELECT * INTO {stagingTableName} FROM {tableName} WHERE 1 = 0;", destinationConnection))
                    {
                        createStagingTableCmd.ExecuteNonQuery();
                    }

                    // Use SqlBulkCopy to transfer data to the staging table
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
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

                        try
                        {
                            bulkCopy.WriteToServer(dataTable);

                            // Get the column names
                            DataTable schemaTable = destinationConnection.GetSchema("Columns", new string[] { null, null, tableName });
                            List<string> columnNames = new List<string>();

                            foreach (DataRow row in schemaTable.Rows)
                            {
                                string columnName = row["COLUMN_NAME"].ToString();
                               
                            }

                            string columnsList = string.Join(", ", columnNames);
                            string sourceColumnList = string.Join(", ", columnNames.Select(col => $"Source.{col}"));
                            string primaryKey = columnNames[0]; // Assuming the first non-identity column is the primary key

                            // Merge data from the staging table to the destination table
                            using (SqlCommand mergeCmd = new SqlCommand(
                                $"MERGE INTO {tableName} AS Target " +
                                $"USING {stagingTableName} AS Source " +
                                $"ON Target.{primaryKey} = Source.{primaryKey} " + // Adjust this condition based on your table's primary key
                                $"WHEN NOT MATCHED BY TARGET THEN " +
                                $"INSERT ({columnsList}) " +
                                $"VALUES ({sourceColumnList});", destinationConnection))
                            {
                                mergeCmd.ExecuteNonQuery();
                            }

                            // Drop the staging table
                            using (SqlCommand dropStagingTableCmd = new SqlCommand(
                                $"DROP TABLE {stagingTableName};", destinationConnection))
                            {
                                dropStagingTableCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show($"Data transferred successfully for table {tableName}.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error occurred while transferring data for table {tableName}: " + ex.Message);
                        }
                    }
                }
            }
        }


        private void AddConnectionStringButton_Click(object sender, EventArgs e)
        {
            using (ConnectionStringConfigForm configForm = new ConnectionStringConfigForm())
            {
                configForm.ShowDialog();
            }
        }



        private List<(string, string)> LoadConnectionStrings()
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

                connections.Add((name, connectionString));
            }

            return connections;
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
            sourceLabel.Text = sourceComboBox.SelectedItem != null ? GetConnectionStringByName(name: sourceComboBox.SelectedItem.ToString()) : "";
        }

        private void DestinationComboBox_SelectedIndexChanged(object sender, EventArgs e) 
        {
            destinationLabel.Text = destinationComboBox.SelectedItem != null ? GetConnectionStringByName(name: destinationComboBox.SelectedItem.ToString()) : "";
        }

        private string GetConnectionStringByName(string name)
        {
            List<(string, string)> connections = LoadConnectionStrings();

            foreach ((string connectionName, string connectionString) in connections)
            {
                if (connectionName == name)
                {
                    return TestConnection(connectionString)?connectionString:"";
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

    }
}
