using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuestionBank.Domain.Entities;

namespace QuestionBank.Infra.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Name)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .IsRequired();

        builder
            .Property(u => u.Password)
            .IsRequired()
            .HasColumnType("VARCHAR(255)");

        builder
            .Property(u => u.IsAdmin)
            .IsRequired();

        builder
            .Property(u => u.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(u => u.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        // Relation: User -> SimulatedExam
        builder
            .HasMany(u => u.SimulatedExams)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}