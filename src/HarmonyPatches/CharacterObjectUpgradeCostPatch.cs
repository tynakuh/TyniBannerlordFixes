using System;
using HarmonyLib;
using System.Windows.Forms;
using TaleWorlds.CampaignSystem;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(CharacterObject), "UpgradeCost")]
    public class CharacterObjectUpgradeCostPatch
    {
        static void Postfix(PartyBase party, int index, ref int __result)
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
        
        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                MessageBox.Show(__exception.FlattenException());
            }
        }
    }
}
