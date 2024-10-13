using System.Net;
using Xunit;

namespace Test.Controller;

public class ControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> server;

    public ControllerTests(WebApplicationFactory<Program> server)
    {
        this.server = server;
    }

    [Fact]
    public async Task IndexPageShouldReturnStatusCode200WithTitle()
    {
        var client = server.CreateClient();
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("<title>", responseContent);
    }

    [Fact]
    public async Task AccountManagePageRequiresAuthorization()
    {
        var client = server.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var response = await client.GetAsync("Identity/Account/Manage");
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }
}
