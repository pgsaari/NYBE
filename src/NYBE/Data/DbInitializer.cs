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
                    new Book{ISBN="8323135460",Title="ASP.NET for Dummies",AuthorLName="Gates",AuthorFName="Bill",Description="Learn how to make an ASP.NET site in minutes!",Edition="2nd",Publisher="Microsoft", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/51UhYFPXMML._SX258_BO1,204,203,200_.jpg"  },
                    new Book{ISBN="0676973760 ",Title="Life of Pi",AuthorLName="Martel",AuthorFName="Yann",Description="The protagonist, Piscine Molitor 'Pi' Patel, an Indian boy from Pondicherry, explores issues of spirituality and practicality from an early age. He survives 227 days after a shipwreck while stranded on a lifeboat in the Pacific Ocean with a Bengal tiger named Richard Parker.",Edition="1st",Publisher="Knopf Canada", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/51xufiFRCtL._SX331_BO1,204,203,200_.jpg"},
                    new Book{ISBN="0446310786",Title="To Kill a Mockingbird",AuthorLName="Lee",AuthorFName="Harper",Description="The unforgettable novel of a childhood in a sleepy Southern town and the crisis of conscience that rocked it, To Kill A Mockingbird became both an instant bestseller and a critical success when it was first published in 1960. It went on to win the Pulitzer Prize in 1961 and was later made into an Academy Award-winning film, also a classic.",Edition="3rd",Publisher="J. B. Lippincott & Co.", Status=1, Image="http://images.contentful.com/7h71s48744nc/2JI02CQtEQI6s0wSoCWS00/dcf8cfad37020fbe5dc104dc38cecff3/To-Kill-a-Mockingbird-Hardcover-L9780061743528.JPG" },
                    new Book{ISBN="1234563370",Title="Moby Dick",AuthorLName="Melville",AuthorFName="Herman",Description="Sailor Ishmael tells the story of the obsessive quest of Ahab, captain of the whaler the Pequod, for revenge on Moby Dick, the white whale that on the previous whaling voyage bit off Ahab's leg at the knee.",Edition="2nd",Publisher="Harper & Brothers", Status=1, Image="http://i.dailymail.co.uk/i/pix/2011/06/15/article-2003632-0C90802A00000578-22_233x351.jpg" },
                    new Book{ISBN="7654312210",Title="Adventures of Huckleberry Finn",AuthorLName="Twain",AuthorFName="Mark",Description="Set in a Southern antebellum society that had ceased to exist about 20 years before the work was published, Adventures of Huckleberry Finn is an often scathing satire on entrenched attitudes, particularly racism.",Edition="150th",Publisher="Charles L. Webster And Company.", Status=1, Image="http://4.bp.blogspot.com/-rqXaL4EtoTI/VYvWc3sBEWI/AAAAAAAAKHo/OS-UlHn_11Q/s1600/theadventuresofhuckleberryfinn.jpg" },
                    new Book{ISBN="1233456740",Title="A Tale of Two Cities",AuthorLName="Dickens",AuthorFName="Charles",Description="The novel depicts the plight of the French peasantry demoralized by the French aristocracy in the years leading up to the revolution, the corresponding brutality demonstrated by the revolutionaries toward the former aristocrats in the early years of the revolution, and many unflattering social parallels with life in London during the same period",Edition="6th",Publisher="Chapman & Hall", Status=1, Image="http://www.freebooks.com/wp-content/uploads/2013/01/a-tale-of-two-cities.jpg" },
                    new Book{ISBN="0000001020",Title="The Tragedy of Hamlet",AuthorLName="Shakespeare",AuthorFName="William",Description="The story of ya boy hamlet",Edition="2nd",Publisher="16th Century Europe", Status=1, Image="http://ebook.uploads.worldlibrary.net/uploads/userfiles/20130423225001tragedy_of_hamlet_jpg.jpg" },
                    new Book{ISBN="3463754230",Title="The Adventures of Tom Sawyer",AuthorLName="Twain",AuthorFName="Mark",Description="1876 novel about a young boy growing up along the Mississippi River. It is set in the fictional town of St. Petersburg, inspired by Hannibal, Missouri, where Twain lived.",Edition="150th",Publisher="American Publishing Company", Status=1, Image="https://s-media-cache-ak0.pinimg.com/originals/51/c5/eb/51c5ebaacbe9818574460c02d41bf4e4.jpg" },
                    new Book{ISBN="1234256704",Title="Charlotte's Web",AuthorLName="Williams",AuthorFName="Garth",Description="The novel tells the story of a pig named Wilbur and his friendship with a barn spider named Charlotte. When Wilbur is in danger of being slaughtered by the farmer, Charlotte writes messages praising Wilbur (such as 'Some Pig') in her web in order to persuade the farmer to let him live.",Edition="9th",Publisher="Harper & Brothers", Status=1, Image="https://bi.hcpdts.com/page/450/EwIaWqDxBJPJUu7rJh2VzSLlPeM!iR6JEYqIC4vJLZnl+pDL14wdGFL3HP9J!2Y4B0IX9EVzs!jwPJDv662h7pSRgjDoYa4D0WEjXV2Xa+y4I8Sa35lqltGQDsxSA7ym/UrP4fwuq1G3L+lCQHXVjJ4WD9n1O4!fHVzU32t1zotb2XltGqt5NH08Zg1lv!rMx0rUDeeqoUwC9Vrx87vEQ1LSjaF5npgVSlS9ewe+XLyc=" }
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
                    new School{Name="None", City="N/A", State="N/A", Status=1 },
                    new School{Name="UW - Milwaukee",City="Milwaukee", State="WI", Status=1 },
                    new School{Name="Marquette University",City="Milwaukee", State="WI", Status=1 },
                    new School{Name="MSOE",City="Milwaukee", State="WI", Status=1 },
                    new School{Name="MIAD",City="Milwaukee", State="WI", Status=1 },
                    new School{Name="UCLA",City="Los Angeles", State="CA", Status=1 }
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
            user.PreferredContact = 0;
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
            user2.PreferredContact = 2;
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
            admin.PreferredContact = 1;
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
                    new BookListing(1, user.Id, 1, "Good", 45.99, 1),
                    new BookListing(3, user.Id, 2, "Fair", 10.00, 0),
                    new BookListing(2, user2.Id, 3, "Fair", 15.49, 0),
                    new BookListing(4, user.Id, 4, "Excellent", 14.00, 1),
                    new BookListing(4, user2.Id, 4, "Fair", 22.95, 0)
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
                    // ---- Same Condition ---- \\
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 102.55, "New", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 96.45, "New", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 98.35, "New", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 105.03, "New", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 101.67, "New", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.34, "New", new DateTime(2016,5,10)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.00, "New", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 101.34, "New", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 99.34, "New", new DateTime(2016,5,11)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 98.47, "New", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.67, "New", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 103.45, "New", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 93.34, "New", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 95.86, "New", new DateTime(2016,5,12)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 104.21, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 102.34, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.61, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 103.23, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 99.94, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 96.01, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 89.34, "New", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 97.75, "New", new DateTime(2016,5,13)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 108.50, "New", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 106.60, "New", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 102.93, "New", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 101.63, "New", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.43, "New", new DateTime(2016,5,14)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 103.94, "New", new DateTime(2016,5,15)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 101.34, "New", new DateTime(2016,5,16)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 100.56, "New", new DateTime(2016,5,16)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 96.63, "New", new DateTime(2016,5,16)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 99.02, "New", new DateTime(2016,5,16)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 103.43, "New", new DateTime(2016,5,16)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 109.73, "New", new DateTime(2016,5,16)),
                    // -------- \\

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 88.15, "Excellent", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 89.35, "Excellent", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 82.15, "Excellent", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 84.05, "Excellent", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 90.12, "Excellent", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 89.34, "Excellent", new DateTime(2016,5,15)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 87.84, "Excellent", new DateTime(2016,5,16)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 79.15, "Good", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 74.63, "Good", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 70.00, "Good", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 72.75, "Good", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 70.05, "Good", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 77.45, "Good", new DateTime(2016,5,15)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 75.50, "Good", new DateTime(2016,5,16)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 53.12, "Fair", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 50.03, "Fair", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 42.75, "Fair", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 45.16, "Fair", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 46.99, "Fair", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 49.73, "Fair", new DateTime(2016,5,15)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 48.00, "Fair", new DateTime(2016,5,16)),

                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 33.12, "Bad", new DateTime(2016,5,10)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 28.34, "Bad", new DateTime(2016,5,11)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 26.75, "Bad", new DateTime(2016,5,12)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 35.95, "Bad", new DateTime(2016,5,13)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 10.65, "Bad", new DateTime(2016,5,14)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 27.94, "Bad", new DateTime(2016,5,15)),
                    new TransactionLog(user.Id, user2.Id, 5, 5, 0, 30.11, "Bad", new DateTime(2016,5,16)),


                    new TransactionLog(user.Id, user2.Id, 6, 2, 0, 43.22, "Good", new DateTime(2016, 8, 13)),
                    new TransactionLog(user2.Id, user.Id, 7, 2, 0, 67.12, "Fair", new DateTime(2017, 1, 13)),
                    new TransactionLog(user2.Id, admin.Id, 8, 3, 0, 12.99, "Bad", new DateTime(2012, 12, 21)),
                    new TransactionLog(user2.Id, user.Id, 9, 4, 0, 150.00, "Excellent", DateTime.Now),
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