using System;
using UnityEngine;

namespace Utils.Anchor
{
    public static class AnchorUtils
    {
        public static void SetAnchorPresetWithoutMoving(RectTransform rt, AnchorPreset preset)
        {
            Vector2 worldPosBefore = rt.TransformPoint(rt.rect.center);

            SetAnchorPreset(rt, preset);

            Vector2 worldPosAfter = rt.TransformPoint(rt.rect.center);
            Vector3 diff = worldPosBefore - worldPosAfter;
            rt.position += diff;
        }

        private static void SetAnchorPreset(RectTransform rt, AnchorPreset preset)
        {
            switch (preset)
            {
                case AnchorPreset.TopLeft:
                    rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
                    break;
                case AnchorPreset.TopCenter:
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1);
                    break;
                case AnchorPreset.TopRight:
                    rt.anchorMin = rt.anchorMax = new Vector2(1, 1);
                    break;
                case AnchorPreset.MiddleLeft:
                    rt.anchorMin = rt.anchorMax = new Vector2(0, 0.5f);
                    break;
                case AnchorPreset.MiddleCenter:
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case AnchorPreset.MiddleRight:
                    rt.anchorMin = rt.anchorMax = new Vector2(1, 0.5f);
                    break;
                case AnchorPreset.BottomLeft:
                    rt.anchorMin = rt.anchorMax = new Vector2(0, 0);
                    break;
                case AnchorPreset.BottomCenter:
                    rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0);
                    break;
                case AnchorPreset.BottomRight:
                    rt.anchorMin = rt.anchorMax = new Vector2(1, 0);
                    break;
                case AnchorPreset.StretchAll:
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(preset), preset, null);
            }
        }
        
        public static AnchorPreset GetAnchorPreset(RectTransform rt)
        {
            var min = rt.anchorMin;
            var max = rt.anchorMax;

            if (min == max)
            {
                if (min == new Vector2(0, 1)) return AnchorPreset.TopLeft;
                if (min == new Vector2(0.5f, 1)) return AnchorPreset.TopCenter;
                if (min == new Vector2(1, 1)) return AnchorPreset.TopRight;

                if (min == new Vector2(0, 0.5f)) return AnchorPreset.MiddleLeft;
                if (min == new Vector2(0.5f, 0.5f)) return AnchorPreset.MiddleCenter;
                if (min == new Vector2(1, 0.5f)) return AnchorPreset.MiddleRight;

                if (min == new Vector2(0, 0)) return AnchorPreset.BottomLeft;
                if (min == new Vector2(0.5f, 0)) return AnchorPreset.BottomCenter;
                if (min == new Vector2(1, 0)) return AnchorPreset.BottomRight;
            }

            if (min == Vector2.zero && max == Vector2.one)
                return AnchorPreset.StretchAll;

            return AnchorPreset.MiddleCenter;
        }
    }
    
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
        StretchAll
    }
}