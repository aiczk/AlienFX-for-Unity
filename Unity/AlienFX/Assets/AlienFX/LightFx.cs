using System;
using System.Text;
using AlienFX.Util;

// ReSharper disable InconsistentNaming

namespace AlienFX
{
    public class LightFx : IDisposable
    {
        private static class Functions
        {
            internal delegate LfxResult LFX_Initialize();
            internal delegate LfxResult LFX_Release();
            internal delegate LfxResult LFX_Reset();
            internal delegate LfxResult LFX_Update();
            internal delegate LfxResult LFX_GetNumDevices(out uint numDevices);
            internal delegate LfxResult LFX_GetDeviceDescription(uint devIndex, StringBuilder devDesc, int devDescSize, out LfxDeviceType devType);
            internal delegate LfxResult LFX_GetNumLights(uint devIndex, out uint numLights);
            internal delegate LfxResult LFX_GetLightDescription(uint devIndex, uint lightIndex, StringBuilder lightDesc, int lightDescSize);
            internal delegate LfxResult LFX_GetLightLocation(uint devIndex, uint lightIndex, out LfxPosition lightLoc);
            internal delegate LfxResult LFX_GetLightColor(uint devIndex, uint lightIndex, ref LfxColor lightCol);
            internal delegate LfxResult LFX_SetLightColor(uint devIndex, uint lightIndex, LfxColor lightCol); 
            internal delegate LfxResult LFX_Light(LfxLocationMask locationMask, uint lightCol);
            internal delegate LfxResult LFX_SetLightActionColor(uint devIndex, uint lightIndex, LfxActionType actionType, LfxColor primaryColor);
            internal delegate LfxResult LFX_SetLightActionColorEx(uint devIndex, uint lightIndex, LfxActionType actionType, LfxColor primaryColor, LfxColor secondaryColor);
            internal delegate LfxResult LFX_ActionColor(LfxLocationMask locationMask, LfxActionType actionType, uint primaryColor);
            internal delegate LfxResult LFX_ActionColorEx(LfxLocationMask locationMask, LfxActionType actionType, uint primaryColor, uint secondaryColor);
            internal delegate LfxResult LFX_GetVersion(StringBuilder version, int versionSize);
        }
        
        private UnManagedDll unManagedDll;

        public LightFx()
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_64 || DEBUG
            unManagedDll = new UnManagedDll(@"C:\Program Files\Alienware\Alienware Command Center\AlienFX SDK\DLLs\x64\LightFX.dll");
#else
            unManagedDll = new UnManagedDll(@"C:\Program Files\Alienware\Alienware Command Center\AlienFX SDK\DLLs\x86\LightFX.dll");
#endif
        }

        /// <summary>
        /// This function initializes the Alienware AlienFX system.
        /// It must be called prior to calling other library functions.
        /// If this function is not called, the system will not be initialized, and the other library functions
        /// will return LFX_ERROR_NOINIT or LFX_FAILURE.
        /// </summary>
        /// <returns></returns>
        public LfxResult Initialize() => unManagedDll.GetProcAddress<Functions.LFX_Initialize>()();
        
        /// <summary>
        /// This function releases the Alienware AlienFX system, freeing memory and restores the system to its initial state.
        /// It may be called when the system is no longer needed.
        /// </summary>
        /// <returns></returns>
        public LfxResult Release() => unManagedDll.GetProcAddress<Functions.LFX_Release>()();
        
        /// <summary>
        /// This function sets all lights in the Alienware AlienFX system to ‘off’ or uncolored state.
        /// It must be noted that the change(s) to the physical light(s) does not occur immediately.
        /// The change(s) occurs only after a call to the LFX_Update function.
        /// For example, to disable all the lights, call LFX_Reset followed by LFX_Update.
        /// </summary>
        /// <returns></returns>
        public LfxResult Reset() => unManagedDll.GetProcAddress<Functions.LFX_Reset>()();
        
