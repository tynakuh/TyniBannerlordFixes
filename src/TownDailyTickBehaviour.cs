using TaleWorlds.CampaignSystem;
using System;
using TaleWorlds.Localization;
using TaleWorlds.Core;

namespace ExperiencePerkFix
{
    class TownDailyTickBehaviour : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            //Strangely DailyTickTownEvent (Even if Castles are Towns) does not include Castles, only actual Towns.
            //So we will have to take the settlement event and cast it.
            CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.addXpToGarrison));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void addXpToGarrison(Settlement settle)
        {
            var town = settle.Town;

            if (town != null)
            {
                if (town.GarrisonChange > 1 && (town.GarrisonParty == null || !town.GarrisonParty.IsActive))
                {
                    town.Owner.Settlement.AddGarrisonParty();
                }
                if (town.GarrisonParty != null && town.GarrisonParty.IsActive && town.GarrisonParty.MapEvent == null && town.GarrisonParty.CurrentSettlement != null)
                {
                    int bonusXp = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateDailyTroopXpBonus(town);
                    float xpMultiplier = Campaign.Current.Models.DailyTroopXpBonusModel.CalculateGarrisonXpBonusMultiplier(town);
                    if (bonusXp > 0)
                    {
                        for (int i = 0; i < town.GarrisonParty.MemberRoster.Count; i++)
                        {
                            TroopRosterElement troopElement = town.GarrisonParty.MemberRoster.GetElementCopyAtIndex(i);
                            int totalTroopXp = TaleWorlds.Library.MathF.Round((float) (bonusXp * 
                                                                                       xpMultiplier * 
                                                                                       troopElement.Number * 
                                                                                       ConfigLoader.Instance.Config.GarrisonTrainingXpMultiplier) - (bonusXp * xpMultiplier));

                            town.GarrisonParty.MemberRoster.AddXpToTroopAtIndex(totalTroopXp, i);
                        }
                    }
                }
            }
		}
    }
}