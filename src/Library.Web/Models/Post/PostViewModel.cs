using System;
using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Models.PostViewModels
{
    public class PostViewModel {
        public int Id { get; set; }
        public User Author { get; set; }       
        public Club Club { get; set; }
        public Term Category { get; set; }       
        public string Text { get; set; }        
        public string Title { get; set; }   
        public int Views { get; set; }  = 0;
        public DateTime DateCreated { get; set; }
        public bool Locked { get; set; }
        public bool Hidden { get; set; }
        public PostCommentViewModel CommentForm { get; set; } = new PostCommentViewModel();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        
        
    }
}