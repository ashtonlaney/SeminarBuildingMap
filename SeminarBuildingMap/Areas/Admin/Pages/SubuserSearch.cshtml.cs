using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using SeminarBuildingMap.Areas.Identity.Data;
using SeminarBuildingMap.GenericClasses;

namespace SeminarBuildingMap.Areas.Admin
{
    [Authorize(Roles="Admin")]
    public class SubuserSearchModel : PageModel
    {
        private readonly IOptions<ConnectionConfig> _connectionConfig;
        private readonly Models.RoomDataAccessLayer ObjRoom = new Models.RoomDataAccessLayer();
        public IQueryable<SeminarBuildingMapUser> users { get; set; }

        public SubuserSearchModel(IOptions<GenericClasses.ConnectionConfig> connectionConfig)
        {
            _connectionConfig = connectionConfig;
        }

        public void OnGet()
        {
            users = ObjRoom.GetSubusers(_connectionConfig.Value.ConnStr);
        }
    }
}
