using TaleWorlds.CampaignSystem;
using System;
using TaleWorlds.Core;
using MountAndBlade.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace TyniBannerlordFixes
{
    class PrisonerDailyTickBehaviour : CampaignBehaviorBase, IRecruitPrisonersCampaignBehavior, ICampaignBehavior
    {
        private float[] _dailyRecruitingProbabilityByTier;
        private float[] _dailyTierRecruitProbabilityDecay;


        public override void RegisterEvents()
        {
            //Appending 1.0 to all as there are currently no 'Tier 0' troops (They're all level 5 or higher)
            //this is just to make config easier for users as they won't be aware of Tier 0.
            string recruitmentTierConfig = ConfigLoader.Instance.Config.DailyRecruitmentProbabilitiesByTier;
            _dailyRecruitingProbabilityByTier =
                Array.ConvertAll(("1.0," + recruitmentTierConfig).Split(','), float.Parse);
            string recruitmentTierDecayConfig = ConfigLoader.Instance.Config.DailyRecruitmentProbabilitiesByTierDecay;
            _dailyTierRecruitProbabilityDecay =
                Array.ConvertAll(("1.0," + recruitmentTierDecayConfig).Split(','), float.Parse);

            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        private void DailyTick()
        {
            TroopRoster prisonRoster = MobileParty.MainParty.PrisonRoster;
            float[] dailyRecruitingProbabilityByTier = (float[]) _dailyRecruitingProbabilityByTier.Clone();
            float[] dailyTierRecruitProbabilityDecay = (float[]) _dailyTierRecruitProbabilityDecay.Clone();

            int randomIndexToStartAt = MBRandom.RandomInt(prisonRoster.Count);

            for (int i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + randomIndexToStartAt) % prisonRoster.Count;

                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                if (!IsPrisonerRecruitable(characterAtIndex))
                {
                    continue;
                }

                int numberOfTroops = prisonRoster.GetElementNumber(index);
                int numberOfCurrentlyRecruitableTroops = this.GetRecruitableNumber(characterAtIndex);


                int troopTier = characterAtIndex.Tier;
                int troopsToRoll = numberOfTroops;

                if (ConfigLoader.Instance.Config.ScaleByReadyToRecruit)
                {
                    troopsToRoll -= numberOfCurrentlyRecruitableTroops;
                }

                //Loop through all the prisoners  we want to roll a probability for changing into a recruitable prisoner
                for (int n = 0; n < troopsToRoll && numberOfCurrentlyRecruitableTroops < numberOfTroops; n++)
                {
                    //If any of them pass their current probability check.
                    if (troopTier < dailyRecruitingProbabilityByTier.Length &&
                        dailyRecruitingProbabilityByTier[troopTier] > 0f &&
                        MBRandom.RandomFloat < dailyRecruitingProbabilityByTier[troopTier])
                    {
                        numberOfCurrentlyRecruitableTroops++;
                        this.SetRecruitableNumber(characterAtIndex, numberOfCurrentlyRecruitableTroops);
                        dailyRecruitingProbabilityByTier[troopTier] -= dailyTierRecruitProbabilityDecay[troopTier];
                    }
                }
            }
        }

        private bool IsPrisonerRecruitable(CharacterObject character)
        {
            if (!character.IsHero)
            {
                return true;
            }
            return false;
        }

        //For some reason we can't use this method here to get the same DataStore as the base Behaviour.
        //Need to investigate how that works exactly and why that is.
        public override void SyncData(IDataStore dataStore)
        {
        }

        public int GetRecruitableNumber(CharacterObject character)
        {
            return CampaignBehaviorBase.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>()
                .GetRecruitableNumber(character);
        }

        public void SetRecruitableNumber(CharacterObject character, int number)
        {
            CampaignBehaviorBase.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>()
                .SetRecruitableNumber(character, number);
        }
    }
}