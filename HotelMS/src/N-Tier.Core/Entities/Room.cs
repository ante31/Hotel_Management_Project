using HotelMS.Core.Common;
using HotelMS.Core.Enums;

namespace HotelMS.Core.Entities
{
    public class Room : BaseEntity, IAuditedEntity
    {
        public int RoomNumber { get; set; }

        public RoomTypeEnum RoomType { get; set; }

        public bool AvailabilityStatus { get; set; }

        public double Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    
        public bool Deleted { get; set; }
    }
}
