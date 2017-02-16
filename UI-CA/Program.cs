using System;
using System.Collections.Generic;

using SC.BL;
using SC.BL.Domain;
using SC.UI.CA.ExtensionMethods;
using SC.UI.CA.Properties;
using System.Threading;
using System.Globalization;
using System.Linq;

namespace SC.UI.CA
{
    class Program
    {
        private static bool quit = false;
        private static readonly ITicketManager mgr = new TicketManager();
        private static readonly Service srv = new Service();
        private static Settings settings = new Settings();

        static void Main(string[] args)
        {
            //zet taal
            if (!string.IsNullOrEmpty(settings.Language))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(settings.Language);
            }
            while (!quit)
                ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine(new string('=', 8 + Resources.Helpdesk.Length));
            Console.WriteLine($"=== {Resources.Helpdesk} ===");
            Console.WriteLine(new string('=', 8 + Resources.Helpdesk.Length));
            Console.WriteLine($"1) {Resources.ShowAllTickets}");
            Console.WriteLine($"2) {Resources.ShowTicketDetails}");
            Console.WriteLine($"3) {Resources.ShowTicketResponses}");
            Console.WriteLine($"4) {Resources.MakeNewTicket}");
            Console.WriteLine($"5) {Resources.MakeNewResponse}");
            Console.WriteLine($"6) {Resources.CloseTicket}");
            Console.WriteLine($"7) {Resources.ChangeLang}");
            Console.WriteLine($"0) {Resources.ShutDown}");
            try
            {
                DetectMenuAction();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(Resources.UnexpectedError);
                Console.WriteLine(e.Message);
                Console.WriteLine();
            }
        }

        private static void DetectMenuAction()
        {
            bool inValidAction = false;
            do
            {
                Console.Write(Resources.AskChoice);
                string input = Console.ReadLine();
                int action;
                if (int.TryParse(input, out action))
                {
                    switch (action)
                    {
                        case 1:
                            PrintAllTickets();
                            break;
                        case 2:
                            ActionShowTicketDetails();
                            break;
                        case 3:
                            ActionShowTicketResponses();
                            break;
                        case 4:
                            ActionCreateTicket();
                            break;
                        case 5:
                            ActionAddResponseToTicket();
                            break;
                        case 6:
                            ActionCloseTicket();
                            break;
                        case 7:
                            ActionChangeLanguage();
                            break;
                        case 0:
                            quit = true;
                            return;
                        default:
                            Console.WriteLine(Resources.InvalidChoice);
                            inValidAction = true;
                            break;
                    }
                }
            } while (inValidAction);
        }

        private static void ActionChangeLanguage()
        {
            Console.WriteLine();
            Console.WriteLine(Resources.Languages);
            Console.WriteLine(new string('-',Resources.Languages.Length));
            Console.WriteLine($"1) {Resources.nl} (Nederlands)");
            Console.WriteLine($"2) {Resources.en} (English)");
            Console.WriteLine();
            Console.Write(Resources.AskChoice);
            string choiceString = Console.ReadLine();
            Console.WriteLine();
            int choice;
            if (int.TryParse(choiceString, out choice))
            {
                switch (choice)
                {
                    case 1:
                        ChangeLanguage("nl-NL");
                        break;
                    case 2:
                        ChangeLanguage("en-EN");
                        break;
                    default:
                        Console.WriteLine(Resources.InvalidChoice);
                        Console.WriteLine();
                        return;
                }
            }
            else
            {
                Console.WriteLine(Resources.InvalidChoice);
                Console.WriteLine();
                return;
            }
        }

        private static void ChangeLanguage(string newLang)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(newLang);
            settings.Language = newLang;
            settings.Save();
            Console.WriteLine(Resources.LangChosen);
            Console.WriteLine();
        }

        private static void ActionCloseTicket()
        {
            Console.Write(Resources.AskTicketnumber);
            int input = int.Parse(Console.ReadLine());

            //mgr.ChangeTicketStateToClosed(input);
            // via WebAPI-service
            srv.ChangeTicketStateToClosed(input);
        }

        private static void PrintAllTickets()
        {
            foreach (var t in mgr.GetTickets())
                Console.WriteLine(t.GetInfo());
        }

        private static void ActionShowTicketDetails()
        {
            Console.Write(Resources.AskTicketnumber);
            int input = int.Parse(Console.ReadLine());

            Ticket t = mgr.GetTicket(input);
            PrintTicketDetails(t);
        }

        private static void PrintTicketDetails(Ticket ticket)
        {
            int[] stringLengths = {
                Resources.Ticket.Length,
                Resources.User.Length,
                Resources.Date.Length,
                Resources.State.Length,
                Resources.Device.Length,
                Resources.QuestionOrProblem.Length
            };
            int maxLength = stringLengths.Max() * -1;
            string format = string.Format("{{0,{0}}}", maxLength);
            Console.WriteLine(string.Format(format + ": {1}", Resources.Ticket, ticket.TicketNumber));
            Console.WriteLine(string.Format(format + ": {1}", Resources.User, ticket.AccountId));
            Console.WriteLine(string.Format(format + ": {1}", Resources.Date, ticket.DateOpened.ToString("dd/MM/yyyy")));
            Console.WriteLine(string.Format(format + ": {1}", Resources.State, ticket.State));

            if (ticket is HardwareTicket)
                Console.WriteLine(string.Format(format + ": {1}", Resources.Device, ((HardwareTicket)ticket).DeviceName));

            Console.WriteLine(string.Format(format + ": {1}", Resources.QuestionOrProblem, ticket.Text));
        }

        private static void ActionShowTicketResponses()
        {
            Console.Write(Resources.AskTicketnumber);
            int input = int.Parse(Console.ReadLine());

            //IEnumerable<TicketResponse> responses = mgr.GetTicketResponses(input);
            // via Web API-service
            IEnumerable<TicketResponse> responses = srv.GetTicketResponses(input);
            if (responses != null) PrintTicketResponses(responses);
        }

        private static void PrintTicketResponses(IEnumerable<TicketResponse> responses)
        {
            foreach (var r in responses)
                Console.WriteLine(r.GetInfo());
        }

        private static void ActionCreateTicket()
        {
            int accountNumber = 0;
            string problem = "";
            string device = "";

            Console.Write(Resources.AskIfHardwareIssue);
            bool isHardwareProblem = (Console.ReadLine().ToLower() == Resources.Yes);
            if (isHardwareProblem)
            {
                Console.Write(Resources.AskDeviceName);
                device = Console.ReadLine();
            }

            Console.Write(Resources.AskUsernumber);
            accountNumber = Int32.Parse(Console.ReadLine());
            Console.Write(Resources.AskProblem);
            problem = Console.ReadLine();

            if (!isHardwareProblem)
                mgr.AddTicket(accountNumber, problem);
            else
                mgr.AddTicket(accountNumber, device, problem);
        }

        private static void ActionAddResponseToTicket()
        {
            Console.Write(Resources.AskTicketnumber);
            int ticketNumber = int.Parse(Console.ReadLine());
            Console.Write(Resources.AskResponse);
            string response = Console.ReadLine();

            //mgr.AddTicketResponse(ticketNumber, response, false);
            // via WebAPI-service
            srv.AddTicketResponse(ticketNumber, response, false);
        }
    }
}