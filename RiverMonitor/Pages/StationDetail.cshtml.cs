using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RiverMonitor.Data;
using System.Text.Json;

namespace RiverMonitor.Pages
{
    public class StationDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Station Data { get; set; }
        public Value? LatestValue { get; set; }

        readonly ApplicationDbContext DB;

        public StationDetailModel(ApplicationDbContext db)
        {
            DB = db;
        }
        public async Task OnGetAsync()
        {
            Data = await DB.Stations
                .AsNoTracking()
                .Include(x => x.Values.OrderByDescending(v => v.TimeStamp))
                .FirstOrDefaultAsync(x => x.Id == Id);

            LatestValue = Data?.Values.FirstOrDefault();
        }
        public string GetValuesAsJson()
        {
            var values = Data?.Values.Select(v => new
            {
                Val = v.Val,
                TimeStamp = v.TimeStamp
            }).ToList();

            return JsonSerializer.Serialize(values);
        }
    }
}
