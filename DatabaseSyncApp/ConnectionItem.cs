using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSyncApp
{
    public class ConnectionItem
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }

        public ConnectionItem(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public override string ToString()
        {
            return Name;
        }
    }

}
