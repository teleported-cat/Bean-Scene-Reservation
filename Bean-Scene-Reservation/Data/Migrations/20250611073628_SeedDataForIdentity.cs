using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bean_Scene_Reservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForIdentity : Migration
    {
        private MigrationBuilder _migrationBuilder;

        // Role and user IDs (so you don't need to look at GUIDs 🤑)
        private Dictionary<string, string> _roleIds = new() {
            { "Member", "ff6b7c9c-1636-4ccb-8558-2cdcb324b0f6" },
            { "Staff", "b4b48b9c-f3ea-4884-b6bf-cf3fa309dfb1" },
            { "Manager", "10f1913a-619f-4cd4-8a45-88336326dd34" },
        };

        private Dictionary<string, string> _userIds = new() {
            { "SeededMember", "1ac0095d-c987-4dfc-a387-0ad330077fe1" }, // Seedler McMember
            { "SeededStaff", "ce30b818-e6c1-44ea-8eab-30145a3f4a87" }, // Seedla Staffinton
            { "SeededManager", "d5bbdf3e-9e6b-4399-97b9-d8f31c6bcd01" }, // Sedro Managero
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make the migration builder available to other methods
            _migrationBuilder = migrationBuilder;

            // You CAN write your raw SQL... but you probably shouldn't do that if there's a better choice.
            // Some reasons why is that SQL statements are considered strings and require escaping,
            // and 

            // Add roles to the AspNetRoles table
            CreateRole(_roleIds["Member"], "Member");
            CreateRole(_roleIds["Staff"], "Staff");
            CreateRole(_roleIds["Manager"], "Manager");

            // Add Users to the AspNetUsers table
            CreateUser(_userIds["SeededMember"], "seededmember@gmail.com", "Password123_", "seededmember@gmail.com", "Seedler", "McMember", null, [_roleIds["Member"]]);
            CreateUser(_userIds["SeededStaff"], "seededstaff@gmail.com", "Password123_", "seededstaff@gmail.com", "Seedla", "Staffinton", null, [_roleIds["Staff"]]);
            CreateUser(_userIds["SeededManager"], "seededmanager@gmail.com", "Password123_", "seededmanager@gmail.com", "Sedro", "Managero", null, [_roleIds["Manager"], _roleIds["Staff"]]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Make the migration builder available to other methods
            _migrationBuilder = migrationBuilder;

            // Delete roles from the AspNetRoles table
            foreach (var roleId in _roleIds)
            {
                DeleteRole(roleId.Value);
            }

            // Delete users from the AspNetUsers table 
            foreach (var userId in _userIds)
            {
                DeleteUser(userId.Value);
            }
        }

        private void CreateRole(string id, string name)
        {
            // NODO: Validation?

            // Create an object to hold the data
            var role = new IdentityRole
            {
                Id = id,
                Name = name,
                // Generate normalised name
                NormalizedName = name.ToUpperInvariant(),
                // Generate concurrency stamp (a random value that should change whenever a role is persisted to the store)
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Build query from object data
            // Insert data into database
            _migrationBuilder.InsertData
                (
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[]
                {
                    role.Id, role.Name, role.NormalizedName, role.ConcurrencyStamp
                }
                );
        }

        private void CreateUser(string id, string userName, string password, string email, string firstName, string lastName, string phoneNumber = null, string[]? roleIds = null)
        {
            // NODO: Validation? no

            PasswordHasher<ApplicationUser> passHash = new();

            // Create an object to hold the data
            var user = new ApplicationUser
            {
                // Main user data
                Id = id,
                UserName = userName,
                PasswordHash = "DUMMY",
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                // Normalised fields
                NormalizedUserName = userName.ToUpperInvariant(),
                NormalizedEmail = email.ToUpperInvariant(),

                // Randomly-generated stamp values
                ConcurrencyStamp = Guid.NewGuid().ToString(), // Generate concurrency stamp (a random value that should change whenever a user is persisted to the store)
                SecurityStamp = Guid.NewGuid().ToString(), // Generate security stamp (a random value that should change whenever a user's information is updated)

                // Other auth data
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                LockoutEnd = null,
                TwoFactorEnabled = false,
            };

            // Generate password hash for user
            user.PasswordHash = passHash.HashPassword(user, password);

            // Build query from object data
            string[] columns =
            {
                nameof(user.Id),
                nameof(user.UserName),
                nameof(user.Email),
                nameof(user. PasswordHash),
                nameof(user. PhoneNumber),
                nameof(user.FirstName),
                nameof(user.LastName),
                nameof(user.NormalizedUserName),
                nameof(user.NormalizedEmail),
                nameof(user. ConcurrencyStamp),
                nameof(user. SecurityStamp),
                nameof(user.EmailConfirmed),
                nameof(user. PhoneNumberConfirmed),
                nameof(user.AccessFailedCount),
                nameof(user.LockoutEnabled),
                nameof(user.LockoutEnd),
                nameof(user. TwoFactorEnabled),
            };

            object[] values =
            {
                user.Id,
                user.UserName,
                user.Email,
                user. PasswordHash,
                user. PhoneNumber,
                user.FirstName,
                user.LastName,
                user.NormalizedUserName,
                user.NormalizedEmail,
                user. ConcurrencyStamp,
                user. SecurityStamp,
                user.EmailConfirmed,
                user. PhoneNumberConfirmed,
                user.AccessFailedCount,
                user.LockoutEnabled,
                user.LockoutEnd,
                user. TwoFactorEnabled,
            };

            // Insert data into database
            _migrationBuilder.InsertData
                (
                table: "AspNetUsers",
                columns: columns,
                values: values
                );

            // Assign Roles
            if (roleIds != null)
            {
                foreach (string roleId in roleIds)
                {
                    CreateUserRole(user.Id, roleId);
                }
            }
        }

        private void CreateUserRole(string userId, string roleId)
        {
            // NODO: Validation? of course not

            // Create an object to hold the data
            var linking = new IdentityUserRole<string> { UserId = userId, RoleId = roleId };

            // Insert data into database
            _migrationBuilder.InsertData
            (
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] {
                    linking.UserId,
                    linking.RoleId,
                }
            );
        }

        private void DeleteRole(string id)
        {
            _migrationBuilder.DeleteData
            (
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: id
            );
        }

        private void DeleteUser(string id)
        {
            _migrationBuilder.DeleteData
                (
                    table: "AspNetUsers",
                    keyColumn: "Id",
                    keyValue: id
                );
        }
    }
}
