using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalCalendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        // GET: api/Calendar
        [HttpGet]
        public IEnumerable<string> Get()
        {
           // insertCalendar();
            return new string[] { "value1", "value2" };
        }

        // GET: api/Calendar/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Calendar
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Calendar/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public void insertCalendar()
        {
            string ApplicationName="OtherApp";
            //string ClientID = "431790610133-v17a2mitkvjsnuu5g0n5vjr4ack152j4.apps.googleusercontent.com";
            //string ClientSecret = "UojLcIuGcxzuKV82nIcrL2rp";
            var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read);
            UserCredential credential = null;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new string[] { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarReadonly },
                "UserID",
                CancellationToken.None,
                new FileDataStore("google calendar")).Result;
            if (credential != null)
            {
                var calendarService = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
                if (calendarService != null)
                {
                    Google.Apis.Calendar.v3.Data.Event calendarEvent = new Google.Apis.Calendar.v3.Data.Event();
                    calendarEvent.Summary = "My test event";
                    calendarEvent.Description = "event description";
                    calendarEvent.Location = "Ha noi";
                    calendarEvent.Start = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = DateTime.Now.AddDays(1).AddHours(2)
                    };
                    calendarEvent.End = new Google.Apis.Calendar.v3.Data.EventDateTime
                    {
                        DateTime = DateTime.Now.AddDays(2).AddHours(3)
                    };
                    calendarEvent.Attendees = new Google.Apis.Calendar.v3.Data.EventAttendee[]
                    {
                        new Google.Apis.Calendar.v3.Data.EventAttendee() { Email = "nguyenduysoict.2310@gmail.com", DisplayName = "duy"},
                        new Google.Apis.Calendar.v3.Data.EventAttendee() { Email = "ltt.1705@gmail.com", DisplayName = "sy"}
                    };
                    var newEvenRequest = calendarService.Events.Insert(calendarEvent, "primary");
                    var eventResult = newEvenRequest.Execute();
                    var insertedEventID = eventResult.Id;
                }
            }
        }
    }
}
