using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingMonitor
{
    public class SubscriptionLicensePlates

    {
        public Guid Id { get; set; }
        public Guid SubscriptionId { get; set; }
        public string Value { get; set; }
        public int IsPrimary { get; set; }
        [ForeignKey("SubscriptionId")]
        public Subscriptions Subscriptions { get; set; }
    }
}