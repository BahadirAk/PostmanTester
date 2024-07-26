using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostmanTester.Application.Extensions;
using PostmanTester.Application.Models.DataObjects;
using PostmanTester.Application.Models.Enums;
using PostmanTester.Domain.Entities;
using PostmanTester.Persistance.Contexts;

namespace PostmanTester.Persistance.Helpers
{
    public static class SeedDataHelper
    {
        public static async Task SeedAsync()
        {
            try
            {
                using var scope = ServiceTool.ServiceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<PostmanTesterDbContext>();
                try
                {
                    await context.Database.EnsureCreatedAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(DateTime.Now.ToString("yyyy.MM.dd:HH:mm:ss") + " | " + ex.Message);
                }

                if (!await context.Roles.AnyAsync())
                {
                    var roles = new[]
                    {
                        new Role { Name = "Super Admin", Status = (byte)GeneralEnum.Active, CreatedDate = DateTime.UtcNow },
                        new Role { Name = "Admin", Status = (byte)GeneralEnum.Active, CreatedDate = DateTime.UtcNow },
                        new Role { Name = "User", Status = (byte)GeneralEnum.Active, CreatedDate = DateTime.UtcNow }
                    };

                    await context.Roles.AddRangeAsync(roles);
                    await context.SaveChangesAsync();
                }

                if (!await context.Users.AnyAsync())
                {
                    string salt = "0x8B6927F0433A6A1243A77642A9316223B9B104BF2E5E522143E128CA40296B96D71E98FEBE06326E428D947C44BED816AD66FB38F191DA84F7C030D8101258C29F201DE9C40DFFB51691253D4113E7F46636F1589F12BFA6DA22E414A1328DDC872E939F4B88C7AD53E10F4C77DEB6A0143905E4B793785C01FD59859B041F80";
                    string hash = "0xE4A386F86B2626A9A01A44A57C27751BE4A01439E718B3D6DFD9A8DD19527D1B4FD62DF2E9D7E887A66EC5E2EDDC365A595D52E40FD1F9B81BF185BC6F4E57F9";

                    var users = new[]
                    {
                        new User { Email = "admin@admin.com", FirstName = "Admin", LastName = "Admin", PasswordSalt = salt.HexStringToByteArray(), PasswordHash = hash.HexStringToByteArray(), Phone = "905556685170", Address = "", RoleId = (short)RolesEnum.SuperAdmin, Status = (byte)GeneralEnum.Active, CreatedDate = DateTime.UtcNow }
                    };

                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(DateTime.Now.ToString("yyyy.MM.dd:HH:mm:ss") + " | " + e.Message);
            }
        }
    }
}
