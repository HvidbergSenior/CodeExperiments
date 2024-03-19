using Xunit;

namespace Insight.Tests.End2End
{
    [CollectionDefinition("End2End")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public class WebAppTestCollection : ICollectionFixture<WebAppFixture>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {

    }

    [CollectionDefinition("End2EndCustomers")]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public class WebAppTestCollectionCustomers : ICollectionFixture<WebAppFixture>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {

    }
}
