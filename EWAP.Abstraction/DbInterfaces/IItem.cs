using MongoORM4NetCore.Interfaces;
using System;

namespace EWAP.Abstraction.DbInterfaces
{
    public interface IItem : IDbObject
    {
        public string IconPath { get; set; }
        public string UserId { get; set; } //OwnerId
    }

}
