using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NYBE.Data;

namespace NYBE.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170409200652_UserID")]
    partial class UserID
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NYBE.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<double>("Rating");

                    b.Property<int>("SchoolID");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("SchoolID");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("NYBE.Models.Book", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorFName");

                    b.Property<string>("AuthorLName");

                    b.Property<string>("Description");

                    b.Property<string>("Edition");

                    b.Property<string>("ISBN");

                    b.Property<string>("Publisher");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("NYBE.Models.BookListing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserID");

                    b.Property<double>("AskingPrice");

                    b.Property<int>("BookID");

                    b.Property<string>("Condition");

                    b.Property<int>("CourseID");

                    b.HasKey("ID");

                    b.HasIndex("ApplicationUserID");

                    b.HasIndex("BookID");

                    b.HasIndex("CourseID");

                    b.ToTable("BookListings");
                });

            modelBuilder.Entity("NYBE.Models.BookToCourse", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookID");

                    b.Property<int>("CourseID");

                    b.HasKey("ID");

                    b.HasIndex("BookID");

                    b.HasIndex("CourseID");

                    b.ToTable("BookToCourses");
                });

            modelBuilder.Entity("NYBE.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CourseNum");

                    b.Property<string>("Dept");

                    b.Property<string>("Name");

                    b.Property<int>("SchoolID");

                    b.HasKey("ID");

                    b.HasIndex("SchoolID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("NYBE.Models.DataModels.EditListingViewModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("bookID");

                    b.Property<string>("condition");

                    b.Property<int?>("courseID");

                    b.Property<double>("price");

                    b.HasKey("ID");

                    b.HasIndex("bookID");

                    b.HasIndex("courseID");

                    b.ToTable("EditListingViewModel");
                });

            modelBuilder.Entity("NYBE.Models.PendingBook", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorFName");

                    b.Property<string>("AuthorLName");

                    b.Property<string>("Description");

                    b.Property<string>("Edition");

                    b.Property<string>("ISBN");

                    b.Property<string>("Publisher");

                    b.Property<string>("Title");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("PendingBooks");
                });

            modelBuilder.Entity("NYBE.Models.PendingSchool", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Name");

                    b.Property<string>("State");

                    b.Property<string>("UserID");

                    b.HasKey("ID");

                    b.ToTable("PendingSchools");
                });

            modelBuilder.Entity("NYBE.Models.School", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Name");

                    b.Property<string>("State");

                    b.HasKey("ID");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("NYBE.Models.TransactionLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookID");

                    b.Property<string>("BuyerID");

                    b.Property<string>("Condition");

                    b.Property<string>("SellerID");

                    b.Property<double>("SoldPrice");

                    b.Property<int>("Status");

                    b.Property<DateTime>("TransDate");

                    b.Property<double>("TransRating");

                    b.HasKey("ID");

                    b.HasIndex("BookID");

                    b.HasIndex("BuyerID");

                    b.HasIndex("SellerID");

                    b.ToTable("TransactionLogs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NYBE.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NYBE.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NYBE.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NYBE.Models.ApplicationUser", b =>
                {
                    b.HasOne("NYBE.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NYBE.Models.BookListing", b =>
                {
                    b.HasOne("NYBE.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("ApplicationUserID");

                    b.HasOne("NYBE.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NYBE.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NYBE.Models.BookToCourse", b =>
                {
                    b.HasOne("NYBE.Models.Book", "Book")
                        .WithMany("BookToCourses")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NYBE.Models.Course", "Course")
                        .WithMany("BookToCourses")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NYBE.Models.Course", b =>
                {
                    b.HasOne("NYBE.Models.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NYBE.Models.DataModels.EditListingViewModel", b =>
                {
                    b.HasOne("NYBE.Models.Book", "book")
                        .WithMany()
                        .HasForeignKey("bookID");

                    b.HasOne("NYBE.Models.Course", "course")
                        .WithMany()
                        .HasForeignKey("courseID");
                });

            modelBuilder.Entity("NYBE.Models.TransactionLog", b =>
                {
                    b.HasOne("NYBE.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NYBE.Models.ApplicationUser", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerID");

                    b.HasOne("NYBE.Models.ApplicationUser", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerID");
                });
        }
    }
}
