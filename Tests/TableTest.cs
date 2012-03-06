using ET.Obj2Schema;
using ET.Obj2Schema.Maps;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.SampleClasses;
using System;
using System.Diagnostics;

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

        private class SampleWithReadOnlyProperty
        {
            public string Name { get { return "Bob"; } }
        }
    }
}