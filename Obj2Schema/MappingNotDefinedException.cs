using System;

namespace ET.Obj2Schema
{
    [Serializable]
    public class MappingNotDefinedException : Exception
    {
        public MappingNotDefinedException(Type type) : base("Undefined mapping for type " + type.Name) { }
        public MappingNotDefinedException(string message) : base(message) { }
        protected MappingNotDefinedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
