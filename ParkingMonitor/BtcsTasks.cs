using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using PrtgSensorSharp;
using SimplerSettings;

namespace ParkingMonitor
{
    public class BtcsTasks
    {
        private readonly IClock _clock;
        private readonly BtcsConnect _btcsConnect;

        public BtcsTasks(BtcsConnect btcsConnect, IClock clock)
        {
            _btcsConnect = btcsConnect;
            _clock = clock;
        }

        public IEnumerable<string> GetAbusingLpnList(string parkingId, List<List<string>> licensePlatesList)
        {
            List<string> abuseTransactions = new List<string>();

            foreach (var row in licensePlatesList)
            {
                var results = GetTransactionsWithoutDuplicates(parkingId, licensePlatesList);

                var results2 = results.Where(s => row.Contains(s.Lpn)).Select(t => t.Lpn).ToList();

                if (results2.Count > 1)
                {
                    abuseTransactions.AddRange(results2);
                }
            }
            return abuseTransactions;
        }

        public List<Transaction> GetTransactionsWithoutDuplicates(string parkingId, List<List<string>> licensePlatesList)
        {
            var transactionList = GetTodayTransactions(parkingId).Result;
            var previousLpn = "";
            var previousPassageTime = _clock.GetCurrentInstant().ToDateTimeUtc().Date;
            var passageTime = previousPassageTime;
            var list = new List<Transaction>();
            var checkList = new List<Transaction>();
            var timeBufforForDuplicatedEntriesInMins = AppSettings.Get("timeBufforForDuplicatedEntriesInMins");

            transactionList.Select(t => t.Lpn);

            foreach (var transaction in transactionList)
            {
                var lpn = transaction.Lpn;
                passageTime = transaction.PassageTime;

                if (lpn != previousLpn)
                {
                    if (previousPassageTime < passageTime.AddSeconds(timeBufforForDuplicatedEntriesInMins))
                    {
                        list.Add(transaction);
                    }
                    else
                    {
                        checkList.Add(transaction);
                    }
                }
                previousLpn = lpn;
                previousPassageTime = passageTime;
            }
            return list;
        }

        public IEnumerable<string> GetTransactionsWhereTemporaryLpnIsOnParking(string parkingId, List<string> licensePlatesList)
        {
            var abuseTransactions = new List<Transaction>();
            var results = GetTodayTransactions(parkingId).Result;

            return results.Where(t => licensePlatesList.Contains(t.Lpn)).Select(t => t.Lpn).ToList();

        }


        public async Task<List<Transaction>> GetTodayTransactions(string parkingId)
        {

            var todayDate = _clock.GetCurrentInstant().ToDateTimeUtc().Date;

            var tommorowDate = _clock.GetCurrentInstant().ToDateTimeUtc().AddDays(1).Date;


            var results = await _btcsConnect.Transaction
                   .Where(t => t.TollDomainIdentity == parkingId
                               && t.PassageTime > todayDate
                               && t.ParkingTransactionTypeId == 0
                               && t.IsInvalid == false
                               && t.PairedTransactionIdentity == null
                               && t.IsManual == false)
                               .ToListAsync();

            return results;
        }


    }


}
