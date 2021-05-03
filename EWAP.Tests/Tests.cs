using EWAP.Entities;
using MongoORM4NetCore;
using NUnit.Framework;
using System.Linq;

namespace EWAP.Tests
{
    public class Tests
    {
        Crud<TableInAppItem> _crud;
        Crud<User> _userCrud;
        Crud<Salt> _saltCrud;
        [SetUp]
        public void Setup()
        {
            MongoDbConnection.InitializeAndStartConnection(databaseName: "EWAP");
            _crud = new Crud<TableInAppItem>();
            _userCrud = new Crud<User>();
            _saltCrud = new Crud<Salt>();
        }

        [Test]
        public void SouldAddToDatabase()
        {
            var initialCount = _crud.Count;
            _crud.Insert(new TableInAppItem
            {
                HtmlText = "<h1>Hello </h1>"
            });
            var afterInsertCount = _crud.Count;
            Assert.AreEqual(initialCount + 1, afterInsertCount);
        }


        [Test]
        public void ShouldHaveHashData()
        {
            var user = new User
            {
                Email = "test",
                Hash = null
            };
            _userCrud.Insert(user);
            var salt = SecurityFacade.SaveSalt(user.Id, "123456");
            var lastUser = _userCrud.GetAll().LastOrDefault(x => x.Id == user.Id);
            _saltCrud.Insert(salt);
            Assert.IsNotNull(lastUser.Hash);
        }

        [Test]
        public void LastUsersPasswordSholdBeCorrect()
        {
            var lastUser = _userCrud.GetAll().LastOrDefault();
            var user = SecurityFacade.IsPasswordCorrect(lastUser.Email, "123456");
            Assert.IsNotNull(user);
        }

        [Test]
        public void LastUsersPasswordSholdBeInCorrect()
        {
            var lastUser = _userCrud.GetAll().LastOrDefault();
            var user = SecurityFacade.IsPasswordCorrect(lastUser.Email, "1234516");
            Assert.IsNull(user);
        }
    }
}