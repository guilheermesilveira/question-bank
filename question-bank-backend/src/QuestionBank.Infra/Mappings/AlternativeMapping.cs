using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class AlternativeMapping : IEntityTypeConfiguration<Alternative>
{
    public void Configure(EntityTypeBuilder<Alternative> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(a => a.Text)
            .IsRequired();

        builder
            .Property(a => a.IsCorrect)
            .IsRequired();

        builder
            .Property(a => a.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(a => a.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        // Relation: Alternative -> Question
        builder
            .HasOne(a => a.Question)
            .WithMany(q => q.Alternatives)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}