using Desyco.Notification.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desyco.Notification.SqlServerStorage
{
    public class SqlServerNotificationDbContext : NotificationDbContext
    {
        private readonly string _connectionString;

        public SqlServerNotificationDbContext(string connectionString)
            : base()
        {
            //TODO:Evaluar si necesito MARS aquí
            //if (!connectionString.Contains("MultipleActiveResultSets"))
            //    connectionString += ";MultipleActiveResultSets=True";

            _connectionString = connectionString;
        }



        protected override void ConfigureMessageStorage(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("Notification", "dls");

            builder.Property("_from").HasColumnName("From");
            builder.Property("_to").HasColumnName("To");
            builder.Property("_data").HasColumnName("Data");
            builder.Property("_replyTo").HasColumnName("ReplyTo");

            builder.Ignore(p => p.From);
            builder.Ignore(p => p.To);
            builder.Ignore(p => p.Data);
            builder.Ignore(p => p.ReplyTo);
        }

        protected override void ConfigureDeliveryErrorStorage(EntityTypeBuilder<DeliveryErrorEntity> builder)
        {
            builder.ToTable("NotificationError", "dls");

        }

        protected override void ConfigureAttachmentStorage(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder.ToTable("NotificationAttachment", "dls");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                _connectionString, 
                x => x.MigrationsHistoryTable("NotificationMigrationsHistory", "dls"));
        }

    }
}
