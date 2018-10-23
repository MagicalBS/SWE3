using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Linq
{
    class Program
    {
        private static string dbName = "SWE3.sqlite";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Linq!");

            CreateDatabaseIfNotExists();

            #region OR Mapper Insert
            var orMapper = new ORMapper("SWE3.sqlite");

            orMapper.Insert(new MyTable
            {
                FirstName = "Peppi",
                LastName = "Huber",
                Age = 55
            });

            #endregion

            var qry = new DemoLinq<MyTable>();

            var filtered = qry.Where(herberto => herberto.Age > 50);

            var lst = filtered.ToList();

            string temp = qry.GetSqlStatement();
            Console.WriteLine(temp);

            foreach (var i in lst)
            {
                Console.WriteLine(i);
            }

            Console.ReadKey();
        }

        private static void CreateDatabaseIfNotExists(bool createTestData = true)
        {
            if (File.Exists(dbName))
            {
                return;
            }

            SQLiteConnection.CreateFile(dbName);

            var db = new SQLiteConnection($"Data Source={dbName};Version=3;");
            db.Open();

            string sql = "CREATE TABLE Person (id INT PRIMARY KEY, FirstName VARCHAR(20), LastName VARCHAR(20), Age INT);";
            var cmd = new SQLiteCommand(sql, db);
            var rd = cmd.ExecuteScalar();

            if (createTestData)
            {
                sql = "INSERT INTO Person(FirstName, LastName, Age) VALUES('Peter', 'Griffin', 23), ('Marie', 'Rosenbauer', 27)";
                cmd = new SQLiteCommand(sql, db);
                rd = cmd.ExecuteScalar();
            }
            db.Close();
        }
    }
}
