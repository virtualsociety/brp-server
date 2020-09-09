using Brp.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtualSociety.BrpServer;

namespace Brp.Api.Controllers
{
    public class BrpStubImplementation : IController
    {
        public Task<NationaliteithistorieHalCollectie> GetnationaliteithistorieAsync(string burgerservicenummer, string fields, DateTimeOffset? peildatum, DateTimeOffset? datumvan, DateTimeOffset? datumtotenmet)
        {
            throw new NotImplementedException();
        }

        public Task<PartnerhistorieHalCollectie> GetpartnerhistorieAsync(string burgerservicenummer, string fields, DateTimeOffset? peildatum, DateTimeOffset? datumvan, DateTimeOffset? datumtotenmet)
        {
            throw new NotImplementedException();
        }

        public Task<VerblijfplaatshistorieHalCollectie> GetverblijfplaatshistorieAsync(string burgerservicenummer, string fields, DateTimeOffset? peildatum, DateTimeOffset? datumvan, DateTimeOffset? datumtotenmet)
        {
            throw new NotImplementedException();
        }

        public Task<VerblijfstitelhistorieHalCollectie> GetverblijfstitelhistorieAsync(string burgerservicenummer, string fields, DateTimeOffset? peildatum, DateTimeOffset? datumvan, DateTimeOffset? datumtotenmet)
        {
            throw new NotImplementedException();
        }

        public Task<IngeschrevenPersoonHalCollectie> IngeschrevenNatuurlijkPersonenAsync(string expand, string fields, IEnumerable<string> burgerservicenummer, DateTimeOffset? geboorte__datum, string geboorte__plaats, Geslacht_enum? geslachtsaanduiding, bool? inclusiefoverledenpersonen, string naam__geslachtsnaam, string naam__voornamen, string verblijfplaats__gemeentevaninschrijving, string verblijfplaats__huisletter, int? verblijfplaats__huisnummer, string verblijfplaats__huisnummertoevoeging, string verblijfplaats__identificatiecodenummeraanduiding, string verblijfplaats__naamopenbareruimte, string verblijfplaats__postcode, string naam__voorvoegsel)
        {
            return null;
           // throw new NotImplementedException();
        }

        public async Task<IngeschrevenPersoonHal> IngeschrevenNatuurlijkPersoonAsync(string burgerservicenummer, string expand, string fields)
        {
            var ret = new IngeschrevenPersoonHal();
            var persoon = new FakeIngeschrevenPersoon(burgerservicenummer, ret);
            ret = persoon.CreateFakePersoon();
            return ret;
        }

        public Task<KindHalCollectie> IngeschrevenpersonenBurgerservicenummerkinderenAsync(string burgerservicenummer)
        {
            var ret = new IngeschrevenPersoonHal();
            var persoon = new FakeIngeschrevenPersoon(burgerservicenummer, ret);
            var kinderen = persoon.CreateKinderen();
            KindHalCollectie collection = new KindHalCollectie();
            collection._embedded = new KindHalCollectie__embedded();
            collection._embedded.Kinderen = kinderen;
            return Task.FromResult(collection);
        }

        public Task<KindHal> IngeschrevenpersonenBurgerservicenummerkinderenIdAsync(string burgerservicenummer, string id)
        {
            throw new NotImplementedException();
        }

        public Task<OuderHalCollectie> IngeschrevenpersonenBurgerservicenummeroudersAsync(string burgerservicenummer)
        {
            throw new NotImplementedException();
        }

        public Task<OuderHal> IngeschrevenpersonenBurgerservicenummeroudersIdAsync(string burgerservicenummer, string id)
        {
            throw new NotImplementedException();
        }

        public Task<PartnerHalCollectie> IngeschrevenpersonenBurgerservicenummerpartnersAsync(string burgerservicenummer)
        {
            throw new NotImplementedException();
        }

        public Task<PartnerHal> IngeschrevenpersonenBurgerservicenummerpartnersIdAsync(string burgerservicenummer, string id)
        {
            throw new NotImplementedException();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class BrpController : ControllerBase
    {
    }
}
