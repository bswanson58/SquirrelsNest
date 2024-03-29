﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SquirrelsNest.Service.Database {
    public class ServiceDbContext : IdentityDbContext {

        public ServiceDbContext( DbContextOptions<ServiceDbContext> options ) : 
            base( options ) {
        }
    }
}
