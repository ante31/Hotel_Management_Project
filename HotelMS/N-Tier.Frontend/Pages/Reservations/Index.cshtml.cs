using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelMS.Application;
using HotelMS.Application.Models.TodoItem;
using HotelMS.Application.Services;
using HotelMS.Application.Services.Impl;
using HotelMS.Core.Entities;
using HotelMS.Core.Entities.Identity;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing.Printing;

namespace HotelMS.Frontend.Pages.Reservations
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;

        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IReservationService reservationService, UserManager<ApplicationUser> userManager, IRoomService roomService)
        {
            _reservationService = reservationService;
            _userManager = userManager;
            _roomService = roomService;
        }

        public int pageIndex { get; set; } = 1;
        public bool HasPreviousPage { get; set; } = false;
        public bool HasNextPage { get; set; } = false;

        public IEnumerable<ReservationResponseModel> Reservations { get; set; } = default!;

        public async Task<PageResult> OnGetAsync(string searchString, int pageNumber, string sortString, string filterString = "none", int pageSize = 5)
        {
            ViewData["searchString"] = searchString;
            ViewData["pageSize"] = pageSize;
            ViewData["filterString"] = filterString;
            ViewData["sortString"] = sortString;

            if (User.IsInRole("Administrator"))
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    Reservations = await _reservationService.GetAllBySearchStringAsync(searchString, filterString);
                }
                else
                {
                    Reservations = await _reservationService.GetAllActiveAsync(filterString);
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = new Guid(user.Id);

                Reservations = await _reservationService.GetAllUserReservationsBySearchStringAsync(userId, searchString, filterString);
                

            }

            switch (sortString)
            {
                case "ToAsc":
                    Reservations = Reservations.OrderBy(item => item.To).ToList();
                    break;
                case "ToDesc":
                    Reservations = Reservations.OrderByDescending(item => item.To).ToList();
                    break;
                case "FromAsc":
                    Reservations = Reservations.OrderBy(item => item.From).ToList();
                    break;
                case "FromDesc":
                    Reservations = Reservations.OrderByDescending(item => item.From).ToList();
                    break;
                case "RoomAsc":
                    Reservations = Reservations.OrderBy( item => item.Room.RoomNumber).ToList();
                    break;
                case "RoomDesc":
                    Reservations = Reservations.OrderByDescending(item => item.Room.RoomNumber).ToList();
                    break;
                case "PayedAsc":
                    Reservations = Reservations.OrderBy(item => item.Payed).ToList();
                    break;
                case "PayedDesc":
                    Reservations = Reservations.OrderByDescending(item => item.Payed).ToList();
                    break;
                case "ApprovalAsc":
                    Reservations = Reservations.OrderBy(item => item.Approval).ToList();
                    break;
                case "ApprovalDesc":
                    Reservations = Reservations.OrderByDescending(item => item.Approval).ToList();
                    break;
                case "PriceAsc":
                    Reservations = Reservations.OrderBy(item => item.Price).ToList();
                    break;
                case "PriceDesc":
                    Reservations = Reservations.OrderByDescending(item => item.Price).ToList();
                    break;
            }

            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            int reservationsSize = Reservations.Count();

            Reservations = PaginatedList<ReservationResponseModel>.Create(Reservations, pageNumber, pageSize);

            if (pageNumber > 1)
            {
                HasPreviousPage = true;
            }
            if ((pageNumber * pageSize) < reservationsSize)
            {
                HasNextPage = true;
            }

            pageIndex = pageNumber;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(Guid ReservationId)
        {
            var reservation = await _reservationService.GetById(ReservationId);
            var user = await _userManager.GetUserAsync(User);
            var room = await _roomService.GetById(reservation.Room.Id);

            // PDF document and page
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Margins
            double leftMargin = 70;
            double rightMargin = page.Width - 70; // Page width minus right margin
            double verticalPosition = 70; // Initial vertical position

            // Create a font and format
            var regularFont = new XFont("Arial", 12, XFontStyle.Regular);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);
            var titleFont = new XFont("Arial", 28, XFontStyle.Bold); // Title font size
            var format = new XStringFormat
            {
                LineAlignment = XLineAlignment.Near,
                Alignment = XStringAlignment.Near
            };

            // Title
            string titleText = "Hotel Dalmatia";
            double titleWidth = gfx.MeasureString(titleText, titleFont).Width;
            double titleHeight = titleFont.GetHeight();
            double titleX = leftMargin;
            double titleY = verticalPosition;

            // Organize data into tuples
            var rowHeadingsData = new List<(string, string)>
            {
                ("Reservation Information:", ""),
                ("Reservation number:", $"{reservation.Id}"),
                ("Room:", $"{room.RoomNumber}"),
                ("Room type:", $"{room.RoomType}"),
                ("From:", $"{reservation.From.Date:yyyy-MM-dd}"),
                ("To:", $"{reservation.To.Date:yyyy-MM-dd}"),
                ("Price:", $"{reservation.Price}")
            };

            var buyerHeadingsData = new List<(string, string)>
            {
                ("Buyer Information:", ""),
                ("First name:", $"{user.FirstName}"),
                ("Last name:", $"{user.LastName}"),
                ("Email:", $"{user.Email}")
            };

            // Column widths and starting positions
            double columnWidth = (rightMargin - leftMargin) / 2;
            double leftColumnStartX = leftMargin;
            double rightColumnStartX = leftMargin + columnWidth + 10;

            // Draw row headings and data in two columns

            gfx.DrawString(titleText, titleFont, XBrushes.Black, new XRect(titleX, titleY, titleWidth, titleHeight), format);

            verticalPosition += titleHeight + 30;

            for (int i = 0; i < rowHeadingsData.Count; i++)
            {
                var (heading, data) = rowHeadingsData[i];
                var currentFont = (i == 0) ? boldFont : regularFont; // First row is bald

                gfx.DrawString(heading, currentFont, XBrushes.Black, new XRect(leftColumnStartX, verticalPosition, columnWidth, page.Height - 20), format);

                gfx.DrawString(data, currentFont, XBrushes.Black, new XRect(rightColumnStartX, verticalPosition, columnWidth, page.Height - 20), format);

                verticalPosition += currentFont.GetHeight() + 5; // Next line
            }

            verticalPosition += 10;

            for (int i = 0; i < buyerHeadingsData.Count; i++)
            {
                var (heading, data) = buyerHeadingsData[i];
                var currentFont = (i == 0) ? boldFont : regularFont; // First row is bald

                gfx.DrawString(heading, currentFont, XBrushes.Black,
                    new XRect(leftColumnStartX, verticalPosition, columnWidth, page.Height - 20), format);

                gfx.DrawString(data, currentFont, XBrushes.Black,
                    new XRect(rightColumnStartX, verticalPosition, columnWidth, page.Height - 20), format);

                verticalPosition += currentFont.GetHeight() + 5;
            }

            using (var ms = new MemoryStream())
            {
                document.Save(ms, false);
                var pdfBytes = ms.ToArray();
                return File(pdfBytes, "application/pdf", "Receipt.pdf");
            }
        }


    }
}
