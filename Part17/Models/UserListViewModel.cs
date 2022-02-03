using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Part17.Models
{
    public class UserListViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public SelectList Companies { get; set; }
    }
}
