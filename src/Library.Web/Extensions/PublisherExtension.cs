using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Library.Web.Models.AddressViewModels;
using Library.Web.Models.PublisherViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Extensions
{
    public static class PublisherExtension {


        public static Publisher MapToPublisher(this PublisherEditorViewModel model) {
            return new Publisher 
            { 
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = new Address {
                    Line = model.Address.Line,
                    City = model.Address.City,
                    StateId = model.Address.StateId,
                    CountryId = model.Address.CountryId
                }              
            };
            
        }
        public static IQueryable<PublisherEditorViewModel> MapToPublisherEditorViewModel(this IQueryable<Publisher> query) {
            return query.Select( 
                p => new PublisherEditorViewModel 
                { 
                    Id = p.Id, 
                    Name = p.Name,
                    Address = new AddressEditorViewModel{
                        Line = p.Address.Line,
                        City = p.Address.City,
                        StateId = p.Address.StateId,
                        CountryId = p.Address.CountryId
                    } 
                     
                });            
        }
        public static PublisherEditorViewModel MapToPublisherEditorViewModel(this Publisher publisher) {
            return new PublisherEditorViewModel 
            { 
                Id = publisher.Id, 
                Name = publisher.Name,
                Address = new AddressEditorViewModel{
                    Line = publisher.Address.Line,
                    City = publisher.Address.City,
                    StateId = publisher.Address.StateId,
                    CountryId = publisher.Address.CountryId
                } 
        };       
        }
        public static IQueryable<PublisherViewModel> MapToPublisherViewModel(this IQueryable<Publisher> query) {
            return query.Select(
                p => new PublisherViewModel {
                    Id = p.Id,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    Books = p.Books.ToList()                    
                });            
        }
        public static IQueryable<SelectListItem> MapToSelectList(this IQueryable<Publisher> publishers) {
            return publishers.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name});
        }
    }
}