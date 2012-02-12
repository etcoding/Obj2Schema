Obj2Schema
==========


This libriary allows you to take an object and generate a CREATE TABLE Sql statement to store it.

It doesn't do any fancy mapping, and at this moment supports only types that can be directly stored to database, e.g. int, string, bool, enums.
There's no support for collections or inner complex objects yet. Also there's no support for foreign keys, indexes, filegroups, etc.

Right now libriary supports 3 databases: Sql Server, MySql, and Sqlite. Adding new ANSI SQL-compliant database should be very straightforward - 
just create a new map by inheriting from DbDataTypesMapBase or one of the predefined maps, and filling out the Maps and Expressions dictionaries.
Looking at one of the predefined maps should get you started in no time.

The library was created when I was making a Visual Studio extension, which was using Sqlite, and I needed a few tables to store some simple classes.
I guess i'm starting to get lazy, and prefer to write more code to do less - in this case have something to generate SQL schema for me.


Usage:
------

Let's say you have class

```csharp
    class SimpleUser
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
        public Genders Gender { set; get; }
    }

    public enum Genders { Male, Female }  // Yay, there's enum support! Take that, EF!
```

then you can generate sql like this:

```csharp
	Table<SimpleUser> tableSimple = new Table<SimpleUser>(MySqlDataTypesMap.Instance);
	string sql = tableSimple.GetSql();
	// sql is "CREATE TABLE SimpleUser (Id INT PRIMARY KEY AUTO_INCREMENT, 
	//         Name VARCHAR(250) NULL, Age INT NULL, Gender VARCHAR(20) NULL)"
```


You can control the output with attributes:

```csharp
    [Required]  // Make all fields NOT NULL by default
    [Table("Users")]
    public class User2
    {
        [Key]
        [Column("FirstName")]
        [Length(20)]
        public string FirstName { set; get; }
        [Key]
        [Column("LastName")]
        [Length(30)]
        public string LastName { set; get; }
        [EnumDbDataType(EnumDbDataTypes.Int)]
        public Genders Gender { set; get; }
        [Ignore]
        public bool IsEmployed { set; get; }
        [Required(false)]
        [Length("10,2")]
        public double Income { set; get; }
    }
```

Again,

```csharp
	Table<SimpleUser> tableSimple = new Table<SimpleUser>(MySqlDataTypesMap.Instance);
	string sql = tableSimple.GetSql();
	// sql is "CREATE TABLE Users (FirstName VARCHAR(20), LastName VARCHAR(30), Gender INT NOT NULL, 
	//			Income DOUBLE(10,2) NULL, CONSTRAINT pk_Users PRIMARY KEY (FirstName, LastName))"
```


Anyhow, if you find it useful - great. If you have any ideas - drop me a line at evgeni at etcoding dot com.