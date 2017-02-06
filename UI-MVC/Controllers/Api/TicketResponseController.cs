using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Models;
using SC.UI.Web.MVC.App_GlobalResources;

namespace SC.UI.Web.MVC.Controllers.Api
{
    public class TicketResponseController : ApiController
    {
        private ITicketManager mgr = new TicketManager();

        public IHttpActionResult Get(int ticketNumber)
        {
            IEnumerable<TicketResponse> responses = mgr.GetTicketResponses(ticketNumber);

            if (responses == null || responses.Count() == 0)
                return StatusCode(HttpStatusCode.NoContent);
            
            return Ok(responses);
        }

        public IHttpActionResult Post(NewTicketResponseDTO response)
        {
            TicketResponse createdResponse = mgr.AddTicketResponse(response.TicketNumber, response.ResponseText, response.IsClientResponse);

            if (createdResponse == null)
                return BadRequest(Resources.BadRequest);

            //// Circulaire referentie!! (TicketResponse <-> Ticket) -> can't be serialized!!
            //return CreatedAtRoute("DefaultApi",
            //                      new { Controller = "TicketResponse", id = createdResponse.Id },
            //                      createdResponse);

            // Gebruik DTO (Data Transfer Object)
            TicketResponseDTO responseData = new TicketResponseDTO()
            {
                Id = createdResponse.Id,
                Text = createdResponse.Text,
                Date = createdResponse.Date,
                IsClientResponse = createdResponse.IsClientResponse,
                TicketNumberOfTicket = createdResponse.Ticket.TicketNumber
            };

            return CreatedAtRoute("DefaultApi",
                                  new { Controller = "TicketResponse", id = responseData.Id },
                                  responseData);
        }
    }
}
