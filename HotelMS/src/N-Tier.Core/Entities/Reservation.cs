using HotelMS.Core.Common;
using Microsoft.AspNetCore.Identity;
using HotelMS.Core.Entities.Identity;

namespace HotelMS.Core.Entities
{
    public class Reservation : BaseEntity, IAuditedEntity
    {
        public Room Room { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool Approval { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool Deleted { get; set; }
        
        public bool Payed{ get; set; }

        public double Price { get; set; }
    }
}
