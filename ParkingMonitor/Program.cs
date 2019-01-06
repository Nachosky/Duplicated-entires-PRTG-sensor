using NodaTime;
using PrtgSensorSharp;
using System;
using System.Collections.Generic;
using SimplerSettings;

namespace ParkingMonitor
{
    class Program
    {

        static void Main(string[] args)
        {
            PrtgExeScriptAdvanced.Run(() =>
          {
              List<List<string>> licensePlatesList;
              List<string> temporaryLpnList;
              using (var parkaConnect = new ParkaConnect())
              {
                  var monitor = new ParkanizerTasks(
                      parkaConnect: parkaConnect,
                      clock: SystemClock.Instance);

                  licensePlatesList = monitor
                  .SubscriptionLpnsList()
                  .Result;

                  temporaryLpnList = monitor
                  .SubscriptionTemporaryLpnsList()
                  .Result;

              }

              using (var btcsConnect = new BtcsConnect())
              {
                  var connection = new BtcsTasks(
                        btcsConnect: btcsConnect,
                        clock: SystemClock.Instance);

                  var abusiveLpns = connection
                    .GetAbusingLpnList(
                        AppSettings.Get("ParkingName"),
                        licensePlatesList);

                  var temporarayLpnOnParking = connection
                    .GetTransactionsWhereTemporaryLpnIsOnParking(
                        AppSettings.Get("ParkingName"),
                        temporaryLpnList);

                  var sensor = new PrtgSensor(new[]
                    {
                        new PrtgChannel(channelName: "TempLpnOnParking",
                        devices: temporarayLpnOnParking),
                        new PrtgChannel(channelName: "Abuse",
                        devices: abusiveLpns)

                  });
                  var report = sensor.Report();
                  return report;

              }

          }
            );

        }
    }

}

