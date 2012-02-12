using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ET.Obj2Schema.Maps
{
    public sealed class SqlServerDataTypesMap : DbDataTypesMapBase
    {
        private static readonly SqlServerDataTypesMap instance = new SqlServerDataTypesMap();
        /// <summary>
        /// Gets the instance of SqlServerDataTypesMap.
        /// </summary>
        public static SqlServerDataTypesMap Instance { get { return instance; } }

        public SqlServerDataTypesMap()
            : base()
        {
            // http://msdn.microsoft.com/en-us/library/bb386947.aspx

            this.Map.Add(DataTypes.Char, "CHAR");
            this.Map.Add(DataTypes.Bool, "BIT");
            this.Map.Add(DataTypes.Byte, "SMALLINT");
            this.Map.Add(DataTypes.DateTime, "DATETIME");
            this.Map.Add(DataTypes.Decimal, "DECIMAL");
            this.Map.Add(DataTypes.Double, "FLOAT");
            this.Map.Add(DataTypes.Enum, "VARCHAR(20)");
            this.Map.Add(DataTypes.Float, "FLOAT");
            this.Map.Add(DataTypes.Guid, "UNIQUEIDENTIFIER");
            this.Map.Add(DataTypes.Int, "INT");
            this.Map.Add(DataTypes.Long, "BIGINT");
            this.Map.Add(DataTypes.Short, "MEDIUMINT");
            this.Map.Add(DataTypes.String, "VARCHAR(250)");

            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnTable, "CONSTRAINT pk_" + DbDataTypesMapBase.ReplacementStrings.TableName + " PRIMARY KEY");
            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnColumn, "PRIMARY KEY");
            this.Expressions.Add(SqlExpressions.AutoIncrement, "IDENTITY");
        }
    }
}