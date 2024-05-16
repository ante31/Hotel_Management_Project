using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace HotelMS.Core.Enums
{
    public enum RoomTypeEnum
    {
        Jednokrevetna = 1,
        Dvokrevetna = 2,
        Trokrevetna = 3,
        Četverokrevetna = 4,
        Penthouse = 5,
        Apartman = 6,
        Bungalov = 7,
        VIPApartman = 8
    }
}

