using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM() {

                OrderHeader = _unitOfWork.OrderHeaders.GetFirstOrDefault(o=>o.Id==orderId, includeProperties:"ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(o=>o.OrderId==orderId, includeProperties:"Product")
            };

            return View(OrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeaders.GetFirstOrDefault(o=>o.Id==OrderVM.OrderHeader.Id);

            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if(OrderVM.OrderHeader.Carrier!=null)
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeaders.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order details updated successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
        }



        #region API CALLS
        [HttpGet]
    public IActionResult GetAll(string status)
    {
            IEnumerable<OrderHeader> orderHeaders;

            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeaders.GetAll(includeProperties:"ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeaders = _unitOfWork.OrderHeaders.GetAll(o=>o.ApplicationUserId ==claim.Value, includeProperties: "ApplicationUser");
            }

           

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u=>u.OrderStatus==SD.StatusPending);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }
            return Json(new { data = orderHeaders });
    }

    #endregion
    }
}
