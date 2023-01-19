using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReceiptScanner.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using JsonHelper;
using Newtonsoft.Json;

namespace ReceiptScanner
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task<List<Product>> GetProductsAsync(string path)
        {
            var response = await client.GetFromJsonAsync<List<Product>>(path);

            var result = await client.GetAsync(path);

            if (result.IsSuccessStatusCode)
            {
                using var contentStream = await result.Content.ReadAsStreamAsync();
            }

            return response;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            try
            {
                var url = "https://interview-task-api.mca.dev/qr-scanner-codes/alpha-qr-gFpwhsQ8fkY1";

                // Get the products
                var products = await GetProductsAsync(url);

                ListProducts(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        static void ListProducts(List<Product> products)
        {
            products = products.OrderBy(item => item.Name).ToList<Product>();
            var domestic = products.FindAll(item => item.Domestic == true);
            var imported = products.FindAll(item => item.Domestic == false);

            double domesticCost = domestic.Sum(item => item.Price);
            double importedCost = imported.Sum(item => item.Price);

            int domesticCount = domestic.Count;
            int importedCount = imported.Count;

            Console.WriteLine(". Domestic");
            foreach (var item in domestic)
            {
                OutputProduct(item);
            }

            Console.WriteLine(". Imported");
            foreach (var item in imported)
            {
                OutputProduct(item);
            }

            OutputCost(domesticCost, importedCost);
            OutputCount(domesticCount, importedCount);
        }

        static void OutputProduct(Product product)
        {
            product.Description = product.Description.Length > 10 ? product.Description.Substring(0, 10) + "..." : product.Description;
            var weight = product.Weight != 0 ? product.Weight.ToString() + "kg" : "N/A";

            Console.WriteLine("... " + product.Name);
            Console.WriteLine("    " + "Price: " + "$" + product.Price);
            Console.WriteLine("    " + product.Description);
            Console.WriteLine("    " + "Weight: " + weight);
        }

        static void OutputCost(double domesticCost, double importedCost)
        {
            Console.WriteLine("Domestic cost: $" + string.Format("{0:N1}", domesticCost));
            Console.WriteLine("Imported cost: $" + string.Format("{0:N1}", importedCost));
        }

        static void OutputCount(int domesticCount, int importedCount)
        {
            Console.WriteLine("Domestic count: " + domesticCount);
            Console.WriteLine("Imported count: " + importedCount);
        }
    }
}
