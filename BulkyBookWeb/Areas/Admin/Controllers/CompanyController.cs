
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET
        public IActionResult Index()
        {
            IEnumerable<Company> objCompanyList = _unitOfWork.Company.GetAll();
            return View(objCompanyList);
        }

        ////GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Company obj)
        //{
        //    //if (obj.Name == obj.DisplayOrder.ToString())
        //    //{
        //    //    //ModelState.AddModelError("Name","Name este identic cu Display Order.");
        //    //    //ModelState.AddModelError("DisplayOrder", "DisplayOrder este identic cu Name.");
        //    //    ModelState.AddModelError("CustomError", "Cele 2 valori nu trebuie sa fie identice.");
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Add(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company created successfully.";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["error"] = "Company was not created.";
        //    return View(obj);
        //}

        //GET
        //public IActionResult Edit(int? id)
        //{
        //    if(id == null || id==0)
        //    {
        //        return NotFound();
        //    }
        //    //var categoryFromDb = _db.Categories.Find(id);
        //    var companyFromDb = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //        return View(companyFromDb);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Company obj)
        //{
        //    //if (obj.Name == obj.DisplayOrder.ToString())
        //    //{
        //    //    //ModelState.AddModelError("Name","Name este identic cu Display Order.");
        //    //    //ModelState.AddModelError("DisplayOrder", "DisplayOrder este identic cu Name.");
        //    //    ModelState.AddModelError("CustomError", "Cele 2 valori nu trebuie sa fie identice.");
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company updated successfully.";
        //        return RedirectToAction("Index");
        //    }
        //    TempData["error"] = "Company was not updates.";
        //    return View(obj);

        //}

        //GET
        //public IActionResult Delete(int? id)
        //{
        //    if(id==null || id==0)
        //    {
        //        return NotFound();
        //    }
        //    //var categoryFromDb = _db.Categories.Find(id);
        //    var companyFromDb = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id == id);

        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}

        ////POST
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST (int? id)
        //{
        //    //var obj = _db.Categories.Find(id);
        //    var obj = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);

        //    if (obj==null)
        //    {
        //        TempData["error"] = "Company was not deleted.";
        //        return NotFound();
        //    }
        //        _unitOfWork.Company.Remove(obj);
        //        _unitOfWork.Save();
        //    TempData["success"] = "Company deleted successfully.";
        //    return RedirectToAction("Index");
        //}

        public IActionResult Upsert(int? id)
        {
            Company company;
            if (id == null || id == 0)
            {
                //create company
                company = new Company();
                return View(company);
            }
            else
            {
                //update company
                company = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Company was not created.";
            return View(obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList});
        }

        //[HttpDelete]
        //public IActionResult Delete (int? id)
        //{
        //    var obj = _unitOfWork.Company.GetFirstOrDefault(c=>c.Id==id);
        //    if(obj==null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting."});
        //    }
        //    else
        //    {
        //        _unitOfWork.Company.Remove(obj);
        //        _unitOfWork.Save();
        //        return Json(new { success = true, message = "Delete successful." });
        //    }
        //}


        #endregion
    }
}
