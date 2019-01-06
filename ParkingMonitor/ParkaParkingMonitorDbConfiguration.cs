using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingMonitor
{
    public class ParkaParkingMonitorDbConfiguration : DbConfiguration
    {
        public ParkaParkingMonitorDbConfiguration()
        {
            SetDatabaseInitializer(new NullDatabaseInitializer<ParkaConnect>());
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
