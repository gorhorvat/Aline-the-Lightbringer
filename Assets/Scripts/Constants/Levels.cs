using System.Collections.Generic;

public static class Levels
{
    public const string MainMenu = "MainMenu";

    public const string LuminaGrove = "LuminaGrove";

    public static class Dev
    {
        public const string Zone = "Dev";
        public const string MechanicsDemo = "MechanicsDemo";
    }

    public static class VerdantHollow
    {
        public const string Zone = "VerdantHollow";
        public const string MossglowCanopy = "MossglowCanopy";
        public const string ThornwallRuins = "ThornwallRuins";
        public const string TheBioluminescentDeep = "TheBioluminescentDeep";
        public const string RootbridgeCrossing = "RootbridgeCrossing";
        public const string EmbervineThicket = "EmbervineThicket";
        public const string VerdantHollowHeart = "VerdantHollowHeart";
    }

    public static class ColdspireReach
    {
        public const string Zone = "ColdspireReach";
    }

    public static class Embervault
    {
        public const string Zone = "Embervault";
    }

    public static class Skyveil
    {
        public const string Zone = "Skyveil";
    }

    public static class Solhaven
    {
        public const string Zone = "Solhaven";
    }

    public static class Shadowmere
    {
        public const string Zone = "Shadowmere";
    }

    static readonly Dictionary<string, string> levelsDictionary = new()
    {
        { LuminaGrove, "Lumina Grove" },
        { Dev.Zone, "Dev" },
        { Dev.MechanicsDemo, "Mechanics Demo" },
        { VerdantHollow.Zone, "Verdant Hollow" },
        { VerdantHollow.MossglowCanopy, "Mossglow Canopy" },
        { VerdantHollow.ThornwallRuins, "Thornwall Ruins" },
        { VerdantHollow.TheBioluminescentDeep, "The Bioluminescent Deep" },
        { VerdantHollow.RootbridgeCrossing, "Rootbridge Crossing" },
        { VerdantHollow.EmbervineThicket, "Embervine Thicket" },
        { VerdantHollow.VerdantHollowHeart, "Verdant Hollow Heart" },
        { ColdspireReach.Zone, "Coldspire Reach" },
        { Embervault.Zone, "Embervault" },
        { Skyveil.Zone, "Skyveil" },
        { Solhaven.Zone, "Solhaven" },
        { Shadowmere.Zone, "Shadowmere" },
    };

    static readonly Dictionary<string, string> levelToZoneDictionary = new()
{
    { LuminaGrove, LuminaGrove },

    { Dev.Zone, Dev.Zone },
    { Dev.MechanicsDemo, Dev.Zone },

    { VerdantHollow.Zone, VerdantHollow.Zone },
    { VerdantHollow.MossglowCanopy, VerdantHollow.Zone },
    { VerdantHollow.ThornwallRuins, VerdantHollow.Zone },
    { VerdantHollow.TheBioluminescentDeep, VerdantHollow.Zone },
    { VerdantHollow.RootbridgeCrossing, VerdantHollow.Zone },
    { VerdantHollow.EmbervineThicket, VerdantHollow.Zone },
    { VerdantHollow.VerdantHollowHeart, VerdantHollow.Zone },

    { ColdspireReach.Zone, ColdspireReach.Zone },

    { Embervault.Zone, "Embervault" },

    { Skyveil.Zone, "Skyveil" },

    { Solhaven.Zone, "Solhaven" },

    { Shadowmere.Zone, "Shadowmere" }
};

    public static string GetLoadingMessage(string levelName)
    {
        return levelsDictionary.TryGetValue(levelName, out string message) ? $"Loading {message}" : "Loading...";
    }

    public static string GetDisplayValue(string levelName)
    {
        return levelsDictionary.GetValueOrDefault(levelName, "Loading...");
    }

    public static KeyValuePair<string, string> GetZoneForLevel(string levelKey)
    {
        string currentZone = LuminaGrove;
        if (levelToZoneDictionary.TryGetValue(levelKey, out string zoneKey))
        {
            if (!levelKey.Equals(zoneKey))
                currentZone = zoneKey;
        }

        return new KeyValuePair<string, string>(currentZone, GetDisplayValue(currentZone));
    }
}