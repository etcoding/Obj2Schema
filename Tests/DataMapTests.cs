using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ET.Obj2Schema;
using Tests.SampleClasses;
using FluentAssertions;
using ET.Obj2Schema.Maps;
using System.Diagnostics;
using System.Reflection;

namespace Tests
{
    [TestClass()]
    public class DataMapTests
    {
        [TestMethod()]
        public void Sql_ShouldContain_PrimaryKey()
        {
            Table<PK> t = new Table<PK>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance.Expressions[SqlExpressions.PrimaryKeyDeclaredOnColumn]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_AutoincrementParameter()
        {
            Table<PK> t = new Table<PK>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance.Expressions[SqlExpressions.AutoIncrement]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_TableName()
        {
            Table<PK> t = new Table<PK>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("PK");
        }

        [TestMethod()]
        public void Sql_ShouldContain_Null()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.SqlStrings.Null);
        }

        #region Type translation tests
        [TestMethod()]
        public void Sql_ShouldContain_INT()
        {
            Table<PK> t = new Table<PK>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("INT");
        }

        [TestMethod()]
        public void Sql_ShouldContain_Varchar()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(string)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Char()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(char)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_DateTime()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(DateTime)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Bool()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(bool)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Double()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(double)]);
        }


        [TestMethod()]
        public void Sql_ShouldContain_Float()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(float)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Byte()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(byte)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Long()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(long)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Short()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(short)]);
        }

        [TestMethod()]
        public void Sql_ShouldContain_Guid()
        {
            Table<User> t = new Table<User>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance[typeof(Guid)]);
        }
        #endregion

        #region Attributes tests
        [TestMethod()]
        public void Sql_ShouldContain_ColumnName_SpecifiedByAttribute()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("FirstName");
        }

        [TestMethod()]
        public void Sql_ShouldContain_DoubleWithLength_SpecifiedByAttribute()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("(10,2)");
        }

        [TestMethod()]
        public void Sql_ShouldContain_StringWithLength_SpecifiedByAttribute()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("VARCHAR(30)");
        }

        [TestMethod()]
        public void Sql_ShouldNotContain_IgnoredProperty()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().NotContain("IsEmployed");
        }

        [TestMethod()]
        public void Sql_ShouldContain_CompositePrimaryKey()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain(MySqlDataTypesMap.Instance.Expressions[SqlExpressions.PrimaryKeyDeclaredOnTable].Replace(DbDataTypesMapBase.ReplacementStrings.TableName, t.TableName));
        }

        [TestMethod()]
        public void Sql_ShouldContain_TableName_SpecifiedByAttribute()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Contain("TABLE Users");
        }

        #endregion

        #region Check SQL in whole 
        //those are really the only tests needed

        [TestMethod()]
        public void MySqlMap_ShouldGenerate_ValidSql_PK()
        {
            Table<PK> t = new Table<PK>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE PK (ID INT PRIMARY KEY AUTO_INCREMENT)");
        }

        [TestMethod()]
        public void MySqlMap_ShouldGenerate_ValidSql_User2()
        {
            Table<User2> t = new Table<User2>(MySqlDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE Users (FirstName VARCHAR(20), LastName VARCHAR(30), Gender INT NULL, Income DOUBLE(10,2) NOT NULL, CONSTRAINT pk_Users PRIMARY KEY (FirstName, LastName))");
        }

        [TestMethod()]
        public void SqlServerMap_ShouldGenerate_ValidSql_PK()
        {
            Table<PK> t = new Table<PK>(SqlServerDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE PK (ID INT PRIMARY KEY IDENTITY)");
        }

        [TestMethod()]
        public void SqlServerMap_ShouldGenerate_ValidSql_User2()
        {
            Table<User2> t = new Table<User2>(SqlServerDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE Users (FirstName VARCHAR(20), LastName VARCHAR(30), Gender INT NULL, Income FLOAT NOT NULL, CONSTRAINT pk_Users PRIMARY KEY (FirstName, LastName))");
        }

        [TestMethod()]
        public void SqliteMap_ShouldGenerate_ValidSql_PK()
        {
            Table<PK> t = new Table<PK>(SqliteDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE PK (ID INTEGER PRIMARY KEY AUTOINCREMENT)");
        }

        [TestMethod()]
        public void SqliteMap_ShouldGenerate_ValidSql_User2()
        {
            Table<User2> t = new Table<User2>(SqliteDataTypesMap.Instance);
            t.GetSql().Should().Be("CREATE TABLE Users (FirstName TEXT, LastName TEXT, Gender INTEGER NULL, Income REAL NOT NULL, PRIMARY KEY (FirstName, LastName))");
        }
        #endregion
    }
}
