using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db_FrontDeskApp.Model
{
    public class StoredPackage
    {
        public int StoredPackageId { get; set; }
        public PackageSize PackageSize { get; set; }
        public DateTime StoredDate { get; set; }
        public bool RetrievedPackage { get; set; } //False: stored, True:Retrieved
        public DateTime? RetrievedDate { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer CustomerLink { get; set; }

        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public Facility FacilityLink { get; set; }
    }

    public enum PackageSize
    {
        Small,
        Medium,
        Large
    }
}
