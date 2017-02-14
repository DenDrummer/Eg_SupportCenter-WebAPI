using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using SC.BL;
using SC.BL.Domain;

namespace SC.UI.Web.MVC.Controllers
{
    public class TicketController : BaseController
    {
        private ITicketManager mgr = new TicketManager();

        // GET: Ticket
        public ActionResult Index()
        {
            CookieMonster();
            IEnumerable<Ticket> tickets = mgr.GetTickets();
            return View(tickets);
        }

        // GET: Ticket/Details/5
        public ActionResult Details(int id)
        {
            CookieMonster();
            Ticket ticket = mgr.GetTicket(id);

            var responses = mgr.GetTicketResponses(id).ToList();

            ticket.Responses = responses;
            // OF: via ViewBag
            //ViewBag.Responses = responses;

            return View(ticket);
        }

        // GET: Ticket/Create
        public ActionResult Create()
        {
            CookieMonster();
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            CookieMonster();
            if (ModelState.IsValid)
            {
                ticket = mgr.AddTicket(ticket.AccountId, ticket.Text);

                return RedirectToAction("Details", new { id = ticket.TicketNumber });
            }

            return View();
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            CookieMonster();
            Ticket ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Ticket ticket)
        {
            CookieMonster();
            if (ModelState.IsValid)
            {
                mgr.ChangeTicket(ticket);

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(int id)
        {
            CookieMonster();
            Ticket ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            CookieMonster();
            try
            {
                mgr.RemoveTicket(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
