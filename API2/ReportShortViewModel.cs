using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API2
{
    public class ReportShortViewModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string ReportName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
    }
}
