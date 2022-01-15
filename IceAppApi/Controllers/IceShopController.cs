using IceAppApi.Helpers;
using IceAppApi.Models;
using IceAppApi.RequestModel;
using IceAppApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IceShopController : ControllerBase
    {
        private readonly IceAppDbContext _context;
        public IceShopController(IceAppDbContext context)
        {
            _context = context;
        }

        [HttpPut("/Register")]
        public async Task<ServiceResponse<bool>> Create(Register provider)
        {
            if (!ModelState.IsValid)
                return ServiceResponse<bool>.Error("User adding error");

            Guid userId = Guid.NewGuid();

                HttpClient client = new HttpClient();

                var result = await client.GetStringAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={provider.City}+{provider.Street}+{provider.Number}&key=AIzaSyDWuh1lZP1loT8uTBwG1pdzzQbf03dKj4c");

                var location = JsonConvert.DeserializeObject<GetCoordinatessModel>(result);
                decimal lat = location.results[0].geometry.location.lat;
                decimal lng = location.results[0].geometry.location.lng;

                IceShopOwner providerToAdd = new IceShopOwner
                {
                    Id = Guid.NewGuid(),
                    Email = provider.Email,
                    Password = Crypto.HashPassword(provider.Password),
                    Name = provider.Name,
                    Phone = provider.Phone,
                    City = provider.City,
                    PostalCode = provider.PostalCode,
                    Description = provider.Description,
                    Number = provider.Number,
                    Street = provider.Street,
                    UserId = userId,
                    Lat = lat,
                    Lng = lng
                };
                _context.Add(providerToAdd);
           
            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "User was added");
        }
        [HttpPut("/AddTaste")]
        public async Task<ServiceResponse<bool>> AddTaste(string tasteName)
        {
            var taste = new IceTaste { IceTaste1 = tasteName, Id = Guid.NewGuid() };
            _context.IceTastes.Add(taste);
            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true,"Taste added");
        }
        [HttpPut("/AddToOffer")]
        public async Task<ServiceResponse<bool>> AddToOffer(string providerId,string tasteName, decimal kindPrice)
        {
            var tasteId = await _context.IceTastes
                .Where(row => row.IceTaste1 == tasteName)
                .Select(row=>row.Id).FirstOrDefaultAsync();
            if (tasteId == Guid.Empty)
                return ServiceResponse<bool>.Error("Taste not exists");

            _context.IceShopOffers.Add(new IceShopOffer
            {
                Id = Guid.NewGuid(),
                IceShopOwner = Guid.Parse(providerId),
                IceTaste = tasteId,
                KindPrice = kindPrice, 
            });
            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "Ice type was added to offer");
        }
        [HttpPut("/RemoveFromOffer")]
        public async Task<ServiceResponse<bool>> RemoveFromOffer(string providerId,string tasteName)
        {
            var tasteId = await _context.IceTastes
                .Where(row => row.IceTaste1 == tasteName)
                .Select(row => row.Id).FirstOrDefaultAsync();
            if (tasteId == Guid.Empty)
                return ServiceResponse<bool>.Error("Taste not exists");

            var offerToRemove =await _context.IceShopOffers
                .Where(row => row.IceShopOwner == Guid.Parse(providerId))
                .Where(row => row.IceTaste == tasteId)
                .FirstOrDefaultAsync();

            _context.IceShopOffers.Remove(offerToRemove);
            await _context.SaveChangesAsync();
            return ServiceResponse<bool>.Ok(true, "Ice type was removed from offer");
        }
        [HttpPut("/GetTastes")]
        public async Task<ServiceResponse<List<TastesView>>> GetTastes()
        {
            var result = await (_context.IceTastes.Select(row => new TastesView
                                {
                                    IceTaste = row.IceTaste1,
                                    IceTasteId = row.Id,
                                })).ToListAsync();

            return ServiceResponse<List<TastesView>>.Ok(result, "Offer returned");
        }
        [HttpPut("/GetOffer")]
        public async Task<ServiceResponse<List<OfferView>>> GetOffer(Guid providerId)
        {
            var result = await (from offer in _context.IceShopOffers.Where(row => row.IceShopOwner == providerId)
                          join taste in _context.IceTastes on offer.IceTaste equals taste.Id
                          select new OfferView
                          {
                              ProviderId = providerId,
                              IceTaste = taste.IceTaste1,
                              IceTasteId = taste.Id,
                              KindPrice = offer.KindPrice,
                          }).ToListAsync();

            return ServiceResponse<List<OfferView>>.Ok(result, "Offer returned");
        }
        [HttpPut("/GetShops")]
        public async Task<ServiceResponse<List<IceShop>>> GetIceShops()
        {
            var result =  await (_context.IceShopOwners.Select(row => new IceShop
            {
                Description = row.Description,
                Lat = row.Lat,
                Lng = row.Lng,
                Name = row.Name,
                ProviderId = row.Id,
                ShopName = row.Name,
                City = row.City,
                Street = row.Street,
                Number = row.Number
            })).ToListAsync();
            return ServiceResponse<List<IceShop>>.Ok(result, "IceCream shops returned");
        }
        [HttpPut("/GetShopsById")]
        public async Task<ServiceResponse<List<IceShop>>> GetIceShopsById(Guid providerId)
        {
            var result = await (_context.IceShopOwners.Where(row => row.Id == providerId).Select(row => new IceShop
            {
                Description = row.Description,
                Lat = row.Lat,
                Lng = row.Lng,
                Name = row.Name,
                ProviderId = row.Id,
                ShopName = row.Name,
                City = row.City,
                Street = row.Street,
                Number = row.Number
            })).ToListAsync();
            return ServiceResponse<List<IceShop>>.Ok(result, "IceCream shop returned");
        }

        private async Task<Guid> GetProvider()
        {
            return await (_context.IceShopOwners
                    .Where(row => row.Email == User.Identity.Name).Select(r=>r.Id)).FirstOrDefaultAsync();
        }

    }
}
