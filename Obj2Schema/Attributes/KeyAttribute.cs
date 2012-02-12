using System;

namespace ET.Obj2Schema
{
    /// <summary>
    /// Specifies that this property is a (part of) a key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
    }
}
