namespace ET.Obj2Schema.Maps
{
    public sealed class MySqlDataTypesMap : DbDataTypesMapBase
    {
        private static readonly MySqlDataTypesMap instance = new MySqlDataTypesMap();
        /// <summary>
        /// Gets the instance of MySqlDataTypesMap.
        /// </summary>
        public static MySqlDataTypesMap Instance { get { return instance; } }

        private MySqlDataTypesMap()
            : base()
        {
            this.Map.Add(DataTypes.Char, "CHAR");
            this.Map.Add(DataTypes.Bool, "BIT");
            this.Map.Add(DataTypes.Byte, "SMALLINT");
            this.Map.Add(DataTypes.DateTime, "DATETIME");
            this.Map.Add(DataTypes.Decimal, "DECIMAL");
            this.Map.Add(DataTypes.Double, "DOUBLE(10,3)");
            this.Map.Add(DataTypes.Enum, "VARCHAR(20)");
            this.Map.Add(DataTypes.Float, "FLOAT");
            this.Map.Add(DataTypes.Guid, "CHAR(36)");
            this.Map.Add(DataTypes.Int, "INT");
            this.Map.Add(DataTypes.Long, "BIGINT");
            this.Map.Add(DataTypes.Short, "MEDIUMINT");
            this.Map.Add(DataTypes.String, "VARCHAR(250)");

            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnTable, "CONSTRAINT pk_" + DbDataTypesMapBase.ReplacementStrings.TableName + " PRIMARY KEY");
            this.Expressions.Add(SqlExpressions.PrimaryKeyDeclaredOnColumn, "PRIMARY KEY");
            this.Expressions.Add(SqlExpressions.AutoIncrement, "AUTO_INCREMENT");
        }
    }
}