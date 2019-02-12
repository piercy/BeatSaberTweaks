using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberTweaks.HarmonyPatches
{
    // Taken from the Harmony Discord server from the dev himself
    // HarmonyPatch(Type declaringType, string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
    [HarmonyPatch(typeof(RenderingScaleSettingsController), "GetInitValues", new Type[] { typeof(int), typeof(int) }, new ArgumentType[] { ArgumentType.Out, ArgumentType.Out })]
    class RenderingScaleSettingsControllerGetInitValues
    {
        static bool Prefix(RenderingScaleSettingsController __instance, out int idx, out int numberOfElements)
        {
            float[] tempResolutionScales = new float[]
            {
                0.5f,
                0.6f,
                0.7f,
                0.8f,
                0.9f,
                1f,
                1.1f,
                1.2f,
                1.3f,
                1.4f,
                1.5f,
                1.6f,
                1.7f,
                1.8f,
                1.9f,
                2f
            };

            idx = 2;
            numberOfElements = tempResolutionScales.Length;
            
            ReflectionUtil.SetPrivateField(__instance, "_resolutionScales", tempResolutionScales);

            var mainSettingsModel = ReflectionUtil.GetPrivateField<MainSettingsModel>(__instance, "_mainSettingsModel");

            float vrResolutionScale = mainSettingsModel.vrResolutionScale;
            for (int i = 0; i < tempResolutionScales.Length; i++)

            {
                if (vrResolutionScale == tempResolutionScales[i])
                {
                    idx = i;
                    return false;
                }
            }
            return false;
        }
    }
}
