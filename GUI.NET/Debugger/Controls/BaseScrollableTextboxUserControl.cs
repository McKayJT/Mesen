﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Controls;

namespace Mesen.GUI.Debugger.Controls
{
	public class BaseScrollableTextboxUserControl : BaseControl
	{
		virtual protected ctrlScrollableTextbox ScrollableTextbox
		{
			get
			{
				return null;
			}
		}

		[DefaultValue(13F)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		virtual public float FontSize
		{
			get { return this.ScrollableTextbox.FontSize; }
			set { this.ScrollableTextbox.FontSize = value; }
		}

		public void OpenSearchBox()
		{
			this.ScrollableTextbox.OpenSearchBox();
		}

		public void FindNext()
		{
			this.ScrollableTextbox.FindNext();
		}

		public void FindPrevious()
		{
			this.ScrollableTextbox.FindPrevious();
		}

		public void GoToAddress()
		{
			this.ScrollableTextbox.GoToAddress();
		}

		public void ScrollToLineNumber(int lineNumber, bool scrollToTop = false)
		{
			this.ScrollableTextbox.ScrollToLineNumber(lineNumber, eHistoryType.Always, scrollToTop);
		}

		public int GetCurrentLine()
		{
			return this.ScrollableTextbox.CurrentLine;
		}

		public void ScrollToTop()
		{
			this.ScrollableTextbox.ScrollToLineNumber(0);
		}

		public string GetWordUnderLocation(Point position, bool useCompareText = false)
		{
			return this.ScrollableTextbox.GetWordUnderLocation(position, useCompareText);
		}
	}
}
