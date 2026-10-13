using Microsoft.EntityFrameworkCore;
using GlowBeauty_Shop.Data;
using GlowBeauty_Shop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ลงทะเบียน AppDbContext (ใช้งาน InMemory Database)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GlowBeautyDb"));

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            // --- 1. Cleanser ---
            new Product { Name = "Glow Gentle Cleansing Foam", Category = "Cleanser", Price = 320, SkinType = "ผิวแพ้ง่าย", Description = "วิปโฟมล้างหน้าสูตรอ่อนโยน ล้างสะอาดล้ำลึก ไม่ทำให้ผิวแห้งตึง", ImageUrl = "/images/Cleansing Foam.jpg" },
            new Product { Name = "Deep Pore Cleansing Oil", Category = "Cleanser", Price = 450, SkinType = "ผิวมัน", Description = "คลีนซิ่งออยล์สลายเครื่องสำอางกันน้ำและสิวเสี้ยนอย่างหมดจด", ImageUrl = "/images/Cleansing Oil.jpg" },
            new Product { Name = "Hydrating Micellar Water", Category = "Cleanser", Price = 290, SkinType = "ทุกสภาพผิว", Description = "คลีนซิ่งวอเตอร์สูตรเติมความชุ่มชื้น เช็ดทำความสะอาดผิวง่ายดาย", ImageUrl = "/images/Micellar Water.png" },
            new Product { Name = "Acne Clear Facial Wash", Category = "Cleanser", Price = 250, SkinType = "ผิวเป็นสิว", Description = "เจลล้างหน้าสูตรลดการสะสมของแบคทีเรียสาเหตุของการเกิดสิว", ImageUrl = "/images/Facial Wash.jpg" },

            // --- 2. Toner ---
            new Product { Name = "Rose Petal Soothing Toner", Category = "Toner", Price = 390, SkinType = "ผิวแห้ง", Description = "โทนเนอร์สกัดจากกุหลาบธรรมชาติ ปลอบประโลมผิวและปรับสมดุล pH", ImageUrl = "/images/Soothing Toner.jpg" },
            new Product { Name = "BHA Clarifying Essence", Category = "Toner", Price = 420, SkinType = "ผิวมัน/เป็นสิว", Description = "เอสเซนส์ BHA ช่วยผลัดเซลล์ผิวเก่า อุดตันในรูขุมขนอย่างอ่อนโยน", ImageUrl = "/images/BHA Clarifying Essence.jpg" },
            new Product { Name = "Cica Calming Water Toner", Category = "Toner", Price = 350, SkinType = "ผิวแพ้ง่าย", Description = "โทนเนอร์สารสกัดใบบัวบก ลดอาการรอยแดงและการอักเสบของผิว", ImageUrl = "/images/Calming Water Toner.jpg" },
            new Product { Name = "Milky Glow Hydrating Toner", Category = "Toner", Price = 380, SkinType = "ผิวธรรมดา", Description = "โทนเนอร์เนื้อน้ำนม มอบความชุ่มชื้นเข้มข้น ให้ผิวเนียนนุ่มฉ่ำวาว", ImageUrl = "/images/Milky Glow Hydrating Toner.jpg" },

            // --- 3. Serum ---
            new Product { Name = "Hyaluronic Acid Intense Serum", Category = "Serum", Price = 590, SkinType = "ผิวแห้ง", Description = "เซรั่มไฮยาโมเลกุลเล็ก เติมน้ำให้ผิวอิ่มฟูฉ่ำวาวทันทีที่ใช้", ImageUrl = "/images/serum1.jpg" },
            new Product { Name = "Vitamin C Brightening Serum", Category = "Serum", Price = 620, SkinType = "ทุกสภาพผิว", Description = "เซรั่มวิตามินซีเข้มข้น ปรับผิวให้กระจ่างใส ลดเลือนจุดด่างดำ", ImageUrl = "/images/serum2.jpg" },
            new Product { Name = "Niacinamide 10% Pore Serum", Category = "Serum", Price = 490, SkinType = "ผิวมัน", Description = "เซรั่มไนอะซินาไมด์ช่วยกระชับรูขุมขน และควบคุมความมันส่วนเกิน", ImageUrl = "/images/serum3.jpg" },
            new Product { Name = "Retinol Night Renewal Serum", Category = "Serum", Price = 690, SkinType = "ทุกสภาพผิว", Description = "เซรั่มเรตินอลบำรุงผิวช่วงกลางคืน ช่วยลดเลือนริ้วรอยอย่างมีประสิทธิภาพ", ImageUrl = "/images/serum4.jpg" },

            // --- 4. Moisturizer ---
            new Product { Name = "Pink Pastel Aqua Water Cream", Category = "Moisturizer", Price = 480, SkinType = "ทุกสภาพผิว", Description = "ครีมเจลเนื้อน้ำ สัมผัสบางเบา เติมความชุ่มชื้นยาวนานตลอดวัน", ImageUrl = "/images/Pink Pastel Aqua Water Cream.jpg" },
            new Product { Name = "Ceramide Barrier Repair Cream", Category = "Moisturizer", Price = 520, SkinType = "ผิวแพ้ง่าย", Description = "มอยส์เจอไรเซอร์เซราไมด์เข้มข้น เสริมเกราะป้องกันผิวให้แข็งแรง", ImageUrl = "/images/Ceramide Barrier Repair Cream.jpg" },
            new Product { Name = "Rich Collagen Night Cream", Category = "Moisturizer", Price = 580, SkinType = "ผิวแห้ง", Description = "ไนท์ครีมคอลลาเจนเข้มข้น บำรุงผิวล้ำลึกยามค่ำคืน", ImageUrl = "/images/Rich Collagen Night Cream.png" },
            new Product { Name = "Oil-Free Mattifying Gel", Category = "Moisturizer", Price = 390, SkinType = "ผิวมัน", Description = "เจลบำรุงผิวสูตรไร้น้ำมัน ช่วยคุมมัน และไม่ก่อให้เกิดการอุดตัน", ImageUrl = "/images/Oil-Free Mattifying Gel.jpg" },

            // --- 5. Sunscreen ---
            new Product { Name = "UV Soft Water Sunscreen SPF50+", Category = "Sunscreen", Price = 450, SkinType = "ทุกสภาพผิว", Description = "กันแดดเนื้อน้ำ บางเบา ซึมไว ไม่เหนียวเหนอะหนะ", ImageUrl = "/images/UV-Soft-Water-Sunscreen-SPF50.jpg" },
            new Product { Name = "Tone-Up Pink Sun Cream SPF50+", Category = "Sunscreen", Price = 480, SkinType = "ทุกสภาพผิว", Description = "กันแดดโทนอัพช่วยปรับสีผิวให้ดูสว่างอมชมพูอย่างเป็นธรรมชาติ", ImageUrl = "/images/Tone-Up-Pink-Sun-Cream-SPF50.jpg" },
            new Product { Name = "No-Sebum Mineral Sun Fluid", Category = "Sunscreen", Price = 420, SkinType = "ผิวมัน", Description = "กันแดดสูตรแร่ธาตุ ควบคุมความมันยาวนานตลอดวัน", ImageUrl = "/images/No-Sebum Mineral Sun Fluid.png" },
            new Product { Name = "Physical Sensitive Sunscreen SPF50+", Category = "Sunscreen", Price = 550, SkinType = "ผิวแพ้ง่าย", Description = "กันแดด Physical อ่อนโยนพิเศษ สะท้อนรังสี UV ได้ดีเยี่ยม", ImageUrl = "/images/Physical-Sensitive-Sunscreen-SPF50.png" },

            // --- 6. Mask ---
            new Product { Name = "Glow Pink Clay Mask", Category = "Mask", Price = 390, SkinType = "ผิวมัน", Description = "มาสก์โคลนสีชมพู ช่วยดูดซับสิ่งสกปรกและสิ่งอุดตันในรูขุมขน", ImageUrl = "/images/Glow Pink Clay Mask.jpg" },
            new Product { Name = "Hyaluron Hydrating Sheet Mask", Category = "Mask", Price = 69, SkinType = "ทุกสภาพผิว", Description = "แผ่นมาสก์หน้าไฮยาลูรอน เติมความชุ่มชื้นเร่งด่วนใน 15 นาที", ImageUrl = "/images/Hyaluron Hydrating Sheet Mask.jpg" },
            new Product { Name = "Sleeping Beauty Gel Mask", Category = "Mask", Price = 420, SkinType = "ทุกสภาพผิว", Description = "สลีปปิ้งมาสก์เนื้อเจล ตื่นมาพร้อมผิวนุ่มอิ่มฟูดูสุขภาพดี", ImageUrl = "/images/SleepingBeautyGelMask.jpg" },
            new Product { Name = "Teatree Calming Sheet Mask", Category = "Mask", Price = 69, SkinType = "ผิวเป็นสิว", Description = "แผ่นมาสก์สารสกัดทีทรี ปลอบประโลมผิวเป็นสิวและลดรอยแดง", ImageUrl = "/images/Teatree Calming Sheet Mask.jpg" },

            // --- 7. Makeup: บลัชออน ---
            new Product { Name = "Soft Pink Powder Blush #01", Category = "Makeup", Price = 280, SkinType = "ทุกสภาพผิว", Description = "บลัชออนเนื้อแป้งสีชมพูนม ให้แก้มละมุนเป็นธรรมชาติ", ImageUrl = "/images/blush1.jpg" },
            new Product { Name = "Peach Glow Cream Blush #02", Category = "Makeup", Price = 290, SkinType = "ทุกสภาพผิว", Description = "บลัชออนเนื้อครีมสีพีช เกลี่ยง่าย ติดทนนานตลอดวัน", ImageUrl = "/images/blush2.jpg" },
            new Product { Name = "Rose Velvet Blush #03", Category = "Makeup", Price = 280, SkinType = "ทุกสภาพผิว", Description = "บลัชออนเนื้อกำมะหยี่สีชมพูกุหลาบ ให้ลุคแก้มดูมีเลือดฝาด", ImageUrl = "/images/blush3.jpg" },
            new Product { Name = "Shimmer Coral Blush #04", Category = "Makeup", Price = 310, SkinType = "ทุกสภาพผิว", Description = "บลัชออนสีคอรัลผสมชิมเมอร์ละเอียด ให้แก้มประกายฉ่ำวาว", ImageUrl = "/images/blush4.jpg" },

            // --- 8. Makeup: รองพื้น ---
            new Product { Name = "Matte Perfection Foundation #21", Category = "Makeup", Price = 590, SkinType = "ผิวมัน", Description = "รองพื้นเนื้อแมตต์ ปกปิดเนียนเรียบ คุมมันยาวนานตลอดวัน", ImageUrl = "/images/foundation1.jpg" },
            new Product { Name = "Glow Cushion Foundation #23", Category = "Makeup", Price = 650, SkinType = "ผิวธรรมดา/ผิวแห้ง", Description = "คุชชั่นผิวฉ่ำวาวสไตล์เกาหลี ปกปิดเบาบางดูธรรมชาติ", ImageUrl = "/images/foundation2.jpg" },
            new Product { Name = "Lightweight Skin Tint Foundation", Category = "Makeup", Price = 490, SkinType = "ทุกสภาพผิว", Description = "รองพื้นเนื้อบางเบา สบายผิว ปรับสีผิวให้สม่ำเสมอ", ImageUrl = "/images/foundation3.jpg" },
            new Product { Name = "Full Coverage Liquid Foundation", Category = "Makeup", Price = 620, SkinType = "ทุกสภาพผิว", Description = "รองพื้นลิควิดระดับการปกปิดขั้นสุด กันน้ำกันเหงื่อ", ImageUrl = "/images/foundation4.jpg" }
        );

        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// 🎯 Route เริ่มต้นเปิดตรงเข้าหน้า Products/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();