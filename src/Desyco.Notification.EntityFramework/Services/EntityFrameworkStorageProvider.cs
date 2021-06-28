using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public class EntityFrameworkStorageProvider : IStorageProvider
    {
        private readonly bool _createDb;
        private readonly bool _migrateDb;
        private readonly INotificationDbFactory _factory;
        private readonly NotificationOptions _options;
        private readonly IMapper _mapper;

        public EntityFrameworkStorageProvider(
            INotificationDbFactory factory,
            NotificationOptions options,
            IMapper mapper,
        bool createDb, bool migrateDb)
        {
            _factory = factory;
            _options = options;
            _mapper = mapper;
            _createDb = createDb;
            _migrateDb = migrateDb;
        }



        private NotificationDbContext DbContextBuilder()
        {
            return _factory.Build();
        }

        public async Task<string> AddOrUpdateAsync(NotificationMessage m)
        {
            using (var db = DbContextBuilder())
            {

                var entity = await db
                    .Set<MessageEntity>()
                    .Include(i => i.Errors)
                    .Include(i => i.Attachments)
                    .FirstOrDefaultAsync(f => f.Id == m.Id);

                if (entity == null)
                {
                    entity = _mapper.Map<MessageEntity>(m);
                    await db.AddAsync(entity);
                }
                else
                    _mapper.Map(m, entity);

                await db.SaveChangesAsync();

                return m.Id;
            }
        }

        public async Task<List<NotificationMessage>> GetFailedNotificationsAsync()
        {
            using (var db = DbContextBuilder())
            {
                return await db
                    .Set<MessageEntity>()
                    .Include(i => i.Errors)
                    .Include(i => i.Attachments)
                    .Where(w => w.Status == MessageStatus.Error && (w.DeliveryAttempts < _options.MaxDeliveryAttempts || _options.MaxDeliveryAttempts == 0))
                    .ProjectTo<NotificationMessage>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }

        public Task<List<NotificationMessage>> GetNotificationsByUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task MarkAsReadAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void EnsureStoreExists()
        {
            using (var context = DbContextBuilder())
            {
                if (_createDb && !_migrateDb)
                {
                    context.Database.EnsureCreated();
                    return;
                }

                if (!_migrateDb)
                    return;


                context.Database.Migrate();

            }
        }
    }
}
