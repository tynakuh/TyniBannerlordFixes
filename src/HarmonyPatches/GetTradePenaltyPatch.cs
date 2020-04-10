using HarmonyLib;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(DefaultTradeItemPriceFactorModel), "GetTradePenalty")]
    public class GetTradePenaltyPatch
    {
        static void Postfix(DefaultTradeItemPriceFactorModel __instance, ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand, ref float __result)
        {
            if (clientParty != null && clientParty.LeaderHero == Hero.MainHero && !item.IsTradeGood)
            {
                __result *= ConfigLoader.Instance.Config.PlayerEquipmentTradePenaltyMultiplier;
            }
        }

        static bool Prepare()
        {
            return ConfigLoader.Instance.Config.PlayerEquipmentTradePenaltyMultiplier != 1.0f; 
        }
    }
}
