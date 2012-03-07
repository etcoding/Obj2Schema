using ET.Obj2Schema;
using ET.Obj2Schema.Maps;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.SampleClasses;
using System;
using System.Diagnostics;
using Obj2Schema.Attributes;
using System.Linq;

namespace Tests
{
    /// <summary>
    ///This is a test class for TableTest and is intended
    ///to contain all TableTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TableTest
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TableConstructor_ShouldThrowException_IfParameterIsNull()
        {
            Table<Simple> t = new Table<Simple>(null);
        }

        [TestMethod]
        public void Table_ShouldContain_ReadonlyProperty()
        {
            Table<SampleWithReadOnlyProperty> table = new Table<SampleWithReadOnlyProperty>(SqliteDataTypesMap.Instance);
            table.Fields.Count.Should().Be(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void When_PrependedString_OfComplexType_ContainsInvalidCharacters_ItShouldFail()
        {
            Table<User4> t = new Table<User4>(SqliteDataTypesMap.Instance);
        }

        [TestMethod]
        public void ComplexTypesColumns_ShouldStartWithPrependedString_WhenSpecified()
        {
            Table<User3> table = new Table<User3>(SqliteDataTypesMap.Instance);
            table.Fields.Where(x => x.ColumnName.StartsWith("Address_")).Count().Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void ComplexTypesColumns_ShouldStartWithComplexTypeName_WhenPrependedString_IsNotSpecified()
        {
            Table<User1> table = new Table<User1>(SqliteDataTypesMap.Instance);
            table.Fields.Where(x => x.ColumnName.StartsWith("Address1")).Count().Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void ComplexTypesColumns_ShouldNotStartWithComplexTypeName_WhenPrependedString_IsEmpty()
        {
            Table<User2> table = new Table<User2>(SqliteDataTypesMap.Instance);
            table.Fields.Where(x => x.ColumnName.StartsWith("Address2")).Count().Should().Be(0);
            table.Fields.Any(x => x.ColumnName == "City").Should().BeTrue();
        }


        [TestMethod]
        public void Table_ShouldInclude_ComplexTypeFields()
        {
            Table<User1> table = new Table<User1>(SqliteDataTypesMap.Instance);
            table.Fields.Any(x => x.ColumnName.Contains("StreetName")).Should().BeTrue();
            table.Fields.Any(x => x.ColumnName.Contains("City")).Should().BeTrue();
            table.Fields.Count.Should().Be(4);
        }

        [TestMethod]
        public void Table_ShouldInclude_ComplexTypeFields_WhenAttributeAppliedToProperty()
        {
            Table<User5> table = new Table<User5>(SqliteDataTypesMap.Instance);
            table.Fields.Any(x => x.ColumnName.Contains("StreetName")).Should().BeTrue();
            table.Fields.Any(x => x.ColumnName.Contains("City")).Should().BeTrue();
            table.Fields.Count.Should().Be(4);
        }



        #region Helper classes
        [ComplexType]
        private class Address1
        {
            public string StreetName { get; set; }
            public string City { get; set; }
        }
        private class User1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address1 Address { get; set; }
        }


        [ComplexType("")]
        private class Address2
        {
            public string StreetName { get; set; }
            public string City { get; set; }
        }
        private class User2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address2 Address { get; set; }
        }

        [ComplexType("Address_")]
        private class Address3
        {
            public string StreetName { get; set; }
            public string City { get; set; }
        }
        private class User3
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address3 Address { get; set; }
        }

        [ComplexType("!address!")]
        private class Address4
        {
            public string StreetName { get; set; }
            public string City { get; set; }
        }
        private class User4
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address4 Address { get; set; }
        }

        private class User5
        {
            public int Id { get; set; }
            public string Name { get; set; }
            [ComplexType]
            public Address5 Address { get; set; }
        }
        private class Address5
        {
            public string StreetName { get; set; }
            public string City { get; set; }
        }

        private class SampleWithReadOnlyProperty
        {
            public string Name { get { return "Bob"; } }
        }
        #endregion
    }
}