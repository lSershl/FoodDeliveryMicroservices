using Catalog.Entities;

namespace Catalog.Data
{
    public class CatalogInitialData
    {
        public List<CatalogItem> GetInitialData()
        {
            var productsList = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Пепперони",
                    Description = "Классическая пицца с колбасками пепперони",
                    ImageUrl = "https://mapizza.ru/wp-content/uploads/2019/09/pp-min.png",
                    Price = 899m
                },
                new CatalogItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Маргарита",
                    Description = "Классическая сырная пицца",
                    ImageUrl = "https://static.insales-cdn.com/images/products/1/8114/717316018/%D0%BC%D0%B0%D1%80%D0%B3%D0%B0%D1%80%D0%B8%D1%82%D0%B0.jpg",
                    Price = 699m
                },
                new CatalogItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Вегетарианская",
                    Description = "Вегетарианская пицца без животных ингредиентов",
                    ImageUrl = "https://pizzatorg.ru/wa-data/public/shop/products/23/00/23/images/29/29.970.jpg",
                    Price = 799m
                },
                new CatalogItem
                {
                    Id = Guid.NewGuid(),
                    Name = "Яблочный сок",
                    Description = "Бутылка яблочного сока 0.95л",
                    ImageUrl = "https://mokostav.com/Products/Steklo/%D0%AF%D0%B1%D0%BB%D0%BE%D1%87%D0%BD%D1%8B%D0%B9%20%D1%81%D1%82%D0%B5%D0%BA%D0%BB%D0%BE%20%D0%B1%D0%BE%D0%BB%D1%8C%D1%88%D0%BE%D0%B9.png",
                    Price = 100m
                }
            };
            return productsList;
        }
    }
}
