using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UpsideDownKittenGenerator.Models
{
    public class UpsideDownKittyGenertorContext:DbContext
    {
        public UpsideDownKittyGenertorContext(DbContextOptions<UpsideDownKittyGenertorContext> options)
            : base(options)
        {
        }

        public DbSet<UpsideDownKittyGenerator> TodoItems { get; set; }
    }
}



