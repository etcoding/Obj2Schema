using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Maps
{
    public sealed class SqliteDataTypesMap : DbDataTypesMapBase
    {
        private static readonly SqliteDataTypesMap instance = new SqliteDataTypesMap();
        /// <summary>
        /// Gets the instance of SqliteDataTypesMap.
        /// </summary>
        public static SqliteDataTypesMap Instance { get { return instance; } }


        private SqliteDataTypesMap()
            : base()
        {
            this.Map.Add(DataTypes.Bool, "INTEGER");
            this.Map.Add(DataTypes.Byte, "INTEGER");
            this.Map.Add(DataTypes.Int, "INTEGER");
            this.Map.Add(DataTypes.String, "TEXT");
            this.Map.Add(DataTypes.Enum, "TEXT");
            this.Map.Add(DataTypes.Char, "TEXT");
            this.Map.Add(DataTypes.DateTime, "TEXT");
            this.Map.Add(DataTypes.Decimal, "REAL");
            this.Map.Add(DataTypes.Double, "REAL");
            this.Map.Add(DataTypes.Float, "REAL");
            this.Map.Add(DataTypes.Guid, "TEXT");
            this.Map.Add(DataTypes.Long, "INTEGER");
            this.Map.Add(DataTypes.Short, "INTEGER");

            this.Expressions.Add(SqlExpressions.AutoIncrement, "AUTOINCREMENT");
            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnTable, "PRIMARY KEY");
            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnColumn, "PRIMARY KEY");
        }
    }
}