using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RiverMonitor.Data;

namespace RiverMonitor.Areas.Editor.Pages
{
    public class StationEditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int IdStation { get; set; }

        [BindProperty]
        public Station Data { get; set; }

        readonly ApplicationDbContext DB;

        public StationEditModel(ApplicationDbContext db)
        {
            DB = db;
        }

        public async Task OnGetAsync()
        {
            if (IdStation == 0)
                Data = new Station();
            else
                Data = await DB.Stations.FindAsync(IdStation);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (IdStation != Data?.Id)
                return Page();

            // Validation: the Flood Warning must be greater than or equal to the Drought Warning
            if (Data.FloodWarningValue < Data.DroughtWarniValue)
            {
                ModelState.AddModelError(string.Empty, "Flood Warning level cannot be lower than Drought Warning level.");
                return Page(); // Returns the same page with the error
            }

            if (IdStation == 0)
                await DB.Stations.AddAsync(Data);
            else
                DB.Stations.Update(Data);

            await DB.SaveChangesAsync();
            return Redirect($"/station/{Data.Id}"); // Redirection after successful save
        }

        public async Task<IActionResult> OnPostDeleteAsync(string idSt)
        {
            if (IdStation != Convert.ToInt32(idSt))
                return Page();

            var station = await DB.Stations.FindAsync(IdStation);
            if (station != null)
            {
                DB.Stations.Remove(station);
                await DB.SaveChangesAsync();
            }

            return Redirect("/stations");
        }
    }
}
