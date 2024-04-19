namespace ChessMaster.Infrastructure.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id)
            .HasName("PK_Users_Id");
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(x => x.Username)
            .HasMaxLength(20)
            .IsRequired();
    }
}