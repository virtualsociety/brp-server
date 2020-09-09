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
            var persoon = await stub.IngeschrevenNatuurlijkPersoonAsync("293423802", null, null);
            var kinderen = await stub.IngeschrevenpersonenBurgerservicenummerkinderenAsync("293423802");
            Assert.Equal("293423802", persoon.Burgerservicenummer);


        }
    }
}
