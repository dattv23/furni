using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin + "," + SystemDefinitions.Role_Employee)]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Calculates monthly earnings
        public async Task<decimal> CalculateMonthlyEarningsAsync()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var earnings = await _context.Orders
                .Where(o => o.OrderDate >= startOfMonth && !o.IsDeleted && o.Status == "Delivered")
                .SumAsync(o => o.TotalAmount);

            return earnings;
        }

        // Calculates annual earnings
        public async Task<decimal> CalculateAnnualEarningsAsync()
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var earnings = await _context.Orders
                .Where(o => o.OrderDate >= startOfYear && !o.IsDeleted && o.Status == "Delivered")
                .SumAsync(o => o.TotalAmount);

            return earnings;
        }

        // Counts pending requests
        public async Task<int> CountPendingRequestsAsync()
        {
            var count = await _context.Orders
                .Where(o => !o.IsDeleted)
                .CountAsync(o => o.Status == "Pending");

            return count;
        }

        // Calculate the percentage of orders with a status different from "Pending"
        public async Task<double> CalculatePercentageOfNonPendingOrdersAsync()
        {
            var totalOrdersCount = await _context.Orders
                .Where(o => !o.IsDeleted)
                .CountAsync();

            if (totalOrdersCount == 0)
            {
                return 0; // Avoid division by zero if there are no orders
            }

            var nonPendingOrdersCount = await _context.Orders
                .Where(o => !o.IsDeleted)
                .CountAsync(o => o.Status != "Pending");

            // Calculate the percentage
            var percentage = (double)nonPendingOrdersCount / totalOrdersCount * 100;

            return percentage;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.MonthlyEarnings = await CalculateMonthlyEarningsAsync();
            ViewBag.AnnualEarnings = await CalculateAnnualEarningsAsync();
            ViewBag.PendingRequests = await CountPendingRequestsAsync();
            ViewBag.Tasks = await CalculatePercentageOfNonPendingOrdersAsync();
            return View();
        }
    }
}
