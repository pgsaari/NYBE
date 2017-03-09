using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NYBE.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace NYBE.Models
{
    public static class DbInitializer
    {

        public enum Roles
        {
            Admin, // can approve & deny books or whatever
            User, // regular user of the website
            SuperUser // a 3rd role we may not use that would be one step above the admin role
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Books.Any())
            {
                var books = new Book[]
                {
                    new Book{ISBN="111111111",Title="ASP.NET for Dummies",AuthorLName="Saari",AuthorFName="Paul",Description="A really dumb book.",Edition="2nd",Publisher="UWM"},
                    new Book{ISBN="222222222",Title="History of Tennis",AuthorLName="Federer",AuthorFName="Roger",Description="Also a really dumb book.",Edition="15th",Publisher="United States Govt."},
                    new Book{ISBN="333333333",Title="Book of Bad Jokes",AuthorLName="Schumer",AuthorFName="Amy",Description="Its just not funny",Edition="3rd",Publisher="Sony"},
                    new Book{ISBN="123456337",Title="Moby Dick",AuthorLName="Smith",AuthorFName="Will",Description="Book about a whale",Edition="2nd",Publisher="who knows"},
                    new Book{ISBN="765431221",Title="Huck Finn",AuthorLName="Twain",AuthorFName="Mark",Description="Old book about a boy.",Edition="150th",Publisher="Universal"},
                    new Book{ISBN="123345674",Title="Fake Book 6",AuthorLName="Reagan",AuthorFName="Ronald",Description="6th book in the long line of fake books!",Edition="6th",Publisher="Reagan Inc."},
                    new Book{ISBN="000000102",Title="Hamlet",AuthorLName="Shakespeare",AuthorFName="Will",Description="The story of ya boy hamlet",Edition="2nd",Publisher="16th Century Europe"},
                    new Book{ISBN="346375423",Title="Tom Sawyer",AuthorLName="Twain",AuthorFName="Mark",Description="Old book about a boy still.",Edition="150th",Publisher="Universal"},
                    new Book{ISBN="123425674",Title="Fake Book 9",AuthorLName="Bush",AuthorFName="Jeb",Description="Why do politicians keep writing fake books?",Edition="9th",Publisher="Bush Inc."}
                };
                foreach (Book s in books)
                {
                    context.Books.Add(s);
                }
                context.SaveChanges();
            }

            if (!context.Schools.Any())
            {
                var schools = new School[]
                {
                    new School{Name="University of Wisconsin Milwaukee",City="Milwaukee", State="Wisconsin"},
                    new School{Name="Marquette University",City="Milwaukee", State="Wisconsin"},
                    new School{Name="MSOE",City="Milwaukee", State="Wisconsin"},
                    new School{Name="MIAD",City="Milwaukee", State="Wisconsin"},
                    new School{Name="UCLA",City="Los Angeles", State="California"}
                };
                foreach (School e in schools)
                {
                    context.Schools.Add(e);
                }
                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                var courses = new Course[]
                {
                    new Course{Dept="CS",Name="Capstone",CourseNum=595,SchoolID=1},
                    new Course{Dept="CS",Name="Intro to Algorithms",CourseNum=351,SchoolID=1},
                    new Course{Dept="Art",Name="Intro Art",CourseNum=101,SchoolID=4},
                    new Course{Dept="Bus",Name="Entrepreneurship",CourseNum=236,SchoolID=5},
                    new Course{Dept="EE",Name="Intro Circuits",CourseNum=201,SchoolID=3},
                    new Course{Dept="Eng",Name="Public Speaking",CourseNum=124,SchoolID=2},
                    new Course{Dept="Art",Name="Advanced Art",CourseNum=201,SchoolID=4},
                    new Course{Dept="Comm",Name="21st Century Communication",CourseNum=350,SchoolID=5}
                };
                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
                }
                context.SaveChanges();
            }

        }

        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var passwordHash = new PasswordHasher<ApplicationUser>();

            // create a test user
            ApplicationUser user = new ApplicationUser();
            user.FirstName = "User";
            user.LastName = "Test";
            user.Rating = 4.5;
            user.PhoneNumber = "111-555-5556";
            user.SchoolID = 1;
            user.Email = "user@test.com";
            user.UserName = user.Email;
            if (!(userManager.Users.Any(a => a.UserName == "user@test.com"))) // if the user doesn't exist then create them
            {
                await userManager.CreateAsync(user, "Password1!");
                await userManager.AddToRoleAsync(user, "User");
            }

            // create a test user
            ApplicationUser user2 = new ApplicationUser();
            user2.FirstName = "Paul";
            user2.LastName = "Sorry";
            user2.Rating = 4.9;
            user2.PhoneNumber = "111-973-4536";
            user2.SchoolID = 1;
            user2.Email = "paul@test.com";
            user2.UserName = user2.Email;
            if (!(userManager.Users.Any(a => a.UserName == "paul@test.com"))) // if the user doesn't exist then create them
            {
                await userManager.CreateAsync(user2, "Password1!");
                await userManager.AddToRoleAsync(user2, "User");
            }

            // create a test admin
            ApplicationUser admin = new ApplicationUser();
            admin.FirstName = "Admin";
            admin.LastName = "Test";
            admin.Rating = 0.1;
            admin.PhoneNumber = "123-456-7890";
            admin.SchoolID = 2;
            admin.Email = "admin@test.com";
            admin.UserName = admin.Email;
            if (!(userManager.Users.Any(a => a.UserName == "admin@test.com")))
            {
                await userManager.CreateAsync(admin, "Password1!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            

        }

        public static async void SeedRelationships(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser user = await userManager.FindByEmailAsync("user@test.com");
            ApplicationUser user2 = await userManager.FindByEmailAsync("paul@test.com");
            ApplicationUser admin = await userManager.FindByEmailAsync("admin@test.com");
            // seed some book listings
            if (!context.BookListings.Any()) 
            {
                var bookListings = new BookListing[]
                {
                    new BookListing(1, user.Id, 1, "Good", 45.99),
                    new BookListing(3, user.Id, 2, "Fair", 10.00),
                    new BookListing(2, user2.Id, 3, "Fair", 15.49),
                    new BookListing(4, user.Id, 4, "Excellent", 14.00)
                };
                foreach (BookListing listing in bookListings)
                {
                    context.Add(listing);
                }
                await context.SaveChangesAsync();
            }

            // seed some transaction history
            if (!context.TransactionLogs.Any())
            {
                var transactions = new TransactionLog[]
                {
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 102.55, "New", new DateTime(2016,5,29)),
                    new TransactionLog(user.Id, user2.Id, 6, 2, 0, 43.22, "Good", new DateTime(2016, 8, 13)),
                    new TransactionLog(user2.Id, user.Id, 7, 2, 0, 67.12, "Fair", new DateTime(2017, 1, 13)),
                    new TransactionLog(user2.Id, admin.Id, 8, 3, 0, 12.99, "Bad", new DateTime(2012, 12, 21)),
                    new TransactionLog(user2.Id, user.Id, 9, 4, 0, 150.00, "Excellent", DateTime.Now)
                };
                foreach(TransactionLog tran in transactions)
                {
                    context.Add(tran);
                }
                await context.SaveChangesAsync();
            }

        }

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}