using System;
using System.Linq;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Party;
using TaleWorlds.Localization;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(DefaultPartyWageModel), "GetTotalWage")]
    public class GetTotalWagePenaltyPatch
    {
        static void Postfix(ref int __result, MobileParty mobileParty, StatExplainer explanation = null)
        {
            if (Clan.PlayerClan.Parties.Contains(mobileParty))
            {
                ExplainedNumber result = new ExplainedNumber(0f, explanation);
                result.Add(__result,null);
                result.AddFactor(ConfigLoader.Instance.Config.PlayerTroopWageMultiplier - 1f, new TextObject("Player Troop Wage Multiplier (Tyni)"));
                __result = (int) result.ResultNumber;
            }
        }

        static bool Prepare()
        {
            return ConfigLoader.Instance.Config.PlayerTroopWageMultiplier != 1.0f;
        }
        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                MessageBox.Show(__exception.FlattenException());
            }
        }
    }
}