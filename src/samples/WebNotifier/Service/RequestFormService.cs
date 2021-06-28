using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotifier.Service
{
    public interface IRequestFormService
    {
        void Add(RequestFormModel model);
        RequestFormModel AssignAgent(string id);
        List<RequestFormModel> GetAll();
    }

    public class RequestFormService : IRequestFormService
    {
        private readonly List<RequestFormModel> _data;

        public RequestFormService()
        {
            _data = new List<RequestFormModel>();
        }

        public void Add(RequestFormModel model)
        {

            model.Agent = GetRandomAgentName();
            model.CreateTime = DateTime.UtcNow;
            model.Status = RequestStatus.Submited;

            _data.Add(model);
        }

        public RequestFormModel GetById(string id)
        {
            return _data.FirstOrDefault(f => f.Id == id);
        }

        public RequestFormModel AssignAgent(string id)
        {
            var item = GetById(id);
            if (item == null)
                throw new NullReferenceException();

            if(item.Status != RequestStatus.Submited)
                throw new Exception("Invalida request status");

            item.Agent = GetRandomAgentName();
            item.Status = RequestStatus.Assigned;

            return item;
        }

        public List<RequestFormModel> GetAll()
        {
            return _data;
        }

        private string GetRandomAgentName()
        {
            var names = new List<string>
            {
                "Juan",
                "Pedro",
                "Maria",
                "Mario",
                "Adela"
            };

           var index = new Random().Next(0, names.Count- 1);
           return names[index];
        }


    }
}
