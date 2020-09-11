using Brp.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using Vs.FactTables;

namespace VirtualSociety.BrpServer
{
    public class FakeIngeschrevenPersoon
    {
        private int _seed;
        private IngeschrevenPersoonHal _persoon;
        private Random _r;
        private int _maxKinderen = 6;

        static FakeIngeschrevenPersoon()
        {
            Adressen_Entity.Init();
            Voornamen_Entity.Init();
            Achternamen_Entity.Init();
        }

        public FakeIngeschrevenPersoon(string bsn, IngeschrevenPersoonHal persoon)
        {
            _seed = int.Parse(bsn);
            _persoon = persoon;
            _persoon.Burgerservicenummer = bsn;
            _persoon.Verblijfplaats = new Verblijfplaats();
            _persoon.Naam = new NaamPersoon();
        }

        public List<KindHal> CreateKinderen()
        {
            // _r is reset on purpose
            _r = new Random(_seed);
            var collection = new List<KindHal>();
            for (int i = 0; i < _r.Next(0,_maxKinderen);i++)
            {
                var kind = new KindHal();
                kind.Burgerservicenummer = CreateBsn();
                kind.Geboorte = new Geboorte();
                kind.Geboorte.Datum = GetDatumOnvolledig(18);
                kind.Geboorte.Land = GetLand();
                kind.Geboorte.Plaats = GetPlaats();
                collection.Add(kind);
            }
            return collection;
        }

        private Datum_onvolledig GetDatumOnvolledig(int maxAge)
        {
            Datum_onvolledig datum = new Datum_onvolledig();
            DateTime birth = new DateTime(2020, 1, 1).AddDays(-_r.Next(18 * 365));
            datum.Dag = birth.Day;
            datum.Maand = birth.Month;
            datum.Jaar = birth.Year;
            datum.Datum = birth;
            return datum;
        }

        private Waardetabel GetLand()
        {
            var waardetabel = new Waardetabel();
            waardetabel.Omschrijving = "Nederland";
            waardetabel.Code = "6030";
            return waardetabel;
        }

        private Waardetabel GetPlaats()
        {
            var waardetabel = new Waardetabel() { Code = "0000", Omschrijving = "Placeholder" };
            return waardetabel;
        }


        private string CreateBsn()
        {
            int rest; string bsn;
            do
            {
                bsn = ""; int total = 0; for (int i = 0; i < 8; i++)
                {
                    int rndDigit = _r.Next(0, i == 0 ? 2 : 9);
                    total += rndDigit * (9 - i);
                    bsn += rndDigit;
                }
                rest = total % 11;
            }
            while (rest > 9);
            return bsn + rest;
        }

        public IngeschrevenPersoonHal CreateFakePersoon()
        {
            // _r is reset on purpose.
            _r = new Random(_seed);
            SetRandomAddress();
            _r = new Random(_seed);
            SetRandomFirstName();
            _r = new Random(_seed);
            SetRandomLastName();
            _r = new Random(_seed);
            SetKinderenLinks();
            _r = new Random(_seed);
            SetPartnerLinks();
            _r = new Random(_seed);
            _persoon.Naam.Aanschrijfwijze = $"{_persoon.Naam.Aanhef} {_persoon.Naam.Voorletters}. {_persoon.Naam.Geslachtsnaam}";
            _persoon.Geboorte = new Geboorte();
            _persoon.Geboorte.Datum = new Datum_onvolledig();
            DateTime birth = new DateTime(2020,1,1).AddDays(-_r.Next(104 * 365));
            _persoon.Geboorte.Datum.Dag = birth.Day;
            _persoon.Geboorte.Datum.Maand = birth.Month;
            _persoon.Geboorte.Datum.Jaar = birth.Year;
            _persoon.Geboorte.Datum.Datum = birth;
            _persoon.Geboorte.InOnderzoek = new GeboorteInOnderzoek();
            _persoon.Geboorte.InOnderzoek.DatumIngangOnderzoek = new Datum_onvolledig();
            _persoon.Geboorte.Land = new Waardetabel();
            _persoon.Geboorte.Plaats = new Waardetabel() { Code = "0000", Omschrijving = "Placeholder" };
            _persoon.Geboorte.Land.Omschrijving = "Nederland";
            _persoon.Geboorte.Land.Code = "6030";
            _persoon.Nationaliteit = new System.Collections.Generic.List<Nationaliteit>();
            _persoon.Nationaliteit.Add(new Nationaliteit()
            {
                RedenOpname = new Waardetabel() { Code = "0000", Omschrijving = "Placeholder" },
                DatumIngangGeldigheid = _persoon.Geboorte.Datum,
                Nationaliteit1 = new Waardetabel() { Code = "0001", Omschrijving = "Nederlandse" },
                InOnderzoek = new NationaliteitInOnderzoek()
                {
                    DatumIngangOnderzoek = new Datum_onvolledig()
                }

            });
            _persoon.InOnderzoek = new PersoonInOnderzoek();
            _persoon.InOnderzoek.DatumIngangOnderzoek = new Datum_onvolledig();
            _persoon.Naam.InOnderzoek = new NaamInOnderzoek();
            _persoon.Naam.InOnderzoek.DatumIngangOnderzoek = new Datum_onvolledig();
            _persoon.DatumEersteInschrijvingGBA = _persoon.Geboorte.Datum;
            return _persoon;
        }

