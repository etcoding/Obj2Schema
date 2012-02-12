using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ET.Obj2Schema.Maps;
using System.Diagnostics;

namespace ET.Obj2Schema
{
    /// <summary>
    /// Represents a table.
    /// Call ToString() method to retrieve a Sql statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Table<T>
    {
        protected Type targetType;
        protected DbDataTypesMapBase typeMap;

        private const string createTable = "CREATE TABLE";

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { set; get; }

        private List<Field> fields = new List<Field>();
        /// <summary>
        /// Gets the fields of this table.
        /// </summary>
        public List<Field> Fields { get { return this.fields; } }


        public Table(DbDataTypesMapBase map)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            this.targetType = typeof(T);
            this.typeMap = map;


            this.CreateSchemaForType();
        }

        private void CreateSchemaForType()
        {
            #region Set table name
            object[] tableName = this.targetType.GetCustomAttributes(typeof(TableAttribute), true);
            if (tableName == null || tableName.Length == 0)
                this.TableName = this.targetType.Name;
            else
                this.TableName = ((TableAttribute)tableName[0]).Name;
            #endregion

            // get public properties, and create fields
            PropertyInfo[] properties = this.targetType.GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();
            foreach (PropertyInfo property in properties)
            {
                Type ptype = property.PropertyType;

                if (!this.typeMap.IsValidType(ptype))
                    continue;

                if (property.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                    continue;

                if (!this.typeMap.HasMappingFor(ptype))
                    throw new MappingNotDefinedException(ptype);

                Field field = new Field(property, this.typeMap);
                this.Fields.Add(field);
            }

            #region By convention stuff
            // If property name is ID, or typeId, and there are no primary keys already set - make it primary key
            if (this.Fields.Count(x => x.IsPrimaryKey) == 0)
            {
                List<Field> keyField = this.Fields.Where(x => x.ColumnName.ToLower() == "id" || x.ColumnName.ToLower() == this.TableName.ToLower() + "id").ToList();
                // if we find more than one matching fields - that's weird.. let user mark primary key explicitly
                if (keyField.Count == 1)
                {
                    keyField[0].IsPrimaryKey = true;
                    keyField[0].SqlFieldAttribute = this.typeMap.Expressions[SqlExpressions.PrimaryKeyDeclaredOnColumn];

                    // if this is an int - make it auto increment
                    if (keyField[0].PropertyType == typeof(int) && this.typeMap.Expressions.ContainsKey(SqlExpressions.AutoIncrement))
                        keyField[0].SqlFieldAttribute += " " + this.typeMap.Expressions[SqlExpressions.AutoIncrement];
                }
            }
            #endregion

            // if class is marked as Required, set a default value on a field, unless the field already has Required set - field property takes a priority!
            // doing it after all properties have been processed because we need to know if property has set this field from it's own Required attribute
            object[] required = typeof(T).GetCustomAttributes(typeof(RequiredAttribute), true);
            if (required.Length > 0)
            {
                bool defaultRequired = ((RequiredAttribute)required[0]).IsRequired;
                this.Fields.Where(x => !x.IsRequired.HasValue && !x.IsPrimaryKey).ToList().ForEach(x => x.IsRequired = defaultRequired);
            }
        }

        /// <summary>
        /// Gets the "CREATE TABLE" SQL statement.
        /// </summary>
        /// <returns></returns>
        public string GetSql()
        {
            return GenerateSql();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        override public string ToString()
        {
            return GenerateSql();
        }

        /// <summary>
        /// Generates the SQL from fields.
        /// </summary>
        /// <returns></returns>
        private string GenerateSql()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(createTable + " " + this.TableName + " (");
            for (int i = 0; i < this.Fields.Count; i++)
            {
                if (i > 0) sbSql.Append(", ");
                sbSql.Append(fields[i].ToString());
            }

            // Set primary keys
            Field[] pkFields = this.Fields.Where(x => x.IsPrimaryKey).ToArray();
            if (pkFields.Length > 0)
            {
                if (!this.typeMap.Expressions.ContainsKey(SqlExpressions.PrimaryKeyDeclaredOnTable))
                    throw new MappingNotDefinedException("Expression for Primary key is not defined. Define in DbDataTypesMap.Expresssions.");

                if (pkFields.Length > 1)
                {
                    StringBuilder sbPK = new StringBuilder();
                    for (int i = 0; i < pkFields.Length; i++)
                    {
                        if (i > 0) sbPK.Append(", ");
                        sbPK.Append(pkFields[i].ColumnName);
                    }
                    sbPK.Insert(0, this.typeMap.Expressions[SqlExpressions.PrimaryKeyDeclaredOnTable] + " (").Append(")");
                    sbSql.Append(", " + sbPK.ToString());
                }
            }

            sbSql.Append(")");

            // ok, last step - see if we need to make any replacements
            sbSql.Replace(DbDataTypesMapBase.ReplacementStrings.TableName, this.TableName);
            return sbSql.ToString(); ;
        }
    }
}