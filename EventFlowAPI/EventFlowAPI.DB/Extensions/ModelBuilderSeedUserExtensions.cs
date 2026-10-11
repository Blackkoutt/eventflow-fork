using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderSeedUserExtensions
    {
        public static void SeedUsers(this ModelBuilder modelBuilder, DateTime today)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    Name = "Admin",
                    Surname = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEK4/hlBoE1k2Sdq8n2DYNHysLOR3Fg/7anvL/W7qoX0GxzJBNLQqG79m14mPJ6iwrw==",
                    Role = Roles.Admin,
                    PhotoName = "admin.jpg",
                    RegisteredDate = today,
                    DateOfBirth = new DateTime(2000, 4, 3)
                },
                new User
                {
                    Id = "2",
                    Name = "Mateusz2",
                    Surname = "Strapczuk2",
                    Email = "mateusz.strapczuk2@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEG6rZhspfbU8XJjYA5dsJzuNC6oO71cVoB6GJ3pm3voqJFOM7pvY0OsDo5PZDIuzxA==",
                    Role = Roles.User,
                    PhotoName = "user2.jpg",
                    RegisteredDate = today,
                    DateOfBirth = new DateTime(1985, 2, 1)
                },
                new User
                {
                    Id = "3",
                    Name = "Mateusz3",
                    Surname = "Strapczuk3",
                    Email = "mateusz.strapczuk3@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEAlgQ4Qs/GFmn3hHt58p/CNdbRcnd2+tGvTWqNDxznjanmwir/VeibWWI3B9jZhf8g==",
                    Role = Roles.User,
                    PhotoName = "user3.jpg",
                    RegisteredDate = today,
                    DateOfBirth = new DateTime(1979, 12, 11)
                },
                new User
                {
                    Id = "4",
                    Name = "Mateusz4",
                    Surname = "Strapczuk4",
                    Email = "mateusz.strapczuk4@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEErLVney/2f4Wm11sn7nK8cNngA8P3qvHSertKvrBHNSE/000BYDaS+/F5tYAhMIFQ==",
                    Role = Roles.User,
                    PhotoName = "user4.jpg",
                    RegisteredDate = today,
                    DateOfBirth = new DateTime(1979, 12, 11)
                },
                new User
                {
                    Id = "5",
                    Name = "Mateusz",
                    Surname = "Strapczuk",
                    Email = "mateusz.strapczuk1@gmail.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEFjlpJmB1mkCX/au5X5r7DmJLIjfboZ0uKo0vyl9ZfqXDpEWpvdISbzfLhblOMN/uQ==",
                    Role = Roles.User,
                    PhotoName = "user5.jpg",
                    RegisteredDate = today,
                    DateOfBirth = new DateTime(1979, 12, 11)
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
