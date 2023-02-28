namespace Grid_Custom_Binding.Data
{
    public class Order
    {
        public int? OrderID { get; set; }
        public string? CustomerID { get; set; }
        public double? Freight { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
