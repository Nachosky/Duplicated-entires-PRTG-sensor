using System.Collections.Generic;
using System.Linq;
using PrtgSensorSharp;

    namespace ParkingMonitor
    {
        public class PrtgChannel
        {
            private readonly string _channelName;
            private readonly string[] _idsSource;

            public PrtgChannel(string channelName, IEnumerable<string> devices)
            {
                _channelName = channelName;
                _idsSource = devices.ToArray();
            }

            public PrtgResult PrtgResult => new PrtgResult(_channelName, _idsSource.Length, PrtgUnit.Count);

            public bool NotEmpty => _idsSource.Any();

            public string Message => $"{_channelName}: {CommaSeparated(_idsSource)}.";

        private static string CommaSeparated(string[] entities) => string.Join("," ,entities
            .OrderBy(id => id).ToArray());
                
    }        
}

