using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Maps
{
    /// <summary>
    /// This class doesn't add any mappings.
    /// </summary>
    public sealed class EmptyDataTypesMap : DbDataTypesMapBase
    {
        private static readonly EmptyDataTypesMap instance = new EmptyDataTypesMap();
        /// <summary>
        /// Gets the instance of EmptyDataTypesMap.
        /// </summary>
        public static EmptyDataTypesMap Instance { get { return instance; } }


        private EmptyDataTypesMap()
            : base()
        {
            this.Map.Add(DataTypes.Char, string.Empty);
            this.Map.Add(DataTypes.Bool, string.Empty);
            this.Map.Add(DataTypes.Byte, string.Empty);
            this.Map.Add(DataTypes.DateTime, string.Empty);
            this.Map.Add(DataTypes.Decimal, string.Empty);
            this.Map.Add(DataTypes.Double, string.Empty);
            this.Map.Add(DataTypes.Enum, string.Empty);
            this.Map.Add(DataTypes.Float, string.Empty);
            this.Map.Add(DataTypes.Guid, string.Empty);
            this.Map.Add(DataTypes.Int, string.Empty);
            this.Map.Add(DataTypes.Long, string.Empty);
            this.Map.Add(DataTypes.Short, string.Empty);
            this.Map.Add(DataTypes.String, string.Empty);
            this.Map.Add(DataTypes.TimeSpan, string.Empty);
        }
    }
}