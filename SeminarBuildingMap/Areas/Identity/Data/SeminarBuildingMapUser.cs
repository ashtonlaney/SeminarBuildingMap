using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SeminarBuildingMap.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the SeminarBuildingMapUser class
    //Simply extends the built in Identity class to also hold someones name
    public class SeminarBuildingMapUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
