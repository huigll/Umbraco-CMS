using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Our.Umbraco.uGoLive.Web.Controls
{
	public class PropertyPanel : Panel
	{
		public string Text
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.AddFormRow(writer, this.Text, this.ToolTip, this.Controls.Cast<Control>());
		}
		private void AddFormRow(HtmlTextWriter writer, string label, string description, IEnumerable<Control> controls)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "propertyItem");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "propertyItemheader");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			Label label2 = new Label
			{
				Text = label
			};
			label2.RenderControl(writer);
			if (!string.IsNullOrEmpty(description))
			{
				writer.WriteBreak();
				writer.RenderBeginTag(HtmlTextWriterTag.Small);
				Label label3 = new Label
				{
					Text = description
				};
				label3.RenderControl(writer);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "propertyItemContent");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			foreach (Control current in controls)
			{
				current.RenderControl(writer);
			}
			writer.RenderEndTag();
			writer.RenderEndTag();
		}
	}
}
