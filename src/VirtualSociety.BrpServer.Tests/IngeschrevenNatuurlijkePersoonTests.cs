using Brp.Api.Controllers;
using Xunit;

namespace Brp.Api.Tests
{
    public class IngeschrevenNatuurlijkePersoonTests
    {
        [Fact]
        public async void Test1()
        {
            BrpStubImplementation stub = new BrpStubImplementation();
            var persoon = await stub.IngeschrevenNatuurlijkPersoonAsync("293423805", null, null);
            Assert.Equal("293423805", persoon.Burgerservicenummer);


        }
    }
}
