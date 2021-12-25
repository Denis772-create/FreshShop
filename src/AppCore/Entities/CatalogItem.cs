using AppCore.Interfaces;
using Ardalis.GuardClauses;
using SharedKernel.Interfaces;

namespace AppCore.Entities
{
    public class CatalogItem : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string PictureUri { get; private set; }
        public int CatalogTypeId { get; private set; }
        public CatalogType CatalogType { get; private set; }

        public CatalogItem(string name, string description, decimal price, string pictureUri, int catalogTypeId)
        {
            Name = name;
            Description = description;
            Price = price;
            PictureUri = pictureUri;
            CatalogTypeId = catalogTypeId;
        }

        public void UpdateDetails(string name, string description, decimal price)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Name = name;
            Description=description;
            Price = price;
        }

        public void UpdateType(int catalogTypeId)
        {
            Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));

            CatalogTypeId = catalogTypeId;
        }

        public void UpdatePictureUri(string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName))
            {
                PictureUri = string.Empty;
                return;
            }
            PictureUri = $"images\\{PictureUri}?{new DateTime().Ticks}";
        }
    }
}
