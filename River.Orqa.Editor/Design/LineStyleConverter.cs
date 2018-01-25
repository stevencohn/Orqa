namespace QWhale.Design
{
    using River.Orqa.Editor;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    public class LineStyleConverter : TypeConverter
    {
        // Methods
        public LineStyleConverter()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(InstanceDescriptor)) && typeof(LineStyle).IsInstanceOfType(value))
            {
                LineStyle style1 = (LineStyle) value;
                Type[] typeArray1 = new Type[0];
                ConstructorInfo info1 = typeof(LineStyle).GetConstructor(typeArray1);
                if (info1 != null)
                {
                    return new InstanceDescriptor(info1, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}

