using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tutorial.Website.Models;
using Tutorial.Website.Services;

namespace Tutorial.Website.Pages
{
    public class IndexModel : PageModel
    {
        public readonly JsonFileProductService ProductService;
        public IEnumerable<Product> Products { get; private set; }

        private readonly ILogger<IndexModel> _logger;

        // Since we added the JsonFileProductService in Startup.ConfigureServices,
        // adding it in the constructor will ensure this class gets a valid instance
        public IndexModel(ILogger<IndexModel> logger, JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        // GET - ie from the web world like GET, POST
        public void OnGet()
        {
            Products = ProductService.GetProducts();
        }
    }
}
