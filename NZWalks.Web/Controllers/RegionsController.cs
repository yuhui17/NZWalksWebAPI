using Microsoft.AspNetCore.Mvc;
using NZWalks.Model.Models.DTOs;
using NZWalks.Model.Models.VM;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace NZWalks.Web.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();

            try
            {
                // Get All Regions from WebAPI
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7001/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();//throw exception when failed

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

                return View(response);
            }
            catch (Exception ex)
            {
                //log the exception
                throw;
            }

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionVM addRegionVM)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7001/api/regions"),
                    Content = new StringContent(JsonSerializer.Serialize(addRegionVM), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();//throw exception when failed

                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if (response is not null)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7001/api/regions/{id.ToString()}");

                if (response is not null)
                {
                    return View(response);
                }

                return View(null);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7001/api/regions/{regionDTO.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();//throw exception when failed
                    
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

                if(response != null)
                {
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7001/api/regions/{id.ToString()}");

                httpResponseMessage.EnsureSuccessStatusCode();//throw exception when failed
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
