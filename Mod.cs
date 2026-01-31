using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using HarmonyLib;
using UnityEngine;

namespace Dark.Signs
{
    public class Mod : Verse.Mod
    {
        public static List<Color> colorChoices = new List<Color>
        {
            Color.HSVToRGB(0f, 0f, 1f),
            Color.HSVToRGB(0f, 0.5f, 1f),
            Color.HSVToRGB(0f, 0.33f, 1f),
            Color.HSVToRGB(1f / 18f, 1f, 1f),
            Color.HSVToRGB(1f / 18f, 0.5f, 1f),
            Color.HSVToRGB(1f / 18f, 0.33f, 1f),
            Color.HSVToRGB(1f / 9f, 1f, 1f),
            Color.HSVToRGB(1f / 9f, 0.5f, 1f),
            Color.HSVToRGB(1f / 9f, 0.33f, 1f),
            Color.HSVToRGB(1f / 6f, 1f, 1f),
            Color.HSVToRGB(1f / 6f, 0.5f, 1f),
            Color.HSVToRGB(1f / 6f, 0.33f, 1f),
            Color.HSVToRGB(2f / 9f, 1f, 1f),
            Color.HSVToRGB(2f / 9f, 0.5f, 1f),
            Color.HSVToRGB(2f / 9f, 0.33f, 1f),
            Color.HSVToRGB(5f / 18f, 1f, 1f),
            Color.HSVToRGB(5f / 18f, 0.5f, 1f),
            Color.HSVToRGB(5f / 18f, 0.33f, 1f),
            Color.HSVToRGB(1f / 3f, 1f, 1f),
            Color.HSVToRGB(1f / 3f, 0.5f, 1f),
            Color.HSVToRGB(1f / 3f, 0.33f, 1f),
            Color.HSVToRGB(7f / 18f, 1f, 1f),
            Color.HSVToRGB(7f / 18f, 0.5f, 1f),
            Color.HSVToRGB(7f / 18f, 0.33f, 1f),
            Color.HSVToRGB(4f / 9f, 1f, 1f),
            Color.HSVToRGB(4f / 9f, 0.5f, 1f),
            Color.HSVToRGB(4f / 9f, 0.33f, 1f),
            Color.HSVToRGB(0.5f, 1f, 1f),
            Color.HSVToRGB(0.5f, 0.5f, 1f),
            Color.HSVToRGB(0.5f, 0.33f, 1f),
            Color.HSVToRGB(5f / 9f, 1f, 1f),
            Color.HSVToRGB(5f / 9f, 0.5f, 1f),
            Color.HSVToRGB(5f / 9f, 0.33f, 1f),
            Color.HSVToRGB(11f / 18f, 1f, 1f),
            Color.HSVToRGB(11f / 18f, 0.5f, 1f),
            Color.HSVToRGB(11f / 18f, 0.33f, 1f),
            Color.HSVToRGB(2f / 3f, 1f, 1f),
            Color.HSVToRGB(2f / 3f, 0.5f, 1f),
            Color.HSVToRGB(2f / 3f, 0.33f, 1f),
            Color.HSVToRGB(13f / 18f, 1f, 1f),
            Color.HSVToRGB(13f / 18f, 0.5f, 1f),
            Color.HSVToRGB(13f / 18f, 0.33f, 1f),
            Color.HSVToRGB(7f / 9f, 1f, 1f),
            Color.HSVToRGB(7f / 9f, 0.5f, 1f),
            Color.HSVToRGB(7f / 9f, 0.33f, 1f),
            Color.HSVToRGB(5f / 6f, 1f, 1f),
            Color.HSVToRGB(5f / 6f, 0.5f, 1f),
            Color.HSVToRGB(5f / 6f, 0.33f, 1f),
            Color.HSVToRGB(8f / 9f, 1f, 1f),
            Color.HSVToRGB(8f / 9f, 0.5f, 1f),
            Color.HSVToRGB(8f / 9f, 0.33f, 1f),
            Color.HSVToRGB(17f / 18f, 1f, 1f),
            Color.HSVToRGB(17f / 18f, 0.5f, 1f),
            Color.HSVToRGB(17f / 18f, 0.33f, 1f)
        };
        public Mod(ModContentPack content) : base(content)
        {
            GetSettings<Settings>();

            Harmony harmony = new Harmony("Dark.Signs");

            harmony.PatchAll();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            GetSettings<Settings>().DoWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Signs_SettingsCategory".Translate();
        }
    }
}
