using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class TestQuestionMapping : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
        builder
            .HasKey(tq => tq.Id);

        builder
            .Property(tq => tq.IsCorrect)
            .IsRequired();

        builder
            .Property(tq => tq.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(tq => tq.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        // Relation: TestQuestion -> Question
        builder
            .HasOne(tq => tq.Question)
            .WithMany()
            .HasForeignKey(tq => tq.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relation: TestQuestion -> Alternative
        builder
            .HasOne(tq => tq.SelectedAlternative)
            .WithMany()
            .HasForeignKey(tq => tq.SelectedAlternativeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}