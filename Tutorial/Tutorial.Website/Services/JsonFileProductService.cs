using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Tutorial.Website.Models;
using Microsoft.AspNetCore.Hosting;

namespace Tutorial.Website.Services
{
    /// <summary>
    /// Service provides a list of all the products available
    /// </summary>
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<Product> GetProducts()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);
            return JsonSerializer.Deserialize<Product[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public void AddRating(string productId, int newRating)
        {
            IEnumerable<Product> products = GetProducts();

            // Grab the first product that matches the id passed in
            Product query = products.First(x => x.Id == productId);

            // TODO Handle unknown product

            // Create ratings if none exist
            if (query.Ratings == null)
            {
                query.Ratings = new int[] { newRating };
            }
            else
            {
                List<int> ratings = query.Ratings.ToList();
                ratings.Add(newRating);
                query.Ratings = ratings.ToArray();
            }

            using (var outputStream = File.OpenWrite(JsonFileName))
            {
                JsonSerializer.Serialize
                    (
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        products
                    );
            }
        }
    }
}
