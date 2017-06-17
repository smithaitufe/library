using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;
using Library.Web.Models;

using Library.Web.Models.PublisherViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Extensions
{
    public static class PublisherExtension {

        public static Publisher MapToPublisher(this RegisterPublisherViewModel model) {
            return new Publisher {
                Name = model.Name
            };
        }
        public static Publisher MapToPublisher(this CreateEditPublisherViewModel model) {
            var publisher = new Publisher { Name = model.Name };
            if(model.Id.HasValue) publisher.Id = model.Id.Value;
            return publisher;
        }
        public static IQueryable<CreateEditPublisherViewModel> MapToCreateEditPublisherViewModel(this IQueryable<Publisher> query) {
            return query
            .Include( p => p.Books)
            .Select( p => new CreateEditPublisherViewModel { Id = p.Id, Name = p.Name } );            
        }
        public static CreateEditPublisherViewModel MapToCreateEditPublisherViewModel(this Publisher publisher) {
            return new CreateEditPublisherViewModel { Id = publisher.Id, Name = publisher.Name };       
        }
        public static IQueryable<PublisherViewModel> MapToPublisherViewModel(this IQueryable<Publisher> query) {
            return query
            .Include( p => p.Books)
            .Select(
                p => new PublisherViewModel {
                    Id = p.Id,
                    Name = p.Name,
                    Books = p.Books.ToList()                    
                }
            );            
        }
        public static IQueryable<SelectListItem> MapToSelectList(this IQueryable<Publisher> publishers) {
            return publishers.Select(t=> new SelectListItem { Value = t.Id.ToString(), Text = t.Name});
        }
    }
}