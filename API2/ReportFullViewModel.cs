using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API2
{
    public class ReportFullViewModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReportName { get; set; }
        public string TextContent { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateReceived { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public String Filename { get; set; }
    }
}
