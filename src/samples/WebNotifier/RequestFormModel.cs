using System;

namespace WebNotifier
{
    public class RequestFormModel 
    {
        public RequestFormModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ServiceDescription { get; set; }
        public string Agent { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.None;
        public DateTime? CreateTime { get; set; } 

    }
}