using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using System.Windows.Forms;
using System;
using HarmonyLib;

namespace TyniBannerlordFixes
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			try
			{
				var harmony = new Harmony("mod.tynakuh.bannerlord");
				harmony.PatchAll();
			}
			catch (Exception e)
			{
				MessageBox.Show("Couldn't apply Harmony due to: " + Utils.FlattenException(e));
			}
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			try
			{
				base.OnGameStart(game, gameStarterObject);
				AddBehaviours(gameStarterObject as CampaignGameStarter);
			}
			catch(Exception e)
			{
				MessageBox.Show(Utils.FlattenException(e));
			}
		}

		private void AddBehaviours(CampaignGameStarter gameStarterObject)
		{
			if (gameStarterObject != null)
			{
				gameStarterObject.AddBehavior(new MobilePartyDailyTickBehaviour());
				gameStarterObject.AddBehavior(new TownDailyTickBehaviour());
				gameStarterObject.AddBehavior(new PrisonerDailyTickBehaviour());
			}
		}

	}
}