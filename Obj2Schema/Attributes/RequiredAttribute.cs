using System;

namespace ET.Obj2Schema
{
    /// <summary>
    /// If applied to a class, will make all properties required (or not).
    /// If applied to a property, will make that property required (or not), and will override the attribute, applied to the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class RequiredAttribute : Attribute
    {
        public bool IsRequired { set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredAttribute"/> class, and sets a marked property to required.
        /// </summary>
        public RequiredAttribute()
        {
            this.IsRequired = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredAttribute"/> class, and sets a marked property to the provided value.
        /// </summary>
        public RequiredAttribute(bool required)
        {
            this.IsRequired = required;
        }
    }
}
