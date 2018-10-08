using System.Collections.Generic;
using Xunit;

namespace backend_tests.Setup
{
    [CollectionDefinition("Integration Collection")]
    public class IntegrationCollection : ICollectionFixture<TestFixture<TestStartupSQLite>>
    {
    }
}