        private void SetKinderenLinks()
        {
            var kinderen = _r.Next(0, _maxKinderen);
            if (kinderen == 0)
                    return;
            _persoon._links.Kinderen = new System.Collections.Generic.List<HalLink>();
            for (int i = 0; i < kinderen; i++)
            {
                _persoon._links.Kinderen.Add(new HalLink() { Title = string.Empty, Href = $"https://placeholder-uri/ingeschrevenpersonen/{_persoon.Burgerservicenummer}/kinderen/" + i });
            }
        }

        private void SetPartnerLinks()
        {
            var partners = _r.Next(0, 1);
            if (partners == 0)
                return;
            _persoon._links.Partners = new System.Collections.Generic.List<HalLink>();
            for (int i = 0; i < partners; i++)
            {
                _persoon._links.Kinderen.Add(new HalLink() {Title = string.Empty, Href = $"https://placeholder-uri/ingeschrevenpersonen/{_persoon.Burgerservicenummer}/partners/" + i });
            }
        }

        private void SetRandomAddress()
        {
            var entity =(
                from p in Adressen_Entity.Entities select p)
                .Skip(_r.Next(Adressen_Entity.Entities.Count)).FirstOrDefault();
            _persoon.Verblijfplaats.Huisnummer = int.Parse(entity.Huisnummer);
            _persoon.Verblijfplaats.Huisletter = entity.Huisletter;
            _persoon.Verblijfplaats.Postcode = entity.Postcode;
            _persoon.Verblijfplaats.Straatnaam = entity.Straatnaam;
            _persoon.Verblijfplaats.Woonplaatsnaam = entity.Woonplaats;
            _persoon.Verblijfplaats.IdentificatiecodeAdresseerbaarObject = string.Empty;
            _persoon.Verblijfplaats.Locatiebeschrijving = string.Empty;
            _persoon.Verblijfplaats.DatumAanvangAdreshouding = new Datum_onvolledig();  
            _persoon.Verblijfplaats.DatumIngangGeldigheid = new Datum_onvolledig();
            _persoon.Verblijfplaats.DatumInschrijvingInGemeente = new Datum_onvolledig();
            _persoon.Verblijfplaats.DatumVestigingInNederland = new Datum_onvolledig();
            _persoon.Verblijfplaats.GemeenteVanInschrijving = new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" };
            _persoon.Verblijfplaats.LandVanwaarIngeschreven = new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" };
            _persoon.Verblijfplaats.Locatiebeschrijving = string.Empty;
            _persoon.Verblijfplaats.NaamOpenbareRuimte = string.Empty;
            _persoon.Verblijfplaats.VerblijfBuitenland = new VerblijfBuitenland() { AdresRegel1="",AdresRegel2="",AdresRegel3="", Land = new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" } };
            _persoon.Verblijfplaats.InOnderzoek = new VerblijfplaatsInOnderzoek() { DatumIngangOnderzoek = new Datum_onvolledig()};
            _persoon.Verblijfplaats.Huisnummertoevoeging = string.Empty;
            _persoon.Verblijfplaats.IdentificatiecodeNummeraanduiding = string.Empty;
            _persoon._links = new IngeschrevenPersoon_links();
            _persoon._links.Self = new HalLink();
            _persoon._links.Self.Href = string.Empty;
            _persoon._links.Self.Title = string.Empty;
            _persoon._links.Ouders = new System.Collections.Generic.List<HalLink>();
            _persoon._links.Reisdocumenten = new System.Collections.Generic.List<HalLink>();
            _persoon._links.Kinderen = new System.Collections.Generic.List<HalLink>();
            _persoon._links.Partners = new System.Collections.Generic.List<HalLink>();
            _persoon._links.Partnerhistorie = new HalLink() { Href = string.Empty, Title = string.Empty };
            _persoon._links.Verblijfplaatshistorie = new HalLink() { Href = string.Empty, Title = string.Empty }; 
            _persoon._links.Verblijfstitelhistorie = new HalLink() { Href = string.Empty, Title = string.Empty };
            _persoon._links.NationaliteitHistorie = new HalLink() { Href = string.Empty, Title = string.Empty };
            _persoon._links.VerblijfplaatsNummeraanduiding = new HalLink() { Href = string.Empty, Title = string.Empty };
            _persoon._embedded = new IngeschrevenPersoon_embedded();
            _persoon._embedded.Kinderen = new System.Collections.Generic.List<KindHal>();
            _persoon._embedded.Ouders = new System.Collections.Generic.List<OuderHal>();
            _persoon._embedded.Partners = new System.Collections.Generic.List<PartnerHal>();
            _persoon.Kiesrecht = new Kiesrecht() { EinddatumUitsluitingEuropeesKiesrecht = new Datum_onvolledig(), EinddatumUitsluitingKiesrecht=new Datum_onvolledig() };
            _persoon.OpschortingBijhouding = new OpschortingBijhouding() { Datum = new Datum_onvolledig() };
            _persoon.Overlijden = new Overlijden() { Datum = new Datum_onvolledig(), Plaats= new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" },  Land= new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" }, InOnderzoek = new OverlijdenInOnderzoek() { DatumIngangOnderzoek = new Datum_onvolledig() } };
            _persoon.Gezagsverhouding = new Gezagsverhouding() { InOnderzoek = new GezagsverhoudingInOnderzoek() { DatumIngangOnderzoek = new Datum_onvolledig() } };
            _persoon.Verblijfstitel = new Verblijfstitel() { Aanduiding = new Waardetabel() { Code = "0000", Omschrijving = "Place  holder" },  DatumIngang = new Datum_onvolledig(), DatumEinde = new Datum_onvolledig(), InOnderzoek = new VerblijfstitelInOnderzoek() { DatumIngangOnderzoek = new Datum_onvolledig() } };
            _persoon.Reisdocumenten = new System.Collections.Generic.List<string>();

        }

