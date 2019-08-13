using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGoOutDemo.ServiceFabric.Actors.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace LetsGoOutDemo.ServiceFabric.WebApi.Controllers
{
    public class AppointmentsController : Controller
    {
        private const string NickNameHeaderName = "x-nick-name";

        [HttpPost]
        [Route("api/new-appointment")]
        public async Task<IActionResult> NewAppointment()
        {
            var nickNames = (await this.Request.ReadAsStringAsync())
                .Split(',')
                .Select(n => n.Trim())
                .ToList();

            string initiatorNickName = this.Request.Headers[NickNameHeaderName];
            if (string.IsNullOrEmpty(initiatorNickName))
            {
                // The initiator's nickName should always be send via that header
                return this.Unauthorized();
            }

            // Also adding the initiator to the list
            nickNames.Add(initiatorNickName);

            // Creating new appointment instance
            var appointmentActor = ActorProxy.Create<IAppointmentActor>(new ActorId(Guid.NewGuid()));

            await appointmentActor.InitializeParticipantsAsync(new HashSet<string>(nickNames));

            return this.Ok();
        }

        [HttpPost]
        [Route("api/appointments/{appointmentId}")]
        public async Task<IActionResult> RespondToAppointment(Guid appointmentId)
        {
            var status = Enum.Parse<AppointmentStatusEnum>(await this.Request.ReadAsStringAsync());
            if (status == AppointmentStatusEnum.Pending)
            {
                return this.BadRequest("Pending status not allowed");
            }

            string nickName = this.Request.Headers[NickNameHeaderName];

            // Obtaining appointment instance
            var appointmentActor = ActorProxy.Create<IAppointmentActor>(new ActorId(appointmentId));

            await appointmentActor.SetResponseAsync(nickName, (status == AppointmentStatusEnum.Accepted));

            return this.Ok();
        }
    }
}
