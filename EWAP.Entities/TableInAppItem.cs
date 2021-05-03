using EWAP.Abstraction.DbInterfaces;
using EWAP.Abstraction.DbInterfaces.InAppItems;
using MongoORM4NetCore;
using MongoORM4NetCore.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EWAP.Entities
{
    public static class HashAndSalt
    {
        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(System.Text.Encoding.UTF8.GetBytes(value), salt);
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();
            return new System.Security.Cryptography.SHA256Managed().ComputeHash(saltedValue);
        }
        public static bool IsCorrect(string password, byte[] salt, byte[] orginal)
        {
            byte[] passwordHash = Hash(password, salt);
            return orginal.SequenceEqual(passwordHash);
        }
    }
    public static class SaltFacade
    {
        public static Salt SaveSalt(string userId, byte[] password)
        {
            try
            {
                var saltCrud = new Crud<Salt>();
                var oldSalt = saltCrud.GetAll().FirstOrDefault(x => x.UserId == userId);
                if (oldSalt != null)
                {
                    saltCrud.Delete(oldSalt.Id);
                }
                return new Salt { UserId = userId, SaltData = password };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] ReadSalt(string userId)
        {
            var saltCrud = new Crud<Salt>();
            return saltCrud.GetAll().FirstOrDefault(x => x.UserId == userId).SaltData;
        }
    }

    public static class SecurityFacade
    {

        static byte[] GeneratePassword(string userPassword, byte[] salt)
        {
            return HashAndSalt.Hash(userPassword, salt);
        }
        public static Salt SaveSalt(string userId, string userPassword)
        {
            var userCrud = new Crud<User>();
            var salt = GetSalt();
            var password = GeneratePassword(userPassword, salt);
            var user = userCrud.GetAll().FirstOrDefault(x => x.Id == userId);
            user.Hash = password;
            userCrud.Update(userId, user);
            return SaltFacade.SaveSalt(userId, salt);
        }
        public static User IsPasswordCorrect(string email, string userPassword)
        {
            var userCrud = new Crud<User>();

            var user = userCrud.GetAll().LastOrDefault(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            byte[] usersSalt = GetUsersSalt(user.Id);
            var isCorect = HashAndSalt.IsCorrect(userPassword, usersSalt, user.Hash);
            if (!isCorect)
            {
                return null;
            }
            return user;

        }
        //todo: read users salt from jsonfile and convert it to byte array and return
        private static byte[] GetUsersSalt(string userId)
        {
            return SaltFacade.ReadSalt(userId);
        }
        static byte[] GetAsByteArray(string input)
        {
            return Encoding.ASCII.GetBytes(input);
        }
        static byte[] GetSalt(int maximumSaltLength = 32)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }


    public class User : DbObject, IUser
    {
        public string Email { get; set; }
        public byte[] Hash { get; set; }
    }

    public class Salt : DbObject, ISalt
    {
        public string UserId { get; set; }
        public byte[] SaltData { get; set; }
    }

    public class TableInAppItem : Item, ITableInAppItem
    {
        public string HtmlText { get; set; }
    }
}
