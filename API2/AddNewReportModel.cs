using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API2
{
    public class AddNewReportModel
    {
        public string senderName { get; set; }
        public string reportName { get; set; }
        public string textContent { get; set; }
        public int description { get; set; }
        public IFormFile? uploadedFile { get; set; }
    }
}
