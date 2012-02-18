using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property)]
    public class EnumDbDataTypeAttribute : Attribute
    {
        public EnumDbDataTypes StoreAs { get; set; }
        public EnumDbDataTypeAttribute(EnumDbDataTypes storeAs)
        {
            this.StoreAs = storeAs;
        }
    }
}
