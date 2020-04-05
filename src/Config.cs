using System;

namespace ExperiencePerkFix
{
    [Serializable]
    public class Config
    {
        public Config()
        {

        }

        public int CombatTipsXpAmount { get; set; }
        public int RaiseTheMeekXpAmount { get; set; }
    }
}
