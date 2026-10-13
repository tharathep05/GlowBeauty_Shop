using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GlowBeauty_Shop.Data;
using GlowBeauty_Shop.Models;

namespace GlowBeauty_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // 1. GET: Products (แสดงรายการสินค้า + ค้นหา + ฟิลเตอร์ + จัดลำดับ)
        // =========================================================
        public async Task<IActionResult> Index(string searchString, string category, string skinType)
        {
            var query = _context.Products.AsQueryable();

            // 1.1 ค้นหาตามคีย์เวิร์ด (ถ้าระบุ)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString) || p.Description.Contains(searchString));
            }

            // 1.2 กรองตามหมวดหมู่สินค้า (ถ้าระบุ)
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            // 1.3 กรองตามสภาพผิวอย่างยืดหยุ่น (ดึงสภาพผิวที่เลือก + สินค้า "ทุกสภาพผิว")
            if (!string.IsNullOrEmpty(skinType) && skinType != "ทุกสภาพผิว")
            {
                query = query.Where(p => p.SkinType == skinType || p.SkinType == "ทุกสภาพผิว");
            }

            // 1.4 จัดลำดับการแสดงผลแบบกำหนดเอง (Custom Priority Ordering)
            // ลำดับ 1: กลุ่มเซรั่ม (Serum)
            // ลำดับ 2: กลุ่มรองพื้น (Foundation / Cushion / Tint)
            // ลำดับ 3: กลุ่มบลัชออน (Blush)
            // ลำดับ 4: สินค้าหมวดอื่นๆ ทั้งหมด (เรียงตาม Id จากน้อยไปมาก)
            query = query.OrderBy(p =>
                (p.Category == "Serum" || p.Name.Contains("Serum") || p.Name.Contains("เซรั่ม")) ? 1 :
                (p.Name.Contains("Foundation") || p.Name.Contains("รองพื้น") || p.Name.Contains("Cushion") || p.Name.Contains("Tint")) ? 2 :
                (p.Name.Contains("Blush") || p.Name.Contains("บลัช")) ? 3 :
                4
            ).ThenBy(p => p.Id);

            var productList = await query.ToListAsync();

            // 1.5 Fallback Guard (ป้องกันหน้าว่างเปล่า 100%)
            if (!productList.Any())
            {
                ViewBag.FallbackMessage = "ไม่พบสินค้าตรงตามเงื่อนไขผสมที่คุณเลือก ระบบจึงแสดงสินค้าแนะนำทั้งหมดให้คุณเลือกชมแทนครับ";

                // ลองดึงสินค้าตามหมวดหมู่ก่อน
                if (!string.IsNullOrEmpty(category))
                {
                    productList = await _context.Products
                        .Where(p => p.Category == category)
                        .OrderBy(p => p.Id)
                        .ToListAsync();
                }

                // หากยังไม่มี ให้ดึงสินค้าทั้งหมดในระบบขึ้นมาแสดง
                if (!productList.Any())
                {
                    productList = await _context.Products
                        .OrderBy(p => p.Id)
                        .ToListAsync();
                }
            }

            return View(productList);
        }

        // =========================================================
        // 2. GET: Products/Details/5 (รายละเอียดสินค้า)
        // =========================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // =========================================================
        // 3. GET: Products/Edit/5 (หน้าฟอร์มแก้ไขสินค้า)
        // =========================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // =========================================================
        // 4. POST: Products/Edit/5 (บันทึกการแก้ไขสินค้า + รูปภาพ)
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Category,SkinType,Description,ImageUrl")] Product product, IFormFile? imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // ค้นหาสินค้าตัวจริงในฐานข้อมูลด้วย id ป้องกันการสลับตัวสินค้า
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // ถ้ามีการอัปโหลดไฟล์รูปภาพใหม่
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // ตั้งชื่อไฟล์ด้วย GUID เพื่อป้องกันชื่อไฟล์ซ้ำและภาพสลับกับสินค้าอื่น
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        // อัปเดต URL รูปภาพใหม่เฉพาะสินค้าตัวนี้
                        existingProduct.ImageUrl = "/images/" + uniqueFileName;
                    }

                    // อัปเดตข้อมูลด้านอื่นๆ
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Category = product.Category;
                    existingProduct.SkinType = product.SkinType;
                    existingProduct.Description = product.Description;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}