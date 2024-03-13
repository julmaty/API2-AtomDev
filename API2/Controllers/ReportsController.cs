using System;
using System.Collections.Generic;
using System.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace API2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public ReportsController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;

        }

        // GET: ReportsSender
        [HttpGet]
        public async Task<ActionResult<List<ReportShortViewModel>>> Index()
        {
            //можно заменить на include позже
            List<Report> reports = await _context.Reports.ToListAsync();
            List<ReportShortViewModel> models = new List<ReportShortViewModel>(reports.Count);
            ReportDescription reportDescription;
            foreach (Report report in reports){
                ReportShortViewModel reportView = new ReportShortViewModel();
                reportDescription = await _context.ReportDescription.FindAsync(report.ReportDescription_Id);
                reportView.Id = report.Id;
                reportView.SenderName = report.SenderName;
                reportView.ReportName = report.ReportName;
                reportView.DateCreated = report.DateCreated;
                reportView.Description =  reportDescription.Description;
                reportView.Status = report.Status;
                models.Add(reportView);
            }
            
            return models;
            
                //return View(await _context.Report.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult<AddNewReportModel>> AddNewReport(AddNewReportModel model)
        {
                ReportTextContent reportTextContent = new ReportTextContent { TextContent = model.textContent };
                await _context.ReportTextContents.AddAsync(reportTextContent);
            await _context.SaveChangesAsync();
            Report report = new Report
                {
                    SenderName = model.senderName,
                    ReportName = model.reportName,
                    DateCreated = DateTime.Now,
                    TextContent_Id = reportTextContent.Id,
                    ReportDescription_Id = model.description,
                    Status = 0

                };
                reportTextContent.ReportId = report.Id;

                if (model.uploadedFile != null)
                {

                report.Filename = await AddFile(model.uploadedFile);
                report.Filesize = model.uploadedFile.Length;
                }
            await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();

            reportTextContent.ReportId = report.Id;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = report.Id }, report);
        }

        // GET: ReportsSender/Details/5
        [Route("datails")]
        public async Task<ActionResult<Report>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Reports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return report;
        }



        [Route("AddFile")]
        public async Task<string> AddFile(IFormFile uploadedFile)
        {
            string path = "";
            string name = "";
            if (uploadedFile != null)
            {
                // путь к папке Files
                name = uploadedFile.FileName;
                path = _appEnvironment.WebRootPath + "/files/" + name;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }

            return name;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<ReportFullViewModel>> ViewReport(int id)
        {
            //можно заменить на include позже
            //var report = _context.Report.Include(p => p.).Where(p => p.BlogPostId == id).FirstOrDefault();
            var report = await _context.Reports.FindAsync(id);

            if (report != null)
            {
                var reportTextContent = await _context.ReportTextContents.FindAsync(report.TextContent_Id);
                var reportDescription = await _context.ReportDescription.FindAsync(report.ReportDescription_Id);
                
                ReportFullViewModel model = new ReportFullViewModel {
                    Id = id,
                    SenderName = report.SenderName,
                    ReportName = report.ReportName,
                    TextContent = reportTextContent.TextContent,
                    DateCreated = report.DateCreated,
                    DateSent = report.DateSent,
                    DateReceived = report.DateReceived,
                    Description = reportDescription.Description,
                    Status = report.Status 
                };
                if (report.Filename != null)
                {
                    string path = _appEnvironment.WebRootPath + "/files/" + report.Filename;
                    if (System.IO.File.Exists(path))
                    {
                        model.Filename = "/files/" + report.Filename;
                    }

                }
                return model;
            }
            return NotFound();
        }


        // POST: ReportsSender/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report.Filename != null)
            {
                string path = _appEnvironment.WebRootPath + "/files/" + report.Filename;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            if ( (report != null) && (report.Status == 0) )
            {
                var reportTextContent = await _context.ReportTextContents.FindAsync(report.TextContent_Id);
                _context.ReportTextContents.Remove(reportTextContent);
                _context.Reports.Remove(report);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("reportexists")]
        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.Id == id);
        }
    }
}
