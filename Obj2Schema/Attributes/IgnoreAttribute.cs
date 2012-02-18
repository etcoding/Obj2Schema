using System;

namespace ET.Obj2Schema.Attributes
{
    /// <summary>
    /// A property with this attribute applied will be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
