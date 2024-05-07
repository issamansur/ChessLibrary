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
        
        builder.Property(x => x.CreatorUserId)
            .HasColumnName("Creator_User_Id")
            .IsRequired();
        
        builder.Property(x => x.CreationTime)
            .HasColumnName("Creation_Time")
            .IsRequired();
        
        builder.Property(x => x.Fen)
            .HasColumnName("Fen")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.GameState)
            .HasColumnName("State")
            .IsRequired();
        
        builder.Property(x => x.WhitePlayerId)
            .HasColumnName("White_Player_Id");
        
        builder.Property(x => x.BlackPlayerId)
            .HasColumnName("Black_Player_Id");
        
        builder.Property(x => x.StartTime)
            .HasColumnName("Start_Time");
        
        builder.Property(x => x.EndTime)
            .HasColumnName("End_Time");
        
        builder.Property(x => x.WinnerId)
            .HasColumnName("Winner_Id");
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.CreatorUserId)
            .HasConstraintName("FK_Games_Users_CreatorUserId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.WhitePlayerId)
            .HasConstraintName("FK_Games_Users_WhitePlayerId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.BlackPlayerId)
            .HasConstraintName("FK_Games_Users_BlackPlayerId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}