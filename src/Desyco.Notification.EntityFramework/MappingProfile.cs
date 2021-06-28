using AutoMapper.EquivalencyExpression;

namespace Desyco.Notification.EntityFramework
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            //converter\
            CreateMap<MessageEntity, NotificationMessage>().ReverseMap();
            CreateMap<DeliveryErrorEntity, NotificationDeliveryError>()
                .EqualityComparison((odto, o) => odto.Id == o.Id)
                .ReverseMap(); 
            CreateMap<AttachmentEntity, NotificationAttachment>()
                .EqualityComparison((odto, o) => odto.Id == o.Id)
                .ReverseMap();
        }

    }
}
