using KeePass.Plugins;
using KeePassLib;
using System;
using System.Collections.Generic;
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
			toolstripMenuItem.Click += this.StartRemoteDesktopConnection;
			entryMenuStrip.Items.Add(toolstripMenuItem);

			return true;
		}

		private void StartRemoteDesktopConnection(object sender, EventArgs e)
		{
			PwEntry entry = m_host.MainWindow.GetSelectedEntry(false);
			entry.Strings.Get("Username");
			entry.Strings.Get("Password");
			entry.Strings.Get("URL");

			//using (StreamWriter w = File.AppendText("log.txt"))
			//{
			//	w.Write(e);
			//}
		}
	}
}