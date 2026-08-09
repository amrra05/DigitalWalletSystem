using DigitalWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SenderWallet)
            .WithMany()
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict);



        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverWallet)
            .WithMany()
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
        .HasIndex(t => t.IdempotencyKey)
        .IsUnique();

        modelBuilder.Entity<RefreshToken>()
    .HasOne(r => r.User)
    .WithMany()
    .HasForeignKey(r => r.UserId)
    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Wallet>()
        .Property(w => w.Balance)
        .HasPrecision(18, 2);


        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Wallet>()
    .Property(w => w.RowVersion)
    .IsRowVersion();


        modelBuilder.Entity<EmailVerificationToken>()
    .HasOne(t => t.User)
    .WithMany()
    .HasForeignKey(t => t.UserId)
    .OnDelete(DeleteBehavior.Cascade);

    }
}