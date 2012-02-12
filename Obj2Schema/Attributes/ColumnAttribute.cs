using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema
{
    /// <summary>
    /// Defines a custom column name for a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { private set; get; }
        public ColumnAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name must be set");
            this.Name = name;
        }
    }
}
