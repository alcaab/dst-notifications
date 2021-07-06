﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification;
using Desyco.Notification.Models;
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


        [HttpPost("[action]")]
        public async Task<IActionResult> SubjectedNorification()
        {
            var model = SubjectedNotification();
            await _notificationHost.Notify(model);

            return Ok(model.Group);
        }


        private async Task<string> SendNotification(RequestFormModel model)
        {

            var subjectDescription = "Should be dynamic";

            var message = new NotificationMessage
            {
                NotificationMethod = NotificationMethod.External,
                Subject = $"Service Request - {subjectDescription}",
                To = new List<NotificationAddress>()
                    {new NotificationAddress(model.Email, $"{model.FirstName} {model.LastName}")},

                //Body should be null so you can use template
                //Body = model.Body,

                //Specify the template key
                TemplateKey = Utils.GetTemplate(model.Status),

                //You can send object you want available in template design
                Data = new Dictionary<string, object>
                {
                    {"form", model}
                }
            };

            //You don't have to wait for notification result
            await _notificationHost.Notify(message);


            return message.Group;
        }


        private  NotificationMessage SubjectedNotification()
        {

            var msg = new NotificationMessage
            {
                Subjects = new List<NotificationSubject>
                {
    
                    new("Request Received")
                    {
                        NotificationMethod = NotificationMethod.External,
                        PersonalizedEachRecipient = true,
                        TemplateKey = "submited-template.t5",
                        Recipients = new List<RecipientInfo>
                        {
                            new("alcaab@gmail.com", "Alexis Castro", NotificationMethod.External),
                            new("a.castro@mopc.gob.do", "Alexander Castro", NotificationMethod.InternalAndExternal)
                        }
                    },
                    new("Request Assignment")
                    {
                        NotificationMethod = NotificationMethod.Internal,
                        TemplateKey = "assigned-template.t5",
                        Recipients = new List<RecipientInfo>
                        {
                            new("supervisor@gmail.com", "Axel Vasquez"),
                        }
                    }
                }
            };


            return msg;
        }
    }

}