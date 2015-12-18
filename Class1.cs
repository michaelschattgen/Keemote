using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keemote
{
	public sealed class KeemoteExt : Plugin
	{
		private IPluginHost m_host = null;

		public override bool Initialize(IPluginHost host)
		{
			m_host = host;

			// Get a reference to the 'Tools' menu item container
			ContextMenuStrip entryMenuStrip = m_host.MainWindow.EntryContextMenu;

			// Add a separator at the bottom
			var toolstripSeperator = new ToolStripSeparator();
			entryMenuStrip.Items.Add(toolstripSeperator);

			// Add menu item 'Do Something'
			var toolstripMenuItem = new ToolStripMenuItem();
			toolstripMenuItem.Text = "Start remote desktop connection";
			toolstripMenuItem.Click += new EventHandler(this.StartRemoteDesktopConnection);
			entryMenuStrip.Items.Add(toolstripMenuItem);

			return true;
		}

		private void StartRemoteDesktopConnection(object sender, EventArgs e)
		{
			try
			{
				PwEntry entry = m_host.MainWindow.GetSelectedEntry(false);
				ProtectedString protectedUsername = entry.Strings.Get("UserName");
				ProtectedString protectedPassword = entry.Strings.Get("Password");
				ProtectedString protectedUrl = entry.Strings.Get("URL");

				Process cmdKeyProcess = new Process();
				cmdKeyProcess.StartInfo.FileName = "cmdkey.exe";
				cmdKeyProcess.StartInfo.Arguments = $"/generic:TERMSRV/{protectedUrl.ReadString()} /user:{protectedUsername.ReadString()} /pass:{protectedPassword.ReadString()}";
				cmdKeyProcess.Start();

				Process remoteDesktopProcess = new Process();
				remoteDesktopProcess.StartInfo.FileName = "mstsc.exe";
				remoteDesktopProcess.StartInfo.Arguments = $"/v:{protectedUrl.ReadString()}";
				remoteDesktopProcess.Start();

			}
			catch (Exception ex)
			{
				using (StreamWriter w = File.AppendText("log.txt"))
				{
					w.Write(ex);
				}
			}
		}
	}
}