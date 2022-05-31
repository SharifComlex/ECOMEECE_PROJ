﻿using Microflake.Core.Domain;
using Microflake.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Utilities
{
    public class GetCurrentUser
    {
        private readonly static ApplicationDbContext dbContext = new ApplicationDbContext();

        public static ApplicationUser Get(string UserId)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == UserId);

          
            return user;
        }
    }
}
