using System.Text.Json.Serialization;
using FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;
using FreshGuard.ColdTrack.Platform.Shared.Domain.Model.Entities;

namespace FreshGuard.ColdTrack.Platform.Iam.Domain.Model.Aggregates;

/// <summary>Represents a ColdTrack user account and its access role.</summary>
public class UserAccount : IAuditableEntity
{
    private UserAccount() { }

    public UserAccount(string fullName, EmailAddress email, string passwordHash, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Full name is required.", nameof(fullName));
        FullName = fullName.Trim();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = true;
    }

    public int Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public EmailAddress Email { get; private set; } = EmailAddress.Create("placeholder@coldtrack.local");
    [JsonIgnore] public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public enum UserRole
{
    LogisticsAdmin,
    Driver
}
