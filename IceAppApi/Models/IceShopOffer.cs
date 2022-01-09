using System;
using System.Collections.Generic;

#nullable disable

namespace IceAppApi.Models
{
    public partial class IceShopOffer
    {
        public Guid Id { get; set; }
        public Guid IceShopOwner { get; set; }
        public Guid IceTaste { get; set; }
        public Guid IceKind { get; set; }

        public virtual IceKind IceKindNavigation { get; set; }
        public virtual IceShopOwner IceShopOwnerNavigation { get; set; }
        public virtual IceTaste IceTasteNavigation { get; set; }
    }
}
