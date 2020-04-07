using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaleWorlds.CampaignSystem;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(MapEventParty), "CommitXpGain")]
    public class MapEventPartyHarmonyPatch
    {
        static bool Prefix(MapEventParty __instance)
        {
            foreach (FlattenedTroopRosterElement item in new FlattenedTroopRoster(__instance.Troops))
            {
                int adjustedExperience = (int) Math.Round((double) item.XpGained / getTroopXpDivisor(__instance.Party.MemberRoster, item));
                __instance.Troops[item.Descriptor] = new FlattenedTroopRosterElement(item.Troop, item.State, item.Xp,item.Conformity, item.Descriptor, adjustedExperience);
            }
            return true;
        }

        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                MessageBox.Show(Utils.FlattenException(__exception));
            }
        }

        //If performance is an issue let's memoize this, reason I haven't done that off the bat is
        //there is no identifier I can use to link the same troop types that i can find right now.
        private static double getTroopXpDivisor(TroopRoster roster, FlattenedTroopRosterElement item)
        {
            int indexOfTroop = roster.FindIndexOfTroop(item.Troop);
            if(indexOfTroop == -1)
            {
                return 1;
            }
            TroopRosterElement element = roster.GetElementCopyAtIndex(indexOfTroop);
            if(element.NumberReadyToUpgrade == 0 || element.Number == 0)
            {
                return 1;
            }
            return (double) element.Number / (double) element.NumberReadyToUpgrade;
        }



        static bool Prepare()
        {
            return ConfigLoader.Instance.Config.ScaleByReadyToUpgrade;
        }
    }
}
