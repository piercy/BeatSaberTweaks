﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace BeatSaberTweaks.Utilites
{
    class TextAlignUtil
    {
        public static TextAlignmentOptions textAlignFromString(String opt)
        {
            if (String.Equals(Settings.ClockAlignment, "left", StringComparison.OrdinalIgnoreCase)) {
                return TextAlignmentOptions.Left;
            }
            else if (String.Equals(Settings.ClockAlignment, "right", StringComparison.OrdinalIgnoreCase)) {
                return TextAlignmentOptions.Right;
            }
            else {
                return TextAlignmentOptions.Center;
            }
        }
    }
}
