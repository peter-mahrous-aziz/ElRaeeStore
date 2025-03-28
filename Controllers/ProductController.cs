using Microsoft.AspNetCore.Mvc;
using elraee.Models;
using elraee.IRepository;
using Application.Helpers;
using elraee.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
namespace elraee.Controllers
{
    public class ProductController : Controller
    {
        IProductRepo prodRepo;
        IRelativeRepo relativeRepo;
        IImageAsStringRepo imgRepo;
        ICategoryRepo catRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;//to detect rootPath if needed to delete from it

        public ProductController(IProductRepo prodRepo, IWebHostEnvironment webHostEnvironment,
                                 IImageAsStringRepo imgRepo , ICategoryRepo catRepo ,
                                 IRelativeRepo relativeRepo)
        {
            this.prodRepo = prodRepo;
            _webHostEnvironment = webHostEnvironment;
            this.imgRepo = imgRepo;
            this.catRepo = catRepo;
            this.relativeRepo = relativeRepo;
        }

        public async Task<IActionResult> Index(string Search = "", int page = 1)
        {
            int pageSize = 6;
            List<product> products = prodRepo.getAllWithSearch(Search)
                   .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

            ViewBag.PageNo = page;
            ViewBag.NoOfPages = (int)Math.Ceiling(prodRepo.GetCount() / (double)pageSize);
            ViewBag.Search = Search; // إضافة Search لـ ViewBag

            return View("allProducts", products);
        }

        public async Task<IActionResult> LoadMore(int page, string Search = "") 
        {
            int pageSize = 6;
            var products = prodRepo.getAllWithSearch(Search) // استخدام getAllWithSearch بدل GetAll
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_productPartial", products);
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
            ViewBag.catId = id; 

            return View("allProducts", products);
        }


        public IActionResult ProductsOfSpecificCat(int id)
        {
          
            List<product> products = prodRepo.GetAllProductsByCategoryId(id);

           
            var result = products.Select(p => new { id = p.Id, name = p.Name }).ToList();

            return Json(result);
        }
        public async Task<IActionResult> LoadMoreProductsOfSpecificCategory(int id, int page)
        {
            int pageSize = 6;
            List<product> products = prodRepo.GetAllProductsByCategoryId(id)
               .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_productPartial", products);
        }


     
        [Authorize(Roles = "admin")]
        public IActionResult addProduct()
        {
            List<category> categories = catRepo.GetAll();
            List<product> products = prodRepo.GetAll();

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.id.ToString(), 
                Text = c.name
            }).ToList();
            
            return View("addProductView");
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> saveProduct(addProductViewModel PVM)
        {
            product prod = new product();
            string msgg = null;
            if (ModelState.IsValid)
            {

                prod.Name = PVM.name;
                prod.Description = PVM.Description;
                prod.brand = PVM.brand;
                prod.madeIn = PVM.madeIn;
                prod.Price = PVM.Price;
                prod.CategoryId = PVM.CategoryId;

                prod.homeImage = await ImageSavingHelper.SaveOneImageAsync(PVM.homeImage, "ProductPics");

                if (PVM.InsteadOfPrice != null)
                {
                    prod.InsteadOfPrice = PVM.InsteadOfPrice;
                }

                prodRepo.Add(prod);
                prodRepo.Save();


                if (PVM.images != null) //must be after saving product to use it's ID as FK 
                {
                    List<imageAsString> imgList = new List<imageAsString>();
                    List<string> imageNames = await ImageSavingHelper.SaveImagesAsync(PVM.images, "ProductPics");
                    for (int i = 0; i < imageNames.Count; i++)
                    {
                        imgList.Add(new imageAsString { Image = imageNames[i], productId = prod.Id });
                    }

                    imgRepo.AddListOfImages(imgList);
                    imgRepo.Save();
                }

                if (PVM.relativeProductIDs != null)
                {
                    List<relativeProducts> relativeList = new List<relativeProducts>();
                    for (int i = 0; i < PVM.relativeProductIDs.Count; i++)
                    {
                        relativeList.Add(new relativeProducts { relativeProductId = PVM.relativeProductIDs[i], parentProductId = prod.Id });
                    }

                    relativeRepo.BulkAdd(relativeList);
                    relativeRepo.Save();

                }

                msgg = "تم الاضافه بنجاح";
                return RedirectToAction("update", new { id = prod.Id, msg = msgg });
            }

            else
            {
                ViewBag.msg = "تأكد من صحة البيانات";

                List<category> categories = catRepo.GetAll();
                List<product> products = prodRepo.GetAll();

                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.id.ToString(), 
                    Text = c.name
                }).ToList();

