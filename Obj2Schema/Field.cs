using System;
using System.Reflection;
using System.Text.RegularExpressions;
using ET.Obj2Schema.Maps;

namespace ET.Obj2Schema
{
    /// <summary>
    /// Contains a field data.
    /// </summary>
    public class Field
    {
        #region Properties
        private DbDataTypesMapBase typeMap = null;

        /// <summary>
        /// Gets or sets the name of the object property this field is derived from.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { private set; get; }

        /// <summary>
        /// Gets the type of the property this field is reflecting on.
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public Type PropertyType { private set; get; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// Can be different from the property name, if property has ColumnAttribute applied.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        public bool? IsRequired { set; get; }

        private bool isPrimaryKey;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is a (part of) primary key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey
        {
            set
            {
                this.isPrimaryKey = value;
                this.IsRequired = value;
            }
            get { return this.isPrimaryKey; }
        }

        /// <summary>
        /// Gets or sets the SQL data type.
        /// </summary>
        /// <value>
        /// The SQL data type.
        /// </value>
        public string SqlType { set; get; }

        /// <summary>
        /// Gets or sets the SQL field attributes, like AutoIncrement for primary keys
        /// </summary>
        /// <value>
        /// The SQL field attributes.
        /// </value>
        public string SqlFieldAttribute { set; get; }
        #endregion

        private static readonly Field empty = new Field() { PropertyName = string.Empty };

        /// <summary>
        /// Gets the empty field instance.
        /// </summary>
        public static Field Empty { get { return empty; } }

        private Field() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="typeMap">The type map.</param>
        public Field(PropertyInfo property, DbDataTypesMapBase typeMap)
        {
            if (property == null)
                throw new ArgumentNullException("property");
            if (typeMap == null)
                throw new ArgumentNullException("typeMap");

            this.typeMap = typeMap;

            PopulateInstance(property);
        }

        /// <summary>
        /// Sets the values of this field, based on passed in property and type map.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="typeMap">The type map.</param>
        private void PopulateInstance(PropertyInfo property)
        {
            Type ptype = property.PropertyType;

            this.PropertyName = property.Name;
            this.ColumnName = property.Name;
            this.PropertyType = property.PropertyType;

            // set sql type, if this property is not an enum. Enums can be stored as int or string, so they make a special case.
            if (!ptype.IsEnum)
                this.SqlType = typeMap[ptype];
            else
                this.SqlType = typeMap[typeof(Enum)];

            #region See if any of the data annotations are applied
            object[] attributes = property.GetCustomAttributes(true);
            foreach (object attribute in attributes)
            {
                Type attributeType = attribute.GetType();

                if (attributeType == typeof(ColumnAttribute))
                {
                    this.ColumnName = ((ColumnAttribute)attribute).Name;
                }
                if (attributeType == typeof(RequiredAttribute))
                {
                    this.IsRequired = ((RequiredAttribute)attribute).IsRequired;
                }
                if (attributeType == typeof(KeyAttribute))
                {
                    this.IsPrimaryKey = true;
                }
                if (attributeType == typeof(EnumDbDataTypeAttribute))
                {
                    if (((EnumDbDataTypeAttribute)attribute).StoreAs == EnumDbDataTypes.Int)
                        this.SqlType = typeMap[typeof(int)];
                    else
                        this.SqlType = typeMap[typeof(string)];
                }
                if (attributeType == typeof(LengthAttribute))
                {
                    string length = ((LengthAttribute)attribute).Length > 0 ? ((LengthAttribute)attribute).Length.ToString() : ((LengthAttribute)attribute).CustomLength;
                    this.SqlType = Regex.Replace(this.SqlType, "\\([0-9]+[,0-9]*\\)", "(" + length + ")");
                }
            }
            #endregion
        }

        /// <summary>
        /// Determines whether the specified property is ignored via ignore attribute.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///   <c>true</c> if the specified property is ignored; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIgnored(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0;
        }

        /// <summary>
        /// Gets the SQL, representing this field.
        /// </summary>
        /// <returns></returns>
        public string GetSql()
        {
            // regenerating this on every call in case user made changes to the field properties - so this will reflect the changes
            string sql = this.ColumnName + " " + this.SqlType +
                (string.IsNullOrEmpty(this.SqlFieldAttribute) ? string.Empty : " " + this.SqlFieldAttribute);
            if (!this.IsPrimaryKey)
            {
                if (this.IsRequired == null || this.IsRequired.Value == false)
                    sql += " " + DbDataTypesMapBase.SqlStrings.Null;
                else
                    sql += " " + DbDataTypesMapBase.SqlStrings.NotNull;
            }
            return sql;
        }

        /// <summary>
        /// Returns a SQL string of this field.
        /// </summary>
        public override string ToString()
        {
            return this.GetSql();
        }
    }
}