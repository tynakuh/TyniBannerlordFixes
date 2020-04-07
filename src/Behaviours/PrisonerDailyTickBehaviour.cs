using TaleWorlds.CampaignSystem;
using System;
using TaleWorlds.Core;
using System.Collections.Generic;
using System.Windows.Forms;
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
            string recruitmentTierConfig = ConfigLoader.Instance.Config.DailyRecruitmentProbabilitiesByTier;
            _dailyRecruitingProbabilityByTier = Array.ConvertAll(("1.0," + recruitmentTierConfig).Split(','), float.Parse);
            string recruitmentTierDecayConfig = ConfigLoader.Instance.Config.DailyRecruitmentProbabilitiesByTierDecay;
            _dailyTierRecruitProbabilityDecay = Array.ConvertAll(("1.0," + recruitmentTierDecayConfig).Split(','), float.Parse);

            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, DailyTick);
        }

        private void DailyTick()
        {
            TroopRoster prisonRoster = MobileParty.MainParty.PrisonRoster;
            float[] dailyRecruitingProbabilityByTier = (float[])_dailyRecruitingProbabilityByTier.Clone();
            float[] dailyTierRecruitProbabilityDecay = (float[]) _dailyTierRecruitProbabilityDecay.Clone();
            int num = MBRandom.RandomInt(prisonRoster.Count);

            for (int i = 0; i < prisonRoster.Count; i++)
            {
                int index = (i + num) % prisonRoster.Count;

                CharacterObject characterAtIndex = prisonRoster.GetCharacterAtIndex(index);
                if (!IsPrisonerRecruitable(characterAtIndex))
                {
                    continue;
                }
                int elementNumber = prisonRoster.GetElementNumber(index);
                int recruitableNumberInternal = GetRecruitableNumber(characterAtIndex);
                prisonRoster.GetElementCopyAtIndex(index);

                if (recruitableNumberInternal < elementNumber)
                {
                    int tier = characterAtIndex.Tier;
                    int troopsLeftToRecruit = elementNumber;

                    if (ConfigLoader.Instance.Config.ScaleByReadyToRecruit)
                    {
                        troopsLeftToRecruit -= recruitableNumberInternal;
                    }

                    for (int n = 0; n < troopsLeftToRecruit; n++)
                    {

                        if (tier < dailyRecruitingProbabilityByTier.Length && dailyRecruitingProbabilityByTier[tier] > 0f && MBRandom.RandomFloat < dailyRecruitingProbabilityByTier[tier])
                        {
                            recruitableNumberInternal++;
                            SetRecruitableNumber(characterAtIndex, recruitableNumberInternal);
                            dailyRecruitingProbabilityByTier[tier] -= dailyTierRecruitProbabilityDecay[tier];
                        }
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
            return CampaignBehaviorBase.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>().GetRecruitableNumber(character);

        }

        public void SetRecruitableNumber(CharacterObject character, int number)
        {
            CampaignBehaviorBase.GetCampaignBehavior<RecruitPrisonersCampaignBehavior>().SetRecruitableNumber(character,number);
        }
    }
}