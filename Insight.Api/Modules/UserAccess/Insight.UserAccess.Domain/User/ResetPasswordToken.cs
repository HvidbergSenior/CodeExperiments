using Insight.BuildingBlocks.Domain;

namespace Insight.UserAccess.Domain.User;

public sealed class ResetPasswordToken : ValueObject
{
    public string Value { get; private set; }

    private ResetPasswordToken()
    {
        Value = String.Empty;
    }

    private ResetPasswordToken(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("value is empty", nameof(value));
        }
        Value = value;
    }

    public static ResetPasswordToken Create(string value)
    {
        return new ResetPasswordToken(value);
    }

    public static ResetPasswordToken Empty()
    {
        return new ResetPasswordToken();
    }
}