using BlazorDateRangePicker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelMS.Frontend.Pages
{
    public class IndexModel : PageModel
    {
        public DateTimeOffset SelectedStartDate { get; set; }
        public DateTimeOffset SelectedEndDate { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void HandleRangeSelect(DateRange selectedDates)
        {
            SelectedStartDate = selectedDates.Start;
            SelectedEndDate = selectedDates.End;
        }
    }
}
