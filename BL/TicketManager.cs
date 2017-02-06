using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using SC.DAL;
using SC.BL.Domain;
using SC.BL.Properties;
using SC.DAL.EF;

namespace SC.BL
{
    public class TicketManager : ITicketManager
    {
        private readonly ITicketRepository repo;

        public TicketManager()
        {
            //repo = new TicketRepositoryHC();
            //repo = new SC.DAL.SqlClient.TicketRepository();
            repo = new TicketRepository();
        }

        public IEnumerable<Ticket> GetTickets()
        {
            List<Ticket> tickets = repo.ReadTickets().ToList();
            foreach (Ticket t in tickets)
            {
                t.DateOpened = t.DateOpened.ToLocalTime();
            }
            return tickets;
        }

        public Ticket GetTicket(int ticketNumber)
        {
            Ticket t = repo.ReadTicket(ticketNumber);
            t.DateOpened = t.DateOpened.ToLocalTime();
            return t;
        }

        public Ticket AddTicket(int accountId, string question)
        {
            Ticket t = new Ticket()
            {
                AccountId = accountId,
                Text = question,
                DateOpened = DateTime.Now.ToUniversalTime(),
                State = TicketState.Open,
            };
            return AddTicket(t);
        }

        public Ticket AddTicket(int accountId, string device, string problem)
        {
            Ticket t = new HardwareTicket()
            {
                AccountId = accountId,
                Text = problem,
                DateOpened = DateTime.Now.ToUniversalTime(),
                State = TicketState.Open,
                DeviceName = device
            };
            return AddTicket(t);
        }

        private Ticket AddTicket(Ticket ticket)
        {
            Validate(ticket);
            return repo.CreateTicket(ticket);
        }

        public void ChangeTicket(Ticket ticket)
        {
            Validate(ticket);
            repo.UpdateTicket(ticket);
        }

        public void RemoveTicket(int ticketNumber)
        {
            repo.DeleteTicket(ticketNumber);
        }

        public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber)
        {
            List<TicketResponse> ticketResponses = repo.ReadTicketResponsesOfTicket(ticketNumber).ToList();
            foreach (TicketResponse tr in ticketResponses)
            {
                tr.Date = tr.Date.ToLocalTime();
            }
            return ticketResponses;
        }

        public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse)
        {
            Ticket ticketToAddResponseTo = GetTicket(ticketNumber);
            if (ticketToAddResponseTo != null)
            {
                // Create response
                TicketResponse newTicketResponse = new TicketResponse();
                newTicketResponse.Date = DateTime.Now.ToUniversalTime();
                newTicketResponse.Text = response;
                newTicketResponse.IsClientResponse = isClientResponse;
                newTicketResponse.Ticket = ticketToAddResponseTo;

                // Add response to ticket
                var responses = GetTicketResponses(ticketNumber);
                if (responses != null)
                    ticketToAddResponseTo.Responses = responses.ToList();
                else
                    ticketToAddResponseTo.Responses = new List<TicketResponse>();
                ticketToAddResponseTo.Responses.Add(newTicketResponse);

                // Change state of ticket
                if (isClientResponse)
                    ticketToAddResponseTo.State = TicketState.ClientAnswer;
                else
                    ticketToAddResponseTo.State = TicketState.Answered;


                // Validatie van ticketResponse en ticket afdwingen!!!
                Validate(newTicketResponse);
                Validate(ticketToAddResponseTo);

                // Bewaren naar db
                repo.CreateTicketResponse(newTicketResponse);
                repo.UpdateTicket(ticketToAddResponseTo);

                return newTicketResponse;
            }
            else
                throw new ArgumentException(Resources.Ticketnumber + ticketNumber + Resources.NotFound);
        }

        public void ChangeTicketStateToClosed(int ticketNumber)
        {
            repo.UpdateTicketStateToClosed(ticketNumber);
        }

        private void Validate(Ticket ticket)
        {
            //Validator.ValidateObject(ticket, new ValidationContext(ticket), validateAllProperties: true);

            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(ticket, new ValidationContext(ticket), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException(Resources.TicketInvalid);
        }

        private void Validate(TicketResponse response)
        {
            //Validator.ValidateObject(response, new ValidationContext(response), validateAllProperties: true);

            List<ValidationResult> errors = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(response, new ValidationContext(response), errors, validateAllProperties: true);

            if (!valid)
                throw new ValidationException(Resources.TicketResponseInvalid);
        }
    }
}