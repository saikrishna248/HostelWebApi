using System.ComponentModel.DataAnnotations;

namespace HostelWebApi.Models
{
    public class SalesDto
    {
        [Required]
        public string ProductCategory { get; set; }
        public int Q1Sales { get; set; }
        public int Q2Sales { get; set; }
        public int Q3Sales { get; set; }
        public int Q4Sales { get; set; }
        public int Total { get; set; }
     
    }
}
