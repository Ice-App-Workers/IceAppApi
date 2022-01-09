using System;
using System.Collections.Generic;

#nullable disable

namespace IceAppApi.Models
{
    public partial class IceTaste
    {
        public IceTaste()
        {
            IceShopOffers = new HashSet<IceShopOffer>();
        }

        public Guid Id { get; set; }
        public string IceTaste1 { get; set; }

        public virtual ICollection<IceShopOffer> IceShopOffers { get; set; }
    }
}
