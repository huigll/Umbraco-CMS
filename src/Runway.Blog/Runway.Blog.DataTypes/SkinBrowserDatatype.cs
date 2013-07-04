using System;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.interfaces;
namespace Runway.Blog.DataTypes
{
	public class SkinBrowserDatatype : AbstractDataEditor
	{
		private SkinBrowserConfig m_config;
		private SkinBrowser m_control = new SkinBrowser();
		public override string DataTypeName
		{
			get
			{
				return "Umbraco Skin Browser";
			}
		}
		public override System.Guid Id
		{
			get
			{
				return new System.Guid("119260A2-9B64-4a08-8A65-42938ECBE799");
			}
		}
		public override IDataPrevalue PrevalueEditor
		{
			get
			{
				if (this.m_config == null)
				{
					this.m_config = new SkinBrowserConfig(this);
				}
				return this.m_config;
			}
		}
		public SkinBrowserDatatype()
		{
			base.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);
			base.RenderControl = this.m_control;
			this.m_control.Init += new System.EventHandler(this.m_control_Init);
		}
		private void DataEditorControl_OnSave(System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.m_control.Value) && !this.m_control.Value.EndsWith(".css"))
			{
				try
				{
					System.Guid guid = new System.Guid(this.m_control.Value);
					if (guid != System.Guid.Empty)
					{
						try
						{
							string zipPath = SkinBrowser.DownloadPackage(guid);
							string skinPath = SkinBrowser.ExtractPackage(zipPath);
							this.m_control.Value = SkinBrowser.InstallSkin(skinPath) + "/style.css";
							this.Data.Value = this.m_control.Value;
							this.m_control.SuccessMessage = "<strong>Success!</strong><br />Your brand new skin is now downloaded and installed!";
						}
						catch (System.Exception ex)
						{
							this.m_control.ErrorMessage = "<strong>Error installing skin:</strong><br/>" + ex.ToString();
							Log.Add(LogTypes.Error, ((DefaultData)this.Data).NodeId, "Error installing skin: " + ex.ToString());
						}
					}
				}
				catch
				{
				}
			}
		}
		private void m_control_Init(object sender, System.EventArgs e)
		{
			this.m_control.Value = ((this.Data.Value != null) ? this.Data.Value.ToString() : string.Empty);
			this.m_control.PackageIdentifier = ((SkinBrowserConfig)this.PrevalueEditor).Configuration;
		}
	}
}
