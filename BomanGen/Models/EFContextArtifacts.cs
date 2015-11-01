using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BomanGen.Models
{
    public class EFContextArtifacts : DbContext
    {
        public DbSet<Artifact> Artifact { get; set; }
    }
}