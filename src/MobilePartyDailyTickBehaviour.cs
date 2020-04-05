using TaleWorlds.CampaignSystem;
using System;

namespace ExperiencePerkFix
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
            Boolean hasBoth = false;
            if (party.IsActive && party.HasPerk(DefaultPerks.Leadership.CombatTips) && party.HasPerk(DefaultPerks.Leadership.CombatTips))
            {
                hasBoth = true;
            }
            if (party.IsActive && party.HasPerk(DefaultPerks.Leadership.CombatTips))
            {
                for (int i = 0; i < party.MemberRoster.Count; i++)
                {
                    TroopRosterElement troopElement = party.MemberRoster.GetElementCopyAtIndex(i);
                    int totalTroopXp = 10 * troopElement.Number;

                    //Remove the default added xp
                    totalTroopXp -= Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.CombatTips);

                    party.Party.MemberRoster.AddXpToTroopAtIndex(totalTroopXp, i);

                }
            }
            if (party.IsActive && party.HasPerk(DefaultPerks.Leadership.RaiseTheMeek))
            {
                for (int i = 0; i < party.MemberRoster.Count; i++)
                {
                    TroopRosterElement troopElement2 = party.MemberRoster.GetElementCopyAtIndex(i);
                    if (troopElement2.Character.Tier < 4)
                    {
                        int defaultSingleTroopPerksXp2 = Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.RaiseTheMeek);
                        int totalTroopXp2 = ConfigLoader.Instance.Config.RaiseTheMeekXpAmount * troopElement2.Number;
                        if (!hasBoth)
                        {
                            //Remove the default added xp only if we haven't removed it for CombatTips, native doesn't support both
                            //even if its technically possible as party can have multiple leaders. It only applies CombatTips instead.
                            totalTroopXp2 -= Campaign.Current.Models.PartyTrainingModel.GetTroopPerksXp(DefaultPerks.Leadership.RaiseTheMeek);
                        }
                        party.Party.MemberRoster.AddXpToTroopAtIndex(totalTroopXp2, i);
                    }

                }
            }
        }
    }
}