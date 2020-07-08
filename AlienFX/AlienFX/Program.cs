using System;
using System.Text;
using System.Threading;
using AlienFX.Util;

namespace AlienFX
{
    internal class Program
    {
        public static void Main()
        {
            var lfx = new LightFx();

            var result = lfx.Initialize();
            switch (result)
            {
                case LfxResult.Success:
                    break;
                case LfxResult.Failure:
                    Console.WriteLine("Failed to load LightFX.dll.");
                    return;
                case LfxResult.ErrorNoDevs:
                    Console.WriteLine("There is not AlienFX device available.");
                    return;
                default:
                    Console.WriteLine("There was an error initializing the AlienFX device.");
                    return;
            }
            
            var version = new StringBuilder(255);
            result = lfx.GetVersion(version);
            Console.WriteLine($"SDK Version: {version}");

            result = lfx.GetNumDevices(out var numDevices);
            Console.WriteLine($"Devices: {numDevices.ToString()}");

            for (uint devIndex = 0; devIndex < numDevices; devIndex++)
            {
                var description = new StringBuilder(255);
                
                result = lfx.GetDeviceDescription(devIndex, description, out _ /*var devType*/);
                Console.WriteLine($"Description: {description}");
                
                description = new StringBuilder(255);
                result = lfx.GetNumLights(devIndex, out var numLights);
                for (uint lightIndex = 0; lightIndex < numLights; lightIndex++)
                {
                    result = lfx.GetLightDescription(devIndex, lightIndex, description);
                    
                    if(result != LfxResult.Success)
                        continue;
                    
                    Console.WriteLine($"\tLight: {lightIndex.ToString()}\tDescription: {description}");
                }
            }
            
            Thread.Sleep(1000);
            lfx.Reset();
            
            for (var i = 0; i <= 0; i++)
            {
                var color = LfxColorEncode.Orange.Brightness(LfxBrightness.Full);
                //lfx.Light(LfxLocationMask.All, new LfxColor(0, 255, 0, 255));
                lfx.Light(LfxLocationMask.All, color);
                lfx.Update();
                Console.WriteLine($"Color: {color:X}");
                Thread.Sleep(100);
            }

            Console.WriteLine("Done.\r\n\r\nPress any key to finish ...");
            Console.ReadKey();
            lfx.Release();
        }
    }
}