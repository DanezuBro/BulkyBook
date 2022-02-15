
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        //GET
        public IActionResult Index()
        {
            //IEnumerable<Product> objProductsList = _unitOfWork.Product.GetAll();
            //return View(objProductsList);
            return View();
        }

        ////GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product created successfully.";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["error"] = "Product was not created.";
        //    return View(obj);

        //}

        //GET

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
            };

            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Employee)]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    var completeFileName = fileName+ extension;

                    if(obj.Product.imageUrl!=null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.imageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using ( var fileStreams = new FileStream(Path.Combine(uploads, completeFileName),FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.imageUrl = @"\images\products\" + completeFileName;
                }

                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Product was not created.";
            return View(obj);

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productsList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productsList});
        }

        //POST
        [HttpDelete]
        [Authorize(Roles = SD.Role_Employee)]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return Json( new { success = false, message = "Error while deleting."});
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.imageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            
            return Json(new { success = true, message = "Delete successful." });
        }
        #endregion

    }
}
