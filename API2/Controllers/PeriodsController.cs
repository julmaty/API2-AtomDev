using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API2;
using Microsoft.Extensions.Primitives;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Hosting;

namespace API2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PeriodsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Periods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeriodTableModel>>> GetPeriods()
        {
            return await _context.Periods.ToListAsync();
        }

        // GET: api/Periods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeriodTableModel>> GetPeriodTableModel(int id)
        {
            var periodTableModel = await _context.Periods.FindAsync(id);

            if (periodTableModel == null)
            {
                return NotFound();
            }

            return periodTableModel;
        }

        // PUT: api/Periods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeriodTableModel(int id, PeriodTableModel periodTableModel)
        {
            if (id != periodTableModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(periodTableModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodTableModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Periods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PeriodTableModel>> PostPeriodTableModel(List<PeriodJsonModel> periods)
        {
            List<PeriodTableModel> periods_now = _context.Periods.ToList();
            for (int i = 0; i < periods_now.Count; i++)
            {
                _context.Periods.Remove(periods_now[i]);
            }

            for (int i = 0; i < periods.Count; i++)
            {
                PeriodTableModel period = new PeriodTableModel { Speed = periods[i].Speed, To = DateTime.ParseExact(periods[i].To, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture), From = DateTime.ParseExact(periods[i].From, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture)
                };
                await _context.Periods.AddAsync(period);
            };
            await _context.SaveChangesAsync();
            return Ok("ok");
        }

        // DELETE: api/Periods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeriodTableModel(int id)
        {
            var periodTableModel = await _context.Periods.FindAsync(id);
            if (periodTableModel == null)
            {
                return NotFound();
            }

            _context.Periods.Remove(periodTableModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeriodTableModelExists(int id)
        {
            return _context.Periods.Any(e => e.Id == id);
        }
    }
}
