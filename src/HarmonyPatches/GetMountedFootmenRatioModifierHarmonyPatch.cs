using System;
using System.Windows.Forms;
using HarmonyLib;
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
        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                MessageBox.Show(Utils.FlattenException(__exception));
            }
        }
    }
}