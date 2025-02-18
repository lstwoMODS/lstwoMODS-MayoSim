using HarmonyLib;
using lstwoMODS_Core;
using lstwoMODS_Core.Hacks;
using lstwoMODS_Core.UI.TabMenus;
using ShadowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace lstwoMODS_MayoSim.Hacks
{
    internal class JumpModifier : BaseHack
    {
        public static bool ignoreGrounded;
        public static bool jetpackMode;

        public override string Name => "Jump Modifier";
        public override string Description => "";
        public override HacksTab HacksTab => Plugin.PlayerHacksTab;

        public override void ConstructUI(GameObject root)
        {
            var ui = new HacksUIHelper(root);
            new Harmony("lstwo.lstwoMODS.jumpModifier").PatchAll(typeof(Patches));

            ui.AddSpacer(6);

            var jumpHeightLIB = ui.CreateLIBTrio("Jump Height", "lstwo.JumpModifier.JumpHeight", "50.0");
            jumpHeightLIB.Input.Component.characterValidation = UnityEngine.UI.InputField.CharacterValidation.Decimal;
            jumpHeightLIB.Button.OnClick = () =>
            {
                UnityEngine.Object.FindObjectOfType<playerController>().jumpForce = float.Parse(jumpHeightLIB.Input.Text);
            };

            ui.AddSpacer(6);

            ui.CreateToggle("lstwo.JumpModifier.AirJumps", "Allow Air Jumps", (b) => ignoreGrounded = b);

            ui.AddSpacer(6);

            ui.CreateToggle("lstwo.JumpModifier.Jetpack", "Jetpack Mode", (b) => jetpackMode = b);

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
            [HarmonyPatch(typeof(playerController), "Update")]
            [HarmonyPrefix]
            [HarmonyPriority(200)]
            public static bool UpdatePatch(ref playerController __instance)
            {
                var r = new QuickReflection<playerController>(__instance, Plugin.Flags);

                var grounded = (bool)r.GetMethod("IsGrounded") || ignoreGrounded;
                var input = Input.GetButtonDown("Jump") || (jetpackMode && Input.GetButton("Jump"));

                if (grounded && input)
                {
                    ((Rigidbody)r.GetField("rb")).AddForce(Vector3.up * __instance.jumpForce, ForceMode.Impulse);
                }

                return true;
            }
        }
    }
}
