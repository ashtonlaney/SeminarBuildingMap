using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeminarBuildingMap.GenericClasses
{
    //can hold multiple connection string references that are loaded from the appsettings.json file, currently only holds ConnStr that points to the single database used
    public class ConnectionConfig
    {
        public string ConnStr { get; set; } 
    }
}
