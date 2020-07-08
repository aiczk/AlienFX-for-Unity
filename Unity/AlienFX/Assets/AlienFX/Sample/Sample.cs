using System;
using System.Text;
using System.Threading;
using AlienFX.Util;
using UnityEngine;

namespace AlienFX
{
    internal class Sample : MonoBehaviour
    {
        private void Start()
        {
            var lfx = new LightFx();

            var result = lfx.Initialize();
            switch (result)
            {
                case LfxResult.Success:
                    break;
                case LfxResult.Failure:
                    Debug.LogError("Failed to load LightFX.dll.");
                    return;
                case LfxResult.ErrorNoDevs:
                    Debug.LogError("There is not AlienFX device available.");
                    return;
                default:
                    Debug.LogError("There was an error initializing the AlienFX device.");
                    return;
            }
            
            var version = new StringBuilder(255);
            result = lfx.GetVersion(version);
            Debug.Log($"SDK Version: {version}");

            result = lfx.GetNumDevices(out var numDevices);
            Debug.Log($"Devices: {numDevices.ToString()}");

            for (uint devIndex = 0; devIndex < numDevices; devIndex++)
            {
                var description = new StringBuilder(255);
                
                result = lfx.GetDeviceDescription(devIndex, description, out _ /*var devType*/);
                Debug.Log($"Description: {description}");
                
                description = new StringBuilder(255);
                result = lfx.GetNumLights(devIndex, out var numLights);
                for (uint lightIndex = 0; lightIndex < numLights; lightIndex++)
                {
                    result = lfx.GetLightDescription(devIndex, lightIndex, description);
                    
                    if(result != LfxResult.Success)
                        continue;
                    
                    Debug.Log($"\tLight: {lightIndex.ToString()}\tDescription: {description}");
                }
            }
            
            Thread.Sleep(1000);
            lfx.Reset();
            
            for (var i = 0; i <= 0; i++)
            {
                var color = LfxColorEncode.Orange.Brightness(LfxBrightness.Full);
                lfx.Light(LfxLocationMask.All, color);
                //lfx.Light(LfxLocationMask.All, new Color(0, 1, 0, 1));
                //lfx.Light(LfxLocationMask.All, new LfxColor(0, 255, 0, 255));
                lfx.Update();
                Debug.Log($"Color: {color:X}");
                Thread.Sleep(100);
            }
            
            lfx.Release();
        }
    }
}