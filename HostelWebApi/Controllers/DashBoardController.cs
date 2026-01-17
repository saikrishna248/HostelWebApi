using HostelWebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly AppDbContext _context;  // For data base
        public DashBoardController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("summary")]
        public IActionResult GetDashboardSummary()
        {
            // Example static data (replace with real DB data later)
            var summary = new
            {
                TotalRooms = 10,
                AvailableRooms = 6,
                OccupiedRooms = 4
            };

            return Ok(summary);
        }
    }
}
