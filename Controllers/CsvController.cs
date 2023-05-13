using MapsJsonCsv.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace MapsJsonCsv.Controllers
{
    public class CsvController : Controller
    {
        //[HttpPost]
        //public async Task<IActionResult> Upload()
        //{
        //    string filePath = @"C:\Users\cakec\Videos\hello.csv";
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        using (StreamWriter writer = new StreamWriter(filePath))
        //        {
        //            writer.WriteLine("hello");
        //        }

        //        return Ok();
        //    }

        //    return BadRequest();
        //}
        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult InputProcess(InputModel model)
        {
            string apiKey = model.Apikey;
            decimal latitude = model.Latitude;
            decimal longitude = model.Longitude;
            decimal radius = model.Radius;
            string category = model.Category;
            string csvOption = model.CsvOption;
            return RedirectToAction("GetApi", new { apiKey = apiKey, latitude = latitude, longitude = longitude, radius = radius, category = category,csvOption=csvOption });
        }
        [HttpGet]
        public IActionResult GetApi(string apiKey, decimal latitude, decimal longitude, decimal radius, string category, string csvOption)
        {
            Console.WriteLine(csvOption);
            string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&type={category}&key={apiKey}";
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync(url).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;
            JObject jsonResponse = JObject.Parse(responseContent);
            var apiArray = new Apiarray();
            apiArray.results = new List<ApiModel>();
            var results = jsonResponse["results"];
            foreach (var result in results)
            {
                var apiModel = new ApiModel();
                apiModel.business_status = (string)result["business_status"];
                apiModel.lat = (decimal?)result["geometry"]["location"]["lat"];
                apiModel.lng = (decimal?)result["geometry"]["location"]["lng"];
                apiModel.name = (string?)result["name"];
                apiModel.rating = (decimal?)result["rating"];
                apiModel.user_ratings_total = (int?)result["user_ratings_total"];
                apiModel.vicinity = (string?)result["vicinity"];
                apiModel.types = result["types"].ToObject<List<string>>();
                apiArray.results.Add(apiModel);
            }

            string filePath = @"lolenterfilepath";
            if (csvOption == "append")
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    foreach (var result in apiArray.results)
                    {
                        string line = $"{result.business_status},{result.lat},{result.lng},{result.name},{result.rating},{result.user_ratings_total},\"{result.vicinity}\",{string.Join("|", result.types)}";
                        writer.WriteLine(line);
                    }
                }
            }
             else if (csvOption == "overwrite")
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("business_status,lat,lng,name,rating,user_ratings_total,vicinity,types");
                    foreach (var result in apiArray.results)
                    {
                        string line = $"{result.business_status},{result.lat},{result.lng},{result.name},{result.rating},{result.user_ratings_total},\"{result.vicinity}\",{string.Join("|", result.types)}";
                        writer.WriteLine(line);
                    }
                }
            }
            string jsonResult = JsonConvert.SerializeObject(apiArray, Formatting.Indented);
            return Content(jsonResult, "application/json");
        }




    }
}