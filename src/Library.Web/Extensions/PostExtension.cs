using System.Linq;
using Library.Core.Models;
using Library.Web.Models;
using Library.Web.Models.PostViewModels;

namespace Library.Web.Extensions
{
    public static class PostExtension {
            public static PostViewModel MapToPostViewModel(this Post post) {
            return new PostViewModel {
                    Id = post.Id,
                    Author = post.Author,
                    Comments = post.Comments,
                    Text = post.Text,
                    Title = post.Title,
                    Views = post.Views,
                    DateCreated = post.InsertedAt,
                    Locked = post.Locked,
                    Hidden = post.Hidden,
                    Club = post.Club,
                    Category = post.Category
            };
        }
        public static IQueryable<PostViewModel> MapToPostViewModel(this IQueryable<Post> query) {
            return query.Select(post => new PostViewModel {
                    Id = post.Id,
                    Author = post.Author,
                    Comments = post.Comments,
                    Text = post.Text,
                    Title = post.Title,
                    Views = post.Views,
                    DateCreated = post.InsertedAt,
                    Locked = post.Locked,
                    Hidden = post.Hidden,
                    Club = post.Club,
                    Category = post.Category
            });
        }
         public static IQueryable<PostMessageViewModel> MapToPostMessageViewModel(this IQueryable<Post> query) {
            return query.Select(post => new PostMessageViewModel {
                    Id = post.Id,
                    AuthorId = post.AuthorId,
                    Text = post.Text,
                    Title = post.Title,
                    ClubId = post.ClubId,
                    CategoryId = post.CategoryId
            });
        }
        public static Post MapToPost(this PostMessageViewModel model) {
            var post = new Post {
                Title = model.Title,
                Text = model.Text,
                CategoryId = model.CategoryId,
                AuthorId = model.AuthorId,
                ClubId = model.ClubId                
            };
            post.Id = model.Id ?? 0;
            return post;
        }
        
    }
}