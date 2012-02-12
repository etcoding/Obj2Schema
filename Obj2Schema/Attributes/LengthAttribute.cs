using System;

namespace ET.Obj2Schema
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : Attribute
    {
        public int Length { get; private set; }

        public string CustomLength { get; private set; }

        public LengthAttribute(int length)
        {
            if (length < 0)
                throw new ArgumentException("Length cannot be less than 0");
            this.Length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LengthAttribute"/> class.
        /// This is for cases when length is not a single digit (e.g. double in MySql can have length of (10,2)).
        /// </summary>
        /// <param name="customLen">The custom length declaration.</param>
        public LengthAttribute(string customLen)
        {
            if (string.IsNullOrWhiteSpace(customLen))
                throw new ArgumentException("Length must be set");
            this.CustomLength = customLen.Replace("(", string.Empty).Replace(")", string.Empty);
        }
    }
}