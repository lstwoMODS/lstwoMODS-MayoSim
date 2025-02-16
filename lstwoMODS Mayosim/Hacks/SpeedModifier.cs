using HarmonyLib;
using NotAzzamods.UI.TabMenus;
using ShadowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotAzzamods.Hacks
{
    internal class SpeedModifier : BaseHack
    {
        public static float walkSpeed = 750f;
        public static float sprintSpeed = 1100f;

        public override string Name => "Speed Modifier";
        public override string Description => "";
        public override HacksTab HacksTab => Plugin.PlayerHacksTab;

        private HacksUIHelper.LIBTrio walkSpeedLIB;
        private HacksUIHelper.LIBTrio sprintSpeedLIB;

        public override void ConstructUI(GameObject root)
        {
            var ui = new HacksUIHelper(root);
            new Harmony("lstwo.lstwoMODS.speedModifier").PatchAll(typeof(Patches));

            ui.AddSpacer(6);

            walkSpeedLIB = ui.CreateLIBTrio("Player Walking Speed", "playerWalkSpeed", "750.0");
            walkSpeedLIB.Input.Component.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Decimal;
            walkSpeedLIB.Button.OnClick = () =>
            {
                walkSpeed = float.Parse(walkSpeedLIB.Input.Text);
            };

            ui.AddSpacer(6);

            sprintSpeedLIB = ui.CreateLIBTrio("Player Sprint Speed", "playerSprintSpeed", "1100.0");
            sprintSpeedLIB.Input.Component.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Decimal;
            sprintSpeedLIB.Button.OnClick = () =>
            {
                sprintSpeed = float.Parse(sprintSpeedLIB.Input.Text);
            };

            ui.AddSpacer(6);
        }

        public override void RefreshUI()
        {
            walkSpeedLIB.Input.Text = walkSpeed.ToString();
            sprintSpeedLIB.Input.Text = sprintSpeed.ToString();
        }

        public override void Update()
        {
        }

        public class Patches
        {
            [HarmonyPatch(typeof(playerController), "Update")]
            [HarmonyPriority(100)]
            [HarmonyPrefix]
            public static bool UpdatePatch(ref playerController __instance)
            {
                var r = new QuickReflection<playerController>(__instance, Plugin.Flags);

                if (Input.GetButton("Shift"))
                {
                    __instance.speed = sprintSpeed;
                    return false;
                }

                __instance.speed = walkSpeed;

                return false;
            }
        }
    }
}
