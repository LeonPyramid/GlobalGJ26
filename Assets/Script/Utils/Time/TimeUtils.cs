using System;
using UnityEngine;

namespace Utils.Anchor
{
    public static class TimeUtils
    {
        public static string FormatTime(float seconds)
        {
            var total = Mathf.CeilToInt(seconds);
    
            var hours = total / 3600;
            var minutes = total % 3600 / 60;
            var secs = total % 60;

            return hours > 0 ? $"{hours:00}:{minutes:00}:{secs:00}" : minutes > 0 ? $"{minutes:00}:{secs:00}" : $"{secs:00}";
        }
    }
}