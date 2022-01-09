using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceAppApi.ViewModel
{
    public class OfferView
    {
        public Guid ProviderId { get; set; }
        public Guid IceTasteId { get; set; }
        public string IceTaste { get; set; }
        public Guid IceKindId { get; set; }
        public string IceKind { get; set; }
    }
}
