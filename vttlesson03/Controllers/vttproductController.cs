using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using vttlesson03.Models;

namespace vttlesson03.Controllers
{
    [Route("/danh-sach-san-pham")]
    public class vttproductController : Controller
    {
        //mock data
        private readonly List<vttproduct> _products = new()
        {
                     new vttproduct
            {
                vttProductid = "BIA001",
                vttProductname = "Bia Hà Nội",
                vttYearRelease = "2020",
                vttPrice = 15000
            },
            new vttproduct
            {
                vttProductid = "BIA002",
                vttProductname = "Bia 333",
                vttYearRelease = "2019",
                vttPrice = 18000
            },
            new vttproduct
            {
                vttProductid = "BIA003",
                vttProductname = "Bia Saigon Special",
                vttYearRelease = "2018",
                vttPrice = 22000
            },
            new vttproduct
            {
                vttProductid = "BIA004",
                vttProductname = "Bia Heineken",
                vttYearRelease = "2017",
                vttPrice = 25000
            },
            new vttproduct
            {
                vttProductid = "BIA005",
                vttProductname = "Bia Tiger",
                vttYearRelease = "2019",
                vttPrice = 23000
            },
            new vttproduct
            {
                vttProductid = "RUOU001",
                vttProductname = "Rượu Vodka Hà Nội",
                vttYearRelease = "2020",
                vttPrice = 95000
            },
            new vttproduct
            {
                vttProductid = "RUOU002",
                vttProductname = "Rượu Whisky Johnnie Walker Red Label",
                vttYearRelease = "2021",
                vttPrice = 450000
            },
            new vttproduct
            {
                vttProductid = "RUOU003",
                vttProductname = "Rượu Chivas Regal 12",
                vttYearRelease = "2020",
                vttPrice = 650000
            },
            new vttproduct
            {
                vttProductid = "RUOU004",
                vttProductname = "Rượu Vang Đà Lạt",
                vttYearRelease = "2022",
                vttPrice = 180000
            },
            new vttproduct
            {
                vttProductid = "RUOU005",
                vttProductname = "Rượu Soju Chum Churum",
                vttYearRelease = "2021",
                vttPrice = 55000
            }
        };
        public IActionResult Index()
        {
            return Json(_products);
        }

        //colletion => view
        [Route("all")]
        public IActionResult vttGetAllProduct()
        {
            ViewData["products"] = _products;
            return View();
        }
    }
}
