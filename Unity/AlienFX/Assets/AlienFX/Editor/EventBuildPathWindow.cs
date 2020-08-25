using System;
using AlienFX.Editor.Util;
using AlienFX.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlienFX.Editor
{
    public class EventBuildPathWindow : EditorWindow
    {
        private string console;
        
        public static void Open()
        {
            var window = GetWindow<EventBuildPathWindow>(true, "Build AlienFxEvent Window");
            window.minSize = new Vector2(450, 200);
        }

        private void OnEnable()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(@"Assets/AlienFX/Editor/UI/EventBuildWindow.uxml");
            uxml.CloneTree(rootVisualElement);
            var build = rootVisualElement.Q<Button>();
            build.clickable.clicked += BuildScript;
        }

        private void InitPathText()
        {
            var scriptName = rootVisualElement.Q<TextField>("NameSpace").text;
            var buildPath = rootVisualElement.Q<TextField>("BuildPath").text;
            var t4Path = rootVisualElement.Q<TextField>("T4Path").text;
            var textTransformPath = rootVisualElement.Q<TextField>("TTPath").text;
            console = $@"{textTransformPath} -out {Application.dataPath}\{buildPath}\{scriptName}.cs {Application.dataPath}\{t4Path}.tt";
        }

        private void BuildScript()
        {
            InitPathText();
            Debug.Log(console);
        }
    }
}
