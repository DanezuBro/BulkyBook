﻿
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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //GET
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Cover Type was not created.";
            return View(obj);

        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id==0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
                return View(coverTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Cover Type was not updates.";
            return View(obj);

        }

        //GET
        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST (int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);

            if (obj==null)
            {
                TempData["error"] = "Category was not deleted.";
                return NotFound();
            }
                _unitOfWork.CoverType.Remove(obj);
                _unitOfWork.Save();
            TempData["success"] = "Cover Type deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
