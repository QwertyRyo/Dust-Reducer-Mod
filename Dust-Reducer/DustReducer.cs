using System.Collections.Generic;
using HarmonyLib;
using MelonLoader;
using GHPC.Effects;
using UnityEngine;

[assembly: MelonInfo(typeof(DustReducer.DustReducerMod), "Dust Reducer", "1.0.0", "Qwertyryo")]
[assembly: MelonGame("Radian Simulations LLC", "GHPC")]

namespace DustReducer {

  public class DustReducerMod : MelonMod {
    public static readonly HashSet<string> CancelledEffects = new HashSet<string> {
      "Artillery 155mm Terrain(Clone)",
      "250kg Bomb Dirt(Clone)",
      "Autocannon HE Dirt Small(Clone)",
      "Autocannon Dirt Medium(Clone)",
      "Autocannon Dirt Small(Clone)",
      "APFSDS Dirt Large(Clone)",
      "HEAT Dirt Impact Medium(Clone)",
      "HEAT Dirt Impact Large(Clone)",
      "HEAT Rocket Dirt Small(Clone)",
      "Cannon Kinetic Dirt Small(Clone)",
      "Bullet Dirt Large(Clone)"
    };

    public static MelonPreferences_Entry<bool> DisableImpactSmoke;
    public static MelonPreferences_Entry<bool> DisableGunSmoke;

    public override void OnInitializeMelon() {
      MelonPreferences_Category cfg = MelonPreferences.CreateCategory("DustReducer");
      DisableImpactSmoke = cfg.CreateEntry("DisableImpactSmoke", true, "Setting to True disables impact dust/dirt effects (WARNING: Use audio cues to avoid artillery barrages, as this will remove ALL DUST from artillery). Determining if your shot went over or under will also be harder.");
      DisableGunSmoke = cfg.CreateEntry("DisableGunSmoke", true, "Setting to True disables gun barrel and muzzle smoke effects.");
      MelonPreferences.Save();

      try {
        HarmonyInstance.PatchAll();
      } catch (System.Exception ex) {
        MelonLogger.Error($"Patching failed: {ex}");
      }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
      BarrelSmoke.OnSceneWasLoadedHandler(buildIndex, sceneName);
    }
  }


  [HarmonyPatch(typeof(ParticleEffectsManager), "CreateImpactEffectOfType")]
  public class CreateImpactEffectPatch {
    public static void Postfix(GameObject __result) {
      if (!DustReducerMod.DisableImpactSmoke.Value) { return; }

      if (__result != null && DustReducerMod.CancelledEffects.Contains(__result.name)) {
        __result.SetActive(false);
        ParticleEffectTags tags = __result.GetComponent<ParticleEffectTags>();
        if (tags != null) {
          ParticleEffectsManager.Instance.StoreEffectInPool(tags);
        }
      }
    }
  }
}
