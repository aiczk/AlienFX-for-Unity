using System;
using AlienFX.Easing;
using AlienFX.Util;
using UnityEngine;

namespace AlienFX.Editor.Util
{
    [Serializable]
    public class EventDetail
    {
        [SerializeField] private string deviceName = default;
        [SerializeField] private LfxDeviceType targetDevice = LfxDeviceType.Custom;
        [SerializeField] private string[] deviceLights = default;
        [SerializeField] private LfxBrightness brightness = LfxBrightness.Full;
        [SerializeField] private bool useEasing = false;
        [SerializeField] private EaseDetail easing = default;

        public string DeviceName => deviceName;
        public LfxDeviceType TargetDevice => targetDevice;
        public string[] DeviceLights => deviceLights;
        public LfxBrightness Brightness => brightness;
        public bool UseEasing => useEasing;
        public EaseDetail Easing => easing;
    }

    [Serializable]
    public struct EaseDetail
    {
        [SerializeField] private AlienFxEaseType easeType;
        [SerializeField] private AnimationCurve easingCurve;

        public AlienFxEaseType EaseType => easeType;
        public AnimationCurve EasingCurve => easingCurve;
        
        public EaseDetail(AlienFxEaseType easeType, AnimationCurve easingCurve)
        {
            this.easeType = easeType;
            this.easingCurve = easingCurve;
        }
    }
}
