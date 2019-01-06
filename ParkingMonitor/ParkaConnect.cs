using System;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Infrastructure;
using Serilog;

namespace ParkingMonitor
{
    [DbConfigurationType(typeof(BtcsParkingMonitorDbConfiguration))]
    public class ParkaConnect : DbContext
    {
        private static readonly ILogger Logger = Log.ForContext<ParkaConnect>();

        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<SubscriptionLicensePlates> SubcribtionLicensePlateses { get; set; }

        public ParkaConnect() : base("ParkanizerDatabase")
        {
            Database.Log = sql => Logger.Verbose("SQL: {SQL}", sql);
        }

    }
}