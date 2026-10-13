namespace GlowBeauty_Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal TotalPrice { get; set; }
        public string SlipPath { get; set; } = string.Empty; // เก็บชื่อไฟล์สลิปโอนเงิน
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "รอตรวจสอบชำระเงิน";
    }
}