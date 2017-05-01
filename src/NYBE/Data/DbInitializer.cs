using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NYBE.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Net.Http;

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
                    new Book{ISBN="1234256704",Title="Charlotte's Web",AuthorLName="Williams",AuthorFName="Garth",Description="The novel tells the story of a pig named Wilbur and his friendship with a barn spider named Charlotte. When Wilbur is in danger of being slaughtered by the farmer, Charlotte writes messages praising Wilbur (such as 'Some Pig') in her web in order to persuade the farmer to let him live.",Edition="9th",Publisher="Harper & Brothers", Status=1, Image="https://bi.hcpdts.com/page/450/EwIaWqDxBJPJUu7rJh2VzSLlPeM!iR6JEYqIC4vJLZnl+pDL14wdGFL3HP9J!2Y4B0IX9EVzs!jwPJDv662h7pSRgjDoYa4D0WEjXV2Xa+y4I8Sa35lqltGQDsxSA7ym/UrP4fwuq1G3L+lCQHXVjJ4WD9n1O4!fHVzU32t1zotb2XltGqt5NH08Zg1lv!rMx0rUDeeqoUwC9Vrx87vEQ1LSjaF5npgVSlS9ewe+XLyc=" },
                    new Book{ISBN="0316015849",Title="Twilight",AuthorLName="Stephanie",AuthorFName="Meyer",Description="Isabella Swan's move to Forks, a small, perpetually rainy town in Washington, could have been the most boring move she ever made. But once she meets the mysterious and alluring Edward Cullen, Isabella's life takes a thrilling and terrifying turn. Up until now, Edward has managed to keep his vampire identity a secret in the small community he lives in, but now nobody is safe, especially Isabella, the person Edward holds most dear. The lovers find themselves balanced precariously on the point of a knife-between desire and danger.Deeply romantic and extraordinarily suspenseful, Twilight captures the struggle between defying our instincts and satisfying our desires. This is a love story with bite.",Edition="",Publisher="Little, Brown Books for Young Readers", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/41K99%2BcInvL._SX326_BO1,204,203,200_.jpg" },
                    //10
                    new Book{ISBN="1542975824",Title="20 Ways How to Make a Sandwhich",AuthorLName="Prochazka",AuthorFName="Lukas",Description="What is a sandwich? This question may sound strange, but the answer is not as obvious as you may think. For most of people sandwich is made of two bread-like slices, or part with a filling in between. That's exactly what you will find in this book! You will learn 20 yummy sandwich recipes starting at some very easy ones continued by more complicated sandwich recipes.",Edition="",Publisher="CreateSpace Independent Publishing Platform", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/515%2BPtUDKBL._SX322_BO1,204,203,200_.jpg" },
                    new Book{ISBN="1539888185",Title="How to Make Sandwiches 101: Over 25 Delicious Sandwich Recipes That Will Leave Your Mouth Watering",AuthorLName="Alling",AuthorFName="Ted",Description="Sandwiches are perhaps one of the most versatile and simplest lunch or dinner dish that you can make today. If you are a huge fan of sandwich recipes, then this is one book you will want to check out! Sandwiches themselves can be highly beneficial and can even help those who are on a tight budget. Inside of this book you will discover over 25 of the most delicious sandwich recipes you will ever come across such as healthy cucumber sandwiches recipes, Cuban sandwich recipes and even pulled pork slider recipes. So, what are you waiting for? Get your copy of the How to Make Sandwiches 101 cookbook today and start making some of your favorite sandwich dishes today!",Edition="",Publisher="CreateSpace Independent Publishing Platform", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/51TDCpxDOBL._SX331_BO1,204,203,200_.jpg" },
                    new Book{ISBN="9871968558",Title="Jimmy Freaky Fast John's Ultimate Snadwhich Making Guide",AuthorLName="John",AuthorFName="Jimmy",Description="Do you know how to make a sandwhich? LOL NO YOU DON'T BECAUSE YOU HAVEN'T READ MY BOOK! Learn how to make the best damn sandwhich you've ever tasted so freaky fast you'll forgot you even learnt it! Trust me, I know how to make a sandwhich I'm freakin' Jimmy John!",Edition="",Publisher="Jimmy John Restaurants", Status=1, Image="http://www.success.com/sites/default/files/main/articles/Jimmy-John-Liautaud_0.jpg" },

                    new Book{ISBN="0345453743",Title="The Ultimate Hitchhiker's Guide to the Galaxy",AuthorLName="Adams",AuthorFName="Douglas",Description="In this collection of novels, Arthur Dent is introduced to the galaxy at large when he is rescued by an alien friend seconds before Earth's destruction, and embarks on a series of amazing adventures with his new companion.",Edition="",Publisher="Ballantine Books", Status=1, Image="http://books.google.com/books/content?id=Fu2VnsEW8S0C&printsec=frontcover&img=1&zoom=1&source=gbs_api" },
                    new Book{ISBN="0316056898",Title="Bossypants",AuthorLName="Fey",AuthorFName="Tina",Description="Before Liz Lemon, before \"Weekend Update,\" before \"Sarah Palin,\" Tina Fey was just a young girl with a dream: a recurring stress dream that she was being chased through a local airport by her middle-school gym teacher. She also had a dream that one day she would be a comedian on TV. She has seen both these dreams come true. At last, Tina Fey's story can be told. From her youthful days as a vicious nerd to her tour of duty on Saturday Night Live; from her passionately halfhearted pursuit of physical beauty to her life as a mother eating things off the floor; from her one-sided college romance to her nearly fatal honeymoon -- from the beginning of this paragraph to this final sentence. Tina Fey reveals all, and proves what we've all suspected: you're no one until someone calls you bossy.",Edition="Reprint",Publisher="Reagan Arthur / Little, Brown", Status=1, Image="https://images-na.ssl-images-amazon.com/images/I/41zAv4Ncy0L._SX309_BO1,204,203,200_.jpg" },
                    new Book{ISBN="1451666179",Title="Hyperbole and a Half",AuthorLName="Brosh",AuthorFName="Allie",Description="Collects autobiographical, illustrated essays and cartoons from the author's popular blog and related new material that humorously and candidly deals with her own idiosyncrasies and battles with depression.",Edition="",Publisher="Simon and Schuster", Status=1, Image="http://books.google.com/books/content?id=KYmYAQAAQBAJ&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api" },
                    new Book{ISBN="1250077028",Title="Furiously Happy",AuthorLName="Lawson",AuthorFName="Jenny",Description="#1 New York Times Bestseller In Furiously Happy, a humor memoir tinged with just enough tragedy and pathos to make it worthwhile, Jenny Lawson examines her own experience with severe depression and a host of other conditions, and explains how it has led her to live life to the fullest: \"I've often thought that people with severe depression have developed such a well for experiencing extreme emotion that they might be able to experience extreme joy in a way that ‘normal people' also might never understand. And that's what Furiously Happy is all about.\" Jenny’s readings are standing room only, with fans lining up to have Jenny sign their bottles of Xanax or Prozac as often as they are to have her sign their books. Furiously Happy appeals to Jenny's core fan base but also transcends it. There are so many people out there struggling with depression and mental illness, either themselves or someone in their family—and in Furiously Happy they will find a member of their tribe offering up an uplifting message (via a taxidermied roadkill raccoon). Let's Pretend This Never Happened ostensibly was about embracing your own weirdness, but deep down it was about family. Furiously Happy is about depression and mental illness, but deep down it's about joy—and who doesn't want a bit more of that?",Edition="",Publisher="Flatiron Books", Status=1, Image="http://books.google.com/books/content?id=pZYkjwEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api" },
                    new Book{ISBN="0451467094",Title="Paddle Your Own Canoe",AuthorLName="Offerman",AuthorFName="Nick",Description="The actor known for roles in such productions as Parks and Recreation shares whimsical musings on a range of topics from love and manliness to grooming and eating meat, offering additional discussions of his life before fame and his courtship of his wife, Megan Mullally.",Edition="",Publisher="N A L Trade", Status=1, Image="http://books.google.com/books/content?id=TF_JoAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api" },
                    new Book{ISBN="1515297888",Title="Kama Sutra for Beginners",AuthorLName="Riley",AuthorFName="R.",Description="Kama Sutra For Beginners, Discover The Best Essential Kama Sutra Love Making Techniques. 3rd EDITION Do you want to bring an amplified sense of passion into the bedroom ? Do you want to know how you and your loved one can find immense pleasure by exploring one another's desires ? This e-book is essential for all couples, and will provide both men and women with the knowledge needed to foster a loving intimate relationship at any point in their lives. In this book, you'll learn: Techniques designed to stimulate and satisfy your lover The benefits of Kamasutra, like how it can make you age better and live longer How to employ kissing techniques during your lovemaking sessions How you can kiss the body in order to get your partner even more aroused About how to encourage communication into your love making How to experiment with different approaches, and bringing incredible passion into your relationship A variety of sexual positions And much, much more.. So if you want to have a loving intimate relationship, then read further! Buy your copy today!",Edition="",Publisher="CreateSpace", Status=1, Image="http://books.google.com/books/content?id=pdUkswEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api" },
                    new Book{ISBN="0312379099",Title="I'm Down",AuthorLName="Wolff",AuthorFName="Mishna",Description="Mishna Wolff grew up in a poor black neighborhood with her single father, a white man who truly believed he was black. “He strutted around with a short perm, a Cosby-esqe sweater, gold chains and a Kangol—telling jokes like Redd Fox, and giving advice like Jesse Jackson. You couldn’t tell my father he was white. Believe me, I tried,” writes Wolff. And so from early childhood on, her father began his crusade to make his white daughter down. Unfortunately, Mishna didn’t quite fit in with the neighborhood kids: she couldn’t dance, she couldn’t sing, she couldn’t double Dutch and she was the worst player on her all-black basketball team. She was shy, uncool, and painfully white. And yet when she was suddenly sent to a rich white school, she found she was too “black” to fit in with her white classmates. I’m Down is a hip, hysterical and at the same time beautiful memoir that will have you howling with laughter, recommending it to friends and questioning what it means to be black and white in America.",Edition="",Publisher="St. Martin's Griffin", Status=1, Image="http://books.google.com/books/content?id=gb3lmAEACAAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api" },
                    new Book{ISBN="1617750255",Title="Go the F**k to Sleep",AuthorLName="Mansbach",AuthorFName="Adam",Description="The #1 New York Times best-seller for parents who live in the real world.",Edition="",Publisher="Akashic Books", Status=1, Image="http://books.google.com/books/content?id=MfsjAgAAQBAJ&printsec=frontcover&img=1&zoom=1&source=gbs_api" }
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

        public static async Task SeedUsers(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var passwordHash = new PasswordHasher<ApplicationUser>();

            if (!userManager.Users.Any())
            {
                // create a test user
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "User";
                user.LastName = "Test";
                user.Rating = 0;
                user.PhoneNumber = "918-372-4450";
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
                ApplicationUser paul = new ApplicationUser();
                paul.FirstName = "Paul";
                paul.LastName = "Sorry";
                paul.Rating = 0;
                paul.PhoneNumber = "701-549-9481";
                paul.SchoolID = 1;
                paul.Email = "paul@test.com";
                paul.UserName = paul.Email;
                paul.PreferredContact = 2;
                paul.Status = 1;
                if (!(userManager.Users.Any(a => a.UserName == "paul@test.com"))) // if the user doesn't exist then create them
                {
                    await userManager.CreateAsync(paul, "Password1!");
                    await userManager.AddToRoleAsync(paul, "User");
                }

                // create a test user
                ApplicationUser jimmy = new ApplicationUser();
                jimmy.FirstName = "Jimmy";
                jimmy.LastName = "John";
                jimmy.Rating = 0;
                jimmy.PhoneNumber = "414-967-9014";
                jimmy.SchoolID = 1;
                jimmy.Email = "jimmy@test.com";
                jimmy.UserName = jimmy.Email;
                jimmy.PreferredContact = 0;
                jimmy.Status = 1;
                if (!(userManager.Users.Any(a => a.UserName == "jimmy@test.com"))) // if the user doesn't exist then create them
                {
                    await userManager.CreateAsync(jimmy, "Password1!");
                    await userManager.AddToRoleAsync(jimmy, "User");
                }

                // create a test user
                ApplicationUser bookstore = new ApplicationUser();
                bookstore.FirstName = "UWM";
                bookstore.LastName = "Bookstore";
                bookstore.Rating = 0;
                bookstore.PhoneNumber = "414-229-2418";
                bookstore.SchoolID = 1;
                bookstore.Email = "bookstore@test.com";
                bookstore.UserName = bookstore.Email;
                bookstore.PreferredContact = 2;
                bookstore.Status = 1;
                if (!(userManager.Users.Any(a => a.UserName == "bookstore@test.com"))) // if the user doesn't exist then create them
                {
                    await userManager.CreateAsync(bookstore, "Password1!");
                    await userManager.AddToRoleAsync(bookstore, "User");
                }

                // create a test user
                ApplicationUser billy = new ApplicationUser();
                billy.FirstName = "Billy";
                billy.LastName = "Jockstrap";
                billy.Rating = 0;
                billy.PhoneNumber = "813-918-4937";
                billy.SchoolID = 4;
                billy.Email = "billy@test.com";
                billy.UserName = billy.Email;
                billy.PreferredContact = 1;
                billy.Status = 1;
                if (!(userManager.Users.Any(a => a.UserName == "billy@test.com"))) // if the user doesn't exist then create them
                {
                    await userManager.CreateAsync(billy, "Password1!");
                    await userManager.AddToRoleAsync(billy, "User");
                }

                // create a test admin
                ApplicationUser admin = new ApplicationUser();
                admin.FirstName = "The Real";
                admin.LastName = "Admin";
                admin.Rating = 0;
                admin.PhoneNumber = "804-614-2973";
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

                HttpClient http = new System.Net.Http.HttpClient();
                var list = await http.GetStringAsync("http://deron.meranda.us/data/census-derived-all-first.txt");
                var names = list.Split('\n');
                names = names.Take(names.Length - 1).ToArray();

                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = names[i].Substring(0, names[i].IndexOf(" ")).ToLower();
                    names[i] = names[i].Substring(0, 1).ToUpper() + names[i].Substring(1);
                }
                var schools = context.Schools.ToArray();
                Random r = new Random();
                for (int i = 0; i < 500; i++)
                {
                    var fName = names[r.Next(0, names.Length)];
                    var lName = names[r.Next(0, names.Length)];
                    ApplicationUser usr = new ApplicationUser();
                    usr.FirstName = fName;
                    usr.LastName = lName;
                    usr.Rating = 0;
                    usr.PhoneNumber = r.Next(0, 10) + "" + r.Next(0, 10) + "" + r.Next(0, 10) + "-" + r.Next(0, 10) + "" + r.Next(0, 10) + "" + r.Next(0, 10) + "-" + r.Next(0, 10) + "" + r.Next(0, 10) + "" + r.Next(0, 10) + "" + r.Next(0, 10);
                    usr.SchoolID = r.Next(1, schools.Length);
                    usr.Email = fName.ToLower() + "@test.com";
                    usr.UserName = usr.Email;
                    usr.PreferredContact = r.Next(0, 2);
                    usr.Status = 1;
                    if (!(userManager.Users.Any(a => a.UserName == (fName.ToLower() + "@test.com"))))
                    {
                        await userManager.CreateAsync(usr, "Password1!");
                        await userManager.AddToRoleAsync(usr, "User");
                    }
                }
            }
        }

        public static void SeedRelationships(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var users = context.Users.ToList();
            // 21 Books seeded.
            var bookCount = 21;
            var condition = new string[] { "New", "Excellent", "Good", "Fair", "Bad" };

            // Seed some book listings.
            if (!context.BookListings.Any())
            {
                Random r = new Random();
                DateTime date = new DateTime(2017, 5, 1);
                var course = context.Courses.ToList();
                for (int i = 1; i < bookCount + 1; i++)
                {
                    int basePrice = r.Next(10, 301);
                    double maxPrice = basePrice + (r.Next(0, (basePrice / 2))) + r.NextDouble();
                    int range = (basePrice / 5);
                    double minPrice = (maxPrice - range) + r.NextDouble();
                    for (int c = 0; c < condition.Length; c++)
                    {
                        int seller = r.Next(0, users.Count);
                        double price = r.NextDouble() * (maxPrice - (minPrice)) + minPrice;
                        var courseId = r.Next(1, course.Count + 1);
                        var listing = new BookListing(i, users[seller].Id, courseId, condition[c], price, r.Next(2));
                        context.BookListings.Add(listing);
                        if (!context.BookToCourses.Where(a => a.BookID == i && a.CourseID == courseId).Any())
                        {
                            var bookToCourse = new BookToCourse(i, courseId);
                            context.BookToCourses.Add(bookToCourse);
                            context.SaveChanges();
                        }
                    }
                }
            }
            context.SaveChanges();

            // Seed some transaction history.
            if (!context.TransactionLogs.Any())
            {
                Random r = new Random();
                for (int i = 1; i < bookCount + 1; i++)
                {
                    int basePrice = r.Next(10, 301);
                    DateTime startDate = new DateTime(2017, 4, 1);
                    DateTime endDate = startDate.AddDays(29);
                    while (DateTime.Compare(startDate, endDate) <= 0)
                    {
                        double maxPrice = basePrice + (r.Next(0, (basePrice / 2))) + r.NextDouble();
                        int range = (basePrice / 5);
                        double minPrice = (maxPrice - range) + r.NextDouble();
                        for (int c = 0; c < condition.Length; c++)
                        {
                            int transCount = r.Next(3, 10);
                            for (int t = 0; t < transCount; t++)
                            {
                                int seller = r.Next(0, users.Count);
                                int buyer = r.Next(0, users.Count);
                                do
                                {
                                    buyer = r.Next(0, users.Count);
                                } while (buyer == seller);
                                double rating = 0 + (5 - 0) * r.NextDouble();
                                double price = r.NextDouble() * (maxPrice - (minPrice)) + minPrice;
                                users[seller].Rating += rating;
                                var tran = new TransactionLog(users[seller].Id, users[buyer].Id, i, Math.Round(rating, 1), 0, Math.Round(price, 2), condition[c], startDate);
                                context.TransactionLogs.Add(tran);
                            }
                            //Change price for next condition.
                            maxPrice = minPrice + r.Next(0, (range / 2)) + r.NextDouble();
                            minPrice = (maxPrice - range) + r.NextDouble();
                        }
                        startDate = startDate.AddDays(1);
                    }
                }
                context.SaveChanges();

                // Update the users' rating.
                foreach (ApplicationUser usr in users)
                {
                    var transCount = context.TransactionLogs.Where(a => a.SellerID == usr.Id).Count();
                    var rating = Math.Round(usr.Rating / transCount, 1);
                    usr.Rating = rating;
                    context.Update(usr);
                    //await userManager.UpdateAsync(usr);
                }
                context.SaveChanges();
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