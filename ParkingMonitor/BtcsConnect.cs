using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Infrastructure;
using Serilog;

namespace ParkingMonitor
{
    [DbConfigurationType(typeof(BtcsParkingMonitorDbConfiguration))]
    public class BtcsConnect : DbContext
    {
        private static readonly ILogger Logger = Log.ForContext<BtcsConnect>();

        public DbSet<Transaction> Transaction { get; set; }
  
        public BtcsConnect() : base("BtcsDatabase")
        {
            Database.Log = sql => Logger.Verbose("SQL: {SQL}", sql);
        }
        
    }
}