// Copyright (c) Sorrow. All rights reserved.  

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace FrenchTranslation
{
	public class Main : MBSubModuleBase
	{
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			try
			{
				Harmony h = new Harmony("TaleWorlds.Localization.TextProcessor.FrenchTextProcessor");
				h.PatchAll();
			}
			catch (Exception e)
			{
				string str = "Error patching:\n";
				string message = e.Message; ;
				string str2 = " \n\n";
				Exception innerException = e.InnerException;
                System.Windows.MessageBox.Show(str + message + str2 + ((innerException != null) ? innerException.Message : null));
			}
			Module.CurrentModule.AddInitialStateOption(new InitialStateOption("Choisir le nom des factions", new TextObject("Choisir le nom des factions", null),
			9990, () => {

				//Get FriendlyName from Application Domain
				string strFriendlyName = AppDomain.CurrentDomain.FriendlyName;
				//Get process collection by the application name without extension (.exe)
				Process[] procs = Process.GetProcessesByName(strFriendlyName.Substring(0, strFriendlyName.LastIndexOf('.')));
				if (procs.Length != 0)
				{
					Form form = new CulturesForm();
					form.ShowDialog(new WindowWrapper(procs[0].MainWindowHandle));
					form.Dispose();
				}
				
			}, false));
		}

		public class WindowWrapper : System.Windows.Forms.IWin32Window
		{
			public WindowWrapper(IntPtr handle)
			{
				_hwnd = handle;
			}
			public IntPtr Handle
			{
				get { return _hwnd; }
			}
			private IntPtr _hwnd;
		}

	}
}