        private void SetRandomFirstName()
        {
            var entity = (
                from p in Voornamen_Entity.Entities select p)
                .Skip(_r.Next(Voornamen_Entity.Entities.Count)).FirstOrDefault();
            var parts = entity.Voornaam.Split(' ');
            _persoon.Naam.GebruikInLopendeTekst = string.Empty;
            _persoon.Naam.Voorvoegsel = string.Empty;
            _persoon.Naam.Voornamen = parts[0];
            _persoon.Naam.Voorletters = parts[0][0].ToString();
            switch (parts[1])
            {
                case "(M)":
                    _persoon.Geslachtsaanduiding = Geslacht_enum.Man;
                    _persoon.Naam.Aanhef = "Dhr.";
                    break;
                case "(V)":
                    _persoon.Naam.Aanhef = "Mevr.";
                    _persoon.Geslachtsaanduiding = Geslacht_enum.Vrouw;
                    break;
                default:
                    _persoon.Naam.Aanhef = "Dhr. / Mevr.";
                    _persoon.Geslachtsaanduiding = Geslacht_enum.Onbekend;
                    break;
            }
        }

        private void SetRandomLastName()
        {
            var entity = (
                from p in Achternamen_Entity.Entities select p)
                .Skip(_r.Next(Achternamen_Entity.Entities.Count)).FirstOrDefault();
            _persoon.Naam.Geslachtsnaam = entity.Achternaam;
        }
    }
}
