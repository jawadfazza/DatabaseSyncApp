using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DatabaseSyncApp
{
    public partial class LoginForm : Form
    {
        public string CurrentUserRole { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center the main form
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (AuthenticateUser(username, password))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
           // string hashedPassword = HashPassword(password);
            XDocument doc = XDocument.Load("users.xml");

            var user = doc.Descendants("User")
                .FirstOrDefault(u => u.Element("Username").Value == username && u.Element("Password").Value == password);

            if (user != null)
            {
                CurrentUserRole = user.Element("Role").Value;
                return true;
            }

            return false;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
