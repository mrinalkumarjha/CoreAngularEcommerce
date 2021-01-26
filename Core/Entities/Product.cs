namespace Core.Entities
{
    public class Product
    {
        // by convention EF will create Id column as PK and identity
        public int Id { get; set; }
        public string  Name { get; set; }     
    }
}