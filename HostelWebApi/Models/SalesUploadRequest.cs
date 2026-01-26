namespace HostelWebApi.Models
{
    public class SalesUploadRequest
    {
        public string Username { get; set; }
        public List<SalesDto> SalesData { get; set; }
    }
}
