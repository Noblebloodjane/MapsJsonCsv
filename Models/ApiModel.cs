namespace MapsJsonCsv.Models
{
    public class Apiarray
    {
        public List<ApiModel> results { get; set; }
    }

    public class ApiModel
    {
        public string? business_status { get; set; }
        public decimal? lat { get; set;}
        public decimal? lng { get; set;}
        public string name { get; set;}
        public decimal? rating { get; set;}
        public int? user_ratings_total { get; set;}
        public string? vicinity { get; set; }
        public List<string> types { get; set; }
    }

    public class KeyModel
    {
        public string key { get; set; }
    }

    public class InputModel
    {
        public string Apikey { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Radius { get; set; }
        public string Category { get; set; }
        public string CsvOption { get; set; }
        //public IFormFile File { get; set; }
    }

}
