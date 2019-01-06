using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingMonitor
{
    public class Subscriptions
    {
        public Subscriptions()
        {
            SubscriptionLicensePlates = new HashSet<SubscriptionLicensePlates>();
        }
        public Guid Id { get; set; }
        public string PriceTableId { get; set; }
        public string Status { get; set; }
        public string PaymentInfo_PaymentStatus { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_AccessCardNumber { get; set; }
        public virtual ICollection<SubscriptionLicensePlates> SubscriptionLicensePlates { get; set; }
    }
}