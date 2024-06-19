using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RiverMonitor.Data;

namespace RiverMonitor.Pages
{
    public class StationsModel : PageModel
    {
        readonly ApplicationDbContext DB;
        public List<Station> Data { get; set; }
        public Dictionary<int, Value> LatestValues { get; set; }
        public Dictionary<int, Value> PreviousValues { get; set; }

        public StationsModel(ApplicationDbContext db)
        {
            DB = db;
            LatestValues = new Dictionary<int, Value>();
            PreviousValues = new Dictionary<int, Value>();
        }

        public async Task OnGetAsync()
        {
            Data = await DB.Stations
                .OrderBy(x => x.Title)
                .Include(x => x.Values.OrderByDescending(v => v.TimeStamp)) // Sort by most recent values
                .ToListAsync();

            // Get the latest and previous values for each station
            LatestValues = Data
                .ToDictionary(
                    s => s.Id,
                    s => s.Values.FirstOrDefault() // The latest value
                );

            PreviousValues = Data
                .ToDictionary(
                    s => s.Id,
                    s => s.Values.Skip(1).FirstOrDefault() // Previous value (second most recent)
                );
        }
    }
}
