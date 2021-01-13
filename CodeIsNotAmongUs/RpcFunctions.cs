using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Hazel;
using Reactor;

namespace VentingCrew
{
    public static class RpcFunctions
    {
        static public void RpcTurnInvis(bool isInvis)
        {
            var writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, 140, SendOption.Reliable);
            writer.Write(isInvis);
            writer.EndMessage();
            TurnInvis(isInvis, PlayerControl.LocalPlayer);
        }
        static public void TurnInvis(bool isInvis, PlayerControl player)
        {
            var playerRenderer = player.myRend;
            if (isInvis)
            {
                PluginSingleton<InvisibilityMod>.Instance.invisPlayers.Add(player.PlayerId);
                if (player == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsImpostor || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    playerRenderer.SetColorAlpha(0.2f);
                    player.HatRenderer.FrontLayer.SetColorAlpha(0.2f);
                    player.HatRenderer.BackLayer.SetColorAlpha(0.2f);
                    player.MyPhysics.Skin.layer.SetColorAlpha(0.2f);
                    if (player.CurrentPet != null)
                    {
                        player.CurrentPet.rend.SetColorAlpha(0.2f);
                        player.CurrentPet.shadowRend.SetColorAlpha(0.2f);
                    }
                }
                else
                {
                    playerRenderer.SetColorAlpha(0f);
                    player.HatRenderer.FrontLayer.SetColorAlpha(0f);
                    player.HatRenderer.BackLayer.SetColorAlpha(0f);
                    player.MyPhysics.Skin.layer.SetColorAlpha(0f);
                    if (player.CurrentPet != null)
                    {
                        player.CurrentPet.rend.SetColorAlpha(0f);
                        player.CurrentPet.shadowRend.SetColorAlpha(0f);
                    }
                    player.nameText.render.enabled = false;
                }
            }
            else
            {
                PluginSingleton<InvisibilityMod>.Instance.invisPlayers.Remove(player.PlayerId);
                playerRenderer.SetColorAlpha(1f);
                player.HatRenderer.FrontLayer.SetColorAlpha(1f);
                player.HatRenderer.BackLayer.SetColorAlpha(1f);
                player.MyPhysics.Skin.layer.SetColorAlpha(1f);
                if (player.CurrentPet != null)
                {
                    player.CurrentPet.rend.SetColorAlpha(1f);
                    player.CurrentPet.shadowRend.SetColorAlpha(1f);
                }
                player.nameText.render.enabled = true;
            }
        }
        static public void SetColorAlpha(this SpriteRenderer renderer, float alpha)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
        }
    }
}
