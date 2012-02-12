using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.SampleClasses;
using ET.Obj2Schema;
using ET.Obj2Schema.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass()]
    public class SampleUsage
    {
        [TestMethod()]
        public void Usage()
        {
            Table<SimpleUser> tableSimple = new Table<SimpleUser>(MySqlDataTypesMap.Instance);
            string sql = tableSimple.GetSql();
            // CREATE TABLE SimpleUser (Id INT PRIMARY KEY AUTO_INCREMENT NOT NULL, Name VARCHAR(250) NULL, Age INT NULL, Gender VARCHAR(20) NULL)

            Table<User2> tableUser2 = new Table<User2>(MySqlDataTypesMap.Instance);
            sql = tableUser2.ToString();
            // CREATE TABLE Users (FirstName VARCHAR(20) NOT NULL, LastName VARCHAR(30) NOT NULL, MiddleInitial CHAR NOT NULL, 
            // Gender INT NOT NULL, Age INT NOT NULL, BirthDate DATETIME NOT NULL, Income DOUBLE(10,2) NULL, CONSTRAINT pk_Users PRIMARY KEY (FirstName, LastName))
        }

        class SimpleUser
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public int Age { set; get; }
            public Genders Gender { set; get; }
        }
    }
}
