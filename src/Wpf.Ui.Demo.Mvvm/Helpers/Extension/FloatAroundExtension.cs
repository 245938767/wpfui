// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Demo.Mvvm.Helpers.Extension;

public static class FloatAroundExtension
{
    public static bool CheckAround(this float data, float value, float around)
    {
        var aValue = data - value;
        return aValue <= around && aValue >= -around;
    }
}