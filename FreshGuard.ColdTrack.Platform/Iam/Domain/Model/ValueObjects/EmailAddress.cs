using System.Net.Mail;

namespace FreshGuard.ColdTrack.Platform.Iam.Domain.Model.ValueObjects;

/// <summary>Represents a validated and normalized email address.</summary>
public sealed record EmailAddress
{
    private EmailAddress(string value) => Value = value;

    public string Value { get; }

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email is required.", nameof(value));
        var normalized = value.Trim().ToLowerInvariant();
        try
        {
            var address = new MailAddress(normalized);
            if (!string.Equals(address.Address, normalized, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Email format is invalid.", nameof(value));
        }
        catch (FormatException)
        {
            throw new ArgumentException("Email format is invalid.", nameof(value));
        }

        return new EmailAddress(normalized);
    }

    public override string ToString() => Value;
}


