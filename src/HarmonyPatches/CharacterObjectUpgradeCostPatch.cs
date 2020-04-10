using HarmonyLib;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(CharacterObject), "UpgradeCost")]
    public class CharacterObjectUpgradeCostPatch
    {
        static void Postfix(CharacterObject __instance, PartyBase party, int index, ref int __result)
        {
            if (party != null && party.LeaderHero == Hero.MainHero)
            {
                __result = (int)(__result * ConfigLoader.Instance.Config.PlayerTroopUpgradeCostMultiplier);
            }
        }

        static bool Prepare()
        {
            return ConfigLoader.Instance.Config.PlayerTroopUpgradeCostMultiplier != 1.0f; 
        }
    }
}
