using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;
using GHPC.Mission;
using GHPC.State;
using GHPC.Vehicle;

namespace DustReducer
{
    public static class BarrelSmoke
    {
        static bool _sceneActive = false;

        static readonly string[] StandardChildren = new string[] {
            "105mm Muzzle Flash/Muzzle Flash Front Smoke 1",
            "105mm Muzzle Flash/Muzzle Flash Front Smoke 2",
            "105mm Muzzle Flash/Muzzle Flash Side Smoke",
            "105mm Muzzle Flash/Dust Kickup 1/Huge Dust",
            "105mm Muzzle Flash/Dust Kickup 2/Huge Dust",
            "Bore Evac FX Large/Side Gunsmoke",
            "Bore Evac FX Large/Smoke Trail"
        };

        static readonly Dictionary<string, (string basePath, string[] children)> VehicleSmoke = new Dictionary<string, (string, string[])> {
            { "T72", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72ULEM", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72UV1", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72UV2", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72GILLS", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72M", ("T72M_skirts_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T72M1", ("---MESH---/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64A", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64B", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64A84", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64B81", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64A74", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64A79", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64A81", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64B1", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "T64B181", ("---T64A_MESH---/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46M/GameObject", StandardChildren) },
            { "M60A1", ("--RIG/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M60A1AOS", ("--RIG/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M60A1RISEP", ("--RIG/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M60A1RISEP77", ("--RIG/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M60A3", ("M60A3TTS_rig/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M60A3TTS", ("M60A3TTS_rig/hull/turret/main gun mantlet/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M1", ("IPM1_rig/HULL/TURRET/GUN/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "M1IP Abrams", ("IPM1_rig/HULL/TURRET/GUN/Gun Scripts/105mm Gun M68/GameObject", StandardChildren) },
            { "LEO2", ("leo2_rig/hull/turret/mantlet/---MAIN GUN SCRIPTS---/120mm Gun Rh-120 L44/muzzle end", StandardChildren) },
            { "LEO2A1", ("leo2_rig/hull/turret/mantlet/---MAIN GUN SCRIPTS---/120mm Gun Rh-120 L44/muzzle end", StandardChildren) },
            { "LEO2A2", ("leo2_rig/hull/turret/mantlet/---MAIN GUN SCRIPTS---/120mm Gun Rh-120 L44/muzzle end", StandardChildren) },
            { "LEO2A3", ("leo2_rig/hull/turret/mantlet/---MAIN GUN SCRIPTS---/120mm Gun Rh-120 L44/muzzle end", StandardChildren) },
            { "LEO1A4", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A1", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A1A2", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A1A3", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A1A4", ("LEO1A1A1_rig/HULL/TURRET/Mantlet/gun/1A1 gun net", StandardChildren) },
            { "LEO1A3", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A3A1", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A3A2", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "LEO1A3A3", ("LEO1A1A1_rig/HULL/TURRET/1A3 mantlet/--Gun Scripts--/105mm Gun L7A3 L52/GameObject", StandardChildren) },
            { "T62", ("---T62_rig---/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/115mm Gun U-5TS/GameObject", StandardChildren) },
            { "T80B", ("T80B_rig/HULL/TURRET/gun/---MAIN GUN SCRIPTS---/125mm Gun 2A46-2/GameObject", StandardChildren) },
            { "T64R", ("T64R_rig/HULL/TURRET/Main gun/---MAIN GUN SCRIPTS---/115mm Gun D-68/GameObject", StandardChildren) },
            { "PT76B", ("PT76_rig/HULL/TURRET/GUN/---MAIN GUN SCRIPTS---/76mm Gun D-56TS/GameObject/D-56T Muzzle Flash", new string[] {
                "Dust Kickup 1/Huge Dust",
                "Dust Kickup 2/Huge Dust",
                "Gunsmoke Side",
                "Gunsmoke Front Long",
                "Gunsmoke Front"
            }) },

            { "M2BRADLEY", ("M2BRADLEY_rig/HULL/Turret/Mantlet/25mm Gun M242/M242 Bushmaster Muzzle Flash", new string[] {
                "Gunsmoke Front",
                "Gunsmoke Side",
                "Gunsmoke Brake Side R",
                "Gunsmoke Brake Side L",
                "Gunsmoke Brake Side Top",
                "Gunsmoke Long",
                "Gunsmoke Front R",
                "Gunsmoke Front L"
            }) },

            { "BMP1", ("BMP1_rig/HULL/TURRET/GUN/Gun Scripts/73mm Gun 2A28 Grom/GameObject", new string[] {
                "Grom Muzzle Flash 1/Muzzle Flash Front Smoke",
                "Grom Muzzle Flash 1/Muzzle Flash Side Smoke",
                "Bore Evac FX Small/Side Gunsmoke",
                "Bore Evac FX Small/Smoke Trail"
            }) },

            { "BMP1_SA", ("BMP1_rig/HULL/TURRET/GUN/Gun Scripts/73mm Gun 2A28 Grom/GameObject", new string[] {
                "Grom Muzzle Flash 1/Muzzle Flash Front Smoke",
                "Grom Muzzle Flash 1/Muzzle Flash Side Smoke",
                "Bore Evac FX Small/Side Gunsmoke",
                "Bore Evac FX Small/Smoke Trail"
            }) },

            { "BMP1P", ("BMP1_rig/HULL/TURRET/GUN/Gun Scripts/73mm Gun 2A28 Grom/GameObject", new string[] {
                "Grom Muzzle Flash 1/Muzzle Flash Front Smoke",
                "Grom Muzzle Flash 1/Muzzle Flash Side Smoke",
                "Bore Evac FX Small/Side Gunsmoke",
                "Bore Evac FX Small/Smoke Trail"
            }) },

            { "BMP1P_SA", ("BMP1_rig/HULL/TURRET/GUN/Gun Scripts/73mm Gun 2A28 Grom/GameObject", new string[] {
                "Grom Muzzle Flash 1/Muzzle Flash Front Smoke",
                "Grom Muzzle Flash 1/Muzzle Flash Side Smoke",
                "Bore Evac FX Small/Side Gunsmoke",
                "Bore Evac FX Small/Smoke Trail"
            }) },

            { "BMP2", ("BMP2_rig/HULL/TURRET/Main gun/mantlet scripts/30mm Gun 2A42/2A42 Shipunov", new string[] {
                "Gunsmoke Front",
                "Gunsmoke Side",
                "Gunsmoke Brake Side R",
                "Gunsmoke Brake Side L",
                "Gunsmoke Brake Side Top",
                "Gunsmoke Long",
                "Gunsmoke Front R",
                "Gunsmoke Front L"
            }) },

            { "BMP2_SA", ("BMP2_rig/HULL/TURRET/Main gun/mantlet scripts/30mm Gun 2A42/2A42 Shipunov", new string[] {
                "Gunsmoke Front",
                "Gunsmoke Side",
                "Gunsmoke Brake Side R",
                "Gunsmoke Brake Side L",
                "Gunsmoke Brake Side Top",
                "Gunsmoke Long",
                "Gunsmoke Front R",
                "Gunsmoke Front L"
            }) },

            { "MARDERA1PLUS", ("Marder1A1_rig/hull/turret/gun", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "MARDERA1MINUS", ("Marder1A1_rig/hull/turret/gun", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "MARDERA1_NO_ATGM", ("Marder1A1_rig/hull/turret/gun", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "MARDER1A2", ("Marder1A1_rig/hull/turret/gun", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "LuchsA2", ("luchs_rig/HULL/TURRET/MANTLET", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "LuchsA1", ("luchs_rig/HULL/TURRET/MANTLET", new string[] {
                "20mm Gun MK20/Marder Vent Smoke long",
                "20mm Gun MK20/Marder Vent Smoke short",
                "20mm Gun MK20/Marder Vent Smoke rear",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Long",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front Short",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Side",
                "20mm Gun MK20/Mk20 Autocannon Muzzle Flash/Gunsmoke Front"
            }) },

            { "STATIC_SPG9", ("spg9 slow traverse/SPG9_rig/SPG9/AZIMUTH/ELEVATION/--GUN SCRIPTS--/Launcher SPG-9/RPG7 Muzzle Flash", new string[] {
                "Gunsmoke Flat",
                "Gunsmoke Billboard",
                "Gunsmoke Side"
            }) }
        };

        public static void OnSceneWasLoadedHandler(int buildIndex, string sceneName)
        {

            if (sceneName == "MainMenu2_Scene" || sceneName == "t64_menu" || sceneName == "MainMenu2-1_Scene" || sceneName == "leo2_menu")
            {
                _sceneActive = false;
                return;
            }

            GameObject gameManager = GameObject.Find("_APP_GHPC_");
            if (gameManager == null)
            {
                return;
            }

            StateController.RunOrDefer(GameState.GameReady, new GameStateEventHandler(DisableBarrelSmokeCoroutine), GameStatePriority.Medium);
        }

        public static IEnumerator DisableBarrelSmokeCoroutine(GameState _)
        {
            if (_sceneActive) { yield break; }
            _sceneActive = true;


            if (DustReducer.DustReducerMod.DisableGunSmoke.Value)
            {
                Vehicle[] vehicles = GameObject.FindObjectsByType<Vehicle>(FindObjectsSortMode.None);

                foreach (var vehicle in vehicles)
                {
                    if (vehicle == null || vehicle.gameObject == null) { continue; }


                    if (!VehicleSmoke.ContainsKey(vehicle.UniqueName))
                    {
                        continue;
                    }

                    var (basePath, children) = VehicleSmoke[vehicle.UniqueName];

                    Transform baseTransform = vehicle.transform.Find(basePath);
                    if (baseTransform == null)
                    {
                        continue;
                    }


                    foreach (var childPath in children)
                    {
                        Transform childTransform = baseTransform.Find(childPath);
                        if (childTransform != null)
                        {
                            childTransform.gameObject.SetActive(false);
                        }

                    }
                }
            }


            _sceneActive = false;
            yield break;
        }
    }
}
