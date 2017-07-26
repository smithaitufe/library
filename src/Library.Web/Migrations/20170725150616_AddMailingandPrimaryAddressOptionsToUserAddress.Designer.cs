﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Library.Repo;

namespace Library.Web.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    [Migration("20170725150616_AddMailingandPrimaryAddressOptionsToUserAddress")]
    partial class AddMailingandPrimaryAddressOptionsToUserAddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Library.Core.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("City");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Line")
                        .IsRequired();

                    b.Property<int?>("StateId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Library.Core.Models.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("ExpiresAt");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Introduction");

                    b.Property<DateTime>("ReleaseAt");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Library.Core.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Library.Core.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("CategoryId");

                    b.Property<int?>("CoverId");

                    b.Property<string>("Description");

                    b.Property<int>("GenreId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int?>("NoInSeries");

                    b.Property<int>("PublisherId");

                    b.Property<bool>("Series");

                    b.Property<string>("SubTitle")
                        .HasMaxLength(255);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CoverId");

                    b.HasIndex("GenreId");

                    b.HasIndex("PublisherId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Library.Core.Models.BookAuthor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("AuthorId");

                    b.Property<int>("BookId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId", "AuthorId")
                        .IsUnique();

                    b.ToTable("BookAuthors");
                });

            modelBuilder.Entity("Library.Core.Models.CheckOut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<bool>("Active");

                    b.Property<int?>("ApprovedDaysId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<long>("PatronId");

                    b.Property<int>("RequestedDaysId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int?>("VariantCopyId");

                    b.Property<int>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedDaysId");

                    b.HasIndex("PatronId");

                    b.HasIndex("RequestedDaysId");

                    b.HasIndex("VariantCopyId");

                    b.HasIndex("VariantId");

                    b.ToTable("CheckOuts");
                });

            modelBuilder.Entity("Library.Core.Models.CheckOutState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("CheckOutId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<long>("ModifiedByUserId");

                    b.Property<int>("StatusId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CheckOutId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("StatusId");

                    b.ToTable("CheckOutStates");
                });

            modelBuilder.Entity("Library.Core.Models.CheckOutStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("Left");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.Property<int>("Right");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("CheckOutStatuses");
                });

            modelBuilder.Entity("Library.Core.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("Library.Core.Models.ClubGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ClubId");

                    b.Property<int>("GenreId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("GenreId");

                    b.ToTable("ClubGenres");
                });

            modelBuilder.Entity("Library.Core.Models.ClubMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ClubId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("UserId");

                    b.ToTable("ClubMembers");
                });

            modelBuilder.Entity("Library.Core.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<long>("CommenterId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int?>("ParentId");

                    b.Property<int>("PostId");

                    b.Property<int>("StatusId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CommenterId");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostId");

                    b.HasIndex("StatusId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Library.Core.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Abbreviation")
                        .HasMaxLength(5);

                    b.Property<string>("Icon");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70);

                    b.Property<string>("TelephoneCode")
                        .HasMaxLength(10);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Library.Core.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("ContentType");

                    b.Property<string>("Extension");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Path");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Library.Core.Models.Inventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("LocationId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("VariantId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Library.Core.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Code");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Library.Core.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<long>("AuthorId");

                    b.Property<int>("CategoryId");

                    b.Property<int>("ClubId");

                    b.Property<bool>("Hidden");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<bool>("Locked");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("Views");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ClubId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Library.Core.Models.PriceOffer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("BookId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<decimal>("NewPrice");

                    b.Property<string>("PromotionalText");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("PriceOffers");
                });

            modelBuilder.Entity("Library.Core.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int?>("AddressId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("PhoneNumber");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("Library.Core.Models.Recall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("CheckOutId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<DateTime>("RecallDate");

                    b.Property<long>("RecalledByUserId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("CheckOutId");

                    b.HasIndex("RecalledByUserId");

                    b.ToTable("Recalls");
                });

            modelBuilder.Entity("Library.Core.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("BasisId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("NumberOfDaysId");

                    b.Property<long>("PatronId");

                    b.Property<long>("ReservedByUserId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("BasisId");

                    b.HasIndex("PatronId");

                    b.HasIndex("ReservedByUserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Library.Core.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("BookId");

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Stars");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Library.Core.Models.Role", b =>
                {
                    b.Property<long>("Id")
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
                        .HasName("RoleNameIndex");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Library.Core.Models.Shelf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Description");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("LocationId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Shelves");
                });

            modelBuilder.Entity("Library.Core.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Abbreviation")
                        .HasMaxLength(10);

                    b.Property<int>("CountryId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("Library.Core.Models.Term", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("TermSetId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.HasIndex("TermSetId");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("Library.Core.Models.TermSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedAt");

                    b.HasKey("Id");

                    b.ToTable("TermSets");
                });

            modelBuilder.Entity("Library.Core.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Approved");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<bool>("ChangePasswordFirstTimeLogin");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LibraryNo");

                    b.Property<bool>("Locked");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int?>("PhotoId");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("Suspended");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("PhotoId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Library.Core.Models.UserAddress", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<int>("AddressId");

                    b.Property<bool>("Mailing");

                    b.Property<bool>("Primary");

                    b.HasKey("UserId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("UserAddresses");
                });

            modelBuilder.Entity("Library.Core.Models.UserLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("LocationId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLocations");
                });

            modelBuilder.Entity("Library.Core.Models.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("BookId");

                    b.Property<string>("CallNumber");

                    b.Property<int>("CollectionModeId");

                    b.Property<int>("DaysAllowedId");

                    b.Property<string>("Edition")
                        .HasMaxLength(50);

                    b.Property<int>("FineId");

                    b.Property<int>("FormatId");

                    b.Property<int>("GrantId");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("LanguageId");

                    b.Property<int>("Pages");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<string>("Volume")
                        .HasMaxLength(50);

                    b.Property<int>("YearId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CollectionModeId");

                    b.HasIndex("DaysAllowedId");

                    b.HasIndex("FineId");

                    b.HasIndex("FormatId");

                    b.HasIndex("GrantId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("YearId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("Library.Core.Models.VariantCopy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("AvailabilityId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("LocationId");

                    b.Property<bool>("Out");

                    b.Property<string>("SerialNo")
                        .HasMaxLength(40);

                    b.Property<int?>("ShelfId");

                    b.Property<int>("SourceId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("VariantId");

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.HasIndex("AvailabilityId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ShelfId");

                    b.HasIndex("SourceId");

                    b.HasIndex("VariantId");

                    b.ToTable("VariantCopies");
                });

            modelBuilder.Entity("Library.Core.Models.VariantPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("ConditionId");

                    b.Property<DateTime>("InsertedAt");

                    b.Property<int>("PriceId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<int>("VariantId");

                    b.HasKey("Id");

                    b.HasIndex("ConditionId");

                    b.HasIndex("PriceId");

                    b.HasIndex("VariantId");

                    b.ToTable("VariantPrices");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.HasIndex("ProviderKey", "LoginProvider");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginProvider")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("UserId");

                    b.HasAlternateKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Library.Core.Models.Address", b =>
                {
                    b.HasOne("Library.Core.Models.Country", "Country")
                        .WithMany("Addresses")
                        .HasForeignKey("CountryId");

                    b.HasOne("Library.Core.Models.State", "State")
                        .WithMany("Addresses")
                        .HasForeignKey("StateId");
                });

            modelBuilder.Entity("Library.Core.Models.Announcement", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Book", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Category")
                        .WithMany("CategoryBooks")
                        .HasForeignKey("CategoryId");

                    b.HasOne("Library.Core.Models.Image", "Cover")
                        .WithMany()
                        .HasForeignKey("CoverId");

                    b.HasOne("Library.Core.Models.Term", "Genre")
                        .WithMany("GenreBooks")
                        .HasForeignKey("GenreId");

                    b.HasOne("Library.Core.Models.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.BookAuthor", b =>
                {
                    b.HasOne("Library.Core.Models.Author", "Author")
                        .WithMany("BooksLink")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.CheckOut", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "ApprovedDays")
                        .WithMany("CheckOutApprovedDays")
                        .HasForeignKey("ApprovedDaysId");

                    b.HasOne("Library.Core.Models.User", "Patron")
                        .WithMany()
                        .HasForeignKey("PatronId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "RequestedDays")
                        .WithMany("CheckOutRequestedDays")
                        .HasForeignKey("RequestedDaysId");

                    b.HasOne("Library.Core.Models.VariantCopy", "VariantCopy")
                        .WithMany("CheckOuts")
                        .HasForeignKey("VariantCopyId");

                    b.HasOne("Library.Core.Models.Variant", "Variant")
                        .WithMany("CheckOuts")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.CheckOutState", b =>
                {
                    b.HasOne("Library.Core.Models.CheckOut", "CheckOut")
                        .WithMany("CheckOutStates")
                        .HasForeignKey("CheckOutId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User", "ModifiedBy")
                        .WithMany("CheckOutStates")
                        .HasForeignKey("ModifiedByUserId");

                    b.HasOne("Library.Core.Models.CheckOutStatus", "Status")
                        .WithMany("CheckOutStates")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.CheckOutStatus", b =>
                {
                    b.HasOne("Library.Core.Models.CheckOutStatus", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Library.Core.Models.ClubGenre", b =>
                {
                    b.HasOne("Library.Core.Models.Club", "Club")
                        .WithMany("GenresLink")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.ClubMember", b =>
                {
                    b.HasOne("Library.Core.Models.Club", "Club")
                        .WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Comment", b =>
                {
                    b.HasOne("Library.Core.Models.User", "Commenter")
                        .WithMany("Comments")
                        .HasForeignKey("CommenterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Comment", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");

                    b.HasOne("Library.Core.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.HasOne("Library.Core.Models.Term", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Inventory", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Variant", "Variant")
                        .WithMany("Inventories")
                        .HasForeignKey("VariantId");
                });

            modelBuilder.Entity("Library.Core.Models.Post", b =>
                {
                    b.HasOne("Library.Core.Models.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Club", "Club")
                        .WithMany("Posts")
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.PriceOffer", b =>
                {
                    b.HasOne("Library.Core.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Publisher", b =>
                {
                    b.HasOne("Library.Core.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");
                });

            modelBuilder.Entity("Library.Core.Models.Recall", b =>
                {
                    b.HasOne("Library.Core.Models.CheckOut", "CheckOut")
                        .WithMany("RecalledBooks")
                        .HasForeignKey("CheckOutId");

                    b.HasOne("Library.Core.Models.User", "RecalledBy")
                        .WithMany("RecalledBooks")
                        .HasForeignKey("RecalledByUserId");
                });

            modelBuilder.Entity("Library.Core.Models.Reservation", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Basis")
                        .WithMany()
                        .HasForeignKey("BasisId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User", "Patron")
                        .WithMany("Reservations")
                        .HasForeignKey("PatronId");

                    b.HasOne("Library.Core.Models.User", "ReservedBy")
                        .WithMany("ReservationBookings")
                        .HasForeignKey("ReservedByUserId");
                });

            modelBuilder.Entity("Library.Core.Models.Review", b =>
                {
                    b.HasOne("Library.Core.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Shelf", b =>
                {
                    b.HasOne("Library.Core.Models.Location", "Location")
                        .WithMany("Shelves")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Term", b =>
                {
                    b.HasOne("Library.Core.Models.TermSet", "TermSet")
                        .WithMany()
                        .HasForeignKey("TermSetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.User", b =>
                {
                    b.HasOne("Library.Core.Models.Image", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId");
                });

            modelBuilder.Entity("Library.Core.Models.UserAddress", b =>
                {
                    b.HasOne("Library.Core.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User", "User")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.UserLocation", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User", "User")
                        .WithMany("LocationsLink")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Library.Core.Models.Variant", b =>
                {
                    b.HasOne("Library.Core.Models.Book", "Book")
                        .WithMany("Variants")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "CollectionMode")
                        .WithMany()
                        .HasForeignKey("CollectionModeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "DaysAllowed")
                        .WithMany("DaysAllowedVariants")
                        .HasForeignKey("DaysAllowedId");

                    b.HasOne("Library.Core.Models.Term", "Fine")
                        .WithMany("FineVariants")
                        .HasForeignKey("FineId");

                    b.HasOne("Library.Core.Models.Term", "Format")
                        .WithMany("FormatVariants")
                        .HasForeignKey("FormatId");

                    b.HasOne("Library.Core.Models.Term", "Grant")
                        .WithMany("GrantVariants")
                        .HasForeignKey("GrantId");

                    b.HasOne("Library.Core.Models.Term", "Language")
                        .WithMany("LanguageVariants")
                        .HasForeignKey("LanguageId");

                    b.HasOne("Library.Core.Models.Term", "Year")
                        .WithMany("YearVariants")
                        .HasForeignKey("YearId");
                });

            modelBuilder.Entity("Library.Core.Models.VariantCopy", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Availability")
                        .WithMany("AvailabilityVariantCopies")
                        .HasForeignKey("AvailabilityId");

                    b.HasOne("Library.Core.Models.Location", "Location")
                        .WithMany("VariantCopies")
                        .HasForeignKey("LocationId");

                    b.HasOne("Library.Core.Models.Shelf", "Shelf")
                        .WithMany()
                        .HasForeignKey("ShelfId");

                    b.HasOne("Library.Core.Models.Term", "Source")
                        .WithMany("SourceVariantCopies")
                        .HasForeignKey("SourceId");

                    b.HasOne("Library.Core.Models.Variant", "Variant")
                        .WithMany("VariantCopies")
                        .HasForeignKey("VariantId");
                });

            modelBuilder.Entity("Library.Core.Models.VariantPrice", b =>
                {
                    b.HasOne("Library.Core.Models.Term", "Condition")
                        .WithMany()
                        .HasForeignKey("ConditionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.Term", "Price")
                        .WithMany("VariantPrices")
                        .HasForeignKey("PriceId");

                    b.HasOne("Library.Core.Models.Variant", "Variant")
                        .WithMany("VariantPrices")
                        .HasForeignKey("VariantId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("Library.Core.Models.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("Library.Core.Models.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("Library.Core.Models.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<long>", b =>
                {
                    b.HasOne("Library.Core.Models.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Library.Core.Models.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}