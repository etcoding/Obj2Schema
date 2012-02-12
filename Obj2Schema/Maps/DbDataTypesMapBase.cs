using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Maps
{

    /// <summary>
    /// Maps .NET types to database types
    /// </summary>
    public abstract class DbDataTypesMapBase
    {
        /// <summary>
        /// Maps .NET types to SQL statements.
        /// </summary>
        private Dictionary<DataTypes, string> map = new Dictionary<DataTypes, string>();

        /// <summary>
        /// Contains the mapping from .net types to database types.
        /// </summary>
        public virtual Dictionary<DataTypes, string> Map { get { return this.map; } }

        private Dictionary<SqlExpressions, string> expressions = new Dictionary<SqlExpressions, string>();

        /// <summary>
        /// Allows to set SQL for expressions defined in SqlExpressions enum (e.g. primary key).
        /// </summary>
        public virtual Dictionary<SqlExpressions, string> Expressions { get { return this.expressions; } }

        /// <summary>
        /// This will map values between DataTypes enum and actual .NET datatypes.
        /// </summary>
        private Dictionary<Type, DataTypes> netToDataTypesMap = new Dictionary<Type, DataTypes>() 
        { 
            #region Definition
            {typeof(bool), DataTypes.Bool },
            {typeof(byte), DataTypes.Byte},
            {typeof(char), DataTypes.Char},
            {typeof(DateTime), DataTypes.DateTime},
            {typeof(decimal), DataTypes.Decimal},
            {typeof(double), DataTypes.Double},
            {typeof(Enum), DataTypes.Enum},
            {typeof(float), DataTypes.Float},
            {typeof(Guid), DataTypes.Guid},
            {typeof(int), DataTypes.Int},
            {typeof(long), DataTypes.Long},
            {typeof(short), DataTypes.Short},
            {typeof(string), DataTypes.String}
            #endregion
        };

        /// <summary>
        /// Gets the <see cref="System.String"/> for given type.
        /// </summary>
        public string this[Type type]
        {
            get
            {
                if (!netToDataTypesMap.ContainsKey(type))
                    return string.Empty;
                if ((!this.map.ContainsKey(netToDataTypesMap[type])) ||
                    (this.map.ContainsKey(netToDataTypesMap[type]) && string.IsNullOrEmpty(this.map[netToDataTypesMap[type]])))
                    throw new MappingNotDefinedException(type);
                return this.map[netToDataTypesMap[type]];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataTypesMapBase"/> class.
        /// </summary>
        public DbDataTypesMapBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbDataTypesMapBase"/> class.
        /// </summary>
        /// <param name="typeMap">The type map.</param>
        public DbDataTypesMapBase(Dictionary<DataTypes, string> typeMap)
        {
            if (typeMap == null)
                throw new ArgumentException("Type map is null");

            this.map = typeMap;
        }

        /// <summary>
        /// Determines whether this type should be valid (that is it's one of the types defined in DataTypes enum).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is valid type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidType(Type type)
        {
            return this.netToDataTypesMap.ContainsKey(type) || type.IsEnum;
        }

        /// <summary>
        /// Determines whether the mapping for a given type was specified.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [has mapping for] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMappingFor(Type type)
        {
            if (type.IsEnum)
                return this.netToDataTypesMap.ContainsKey(typeof(Enum)) && map.ContainsKey(netToDataTypesMap[typeof(Enum)]) && !string.IsNullOrEmpty(map[netToDataTypesMap[typeof(Enum)]]);

            return this.netToDataTypesMap.ContainsKey(type) && map.ContainsKey(netToDataTypesMap[type]) && !string.IsNullOrEmpty(map[netToDataTypesMap[type]]);
        }

        public static class SqlStrings
        {
            public const string NotNull = "NOT NULL";
            public const string Null = "NULL";
        }

        /// <summary>
        /// Those strings can be used in data type definitions and expressions.
        /// They will be replaced with actual values when sql statement is compiled.
        /// </summary>
        public static class ReplacementStrings
        {
            /// <summary>
            /// This will be replaced with actual table name.
            /// </summary>
            public const string TableName  = "{TableName}";
        }
    }
}