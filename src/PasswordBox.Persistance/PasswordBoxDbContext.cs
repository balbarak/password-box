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

namespace PasswordBox.Persistance
{
    public class PasswordBoxDbContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductItem> ProductItems { get; set; }
        
        public PasswordBoxDbContext()
        {

        }

        public PasswordBoxDbContext(DbContextOptions<PasswordBoxDbContext> options) : base(options)
        {

        }


        public void EnsureSeeding()
        {
            SeedRoles();
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

            builder.Entity<Product>().HasMany(a => a.Items)
                .WithOne(a => a.Product)
                .OnDelete(DeleteBehavior.Cascade);

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
        
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
