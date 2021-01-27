namespace Core.Entities
{
    public class Product: BaseEntity
    {
        // by convention EF will create Id column as PK and identity
        public string  Name { get; set; }    
        public string Description { get; set; }
        public decimal Price { get; set; } 
        public string PictureUrl { get; set; }

        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}