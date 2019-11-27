using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DUDe.Models
{
    public class DeviceContext : DbContext
    {
        public DeviceContext() :
          base("DefaultConnection")
        { }
        public DbSet<Device> devices { get; set; }
    }
}