using ET.Obj2Schema;
using ET.Obj2Schema.Maps;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.SampleClasses;
using System;

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
    }
}