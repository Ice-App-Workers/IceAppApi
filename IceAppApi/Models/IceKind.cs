using System;
using System.Collections.Generic;

#nullable disable

namespace IceAppApi.Models
{
    public partial class IceKind
    {
        public IceKind()
        {
            IceShopOffers = new HashSet<IceShopOffer>();
        }

        public Guid Id { get; set; }
        public string IceKind1 { get; set; }

        public virtual ICollection<IceShopOffer> IceShopOffers { get; set; }
    }
}
