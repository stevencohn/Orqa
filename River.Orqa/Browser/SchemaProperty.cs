
namespace River.Orqa.Browser
{
	using System;
	using System.ComponentModel;
	using River.Orqa.Resources;


	internal class SchemaProperty : PropertyDescriptor
	{
		private string displayName;
		private string displayValue;


		public SchemaProperty (string displayName)
			: base(displayName, null)
		{
			this.displayName = displayName;
			this.displayValue = "<undefined>";
		}


		public SchemaProperty (string displayName, string category)
			: base(displayName, new Attribute[] { new CategoryAttribute(category) })
		{
			this.displayName = displayName;
			this.displayValue = "<undefined>";
		}


		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(null);
			}
		}

		public override bool CanResetValue (object component)
		{
			return true;
		}

		public override Type ComponentType
		{
			get { return displayName.GetType(); }
		}

		public override string DisplayName
		{
			get { return displayName; }
		}


		public override object GetValue (object component)
		{
			return displayValue;
		}

		public override bool IsReadOnly
		{
			get { return true; }
		}


		public override string Name
		{
			get { return displayName; }
		}

		public override Type PropertyType
		{
			get { return displayName.GetType(); }
		}

		public override void ResetValue (object component)
		{
		}

		public override bool ShouldSerializeValue (object component)
		{
			return true;
		}

		public override void SetValue (object component, object value)
		{
			displayValue = (string)value;
		}
	}
}
