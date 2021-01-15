using System;
using System.Collections.Generic;
using System.IO;
using static FrenchTranslation.FrenchTranslation.ConfigCulture;

namespace FrenchTranslation
{
    public class FrenchTranslation
    {
        public readonly string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public WordConfig wordConfig;
        public WordMale wordMale;
        public WordFemale wordFemale;
        public WordPlural wordPlural;
        public WordItem wordItem;

        public ConfigCulture configCulture;

        private static FrenchTranslation _instance;
        static readonly object instanceLock = new object();

        public static FrenchTranslation Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null) 
                            _instance = new FrenchTranslation();
                    }
                }
                return _instance;
            }
        }

        private FrenchTranslation()
        {
            string dataDirectory = Path.Combine(currentPath, @"..\..\Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }


            try
            {
                configCulture = XmlSerialization.ReadFromXmlFile<ConfigCulture>(Path.Combine(currentPath, @"..\..\Cultures.xml"));
            }
            catch
            {
                configCulture = GetBaseConfig();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Cultures.xml"), configCulture);
            }

            try
            {
                wordMale = XmlSerialization.ReadFromXmlFile<WordMale>(Path.Combine(currentPath, @"..\..\Data\Male.xml"));
            }
            catch
            {
                wordMale = GetWordMaleData();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Male.xml"), wordMale);
            }
            
            try
            {
                wordFemale = XmlSerialization.ReadFromXmlFile<WordFemale>(Path.Combine(currentPath, @"..\..\Data\Female.xml"));
            }
            catch
            {
                wordFemale = GetWordFemaleData();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Female.xml"), wordFemale);
            }

            try
            {
                wordPlural = XmlSerialization.ReadFromXmlFile<WordPlural>(Path.Combine(currentPath, @"..\..\Data\Plural.xml"));
            }
            catch
            {
                wordPlural = GetWordPluralData();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Plural.xml"), wordPlural);
            }

            try
            {
                wordItem = XmlSerialization.ReadFromXmlFile<WordItem>(Path.Combine(currentPath, @"..\..\Data\Item.xml"));
            }
            catch
            {
                wordItem = GetWordItemData();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Item.xml"), wordItem);
            }

            try
            {
                wordConfig = XmlSerialization.ReadFromXmlFile<WordConfig>(Path.Combine(currentPath, @"..\..\Data\Config.xml"));
            }
            catch
            {
                wordConfig = GetWordConfigData();
                XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Config.xml"), wordConfig);
            }

            List<Culture> cultures = new List<Culture>
            {
                configCulture.Aserai,
                configCulture.Battanians,
                configCulture.Empire,
                configCulture.Khuzaits,
                configCulture.Sturgians,
                configCulture.Vlandians
            };


            bool save = false;
            foreach (Culture culture in cultures)
            {
                if (AddCultureToList(culture))
                {
                    save = true;
                }
            }
            if (save)
            {
                SaveData();
            }
        }

        public void SaveData()
        {
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Male.xml"), wordMale);
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Female.xml"), wordFemale);
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Plural.xml"), wordPlural);
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Item.xml"), wordItem);
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Data\Config.xml"), wordConfig);
            XmlSerialization.WriteToXmlFile(Path.Combine(currentPath, @"..\..\Cultures.xml"), configCulture);
        }

        public bool AddCultureToList(Culture culture)
        {
            bool save = true;
            string poorValue;

            string male = culture.Male.ToLower();
            if (!wordMale.Words.TryGetValue(male, out poorValue))
            {
                wordMale.Words.Add(male, male);
                save = true;
            }

            string female = culture.Female.ToLower();
            if (!wordFemale.Words.TryGetValue(female, out poorValue))
            {
                wordFemale.Words.Add(female, female);
                save = true;
            }

            string plural = culture.MalePlural.ToLower();
            if (!wordPlural.Words.TryGetValue(plural, out poorValue))
            {
                wordPlural.Words.Add(plural, plural);
                save = true;
            }

            string country = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(culture.Region.ToLower());
            string twoLastChar = country.Substring(country.Length - 2);

            if (wordConfig.RegionException.Male.TryGetValue(country, out poorValue))
            {
                if (!wordMale.Words.TryGetValue(country, out poorValue))
                {
                    wordMale.Words.Add(country, country);
                    save = true;
                }
            }
            else if (wordConfig.RegionException.Female.TryGetValue(country, out poorValue))
            {
                if (!wordFemale.Words.TryGetValue(country, out poorValue))
                {
                    wordFemale.Words.Add(country, country);
                    save = true;
                }
            }
            else if (wordConfig.RegionException.Plural.TryGetValue(country, out poorValue))
            {
                if (!wordPlural.Words.TryGetValue(country, out poorValue))
                {
                    wordPlural.Words.Add(country, country);
                    save = true;
                }
            }
            else if (twoLastChar == "ia" || twoLastChar[1] == 'e')
            {
                if (!wordFemale.Words.TryGetValue(country, out poorValue))
                {
                    wordFemale.Words.Add(country, country);
                    save = true;
                }
            }
            else if (twoLastChar[1] == 's')
            {
                if (!wordPlural.Words.TryGetValue(country, out poorValue))
                {
                    wordPlural.Words.Add(country, country);
                    save = true;
                }
            }
            else
            {
                if (!wordMale.Words.TryGetValue(country, out poorValue))
                {
                    wordMale.Words.Add(country, country);
                    save = true;
                }
            }

            return save;
        }

        public void RemoveCultureToList(Culture culture)
        {
            string poorValue;

            string male = culture.Male.ToLower();
            if (wordMale.Words.TryGetValue(male, out poorValue))
            {
                wordMale.Words.Remove(male);
            }

            string female = culture.Female.ToLower();
            if (wordFemale.Words.TryGetValue(female, out poorValue))
            {
                wordFemale.Words.Remove(female);
            }

            string plural = culture.MalePlural.ToLower();
            if (wordPlural.Words.TryGetValue(plural, out poorValue))
            {
                wordPlural.Words.Remove(plural);
            }

            string region = culture.Region;
            if (wordMale.Words.TryGetValue(region, out poorValue))
            {
                wordMale.Words.Remove(region);
            }
            else if (wordFemale.Words.TryGetValue(region, out poorValue))
            {
                wordFemale.Words.Remove(region);
            }
            else if (wordPlural.Words.TryGetValue(region, out poorValue))
            {
                wordPlural.Words.Remove(region);
            }
        }

        [Serializable]
        public class ConfigCulture
        {
            public abstract class Culture
            {
                public string Male { get; set; }
                public string Female { get; set; }
                public string MalePlural { get; set; }
                public string FemalePlural { get; set; }
                public string Region { get; set; }
            }
            public CultureAserai Aserai { get; set; }
            [Serializable]
            public class CultureAserai : Culture { }

            public CultureBattanians Battanians { get; set; }
            [Serializable]
            public class CultureBattanians : Culture { }

            public CultureEmpire Empire { get; set; }
            [Serializable]
            public class CultureEmpire : Culture { }

            public CultureKhuzaits Khuzaits { get; set; }
            [Serializable]
            public class CultureKhuzaits : Culture { }

            public CultureSturgians Sturgians { get; set; }
            [Serializable]
            public class CultureSturgians : Culture { }

            public CultureVlandians Vlandians { get; set; }
            [Serializable]
            public class CultureVlandians : Culture { }
        }
        public ConfigCulture GetBaseConfig()
        {
            return new ConfigCulture
            {
                Aserai = new ConfigCulture.CultureAserai
                {
                    Male = "Azeri",
                    Female = "Azeri",
                    MalePlural = "Azerii",
                    FemalePlural = "Azerii",
                    Region = "Azeraï",
                },
                Battanians = new ConfigCulture.CultureBattanians
                {
                    Male = "Battanois",
                    Female = "Battanoise",
                    MalePlural = "Battanois",
                    FemalePlural = "Battanoises",
                    Region = "Battania",
                },
                Empire = new ConfigCulture.CultureEmpire
                {
                    Male = "Impérial",
                    Female = "Impériale",
                    MalePlural = "Impériaux",
                    FemalePlural = "Impériales",
                    Region = "Empire",
                },
                
                Khuzaits = new ConfigCulture.CultureKhuzaits
                {
                    Male = "Khuzan",
                    Female = "Khuzane",
                    MalePlural = "Khuzans",
                    FemalePlural = "Khuzanes",
                    Region = "Khuzait",
                },
                Sturgians = new ConfigCulture.CultureSturgians
                {
                    Male = "Sturgue",
                    Female = "Sturgine",
                    MalePlural = "Sturgues",
                    FemalePlural = "Sturgues",
                    Region = "Sturgie",
                },
                Vlandians = new ConfigCulture.CultureVlandians
                {
                    Male = "Vlandien",
                    Female = "Vlandienne",
                    MalePlural = "Vlandiens",
                    FemalePlural = "Vlandiennes",
                    Region = "Vlandia",
                },
            };
        }

        [Serializable]
        public class WordConfig
        {
            public ConfigChars Chars { get; set; }
            [Serializable]
            public class ConfigChars 
            {
                public string Vowels { get; set; }
                public string Consonants { get; set; }
            }

            public ConfigAdjective Adjective { get; set; }
            [Serializable]
            public class ConfigAdjective
            {
                public XmlSerialization.SerializableDictionary<string, string> Male { get; set; }
                public XmlSerialization.SerializableDictionary<string, string> Female { get; set; }
                public XmlSerialization.SerializableDictionary<string, string> MalePlural { get; set; }
                public XmlSerialization.SerializableDictionary<string, string> FemalePlural { get; set; }
            }

            public ConfigRegionException RegionException { get; set; }
            [Serializable]
            public class ConfigRegionException
            {
                public XmlSerialization.SerializableDictionary<string, string> Male { get; set; }
                public XmlSerialization.SerializableDictionary<string, string> Female { get; set; }
                public XmlSerialization.SerializableDictionary<string, string> Plural { get; set; }
            }

            public XmlSerialization.SerializableDictionary<string, string> JobName { get; set; }
        }

        public WordConfig GetWordConfigData()
        {
            return new WordConfig
            {
                Chars = new WordConfig.ConfigChars
                {
                    Vowels = "aeiouyàâéèêhAEIOUYÀÂÉÈÊH",
                    Consonants = "bcdfgjklmnpqrstvwxzBCDFGJKLMNPQRSTVWXZ",
                },
                Adjective = new WordConfig.ConfigAdjective
                {
                    Male = new XmlSerialization.SerializableDictionary<string, string> { },
                    Female = new XmlSerialization.SerializableDictionary<string, string>
                    {
                        { "ancien", "ancienne" },
                        { "collecté", "collectée" },
                        { "léger", "légère" },
                        { "épais", "épaisse" },

                    },
                    MalePlural = new XmlSerialization.SerializableDictionary<string, string>
                    {
                        { "ancien", "anciens" },
                        { "collecté", "collectés" },
                        { "léger", "légers" },
                        { "épais", "épais" },
                    },
                    FemalePlural = new XmlSerialization.SerializableDictionary<string, string>
                    {
                        { "ancien", "anciennes" },
                        { "collecté", "collectées" },
                        { "léger", "légères" },
                        { "épais", "épaisses" },
                    },
                },
                RegionException = new WordConfig.ConfigRegionException
                {
                    Male = new XmlSerialization.SerializableDictionary<string, string>
                    {
                        { "Cambodge", "Cambodge" },
                        { "Mexique", "Mexique" },
                        { "Mozambique", "Mozambique" },
                        { "Zimbabwe", "Zimbabwe" },
                    },
                    Female = new XmlSerialization.SerializableDictionary<string, string> { },
                    Plural = new XmlSerialization.SerializableDictionary<string, string> { },
                },
                JobName = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "Artisan", "Artisane" },
                    { "Brasseur", "Brasseuse" },
                    { "Drapier", "Drapière" },
                    { "Ferronnier", "Ferronnière" },
                    { "Menuisier", "Menuisière" },
                    { "Potier", "Potière" },
                    { "Presseur d'huile", "Presseuse d'huile" },
                    { "Tanneur", "Tanneuse" },
                    { "Tisserand de laine", "Tisserande de laine" },
                    { "Tisserand de velours", "Tisserande de velours" },
                    { "Vigneron", "Vigneronne" },
                },
            };
        }
        
        [Serializable]
        public class WordMale
        {
            public XmlSerialization.SerializableDictionary<string, string> Words { get; set; }
        }

        [Serializable]
        public class WordFemale
        {
            public XmlSerialization.SerializableDictionary<string, string> Words { get; set; }
        }

        [Serializable]
        public class WordPlural
        {
            public XmlSerialization.SerializableDictionary<string, string> Words { get; set; }
        }

        public WordMale GetWordMaleData()
        {
            return new WordMale
            {
                Words = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "artisan", "artisan" },
                    { "aserai", "aserai" },
                    { "atelier", "atelier" },
                    { "bosquet", "bosquet" },
                    { "château", "château" },
                    { "empire", "empire" },
                    { "haut-royaume", "haut-royaume" },
                    { "khuzait", "khuzait" },
                    { "peuple", "peuple" },
                    { "pâturage", "pâturage" },
                    { "royaume", "royaume" },
                    { "sultanat", "sultanat" },
                },
            };
        }

        public WordFemale GetWordFemaleData()
        {
            return new WordFemale
            {
                Words = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "armée", "armée" },
                    { "avant-bras", "avant-bras" },
                    { "bandages", "bandages" },
                    { "battania", "battania" },
                    { "bijoux", "bijoux" },
                    { "boutique", "boutique" },
                    { "braise", "braise" },
                    { "brasserie", "brasserie" },
                    { "cachette", "cachette" },
                    { "caravane", "caravane" },
                    { "clairière", "clairière" },
                    { "compagnie", "compagnie" },
                    { "fabrique", "fabrique" },
                    { "forge", "forge" },
                    { "fraternité", "fraternité" },
                    { "garnison", "garnison" },
                    { "légion", "légion" },
                    { "main", "main" },
                    { "milice", "milice" },
                    { "orféverie", "orféverie" },
                    { "presse", "presse" },
                    { "principauté", "principauté" },
                    { "protège-bras", "protège-bras" },
                    { "ruelle", "ruelle" },
                    { "sturgia", "sturgia" },
                    { "tannerie", "tannerie" },
                    { "tourbière", "tourbière" },
                    { "vlandia", "vlandia" },
                    { "échoppe", "échoppe" },
                },
            };
        }

        public WordPlural GetWordPluralData()
        {
            return new WordPlural
            {
                Words = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "aserais", "aserais" },
                    { "bandits", "bandits" },
                    { "battanians", "battanians" },
                    { "caravanes", "caravanes" },
                    { "impériaux", "impériaux" },
                    { "khuzaits", "khuzaits" },
                    { "pillards", "pillards" },
                    { "pirates", "pirates" },
                    { "quais", "quais" },
                    { "sturgians", "sturgians" },
                    { "vlandians", "vlandians" },
                },
            };
        }


        [Serializable]
        public class WordItem
        {
            public XmlSerialization.SerializableDictionary<string, string> Male { get; set; }
            public XmlSerialization.SerializableDictionary<string, string> Female { get; set; }
            public XmlSerialization.SerializableDictionary<string, string> MalePlural { get; set; }
            public XmlSerialization.SerializableDictionary<string, string> FemalePlural { get; set; }
        }

        public WordItem GetWordItemData()
        {
            return new WordItem
            {
                Male = new XmlSerialization.SerializableDictionary<string, string> { },
                Female = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "ahlspiess", "ahlspiess" },
                    { "anicroche", "anicroche" },
                    { "arbalète", "arbalète" },
                    { "armure", "armure" },
                    { "bannière", "bannière" },
                    { "barde", "barde" },
                    { "bardiche", "bardiche" },
                    { "bière", "bière" },
                    { "calotte", "calotte" },
                    { "cape", "cape" },
                    { "capuche", "capuche" },
                    { "casquette", "casquette" },
                    { "cervelière", "cervelière" },
                    { "chemise", "chemise" },
                    { "coiffe", "coiffe" },
                    { "coiffure", "coiffure" },
                    { "cotte", "cotte" },
                    { "couronne", "couronne" },
                    { "cuirasse", "cuirasse" },
                    { "dague", "dague" },
                    { "demi-barde", "demi-barde" },
                    { "dishdasha", "dishdasha" },
                    { "falx", "falx" },
                    { "faucille", "faucille" },
                    { "faux", "faux" },
                    { "flissa", "flissa" },
                    { "flèche", "flèche" },
                    { "fourche", "fourche" },
                    { "fourrure", "fourrure" },
                    { "francisque", "francisque" },
                    { "fronde", "fronde" },
                    { "furie-des-vents", "furie-des-vents" },
                    { "guisarme", "guisarme" },
                    { "hache", "hache" },
                    { "hachette", "hachette" },
                    { "hallebarde", "hallebarde" },
                    { "houe", "houe" },
                    { "javeline", "javeline" },
                    { "jupe", "jupe" },
                    { "laine", "laine" },
                    { "lame", "lame" },
                    { "lamellaire", "lamellaire" },
                    { "lance", "lance" },
                    { "maille", "maille" },
                    { "masse", "masse" },
                    { "monture", "monture" },
                    { "peau", "peau" },
                    { "pierre", "pierre" },
                    { "pioche", "pioche" },
                    { "pique", "pique" },
                    { "robe", "robe" },
                    { "rondache", "rondache" },
                    { "selle", "selle" },
                    { "shishpar", "shishpar" },
                    { "sparth", "sparth" },
                    { "spatha", "spatha" },
                    { "tenue", "tenue" },
                    { "toque", "toque" },
                    { "torche", "torche" },
                    { "tunique", "tunique" },
                    { "tête", "tête" },
                    { "tête-de-loup", "tête-de-loup" },
                    { "vache", "vache" },
                    { "veste", "veste" },
                    { "écharpe", "écharpe" },
                    { "épée", "épée" },
                },
                MalePlural = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "avant-bras", "avant-bras" },
                    { "bandages", "bandages" },
                    { "bijoux", "bijoux" },
                    { "bracelets", "bracelets" },
                    { "brassards", "brassards" },
                    { "carreaux", "carreaux" },
                    { "couteaux", "couteaux" },
                    { "crispins", "crispins" },
                    { "gantelets", "gantelets" },
                    { "gants", "gants" },
                    { "habits", "habits" },
                    { "haillons", "haillons" },
                    { "javelots", "javelots" },
                    { "mocassins", "mocassins" },
                    { "protège-bras", "protège-bras" },
                    { "rembourrages", "rembourrages" },
                    { "vêtements", "vêtements" },
                },
                FemalePlural = new XmlSerialization.SerializableDictionary<string, string>
                {
                    { "bandes", "bandes" },
                    { "bottes", "bottes" },
                    { "chausses", "chausses" },
                    { "chaussures", "chaussures" },
                    { "cuissardes", "cuissardes" },
                    { "dagues", "dagues" },
                    { "flèches", "flèches" },
                    { "haches", "haches" },
                    { "javelines", "javelines" },
                    { "lances", "lances" },
                    { "mailles", "mailles" },
                    { "mitaines", "mitaines" },
                    { "robes", "robes" },
                    { "spallières", "spallières" },
                    { "épaules", "épaules" },
                    { "épaulières", "épaulières" },
                },
            };
        }
    }
}