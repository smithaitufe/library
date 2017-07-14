using System.Collections.Generic;

namespace Library.Web.Models.PostViewModels {
    public class PostListingViewModel {
        
        public IList<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
    }
}