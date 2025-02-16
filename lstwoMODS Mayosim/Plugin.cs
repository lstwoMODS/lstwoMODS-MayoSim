using BepInEx;
using UnityEngine;
using System.Reflection;
using System.Collections;
using lstwoMODS_Core.UI.TabMenus;

namespace lstwoMODS_MayoSim
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
        public static Plugin Instance { get; private set; }

        public static HacksTab PlayerHacksTab { get; private set; } = new("Player Mods");
        public static HacksTab ServerHacksTab { get; private set; } = new("Game Mods");

        private void Awake()
        {
            Instance = this;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        public static Coroutine _StartCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }
    }
}