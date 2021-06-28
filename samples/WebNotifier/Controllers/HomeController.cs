using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification;
using Microsoft.AspNetCore.Mvc;
using WebNotifier.Service;

namespace WebNotifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly INotificationHost _notificationHost;
        private readonly IRequestFormService _requestFormService;

        public HomeController(
            INotificationHost notificationHost,
            IRequestFormService requestFormService
        )
        {
            _notificationHost = notificationHost;
            _requestFormService = requestFormService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddRequest(RequestFormModel model)
        {

            //var result = await new RequestFormValidator().ValidateAsync(model);
            //if (!result.IsValid)
            //    throw new Exception();

            _requestFormService.Add(model);

            return Ok(await SendNotification(model));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AssingRequest(string id)
        {
             var model = _requestFormService.AssignAgent(id);

             return Ok(await SendNotification(model));
        }

        [HttpPost("[action]")]
        public IActionResult Get()
        {
            return Ok(_requestFormService.GetAll());
        }


        private async Task<string> SendNotification(RequestFormModel model)
        {

            var subjectDescription = "Submited";

            var message = new NotificationMessage
            {
                NotificationMethod = NotificationMethod.External,
                Subject = $"Service Request - {subjectDescription}",
                //Body = model.Body,
                TemplateKey = Utils.GetTemplate(model.Status),
                To = new List<NotificationAddress>() { new NotificationAddress(model.Email, $"{model.FirstName} {model.LastName}") },
                Data = new Dictionary<string, object>
                {
                    { "form", model}
                }
            };

            await _notificationHost.Notify(message);

            return message.Group;
        }

    }
}