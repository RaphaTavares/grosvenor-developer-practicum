namespace Domain
{
    public class Order
    {
        public Order()
        {
            Dishes = new List<int>();
        }
        public List<int> Dishes { get; set; }
        public ServingTime ServingTime { get; set; }
    }
}