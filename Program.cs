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

            var filtered = qry
                .Where(i => i.Age > 18 && i.Age < 30);


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
