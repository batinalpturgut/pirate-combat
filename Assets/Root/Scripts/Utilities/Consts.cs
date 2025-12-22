namespace Root.Scripts.Utilities
{
    public static class Consts
    {
        public static class SOFileNames
        {
            public const string PoisonousCaptain = nameof(PoisonousCaptain);
            public const string Heavy = nameof(Heavy);
            public const string Archer = nameof(Archer);
            public const string FreezingTower = nameof(FreezingTower);
            public const string RumTower = nameof(RumTower);
            public const string MusicTower = nameof(MusicTower);
            public const string LegendaryRarity = nameof(LegendaryRarity);
            public const string EpicRarity = nameof(EpicRarity);
            public const string RareRarity = nameof(RareRarity);
            public const string UncommonRarity = nameof(UncommonRarity);
            public const string CommonRarity = nameof(CommonRarity);
            public const string RaritySystem = nameof(RaritySystem);
            public const string EnemyShip = nameof(EnemyShip);
        }

        public static class SOMenuNames
        {
            private const string Captains = "Captains/";
            private const string DefenderShips = "DefenderShips/";
            private const string Towers = "Towers/";
            private const string Rarities = "Rarities/";
            private const string Hostiles = "Hostiles/";


            public const string PoisonousCaptainMenu = Captains + SOFileNames.PoisonousCaptain;
            public const string HeavyMenu = DefenderShips + SOFileNames.Heavy;
            public const string ArcherMenu = DefenderShips + SOFileNames.Archer;
            public const string FreezingTowerMenu = Towers + SOFileNames.FreezingTower;
            public const string RumTowerMenu = Towers + SOFileNames.RumTower;
            public const string MusicTowerMenu = Towers + SOFileNames.MusicTower;
            public const string LegendaryRarityMenu = Rarities + SOFileNames.LegendaryRarity;
            public const string EpicRarityMenu = Rarities + SOFileNames.EpicRarity;
            public const string RareRarityMenu = Rarities + SOFileNames.RareRarity;
            public const string UncommonRarityMenu = Rarities + SOFileNames.UncommonRarity;
            public const string CommonRarityMenu = Rarities + SOFileNames.CommonRarity;
            public const string RaritySystemMenu = Rarities + SOFileNames.RaritySystem;
            public const string EnemyShipMenu = Hostiles + SOFileNames.EnemyShip;
        }

        public static class Preprocessors
        {
            public const string UNITY_EDITOR = nameof(UNITY_EDITOR);
            public const string ENABLE_CHEATS = nameof(ENABLE_CHEATS);
        }

        public static class InspectorTitles
        {
            private const string DoubleDotSeparator = ":";

            public const string REFERENCES = nameof(REFERENCES) + DoubleDotSeparator;
            public const string SETTINGS = nameof(SETTINGS) + DoubleDotSeparator;
            public const string ANIMATION_SETTINGS = nameof(ANIMATION_SETTINGS) + DoubleDotSeparator;
        }
    }
}