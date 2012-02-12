using System;

namespace ET.Obj2Schema.Maps
{
    public enum SqlExpressions
    {

        /// <summary>
        /// Primary keys defined on tables (e.g. "PRIMARY KEY" in Sqlite "CREATE TABLE t(x INTEGER, y, z, PRIMARY KEY(x DESC));")
        /// </summary>
        PrimaryKeyDeclaredOnTable,
        /// <summary>
        /// Primary key defined on column declaration (e.g. "PRIMARY KEY" for Sqlite "CREATE TABLE t(x INTEGER PRIMARY KEY ASC, y, z);")
        /// </summary>
        PrimaryKeyDeclaredOnColumn,
        /// <summary>
        /// Autoincrement keyword (e.g. AUTO_INCREMENT for MySql, or IDENTITY for Sql Server)
        /// </summary>
        AutoIncrement
    }
}
