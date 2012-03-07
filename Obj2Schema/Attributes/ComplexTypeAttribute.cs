using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Obj2Schema.Attributes
{
    /// <summary>
    /// Properties marked with this attribute will be treated as complex types.
    /// This attribute can be applied either at a class or a property level.
    /// E.g., if User class contains Address property (which includes street/city/country properties), which is marked with this attribute - 
    /// address properties will be added to a User table.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ComplexTypeAttribute : Attribute
    {
        string prependedString = null;
        /// <summary>
        /// Gets the prepended string, which will be added before the columns of complex type.
        /// </summary>
        /// <value>The name of the prepended.</value>
        public string PrependedString { get { return this.prependedString; } }
        public ComplexTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexTypeAttribute"/> class.
        /// </summary>
        /// <param name="prependedString">If not empty, this string will be added before the columns names of the complex type.</param>
        public ComplexTypeAttribute(string prependedString)
        {
            if (!string.IsNullOrEmpty(prependedString) && !Regex.IsMatch(prependedString, "^[a-zA-Z][a-zA-Z0-9_]*$"))
                throw new ArgumentException("Provided name cannot be used as a column name. Column name must start with a letter, and can contain letters, numbers and underscores.");

            this.prependedString = prependedString;
        }
    }
}
