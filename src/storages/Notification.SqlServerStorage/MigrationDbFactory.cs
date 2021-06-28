using Microsoft.EntityFrameworkCore.Design;

namespace Desyco.Notification.SqlServerStorage
{
    public class MigrationDbFactory : IDesignTimeDbContextFactory<SqlServerNotificationDbContext>
    {
        public SqlServerNotificationDbContext CreateDbContext(string[] args)
        {
            return new SqlServerNotificationDbContext(@"Server=.;Database=NotificationDb;Trusted_Connection=True;");
        }
    }
}