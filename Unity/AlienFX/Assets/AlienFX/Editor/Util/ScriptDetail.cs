using System;
using AlienFX.Editor.Util;
using AlienFX.Util;
using UnityEngine;

namespace AlienFx.Editor.Util
{ 
    [Serializable]
    public class ScriptDetail
    {
        [SerializeField] private string functionName = default;
        [SerializeField] private AlienFxLightFuncType functionType;
        [SerializeField] private LightDetail lightDetail;

        public string FunctionName => functionName;
        public AlienFxLightFuncType FunctionType => functionType;
        public LightDetail LightDetail => lightDetail;
        
        public ScriptDetail(string functionName, AlienFxLightFuncType functionType, LightDetail lightDetail)
        {
            this.functionName = functionName;
            this.functionType = functionType;
            this.lightDetail = lightDetail;
        }
    }

    [Serializable]
    public struct LightDetail
    {
        [SerializeField] private Color primaryColor;
        [SerializeField] private Color secondaryColor;
        [SerializeField] private LfxLocationMask locationMask;
        [SerializeField] private LfxActionType actionType;

        public Color PrimaryColor => primaryColor;
        public Color SecondaryColor => secondaryColor;
        public LfxLocationMask LocationMask => locationMask;
        public LfxActionType ActionType => actionType;
        
        public LightDetail(Color primaryColor, Color secondaryColor = default, LfxLocationMask locationMask = default, LfxActionType actionType = default)
        {
            this.primaryColor = primaryColor;
            this.secondaryColor = secondaryColor;
            this.locationMask = locationMask;
            this.actionType = actionType;
        }
    }
}