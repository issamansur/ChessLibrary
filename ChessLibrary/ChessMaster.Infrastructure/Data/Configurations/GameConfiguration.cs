namespace ChessMaster.Infrastructure.Data.Configurations;

public class GameConfiguration: IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(x => x.Id)
            .HasName("PK_Games_Id");
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .IsRequired();
        
        builder.Property(x => x.GameState)
            .HasColumnName("State")
            .IsRequired();
        
        builder.Property(x => x.Fen)
            .HasColumnName("FEN")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.WhitePlayerId)
            .HasColumnName("White_Player_Id");
        
        builder.Property(x => x.BlackPlayerId)
            .HasColumnName("Black_Player_Id");
        
        builder.Property(x => x.WinnerId)
            .HasColumnName("Winner_Id");
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.Id)
            .HasConstraintName("FK_Games_Users_UserId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}