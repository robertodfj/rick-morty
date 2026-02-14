namespace Bot.model.request
{
    public class ItemForSaleRequest
    {
        public required int ItemID { get; set; }
        public required decimal Price { get; set; }
    }
}