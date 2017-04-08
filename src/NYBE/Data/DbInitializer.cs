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
                    new Book{ISBN="8323135460",Title="ASP.NET for Dummies",AuthorLName="Gates",AuthorFName="Bill",Description="Learn how to make an ASP.NET site in minutes!",Edition="2nd",Publisher="Microsoft"},
                    new Book{ISBN="0676973760 ",Title="Life of Pi",AuthorLName="Martel",AuthorFName="Yann",Description="The protagonist, Piscine Molitor 'Pi' Patel, an Indian boy from Pondicherry, explores issues of spirituality and practicality from an early age. He survives 227 days after a shipwreck while stranded on a lifeboat in the Pacific Ocean with a Bengal tiger named Richard Parker.",Edition="1st",Publisher="Knopf Canada"},
                    new Book{ISBN="0446310786",Title="To Kill a Mockingbird",AuthorLName="Lee",AuthorFName="Harper",Description="The unforgettable novel of a childhood in a sleepy Southern town and the crisis of conscience that rocked it, To Kill A Mockingbird became both an instant bestseller and a critical success when it was first published in 1960. It went on to win the Pulitzer Prize in 1961 and was later made into an Academy Award-winning film, also a classic.",Edition="3rd",Publisher="J. B. Lippincott & Co."},
                    new Book{ISBN="1234563370",Title="Moby Dick",AuthorLName="Melville",AuthorFName="Herman",Description="Sailor Ishmael tells the story of the obsessive quest of Ahab, captain of the whaler the Pequod, for revenge on Moby Dick, the white whale that on the previous whaling voyage bit off Ahab's leg at the knee.",Edition="2nd",Publisher="Harper & Brothers"},
                    new Book{ISBN="7654312210",Title="Adventures of Huckleberry Finn",AuthorLName="Twain",AuthorFName="Mark",Description="Set in a Southern antebellum society that had ceased to exist about 20 years before the work was published, Adventures of Huckleberry Finn is an often scathing satire on entrenched attitudes, particularly racism.",Edition="150th",Publisher="Charles L. Webster And Company."},
                    new Book{ISBN="1233456740",Title="A Tale of Two Cities",AuthorLName="Dickens",AuthorFName="Charles",Description="The novel depicts the plight of the French peasantry demoralized by the French aristocracy in the years leading up to the revolution, the corresponding brutality demonstrated by the revolutionaries toward the former aristocrats in the early years of the revolution, and many unflattering social parallels with life in London during the same period",Edition="6th",Publisher="Chapman & Hall"},
                    new Book{ISBN="0000001020",Title="The Tragedy of Hamlet",AuthorLName="Shakespeare",AuthorFName="William",Description="The story of ya boy hamlet",Edition="2nd",Publisher="16th Century Europe"},
                    new Book{ISBN="3463754230",Title="The Adventures of Tom Sawyer",AuthorLName="Twain",AuthorFName="Mark",Description="1876 novel about a young boy growing up along the Mississippi River. It is set in the fictional town of St. Petersburg, inspired by Hannibal, Missouri, where Twain lived.",Edition="150th",Publisher="American Publishing Company"},
                    new Book{ISBN="1234256704",Title="Charlotte's Web",AuthorLName="Williams",AuthorFName="Garth",Description="The novel tells the story of a pig named Wilbur and his friendship with a barn spider named Charlotte. When Wilbur is in danger of being slaughtered by the farmer, Charlotte writes messages praising Wilbur (such as 'Some Pig') in her web in order to persuade the farmer to let him live.",Edition="9th",Publisher="Harper & Brothers"}
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
                    new School{Name="None", City="N/A", State="N/A" },
                    new School{Name="UW - Milwaukee",City="Milwaukee", State="WI"},
                    new School{Name="Marquette University",City="Milwaukee", State="WI"},
                    new School{Name="MSOE",City="Milwaukee", State="WI"},
                    new School{Name="MIAD",City="Milwaukee", State="WI"},
                    new School{Name="UCLA",City="Los Angeles", State="CA"}
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
            user.SchoolID = 3;
            user.Email = "user@test.com";
            user.UserName = user.Email;
            user.Status = 1;
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
            user2.Status = 1;
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
            admin.Status = 1;
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