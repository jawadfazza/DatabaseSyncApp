using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DatabaseSyncApp
{
    public partial class ConnectionStringConfigForm : Form
    {
        public ConnectionStringConfigForm()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string serverName = serverTextBox.Text;
            string databaseName = databaseTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            string connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};";

            // Save the connection string to the XML file
            SaveConnectionString(name, connectionString);

            MessageBox.Show("Connection string saved successfully.");
            this.Close();
        }

        private void SaveConnectionString(string name, string connectionString)
        {
            // Load existing XML or create a new one if it doesn't exist
            XDocument doc;
            if (File.Exists("connectionStrings.xml"))
            {
                doc = XDocument.Load("connectionStrings.xml");
            }
            else
            {
                doc = new XDocument(new XElement("Connections"));
            }

            // Add the new connection string to the XML
            doc.Root.Add(new XElement("Connection",
                                new XElement("Name", name),
                                new XElement("ConnectionString", connectionString)));

            // Save the XML file
            doc.Save("connectionStrings.xml");
        }

        private bool TestConnection(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show($"Connected Successfully");
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
                return false;
            }
        }




        private void testButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string serverName = serverTextBox.Text;
            string databaseName = databaseTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            string connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};";

            TestConnection(connectionString);
        }
    }
}
