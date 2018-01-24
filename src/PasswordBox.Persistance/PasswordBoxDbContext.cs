using PasswordBox.Core;
using PasswordBox.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace PasswordBox.Persistance
{
    public class PasswordBoxDbContext : IdentityDbContext<User, Role, string>
    {

        public DbSet<Vault> Vaults { get; set; }


        public PasswordBoxDbContext()
        {

        }

        public PasswordBoxDbContext(DbContextOptions<PasswordBoxDbContext> options) : base(options)
        {

        }


        public void EnsureSeeding()
        {
            //SeedRoles();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var connectionString = AppSettings.Configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            

        }
        
        private void SeedRoles()
        {

            var separtor = Path.DirectorySeparatorChar;

            var rolesFilePath = $"Config{separtor}Seeds{separtor}roles.json";

            if (File.Exists(rolesFilePath))
            {
                var rolesData = File.ReadAllText(rolesFilePath);

                var roles = JsonConvert.DeserializeObject<List<Role>>(rolesData);

                foreach (var item in roles)
                {
                    var found = Roles.Find(item.Id);

                    if (found != null)
                    {
                        found = found.Update(item);
                        Roles.Update(found);
                    }
                    else
                        Roles.Add(item);
                }


                SaveChanges();
            }
        }

        private void SeedDefaultUser()
        {
            var id = "E428D8D6-4C5E-4D33-9437-569195B3B80A".ToLower();
            var email = "user@admin.com";
            var username = "Admin";
            var password = "1122";

            var user = Users.FirstOrDefault(a => a.Id == id);

            if (user == null)
            {
                user = new User()
                {
                    Id = "E428D8D6-4C5E-4D33-9437-569195B3B80A".ToLower(),
                    UserName = username,
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = username.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = true,
                };

                var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);
                user.PasswordHash = hashedPassword;
                Users.Add(user);
            }
            else
            {
                var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);

                user.UserName = username;
                user.Email = email;
                user.NormalizedEmail = email.ToUpper();
                user.NormalizedUserName = username.ToUpper();
                user.PasswordHash = hashedPassword;
                user.LockoutEnabled = true;
            }

            SaveChanges();

            var foundRole = UserRoles.FirstOrDefault(a => a.UserId == user.Id && a.RoleId == AppRoles.ADMIN_ROLE);

            if (foundRole == null)
            {
                var role = new IdentityUserRole<string>();
                role.UserId = user.Id;
                role.RoleId = AppRoles.ADMIN_ROLE;
                UserRoles.Add(role);
            }

            SaveChanges();
        }


        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
