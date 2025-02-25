// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Helpers.Converter;
public class EnumDescriptionTypeConverter : EnumConverter
{
    public EnumDescriptionTypeConverter(Type type) : base(type)
    {
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (null != value)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (null != fi)
                {
                    var attributes =
                        (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    return ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description)))
                        ? attributes[0].Description
                        : value.ToString();
                }
            }

            return string.Empty;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}
