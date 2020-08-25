using System;
using System.Collections.Generic;
using AlienFx.Editor.Util;
using AlienFX.Editor.Util;
using AlienFX.Util;
using UnityEngine;

namespace AlienFX.Editor.Scriptable
{
    [CreateAssetMenu(menuName = "AlienFxEventBuilder/Events", fileName = "Event")]
    public class UserDefinedEvents : ScriptableObject
    {
        [SerializeField] private List<AlienEvent> events = new List<AlienEvent>();

        //events.Array.data[N].eventDetail.deviceName
        //events.Array.data[N].eventDetail.targetDevice
        //events.Array.data[N].eventDetail.deviceLights
        //events.Array.data[N].eventDetail.brightness
        //events.Array.data[N].eventDetail.useEasing
        //events.Array.data[N].eventDetail.easing.easeType
        //events.Array.data[N].eventDetail.easing.easingCurve
        
        //events.Array.data[N].scriptDetail.functionName
        //events.Array.data[N].scriptDetail.functionType
        //events.Array.data[N].scriptDetail.lightDetail.primaryColor
        //events.Array.data[N].scriptDetail.lightDetail.secondaryColor
        //events.Array.data[N].scriptDetail.lightDetail.locationMask
        //events.Array.data[N].scriptDetail.lightDetail.actionType
    }

    [Serializable]
    public class AlienEvent
    {
        [SerializeField] private EventDetail eventDetail = default;
        [SerializeField] private ScriptDetail scriptDetail = default;

        public EventDetail EventDetail => eventDetail;
        public ScriptDetail ScriptDetail => scriptDetail;

        public AlienEvent(EventDetail eventDetail, ScriptDetail scriptDetail)
        {
            this.eventDetail = eventDetail;
            this.scriptDetail = scriptDetail;
        }
    }
}
