namespace QuestionBank.Application.Configurations;

public class JwtSettings
{
    public int HoursUntilExpiry { get; set; }
    public string KeyPath { get; set; } = null!;
}