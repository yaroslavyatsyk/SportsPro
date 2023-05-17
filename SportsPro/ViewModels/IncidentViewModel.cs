using SportsPro.Models;

namespace SportsPro.ViewModels
{
    public class IncidentViewModel
    {
        public int IncidentID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string CustomerName { get; set; }
        public string TechnicianName { get; set; }

        public string ProductName { get; set; }

        public DateTime dateOpened { get; set; }

    }
}
