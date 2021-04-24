using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using upband.Data.Entities;

namespace upband
{

    public class SHA512Service : IPasswordHasher<user>
    {
        public HashAlgorithm cc { get; set; }
        public SHA512Service() {
            cc = SHA512.Create();
        }
        public string HashPassword(user User,string password) {
            
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            var hashedData = cc.ComputeHash(bytes);
            cc.Clear();
            return Encoding.ASCII.GetString(hashedData);
        }
        public PasswordVerificationResult VerifyHashedPassword(user User,string x, string y) {
            if (x.Equals(y))
            {
                return PasswordVerificationResult.Success;
            }
            else return PasswordVerificationResult.Failed;
        }
    }
}
