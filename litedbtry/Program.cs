using System;
using System.Collections.Generic;
using LiteDB;

namespace litedbtry
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start lite");

            IEnumerable<Customer> data = null;
            var connectionString = new ConnectionString { Filename = @"C:\\prvn\\repo\\TeamApps\\Database\\data", Password = "dataadmin!" };

            using (var db = new LiteDatabase(connectionString))
            {
                var coll = db.GetCollection<Customer>("customer");

                coll.Insert(new Customer { Id = 1, Name = "First User" });
                coll.Insert(new Customer { Id = 2, Name = "Second User" });

                coll.EnsureIndex(i => i.Id);
            }

            using (var db = new LiteDatabase(connectionString))
            {
                var coll = db.GetCollection<Customer>("customer");

                data = coll.FindAll();
            }

            Console.WriteLine(data);
        }
    }

    class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
