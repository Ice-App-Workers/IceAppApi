using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceAppApi.ViewModel
{
    public class IceShop
    {
        public Guid ProviderId { get; set; }
        public string ShopName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}
