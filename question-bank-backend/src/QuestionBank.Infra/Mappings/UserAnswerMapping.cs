using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class UserAnswerMapping : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder
            .HasKey(ua => ua.Id);

        builder
            .Property(ua => ua.IsCorrect)
            .IsRequired();

        builder
            .Property(ua => ua.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(ua => ua.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        // Relation: UserAnswer -> User
        builder
            .HasOne(ua => ua.User)
            .WithMany(u => u.UserAnswers)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relation: UserAnswer -> Question
        builder
            .HasOne(ua => ua.Question)
            .WithMany(q => q.UserAnswers)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relation: UserAnswer -> Alternative
        builder
            .HasOne(ua => ua.Alternative)
            .WithMany(a => a.UserAnswers)
            .HasForeignKey(ua => ua.AlternativeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}