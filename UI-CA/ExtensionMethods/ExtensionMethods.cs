using System;

using SC.BL.Domain;
using SC.UI.CA.Properties;

namespace SC.UI.CA.ExtensionMethods
{
    internal static class ExtensionMethods
    {
        internal static string GetInfo(this Ticket t)
        {
            return $"[{t.TicketNumber}] {t.Text} ({(t.Responses == null ? 0 : t.Responses.Count)} {Resources.Responses})";
        }

        internal static string GetInfo(this TicketResponse r)
        {
            return $"{r.Date:dd/MM/yyyy} {r.Text}{(r.IsClientResponse ? $" ({Resources.Client})" : "")}";
        }
    }
}