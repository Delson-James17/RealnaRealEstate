using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Models;

namespace Real_Estate.Data
{
    public static class REDbInitializer
    {
        public static void RolesSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "fb63abec-98f5-448e-8f56-302fafd16df4",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Id = "5c965850-234a-4d90-9c24-024ebfac6f20",
                    Name = "Client",
                    NormalizedName = "CLIENT",
                },
                new IdentityRole
                {
                    Id = "51d0771e-de96-4882-a01e-8f0b9949e90c",
                    Name = "Owner",
                    NormalizedName = "OWNER",
                }
            );
        }

        public static void EstatePropertyCategorySeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyCategory>().HasData(
                new PropertyCategory
                {
                    Id = 1,
                    Name = "House",
                    Description = "A House.",
                },
                new PropertyCategory
                {
                    Id = 2,
                    Name = "Condominium",
                    Description = "A Condominium.",
                },
                new PropertyCategory
                {
                    Id = 3,
                    Name = "Commercial",
                    Description = "A Commercial.",
                }
            );
        }

        public static void SaleOrRentSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleorRentModel>().HasData(
                new SaleorRentModel
                {
                    Id = 1,
                    Name = "Sale"
                },
                new SaleorRentModel
                {
                    Id = 2,
                    Name = "Rent"
                }
            );
        }

        public static void UserSeed(this ModelBuilder modelBuilder)
        {
            string defaultPassword = "P@ssword123";

            var passwordHasher = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "f0fbf9f0-eb17-4c87-9c76-9de5451f74ae",
                    Name = "Admin",
                    Age = 23,
                    Address = "Laguna",
                    DOB = DateTime.Now,
                    UrlImages = "https://live.staticflickr.com/65535/52837724066_4d882431b9_w.jpg",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "admin@gmail.com".ToUpper(),
                    NormalizedEmail = "admin@gmail.com".ToUpper(),
                    Email = "admin@gmail.com",
                    PasswordHash = passwordHasher.HashPassword(null, defaultPassword)
                },
                new ApplicationUser
                {
                    Id = "b93130a7-a14b-46d0-b00d-f23536494dd2",
                    Name = "Client One",
                    Age = 23,
                    Address = "Laguna",
                    DOB = DateTime.Now,
                    UrlImages = "https://live.staticflickr.com/65535/52837907994_47a8b89ac4_n.jpg",
                    UserName = "client1@gmail.com",
                    NormalizedUserName = "client1@gmail.com".ToUpper(),
                    NormalizedEmail = "client1@gmail.com".ToUpper(),
                    Email = "client1@gmail.com",
                    PasswordHash = passwordHasher.HashPassword(null, defaultPassword)
                },
                new ApplicationUser
                {
                    Id = "e1e3dc24-4d24-4468-b2db-017de922c7a6",
                    Name = "Client Two",
                    Age = 23,
                    Address = "Laguna",
                    DOB = DateTime.Now,
                    UrlImages = "https://live.staticflickr.com/65535/52837908254_3c81ba99c7_n.jpg",
                    UserName = "client2@gmail.com",
                    NormalizedUserName = "client2@gmail.com".ToUpper(),
                    NormalizedEmail = "client2@gmail.com".ToUpper(),
                    Email = "client2@gmail.com",
                    PasswordHash = passwordHasher.HashPassword(null, defaultPassword)
                },
                new ApplicationUser
                {
                    Id = "62550723-3df6-4886-80c0-5ff90804ec07",
                    Name = "Owner one",
                    Age = 23,
                    Address = "Laguna",
                    DOB = DateTime.Now,
                    UrlImages = "https://live.staticflickr.com/65535/52837723986_d9afeb97e0_w.jpg",
                    UserName = "owner1@gmail.com",
                    Zoomlink = "https://us05web.zoom.us/j/82148537267?pwd=NjlYUWQzeFF6K1AxZEZRaklxbnF6QT09",
                    NormalizedUserName = "owner1@gmail.com".ToUpper(),
                    NormalizedEmail = "owner1@gmail.com".ToUpper(),
                    Email = "owne1r@gmail.com",
                    PasswordHash = passwordHasher.HashPassword(null, defaultPassword)
                },
                 new ApplicationUser
                 {
                     Id = "72550723-3df6-4886-80c0-5ff90804ec07",
                     Name = "Owner two",
                     Age = 23,
                     Address = "Laguna",
                     DOB = DateTime.Now,
                     UrlImages = "https://live.staticflickr.com/65535/52837723986_d9afeb97e0_w.jpg",
                     UserName = "owner2@gmail.com",
                     Zoomlink = "https://live.staticflickr.com/65535/52837152367_3f166e4330_n.jpg",
                     NormalizedUserName = "owner2@gmail.com".ToUpper(),
                     NormalizedEmail = "owner2@gmail.com".ToUpper(),
                     Email = "owner2@gmail.com",
                     PasswordHash = passwordHasher.HashPassword(null, defaultPassword)
                 }
           );
        }

        public static void EstatePropertySeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstateProperty>().HasData(
                new EstateProperty
                {
                    Id = 1,
                    Name = "GREGORIA",
                    Description = "Gregoria Model (2-Storey Single Attached)\r\n\r\nUnit Price: Php 3,173,851.00\r\n\r\nNo. of Bedroom: 3\r\n\r\nNo. of Bathroom: 2\r\n\r\nLot Area: 110 sqm\r\n\r\nFloor Area: 72 sqm\r\n\r\n\r\n\r\nHEROES' LANE\r\n\r\nA mix-used horizontal development with a total of 668 units which is eyed to be the 1st EDGE Certified Green Project in Cagayan Valley and North Luzon.\r\n\r\n\r\n\r\nAmenities\r\n\r\nCommunity Function Hall\r\nReligious Chapel\r\nSolar Powered Units and Facilities\r\nBasketball and Tennis Courts\r\nHeroes Park\r\nChildren's Playground\r\nCommercial Lane\r\nGardens and Open Spaces\r\n\r\n\r\nLocation\r\n\r\nGamu - Roxas Highway, District 2, Gamu, Isabela, PH\r\n\r\n\r\n\r\nHLURB LTS No. CR # 021 / LTS # 038 / 039 / 040\r\n\r\nYear Built: 2022\r\n\r\nTurnover Date: 2024 - 2025\r\n\r\nTotal No. of Model Units: 7",
                    Address = "GAMU - ROXAS HIGHWAY, DISTRICT II, GAMU",
                    UrlImages = "https://live.staticflickr.com/65535/52838168898_681cda2416_z.jpg",
                    Price = 3173851.00,
                    SaleOrRentModelId = 1,
                    PropertyCategoryId = 1,
                    ApplicationUserId = "62550723-3df6-4886-80c0-5ff90804ec07",
                    OwnerName = "Owner One"
                },
                new EstateProperty
                {
                    Id = 2,
                    Name = "Mantawi Residences ",
                    Description = "Mantawi Residences proudly calls Ouano Avenue, Mandaue City, Cebu its home, a prime spot where everything you need is within reach. With a multitude of infrastructure and development plans in the pipeline, this location is set to transform into a progressive district where you can reap the benefits of living at the center of everything. Take charge of your future with Mantawi Residences’ units equipped with upgraded smart home devices* so you can easily manage your home life without worry.\r\n\r\n",
                    Address = "OUANO AVE. COR. F.E. ZUELLIG AVE. SUBANGDAKU, MANDAUE",
                    UrlImages = "https://live.staticflickr.com/65535/52846569259_000336d9bd.jpg",
                    Price = 37200000,
                    SaleOrRentModelId = 2,
                    PropertyCategoryId = 2,
                    ApplicationUserId = "62550723-3df6-4886-80c0-5ff90804ec07",
                    OwnerName = "Owner One"
                },
                 new EstateProperty
                 {
                     Id = 3,
                     Name = "Lynville ",
                     Description = "Mantawi Residences proudly calls Ouano Avenue, Mandaue City, Cebu its home, a prime spot where everything you need is within reach. With a multitude of infrastructure and development plans in the pipeline, this location is set to transform into a progressive district where you can reap the benefits of living at the center of everything. Take charge of your future with Mantawi Residences’ units equipped with upgraded smart home devices* so you can easily manage your home life without worry.\r\n\r\n",
                     Address = "OUANO AVE. COR. F.E. ZUELLIG AVE. SUBANGDAKU, MANDAUE",
                     UrlImages = "https://live.staticflickr.com/65535/52838168903_0504483e6e.jpg",
                     Price = 37200000,
                     SaleOrRentModelId = 2,
                     PropertyCategoryId = 1,
                     ApplicationUserId = "62550723-3df6-4886-80c0-5ff90804ec07",
                     OwnerName = "Owner One"
                 },
                  new EstateProperty
                  {
                      Id = 4,
                      Name = "COMMERCIAL",
                      Description = "COMMERCIAL DESCRIPTION",
                      Address = "ADDRESS SAMPLE",
                      UrlImages = "https://live.staticflickr.com/65535/52838125720_0def4691c2.jpg",
                      Price = 2000.00,
                      SaleOrRentModelId = 2,
                      PropertyCategoryId = 3,
                      ApplicationUserId = "72550723-3df6-4886-80c0-5ff90804ec07",
                      OwnerName = "Owner Two"
                  },
                new EstateProperty
                {
                    Id = 5,
                    Name = "Pioneer Woodlands ",
                    Description = "Pioneer Woodlands Condo",
                    Address = "OUANO AVE. COR. F.E. ZUELLIG AVE. SUBANGDAKU, MANDAUE",
                    UrlImages = "https://live.staticflickr.com/65535/52838168918_e35af8184f_w.jpg",
                    Price = 37200000,
                    SaleOrRentModelId = 2,
                    PropertyCategoryId = 1,
                    ApplicationUserId = "72550723-3df6-4886-80c0-5ff90804ec07",
                    OwnerName = "Owner Two"
                },
                 new EstateProperty
                 {
                     Id = 6,
                     Name = "COMMERCIAL 2",
                     Description = "THIS IS COMMERCIAL 2",
                     Address = "OUANO AVE. COR. F.E. ZUELLIG AVE. SUBANGDAKU, MANDAUE",
                     UrlImages = "https://live.staticflickr.com/65535/52838125630_a96d82c343_w.jpg",
                     Price = 37200000,
                     SaleOrRentModelId = 1,
                     PropertyCategoryId = 3,
                     ApplicationUserId = "72550723-3df6-4886-80c0-5ff90804ec07",
                     OwnerName = "Owner Two"
                 }

            );
        }

        public static void UserRoleSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "fb63abec-98f5-448e-8f56-302fafd16df4",
                    UserId = "f0fbf9f0-eb17-4c87-9c76-9de5451f74ae"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "5c965850-234a-4d90-9c24-024ebfac6f20",
                    UserId = "b93130a7-a14b-46d0-b00d-f23536494dd2"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "5c965850-234a-4d90-9c24-024ebfac6f20",
                    UserId = "e1e3dc24-4d24-4468-b2db-017de922c7a6"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "51d0771e-de96-4882-a01e-8f0b9949e90c",
                    UserId = "62550723-3df6-4886-80c0-5ff90804ec07"
                },
                 new IdentityUserRole<string>
                 {
                     RoleId = "51d0771e-de96-4882-a01e-8f0b9949e90c",
                     UserId = "72550723-3df6-4886-80c0-5ff90804ec07"
                 }
            );
        }
    }
}