        /// <summary>
        /// This function updates the Alienware AlienFX system by submitting any state changes (since the last call to LFX_Reset) to the hardware.
        /// </summary>
        /// <returns></returns>
        public LfxResult Update() => unManagedDll.GetProcAddress<Functions.LFX_Update>()();
        
        /// <summary>
        /// This function gets the number of Alienware AlienFX devices attached to the system.
        /// </summary>
        /// <param name="numDevices">integer to be populated with the number of devices</param>
        /// <returns></returns>
        public LfxResult GetNumDevices(out uint numDevices) => 
            unManagedDll.GetProcAddress<Functions.LFX_GetNumDevices>()(out numDevices);
        
        /// <summary>
        /// This function gets the description and type of a device attached to the system.
        /// </summary>
        /// <param name="devIndex">index to the target device </param>
        /// <param name="devDesc">character array to be populated with the description of the target device</param>
        /// <param name="devDescSize">size of the character array provided in devDesc</param>
        /// <param name="devType">unsigned short to be populated with the device type </param>
        /// <returns></returns>
        public LfxResult GetDeviceDescription(uint devIndex, StringBuilder devDesc, out LfxDeviceType devType) =>
            unManagedDll.GetProcAddress<Functions.LFX_GetDeviceDescription>()(devIndex, devDesc, devDesc.Capacity, out devType);
        
        /// <summary>
        /// This function gets the number of Alienware AlienFX lights attached to a device in the system.
        /// </summary>
        /// <param name="devIndex">Index to the device </param>
        /// <param name="numLights">Unsigned integer to be populated with the number of lights at the device index</param>
        /// <returns></returns>
        public LfxResult GetNumLights(uint devIndex, out uint numLights) =>
            unManagedDll.GetProcAddress<Functions.LFX_GetNumLights>()(devIndex, out numLights);
        
        /// <summary>
        /// This function gets the description of a light attached to the system.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="lightDesc">Character array to be populated with the description of the target light</param>
        /// <param name="lightDescSize">Size of the character array provided in lightDesc</param>
        /// <returns></returns>
        public LfxResult GetLightDescription(uint devIndex, uint lightIndex, StringBuilder lightDesc) => 
            unManagedDll.GetProcAddress<Functions.LFX_GetLightDescription>()(devIndex, lightIndex, lightDesc, lightDesc.Capacity);
        
        /// <summary>
        /// This function gets the location of a light attached to the system.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="lightLoc">Pointer to an LFX_POSITION structure to be populated with the light location</param>
        /// <returns></returns>
        public LfxResult GetLightLocation(uint devIndex, uint lightIndex, out LfxPosition lightLoc) =>
            unManagedDll.GetProcAddress<Functions.LFX_GetLightLocation>()(devIndex, lightIndex, out lightLoc);
        
        /// <summary>
        /// This function gets the color of a light attached to the system.
        /// This function provides the current color stored in the active state.
        /// It does not necessarily represent the color of the physical light.
        /// To ensure that the returned value represents the state of the physical light, call this function immediately after a call to LFX_Update.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="lightCol">Pointer to an LFX_COLOR structure to be populated with the light location</param>
        /// <returns></returns>
        public LfxResult GetLightColor(uint devIndex, uint lightIndex, ref LfxColor lightCol) =>
            unManagedDll.GetProcAddress<Functions.LFX_GetLightColor>()(devIndex, lightIndex, ref lightCol);
        
        /// <summary>
        /// This function submits a light command into the command queue, which sets the current color of a light to the provided color value.
        /// This function changes the current color stored in active state since the last reset.
        /// It does not immediately update the physical light settings, instead requires a call to LFX_Update.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="lightCol">Pointer to an LFX_COLOR structure to be populated with the light location</param>
        /// <returns></returns>
        public LfxResult SetLightColor(uint devIndex, uint lightIndex, LfxColor lightCol) =>
            unManagedDll.GetProcAddress<Functions.LFX_SetLightColor>()(devIndex, lightIndex, lightCol);
        
