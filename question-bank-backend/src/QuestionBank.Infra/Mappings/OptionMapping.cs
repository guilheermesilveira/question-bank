using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class OptionMapping : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder
            .HasKey(o => o.Id);

        builder
            .Property(o => o.Text)
            .IsRequired();

        builder
            .Property(o => o.IsCorrect)
            .IsRequired();

        builder
            .Property(o => o.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(o => o.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");
    }
}