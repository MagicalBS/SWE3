using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Linq
{
    class ORMapper
    {
        private string _dbName;

        public ORMapper(string dbname)
        {
            _dbName = dbname;
        }

        public void Insert(object obj)
        {
            string tableName = obj.GetType().GetCustomAttributes(true).OfType<Table>().FirstOrDefault().Name;
            Type tableType = Type.GetType(obj.GetType().FullName);
            var tableProps = tableType.GetProperties();
            List<string> columnNames = new List<string>();
            List<string> columnValues = new List<string>();

            foreach (var prop in tableProps)
            {
                columnNames.Add(prop.GetCustomAttributes(true).OfType<Column>().FirstOrDefault().Name);

                if (prop.GetValue(obj).GetType() == typeof(string))
                {
                    columnValues.Add($"'{prop.GetValue(obj).ToString()}'");
                }
                else
                {
                    columnValues.Add(prop.GetValue(obj).ToString());
                }
            }

            string statement = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(",", columnValues)});";

            var db = new SQLiteConnection($"Data Source={_dbName};Version=3;");
            db.Open();

            try
            {
                var cmd = new SQLiteCommand(statement, db);
                var rd = cmd.ExecuteScalar();
            }
            finally
            {
                db.Close();
            }
        }

        public void Select(object obj)
        {

        }

        public void Update(object obj)
        {

        }

        public void Delete()
        {

        }
    }
}
