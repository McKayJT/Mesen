﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;

namespace Mesen.GUI.Forms
{
	public partial class BaseConfigForm : BaseForm
	{
		private Dictionary<string, Control> _bindings = new Dictionary<string, Control>();
		private Dictionary<string, FieldInfo> _fieldInfo = null;
		private object _entity;
		private Timer _validateTimer;

		public BaseConfigForm()
		{
			InitializeComponent();

			_validateTimer = new Timer();
			_validateTimer.Interval = 50;
			_validateTimer.Tick += OnValidateInput;
			_validateTimer.Start();
		}

		private void OnValidateInput(object sender, EventArgs e)
		{
			btnOK.Enabled = ValidateInput();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if(DialogResult == System.Windows.Forms.DialogResult.OK) {
				if(!ValidateInput()) {
					e.Cancel = true;
				} else {
					_validateTimer.Tick -= OnValidateInput;
					_validateTimer.Stop();
				}
			}
			base.OnFormClosing(e);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			if(this.DialogResult == System.Windows.Forms.DialogResult.OK) {
				UpdateConfig();
				if(ApplyChangesOnOK) {
					ConfigManager.ApplyChanges();
				}
			} else {
				ConfigManager.RejectChanges();
			}
			base.OnFormClosed(e);
		}

		protected virtual bool ApplyChangesOnOK
		{
			get { return true; }
		}

		protected virtual void UpdateConfig()
		{
		}

		protected object Entity
		{
			get { return _entity; }
			set { _entity = value; }
		}

		protected virtual Type BindedType
		{
			get { return _entity.GetType(); }
		}

		protected virtual bool ValidateInput()
		{
			return true;
		}

		protected void AddBinding(string fieldName, Control bindedField, object enumValue = null)
		{
			if(BindedType == null) {
				throw new Exception("Need to override BindedType to use bindings");
			} else {
				_fieldInfo = new Dictionary<string,FieldInfo>();
				FieldInfo[] members = BindedType.GetFields();
				foreach(FieldInfo info in members) {
					_fieldInfo[info.Name] = info;
				}
			}
			bindedField.Tag = enumValue;
			_bindings[fieldName] = bindedField;
		}

		protected void UpdateUI()
		{
			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					FieldInfo field = _fieldInfo[kvp.Key];
					object value = field.GetValue(this.Entity);
					if(kvp.Value is TextBox) {
						if(field.FieldType == typeof(UInt32)) {
							kvp.Value.Text = ((UInt32)value).ToString("X");
						} else if(field.FieldType == typeof(Byte)) {
							kvp.Value.Text = ((Byte)value).ToString("X");
						} else {
							kvp.Value.Text = (string)value;
						}
					} else if(kvp.Value is CheckBox) {
						((CheckBox)kvp.Value).Checked = (bool)value;
					} else if(kvp.Value is Panel) {
						RadioButton radio = kvp.Value.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Tag.Equals(value));
						if(radio != null) {
							radio.Checked = true;
						} else {
							throw new Exception("No radio button matching value found");
						}
					}
				}
			}	
		}

		protected void UpdateObject()
		{
			foreach(KeyValuePair<string, Control> kvp in _bindings) {
				if(!_fieldInfo.ContainsKey(kvp.Key)) {
					throw new Exception("Invalid binding key");
				} else {
					FieldInfo field = _fieldInfo[kvp.Key];
					if(kvp.Value is TextBox) {
						object value = kvp.Value.Text;
						if(field.FieldType == typeof(UInt32)) {
							value = (object)UInt32.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
						} else if(field.FieldType == typeof(Byte)) {
							value = (object)Byte.Parse((string)value, System.Globalization.NumberStyles.HexNumber);
						}
						field.SetValue(Entity, value);
					} else if(kvp.Value is CheckBox) {
						field.SetValue(Entity, ((CheckBox)kvp.Value).Checked);
					} else if(kvp.Value is Panel) {
						field.SetValue(Entity, kvp.Value.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked).Tag);
					}
				}
			}			
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}