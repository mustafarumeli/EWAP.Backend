using MongoORM4NetCore.Interfaces;

namespace EWAP.Abstraction.DbInterfaces
{
    public interface IUser : IDbObject
    {
        public string Email { get; set; }
        public byte[] Hash { get; set; }
    }
}
