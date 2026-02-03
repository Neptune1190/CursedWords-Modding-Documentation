using MelonLoader;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(CustomCharacters.Core), "Core Character Patchs", "1.0.0", "Neptune6866")]
[assembly: MelonGame("Buried Things", "Cursed Words")]

namespace CustomCharacters
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("[Core Character Patchs] Mod initialized!");
        }

        public static Sprite LoadEmbeddedSprite(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            using var stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                MelonLogger.Warning($"Embedded sprite {resourceName} not found!");
                return null;
            }

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            tex.LoadImage(bytes);
            tex.Apply();

            return Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }
    }


    [HarmonyPatch(typeof(SaveManager), "IsCharacterUnlocked", new Type[] { typeof(Type) })]
    internal static class SaveManager_IsCharacterUnlocked_Patch
    {
        private static bool Prefix(Type characterType, ref bool __result)
        {
            if (!typeof(Character).IsAssignableFrom(characterType))
                return true;

            
            if (characterType.Assembly.GetName().Name != "Assembly-CSharp")
            {
                __result = true;
                return false;
            }

            return true; 
        }
    }

    [HarmonyPatch(typeof(CharacterSelectController), "GetCharacters")]
    internal static class CharacterSelectController_GetCharacters_Patch
    {
        private static void Postfix(CharacterSelectController __instance)
        {
            var dictField = __instance.GetType().GetField("_characters", BindingFlags.Instance | BindingFlags.NonPublic);
            if (dictField == null) return;

            var characters = dictField.GetValue(__instance) as Dictionary<Type, bool>;
            if (characters == null) return;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.GetTypes())
                {
                    if (!type.IsClass || !typeof(Character).IsAssignableFrom(type)) continue;

                    // Only add modded characters to the select dictionary
                    if (type.Assembly.GetName().Name != "Assembly-CSharp" && !characters.ContainsKey(type))
                    {
                        characters[type] = true;
                        MelonLogger.Msg($"[Core Character Patchs] Unlocked modded character: {type.Name}");
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.GetSaveFileName))]
    internal static class SaveManager_GetSaveFileName_Patch
    {
        private static void Postfix(ref string __result, int slotIndex)
        {
            if (__result.Contains("slot_") && !__result.StartsWith("modded_"))
                __result = "modded_" + __result;
        }
    }

    // Example modded character
    public class CustomCharacter : Character
    {
        public CustomCharacter()
            : base(
                "CustomCharacter",
                "This is a custom character test.",
                new Vowelizer(),
                "CatFish",
                new Color32(180, 111, 74, byte.MaxValue),
                new Color32(98, 158, 233, byte.MaxValue),
                Vector3.zero,
                Vector3.zero,
                new List<Type> { typeof(Pear), typeof(Egg), typeof(Doughnut) },
                Emotions.DinosaurPowerful,
                false,
                ""
            )
        {
        }
    }

    // Example modded item
    public class Vowelizer : Item
    {
        public int MostRecentUpgrade;

        public Vowelizer()
        {
            Name = "Vowelizer";
            SpriteData.Add(new ItemSpriteData(ItemSpriteUsage.Default, "Vowelizer"));

            Sprite sprite = Core.LoadEmbeddedSprite("CorePatchs.Resources.Vowelizer.png");
            if (sprite != null)
            {
                foreach (var s in SpriteData)
                    s.ItemSprite = sprite;
                MelonLogger.Msg("[Core Character Patchs] Vowelizer sprite loaded successfully!");
            }

            Rarity = ItemRarity.Unique;
            UpgradeableComponents = new List<UpgradeableComponent>
            {
                new UpgradeableComponent(1, 1, 1),
                new UpgradeableComponent(1, 8, 8)
            };
            Tags = new List<ItemTag> { ItemTag.CurseBuild };
            PinColors = new List<Color>
            {
                new Color32(byte.MaxValue, 183, 77, byte.MaxValue),
                new Color32(byte.MaxValue, 87, 34, byte.MaxValue)
            };
            ItemFunctionTags = new List<ItemFunctionTag>
            {
                ItemFunctionTag.Scatterer,
                ItemFunctionTag.SpecificAdditive
            };
        }

        public override string GetDescription()
        {
            string text = "START OF GRID: Turn most tiles into vowels.";
            if (MostRecentUpgrade == 0) text += " +1 Vowel becomes a wildcard (?)";
            if (MostRecentUpgrade == 1) text += " +8 score per word length";
            return text;
        }

        public override void Upgrade(int componentIndex)
        {
            base.Upgrade(componentIndex);
            MostRecentUpgrade = componentIndex;
        }

        public override GridData ApplyStartOfGridEffect(
            GridData gridData,
            int gridNumber,
            int numberOfGrids,
            List<HistoricWord> previousWords,
            List<BoardGenVizInfo> vizSteps,
            bool isReroll)
        {
            var affected = new List<Tile>();
            var all = new List<Tile>();
            string[] vowels = { "A", "E", "I", "O", "U", "Y" };
            int attempts = gridData.Width * gridData.Height * 3;

            for (int i = 0; i < attempts; i++)
            {
                Tile t = GridUtility.Singleton.GetTileForItemScatter(
                    gridData,
                    TileType.Normal,
                    false,
                    null,
                    false
                );

                if (t != null && t.GetGlyphType() == GlyphType.Letter)
                {
                    t.SetLetter(vowels[UnityEngine.Random.Range(0, vowels.Length)]);
                    affected.Add(t);
                    all.Add(t);
                }
            }

            if (MostRecentUpgrade >= 0 && all.Count > 0)
                all[UnityEngine.Random.Range(0, all.Count)]
                    .SetGlyphType(GlyphType.Blank);

            if (affected.Count > 0)
                vizSteps.Add(new BoardGenVizInfo(gridData, this, affected, false, false, false, false, false));

            return gridData;
        }
    }
}
