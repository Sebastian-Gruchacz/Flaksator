using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Obscureware.Flaksator;
using Obscureware.Flaksator.Data;

namespace DataOperations.Tests
{
    [TestFixture]
    public class CreateData
    {
        FlaksatorDataFactory factory = new FlaksatorDataFactory(
            @"Flaksator.sln|Data\Flaksator.db");
        //FlaksatorDataFactory factory = new FlaksatorDataFactory(
        //    @"Data|Data\Flaksator.db");

        [Test]
        [Ignore("Build-time only test")]
        public void Fill_in_title_strings()
        {
            using (var db = this.factory.CreateDatabase())
            {
                db.ListResources.SaveTitles(new[]
                {
                    @"{NN1P}",
                    @"{NN2P}",
                    @"{NN1F}",
                    @"{NN2F}",
                    @"{1:A???E} {1:NN1P}",
                    @"{1:A???E} {1:NN2P}",
                    @"{1:A???E} {1:NN1F}",
                    @"{1:A???E} {1:NN2F}",
                    @"{1:A???S} {1:NN1P}",
                    @"{1:A???S} {1:NN2P}",
                    @"{1:A???S} {1:NN1F}",
                    @"{1:A???S} {1:NN2F}",
                    @"{NN1N} {NG1F}",
                    @"{NN2T} {NG2N}",
                    @"{1:A???H} {1:NN1N}",
                    @"W {NN2N} {NG2F}",
                    @"{1:A???E} {1:NN1F} {2:A???E} {2:NG2P}",
                    @"{1:A???E} {1:NN2N} {2:A???E} {2:NG2F}",
                    @"{1:A???E} {1:NN1T} W {NL1N} {NG1T}",
                    @"{1:A???E} {1:NN1T} {2:A???E} {2:NG2P}",
                    @"{1:A???E} {1:NN1F} W {2:A???E} {2:NL1T}",
                    @"{1:A???E} {1:NN1F} W {NL1N} {2:A???E} {2:NG1T}",
                    @"{1:A???E} {1:NN1F} Pod {NA1T} {2:A???E} {2:NG1T}",
                    @"{1:A???E} {1:NN1T} {2:A???E} {2:NG1F}",
                    @"{1:A???S} {1:NN1F} {NG1P} {NG1F}",
                    @"{1:A???E} {1:NN2N} U {NG2F} {NG2N}",
                    @";{AN2NE} {NN2N} W {NL1N} {NG1T} {NG1T}",
                    @"{1:A???E} {1:NN2T} W {NL1N} {2:A???E} {2:NG1F}",
                    @"{1:A???E} {1:NN1F} U {NG1T} {NG1N}",
                    @"{1:A???E} {1:NN1P} W {NL1F} {2:A???E} {2:NG1T}",
                    @"{1:A???E} {1:NN1T} Nieopodal {2:A???E} {2:NG1T}",
                    @"{NN1F} {1:A???E} {1:NG2P}",
                    @"{NN2N} {1:A???E} {1:NG2F}",
                    @"{NN1T} W {NL1N} {NG1T}",
                    @"{NN1T} {1:A???E} {1:NG2P}",
                    @"{NN1F} W {1:A???E} {1:NL1T}",
                    @"{NN1F} W {NL1N} {1:A???E} {1:NG1T}",
                    @"{NN1F} Pod {NA1T} {1:A???E} {1:NG1T}",
                    @"{NN1T} {1:A???E} {1:NG1F}",
                    @"{NN1F} {NG1P} {NG1F}",
                    @"{NN2N} U {NG2F} {NG2N}",
                    @"{NN2N} W {NL1N} {NG1T} {NG1T}",
                    @"{NN2T} W {NL1N} {1:A???E} {1:NG1F}",
                    @"{NN1F} U {NG1T} {NG1N}",
                    @"{NN1P} W {NL1F} {1:A???E} {1:NG1T}",
                    @"{NN1T} Nieopodal {1:A???E} {1:NG1T}",
                    @"{1:A???E} {1:NN1F} Nordyckich Wojowników",
                    @"{1:A???E} {1:NN2N} Gwałconych Dziewic",
                    @"{1:A???E} {1:NN1T} W Świetle Mroku",
                    @"{1:A???E} {1:NN1T} Pogańskich Przodków",
                    @"{1:A???E} {1:NN1F} W Mrocznym Lesie",
                    @"{1:A???E} {1:NN1F} W Świetle Krwawego Księżyca",
                    @"{1:A???E} {1:NN1F} Pod Konarem Umierającego Dębu",
                    @"{1:A???E} {1:NN1T} Nadchodzącej Zguby",
                    @"{1:A???S} {1:NN1F} Pana Ciemności",
                    @"{1:A???E} {1:NN2N} U Bram Piekieł",
                    @"{1:A???E} {1:NN2N} W Obliczu Końca Świata",
                    @"{1:A???E} {1:NN2T} W Obliczu Narastającej Ciszy",
                    @"{1:A???E} {1:NN1F} U Kresu Istnienia",
                    @"{1:A???E} {1:NN1P} W Obliczu Wzmagającego Bólu",
                    @"{1:A???E} {1:NN1T} Nieopodal Mrocznego Dębu"
                });
            }
        }

        [Test]
        public void verify_that_some_titles_exist()
        {
            using (var db = this.factory.CreateDatabase())
            {
                var titles = db.ListResources.GetTitles();

                Assert.That(titles.Contains(@"{NN1N} {NG1F}"));
                Assert.That(titles.Contains(@"{NN1T} Nieopodal {1:A???E} {1:NG1T}"));
            }
        }
        
        [Test]
        //[Ignore("Build-time only test")]
        public void constant_piece_transaltionsFill_in_title_strings()
        {
            using (var db = this.factory.CreateDatabase())
            {
                var dic = new Dictionary<SongPiece, string>();
                dic.Add(SongPiece.Stanza, "--zwrotka--");
                dic.Add(SongPiece.Chorus, "chór");
                dic.Add(SongPiece.Bridge, "przejście");
                dic.Add(SongPiece.Interlude, "interludium");
                dic.Add(SongPiece.Refrain, "refren");

                db.DictionaryResources.SaveConstantSongPieces(dic);
            }
        }


        [Test]
        public void verify_that_some_constant_piece_transaltions_exist()
        {
            using (var db = this.factory.CreateDatabase())
            {
                var translations = db.DictionaryResources.GetConstantSongPieces();

                Assert.That(translations.ContainsKey(SongPiece.Chorus));
                Assert.That(translations[SongPiece.Chorus] == "chór");
            }
        }
    }
}
