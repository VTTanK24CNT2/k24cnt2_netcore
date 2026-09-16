namespace VttBt02.Models
{
    public class VttProduct
    {
         public int VttId { get; set; }
        public string VttName { get; set; }
        public string VttImages { get; set; }
        public decimal VttPrice { get; set; }
        public decimal? VttPriceSale { get; set; }
        public int VttCategoryId { get; set; }
        public string VttDescription { get; set; }
        public bool VttStatus { get; set; }
        public DateTime? VttCreatedAt { get; set; }
    }
}
