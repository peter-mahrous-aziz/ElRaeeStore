using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using elraee.Models;
using elraee.ViewModels;
using System.Security.Claims;
using elraee.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace elraee.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        ICategoryRepo catRepo;
        IProductRepo prodRepo;
        public DashboardController(ICategoryRepo catRepo, IProductRepo prodRepo)
        {
            this.catRepo = catRepo;
            this.prodRepo = prodRepo;
        }

  

        public IActionResult categoryDashBoard(string Search = "", int page = 1, string msg = null)
        {
            int pageSize = 6;
            List<category> categories = catRepo.getAllWithSearch(Search)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = (int)Math.Ceiling(catRepo.GetCount() / (double)pageSize);
            ViewBag.msg = msg;
            ViewBag.search = Search;

            return View("categoryDashBoard", categories);
        }

        public IActionResult LoadMoreCategories(int page, string Search = "")
        {
            int pageSize = 6;
            var categories = catRepo.getAllWithSearch(Search)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.NoOfPages = (int)Math.Ceiling(catRepo.GetCount() / (double)pageSize);
            return PartialView("_categoryDashBoardPartial", categories);
        }


        public async Task<IActionResult> productDashBoard(string Search = "", int page = 1 , string msg = null)
        {
            int pageSize = 6;
            List<product> products = prodRepo.getAllWithSearch(Search)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = (int)Math.Ceiling(prodRepo.GetCount() / (double)pageSize);
            ViewBag.msg = msg;

            return View("ProductsDashBoard", products);
        }


        public async Task<IActionResult> LoadMoreProducts(int page)
        {
            int pageSize = 6;
            var products = prodRepo.GetAll()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_productPartialDashBoard", products);
        }


        public IActionResult ProductsOfSpecificCategory(int id, int page = 1)
        {


            int pageSize = 6;
            List<product> products = prodRepo.GetAllProductsByCategoryId(id)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = (int)Math.Ceiling(prodRepo.GetCount() / (double)pageSize);


            //pagination
            int NoOfRecordsPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (page - 1) * NoOfRecordsPerPage;

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = NoOfPages;

            products = products.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();


            return View("ProductsDashBoard", products);
        }


        public IActionResult SearchProduct(string name, int PageNo = 1)
        {
            List<product> prods = prodRepo.getProductsByName(name);

            //pagination
            int NoOfRecordsPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(prods.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;

            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = NoOfPages;

            prods = prods.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();


            if (prods.Count > 0)
            {
                return View("ProductsDashBoard", prods);
            }
            ViewBag.controller = "dashboard";
            ViewBag.action = "productDashboard";
            return View("notFound");
        }


        public IActionResult SearchCategory(string name, int PageNo = 1)
        {
            List<category> categories = catRepo.getCategoryByName(name);

            //pagination
            int NoOfRecordsPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(categories.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;

            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = NoOfPages;

            categories = categories.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();


            if (categories.Count > 0)
            {
                return View("CategoryDashboard", categories);
            }

            ViewBag.controller = "dashboard";
            ViewBag.action = "CategoryDashboard";

            return View("notFound");
        }


    }
}