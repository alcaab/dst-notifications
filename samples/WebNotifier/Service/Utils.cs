using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNotifier.Service
{
    public static class Utils
    {

        private static readonly Dictionary<RequestStatus, string> Templates = new Dictionary<RequestStatus, string> 
        {
            {RequestStatus.None,null},
            {RequestStatus.Submited,"submited-template.t5"},
            {RequestStatus.Assigned,"assigned-template.t5"},
        };

        public static string GetTemplate(RequestStatus status) => Templates[status];



    }
}
