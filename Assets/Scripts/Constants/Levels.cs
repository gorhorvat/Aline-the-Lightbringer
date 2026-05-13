using System.Collections.Generic;
using UnityEngine;

public static class Levels
{
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

    static readonly Dictionary<string, string> loadingMessages = new Dictionary<string, string>
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

    public static string GetLoadingMessage(string levelName)
    {
        return loadingMessages.TryGetValue(levelName, out string message) ? $"Loading {message}" : "Loading...";
    }

    public static string GetDisplayValue(string levelName)
    {
        return loadingMessages.GetValueOrDefault(levelName, "Loading...");
    }
}