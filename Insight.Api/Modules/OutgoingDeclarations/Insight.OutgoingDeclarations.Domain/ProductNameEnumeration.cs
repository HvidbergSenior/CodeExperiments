namespace Insight.OutgoingDeclarations.Domain
{
    public enum ProductNameEnumeration
    {
        Unknown,
        Hvo100,
        HvoDiesel,
        Adblue,
        B100,
        Diesel,
        Petrol,
        HeatingOil,
        Other
    }

    public static class ProductNameEnumerationExtensions
    {
        public static string ProductNameEnumerationToTranslatedString(ProductNameEnumeration productNameEnumeration)
        {
            var stringValue = productNameEnumeration switch
            {
                ProductNameEnumeration.Hvo100 => "HVO100",
                ProductNameEnumeration.HvoDiesel => "HVO DIESEL",
                ProductNameEnumeration.Adblue => "ADBLUE",
                ProductNameEnumeration.B100 => "B100",
                ProductNameEnumeration.Diesel => "DIESEL",
                ProductNameEnumeration.Petrol => "PETROL",
                ProductNameEnumeration.HeatingOil => "HEATINGOIL",
                ProductNameEnumeration.Other => "OTHER",
                _ => "UNKNOWN"
            };

            return stringValue;
        }

        public static ProductNameEnumeration TranslatedStringToProductNameEnumeration(string fuelType)
        {
            var stringValue = fuelType switch
            {
                "HVO100" => ProductNameEnumeration.Hvo100,
                "HVO DIESEL" => ProductNameEnumeration.HvoDiesel,
                "ADBLUE" =>  ProductNameEnumeration.Adblue,
                "B100" => ProductNameEnumeration.B100,
                "DIESEL" => ProductNameEnumeration.Diesel,
                "PETROL" => ProductNameEnumeration.Petrol,
                "HEATINGOIL" => ProductNameEnumeration.HeatingOil,
                "OTHER" => ProductNameEnumeration.Other,
                _ => ProductNameEnumeration.Unknown
            };
            
            return stringValue;
        }

        public static bool IsRenewable(ProductNameEnumeration productNameEnumeration)
        {
            return productNameEnumeration is ProductNameEnumeration.B100 or ProductNameEnumeration.Hvo100 or ProductNameEnumeration.HvoDiesel;
        }
    }

}
