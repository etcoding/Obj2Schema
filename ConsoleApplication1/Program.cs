using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ET.Obj2Schema.Table<User> user = new ET.Obj2Schema.Table<User>(ET.Obj2Schema.Maps.SqliteDataTypesMap.Instance);
            string s = user.GetSql();
            Console.WriteLine(s);
        }


        class User
        {
            public int ID { set; get; }
            public string Name { set; get; }
        }
    }
}
