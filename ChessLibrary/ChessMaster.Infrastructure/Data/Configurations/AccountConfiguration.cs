namespace ChessMaster.Infrastructure.Data.Configurations;

public class AccountConfiguration: IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.UserId)
            .HasName("PK_Accounts_UserId");
        
        builder.Property(x => x.UserId)
            .HasColumnName("User_Id")
            .IsRequired();

        builder.Property(x => x.NormalizedEmail)
            .HasColumnName("Email")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Account>(x => x.UserId)
            .HasConstraintName("FK_Accounts_Users_UserId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}