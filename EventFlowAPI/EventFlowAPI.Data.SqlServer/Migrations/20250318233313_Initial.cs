using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventFlowAPI.Data.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Price = table.Column<decimal>(type: "NUMERIC(6,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventPassType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    ValidityPeriodInMonths = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    Price = table.Column<decimal>(type: "NUMERIC(6,2)", nullable: false),
                    RenewalDiscountPercentage = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPassType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FestivalDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LongDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FestivalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HallType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    HallTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaPatron",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MediaPatronGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaPatron", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewsGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnerGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeatType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false),
                    AddtionalPaymentPercentage = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    SeatColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sponsor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SponsorGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketJPG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketJPG", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketPDF",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPDF", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSoftUpdated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HouseNumber = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: true),
                    FlatNumber = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserData_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FestivalGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Festival_FestivalDetails_Id",
                        column: x => x.Id,
                        principalTable: "FestivalDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefaultId = table.Column<int>(type: "int", nullable: true),
                    HallNr = table.Column<int>(type: "int", nullable: false),
                    RentalPricePerHour = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    IsCopy = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Floor = table.Column<decimal>(type: "NUMERIC(1,0)", nullable: false),
                    HallViewFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hall_HallType_HallTypeId",
                        column: x => x.HallTypeId,
                        principalTable: "HallType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallType_Equipment",
                columns: table => new
                {
                    HallTypeId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallType_Equipment", x => new { x.EquipmentId, x.HallTypeId });
                    table.ForeignKey(
                        name: "FK_HallType_Equipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HallType_Equipment_HallType_HallTypeId",
                        column: x => x.HallTypeId,
                        principalTable: "HallType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventPass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventPassGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "NUMERIC(6,2)", nullable: false),
                    TotalDiscountPercentage = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    EventPassJPGName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventPassPDFName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PassTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventPass_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPass_EventPassType_PassTypeId",
                        column: x => x.PassTypeId,
                        principalTable: "EventPassType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventPass_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_MediaPatron",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    MediaPatronId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_MediaPatron", x => new { x.FestivalId, x.MediaPatronId });
                    table.ForeignKey(
                        name: "FK_Festival_MediaPatron_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_MediaPatron_MediaPatron_MediaPatronId",
                        column: x => x.MediaPatronId,
                        principalTable: "MediaPatron",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_Organizer",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    OrganizerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Organizer", x => new { x.FestivalId, x.OrganizerId });
                    table.ForeignKey(
                        name: "FK_Festival_Organizer_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Organizer_Organizer_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Organizer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_Sponsor",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    SponsorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Sponsor", x => new { x.FestivalId, x.SponsorId });
                    table.ForeignKey(
                        name: "FK_Festival_Sponsor_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Sponsor_Sponsor_SponsorId",
                        column: x => x.SponsorId,
                        principalTable: "Sponsor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AddDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Event_EventCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "EventCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_EventDetails_Id",
                        column: x => x.Id,
                        principalTable: "EventDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TotalLength = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    TotalWidth = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: false),
                    TotalArea = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    StageLength = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: true),
                    StageWidth = table.Column<decimal>(type: "NUMERIC(4,2)", nullable: true),
                    MaxNumberOfSeatsRows = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    MaxNumberOfSeatsColumns = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    NumberOfSeats = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: false),
                    MaxNumberOfSeats = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallDetails_Hall_Id",
                        column: x => x.Id,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallRent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HallRentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    RentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "NUMERIC(7,2)", nullable: false),
                    HallRentPDFName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallRent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallRent_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HallRent_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HallRent_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeatNr = table.Column<decimal>(type: "NUMERIC(3,0)", nullable: false),
                    Row = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    GridRow = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    Column = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    GridColumn = table.Column<decimal>(type: "NUMERIC(2,0)", nullable: false),
                    SeatTypeId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seat_Hall_HallId",
                        column: x => x.HallId,
                        principalTable: "Hall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seat_SeatType_SeatTypeId",
                        column: x => x.SeatTypeId,
                        principalTable: "SeatType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Festival_Event",
                columns: table => new
                {
                    FestivalId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Festival_Event", x => new { x.FestivalId, x.EventId });
                    table.ForeignKey(
                        name: "FK_Festival_Event_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Festival_Event_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TicketTypeId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    FestivalId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ticket_Festival_FestivalId",
                        column: x => x.FestivalId,
                        principalTable: "Festival",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_TicketType_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HallRent_AdditionalServices",
                columns: table => new
                {
                    HallRentId = table.Column<int>(type: "int", nullable: false),
                    AdditionalServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallRent_AdditionalServices", x => new { x.HallRentId, x.AdditionalServicesId });
                    table.ForeignKey(
                        name: "FK_HallRent_AdditionalServices_AdditionalServices_AdditionalServicesId",
                        column: x => x.AdditionalServicesId,
                        principalTable: "AdditionalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HallRent_AdditionalServices_HallRent_HallRentId",
                        column: x => x.HallRentId,
                        principalTable: "HallRent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFestivalReservation = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAddtionalPaymentPercentage = table.Column<decimal>(type: "NUMERIC(5,2)", nullable: false),
                    TotalAdditionalPaymentAmount = table.Column<decimal>(type: "NUMERIC(6,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "NUMERIC(7,2)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "NUMERIC(7,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    EventPassId = table.Column<int>(type: "int", nullable: true),
                    TicketPDFId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_EventPass_EventPassId",
                        column: x => x.EventPassId,
                        principalTable: "EventPass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_TicketPDF_TicketPDFId",
                        column: x => x.TicketPDFId,
                        principalTable: "TicketPDF",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation_Seat",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation_Seat", x => new { x.ReservationId, x.SeatId });
                    table.ForeignKey(
                        name: "FK_Reservation_Seat_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservation_Seat_Seat_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation_TicketJPG",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    TicketJPGId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation_TicketJPG", x => new { x.ReservationId, x.TicketJPGId });
                    table.ForeignKey(
                        name: "FK_Reservation_TicketJPG_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_TicketJPG_TicketJPG_TicketJPGId",
                        column: x => x.TicketJPGId,
                        principalTable: "TicketJPG",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdditionalServices",
                columns: new[] { "Id", "DeleteDate", "Description", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "Price", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, null, false, false, false, "DJ", 400.00m, null },
                    { 2, null, null, false, false, false, "Obsługa oświetlenia", 340.00m, null },
                    { 3, null, null, false, false, false, "Obsługa nagłośnienia", 250.00m, null },
                    { 4, null, null, false, false, false, "Fotograf", 200.00m, null },
                    { 5, null, null, false, false, false, "Promocja wydarzenia", 140.00m, null },
                    { 6, null, null, false, false, false, "Kamerzysta", 500.00m, null },
                    { 7, null, null, false, false, false, "Catering", 1000.00m, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "Admin role", "Admin", "ADMIN" },
                    { "2", null, "User role", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "IsVerified", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhotoName", "Provider", "RegisteredDate", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "95ca56b0-be1b-4b9a-b9cb-77b705ab1bbc", new DateTime(2000, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, true, false, null, "Admin", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAIAAYagAAAAEK4/hlBoE1k2Sdq8n2DYNHysLOR3Fg/7anvL/W7qoX0GxzJBNLQqG79m14mPJ6iwrw==", null, false, "admin.jpg", "APP", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "b0fcb126-8927-42fb-8d13-13c4f1681351", "Admin", false, "admin@gmail.com" },
                    { "2", 0, "c79bc252-1135-418f-a752-59ea3ced3ad1", new DateTime(1985, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mateusz.strapczuk2@gmail.com", true, true, false, null, "Mateusz2", "MATEUSZ.STRAPCZUK2@GMAIL.COM", "MATEUSZ.STRAPCZUK2@GMAIL.COM", "AQAAAAIAAYagAAAAEG6rZhspfbU8XJjYA5dsJzuNC6oO71cVoB6GJ3pm3voqJFOM7pvY0OsDo5PZDIuzxA==", null, false, "user2.jpg", "APP", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "28db39ee-4875-4a8c-b689-02eabddb96aa", "Strapczuk2", false, "mateusz.strapczuk2@gmail.com" },
                    { "3", 0, "fdf14307-4b05-422d-b5ae-a8571afd609d", new DateTime(1979, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "mateusz.strapczuk3@gmail.com", true, true, false, null, "Mateusz3", "MATEUSZ.STRAPCZUK3@GMAIL.COM", "MATEUSZ.STRAPCZUK3@GMAIL.COM", "AQAAAAIAAYagAAAAEAlgQ4Qs/GFmn3hHt58p/CNdbRcnd2+tGvTWqNDxznjanmwir/VeibWWI3B9jZhf8g==", null, false, "user3.jpg", "APP", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "959425f8-a938-433a-923e-d5d01925122c", "Strapczuk3", false, "mateusz.strapczuk3@gmail.com" },
                    { "4", 0, "cbbf0722-3f05-411d-b87f-a05fe3726e3c", new DateTime(1979, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "mateusz.strapczuk4@gmail.com", true, true, false, null, "Mateusz4", "MATEUSZ.STRAPCZUK4@GMAIL.COM", "MATEUSZ.STRAPCZUK4@GMAIL.COM", "AQAAAAIAAYagAAAAEErLVney/2f4Wm11sn7nK8cNngA8P3qvHSertKvrBHNSE/000BYDaS+/F5tYAhMIFQ==", null, false, "user4.jpg", "APP", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "9bd8c1e8-5875-4e50-876d-3a69c2fdc470", "Strapczuk4", false, "mateusz.strapczuk4@gmail.com" },
                    { "5", 0, "1a9d5c0a-7f87-4f87-bbfe-ea099479ac03", new DateTime(1979, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "mateusz.strapczuk1@gmail.com", true, true, false, null, "Mateusz", "MATEUSZ.STRAPCZUK1@GMAIL.COM", "MATEUSZ.STRAPCZUK1@GMAIL.COM", "AQAAAAIAAYagAAAAEFjlpJmB1mkCX/au5X5r7DmJLIjfboZ0uKo0vyl9ZfqXDpEWpvdISbzfLhblOMN/uQ==", null, false, "user5.jpg", "APP", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "f1e975b0-0b98-4b68-8f3b-fbcb7a0c22f4", "Strapczuk", false, "mateusz.strapczuk1@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "Description", "IsUpdated", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "Nowoczesny projektor, idealny do prezentacji multimedialnych, z wysoką rozdzielczością i możliwością podłączenia różnych źródeł sygnału.", false, "Projektor multimedialny", null },
                    { 2, "Wysokiej klasy oświetlenie, umożliwiające regulację jasności i kolorów, idealne do sal konferencyjnych i eventów.", false, "Oświetlenie", null },
                    { 3, "Głośniki przeznaczone do odtwarzania filmów, zapewniające doskonałą jakość dźwięku przestrzennego w dużych przestrzeniach.", false, "Nagłośnienie kinowe", null },
                    { 4, "Głośniki przeznaczone do koncertów, oferujące mocny dźwięk i wyrazistość w każdych warunkach, z możliwością regulacji.", false, "Nagłośnienie koncertowe", null },
                    { 5, "Profesjonalne mikrofony bezprzewodowe, zapewniające czysty dźwięk i niezawodność w ruchu, idealne do konferencji i wystąpień.", false, "Mikrofony bezprzewodowe", null },
                    { 6, "Tablica interaktywna do rysowania i prezentowania treści w sposób dynamiczny.", false, "Tablica interaktywna", null },
                    { 7, "System klimatyzacji zapewniający komfortową temperaturę w sali podczas wydarzeń.", false, "Klimatyzacja", null },
                    { 8, "Duży stół konferencyjny, idealny do spotkań biznesowych i prezentacji.", false, "Stół konferencyjny", null },
                    { 9, "Kurtyna teatralna służąca do oddzielania sceny od widowni lub zmiany scenerii w trakcie przedstawienia.", false, "Kurtyna", null },
                    { 10, "Tablice służące do umieszczania informacji o eksponatach.", false, "Tablice informacyjne", null },
                    { 11, "Stojaki umożliwiają prezentowanie dzieł sztuki, takich jak obrazy, grafiki czy rzeźby.", false, "Stojaki", null }
                });

            migrationBuilder.InsertData(
                table: "EventCategory",
                columns: new[] { "Id", "Color", "DeleteDate", "Icon", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, "#82CAFC", null, "fa-solid fa-music", false, false, false, "Koncert", null },
                    { 2, "#6BD49B", null, "fa-solid fa-film", false, false, false, "Film", null },
                    { 3, "#C33EB1", null, "fa-solid fa-masks-theater", false, false, false, "Spektakl", null },
                    { 4, "#FC5353", null, "fa-solid fa-landmark", false, false, false, "Wystawa", null }
                });

            migrationBuilder.InsertData(
                table: "EventDetails",
                columns: new[] { "Id", "LongDescription" },
                values: new object[,]
                {
                    { 1, "Koncert Mystic Waves to wyjątkowe wydarzenie muzyczne, które przeniesie Cię w świat atmosferycznych dźwięków. Zespół wystąpi z premierowym materiałem, łączącym elementy muzyki elektronicznej i ambientowej. Koncert odbędzie się w klimatycznej sali, z wykorzystaniem nowoczesnej technologii dźwiękowej i wizualnej, co zapewni niezapomniane przeżycia. Muzycy znani z licznych występów na scenach festiwalowych zaprezentują swoją najnowszą produkcję, która z pewnością oczaruje fanów muzyki eksperymentalnej." },
                    { 2, "Spektakl 'Cień Przeszłość' to wciągająca produkcja teatralna, która porusza tematykę przemijania i refleksji nad przeszłością. Historia opowiada o dwóch postaciach, które spotykają się po latach, by skonfrontować swoje wspomnienia i tajemnice. Reżyseria, mistrzowskie aktorstwo oraz emocjonalna głębia tej sztuki sprawiają, że jest to wydarzenie, które zostanie z widzami na długo. Spektakl jest pełen symboliki i zaskakujących zwrotów akcji, które skłonią do przemyśleń o upływającym czasie." },
                    { 3, "Film 'Królestwo planety małp' to spektakularna produkcja science-fiction, która kontynuuje historię walczących o przetrwanie małp. Akcja filmu toczy się w post-apokaliptycznym świecie, w którym małpy zaczynają dominować nad ludźmi. W filmie zobaczymy dynamiczne sceny bitewne, dramatyczne decyzje bohaterów i wciągającą fabułę, która porusza kwestie moralności i przyszłości ludzkości. Wysoka jakość efektów specjalnych oraz rewelacyjna gra aktorska sprawiają, że jest to jeden z najważniejszych filmów tego roku." },
                    { 4, "Wystawa 'Nowe inspiracje' to wyjątkowa ekspozycja sztuki współczesnej, która gromadzi prace młodych artystów z całego świata. Wystawa skupia się na nowych nurtach artystycznych, które łączą tradycję z nowoczesnością. Można tu zobaczyć prace w różnych mediach, od malarstwa po instalacje interaktywne. Artyści eksperymentują z formą i materiałami, zapraszając widzów do interakcji z dziełami. Jest to przestrzeń, która otwiera umysł na nowe pomysły i inspiracje, zachęcając do refleksji nad współczesnym światem sztuki." },
                    { 5, "Koncert 'New Era' to wyjątkowe wydarzenie muzyczne, które łączy najnowsze trendy w muzyce elektronicznej z klasycznymi brzmieniami. Zespół zaprezentuje utwory z debiutanckiego albumu, który zdobył uznanie wśród krytyków muzycznych. Występ będzie pełen energetycznych melodii i przejmujących dźwięków, które z pewnością poruszą serca słuchaczy. Nowoczesne technologie audio-wizualne, wykorzystywane podczas koncertu, sprawią, że każdy utwór nabierze nowego wymiaru. Warto być częścią tego muzycznego doświadczenia!" },
                    { 6, "Film 'Gladiator' to epicka opowieść o wojowniku, który staje do walki nie tylko o swoje życie, ale także o wolność i sprawiedliwość. Akcja rozgrywa się w starożytnym Rzymie, gdzie główny bohater, Maximus, zostaje zdradzony przez cesarza, a jego rodzina zostaje zamordowana. Maximus trafia do areny, gdzie walczy o przetrwanie i zemstę. Film pełen jest emocjonujących walk, dramatycznych zwrotów akcji i wciągającej fabuły, która porusza tematy honoru, lojalności i zemsty." },
                    { 7, "Wystawa 'Nowa sztuka' to prezentacja młodych artystów, którzy swoją twórczość ukierunkowali na przekroczenie granic tradycyjnej sztuki. Na wystawie można zobaczyć prace w różnorodnych technikach: od klasycznych obrazów po awangardowe instalacje. Artyści bawią się konwencjami, angażując widza w interaktywną formę prezentacji. Wystawa jest zaproszeniem do refleksji nad tym, czym współczesna sztuka może być i jakie emocje potrafi wywołać. To wydarzenie, które pokazuje, jak sztuka może zmieniać nasze postrzeganie świata." },
                    { 8, "Cień Nad Miastem to mroczny thriller teatralny, który przenosi widza w świat pełen tajemniczych wydarzeń, które zaczynają wstrząsać miastem po zmroku. Główna fabuła opowiada historię, w której pozornie spokojne miasto staje się areną niebezpiecznych wydarzeń, których rozwiązanie wciąga nie tylko bohaterów, ale również publiczność. W spektaklu dominują napięcie i atmosfera niepokoju, które potęgują świetnie dobrane efekty wizualne oraz świetne aktorstwo. Każda scena jest przemyślana, a dramaturgia składa się z zagadek i zwrotów akcji, które trzymają widza w ciągłym napięciu. To przedstawienie nie tylko o mrocznych wydarzeniach, ale także o ludzkich emocjach, które rodzą się w sytuacjach kryzysowych, zmieniając życie bohaterów na zawsze. W trakcie spektaklu widzowie będą mieli okazję odkryć tajemnice miasta i skonfrontować się z pytaniem, jak cienka jest granica między rzeczywistością a tym, co wytworzyła wyobraźnia." },
                    { 9, "Jazzowe Brzmienia to wyjątkowy koncert, który przenosi uczestników w świat żywego jazzu. Podczas wydarzenia wystąpią znani artyści jazzowi, którzy zabiorą widzów w podróż po różnorodnych stylach tego gatunku. Atmosfera pełna emocji, improwizacji i magicznych dźwięków sprawia, że każda chwila staje się niezapomnianym doświadczeniem. Koncert jest idealną okazją, by poczuć autentyczną magię jazzu w wyjątkowym wykonaniu na żywo, a publiczność wciągnie się w niepowtarzalny klimat muzycznej ekspresji." },
                    { 10, "Interstellar to epicka podróż w głąb kosmosu, która bada granice ludzkiej wytrzymałości, miłości i poświęcenia. Grupa astronautów, wyruszając na misję w poszukiwaniu nowego domu dla ludzkości, zmierzy się z niewyobrażalnymi wyzwaniami. Film porusza tematykę czasu, przestrzeni i ofiary, ukazując niesamowite krajobrazy kosmiczne oraz ludzką determinację w walce o przetrwanie. Każdy moment to prawdziwa uczta dla zmysłów, a historia miłości i poświęcenia łączy się z naukową fascynacją, tworząc niezapomniane doświadczenie filmowe." },
                    { 11, "Spektakl 'Zatrzymać Przeznaczenie' to głęboka opowieść o postaci, która staje przed obliczem nieuchronnego przeznaczenia. Bohaterowie zmagają się z wyborem, który może zmienić bieg wydarzeń, a ich działania prowadzą do nieodwracalnych konsekwencji. Spektakl skłania do refleksji nad wolną wolą, nieuchronnością losu i odpowiedzialnością za własne wybory. Z pomocą wciągającej dramaturgii oraz poruszających dialogów, spektakl ukazuje, jak delikatna jest granica między tym, co możemy kontrolować, a tym, co jest zapisane w naszych losach." },
                    { 12, "Wystawa 'Nowe Perspektywy' zaprasza do odkrywania sztuki współczesnej z zupełnie nowych punktów widzenia. Artyści wykorzystują różnorodne techniki, od tradycyjnych obrazów po innowacyjne instalacje, które angażują widza w interaktywną formę prezentacji. Każda praca na wystawie to zaproszenie do zastanowienia się nad tym, jak sztuka może zmieniać nasze postrzeganie rzeczywistości. To także próba przekroczenia granic tradycyjnych konwencji, poszukiwania nowych środków wyrazu i głębokiej refleksji nad naturą współczesnej sztuki." }
                });

            migrationBuilder.InsertData(
                table: "EventPassType",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "Price", "RenewalDiscountPercentage", "UpdateDate", "ValidityPeriodInMonths" },
                values: new object[,]
                {
                    { 1, null, false, false, false, "Karnet miesięczny", 89.99m, 5m, null, 1m },
                    { 2, null, false, false, false, "Karnet kwartalny", 235.99m, 10m, null, 3m },
                    { 3, null, false, false, false, "Karnet półroczny", 499.99m, 15m, null, 6m },
                    { 4, null, false, false, false, "Karnet roczny", 999.99m, 20m, null, 12m }
                });

            migrationBuilder.InsertData(
                table: "FAQ",
                columns: new[] { "Id", "Answer", "Question" },
                values: new object[,]
                {
                    { 1, "Bilety możesz kupić przechodząc do zakładki Wydarzenia. Następnie wybierz interesujące cię wydarzenie i kliknij przycisk Kup bilet. Wyświetli okno dialogowe w którym możliwy jest wybór miejsca i typu biletu. Wybierz miejsca i typ biletu, a następnie kliknij ponownie Kup bilet. Zostaniesz przekierowany do bramki płatniczej za pomocą której dokonasz opłaty za bilet. Po dokonaniu opłaty bilet zostanie przesłany na adres e-mail oraz będzie możliwy do pobrania w zakładce Profil > Moje rezerwacje.", "W jaki sposób mogę kupić bilet na wydarzenie ?" },
                    { 2, "Aby wynająć salę przejdź do zakładki Wynajem. Następnie wybierz interesującą cię sale i kliknij przycisk Wynajmij. Pojawi się okno dialogowe z kalendarzem możliwych do wybrania dat rezerwacji. Wybierz datę rozpoczęcia i zakończenia rezerwacji oraz ewentualne dodatkowe usługi po czym kliknij przycisk Wynajmij. Zostaniesz przekierowany do bramki płątniczej za pomocą której dokonasz opłaty za wynajem. Po dokonaniu opłaty potwierdzenie wynajmu zostanie wysłane na adres e-mail oraz będzie możliwe do pobrania w zakładce Profil > Moje wynajmy.", "Jak mogę wynająć salę ?" },
                    { 3, "Karnet możesz przedłużyć w dowolnym momencie przed upływem jego daty ważności. Przy przedłużeniu karnetu zawsze otrzymasz zniżkę w zależności od aktualnie posiadanego karnetu. Przykładowo jeśli posiadasz karnet roczny otrzymasz 20% zniżki na każdy typ karnetu.", "Co ile czasu muszę przedłużać karnet ?" },
                    { 4, "Aby zwrócić zakupiony bilet konieczne jest anulowanie rezerwacji. Możesz tego dokonać przechodząc do zakładki Profil > Moje rezerwacje. Następnie z listy wybierz rezerwację którą chcesz anulowac i kliknij przycisk Anuluj. Pojawi się okno dialogowe z zapytaniem czy naewno chcesz wykonać daną operację. Po potwierdzeniu wykonania operacji otrzymasz na maila informację o anulowaniu biletu, a zwrot kosztów otrzymasz w ciągu 7 dni robocznych. Po anulowaniu rezerwacji posiadany bilet staje się nieważny.s", "Jak zwrócić zakupiony bilet ?" }
                });

            migrationBuilder.InsertData(
                table: "FestivalDetails",
                columns: new[] { "Id", "LongDescription" },
                values: new object[,]
                {
                    { 1, "Festiwal Muzyki Hip-Hop to coroczne wydarzenie dedykowane nowoczesnym brzmieniom i eksperymentalnym kompozycjom. Na scenie pojawiają się zarówno uznani artyści, jak i młode talenty, prezentując innowacyjne podejście do muzyki. W programie znajdują się koncerty, warsztaty, spotkania z twórcami oraz panele dyskusyjne dotyczące przyszłości muzyki. Festiwal przyciąga miłośników dźwięków elektronicznych, jazzowych improwizacji, minimalistycznych kompozycji i nowych form ekspresji dźwiękowej. To wyjątkowa okazja, by odkryć najnowsze trendy muzyczne i poszerzyć swoje horyzonty artystyczne." },
                    { 2, "Festiwal Filmowy to wyjątkowe święto kina, które gromadzi pasjonatów filmów z różnych zakątków świata. W repertuarze znajdują się zarówno filmy fabularne, dokumentalne, jak i krótkometrażowe, reprezentujące różne nurty i stylistyki. Organizowane są spotkania z reżyserami, aktorami oraz warsztaty dla młodych filmowców. Każda edycja festiwalu poświęcona jest określonemu tematowi przewodniemu, który inspiruje twórców do dyskusji nad istotnymi kwestiami społecznymi, kulturowymi i artystycznymi. To doskonała okazja, by zobaczyć premiery, odkryć nowe talenty i zanurzyć się w magiczny świat kinematografii." },
                    { 3, "Festiwal Sztuki to wydarzenie celebrujące kreatywność i nieograniczoną ekspresję artystyczną. W ramach festiwalu prezentowane są prace artystów, którzy eksperymentują z formą, kolorem i strukturą, tworząc unikalne dzieła sztuki nowoczesnej. Oprócz wystaw odbywają się warsztaty, podczas których uczestnicy mogą spróbować swoich sił w malarstwie, rzeźbie czy instalacjach multimedialnych. Festiwal to także przestrzeń do rozmów o współczesnych trendach w sztuce, spotkania z artystami oraz wykłady ekspertów. To niepowtarzalna okazja, by odkryć nowe inspiracje i spojrzeć na sztukę z innej perspektywy." }
                });

            migrationBuilder.InsertData(
                table: "HallType",
                columns: new[] { "Id", "DeleteDate", "Description", "HallTypeGuid", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, "Nowoczesna sala kinowa, wyposażona w wysokiej jakości projektor oraz system nagłośnienia Dolby Digital. Idealna do projekcji filmów w różnych formatach, zapewniająca doskonałe wrażenia wizualne i dźwiękowe. Pomieszczenie jest przystosowane do projekcji 2D i 3D, z przestronnymi fotelami zapewniającymi komfort podczas długotrwałego seansu.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala filmowa", "filmowa.jpg", null },
                    { 2, null, "Sala koncertowa o najwyższej jakości akustyce, przeznaczona do organizacji koncertów, recitali i występów muzycznych. Wyposażona w profesjonalny system nagłośnienia i adaptację akustyczną, która zapewnia doskonałe warunki dźwiękowe dla artystów oraz publiczności. Przestronny design, komfortowe siedzenia oraz nowoczesne oświetlenie sprawiają, że jest to idealne miejsce do wydarzeń muzycznych.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala koncertowa", "koncertowa.jpg", null },
                    { 3, null, "Sala teatralna o wysokiej jakości akustyce i estetyce, przystosowana do organizacji spektakli teatralnych, przedstawień, recitalów oraz wydarzeń artystycznych. Wyposażona w profesjonalne oświetlenie sceniczne, nagłośnienie oraz przestronną scenę, która zapewnia doskonałą widoczność i akustykę. To przestrzeń, która sprzyja artystycznemu wyrazowi i tworzeniu niezapomnianych doznań dla widzów.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala teatralna", "teatralna.jpg", null },
                    { 4, null, "Przestronna sala wystawowa dedykowana organizacji wystaw, targów i ekspozycji artystycznych. Zapewnia dużą powierzchnię do prezentacji dzieł sztuki, rzemiosła i innych ekspozycji. Zmodernizowane oświetlenie i możliwość aranżacji przestrzeni pozwalają na profesjonalne wyeksponowanie dzieł. Jest to przestrzeń elastyczna, doskonała do organizacji wydarzeń kulturalnych i wystaw artystycznych.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala wystawowa", "wystawowa.jpg", null },
                    { 5, null, "Sala ogólna to wszechstronna przestrzeń, która może być wykorzystana do różnorodnych wydarzeń, takich jak konferencje, szkolenia, spotkania biznesowe, czy kameralne wydarzenia kulturalne. Elastyczne ustawienie sali oraz nowoczesne wyposażenie audiowizualne sprawiają, że jest to idealne miejsce do organizacji wydarzeń o różnym charakterze. Wysoka jakość akustyki i komfortowe warunki zapewniają dogodną atmosferę.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala ogólna", "ogolna.jpg", null },
                    { 6, null, "Sala konferencyjna dedykowana organizacji spotkań biznesowych, konferencji, prezentacji i seminariów. Wyposażona w nowoczesny sprzęt audiowizualny, systemy do wideokonferencji oraz przestronną salę, która pomieści większą liczbę uczestników. Komfortowe, ergonomiczne fotele i stół konferencyjny sprzyjają profesjonalnym spotkaniom, a elastyczne ustawienie sali pozwala dostosować ją do różnych formatów spotkań.", new Guid("00000000-0000-0000-0000-000000000000"), false, false, false, "Sala konferencyjna", "konferencyjna.jpg", null }
                });

            migrationBuilder.InsertData(
                table: "MediaPatron",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "MediaPatronGuid", "Name", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, new Guid("ae371ef5-1408-4a99-9df5-c02da3894a64"), "Gazeta Nowoczesna", "gazeta_nowoczesna.png", null },
                    { 2, null, false, false, new Guid("1888ac98-9879-49d8-b5a0-a9bdedf65b79"), "Nowy świat TV", "nowy_swiat.png", null },
                    { 3, null, false, false, new Guid("21721629-b1aa-4fd0-8fce-689d78d6c326"), "Tygodnik Nowiny", "tygodnik_nowiny.png", null }
                });

            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "LongDescription", "NewsGuid", "PhotoName", "PublicationDate", "ShortDescription", "Title" },
                values: new object[,]
                {
                    { 1, "Finał konkursu artystycznego, który odbył się w naszym centrum, okazał się prawdziwym świętem sztuki. Udział wzięli artyści z różnych dziedzin: malarstwo, rzeźba, fotografia oraz sztuki wizualne. Każdy z uczestników miał okazję zaprezentować swoje dzieła przed szerszą publicznością, a także przed specjalnie powołaną komisją, składającą się z profesjonalistów z branży artystycznej. Zwycięzcy konkursu otrzymali nagrody pieniężne oraz profesjonalne wsparcie w dalszym rozwoju kariery artystycznej. Wydarzenie zakończyło się uroczystą galą, która na długo pozostanie w pamięci uczestników i widzów.", new Guid("c771ea3f-aa24-45c9-9fde-76695d5eab1d"), "konkurs_artystyczny.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Z radością ogłaszamy finał naszego konkursu artystycznego, w którym uczestnicy rywalizowali w różnych kategoriach. Wydarzenie przyciągnęło wielu utalentowanych artystów. Zwycięzcy otrzymali cenne nagrody i wyróżnienia.", "Finał konkursu artystycznego" },
                    { 2, "Koncert zespołu Lunar Vibes, który odbył się w naszej sali koncertowej, przyciągnął rzeszę miłośników muzyki alternatywnej i elektronicznej. Zespół, znany z dynamicznych i innowacyjnych brzmień, zapewnił publiczności niezapomniane przeżycia muzyczne. Dźwięki, które łączyły różnorodne gatunki muzyczne, wprowadziły słuchaczy w trans, a energetyzujące występy artystów zapewniły fantastyczną atmosferę. W trakcie koncertu widzowie mogli usłyszeć zarówno starsze utwory zespołu, jak i nowości, które dopiero mają trafić na nadchodzący album. Zespół zagrał przed pełną salą, a po wydarzeniu odbył się kameralny after party, podczas którego fani mieli okazję porozmawiać z artystami. Koncert Lunar Vibes był jednym z najważniejszych wydarzeń muzycznych w tym roku.", new Guid("94ab03cd-63d8-4516-a45b-990351680077"), "koncert_lunar_vibes.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zespół Lunar Vibes zachwycił publiczność podczas swojego koncertu w naszej sali koncertowej. Przenoszący w inny wymiar dźwięk, energetyzujący występ, który zapisał się na stałe w pamięci uczestników.", "Relacja z koncertu zespołu Lunar Vibes" },
                    { 3, "Z radością ogłaszamy zakończenie modernizacji naszej sali koncertowej. Dzięki inwestycjom w nowoczesne technologie, poprawiliśmy zarówno akustykę, jak i komfort naszych gości. Wymieniono systemy nagłośnienia i oświetlenia, a także dostosowano przestrzeń do większych wydarzeń, takich jak koncerty, festiwale muzyczne czy transmisje na żywo. Zwiększona pojemność sali oraz nowoczesne fotele zapewniają wygodę i komfort nawet podczas długotrwałych wydarzeń. Ponadto wprowadziliśmy innowacyjne rozwiązania z zakresu multimediów, dzięki czemu organizacja eventów o zróżnicowanej tematyce stała się jeszcze łatwiejsza. Od teraz, nasza sala koncertowa oferuje jeszcze lepsze warunki dla artystów i publiczności. Zapraszamy do odwiedzenia nowego, odmienionego wnętrza.", new Guid("253bc760-25d9-43ef-89dc-28453c96ef9a"), "modernizacja sali.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nasza sala koncertowa zyskała nowy wygląd i nowoczesne wyposażenie, które pozwoli na organizację jeszcze lepszych wydarzeń muzycznych. Zapraszamy do odwiedzenia odnowionego wnętrza.", "Modernizacja sali koncertowej" },
                    { 4, "Przygotowaliśmy wyjątkową ofertę dla naszych stałych i nowych gości. Tylko teraz oferujemy 20% zniżki na wszystkie karnety w sprzedaży. Karnet upoważnia do udziału w wydarzeniach organizowanych w naszym centrum przez najbliższy sezon. Oferujemy szeroką gamę wydarzeń, od koncertów, przez wystawy artystyczne, po spektakle teatralne. Zniżka dotyczy zarówno karnetów indywidualnych, jak i grupowych. To doskonała okazja, by zagwarantować sobie dostęp do najlepszych wydarzeń w atrakcyjnej cenie. Oferta jest ograniczona czasowo, dlatego warto się pospieszyć i skorzystać z tej wyjątkowej promocji, zanim będzie za późno.", new Guid("457bb9e7-2489-4854-952c-b92c452a7e63"), "znizka.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zachęcamy do zakupu karnetów na nadchodzące wydarzenia w naszym centrum. Tylko teraz oferujemy 20% zniżki na wszystkie karnety. To doskonała okazja, by nie przegapić żadnego z naszych wydarzeń.", "Zniżka 20% na zakup karnetów" },
                    { 5, "Noc Filmowa to wydarzenie, które przyciąga wszystkich miłośników dobrego kina. Podczas tego specjalnego wieczoru będziemy wyświetlać klasyki światowego kina, które na zawsze zapisały się w historii filmografii. Filmy będą prezentowane na dużym ekranie, a publiczność będzie miała okazję zobaczyć je w towarzystwie innych pasjonatów kina. Zadbaliśmy o wyjątkową atmosferę, w tym profesjonalne nagłośnienie i oświetlenie, by każda projekcja była niezapomnianym przeżyciem. To wydarzenie jest doskonałą okazją, by spędzić czas w gronie osób, które dzielą tę samą pasję do sztuki filmowej. Zapraszamy do wspólnego świętowania klasyki kina.", new Guid("57da63cb-91af-4afb-9b3b-092f72620f75"), "noc_filmowa.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zapraszamy na wyjątkowy wieczór z klasykami kina. Noc Filmowa to doskonała okazja, by obejrzeć kultowe filmy na dużym ekranie w towarzystwie innych miłośników kina.", "Noc Filmowa z Klasykami Kina" },
                    { 6, "Wernisaż wystawy pt. 'Nowe inspiracje' to wydarzenie, które łączy w sobie różne formy sztuki współczesnej. Na wystawie zaprezentowane zostaną prace artystów z różnych dziedzin: malarstwo, fotografia, rzeźba oraz multimedia. Każdy z artystów pokaże swoje najnowsze dzieła, które są wynikiem ich poszukiwań i eksperymentów artystycznych. Wystawa daje możliwość zobaczenia sztuki w jej najnowszej odsłonie i odkrycia nowych, świeżych inspiracji. Wernisaż to doskonała okazja, by spotkać się z twórcami, porozmawiać o ich pracy i zbliżyć się do współczesnej sztuki. Wydarzenie odbędzie się w naszej przestronnej galerii, która doskonale nadaje się do prezentacji tego typu wystaw.", new Guid("8bb51e9e-1807-48b8-b48b-5eb6cfa04980"), "wernisaz.png", new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zapraszamy na wernisaż wystawy 'Nowe inspiracje'. Artyści z różnych dziedzin sztuki zaprezentują swoje najnowsze prace. To doskonała okazja, by odkryć świeże spojrzenie na współczesną sztukę.", "Wernisaż: Nowe inspiracje" }
                });

            migrationBuilder.InsertData(
                table: "Organizer",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "OrganizerGuid", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "EventFlow", new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"), "eventflow.png", null },
                    { 2, null, false, false, "Snowflake", new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"), "snowflake.png", null },
                    { 3, null, false, false, "Aura", new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"), "aura.png", null }
                });

            migrationBuilder.InsertData(
                table: "Partner",
                columns: new[] { "Id", "Name", "PartnerGuid", "PhotoName" },
                values: new object[,]
                {
                    { 1, "Basel", new Guid("8e0ab6f0-ebbd-4d96-ae19-574b276e985c"), "basel.png" },
                    { 2, "Aura", new Guid("fa1fefc3-3a92-4162-a5cc-30aafea11323"), "aura.png" },
                    { 3, "Vision", new Guid("bb243a20-bf87-4534-b9b2-a994004f00c4"), "vision.png" },
                    { 4, "Snowflake", new Guid("2a1dbe6b-42f3-4fec-ab63-dffba70184b7"), "snowflake.png" },
                    { 5, "Waveless", new Guid("20f68122-1bdf-4881-8ce4-f808701648a5"), "waveless.png" }
                });

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "PaymentTypeGuid", "PhotoName", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "PayU", new Guid("2aa93a32-7232-48e7-a05b-8364c8aafffc"), "", null },
                    { 2, null, false, false, "Karnet", new Guid("1938093d-7427-40e5-a736-c2aa4305dc16"), "", null }
                });

            migrationBuilder.InsertData(
                table: "SeatType",
                columns: new[] { "Id", "AddtionalPaymentPercentage", "DeleteDate", "Description", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "SeatColor", "UpdateDate" },
                values: new object[,]
                {
                    { 1, 25.00m, null, "Opis miejsca VIP", false, false, false, "Miejsce VIP", "#9803fc", null },
                    { 2, 10.00m, null, "Opis miejsca klasy premium", false, false, false, "Miejsce klasy premium", "#ffa600", null },
                    { 3, 0.00m, null, "Opis miejsca zwykłego", false, false, false, "Miejsce zwykłe", "#039aff", null }
                });

            migrationBuilder.InsertData(
                table: "Sponsor",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsUpdated", "Name", "PhotoName", "SponsorGuid", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, "Basel", "basel.png", new Guid("423d7369-b70c-4471-97a8-968d001718a7"), null },
                    { 2, null, false, false, "Vision", "vision.png", new Guid("6ba10301-43f6-4207-835c-15f03d7904c7"), null },
                    { 3, null, false, false, "Waveless", "waveless.png", new Guid("ca9f16d2-cfa6-47dc-9aff-a0821ce6bf79"), null }
                });

            migrationBuilder.InsertData(
                table: "TicketJPG",
                columns: new[] { "Id", "FileName", "ReservationGuid" },
                values: new object[,]
                {
                    { 1, "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.jpg", new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3") },
                    { 2, "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.jpg", new Guid("ed8b9230-223b-4609-8d13-aa6017edad09") },
                    { 3, "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.jpg", new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a") },
                    { 4, "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.jpg", new Guid("de1d6773-f027-4888-996a-0296e5c52708") },
                    { 5, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_1.jpg", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 6, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd_2.jpg", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 7, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_1.jpg", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") },
                    { 8, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6_2.jpg", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") }
                });

            migrationBuilder.InsertData(
                table: "TicketPDF",
                columns: new[] { "Id", "FileName", "ReservationGuid" },
                values: new object[,]
                {
                    { 1, "eventflow_bilet_test_53538b58-f885-4f4a-b675-a4aa4063ccf3.pdf", new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3") },
                    { 2, "eventflow_bilet_test_ed8b9230-223b-4609-8d13-aa6017edad09.pdf", new Guid("ed8b9230-223b-4609-8d13-aa6017edad09") },
                    { 3, "eventflow_bilet_test_f9a076c4-3475-4a28-a60c-6e0e3c03731a.pdf", new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a") },
                    { 4, "eventflow_bilet_test_de1d6773-f027-4888-996a-0296e5c52708.pdf", new Guid("de1d6773-f027-4888-996a-0296e5c52708") },
                    { 5, "eventflow_bilet_test_0b74ec7b-933b-4163-afa5-e0997681dccd.pdf", new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd") },
                    { 6, "eventflow_bilet_test_806cade1-2685-43dc-8cfc-682fc4229db6.pdf", new Guid("806cade1-2685-43dc-8cfc-682fc4229db6") }
                });

            migrationBuilder.InsertData(
                table: "TicketType",
                columns: new[] { "Id", "DeleteDate", "IsDeleted", "IsSoftUpdated", "IsUpdated", "Name", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, false, false, false, "Bilet normalny", null },
                    { 2, null, false, false, false, "Bilet ulgowy", null },
                    { 3, null, false, false, false, "Bilet rodzinny", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "1" },
                    { "2", "2" },
                    { "2", "3" },
                    { "2", "4" },
                    { "2", "5" }
                });

            migrationBuilder.InsertData(
                table: "EventPass",
                columns: new[] { "Id", "DeleteDate", "EndDate", "EventPassGuid", "EventPassJPGName", "EventPassPDFName", "IsDeleted", "IsUpdated", "PassTypeId", "PaymentAmount", "PaymentDate", "PaymentTypeId", "PreviousEndDate", "RenewalDate", "StartDate", "TotalDiscount", "TotalDiscountPercentage", "UpdateDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b00ca94a-e6b2-4d2e-b270-244b3e76048d"), "eventflow_karnet_b00ca94a-e6b2-4d2e-b270-244b3e76048d.jpg", "eventflow_karnet_b00ca94a-e6b2-4d2e-b270-244b3e76048d.pdf", false, false, 3, 499.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 0m, null, "2" },
                    { 2, null, new DateTime(2026, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("766245b4-8c08-49dd-9480-2606aaa590be"), "eventflow_karnet_766245b4-8c08-49dd-9480-2606aaa590be.jpg", "eventflow_karnet_766245b4-8c08-49dd-9480-2606aaa590be.pdf", false, false, 4, 999.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 0m, null, "3" },
                    { 3, null, new DateTime(2025, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("33610a0d-a1b7-4700-bffe-9e334b977e6a"), "eventflow_karnet_33610a0d-a1b7-4700-bffe-9e334b977e6a.jpg", "eventflow_karnet_33610a0d-a1b7-4700-bffe-9e334b977e6a.pdf", false, false, 2, 235.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, null, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 0m, null, "4" }
                });

            migrationBuilder.InsertData(
                table: "Festival",
                columns: new[] { "Id", "AddDate", "DeleteDate", "Duration", "EndDate", "FestivalGuid", "IsDeleted", "IsUpdated", "Name", "PhotoName", "ShortDescription", "StartDate", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, -2682000L, new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0e36fd43-030a-4459-88fa-f99f53e601a3"), false, false, "Festiwal Muzyki Hip-Hop", "festiwal_muzyki_hip_hop.png", "Największy festiwal hip-hopowy! Koncerty, freestyle battle, warsztaty DJ-skie i spotkania z artystami.", new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, -2685600L, new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d946406d-7b80-40ca-aeff-c2966dcb6727"), false, false, "Festiwal Filmowy", "festiwal_filmowy.png", "Przegląd najlepszych filmów z całego świata! Premiera, spotkania z reżyserami i nocne maratony kinowe.", new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, -2602800L, new DateTime(2025, 5, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("d96a3633-6a7f-42fb-ba84-94e5902adce7"), false, false, "Festiwal Sztuki", "festiwal_sztuki.png", "Wystawy, instalacje i performanse! Odkryj nowoczesne formy sztuki i weź udział w kreatywnych warsztatach.", new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Hall",
                columns: new[] { "Id", "DefaultId", "Floor", "HallNr", "HallTypeId", "HallViewFileName", "IsCopy", "IsUpdated", "IsVisible", "RentalPricePerHour", "UpdateDate" },
                values: new object[,]
                {
                    { 1, 1, 2m, 1, 1, "sala_5_5_1.pdf", false, false, true, 120.99m, null },
                    { 2, 2, 1m, 2, 2, "sala_5_5_2.pdf", false, false, true, 89.99m, null },
                    { 3, 3, 2m, 3, 3, "sala_5_5_3.pdf", false, false, true, 179.99m, null },
                    { 4, 4, 1m, 4, 4, "sala_5_5_4.pdf", false, false, true, 199.99m, null },
                    { 5, 1, 2m, 1, 1, "sala_5_5_5.pdf", true, false, false, 120.99m, null },
                    { 6, 2, 1m, 2, 2, "sala_5_5_6.pdf", true, false, false, 89.99m, null },
                    { 7, 3, 2m, 3, 3, "sala_5_5_7.pdf", true, false, false, 179.99m, null },
                    { 8, 4, 1m, 4, 4, "sala_5_5_8.pdf", true, false, false, 199.99m, null },
                    { 9, 1, 2m, 1, 1, "sala_5_5_9.pdf", true, false, false, 120.99m, null },
                    { 10, 2, 1m, 2, 2, "sala_5_5_10.pdf", true, false, false, 89.99m, null },
                    { 11, 4, 1m, 4, 4, "sala_5_5_11.pdf", true, false, false, 199.99m, null },
                    { 12, 1, 2m, 1, 1, "sala_5_5_12.pdf", true, false, false, 120.99m, null },
                    { 13, 2, 1m, 2, 2, "sala_5_5_13.pdf", true, false, false, 89.99m, null },
                    { 14, 3, 2m, 3, 3, "sala_5_5_14.pdf", true, false, false, 179.99m, null },
                    { 15, 4, 1m, 4, 4, "sala_5_5_15.pdf", true, false, false, 199.99m, null },
                    { 16, 3, 2m, 3, 3, "sala_5_5_16.pdf", true, false, false, 179.99m, null },
                    { 17, 2, 1m, 2, 2, "sala_5_5_17.pdf", true, false, false, 89.99m, null },
                    { 18, 1, 2m, 1, 1, "sala_5_5_18.pdf", true, false, false, 120.99m, null },
                    { 19, 3, 2m, 3, 3, "sala_5_5_19.pdf", true, false, false, 179.99m, null },
                    { 20, 4, 1m, 4, 4, "sala_5_5_20.pdf", true, false, false, 199.99m, null }
                });

            migrationBuilder.InsertData(
                table: "HallType_Equipment",
                columns: new[] { "EquipmentId", "HallTypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 2 },
                    { 5, 2 },
                    { 5, 5 },
                    { 5, 6 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 1 },
                    { 7, 2 },
                    { 7, 3 },
                    { 7, 4 },
                    { 7, 5 },
                    { 7, 6 },
                    { 8, 6 },
                    { 9, 3 },
                    { 10, 4 },
                    { 11, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserData",
                columns: new[] { "Id", "City", "FlatNumber", "HouseNumber", "PhoneNumber", "Street", "ZipCode" },
                values: new object[,]
                {
                    { "1", "Warszawa", 14m, 12m, "789456123", "Wesoła", "15-264" },
                    { "2", "Poznań", 31m, 10m, "123456789", "Wiejska", "01-342" },
                    { "3", "Białystok", 21m, 7m, "147852369", "Pogodna", "14-453" },
                    { "4", "Warszawa", 42m, 21m, "147852369", "Słoneczna", "14-453" }
                });

            migrationBuilder.InsertData(
                table: "Event",
                columns: new[] { "Id", "AddDate", "CategoryId", "DeleteDate", "Duration", "EndDate", "EventGuid", "HallId", "IsDeleted", "IsUpdated", "Name", "PhotoName", "ShortDescription", "StartDate", "UpdateDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, -3600L, new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new Guid("bd9cf606-260b-4611-bb6f-3cde4298edc7"), 6, false, false, "Koncert: Mystic Waves", "koncert_mystic_waves.png", "Mystic Waves to niezapomniana podróż przez elektroniczne brzmienia i hipnotyzujące melodie.", new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, -10800L, new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("30f24949-51df-4274-a804-544ee7e2e1d2"), 7, false, false, "Cień Przeszłości", "cien_przeszlosci.png", "Dramatyczna opowieść o sekretach przeszłości, które powracają, by zmienić teraźniejszość.", new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, -7200L, new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new Guid("63b9ba64-48dd-4997-95c9-dbfa2389f066"), 5, false, false, "Królestwo planety małp", "krolestwo_planety_malp.png", "Kolejna część kultowej serii sci-fi. Świat, w którym dominują małpy, a ludzie walczą o przetrwanie.", new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, -10800L, new DateTime(2025, 4, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5e01e785-8f63-4f8c-af89-25ee89f22b83"), 8, false, false, "Nowe inspiracje", "nowe_inspiracje.png", "Wystawa prezentująca prace nowoczesnych artystów, pełna świeżych perspektyw i innowacji.", new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, -3600L, new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), new Guid("7f940a53-f3a2-46d8-8d39-b28e076d40aa"), 10, false, false, "Koncert: New Era", "new_era.png", "Energetyczny koncert zespołu New Era – mieszanka rocka, elektroniki i alternatywnych brzmień.", new DateTime(2025, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, -7200L, new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), new Guid("bc7b2f85-b498-4186-ad5c-5cf25d938499"), 9, false, false, "Gladiator", "gladiator.png", "Legendarna opowieść o honorze i zemście w starożytnym Rzymie.", new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, -10800L, new DateTime(2025, 5, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("775b339d-4524-4829-bd42-4da8f88cf3f1"), 11, false, false, "Nowa sztuka", "", "Wystawa prezentująca najnowsze prace artystów eksperymentalnych i awangardowych.", new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, 9000L, new DateTime(2025, 6, 2, 2, 30, 0, 0, DateTimeKind.Unspecified), new Guid("08050835-f7b3-4395-9679-b7bc915c84a6"), 16, false, false, "Cień Nad Miastem", "cien_nad_miastem.png", "Mroczny thriller teatralny o tajemniczych wydarzeniach, które wstrząsają miastem po zmroku.", new DateTime(2025, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, 7200L, new DateTime(2025, 3, 17, 3, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a4f24bff-10c1-4763-b410-ba0e6e7bb3d3"), 17, false, false, "Jazzowe Brzmienia", "jazz.png", "Magiczny czas z jazzem na żywo – koncert znanych artystów jazzowych, którzy przeniosą Cię w świat wyjątkowych dźwięków.", new DateTime(2025, 3, 17, 1, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, 7200L, new DateTime(2025, 3, 16, 4, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f9b817c5-e386-4107-9ed3-49b7ce38c519"), 18, false, false, "Interstellar", "interstellar.jpg", "Epicka podróż kosmiczna, która bada granice ludzkiej wytrzymałości i miłości.", new DateTime(2025, 3, 16, 2, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, 7200L, new DateTime(2025, 3, 15, 5, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ef97fdf1-9b04-47aa-bc42-1ff4f7fd5a07"), 19, false, false, "Zatrzymać Przeznaczenie", "zatrzymac_przeznaczenie.jpg", "Spektakl o postaci, która staje w obliczu nieuchronnego przeznaczenia.", new DateTime(2025, 3, 15, 3, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, 7200L, new DateTime(2025, 3, 14, 6, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3c229abc-c4e2-41e9-9aad-487cd733f8a5"), 20, false, false, "Nowe Perspektywy", "nowe_perspektywy.jpg", "Wystawa sztuki współczesnej, która zaprasza do odkrywania świata z nowych punktów widzenia.", new DateTime(2025, 3, 14, 4, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Festival_MediaPatron",
                columns: new[] { "FestivalId", "MediaPatronId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Festival_Organizer",
                columns: new[] { "FestivalId", "OrganizerId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Festival_Sponsor",
                columns: new[] { "FestivalId", "SponsorId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "HallDetails",
                columns: new[] { "Id", "MaxNumberOfSeats", "MaxNumberOfSeatsColumns", "MaxNumberOfSeatsRows", "NumberOfSeats", "StageLength", "StageWidth", "TotalArea", "TotalLength", "TotalWidth" },
                values: new object[,]
                {
                    { 1, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 2, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 3, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 4, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 5, 255m, 15m, 15m, 100m, 4.4m, 7m, 108m, 22m, 14m },
                    { 6, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 7, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 8, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 9, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 10, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 11, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 12, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 13, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 14, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 15, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m },
                    { 16, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 17, 150m, 10m, 15m, 150m, null, null, 198m, 22m, 9m },
                    { 18, 255m, 15m, 15m, 100m, 4.4m, 7m, 308m, 22m, 14m },
                    { 19, 60m, 10m, 6m, 60m, 4.4m, 5.6m, 99m, 11m, 9m },
                    { 20, 100m, 10m, 10m, 100m, 6m, 7.2m, 153m, 17m, 9m }
                });

            migrationBuilder.InsertData(
                table: "HallRent",
                columns: new[] { "Id", "DeleteDate", "Duration", "EndDate", "HallId", "HallRentGuid", "HallRentPDFName", "IsDeleted", "IsUpdated", "PaymentAmount", "PaymentDate", "PaymentTypeId", "RentDate", "StartDate", "UpdateDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, 28800L, new DateTime(2025, 4, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), 12, new Guid("48972d7a-ab0d-4827-bdd6-2002dc1f1e08"), "wynajem_sali_1.pdf", false, false, 899.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "4" },
                    { 2, null, 14400L, new DateTime(2025, 4, 20, 4, 0, 0, 0, DateTimeKind.Unspecified), 13, new Guid("3ad58762-60f0-4753-bdf7-c9c7e3eb775c"), "wynajem_sali_2.pdf", false, false, 699.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "3" },
                    { 3, null, 7200L, new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), 14, new Guid("38daa558-0f6e-414a-913c-c500d41d5dc5"), "wynajem_sali_3.pdf", false, false, 399.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "3" },
                    { 4, null, 3600L, new DateTime(2025, 4, 22, 1, 0, 0, 0, DateTimeKind.Unspecified), 15, new Guid("e690b490-7387-490c-89df-bd9eaf2cc36b"), "wynajem_sali_4.pdf", false, false, 150.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2" }
                });

            migrationBuilder.InsertData(
                table: "Seat",
                columns: new[] { "Id", "Column", "GridColumn", "GridRow", "HallId", "IsUpdated", "Row", "SeatNr", "SeatTypeId", "UpdateDate" },
                values: new object[,]
                {
                    { 1, 1m, 1m, 1m, 1, false, 1m, 1m, 1, null },
                    { 2, 2m, 2m, 1m, 1, false, 1m, 2m, 1, null },
                    { 3, 3m, 3m, 1m, 1, false, 1m, 3m, 1, null },
                    { 4, 4m, 4m, 1m, 1, false, 1m, 4m, 1, null },
                    { 5, 1m, 1m, 1m, 2, false, 1m, 1m, 2, null },
                    { 6, 2m, 2m, 1m, 2, false, 1m, 2m, 2, null },
                    { 7, 3m, 3m, 1m, 2, false, 1m, 3m, 1, null },
                    { 8, 4m, 4m, 1m, 2, false, 1m, 4m, 2, null },
                    { 9, 1m, 1m, 1m, 3, false, 1m, 1m, 3, null },
                    { 10, 2m, 2m, 1m, 3, false, 1m, 2m, 3, null },
                    { 11, 3m, 3m, 1m, 3, false, 1m, 3m, 3, null },
                    { 12, 4m, 4m, 1m, 3, false, 1m, 4m, 3, null },
                    { 13, 1m, 1m, 1m, 4, false, 1m, 1m, 3, null },
                    { 14, 1m, 1m, 1m, 5, false, 1m, 1m, 1, null },
                    { 15, 2m, 2m, 1m, 5, false, 1m, 2m, 1, null },
                    { 16, 3m, 3m, 1m, 5, false, 1m, 3m, 1, null },
                    { 17, 4m, 4m, 1m, 5, false, 1m, 4m, 1, null },
                    { 18, 1m, 1m, 1m, 9, false, 1m, 1m, 1, null },
                    { 19, 2m, 2m, 1m, 9, false, 1m, 2m, 1, null },
                    { 20, 3m, 3m, 1m, 9, false, 1m, 3m, 1, null },
                    { 21, 4m, 4m, 1m, 9, false, 1m, 4m, 1, null },
                    { 22, 1m, 1m, 1m, 12, false, 1m, 1m, 1, null },
                    { 23, 2m, 2m, 1m, 12, false, 1m, 2m, 1, null },
                    { 24, 3m, 3m, 1m, 12, false, 1m, 3m, 1, null },
                    { 25, 4m, 4m, 1m, 12, false, 1m, 4m, 1, null },
                    { 26, 1m, 1m, 1m, 6, false, 1m, 1m, 2, null },
                    { 27, 2m, 2m, 1m, 6, false, 1m, 2m, 2, null },
                    { 28, 3m, 3m, 1m, 6, false, 1m, 3m, 1, null },
                    { 29, 4m, 4m, 1m, 6, false, 1m, 4m, 2, null },
                    { 30, 1m, 1m, 1m, 10, false, 1m, 1m, 2, null },
                    { 31, 2m, 2m, 1m, 10, false, 1m, 2m, 2, null },
                    { 32, 3m, 3m, 1m, 10, false, 1m, 3m, 1, null },
                    { 33, 4m, 4m, 1m, 10, false, 1m, 4m, 2, null },
                    { 34, 1m, 1m, 1m, 13, false, 1m, 1m, 2, null },
                    { 35, 2m, 2m, 1m, 13, false, 1m, 2m, 2, null },
                    { 36, 3m, 3m, 1m, 13, false, 1m, 3m, 1, null },
                    { 37, 4m, 4m, 1m, 13, false, 1m, 4m, 2, null },
                    { 38, 1m, 1m, 1m, 7, false, 1m, 1m, 3, null },
                    { 39, 2m, 2m, 1m, 7, false, 1m, 2m, 3, null },
                    { 40, 3m, 3m, 1m, 7, false, 1m, 3m, 3, null },
                    { 41, 4m, 4m, 1m, 7, false, 1m, 4m, 3, null },
                    { 42, 1m, 1m, 1m, 14, false, 1m, 1m, 3, null },
                    { 43, 2m, 2m, 1m, 14, false, 1m, 2m, 3, null },
                    { 44, 3m, 3m, 1m, 14, false, 1m, 3m, 3, null },
                    { 45, 4m, 4m, 1m, 14, false, 1m, 4m, 3, null },
                    { 46, 1m, 1m, 1m, 8, false, 1m, 1m, 3, null },
                    { 47, 1m, 1m, 1m, 11, false, 1m, 1m, 3, null },
                    { 48, 1m, 1m, 1m, 15, false, 1m, 1m, 3, null },
                    { 49, 1m, 1m, 1m, 16, false, 1m, 1m, 3, null },
                    { 50, 2m, 2m, 1m, 16, false, 1m, 2m, 3, null },
                    { 51, 3m, 3m, 1m, 16, false, 1m, 3m, 3, null },
                    { 52, 4m, 4m, 1m, 16, false, 1m, 4m, 3, null },
                    { 53, 1m, 1m, 1m, 17, false, 1m, 1m, 2, null },
                    { 54, 2m, 2m, 1m, 17, false, 1m, 2m, 2, null },
                    { 55, 3m, 3m, 1m, 17, false, 1m, 3m, 1, null },
                    { 56, 4m, 4m, 1m, 17, false, 1m, 4m, 2, null },
                    { 57, 1m, 1m, 1m, 18, false, 1m, 1m, 1, null },
                    { 58, 2m, 2m, 1m, 18, false, 1m, 2m, 1, null },
                    { 59, 3m, 3m, 1m, 18, false, 1m, 3m, 1, null },
                    { 60, 4m, 4m, 1m, 18, false, 1m, 4m, 1, null },
                    { 61, 1m, 1m, 1m, 19, false, 1m, 1m, 3, null },
                    { 62, 2m, 2m, 1m, 19, false, 1m, 2m, 3, null },
                    { 63, 3m, 3m, 1m, 19, false, 1m, 3m, 3, null },
                    { 64, 4m, 4m, 1m, 19, false, 1m, 4m, 3, null },
                    { 65, 1m, 1m, 1m, 20, false, 1m, 1m, 3, null }
                });

            migrationBuilder.InsertData(
                table: "Festival_Event",
                columns: new[] { "EventId", "FestivalId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 5, 1 },
                    { 2, 2 },
                    { 6, 2 },
                    { 4, 3 },
                    { 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "HallRent_AdditionalServices",
                columns: new[] { "AdditionalServicesId", "HallRentId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 2, 3 },
                    { 4, 4 },
                    { 5, 4 }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "Id", "DeleteDate", "EventId", "FestivalId", "IsDeleted", "IsUpdated", "Price", "TicketTypeId", "UpdateDate" },
                values: new object[,]
                {
                    { 1, null, 1, null, false, false, 24.99m, 1, null },
                    { 2, null, 2, null, false, false, 34.99m, 1, null },
                    { 3, null, 3, null, false, false, 29.99m, 1, null },
                    { 4, null, 4, null, false, false, 19.99m, 1, null },
                    { 5, null, 1, 1, false, false, 19.99m, 1, null },
                    { 6, null, 5, 1, false, false, 19.99m, 1, null },
                    { 7, null, 2, 2, false, false, 29.99m, 2, null },
                    { 8, null, 6, 2, false, false, 29.99m, 2, null },
                    { 9, null, 5, null, false, false, 24.99m, 1, null },
                    { 10, null, 6, null, false, false, 32.99m, 1, null },
                    { 11, null, 7, null, false, false, 34.99m, 1, null },
                    { 12, null, 8, null, false, false, 19.99m, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Reservation",
                columns: new[] { "Id", "DeleteDate", "EndDate", "EventPassId", "IsDeleted", "IsFestivalReservation", "IsUpdated", "PaymentAmount", "PaymentDate", "PaymentTypeId", "ReservationDate", "ReservationGuid", "StartDate", "TicketId", "TicketPDFId", "TotalAdditionalPaymentAmount", "TotalAddtionalPaymentPercentage", "TotalDiscount", "UpdateDate", "UserId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, 24.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3"), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 2.5m, 10m, 0m, null, "4" },
                    { 2, null, new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, 34.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("ed8b9230-223b-4609-8d13-aa6017edad09"), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 0m, 0m, 0m, null, "2" },
                    { 3, null, new DateTime(2025, 4, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, 29.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a"), new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 7.5m, 25m, 0m, null, "3" },
                    { 4, null, new DateTime(2025, 4, 22, 3, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, 19.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("de1d6773-f027-4888-996a-0296e5c52708"), new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 4, 0m, 0m, 0m, null, "3" },
                    { 5, null, new DateTime(2025, 4, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, false, 19.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd"), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 5, 2m, 10m, 0m, null, "2" },
                    { 6, null, new DateTime(2025, 5, 19, 1, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, false, 19.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd"), new DateTime(2025, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 5, 2m, 10m, 0m, null, "2" },
                    { 7, null, new DateTime(2025, 4, 20, 3, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, false, 29.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("806cade1-2685-43dc-8cfc-682fc4229db6"), new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 6, 0m, 0m, 0m, null, "2" },
                    { 8, null, new DateTime(2025, 5, 21, 2, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, false, 29.99m, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("806cade1-2685-43dc-8cfc-682fc4229db6"), new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 6, 0m, 0m, 0m, null, "2" }
                });

            migrationBuilder.InsertData(
                table: "Reservation_Seat",
                columns: new[] { "ReservationId", "SeatId" },
                values: new object[,]
                {
                    { 1, 26 },
                    { 2, 38 },
                    { 3, 14 },
                    { 4, 46 },
                    { 5, 29 },
                    { 6, 33 },
                    { 7, 41 },
                    { 8, 20 }
                });

            migrationBuilder.InsertData(
                table: "Reservation_TicketJPG",
                columns: new[] { "ReservationId", "TicketJPGId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 5, 6 },
                    { 6, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 7, 8 },
                    { 8, 7 },
                    { 8, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Event_CategoryId",
                table: "Event",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_HallId",
                table: "Event",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPass_PassTypeId",
                table: "EventPass",
                column: "PassTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPass_PaymentTypeId",
                table: "EventPass",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPass_UserId",
                table: "EventPass",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Event_EventId",
                table: "Festival_Event",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_MediaPatron_MediaPatronId",
                table: "Festival_MediaPatron",
                column: "MediaPatronId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Organizer_OrganizerId",
                table: "Festival_Organizer",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_Festival_Sponsor_SponsorId",
                table: "Festival_Sponsor",
                column: "SponsorId");

            migrationBuilder.CreateIndex(
                name: "IX_Hall_HallTypeId",
                table: "Hall",
                column: "HallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HallRent_HallId",
                table: "HallRent",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_HallRent_PaymentTypeId",
                table: "HallRent",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HallRent_UserId",
                table: "HallRent",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HallRent_AdditionalServices_AdditionalServicesId",
                table: "HallRent_AdditionalServices",
                column: "AdditionalServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_HallType_Equipment_HallTypeId",
                table: "HallType_Equipment",
                column: "HallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_EventPassId",
                table: "Reservation",
                column: "EventPassId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_PaymentTypeId",
                table: "Reservation",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TicketId",
                table: "Reservation",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TicketPDFId",
                table: "Reservation",
                column: "TicketPDFId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Seat_SeatId",
                table: "Reservation_Seat",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TicketJPG_TicketJPGId",
                table: "Reservation_TicketJPG",
                column: "TicketJPGId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_HallId",
                table: "Seat",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_SeatTypeId",
                table: "Seat",
                column: "SeatTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventId",
                table: "Ticket",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_FestivalId",
                table: "Ticket",
                column: "FestivalId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketTypeId",
                table: "Ticket",
                column: "TicketTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FAQ");

            migrationBuilder.DropTable(
                name: "Festival_Event");

            migrationBuilder.DropTable(
                name: "Festival_MediaPatron");

            migrationBuilder.DropTable(
                name: "Festival_Organizer");

            migrationBuilder.DropTable(
                name: "Festival_Sponsor");

            migrationBuilder.DropTable(
                name: "HallDetails");

            migrationBuilder.DropTable(
                name: "HallRent_AdditionalServices");

            migrationBuilder.DropTable(
                name: "HallType_Equipment");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.DropTable(
                name: "Reservation_Seat");

            migrationBuilder.DropTable(
                name: "Reservation_TicketJPG");

            migrationBuilder.DropTable(
                name: "UserData");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MediaPatron");

            migrationBuilder.DropTable(
                name: "Organizer");

            migrationBuilder.DropTable(
                name: "Sponsor");

            migrationBuilder.DropTable(
                name: "AdditionalServices");

            migrationBuilder.DropTable(
                name: "HallRent");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "TicketJPG");

            migrationBuilder.DropTable(
                name: "SeatType");

            migrationBuilder.DropTable(
                name: "EventPass");

            migrationBuilder.DropTable(
                name: "TicketPDF");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EventPassType");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Festival");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropTable(
                name: "EventCategory");

            migrationBuilder.DropTable(
                name: "EventDetails");

            migrationBuilder.DropTable(
                name: "Hall");

            migrationBuilder.DropTable(
                name: "FestivalDetails");

            migrationBuilder.DropTable(
                name: "HallType");
        }
    }
}
