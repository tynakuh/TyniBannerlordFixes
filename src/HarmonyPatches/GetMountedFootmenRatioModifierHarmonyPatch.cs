using HarmonyLib;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;

namespace TyniBannerlordFixes
{
    [HarmonyPatch(typeof(DefaultPartySpeedCalculatingModel), "GetMountedFootmenRatioModifier")]
    public class GetMountedFootmenRatioModifierHarmonyPatch
    {
        static void Postfix(DefaultPartySpeedCalculatingModel __instance, ref float __result)
        {
            __result *= ConfigLoader.Instance.Config.MountedFootmenSpeedMultiplier;
        }

        static bool Prepare()
        {
            return ConfigLoader.Instance.Config.MountedFootmenSpeedMultiplier != 1.0f;
        }
    }
}
