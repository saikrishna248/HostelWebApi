using HostelWebApi.Data;
using HostelWebApi.Entities;
using HostelWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadSales([FromBody] SalesUploadRequest request)
        {
            if (request?.SalesData == null || !request.SalesData.Any())
                return BadRequest("No data received");

            if (string.IsNullOrEmpty(request.Username))
                return BadRequest("Username is required");
            var entities = request.SalesData.Select(x => new SalesData
            {
                ProductCategory = x.ProductCategory,
                Q1Sales = x.Q1Sales,
                Q2Sales = x.Q2Sales,
                Q3Sales = x.Q3Sales,
                Q4Sales = x.Q4Sales,
                Total = x.Q1Sales + x.Q2Sales + x.Q3Sales + x.Q4Sales,
                CreatedDate = DateTime.Now,
                CreatedBy = request.Username
            }).ToList();

            await _context.SalesData.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sales data saved successfully" });
        }
    }
}
