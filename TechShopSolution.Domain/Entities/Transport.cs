using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Transport
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TransporterId { get; set; }
        public int ShipStatus { get; set; }
        public string? FromAddress { get; set; }
        public string? ToAddress { get; set; }
        public decimal CodPrice { get; set; }
        public string? LadingCode { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? CancelAt { get; set; }
        public DateTime? DoneAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public Transporter? Transporter { get; set; }
        public Order? Order { get; set; }
    }
}
