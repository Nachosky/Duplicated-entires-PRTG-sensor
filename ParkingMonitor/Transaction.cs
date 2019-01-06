using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingMonitor
{
    [Table("t_Transaction")]
    public class Transaction
    {
        public Int64 TransactionId { get; set; }
      
        public string TransactionIdentity { get; set; }

        public DateTime PassageTime { get; set; }
        
        public string TollDomainIdentity { get; set; }
        
        public string ChargingPointIdentity { get; set; }
        
        public string LaneIdentity { get; set; }

        public bool IsManual { get; set; }

        public Int16 ParkingTransactionTypeId { get; set; }
        
        public string Lpn { get; set; }

        public bool IsInvalid { get; set; }
        
        public string Comment { get; set; }
        
        public string CustomerIdentity { get; set; }
        
        public string AccountIdentity { get; set; }
        
        public string ContractIdentity { get; set; }
        
        public string PairedTransactionIdentity { get; set; }
        
        public Int16 ParkingVerificationResultId { get; set; }

        public bool QuotaOnRelatedAccountExceeded { get; set; }

        public Int16 ParkingSignalTypeId { get; set; }

    }
}