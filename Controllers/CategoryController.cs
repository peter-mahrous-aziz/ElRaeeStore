using Application.Helpers;
using elraee.IRepository;
using elraee.Models;
using elraee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.VisualBasic;

namespace elraee.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepo catRepo;
        IProductRepo prodRepo;
        IImageAsStringRepo imgRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;//to detect rootPath if needed to delete from it

        public CategoryController(ICategoryRepo catRepo, IWebHostEnvironment webHostEnvironment ,
                                  context context , IProductRepo productRepo , IImageAsStringRepo imageAsStringRepo)
        {
            this.catRepo = catRepo;
            _webHostEnvironment = webHostEnvironment;
            this.prodRepo = productRepo;
          
            this.imgRepo = imageAsStringRepo;
        }


        public async Task<IActionResult> Index(string Search = "" , int page = 1)
        {
            int pageSize = 6; 
           List<category> categories = catRepo.getAllWithSearch(Search)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = (int)Math.Ceiling(catRepo.GetCount() / (double)pageSize);
            ViewBag.Search = Search; 

            return View("allCategories", categories);
        }

   
        public async Task<IActionResult> LoadMore(int page , string Search = "")
        {
            int pageSize = 6; 
            var categories = catRepo.getCategoryByName(Search)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_CategoryPartial", categories);
        }

        

        [Authorize(Roles = "admin")]
        public IActionResult addCategory()
        {
            return View("addCategoryView");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> add(addCategoryViewModel CVM )
        {
            string msgg;
            category cat = new category(); ;
            if (ModelState.IsValid)
            {
                
                cat.name = CVM.name;
                cat.img = await ImageSavingHelper.SaveOneImageAsync(CVM.img , "categoryPics");

                catRepo.Add(cat);
                catRepo.Save();
                msgg = "تم الاضافه بنجاح";
                return RedirectToAction("update", new { id = cat.id, msg = msgg }); 
            }
         
            else
            {
                ViewBag.msg = "تأكد من صحة البيانات";
                return View("addCategoryView",CVM);
            }
            
        }


      
        
        [HttpGet] //why not post? as we take it from <a> and it's always get method
        [Authorize(Roles ="admin")]
        public IActionResult update(int id , string msg)
        {
            category cat = catRepo.GetById(id);
            updateCategoryViewModel temp = new updateCategoryViewModel { name = cat.name,id = cat.id , oldImg=cat.img};

            ViewBag.msg = msg;
              
            return View("updateCategoryView",temp);

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> saveUpdate(updateCategoryViewModel CVM)
        {
            category cat;
            if(ModelState.IsValid)
            {
                cat = catRepo.GetById(CVM.id);

                cat.name = CVM.name;
                if (CVM.img != null) 
                {
                    cat.img = await ImageSavingHelper.SaveOneImageAsync(CVM.img, "categoryPics");
                    ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{CVM.oldImg}");
                }

                else //no need to update old pic
                {
                    cat.img = CVM.oldImg;
                }


                catRepo.Update(cat);
                catRepo.Save();
                CVM.oldImg = cat.img;
                ViewBag.msg = "تم التعديل بنجاح";
                return View("updateCategoryView", CVM);
            }

            else
            {
                ViewBag.msg = "تأكد من صحة البيانات";
                return View("updateCategoryView", CVM);

            }
         
          
        }

        [HttpGet] 
        [Authorize(Roles = "admin")]
        public IActionResult deleteCategory(int id)
        {
            string msgg;
            if (catRepo.GetById(id)!=null)
            {
                category temp = catRepo.GetCatWithProdsById(id); //used to delete photo from rootFiles
                if(temp.Products!=null)
                {
                    prodRepo.DeleteAllProducts(temp.Products);
                    prodRepo.Save();
                }

                catRepo.Delete(id);
                catRepo.Save();
                ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{temp.img}");
                 msgg = "تم الحذف بنجاح";
            }

            else {  msgg = "هذا العنصر غير موجود بالفعل"; }

            return RedirectToAction("categoryDashboard", "dashboard", new { msg = msgg });
        }



        public IActionResult Search(string name, int PageNo = 1)
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
                ViewBag.Search = name;
                return View("allCategories", categories);
            }

            ViewBag.controller = "category";
            ViewBag.action = "index";
            return View("notFound");
        }

        [HttpGet]
        public IActionResult categoryList()
        {
            List<category> categories =  catRepo.GetAll();
           List<categoryNameWithIDViewModel>catList = new List<categoryNameWithIDViewModel>();
            foreach (category cat in categories)
            {
                catList.Add(new categoryNameWithIDViewModel{ categorName = cat.name, Id = cat.id });
            }

            return Json(catList);
        }



     

          

        }


    }

