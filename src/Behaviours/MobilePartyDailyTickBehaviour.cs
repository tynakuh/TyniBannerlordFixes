using TaleWorlds.CampaignSystem;
using System;

namespace TyniBannerlordFixes
{
    class MobilePartyDailyTickBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.addXp));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void addXp(MobileParty party)
        {
            if (!party.IsActive)
                return;

            bool hasCombatTips = party.HasPerk(DefaultPerks.Leadership.CombatTips);
            bool hasRaiseTheMeek = party.HasPerk(DefaultPerks.Leadership.RaiseTheMeek);

            if (!hasCombatTips && !hasRaiseTheMeek)
                return;

            for (int i = 0; i < party.MemberRoster.Count; i++)
            {
                TroopRosterElement troopElement = party.MemberRoster.GetElementCopyAtIndex(i);
                int troopMultiplier = troopElement.Number;
                int totalTroopXp = 0;

                // If we scaleByReadyToUpgrade, ignore the amount of units ready to upgrade
                if (ConfigLoader.Instance.Config.ScaleByReadyToUpgrade)
                {
                    troopMultiplier -= troopElement.NumberReadyToUpgrade;
                }

                if (hasCombatTips)
                {
                    // Add combatTips xp
                    totalTroopXp += ConfigLoader.Instance.Config.CombatTipsXpAmount * troopMultiplier;

                    // Remove the default added xp
                    totalTroopXp -= Campaign.Current.Models.PartyTrainingModel.GetPerkExperiencesForTroops(DefaultPerks.Leadership.CombatTips);
                }

                if (hasRaiseTheMeek && troopElement.Character.Tier < 4)
                {
                    // Add raiseTheMeek xp
                    totalTroopXp += ConfigLoader.Instance.Config.RaiseTheMeekXpAmount * troopMultiplier;

                    // Remove the default added xp only if we haven't removed it for CombatTips, native doesn't support both
                    // even if its technically possible as party can have multiple leaders. It only applies CombatTips instead.
                    if (!hasCombatTips)
                    {
                        totalTroopXp -= Campaign.Current.Models.PartyTrainingModel.GetPerkExperiencesForTroops(DefaultPerks.Leadership.RaiseTheMeek);
                    }
                }

                // Add xp to the troop
                party.Party.MemberRoster.AddXpToTroopAtIndex(totalTroopXp, i);
            }
        }
    }
}