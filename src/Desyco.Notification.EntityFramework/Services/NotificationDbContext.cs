using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public abstract class NotificationDbContext : DbContext
    {

        protected abstract void ConfigureMessageStorage(EntityTypeBuilder<MessageEntity> builder);
        protected abstract void ConfigureDeliveryErrorStorage(EntityTypeBuilder<DeliveryErrorEntity> builder);
        protected abstract void ConfigureAttachmentStorage(EntityTypeBuilder<AttachmentEntity> builder);


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(p => p.Id).HasMaxLength(36);
                entity.Property(p => p.Group).HasMaxLength(36);
                entity.Property(x => x.Subject).HasMaxLength(200);
                entity.Property(x => x.TemplateKey).HasMaxLength(255);


                entity.HasMany(x=> x.Errors)
                    .WithOne()
                    .HasPrincipalKey(fk => fk.Id)
                    .HasForeignKey(ur => ur.MessageId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasMany(x=> x.Attachments)
                    .WithOne()
                    .HasPrincipalKey(fk => fk.Id)
                    .HasForeignKey(ur => ur.MessageId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                ConfigureMessageStorage(entity);
            });

            modelBuilder.Entity<DeliveryErrorEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.MessageId);
                entity.Property(p => p.Id).HasMaxLength(36);
                entity.Property(p => p.MessageId).HasMaxLength(36);
                entity.Property(x => x.Message);

                ConfigureDeliveryErrorStorage(entity);
            });

            modelBuilder.Entity<AttachmentEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.MessageId);
                entity.Property(p => p.Id).HasMaxLength(36);
                entity.Property(p => p.MessageId).HasMaxLength(36);
                entity.Property(x => x.FileName).HasMaxLength(255);
                entity.Property(x => x.MediaType).HasMaxLength(30);
                entity.Property(x => x.MediaSubType).HasMaxLength(30);

                ConfigureAttachmentStorage(entity);

            });


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