                return View("addProductView",PVM);

            }
           
        }

        public IActionResult productView (int id)
        {
            product prod = prodRepo.GetById(id);

            return View("productDetails", prod);
        }

        [Authorize(Roles = "admin")]
        public IActionResult update(int id , string msg)
        {
            product prod = prodRepo.GetById(id);
            List<string> oldImgList = new List<string>();
            List<relativeProducts> relativeList = relativeRepo.GetRelativesByParentId(id);
            ViewBag.msg = msg;

            foreach (imageAsString img in prod.images)
            {
                oldImgList.Add(img.Image);
            }

            updateProductViewModel temp = new updateProductViewModel
            {
                name = prod.Name,
                id = prod.Id,
                Description = prod.Description,
                Price = prod.Price,
                madeIn = prod.madeIn,
                brand = prod.brand,
                categoryId = prod.CategoryId,
                oldHomeImage = prod.homeImage,
                oldImageList = oldImgList
               
            };
            List<category> categories = catRepo.GetAll();
            List<product> products = prodRepo.GetAll();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.id.ToString(), 
                Text = c.name 
            }).ToList();

            ViewBag.products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(), 
                Text = p.Name
            }).ToList();

            if (relativeList != null)
            {
                ViewBag.relatives = relativeList.Select(r => r.relativeProductId.ToString()).ToList();
            }


                return View("updateProductView", temp);

        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> saveUpdate(updateProductViewModel PVM)
        {
            product prod;
            string msgg;



            if (ModelState.IsValid)
            {
                prod = prodRepo.GetById(PVM.id);

                prod.Name = PVM.name;
                prod.Description = PVM.Description;
                prod.Price = PVM.Price;
                prod.brand = PVM.brand;
                prod.madeIn = PVM.madeIn;
                prod.CategoryId = PVM.categoryId;

                if (PVM.newHomeImage != null)
                {
                    prod.homeImage = await ImageSavingHelper.SaveOneImageAsync(PVM.newHomeImage, "ProductPics");
                    ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{PVM.oldHomeImage}");
                    PVM.oldHomeImage = prod.homeImage;
                  
                }

                if (PVM.newImageList != null)
                {
                    //remove old images
                    if (PVM.oldImageList!=null)
                    {
                        foreach (string img in PVM.oldImageList)
                        {
                            ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{img}");
                            imgRepo.DeleteWithProductId(PVM.id);//to remove pics record from db
                            imgRepo.Save();//inside the loop not outside
                        }

                        PVM.oldImageList.Clear();
                    }

                    PVM.oldImageList = new List<string>();
                    //add new imageList
                    List<string> newImgList = await ImageSavingHelper.SaveImagesAsync(PVM.newImageList , "ProductPics");
                    foreach(string img in newImgList)
                    {
                        imageAsString temp = new imageAsString{ Image = img, productId = prod.Id };
                        imgRepo.Add(temp);
                    }

                    
                    foreach (string img in newImgList)
                    {
                        PVM.oldImageList.Add(img);
                    }

                }

                if (PVM.relativeProductIDs!=null)
                {
                    //removeOldRelative
                    List<relativeProducts> relatives = relativeRepo.GetRelativesByParentId(PVM.id);
                    relativeRepo.BulkDelete(relatives);

                    List<relativeProducts> newrelativeList = new List<relativeProducts>();

                    for (int i = 0; i < PVM.relativeProductIDs.Count; i++)
                    {
                        newrelativeList.Add(new relativeProducts { relativeProductId = PVM.relativeProductIDs[i], parentProductId = PVM.id });
                    }

                    relativeRepo.BulkAdd(newrelativeList);
                    relativeRepo.Save();
                }
                else //user removed all relatives
                {
                    //removeOldRelative
                    List<relativeProducts> relatives = relativeRepo.GetRelativesByParentId(PVM.id);
                    relativeRepo.BulkDelete(relatives);
                    relativeRepo.Save();
                }



                prodRepo.Update(prod);
                prodRepo.Save();
               
                 msgg = "تم التعديل بنجاح";
               
                List<category> categories = catRepo.GetAll();
                List<product> products = prodRepo.GetAll();
                List<relativeProducts> relativeList = relativeRepo.GetRelativesByParentId(PVM.id);

                ViewBag.products = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(), // القيمة اللي هتتخزن
                    Text = p.Name
                }).ToList();
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.name
                }).ToList();

                if (relativeList != null)
                {
                    ViewBag.relatives = relativeList.Select(r => r.relativeProductId.ToString()).ToList();
                }

                return RedirectToAction("update", new { id = PVM.id, msg = msgg });

            }
            
            else
            {
                 msgg = "تأكد من صحة البيانات";
                return RedirectToAction("update", new { id = PVM.id, msg = msgg });
            }
           
        }


        public IActionResult ProductDetails (int id)
        {
            product prod = prodRepo.GetById(id);
            return View("ProductDetails", prod);
        }
       

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult deleteProduct(int id)
        {
            string msgg;
            if (prodRepo.GetById(id) != null)
            {
                product temp = prodRepo.GetById(id); //used to delete photo from rootFiles
                prodRepo.Delete(id);
                prodRepo.Save();
                ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{temp.homeImage}");
                foreach (imageAsString img in temp.images)
                {
                 ImageSavingHelper.RemoveImageFromRoot($"{_webHostEnvironment.WebRootPath}{img.Image}");
                }
               
                msgg = "تم الحذف بنجاح";
            }

            else {msgg = "هذا العنصر غير موجود بالفعل"; }

            return RedirectToAction("productDashboard", "dashboard", new { msg = msgg });
        }


        public IActionResult Search(string name, int PageNo = 1)
        {
            int pageSize = 6;
            List<product> prods = prodRepo.getProductsByName(name);

            //pagination
            int NoOfRecordsPerPage = 6;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(prods.Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (PageNo - 1) * NoOfRecordsPerPage;

            ViewBag.PageNo = PageNo;
            ViewBag.NoOfPages = (int)Math.Ceiling(prodRepo.GetCount() / (double)pageSize);
            ViewBag.Search = name; 

            prods = prods.Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage).ToList();


            if (prods.Count > 0)
            {
                return View("allProducts", prods);
            }

            return RedirectToAction("search" , "category" , new { name = name });
        }

        public IActionResult relativeProds (int parentId)
        {
            List<relativeProducts> relativeListIDs = relativeRepo.GetRelativesByParentId(parentId);
            List<RelativeViewModel> relatives = new List<RelativeViewModel>();

            foreach(relativeProducts rel in relativeListIDs)
            {
                product temp = prodRepo.GetById(rel.relativeProductId);

                relatives.Add(new RelativeViewModel { id = temp.Id , Name = temp.Name , homeImage = temp.homeImage});
            }

            return Json(relatives);

        }




    }
}

