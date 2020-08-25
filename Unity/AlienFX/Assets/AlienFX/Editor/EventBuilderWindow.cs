using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlienFX.Easing;
using AlienFX.Editor.Scriptable;
using AlienFX.Editor.Util;
using AlienFX.Util;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlienFX.Editor
{
    public class EventBuilderWindow : EditorWindow
    {
        private string BindingPath => $"events.Array.data[{index.ToString()}]";
        
        private LightFx lfx;
        private string sdkVersion;
        private AlienDeviceDesc[] deviceDescs;
        private List<string>[] lights;
        private ListView eventList;
        private int index;
        private SerializedObject so;
        private SerializedProperty assetSp;
        
        [MenuItem("Tools/AlienFx Event Builder")]
        public static void Create()
        {
            var window = GetWindow<EventBuilderWindow>("AlienFx Event Builer");
            window.minSize = new Vector2(630, 450);
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture2D>(@"Assets/AlienFX/Editor/icon.png");
        }

        private void OnEnable()
        {
            var eventElementUss = AssetDatabase.LoadAssetAtPath<StyleSheet>(@"Assets/AlienFX/Editor/UI/EventElement.uss");
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(@"Assets/AlienFX/Editor/UI/EventWindow.uxml"); 
            uxml.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(eventElementUss);
            InitLightFx();
            
            rootVisualElement.Q<Button>("addElement").clickable.clicked += AddNewElement;
            rootVisualElement.Q<Button>("build").clickable.clicked += BuildScript;
            rootVisualElement.Q<Label>("sdkVersion").text = $"SDK Version: {sdkVersion}";
            eventList = rootVisualElement.Q<ListView>("eventList");

            var ldAsset = rootVisualElement.Q<ObjectField>("loadAsset");
            ldAsset.objectType = typeof(UserDefinedEvents);
            ldAsset.RegisterValueChangedCallback(x => LoadAsset(x.newValue as UserDefinedEvents));
        }

        private void OnDestroy() => lfx?.Release();

        private void InitLightFx()
        {
            lfx = new LightFx();
            lfx.Initialize();
            
            var version = new StringBuilder(byte.MaxValue);
            lfx.GetVersion(version);
            sdkVersion = version.ToString();
            
            lfx.GetNumDevices(out var numDevices);
            deviceDescs = new AlienDeviceDesc[numDevices];
            lights = new List<string>[numDevices];
            
            for (var i = 0; i < lights.Length; i++) 
                lights[i] = new List<string>();

            var description = new StringBuilder(byte.MaxValue);
            for (uint devIndex = 0; devIndex < numDevices; devIndex++)
            {
                lfx.GetDeviceDescription(devIndex, description, out var devType);
                var deviceDesc = description.ToString();

                description.Clear();
                lfx.GetNumLights(devIndex, out var numLights);
                
                for (uint lightIndex = 0; lightIndex < numLights; lightIndex++)
                {
                    var result = lfx.GetLightDescription(devIndex, lightIndex, description);
                    
                    if(result != LfxResult.Success)
                        continue;
                    
                    lights[devIndex].Add(description.ToString());
                    description.Clear();
                }

                deviceDescs[devIndex] = new AlienDeviceDesc(deviceDesc, devType);
            }
        }

        private void AddNewElement()
        {
            if (assetSp.arraySize == index)
            {
                assetSp.AddElement();
                so.SaveState();
            }
            
            var eventElement = new VisualElement {name = "EventElement"};
            var header = new VisualElement {name = "EventHeader"};
            var footer = new VisualElement {name = "EventFooter"};
            var light = new VisualElement {name = "AlienLight"};

            var devices = deviceDescs.Select(x => x.Description).ToList();
            var devicesPopup = new PopupField<string>(devices, 0) {bindingPath = $"{BindingPath}.eventDetail.deviceName", tooltip = "List of currently attached AlienFxDevices."};
            var removeElem = new Button(() => eventList.Remove(eventElement)) {name = "RemoveElement", text = "×"};
            var deviceType = new EnumField(LfxDeviceType.Custom) {bindingPath = $"{BindingPath}.eventDetail.targetDevice", tooltip = "If you want to cover all devices, please select 'Custom'."};
            var lightMask = new VisualElement {name = "LightMask"};
            var deviceLightMask = new MaskField {bindingPath = $"{BindingPath}.eventDetail.deviceLights" , tooltip = "List of lights in AlienFxDevice."};
            var brightness = new EnumField(LfxBrightness.Full) {bindingPath = $"{BindingPath}.eventDetail.brightness", tooltip = "Brightness of AlienDevice."};
            var useEase = new Toggle("Easing :") {bindingPath = $"{BindingPath}.eventDetail.useEasing", name = "EaseToggle"};
            useEase.ChildLabelStyle(StyleKeyword.Auto).Q<VisualElement>(className: "unity-toggle__input").Column(FlexDirection.RowReverse);
            var easeType = new EnumField(AlienFxEaseType.EaseInLinear) {bindingPath = $"{BindingPath}.eventDetail.easing.easeType", tooltip = "Note: If you select 'Linear', no AnimationCurve will be applied."};
            var easeCurve = new CurveField {bindingPath = $"{BindingPath}.eventDetail.easing.easingCurve"};
            
            devicesPopup.Bind(so);
            deviceType.Bind(so);
            lightMask.Bind(so);
            brightness.Bind(so);
            useEase.Bind(so);
            easeType.Bind(so);
            easeCurve.Bind(so);

            devicesPopup.Add(removeElem);
            lightMask.Add(deviceLightMask);
            light.Add(deviceType);
            light.Add(lightMask);
            light.Add(brightness);
            light.Add(useEase);

            var easeFunc = new VisualElement {name = "EaseFunc", visible = false};
            easeFunc.Add(easeType);
            easeFunc.Add(easeCurve);
            light.Add(easeFunc);
            
            useEase.RegisterValueChangedCallback(x => easeFunc.visible = x.newValue);

            devicesPopup.formatSelectedValueCallback = x =>
            {
                lightMask.RemoveAt(0);
                deviceLightMask = new MaskField(lights[devicesPopup.index], -1) {bindingPath = deviceLightMask.bindingPath, tooltip = deviceLightMask.tooltip};
                lightMask.Add(deviceLightMask);
                deviceLightMask.Bind(so);
                return x;
            };
            
            var alienCode = new VisualElement {name = "AlienCodes"};
            var funcName = new TextField("Function Name :") {bindingPath = $"{BindingPath}.scriptDetail.functionName", tooltip = "The function name of the generated program."};
            var funcType = new EnumField("Function Type :", AlienFxLightFuncType.Light) {bindingPath = $"{BindingPath}.scriptDetail.functionType", tooltip = "Select a function of LFX."};
            
            funcName.Bind(so);
            funcType.Bind(so);
            
            alienCode.Add(funcName);
            alienCode.Add(funcType);
            AlienFxLightFuncType.Light.ShowDetail(alienCode);
            funcType.RegisterValueChangedCallback(x => ((AlienFxLightFuncType) x.newValue).ShowDetail(alienCode));
            
            header.Add(devicesPopup);
            footer.Add(light);
            footer.Add(alienCode);
            eventElement.Add(header);
            eventElement.Add(footer);
            eventList.Add(eventElement);
            index++;
        }

        private void BuildScript() => EventBuildPathWindow.Open();

        private void LoadAsset(UserDefinedEvents asset)
        {
            eventList.Clear();
            index = 0;
            
            if(asset == null)
                return;

            so = new SerializedObject(asset);
            assetSp = so.FindProperty("events");

            for(var i = 0; i < assetSp.arraySize; i++) 
                AddNewElement();
        }
    }

    [Serializable]
    public readonly struct AlienDeviceDesc
    {
        public string Description { get; }
        public LfxDeviceType DeviceType { get; }

        public AlienDeviceDesc(string description, LfxDeviceType deviceType)
        {
            Description = description;
            DeviceType = deviceType;
        }
    }
}
