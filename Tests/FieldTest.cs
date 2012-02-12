using System;
using System.Reflection;
using ET.Obj2Schema;
using ET.Obj2Schema.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass()]
    public class FieldTest
    {
        public int Property { set; get; }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldConstructor_ShouldThrowException_IfParameterProperty_IsNull()
        {
            PropertyInfo property = null; 
            DbDataTypesMapBase typeMap = MySqlDataTypesMap.Instance;
            Field target = new Field(property, typeMap);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FieldConstructor_ShouldThrowException_IfParameterTypeMap_IsNull()
        {
            PropertyInfo property = this.GetType().GetProperties()[0];
            DbDataTypesMapBase typeMap = null;
            Field target = new Field(property, typeMap);
        }

    }
}
