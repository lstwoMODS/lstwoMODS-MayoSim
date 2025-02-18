using HarmonyLib;
using lstwoMODS_Core;
using lstwoMODS_Core.Hacks;
using lstwoMODS_Core.UI.TabMenus;
using ShadowLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lstwoMODS_MayoSim.Hacks
{
    internal class PowerUpModifier : BaseHack
    {
        public static bool infinitePowerUps;

        public override string Name => "Power Up Modifier";

        public override string Description => "";

        public override HacksTab HacksTab => Plugin.PlayerHacksTab;

        public override void ConstructUI(GameObject root)
        {
            var ui = new HacksUIHelper(root);
            new Harmony("lstwo.lstwoMODS.PowerUpModifier").PatchAll(typeof(Patches));

            ui.AddSpacer(6);

            ui.CreateToggle("lstwo.PowerUpModifier.InfinitePowerUps", "Infinite Power Ups", (b) => infinitePowerUps = b);

            ui.AddSpacer(6);

            ui.CreateLBBTrio("Trigger Power Up", "lstwo.PowerUpModifier.TriggerPowerUps", () =>
            {
                UnityEngine.Object.FindObjectOfType<FireProjectile>().powerUpShooting = true;
            }, "Shooting Power Up", "lstwo.PowerUpModifier.TriggerShootingPowerUp", () =>
            {
                UnityEngine.Object.FindObjectOfType<PlayerCollision>().health += UnityEngine.Random.Range(1, 4);
            }, "Health Power Up", "lstwo.PowerUpModifier.TriggerHealthPowerUp");

            ui.AddSpacer(6);
        }

        public override void RefreshUI()
        {
        }

        public override void Update()
        {
        }

        public class Patches
        {
            [HarmonyPatch(typeof(FireProjectile), "Update")]
            [HarmonyPrefix]
            public static bool UpdatePatch(ref FireProjectile __instance)
            {
                var r = new QuickReflection<FireProjectile>(__instance, Plugin.Flags);

                if (__instance.powerUpShooting)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        r.SetField("fireIntent", true);

                        if(!infinitePowerUps)
                        {
                            __instance.StartCoroutine((IEnumerator)r.GetMethod("deactivatePowerUp"));
                        }

                        return false;
                    }
                }

                else if (Input.GetButtonDown("Fire1"))
                {
                    r.SetField("fireIntent", true);
                }

                return false;
            }
        }
    }
}
