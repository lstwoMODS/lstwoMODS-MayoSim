using BepInEx;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib;
using UniverseLib.Config;
using NotAzzamods.UI;
using System.Collections.Generic;
using NotAzzamods.UI.TabMenus;
using NotAzzamods.Hacks;
using System.Reflection;
using System.Collections;
using System;
using BepInEx.Logging;
using NotAzzamods.UI.Keybinds;
using NotAzzamods.Keybinds;
using BepInEx.Configuration;
using System.Linq;
using ShadowLib;

namespace NotAzzamods
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // ASSETS
        public static AssetBundle AssetBundle { get; private set; }

        // QUICK ACCESS
        public const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
        
        public static ManualLogSource LogSource { get => Instance.Logger; }
        public static ConfigFile ConfigFile { get => Instance.Config; }

        // INSTANCES
        public static Plugin Instance { get; private set; }

        public static UIBase UiBase { get; private set; }
        public static MainPanel MainPanel { get; private set; }
        public static KeybindPanel KeybindPanel { get; private set; }

        public static List<BaseTab> TabMenus { get; private set; } = new();
        public static List<BaseHack> Hacks { get; private set; } = new();

        // TABS
        public static HacksTab PlayerHacksTab { get; private set; } = new("Player Mods");
        public static HacksTab ServerHacksTab { get; private set; } = new("Game Mods");

        // OTHER FEATURES
        public static KeybindManager KeybindManager { get; private set; }

        private void Awake()
        {
            Instance = this;
            AssetBundle = AssetUtils.LoadAssetBundleFromPluginsFolder("lstwo.lstwomods.assets");
            KeybindManager = gameObject.AddComponent<KeybindManager>();

            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Start()
        {
            InitMods();
            InitUI();
        }

        public static void InitMods()
        {
            InitChildClasses<BaseHack>();
        }

        public static void InitUI()
        {
            float startupDelay = 1f;
            UniverseLibConfig config = new()
            {
                Disable_EventSystem_Override = false,
                Force_Unlock_Mouse = true
            };

            Universe.Init(startupDelay, () =>
            {
                UiBase = UniversalUI.RegisterUI("lstwo.NotAzza", null);

                MainPanel = new(UiBase);
                KeybindPanel = new(UiBase);

                KeybindPanel.Enabled = false;
                UiBase.Enabled = false;
            }, null, config);
        }

        public static void InitChildClasses<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var childTypes = assemblies.SelectMany(assembly => assembly.GetTypes()).Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);

            foreach (var type in childTypes)
            {
                Activator.CreateInstance(type);
            }
        }

        public static Coroutine _StartCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F2))
            {
                ToggleUI();
            }

            foreach(var hack in Hacks)
            {
                hack.Update();
            }
        }

        private void ToggleUI()
        {
            var enabled = !UiBase.Enabled;
            UiBase.Enabled = enabled;

            if (enabled)
            {
                MainPanel.Refresh();
            }
        }
    }
}