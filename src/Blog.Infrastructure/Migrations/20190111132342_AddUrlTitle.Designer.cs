﻿// <auto-generated />
using System;
using Blog.Domain;
using Blog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.Migrations
{
   [DbContext(typeof(BlogContext))]
   [Migration("20190111132342_AddUrlTitle")]
   partial class AddUrlTitle
   {
      protected override void BuildTargetModel(ModelBuilder modelBuilder)
      {
#pragma warning disable 612, 618
         modelBuilder
             .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
             .HasAnnotation("Relational:MaxIdentifierLength", 128)
             .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

         modelBuilder.Entity("Blog.Model.Post", b =>
             {
                b.Property<int>("Id")
                       .ValueGeneratedOnAdd()
                       .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Content")
                       .IsRequired();

                b.Property<int>("Language")
                       .ValueGeneratedOnAdd()
                       .HasDefaultValue(1);

                b.Property<DateTime>("PublishDate");

                b.Property<bool>("Show");

                b.Property<string>("Summary")
                       .IsRequired();

                b.Property<string>("Tags")
                       .IsRequired()
                       .ValueGeneratedOnAdd()
                       .HasDefaultValue("");

                b.Property<string>("Title")
                       .IsRequired()
                       .HasMaxLength(150);

                b.Property<string>("UrlTitle")
                       .IsRequired()
                       .ValueGeneratedOnAdd()
                       .HasMaxLength(200)
                       .HasDefaultValue("[NOT SET]");

                b.HasKey("Id");

                b.HasAlternateKey("Title");

                b.ToTable("Posts");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
             {
                b.Property<string>("Id")
                       .ValueGeneratedOnAdd();

                b.Property<string>("ConcurrencyStamp")
                       .IsConcurrencyToken();

                b.Property<string>("Name")
                       .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                       .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                       .IsUnique()
                       .HasName("RoleNameIndex")
                       .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
             {
                b.Property<int>("Id")
                       .ValueGeneratedOnAdd()
                       .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("RoleId")
                       .IsRequired();

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
             {
                b.Property<string>("Id")
                       .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                       .IsConcurrencyToken();

                b.Property<string>("Email")
                       .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

                b.Property<string>("NormalizedEmail")
                       .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                       .HasMaxLength(256);

                b.Property<string>("PasswordHash");

                b.Property<string>("PhoneNumber");

                b.Property<bool>("PhoneNumberConfirmed");

                b.Property<string>("SecurityStamp");

                b.Property<bool>("TwoFactorEnabled");

                b.Property<string>("UserName")
                       .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                       .HasName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                       .IsUnique()
                       .HasName("UserNameIndex")
                       .HasFilter("[NormalizedUserName] IS NOT NULL");

                b.ToTable("AspNetUsers");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
             {
                b.Property<int>("Id")
                       .ValueGeneratedOnAdd()
                       .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("UserId")
                       .IsRequired();

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
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

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
             {
                b.Property<string>("UserId");

                b.Property<string>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
             {
                b.Property<string>("UserId");

                b.Property<string>("LoginProvider");

                b.Property<string>("Name");

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
             {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                       .WithMany()
                       .HasForeignKey("RoleId")
                       .OnDelete(DeleteBehavior.Cascade);
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
             {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                       .WithMany()
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.Cascade);
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
             {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                       .WithMany()
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.Cascade);
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
             {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                       .WithMany()
                       .HasForeignKey("RoleId")
                       .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                       .WithMany()
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.Cascade);
             });

         modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
             {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                       .WithMany()
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.Cascade);
             });
#pragma warning restore 612, 618
      }
   }
}
