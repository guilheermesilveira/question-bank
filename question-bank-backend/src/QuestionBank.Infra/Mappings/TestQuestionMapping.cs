using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class TestQuestionMapping : IEntityTypeConfiguration<TestQuestion>
{
    public void Configure(EntityTypeBuilder<TestQuestion> builder)
    {
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