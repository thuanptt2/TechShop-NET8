using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CusId { get; set; }
        public string? NameReceiver { get; set; }
        public string? PhoneReceiver { get; set; }
        public string? AddressReceiver { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal TransportFee { get; set; }
        public int? CouponId { get; set; }
        public int PaymentId { get; set; }
        public int Status { get; set; }
        public bool IsPay { get; set; }
        public string? Note { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ConfirmAt { get; set; }
        public DateTime? PayAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? CancelAt { get; set; }
        public string? CancelReason { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public Customer? Customers { get; set; }
        public Transport? Transport { get; set; }
        public Coupon? Coupon { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
