namespace NorthWind.ClasesAux
{
    public class ProductCLS
    {
        public string? ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string SupplierId { get; set; }
        public string CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public string UnitPrice { get; set; }
        public string UnitsInStock { get; set; }
        public string UnitsOnOrder { get; set; }
        public string ReorderLevel { get; set; }
        public string Discontinued { get; set; }
    }

}
