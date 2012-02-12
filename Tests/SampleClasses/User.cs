using System;
using System.Collections.Generic;

namespace Tests.SampleClasses
{
    /// <summary>
    /// Sample class. Uses default values.
    /// </summary>
    public class User
    {
        public int ID { set; get; }
        public Guid UniversalID { set; get; }
        public string FName { set; get; }
        public string LName { set; get; }
        public char MiddleInitial { set; get; }
        public Genders Gender { set; get; }
        public int Age { set; get; }
        public DateTime BirthDate { set; get; }
        public bool IsEmployed { set; get; }
        
        public double Income { set; get; }
        public float F { set; get; }
        public long L { set; get; }
        public short S { set; get; }
        public byte B { set; get; }
        public decimal D { set; get; }

        private List<string> interests = new List<string>();
        public List<string> Interests { get { return this.interests; } }    // Won't be mapped
    }
}
