using System;
using System.Linq;

namespace Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Linq!");

            var qry = new DemoLinq<MyTable>();

            var filtered = qry.Where(herberto => herberto.Age > 18 && herberto.Age < 35 || herberto.FirstName == "Peter")
                .Where(herberto => herberto.Age > 15 && herberto.Age < 25 || herberto.FirstName == "Hans");

            var lst = filtered.ToList();

            string temp = qry.GetSqlStatement();
            Console.WriteLine(temp);

            foreach (var i in lst)
            {
                Console.WriteLine(i);
            }

            Console.ReadKey();
        }
    }
}
