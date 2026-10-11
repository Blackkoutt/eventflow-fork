using EventFlowAPI.DB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderSeedIdentityExtensions
    {
        public static void SeedUsers(this ModelBuilder modelBuilder, DateTime today)
        {
            var passwordHasher = new SeedPasswordHasher();
            var user1 = new User
            {
                Id = "1",
                ConcurrencyStamp = "95ca56b0-be1b-4b9a-b9cb-77b705ab1bbc",
                SecurityStamp = "b0fcb126-8927-42fb-8d13-13c4f1681351",
                Name = "Admin",
                Surname = "Admin",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                PhotoName = "admin.jpg",
                EmailConfirmed = true,
                RegisteredDate = today,
                DateOfBirth = new DateTime(2000, 4, 3)
            };
            user1.PasswordHash = passwordHasher.Hash(user1.Id, "admin123");

            var user2 = new User
            {
                Id = "2",
                ConcurrencyStamp = "c79bc252-1135-418f-a752-59ea3ced3ad1",
                SecurityStamp = "28db39ee-4875-4a8c-b689-02eabddb96aa",
                Name = "Mateusz2",
                Surname = "Strapczuk2",
                Email = "mateusz.strapczuk2@gmail.com",
                UserName = "mateusz.strapczuk2@gmail.com",
                NormalizedEmail = "mateusz.strapczuk2@gmail.com".ToUpper(),
                NormalizedUserName = "mateusz.strapczuk2@gmail.com".ToUpper(),
                PhotoName = "user2.jpg",
                RegisteredDate = today,
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1985, 2, 1)
            };
            user2.PasswordHash = passwordHasher.Hash(user2.Id, "789456123qaz");

            var user3 = new User
            {
                Id = "3",
                ConcurrencyStamp = "fdf14307-4b05-422d-b5ae-a8571afd609d",
                SecurityStamp = "959425f8-a938-433a-923e-d5d01925122c",
                Name = "Mateusz3",
                Surname = "Strapczuk3",
                Email = "mateusz.strapczuk3@gmail.com",
                UserName = "mateusz.strapczuk3@gmail.com",
                NormalizedEmail = "mateusz.strapczuk3@gmail.com".ToUpper(),
                NormalizedUserName = "mateusz.strapczuk3@gmail.com".ToUpper(),
                PhotoName = "user3.jpg",
                RegisteredDate = today,
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1979, 12, 11)
            };
            user3.PasswordHash = passwordHasher.Hash(user3.Id, "qwe123QWE!@#");

            var user4 = new User
            {
                Id = "4",
                ConcurrencyStamp = "cbbf0722-3f05-411d-b87f-a05fe3726e3c",
                SecurityStamp = "9bd8c1e8-5875-4e50-876d-3a69c2fdc470",
                Name = "Mateusz4",
                Surname = "Strapczuk4",
                Email = "mateusz.strapczuk4@gmail.com",
                UserName = "mateusz.strapczuk4@gmail.com",
                NormalizedEmail = "mateusz.strapczuk4@gmail.com".ToUpper(),
                NormalizedUserName = "mateusz.strapczuk4@gmail.com".ToUpper(),
                PhotoName = "user4.jpg",
                RegisteredDate = today,
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1979, 12, 11)
            };
            user4.PasswordHash = passwordHasher.Hash(user4.Id, "qwe123QWE!@#");

            var user5 = new User
            {
                Id = "5",
                ConcurrencyStamp = "1a9d5c0a-7f87-4f87-bbfe-ea099479ac03",
                SecurityStamp = "f1e975b0-0b98-4b68-8f3b-fbcb7a0c22f4",
                Name = "Mateusz",
                Surname = "Strapczuk",
                Email = "mateusz.strapczuk1@gmail.com",
                UserName = "mateusz.strapczuk1@gmail.com",
                NormalizedEmail = "mateusz.strapczuk1@gmail.com".ToUpper(),
                NormalizedUserName = "mateusz.strapczuk1@gmail.com".ToUpper(),
                PhotoName = "user5.jpg",
                RegisteredDate = today,
                EmailConfirmed = true,
                DateOfBirth = new DateTime(1979, 12, 11)
            };
            user5.PasswordHash = passwordHasher.Hash(user5.Id, "qazzaq1@WSX");

            modelBuilder.Entity<User>().HasData(
                user1, user2, user3, user4, user5
            );
        }

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = "1",
                    ConcurrencyStamp = null,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    Description = "Admin role"
                },
                new Role
                {
                    Id = "2",
                    ConcurrencyStamp = null,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    Description = "User role"
                }
            );
        }
        public static void SeedUsersInRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1",
                    RoleId = "1",
                },
                new IdentityUserRole<string>
                {
                    UserId = "2",
                    RoleId = "2",
                },
                new IdentityUserRole<string>
                {
                    UserId = "3",
                    RoleId = "2",
                },
                new IdentityUserRole<string>
                {
                    UserId = "4",
                    RoleId = "2",
                },
                new IdentityUserRole<string>
                {
                    UserId = "5",
                    RoleId = "2",
                }
            );
        }


        public static void SeedUsersData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>().HasData(
               new UserData
               {
                   Id = "1",
                   Street = "Wesoła",
                   HouseNumber = 12,
                   FlatNumber = 14,
                   City = "Warszawa",
                   ZipCode = "15-264",
                   PhoneNumber = "789456123"
               },
               new UserData
               {
                   Id = "2",
                   Street = "Wiejska",
                   HouseNumber = 10,
                   FlatNumber = 31,
                   City = "Poznań",
                   ZipCode = "01-342",
                   PhoneNumber = "123456789"
               },
               new UserData
               {
                   Id = "3",
                   Street = "Pogodna",
                   HouseNumber = 7,
                   FlatNumber = 21,
                   City = "Białystok",
                   ZipCode = "14-453",
                   PhoneNumber = "147852369"
               },
               new UserData
               {
                   Id = "4",
                   Street = "Słoneczna",
                   HouseNumber = 21,
                   FlatNumber = 42,
                   City = "Warszawa",
                   ZipCode = "14-453",
                   PhoneNumber = "147852369"
               }
           );
        }
    }
}
