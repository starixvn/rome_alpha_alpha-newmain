namespace Alpha.Main.Models.MemberRequests
{
    public class ProductRequest
    {
        public ProductRequest()
        {
        }

        public ProductRequest(
            int productId,
            string productName,
            int numberOfRequest)
        {
            ProductId = productId;
            ProductName = productName;
            NumberOfRequest = numberOfRequest;
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public int NumberOfRequest { get; }
    }
}