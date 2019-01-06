using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrtgSensorSharp;

namespace ParkingMonitor
{
    public class PrtgSensor
    {
        private readonly PrtgChannel[] _prtgChannels;

        public PrtgSensor(IEnumerable<PrtgChannel> prtgChannels)
        {
            _prtgChannels = prtgChannels.ToArray();
        }

        public string ReportMessage()
        {
            var message = _prtgChannels
                .Where(channel => channel.NotEmpty)
                .Select(channel => channel.Message)
                .ToArray();

            return message.Any()
                 ? string.Join(Environment.NewLine, message)
                 : "OK";
        }

        public IPrtgReport Report()
        {
            var results = _prtgChannels
                .Select(channel => channel.PrtgResult)
                .ToArray();

            var message = ReportMessage();

            return PrtgReport.Successful(message, results);
        }
    }
}
