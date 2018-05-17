using System.Collections.Generic;
using DatingApp.Api.Models;
using DatingApp.API.Data;
using Newtonsoft.Json;

namespace DatingApp.Api.Data {
    public class Seed {
        private readonly DataContext _context;
        public Seed (DataContext context) {
            _context = context;

        }

        public void SeedUsers () {
            //remove existing users in the database
            // _context.Users.RemoveRange (_context.Users);
            // _context.SaveChanges ();

            //seed the users

            var userData = System.IO.File.ReadAllText ("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>> (userData);
            foreach (var user in users) {
                //create the password hash
                byte[] passwordHash, passwordSalt;
                createPasswordHash ("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UserName = user.UserName.ToLower ();

                _context.Users.Add (user);
            }
            _context.SaveChanges ();
        }

        private void createPasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (System.Text.Encoding.UTF8.GetBytes (password));
            }
        }
    }
}