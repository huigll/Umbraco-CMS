﻿using System;
using Umbraco.Core;

namespace umbraco.editorControls.textfield
{
	public class TextFieldDataType : cms.businesslogic.datatype.BaseDataType,interfaces.IDataType
	{
		private interfaces.IDataEditor _Editor;
		private interfaces.IData _baseData;
		private interfaces.IDataPrevalue _prevalueeditor;

		public override interfaces.IDataEditor DataEditor 
		{
			get
			{
				if (_Editor == null)
					_Editor = new TextFieldEditor(Data);
				return _Editor;
			}
		}

		public override interfaces.IData Data 
		{
			get 
			{
				if (_baseData == null)
					_baseData = new cms.businesslogic.datatype.DefaultData(this);
				return _baseData;
			}
		}
		public override Guid Id 
		{
			get { return new Guid(Constants.PropertyEditors.Textbox); }
		}

		public override string DataTypeName 
		{
			get {return "Textbox";}
		}

		public override interfaces.IDataPrevalue PrevalueEditor 
		{
			get 
			{
				if (_prevalueeditor == null)
					_prevalueeditor = new DefaultPrevalueEditor(this,false);
				return _prevalueeditor;
			}
		}
	}
}