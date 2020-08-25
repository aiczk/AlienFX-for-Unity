using System;
using System.Linq;
using AlienFX.Util;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlienFX.Editor.Util
{
    public enum AlienFxLightFuncType
    {
        Light,
        SetLightColor,
        SetLightActionColor,
        SetLightActionColorEx,
        ActionColor,
        ActionColorEx,
    }

    public static class AlienFxEditorUtil
    {
        public static void AddElement(this SerializedProperty sp) => sp.InsertArrayElementAtIndex(sp.arraySize);

        public static void SaveState(this SerializedObject so)
        {
            so.ApplyModifiedProperties();
            so.Update();
        }

        public static SerializedProperty Search(this SerializedProperty parent, params string[] properties)
        {
            var result = parent;
            for (var i = 0; i < properties.Length; i++)
            {
                ref var propertyName = ref properties[i];
                result = result.FindPropertyRelative(propertyName);
            }

            return result;
        }
        
        public static bool TryRemove(this VisualElement ve, string name, string className = null)
        {
            var elem = ve.Q(name, className);
            
            if(elem == null)
                return false;
            
            ve.Remove(elem);
            return true;
        }

        public static VisualElement ChildLabelStyle(this VisualElement parent, StyleKeyword keyword)
        {
            parent.Q<Label>().style.minWidth = keyword;
            return parent;
        }

        public static void Column(this VisualElement parent, FlexDirection flexDirection) => parent.style.flexDirection = flexDirection;

        public static void ShowDetail(this AlienFxLightFuncType funcType, VisualElement ve)
        {
            if (ve.Children().Any(x => x.name == "AlienFuncDesc"))
                ve.RemoveAt(2);

            var elem = new VisualElement {name = "AlienFuncDesc"};
            
            switch (funcType)
            {
                case AlienFxLightFuncType.Light:
                    var locMask = new EnumField("LocationMask :", LfxLocationMask.All);
                    var color = new ColorField("LightColor :") {tooltip = "Select the color you want to shine in the AlienDevice."};
                    elem.Add(locMask);
                    elem.Add(color);
                    break;
                
                case AlienFxLightFuncType.SetLightColor:
                    color = new ColorField("LightColor :") {tooltip = "Select the color you want to shine in the AlienDevice."};
                    elem.Add(color);
                    break;
                
                case AlienFxLightFuncType.SetLightActionColor: 
                    color = new ColorField("LightColor :") {tooltip = "Select the color you want to shine in the AlienDevice."};
                    var actType = new EnumField("ActionType :", LfxActionType.Morph);
                    elem.Add(actType);
                    elem.Add(color);
                    break;
                
                case AlienFxLightFuncType.SetLightActionColorEx:
                    var primary = new ColorField("PrimaryColor :");
                    var secondary = new ColorField("SecondaryColor :");
                    actType = new EnumField("ActionType :", LfxActionType.Morph);
                    elem.Add(actType);
                    elem.Add(primary);
                    elem.Add(secondary);
                    break;
                
                case AlienFxLightFuncType.ActionColor:
                    locMask = new EnumField("LocationMask :",LfxLocationMask.All);
                    actType = new EnumField("ActionType :", LfxActionType.Morph);
                    color = new ColorField("LightColor :") {tooltip = "Select the color you want to shine in the AlienDevice."};
                    elem.Add(locMask);
                    elem.Add(actType);
                    elem.Add(color);
                    break;
                
                case AlienFxLightFuncType.ActionColorEx:
                    locMask = new EnumField("LocationMask :",LfxLocationMask.All);
                    actType = new EnumField("ActionType :", LfxActionType.Morph);
                    primary = new ColorField("PrimaryColor :");
                    secondary = new ColorField("SecondaryColor :");
                    elem.Add(locMask);
                    elem.Add(actType);
                    elem.Add(primary);
                    elem.Add(secondary);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(funcType), funcType, null);
            }
            
            ve.Add(elem);
        }
    }
}
