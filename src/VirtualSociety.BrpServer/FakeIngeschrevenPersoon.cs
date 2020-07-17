using Brp.Api;
using System;
using System.Linq;
using Vs.FactTables;

namespace VirtualSociety.BrpServer
{
    public class FakeIngeschrevenPersoon
    {
        private int _seed;
        private IngeschrevenPersoonHal _persoon;
        private Random _r;

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
            _persoon.Naam.Aanschrijfwijze = $"{_persoon.Naam.Aanhef} {_persoon.Naam.Voorletters}. {_persoon.Naam.Geslachtsnaam}";
            _persoon.Geboorte = new Geboorte();
            _persoon.Geboorte.Datum = new Datum_onvolledig();
            DateTime birth = new DateTime(2020,1,1).AddDays(-_r.Next(104 * 365));
            _persoon.Geboorte.Datum.Dag = birth.Day;
            _persoon.Geboorte.Datum.Maand = birth.Month;
            _persoon.Geboorte.Datum.Jaar = birth.Year;
            _persoon.Geboorte.Datum.Datum = birth;
            _persoon.Geboorte.Land = new Waardetabel();
            _persoon.Geboorte.Land.Omschrijving = "Nederland";
            _persoon.Geboorte.Land.Code = "6030";
            _persoon.Nationaliteit = new System.Collections.Generic.List<Nationaliteit>();
            _persoon.Nationaliteit.Add(new Nationaliteit()
            {
                DatumIngangGeldigheid = _persoon.Geboorte.Datum,
                Nationaliteit1 = new Waardetabel() { Code = "0001", Omschrijving = "Nederlandse" }
            });
            _persoon.DatumEersteInschrijvingGBA = _persoon.Geboorte.Datum;
            return _persoon;
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
        }

        private void SetRandomFirstName()
        {
            var entity = (
                from p in Voornamen_Entity.Entities select p)
                .Skip(_r.Next(Voornamen_Entity.Entities.Count)).FirstOrDefault();
            var parts = entity.Voornaam.Split(' ');
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
