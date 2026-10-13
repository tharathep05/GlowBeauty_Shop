using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlowBeauty_Shop.Data;
using GlowBeauty_Shop.Models;

namespace GlowBeauty_Shop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public OrdersController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // หน้าฟอร์มสั่งซื้อสินค้า
        public async Task<IActionResult> Create(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var order = new Order
            {
                ProductId = product.Id,
                Product = product,
                TotalPrice = product.Price
            };

            return View(order);
        }

        // บันทึกการสั่งซื้อ และแนบสลิปโอนเงิน
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, IFormFile? slipFile)
        {
            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null) return NotFound();

            if (slipFile != null && slipFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + slipFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await slipFile.CopyToAsync(fileStream);
                }

                order.SlipPath = "/uploads/" + uniqueFileName;
            }

            order.Product = product;
            order.TotalPrice = product.Price;
            order.OrderDate = DateTime.Now;
            order.Status = "ชำระเงินเรียบร้อย";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // หน้าสั่งซื้อสำเร็จ
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return View(order);
        }

        // หน้าประวัติรายการสั่งซื้อ
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }
    }
}