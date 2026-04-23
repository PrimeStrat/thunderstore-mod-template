using HarmonyLib;

namespace MyMod.Patches;

// Example Harmony patch. Replace TargetType/TargetMethod with the game class you want to hook.
// Delete this file or expand into multiple patch classes as needed.
[HarmonyPatch]
internal static class ExamplePatch
{
    // [HarmonyPatch(typeof(SomeGameClass), nameof(SomeGameClass.SomeMethod))]
    // [HarmonyPostfix]
    // private static void Postfix()
    // {
    //     Plugin.Log.LogInfo("SomeMethod ran.");
    // }
}
