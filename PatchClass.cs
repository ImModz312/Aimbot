using Harmony;
using Il2Cpp;
using Il2CppDecaGames.RotMG.Managers.Net;
using static MelonLoader.MelonLogger;

namespace BasicMelonMod.Patches
{
    // This is the class that HarmonyX is patching - in this case ALCOHEIDGKL is the Tile class for RotMG.
    /*[HarmonyLib.HarmonyPatch(typeof(HIFLMGKMLAK))]
    // This is the method that will be patched. Every time it is called in-game it will run through the patch below.
    // Prefix() runs the patch before the method call and Postfix() runs it after. Check MelonLoader documentation for more info.
    [HarmonyLib.HarmonyPatch("HILBBBOHAAH")]
    public static class FogPatch
    {

        [HarmonyLib.HarmonyPrefix]
        public static bool Prefix(HIFLMGKMLAK __instance, int __0)
        {
            return false;
        }
    }*/
    /*[HarmonyLib.HarmonyPatch(typeof(HJMBOMEHGDJ))]
    [HarmonyLib.HarmonyPatch("MBNGNCODGBF", typeof(HJMBOMEHGDJ))]
    public static class DropNotifController
    {
        [HarmonyLib.HarmonyPostfix]
        private static void Postfix(IBNHCOGBDLG PGOBHADNMND)
        {
            if (PGOBHADNMND.GetType().GetProperty("PBGKBLINMPM") != null)
            {
                switch (PGOBHADNMND.PBGKBLINMPM)
                {
                    case 0x050C:
                    case 0x0510:
                        // white bag

                        break;
                    case 0x050E:
                    case 0x6bc:
                        // yellow bag
                        break;
                    case 0x50f:
                    case 0x6bf:
                        // orange bag

                        break;
                    case 0x6ac:
                    case 0x6c0:
                        // red bag
                        break;
                }
            }
        }
    }*/
    // Add more Harmony patches here or expand the mod!
}
