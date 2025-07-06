using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class TestMapping : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .Property(t => t.Title)
            .IsRequired();

        builder
            .Property(t => t.TotalQuestions)
            .IsRequired();

        builder
            .Property(t => t.NumberOfCorrectAnswers)
            .IsRequired();

        builder
            .Property(t => t.Status)
            .IsRequired();

        builder
            .Property(t => t.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(t => t.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        // Relation: Test -> TestQuestion
        builder
            .HasMany(t => t.TestQuestions)
            .WithOne(tq => tq.Test)
            .HasForeignKey(tq => tq.TestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}