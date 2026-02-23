namespace CarGalary.Application.Dtos.Offer.Command
{
    public class BulkDeleteOfferRequest
    {
        public List<int> OfferIds { get; set; } = new();
    }
}
