using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using SimplerSettings;

namespace ParkingMonitor
{
    public class ParkanizerTasks
    {
        private readonly IClock _clock;
        private readonly ParkaConnect _parkaConnect;

        public ParkanizerTasks(ParkaConnect parkaConnect,IClock clock)
        {
            _parkaConnect = parkaConnect;
            _clock = clock;
        }

        public async Task<List<List<string>>> SubscriptionLpnsList()
        {
            var month = _clock.GetCurrentInstant().ToDateTimeUtc().Month;
            var year = _clock.GetCurrentInstant().ToDateTimeUtc().Year;
            var priceTable = AppSettings.Get("PriceTableIdWithoutDate");
            var result = await _parkaConnect.Subscriptions
              .Join(_parkaConnect.SubcribtionLicensePlateses, s => s.Id, slpn => slpn.SubscriptionId,
                  (s, slpn) => new { s, slpn })
              .Where(action => action.s.PriceTableId == priceTable + "-" + year.ToString() + "-" + month.ToString()
                               && action.s.Status == "Completed"
                               && action.s.PaymentInfo_PaymentStatus == "PaymentSucceeded"
                               && action.s.Customer_AccessCardNumber != null)
              .Select(action => new SubscriptionLicensePlates
              {
                  SubscriptionId = action.s.Id.ToString(),
                  AccessCardNumber = action.s.Customer_AccessCardNumber,
                  LicensePlatesList = action.slpn.Value                  
              })
              .GroupBy(s => new { s.SubscriptionId , s.AccessCardNumber }, s => s.LicensePlatesList)
              .ToListAsync();

            List<List<string>> LicensePlatesList = new List<List<string>>();

            foreach (var row in result)
            {
                List<string> LicensePlatesAggregation = new List<string>();

                LicensePlatesAggregation.Add(row.Key.AccessCardNumber);
                var lpnList = row.ToList();
                foreach (var licensePlate in lpnList)
                {
                    LicensePlatesAggregation.Add(licensePlate);
                }
                LicensePlatesList.Add(LicensePlatesAggregation);
            }
            
            return LicensePlatesList;                
        }

        public async Task<List<string>> SubscriptionTemporaryLpnsList()
        {
            var month = _clock.GetCurrentInstant().ToDateTimeUtc().Month;
            var year = _clock.GetCurrentInstant().ToDateTimeUtc().Year;
            var priceTable = AppSettings.Get("PriceTableIdWithoutDate").Value;
            var result = await _parkaConnect.Subscriptions
              .Join(_parkaConnect.SubcribtionLicensePlateses, s => s.Id, slpn => slpn.SubscriptionId,
                  (s, slpn) => new { s, slpn })
              .Where(action => action.s.PriceTableId == priceTable + "-" + year.ToString() + "-" + month.ToString()
                               && action.s.Status == "Completed"
                               && action.s.PaymentInfo_PaymentStatus == "PaymentSucceeded"
                               && action.slpn.IsPrimary == 0)

              .Select(action => action.slpn.Value
              )
              .ToListAsync();

            List<List<string>> LicensePlatesList = new List<List<string>>();
            
            return result;
        }

         public class LpnList
        {
            public List<Lpn> LicensePlateList { get; set; }
        }

        public class Lpn
        {
            public string LicensePlate { get; set; }
        }

        public class SubscriptionLicensePlates
        {
            public string SubscriptionId { get; set; }
            public string LicensePlatesList { get; set; }
            public string AccessCardNumber { get; set; }
        }        
    }
}
