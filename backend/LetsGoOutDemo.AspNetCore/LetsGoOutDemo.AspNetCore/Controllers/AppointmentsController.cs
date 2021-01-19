using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;
using StackExchange.Redis;

namespace LetsGoOutDemo.AspNetCore
{
    /// <summary>
    /// Implements the REST API for handling the appointment negotiation process
    /// </summary>
    public class AppointmentsController : Controller
    {
        private const string NickNameHeaderName = "x-nick-name";
        private const string AppointmentStateChangedClientHandlerName = "appointment-state-changed";
        private const string ParticipantsSetSuffix = "_participants";
        private const string ParticipantsAcceptedSetSuffix = "_participants_accepted";

        private readonly IServiceHubContext hubContext;
        private readonly IDatabase redis;

        public AppointmentsController(Task<IServiceHubContext> serviceHubContextTask, ConnectionMultiplexer redisConnection)
        {
            this.hubContext = serviceHubContextTask.Result;
            this.redis = redisConnection.GetDatabase();
        }

        [HttpPost]
        [Route("api/new-appointment")]
        public async Task<IActionResult> NewAppointment()
        {
            var participants = (await this.Request.ReadAsStringAsync())
                .Split(',')
                .Select(n => n.Trim())
                .ToList();

            string initiatorNickName = this.Request.Headers[NickNameHeaderName];
            if (string.IsNullOrEmpty(initiatorNickName))
            {
                // The initiator's nickName should always be sent via that header
                return this.Unauthorized();
            }

            // Also adding the initiator to the list
            if(!participants.Contains(initiatorNickName))
            {
                participants.Add(initiatorNickName);
            }

            // Creating new appointment instance
            var appointmentId = Guid.NewGuid();
            await this.redis.SetAddAsync(appointmentId + ParticipantsSetSuffix, participants.ToRedisValues());

            // Notifying everyone
            await this.NotifyParticipantsAsync(appointmentId.ToString(), participants.ToArray(), AppointmentStatusEnum.Pending);
            return this.Ok();
        }

        [HttpPost]
        [Route("api/appointments/{appointmentId}")]
        public async Task<IActionResult> RespondToAppointment(Guid appointmentId)
        {
            string nickName = this.Request.Headers[NickNameHeaderName];

            var status = Enum.Parse<AppointmentStatusEnum>(await this.Request.ReadAsStringAsync());
            if (status == AppointmentStatusEnum.Pending)
            {
                return this.BadRequest("Pending status not allowed");
            }

            var participants = (await this.redis.SetMembersAsync(appointmentId + ParticipantsSetSuffix)).FromRedisValues();

            // Just to make sure this response came from one of participants
            if (!participants.Contains(nickName))
            {
                return this.Ok();
            }

            // If someone has rejected...
            if (status == AppointmentStatusEnum.Declined)
            {
                await this.NotifyParticipantsAsync(appointmentId.ToString(), participants.ToArray(), AppointmentStatusEnum.Declined);
                return this.Ok();
            }

            // Adding to the set of participants accepted
            await this.redis.SetAddAsync(appointmentId + ParticipantsAcceptedSetSuffix, nickName);

            var notAccepted = await this.redis.SetCombineAsync(SetOperation.Difference, 
                appointmentId + ParticipantsSetSuffix, 
                appointmentId + ParticipantsAcceptedSetSuffix);

            // If everybody have accepted
            if (notAccepted.Length <= 0)
            {
                await this.NotifyParticipantsAsync(appointmentId.ToString(), participants.ToArray(), AppointmentStatusEnum.Accepted);
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("api/check")]
        public async Task<IActionResult> HealthCheck()
        {
            return this.Ok(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        /// <summary>
        /// Sends SignalR change notifications to all participants of the appointment
        /// </summary>
        private async Task NotifyParticipantsAsync(string appointmentId, string[] nickNames, AppointmentStatusEnum status)
        {
            foreach (string nickName in nickNames)
            {
                await this.hubContext.Clients.User(nickName).SendCoreAsync(AppointmentStateChangedClientHandlerName,
                    new[]
                    {
                        new
                        {
                            id = appointmentId,
                            participants = nickNames,
                            status
                        }
                    }
                );
            }
        }
    }
}
