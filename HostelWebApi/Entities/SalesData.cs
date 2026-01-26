namespace HostelWebApi.Entities
{
    public class SalesData
    {
        public int Id { get; set; }
        public string ProductCategory { get; set; }
        public int Q1Sales { get; set; }
        public int Q2Sales { get; set; }
        public int Q3Sales { get; set; }
        public int Q4Sales { get; set; }
        public int Total { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
