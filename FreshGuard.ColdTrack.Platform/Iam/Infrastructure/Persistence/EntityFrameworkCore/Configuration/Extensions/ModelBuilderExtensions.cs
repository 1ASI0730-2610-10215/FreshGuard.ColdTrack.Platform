using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FreshGuard.ColdTrack.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Id).ValueGeneratedOnAdd();
            entity.Property(user => user.FullName).IsRequired().HasMaxLength(120);
            entity.Property(user => user.Email)
                .HasConversion(email => email.Value, value => Domain.Model.ValueObjects.EmailAddress.Create(value))
                .IsRequired()
                .HasMaxLength(254);
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(user => user.Role).HasConversion<string>().IsRequired().HasMaxLength(30);
            entity.Property(user => user.IsActive).IsRequired();
        });
    }
}
