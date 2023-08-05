using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db_FrontDeskApp.Model
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int SmallCapacity { get; set; }
        public int MediumCapacity { get; set; }
        public int LargeCapacity { get; set; }

        public int SmallRemainingCapacity { get; set; }
        public int MediumRemainingCapacity { get; set; }
        public int LargeRemainingCapacity { get; set; }
        
        public ICollection<StoredPackage> PackageList { get; set; }
    }
}
