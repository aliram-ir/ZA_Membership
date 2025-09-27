using System.ComponentModel.DataAnnotations.Schema;

namespace ZA_Membership.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressTitle { get; set; } = string.Empty; // منزل
        public string Province { get; set; } = string.Empty; // "تهران",
        public string Town { get; set; } = string.Empty; // "تهران",
        public string Floor { get; set; } = string.Empty; // -1
        public string Number { get; set; } = string.Empty; // "1234",
        public string PostalCode { get; set; } = string.Empty; // "123
        public string Street { get; set; } = string.Empty; // "خیابان انقلاب",
        public string Street2 { get; set; } = string.Empty; // "خیابان آزادی",
        public string District { get; private set; } = string.Empty; // "منطقه 12",
        public string SideFloor { get; set; } = string.Empty; // "4",
        public string Description { get; set; } = string.Empty; // "آپارتمان 4، طبقه 2، پلاک 1234",
        public string BuildingName { get; set; } = string.Empty; // "ساختمان آزادی",

        public bool IsDefault { get; private set; } // آدرس پیش‌فرض است؟

        // --- کلید خارجی و Navigation Property ---
        public int UserId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; private set; }

    }
}
