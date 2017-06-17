using System.ComponentModel.DataAnnotations;

namespace Library.Code {
    public enum CommentStatus {
        [Display(Name="In Review")]
        InReview = 1,
        [Display(Name="Approved")]
        Approved,
        [Display(Name="Rejected")]
        Rejected
    }
}