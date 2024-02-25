using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Imag.Demo.Shared;

public partial class ImagDataContext : DbContext
{

    public string MyConnString { get; set; }
    
    public ImagDataContext(string connString)
    {
        MyConnString = connString;
    }

    public ImagDataContext(DbContextOptions<ImagDataContext> options)
        : base(options)
    {
    }   

    public virtual DbSet<Customer> Customers { get; set; }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(MyConnString);
    
}
