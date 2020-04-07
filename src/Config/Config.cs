using System;

namespace TyniBannerlordFixes
{
    [Serializable]
    public class Config
    {

        public Config()
        {

        }

        public int CombatTipsXpAmount { get; set; }
        public int RaiseTheMeekXpAmount { get; set; }
        public float GarrisonTrainingXpMultiplier { get; set; }
        public string DailyRecruitmentProbabilitiesByTierDecay { get; set; }
        public string DailyRecruitmentProbabilitiesByTier { get; set; }
        public bool ScaleByReadyToRecruit { get; set; }
        public bool ScaleByReadyToUpgrade { get; set; }
        public float MountedFootmenSpeedMultiplier { get; set; }
    }
}
