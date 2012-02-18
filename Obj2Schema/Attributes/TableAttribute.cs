using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Attributes
{
    /// <summary>
    /// Defines a custom name for a table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { private set; get; }

        public TableAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name is not specified");
            this.Name = name;
        }
    }
}
