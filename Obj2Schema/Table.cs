using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ET.Obj2Schema.Maps;
using System.Diagnostics;
using ET.Obj2Schema.Attributes;
using Obj2Schema.Attributes;

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

            ProcessType();
        }


        private void ProcessType()
        {
            this.TableName = GetTableName();
            this.Fields.AddRange(CreateFields(this.targetType));

            // If property name is ID, or typeId, and there are no primary keys already set - make it primary key
            SetPrimaryKeyByConvention(this.Fields);

            MarkFieldsRequired();
        }


        /// <summary>
        /// Gets the "CREATE TABLE" SQL statement.
        /// </summary>
        /// <returns></returns>
        public virtual string GetSql()
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
            return sbSql.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        override public string ToString()
        {
            return GetSql();
        }

        #region Utility methods
        /// <summary>
        /// Gets the name of the table for a given type - either from a Table attribute, if exists, or derived from a type itself.
        /// </summary>
        /// <param name="targetType">The type.</param>
        /// <returns></returns>
        protected virtual string GetTableName()
        {
            object[] tableName = targetType.GetCustomAttributes(typeof(TableAttribute), true);
            if (tableName == null || tableName.Length == 0)
                return targetType.Name;
            else
                return ((TableAttribute)tableName[0]).Name;
        }

        /// <summary>
        /// Creates the fields for a given type, mapping them from provided type map.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="typeMap">The type map.</param>
        /// <returns></returns>
        protected virtual List<Field> CreateFields(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            if (typeMap == null)
                throw new ArgumentNullException("typeMap");

            // get public properties, and create fields
            PropertyInfo[] properties = targetType.GetProperties().Where(x => x.CanRead).ToArray();
            List<Field> fields = new List<Field>(properties.Length);

            foreach (PropertyInfo property in properties)
            {
                Type ptype = property.PropertyType;

                // check if property is a complex type; attribute can be applied wither at class or property level - check both.
                ComplexTypeAttribute complexTypeAtt = (ptype.GetCustomAttributes(typeof(ComplexTypeAttribute), true).Any() ?
                        ptype.GetCustomAttributes(typeof(ComplexTypeAttribute), true).SingleOrDefault() as ComplexTypeAttribute :
                        property.GetCustomAttributes(typeof(ComplexTypeAttribute), true).SingleOrDefault() as ComplexTypeAttribute);

                if (complexTypeAtt != null)
                {
                    List<Field> ctFields = CreateFields(ptype);

                    // if prepended string is not set (is null) - use the type name as a prepended string; otherwise use user-supplied string.
                    string prepString = (complexTypeAtt.PrependedString == null ? ptype.Name : complexTypeAtt.PrependedString);
                    ctFields.ForEach(x => x.ColumnName = prepString + x.ColumnName);
                    fields.AddRange(ctFields);
                    continue;
                }
                else if ((!typeMap.HasMappingFor(ptype) && !property.GetCustomAttributes(typeof(ComplexTypeAttribute), true).Any()) ||
                        property.GetCustomAttributes(typeof(IgnoreAttribute), true).Any())
                    continue;

                // normal properties (not complex types)
                Field field = new Field(property, typeMap);
                fields.Add(field);
            }
            return fields;
        }

        /// <summary>
        /// If given set of fields doesn't contain primary key, and collection contains field named "id" or 
        /// </summary>
        /// <param name="fields">The fields.</param>
        protected virtual void SetPrimaryKeyByConvention(List<Field> fields)
        {
            if (this.Fields.Any(x => x.IsPrimaryKey))
                return;

            List<Field> keyField = fields.Where(x => x.ColumnName.ToLower() == "id" || x.ColumnName.ToLower() == this.TableName.ToLower() + "id").ToList();
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

        /// <summary>
        /// If class is marked as Required, set a default value on a field, unless the field already has Required set - field property takes a priority!
        /// Do it after all properties have been processed because we need to know if property has set this field from it's own Required attribute.
        /// </summary>
        protected virtual void MarkFieldsRequired()
        {

            object[] required = this.targetType.GetCustomAttributes(typeof(RequiredAttribute), true);
            if (required.Length > 0)
            {
                bool defaultRequired = ((RequiredAttribute)required[0]).IsRequired;
                this.Fields.Where(x => !x.IsRequired.HasValue && !x.IsPrimaryKey).ToList().ForEach(x => x.IsRequired = defaultRequired);
            }
        }

        #endregion
    }
}