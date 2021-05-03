using MongoORM4NetCore.Interfaces;
using System;

namespace EWAP.Abstraction.DbInterfaces
{
    public interface ISalt : IDbObject
    {
        public string UserId { get; set; }
        public byte[] SaltData { get; set; }
    }
}
