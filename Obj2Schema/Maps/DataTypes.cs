namespace ET.Obj2Schema.Maps
{
    /// <summary>
    /// Lists .NET data types, that can be mapped to Sql data types. 
    /// This enum exists to make it easier to map between .NET and SQL.
    /// </summary>
    public enum DataTypes
    {
        Bool, Byte, Char, DateTime, Decimal, Double, Float, Int, Long, Short, String, Guid, Enum, TimeSpan
    }
}
