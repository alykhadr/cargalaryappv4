namespace CarGalary.Domain.Entities
{
    public class UserRate : BaseEntity
    {
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int? CarId { get; set; }
        public Car? Car { get; set; }

        public string? ReviewerNameAr { get; set; }
        public string? ReviewerNameEn { get; set; }

        public string? CommentAr { get; set; }
        public string? CommentEn { get; set; }

        public decimal RateValue { get; set; }

        // False = customer reviews, True = product reviews.
        public bool IsProductReview { get; set; }
    }
}
