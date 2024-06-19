using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RiverMonitor.Data;

namespace RiverMonitor.Pages
{
    public class AllValuesModel : PageModel
    {
        readonly ApplicationDbContext DB;
        public List<Value> Data { get; set; }
        public Dictionary<int, Value> LatestValues { get; set; }

        public AllValuesModel(ApplicationDbContext db)
        {
            DB = db;
            LatestValues = new Dictionary<int, Value>();
        }

        public async Task OnGetAsync()
        {
            Data = await DB.Values
                .AsNoTracking()
                .Include(x => x.Station)
                .OrderByDescending(x => x.TimeStamp)
                .ToListAsync();

            // Find the latest values for each station
            LatestValues = Data
                .GroupBy(x => x.Station.Id)
                .ToDictionary(g => g.Key, g => g.First());
        }
    }
}
