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

            if (context.Books.Any())
            {
                // books has been seeded
            }
            else
            {
                var books = new Book[]
                {
                    new Book{ISBN="111111111",Title="ASP.NET for Dummies",AuthorLName="Saari",AuthorFName="Paul",Description="A really dumb book.",Edition="2nd",Publisher="UWM"},
                    new Book{ISBN="222222222",Title="History of Tennis",AuthorLName="Federer",AuthorFName="Roger",Description="Also a really dumb book.",Edition="15th",Publisher="United States Govt."},
                    new Book{ISBN="333333333",Title="Book of Bad Jokes",AuthorLName="Schumer",AuthorFName="Amy",Description="Its just not funny",Edition="3rd",Publisher="Sony"}
                };
                foreach (Book s in books)
                {
                    context.Books.Add(s);
                }
                context.SaveChanges();
            }

            if (context.Schools.Any())
            {
                // already have schools
            }
            else
            {
                var schools = new School[]
                {
                    new School{Name="University of Wisconsin Milwaukee",City="Milwaukee", State="Wisconsin"},
                    new School{Name="Marquette University",City="Milwaukee", State="Wisconsin"}
                };
                foreach (School e in schools)
                {
                    context.Schools.Add(e);
                }
                context.SaveChanges();
            }

            if (context.Courses.Any())
            {

            }
            else
            {
                var courses = new Course[]
                {
                    new Course{Dept="CS",Name="Capstone",CourseNum=595,SchoolID=1},
                    new Course{Dept="CS",Name="Intro to Algorithms",CourseNum=351,SchoolID=1}
                };
                foreach (Course c in courses)
                {
                    context.Courses.Add(c);
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