        /// <summary>
        /// This function submits a light command into the command queue, which sets the current color of any light within the provided location mask to the provided color setting.
        /// Similar to LFX_SetLightColor, these settings are changed in the active state and must be submitted with a call to LFX_Update.
        /// Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness. 
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LfxLocationMask.</param>
        /// <param name="lightCol">32-bit color value</param>
        /// <returns></returns>
        public LfxResult Light(LfxLocationMask locationMask, LfxColorEncode lightCol) =>
            unManagedDll.GetProcAddress<Functions.LFX_Light>()(locationMask, (uint) lightCol);
        
        /// <summary>
        /// This function submits a light command into the command queue, which sets the current color of any light within the provided location mask to the provided color setting.
        /// Similar to LFX_SetLightColor, these settings are changed in the active state and must be submitted with a call to LFX_Update.
        /// Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LfxLocationMask.</param>
        /// <param name="lightCol">32-bit color value</param>
        /// <returns></returns>
        public LfxResult Light(LfxLocationMask locationMask, LfxColor lightCol) =>
            unManagedDll.GetProcAddress<Functions.LFX_Light>()(locationMask, ConvertToUint(lightCol));

        /// <summary>
        /// This function sets the primary color and an action type to a light.
        /// It changes the current color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is a morph, then the secondary color for the action is black.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">Pointer to an LFX_COLOR structure with the desired color</param>
        /// <returns></returns>
        public LfxResult SetLightActionColor(uint devIndex, uint lightIndex, LfxActionType actionType, LfxColor primaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_SetLightActionColor>()(devIndex, lightIndex, actionType, primaryColor);
        
        /// <summary>
        /// This function sets the primary and secondary colors and an action type to a light.
        /// It changes the current color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is not a morph, then the secondary color is ignored.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">Pointer to an LFX_COLOR structure with the desired color</param>
        /// <param name="secondaryColor">Pointer to an LFX_COLOR structure with the desired secondary color</param>
        /// <returns></returns>
        public LfxResult SetLightActionColorEx(uint devIndex, uint lightIndex, LfxActionType actionType, LfxColor primaryColor, LfxColor secondaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_SetLightActionColorEx>()(devIndex, lightIndex, actionType, primaryColor, secondaryColor);
        
        /// <summary>
        /// This function sets the primary color and an action type for any devices with lights in a location.
        /// It changes the current primary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is a morph, then the secondary color for the action is black. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <returns></returns>
        public LfxResult ActionColor(LfxLocationMask locationMask, LfxActionType actionType, LfxColorEncode primaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColor>()(locationMask, actionType, (uint) primaryColor);
        
        /// <summary>
        /// This function sets the primary color and an action type for any devices with lights in a location.
        /// It changes the current primary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is a morph, then the secondary color for the action is black. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <returns></returns>
        public LfxResult ActionColor(LfxLocationMask locationMask, LfxActionType actionType, LfxColor primaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColor>()(locationMask, actionType, ConvertToUint(primaryColor));
        
        /// <summary>
        /// This function sets the primary and secondary color and an action type for any devices with lights in a location.
        /// It changes the current primary and secondary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update.
        /// If the action type is not a morph, then the secondary color is ignored. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <param name="secondaryColor">32-bit secondary color value</param>
        /// <returns></returns>
        public LfxResult ActionColorEx(LfxLocationMask locationMask, LfxActionType actionType, LfxColorEncode primaryColor, LfxColorEncode secondaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColorEx>()(locationMask, actionType, (uint) primaryColor, (uint) secondaryColor);
        
        /// <summary>
        /// This function sets the primary and secondary color and an action type for any devices with lights in a location.
        /// It changes the current primary and secondary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update.
        /// If the action type is not a morph, then the secondary color is ignored. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <param name="secondaryColor">32-bit secondary color value</param>
        /// <returns></returns>
        public LfxResult ActionColorEx(LfxLocationMask locationMask, LfxActionType actionType, LfxColor primaryColor, LfxColor secondaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColorEx>()(locationMask, actionType, ConvertToUint(primaryColor), ConvertToUint(secondaryColor));
        
        /// <summary>
        /// This function gets the version of the SDK installed in the system. 
        /// </summary>
        /// <param name="version">character array to be populated with the version</param>
        /// <returns></returns>
        public LfxResult GetVersion(StringBuilder version) =>
            unManagedDll.GetProcAddress<Functions.LFX_GetVersion>()(version, version.Capacity);
        
        private static uint ConvertToUint(LfxColor color) => 
            (uint)(((color.brightness << 24) | (color.red << 16) | (color.green << 8) | color.blue) & 0xffffffffL);

        public void Dispose() => unManagedDll?.Dispose();
        
        
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        /// <summary>
        /// This function submits a light command into the command queue, which sets the current color of any light within the provided location mask to the provided color setting.
        /// Similar to LFX_SetLightColor, these settings are changed in the active state and must be submitted with a call to LFX_Update.
        /// Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LfxLocationMask.</param>
        /// <param name="lightCol">32-bit color value</param>
        /// <returns></returns>
        public LfxResult Light(LfxLocationMask locationMask, UnityEngine.Color lightCol) =>
            unManagedDll.GetProcAddress<Functions.LFX_Light>()(locationMask, ConvertToUint(lightCol));

        /// <summary>
        /// This function sets the primary color and an action type for any devices with lights in a location.
        /// It changes the current primary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is a morph, then the secondary color for the action is black. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <returns></returns>
        public LfxResult ActionColor(LfxLocationMask locationMask, LfxActionType actionType, UnityEngine.Color primaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColor>()(locationMask, actionType, ConvertToUint(primaryColor));

        /// <summary>
        /// This function sets the primary and secondary color and an action type for any devices with lights in a location.
        /// It changes the current primary and secondary color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update.
        /// If the action type is not a morph, then the secondary color is ignored. Location mask is a 32-bit field, where each of the first 27 bits represents a zone in the virtual cube representing the system.
        /// The color is packed into a 32-bit value as ARGB, with the alpha value corresponding to brightness.
        /// </summary>
        /// <param name="locationMask">32-bit location mask. See the defined values in LocationMask</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">32-bit color value</param>
        /// <param name="secondaryColor">32-bit secondary color value</param>
        /// <returns></returns>
        public LfxResult ActionColorEx(LfxLocationMask locationMask, LfxActionType actionType, UnityEngine.Color primaryColor, UnityEngine.Color secondaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_ActionColorEx>()(locationMask, actionType, ConvertToUint(primaryColor), ConvertToUint(secondaryColor));

        /// <summary>
        /// This function sets the primary color and an action type to a light.
        /// It changes the current color and action type stored in the active state since the last LFX_Reset() call.
        /// It does NOT immediately update the physical light settings, but instead requires a call to LFX_Update().
        /// If the action type is a morph, then the secondary color for the action is black.
        /// </summary>
        /// <param name="devIndex">Index to the target device</param>
        /// <param name="lightIndex">Index to the target light</param>
        /// <param name="actionType">Action type</param>
        /// <param name="primaryColor">Pointer to an LFX_COLOR structure with the desired color</param>
        /// <returns></returns>
        public LfxResult SetLightActionColor(uint devIndex, uint lightIndex, LfxActionType actionType, UnityEngine.Color primaryColor) =>
            unManagedDll.GetProcAddress<Functions.LFX_SetLightActionColor>()(devIndex, lightIndex, actionType, ConvertToLfxColor(primaryColor));
        
        private static uint ConvertToUint(UnityEngine.Color color)
        {
            UnityEngine.Color32 col = color; 
            return (uint) (((col.a << 24) | (col.r << 16) | (col.g << 8) | col.b) & 0xffffffffL);
        }
        
        private static LfxColor ConvertToLfxColor(UnityEngine.Color color)
        {
            UnityEngine.Color32 col = color; 
            return new LfxColor(col.r, col.g, col.b, col.a);
        }
#endif
    }
}