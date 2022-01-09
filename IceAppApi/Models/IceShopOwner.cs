using System;
using System.Collections.Generic;

#nullable disable

namespace IceAppApi.Models
{
    public partial class IceShopOwner
    {
        public IceShopOwner()
        {
            IceShopOffers = new HashSet<IceShopOffer>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string PostalCode { get; set; }
        public Guid UserId { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public virtual ICollection<IceShopOffer> IceShopOffers { get; set; }
    }
}
