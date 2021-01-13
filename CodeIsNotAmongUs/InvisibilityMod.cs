using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using Hazel;
using System;
using System.IO;
using Hazel.Udp;
using Reactor;

namespace VentingCrew
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    [ReactorPluginSide(PluginSide.ClientOnly)]
    public class InvisibilityMod : BasePlugin
    {
        public const string Id = "net.minethingo.InvisibilityMod";
        public List<byte> invisPlayers = new List<byte>();
        Harmony Harmony = new Harmony(Id);

        public override void Load()
        {
            Harmony.PatchAll();
        }
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch
        {
            public static void Prefix()
            {
                PluginSingleton<InvisibilityMod>.Instance.invisPlayers.Clear();
            }
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.LateUpdate))]
        public static class PlayerPhysicsLateUpdatePatch
        {
            public static void Prefix(PlayerPhysics __instance)
            {
                if (PluginSingleton<InvisibilityMod>.Instance.invisPlayers.Contains(__instance.Field_5.PlayerId))
                {
                    if (PlayerControl.LocalPlayer.Data.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead)
                    {
                        __instance.Field_5.SetHatAlpha(0.2f);
                    } else
                    {
                        __instance.Field_5.SetHatAlpha(0f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class RpcPatch
        {
            public static void Prefix(PlayerControl __instance, [HarmonyArgument(1)] MessageReader message, [HarmonyArgument(0)] int callId)
            {
                if (callId == 140)
                {
                    bool isInvis = message.ReadBoolean();
                    RpcFunctions.TurnInvis(isInvis, __instance);
                }
            }
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
        public static class HudStartPatch
        {
            public static void Postfix(HudManager __instance)
            {
                CooldownButton button = new CooldownButton(
                    () => { RpcFunctions.RpcTurnInvis(true); },
                    30f,
                    "VentingCrew.Resources.invisibility.png",
                    270f,
                    Vector2.zero,
                    CooldownButton.Category.OnlyImpostor,
                    __instance,
                    10f,
                    () => { RpcFunctions.RpcTurnInvis(false); }
                 );
            }
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudUpdatePatch
        {
            public static void Postfix(HudManager __instance)
            {
                CooldownButton.HudUpdate();
            }
        }
        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        public static class VersionPatch
        {
            public static void Postfix(VersionShower __instance)
            {
                __instance.text.Text += " [00FF00FF]Modeed by minethingo[]";
            }
        }
    }
}