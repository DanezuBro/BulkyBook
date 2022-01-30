
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                //ModelState.AddModelError("Name","Name este identic cu Display Order.");
                //ModelState.AddModelError("DisplayOrder", "DisplayOrder este identic cu Name.");
                ModelState.AddModelError("CustomError", "Cele 2 valori nu trebuie sa fie identice.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Category was not created.";
            return View(obj);

        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id==0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
                return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                //ModelState.AddModelError("Name","Name este identic cu Display Order.");
                //ModelState.AddModelError("DisplayOrder", "DisplayOrder este identic cu Name.");
                ModelState.AddModelError("CustomError", "Cele 2 valori nu trebuie sa fie identice.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Category was not updates.";
            return View(obj);

        }

        //GET
        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST (int? id)
        {
            //var obj = _db.Categories.Find(id);
            var obj = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);

            if (obj==null)
            {
                TempData["error"] = "Category was not deleted.";
                return NotFound();
            }
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
