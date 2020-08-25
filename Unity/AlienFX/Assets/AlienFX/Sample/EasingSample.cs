using System;
using System.Collections;
using System.Globalization;
using AlienFX.Easing;
using AlienFX.Util;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AlienFX
{
    public class EasingSample : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            var ease = new AlienFxEasing();
            var lfx = new LightFx();
            lfx.Initialize();
            lfx.Reset(); 
            StartCoroutine(Test(ease.EaseInBounce, lfx));
            //StartCoroutine(Fades(lfx));
        }
        

        private IEnumerator Fades(LightFx lfx)
        {
            lfx.Light(LfxLocationMask.All, LfxColorEncode.Red);
            lfx.Update();
            yield return new WaitForSeconds(1);
            
            // lfx.Light(LfxLocationMask.All, new Color(0, 1, 0, 0.5f)); 
            // lfx.Update();
            // yield return new WaitForSeconds(1);
            //
            // lfx.Light(LfxLocationMask.All,  new Color(0, 1, 0, 1));
            // lfx.Update();
            // yield return new WaitForSeconds(1);

            yield return null;
            Debug.Log("end");
            lfx.Release();
        }

        private IEnumerator Test(EasingFunction easingFunction, LightFx lfx, float duration = 3000f)
        {
            var elapsed = 0f;
            while (true)
            {
                if (elapsed >= duration)
                    break;

                var rate = elapsed / duration;
                var value = easingFunction(rate * 1);
                Debug.Log(value.ToString("F"));

                lfx.Light(LfxLocationMask.All, new Color(0, 1, 0, value));
                lfx.Update();
                
                elapsed += 16.7f;
                yield return null;
            }
            
            yield return null;
            lfx.Release();
        }
    }
}
