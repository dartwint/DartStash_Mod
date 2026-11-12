namespace dartwint.DartStash.Tests;

using System.Linq;
using System.Reflection;
using Xunit;

public class DtoConsistencyTests
{
    [Fact]
    public void AccessToStashRequestData_ShouldHaveSamePropertiesOnClientAndServer()
    {
        var clientType = typeof(dartwint.DartStash.Client.AccessToStashRequestData);
        var serverType = typeof(dartwint.DartStash.Server.AccessToStashRequestData);

        Assert.NotNull(clientType);
        Assert.NotNull(serverType);

        var clientProps = clientType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => (p.Name, p.PropertyType.FullName))
            .OrderBy(x => x.Name)
            .ToArray();

        var serverProps = serverType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => (p.Name, p.PropertyType.FullName))
            .OrderBy(x => x.Name)
            .ToArray();

        Assert.Equal(serverProps, clientProps);
    }
}