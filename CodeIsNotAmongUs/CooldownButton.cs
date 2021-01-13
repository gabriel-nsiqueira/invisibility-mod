using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using System.IO;
using UnhollowerBaseLib;
using Reactor.Unstrip;
using Reactor.Extensions;

namespace VentingCrew
{
    public class CooldownButton
    {
        public static List<CooldownButton> buttons = new List<CooldownButton>();
        public KillButtonManager killButtonManager;
        private Color startColorButton = new Color(255, 255, 255);
        private Color startColorText = new Color(255, 255, 255);
        public Vector2 PositionOffset = Vector2.zero;
        public float MaxTimer = 0f;
        public float Timer = 0f;
        public float EffectDuration = 0f;
        public bool isEffectActive;
        public bool hasEffectDuration;
        public bool enabled = true;
        public Category category;
        private string ResourceName;
        private Action OnClick;
        private Action OnEffectEnd;
        private HudManager hudManager;
        private float pixelsPerUnit;
        private bool canUse;

        public CooldownButton(Action OnClick, float Cooldown, string ImageEmbededResourcePath, float PixelsPerUnit, Vector2 PositionOffset, Category category, HudManager hudManager, float EffectDuration, Action OnEffectEnd)
        {
            this.hudManager = hudManager;
            this.OnClick = OnClick;
            this.OnEffectEnd = OnEffectEnd;
            this.PositionOffset = PositionOffset;
            this.EffectDuration = EffectDuration;
            this.category = category;
            pixelsPerUnit = PixelsPerUnit;
            MaxTimer = Cooldown;
            Timer = MaxTimer;
            ResourceName = ImageEmbededResourcePath;
            hasEffectDuration = true;
            isEffectActive = false;
            buttons.Add(this);
            Start();
        }

        public CooldownButton(Action OnClick, float Cooldown, string ImageEmbededResourcePath, float pixelsPerUnit, Vector2 PositionOffset, Category category, HudManager hudManager)
        {
            this.hudManager = hudManager;
            this.OnClick = OnClick;
            this.pixelsPerUnit = pixelsPerUnit;
            this.PositionOffset = PositionOffset;
            this.category = category;
            MaxTimer = Cooldown;
            Timer = MaxTimer;
            ResourceName = ImageEmbededResourcePath;
            hasEffectDuration = false;
            buttons.Add(this);
            Start();
        }

        private void Start()
        {
            killButtonManager = UnityEngine.Object.Instantiate(hudManager.KillButton, hudManager.transform);
            startColorButton = killButtonManager.renderer.color;
            startColorText = killButtonManager.TimerText.Color;
            killButtonManager.gameObject.SetActive(true);
            killButtonManager.renderer.enabled = true;
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            Assembly assembly = Assembly.GetExecutingAssembly();
            tex.LoadImage(assembly.GetManifestResourceStream(ResourceName).ReadFully(), false);
            killButtonManager.renderer.sprite = tex.CreateSprite();
            PassiveButton button = killButtonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)listener);
            void listener()
            {
                if (Timer < 0f && canUse)
                {
                    killButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
                    if (hasEffectDuration)
                    {
                        isEffectActive = true;
                        Timer = EffectDuration;
                        killButtonManager.TimerText.Color = new Color(0, 255, 0);
                    }
                    OnClick();
                }
            }
        }
        public void CanUse()
        {
            if (PlayerControl.LocalPlayer.Data == null) return;
            bool isImpostor = PlayerControl.LocalPlayer.Data.IsImpostor;
            switch (category)
            {
                case Category.Everyone:
                    {
                        canUse = true;
                        break;
                    }
                case Category.OnlyCrewmate:
                    {
                        canUse = !isImpostor;
                        break;
                    }
                case Category.OnlyImpostor:
                    {
                        canUse = isImpostor;
                        break;
                    }
            }
        }
        public static void HudUpdate()
        {
            buttons.RemoveAll(item => item.killButtonManager == null);
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].CanUse();
                buttons[i].Update();
            }
        }
        private void Update()
        {
            if (killButtonManager.transform.localPosition.x > 0f)
                killButtonManager.transform.localPosition = new Vector3((killButtonManager.transform.localPosition.x + 1.3f) * -1, killButtonManager.transform.localPosition.y, killButtonManager.transform.localPosition.z) + new Vector3(PositionOffset.x, PositionOffset.y);
            if (Timer < 0f)
            {
                killButtonManager.renderer.color = new Color(1f, 1f, 1f, 1f);
                if (isEffectActive)
                {
                    killButtonManager.TimerText.Color = startColorText;
                    Timer = MaxTimer;
                    isEffectActive = false;
                    OnEffectEnd();
                }
            }
            else
            {
                if (canUse && (isEffectActive || PlayerControl.LocalPlayer.CanMove))
                    Timer -= Time.deltaTime;
                    
                killButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
            }
            killButtonManager.gameObject.SetActive(canUse);
            killButtonManager.renderer.enabled = canUse;
            if (canUse)
            {
                killButtonManager.renderer.material.SetFloat("_Desat", 0f);
                killButtonManager.SetCoolDown(Timer, MaxTimer);
            }
        }
        public enum Category
        {
            Everyone,
            OnlyCrewmate,
            OnlyImpostor
        }
    }
}
