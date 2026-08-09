using BCrypt.Net;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        AppDbContext context)
    {
        var systemUser =
            await context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(
                    u => u.Role == UserRole.System);


        if (systemUser != null)
        {
            return;
        }


        var user = new User
        {
            Name = "Digital Wallet Bank",

            Email = "system@digitalwallet.com",

            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    Guid.NewGuid().ToString()),

            Role = UserRole.System,


            Wallet = new Wallet
            {
                Balance = 1000000,

                Currency = "JOD"
            }
        };


        await context.Users.AddAsync(user);


        await context.SaveChangesAsync();
    }
}