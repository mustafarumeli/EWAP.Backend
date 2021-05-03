using EWAP.Abstraction.DbInterfaces;
using MongoORM4NetCore.Interfaces;
using System;

namespace EWAP.Entities
{
    public abstract class Item : DbObject, IItem
    {
        public string IconPath { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
