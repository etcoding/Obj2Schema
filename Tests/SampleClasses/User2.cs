using System;
using ET.Obj2Schema;

namespace Tests.SampleClasses
{
    /// <summary>
    /// Sample class, configured using attributes.
    /// </summary>
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
}
