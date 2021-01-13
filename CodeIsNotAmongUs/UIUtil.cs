using System;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace VentingCrew
{
    class UIUtil
    {
        internal static Vector2 thickSize = new Vector2(160f, 30f);
        internal static Vector2 thinSize = new Vector2(160f, 20f);
        internal static Color defaultTextColor = new Color(0.95f, 0.95f, 0.95f, 1f);
        internal static Font s_defaultFont;

        public static GameObject CreateUIObject(string name, GameObject parent, Vector2 size = default)
        {
            GameObject obj = new GameObject(name);

            RectTransform rect = obj.AddComponent<RectTransform>();
            if (size != default)
            {
                rect.sizeDelta = size;
            }

            SetParentAndAlign(obj, parent);

            return obj;
        }

        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
            {
                return;
            }
            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child);
        }

        public static void SetLayerRecursively(GameObject go)
        {
            go.layer = 5;
            Transform transform = go.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                SetLayerRecursively(transform.GetChild(i).gameObject);
            }
        }

        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = defaultTextColor;

            if (!s_defaultFont)
                s_defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

            if (s_defaultFont)
                lbl.font = s_defaultFont;
        }

        public static void SetDefaultColorTransitionValues(Selectable selectable)
        {
            ColorBlock colors = selectable.colors;
            colors.normalColor = new Color(0.35f, 0.35f, 0.35f);
            colors.highlightedColor = new Color(0.45f, 0.45f, 0.45f);
            colors.pressedColor = new Color(0.25f, 0.25f, 0.25f);

            if (selectable is Button button)
            {
                button.onClick.AddListener((UnityEngine.Events.UnityAction)Deselect);
                void Deselect()
                {
                    button.OnDeselect(null);
                }

            }

            selectable.colors = colors;
        }

        public static GameObject CreateGridGroup(GameObject parent, Vector2 cellSize, Vector2 spacing, Color color = default)
        {
            GameObject groupObj = CreateUIObject("GridLayout", parent);

            GridLayoutGroup gridGroup = groupObj.AddComponent<GridLayoutGroup>();
            gridGroup.childAlignment = TextAnchor.UpperLeft;
            gridGroup.cellSize = cellSize;
            gridGroup.spacing = spacing;

            Image image = groupObj.AddComponent<Image>();
            if (color != default)
            {
                image.color = color;
            }
            else
            {
                image.color = new Color(44f / 255f, 44f / 255f, 44f / 255f);
            }

            return groupObj;
        }

        public static GameObject CreateVerticalGroup(GameObject parent, Color color = default)
        {
            GameObject groupObj = CreateUIObject("VerticalLayout", parent);

            VerticalLayoutGroup horiGroup = groupObj.AddComponent<VerticalLayoutGroup>();
            horiGroup.childAlignment = TextAnchor.UpperLeft;
            horiGroup.childControlWidth = true;
            horiGroup.childControlHeight = true;

            Image image = groupObj.AddComponent<Image>();
            if (color != default)
            {
                image.color = color;
            }
            else
            {
                image.color = new Color(44f / 255f, 44f / 255f, 44f / 255f);
            }

            return groupObj;
        }

        public static GameObject CreateHorizontalGroup(GameObject parent, Color color = default)
        {
            GameObject groupObj = CreateUIObject("HorizontalLayout", parent);

            HorizontalLayoutGroup horiGroup = groupObj.AddComponent<HorizontalLayoutGroup>();
            horiGroup.childAlignment = TextAnchor.UpperLeft;
            horiGroup.childControlWidth = true;
            horiGroup.childControlHeight = true;

            Image image = groupObj.AddComponent<Image>();
            if (color != default)
                image.color = color;
            else
                image.color = new Color(44f / 255f, 44f / 255f, 44f / 255f);

            return groupObj;
        }

        public static GameObject CreateLabel(GameObject parent, TextAnchor alignment)
        {
            GameObject labelObj = CreateUIObject("Label", parent, thinSize);

            Text text = labelObj.AddComponent<Text>();
            SetDefaultTextValues(text);
            text.alignment = alignment;
            text.supportRichText = true;

            return labelObj;
        }

        public static GameObject CreateButton(GameObject parent, Color normalColor = default)
        {
            GameObject buttonObj = CreateUIObject("Button", parent, thinSize);

            GameObject textObj = new GameObject("Text");
            textObj.AddComponent<RectTransform>();
            SetParentAndAlign(textObj, buttonObj);

            Image image = buttonObj.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(1, 1, 1, 0.75f);

            SetDefaultColorTransitionValues(buttonObj.AddComponent<Button>());

            if (normalColor != default)
            {
                var btn = buttonObj.GetComponent<Button>();
                var colors = btn.colors;
                colors.normalColor = normalColor;
                btn.colors = colors;
            }

            Text text = textObj.AddComponent<Text>();
            text.text = "Button";
            SetDefaultTextValues(text);
            text.alignment = TextAnchor.MiddleCenter;

            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            return buttonObj;
        }

        public static GameObject CreateSlider(GameObject parent)
        {
            GameObject sliderObj = CreateUIObject("Slider", parent, thinSize);

            GameObject bgObj = CreateUIObject("Background", sliderObj);
            GameObject fillAreaObj = CreateUIObject("Fill Area", sliderObj);
            GameObject fillObj = CreateUIObject("Fill", fillAreaObj);
            GameObject handleSlideAreaObj = CreateUIObject("Handle Slide Area", sliderObj);
            GameObject handleObj = CreateUIObject("Handle", handleSlideAreaObj);

            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.type = Image.Type.Sliced;
            bgImage.color = new Color(0.15f, 0.15f, 0.15f, 1.0f);

            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 0.25f);
            bgRect.anchorMax = new Vector2(1f, 0.75f);
            bgRect.sizeDelta = new Vector2(0f, 0f);

            RectTransform fillAreaRect = fillAreaObj.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1f, 0.75f);
            fillAreaRect.anchoredPosition = new Vector2(-5f, 0f);
            fillAreaRect.sizeDelta = new Vector2(-20f, 0f);

            Image fillImage = fillObj.AddComponent<Image>();
            fillImage.type = Image.Type.Sliced;
            fillImage.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);

            fillObj.GetComponent<RectTransform>().sizeDelta = new Vector2(10f, 0f);

            RectTransform handleSlideRect = handleSlideAreaObj.GetComponent<RectTransform>();
            handleSlideRect.sizeDelta = new Vector2(-20f, 0f);
            handleSlideRect.anchorMin = new Vector2(0f, 0f);
            handleSlideRect.anchorMax = new Vector2(1f, 1f);

            Image handleImage = handleObj.AddComponent<Image>();
            handleImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            handleObj.GetComponent<RectTransform>().sizeDelta = new Vector2(20f, 0f);

            Slider slider = sliderObj.AddComponent<Slider>();
            slider.fillRect = fillObj.GetComponent<RectTransform>();
            slider.handleRect = handleObj.GetComponent<RectTransform>();
            slider.targetGraphic = handleImage;
            slider.direction = Slider.Direction.LeftToRight;
            SetDefaultColorTransitionValues(slider);

            return sliderObj;
        }

        public static GameObject CreateScrollbar(GameObject parent)
        {
            GameObject scrollObj = CreateUIObject("Scrollbar", parent, thinSize);

            GameObject slideAreaObj = CreateUIObject("Sliding Area", scrollObj);
            GameObject handleObj = CreateUIObject("Handle", slideAreaObj);

            Image scrollImage = scrollObj.AddComponent<Image>();
            scrollImage.type = Image.Type.Sliced;
            scrollImage.color = new Color(0.1f, 0.1f, 0.1f);

            Image handleImage = handleObj.AddComponent<Image>();
            handleImage.type = Image.Type.Sliced;
            handleImage.color = new Color(0.4f, 0.4f, 0.4f);

            RectTransform slideAreaRect = slideAreaObj.GetComponent<RectTransform>();
            slideAreaRect.sizeDelta = new Vector2(-20f, -20f);
            slideAreaRect.anchorMin = Vector2.zero;
            slideAreaRect.anchorMax = Vector2.one;

            RectTransform handleRect = handleObj.GetComponent<RectTransform>();
            handleRect.sizeDelta = new Vector2(20f, 20f);

            Scrollbar scrollbar = scrollObj.AddComponent<Scrollbar>();
            scrollbar.handleRect = handleRect;
            scrollbar.targetGraphic = handleImage;
            SetDefaultColorTransitionValues(scrollbar);

            return scrollObj;
        }

        public static GameObject CreateToggle(GameObject parent, out Toggle toggle, out Text text, Color bgColor = default)
        {
            GameObject toggleObj = CreateUIObject("Toggle", parent, thinSize);

            GameObject bgObj = CreateUIObject("Background", toggleObj);
            GameObject checkObj = CreateUIObject("Checkmark", bgObj);
            GameObject labelObj = CreateUIObject("Label", toggleObj);

            toggle = toggleObj.AddComponent<Toggle>();
            toggle.isOn = true;
            Toggle toggleComp = toggle;

            toggle.onValueChanged.AddListener((UnityEngine.Events.UnityAction<bool>)Deselect);
            void Deselect(bool _)
            {
                toggleComp.OnDeselect(null);
            }

            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = bgColor == default
                ? new Color(0.2f, 0.2f, 0.2f, 1.0f)
                : bgColor;

            Image checkImage = checkObj.AddComponent<Image>();
            checkImage.color = new Color(0.3f, 0.5f, 0.3f, 1.0f);

            text = labelObj.AddComponent<Text>();
            text.text = "Toggle";
            SetDefaultTextValues(text);

            toggle.graphic = checkImage;
            toggle.targetGraphic = bgImage;
            SetDefaultColorTransitionValues(toggle);

            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0f, 1f);
            bgRect.anchorMax = new Vector2(0f, 1f);
            bgRect.anchoredPosition = new Vector2(13f, -13f);
            bgRect.sizeDelta = new Vector2(20f, 20f);

            RectTransform checkRect = checkObj.GetComponent<RectTransform>();
            checkRect.anchorMin = new Vector2(0.5f, 0.5f);
            checkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkRect.anchoredPosition = Vector2.zero;
            checkRect.sizeDelta = new Vector2(14f, 14f);

            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(1f, 1f);
            labelRect.offsetMin = new Vector2(28f, 2f);
            labelRect.offsetMax = new Vector2(-5f, -5f);
            return toggleObj;
        }

        public static GameObject CreateInputField(GameObject parent, int fontSize = 14, int alignment = 3, int wrap = 0)
        {
            GameObject mainObj = CreateUIObject("InputField", parent);

            Image mainImage = mainObj.AddComponent<Image>();
            mainImage.type = Image.Type.Sliced;
            mainImage.color = new Color(0.15f, 0.15f, 0.15f);

            InputField mainInput = mainObj.AddComponent<InputField>();
            Navigation nav = mainInput.navigation;
            nav.mode = Navigation.Mode.None;
            mainInput.navigation = nav;
            mainInput.lineType = InputField.LineType.SingleLine;
            mainInput.interactable = true;
            mainInput.transition = Selectable.Transition.ColorTint;
            mainInput.targetGraphic = mainImage;

            ColorBlock mainColors = mainInput.colors;
            mainColors.normalColor = new Color(1, 1, 1, 1);
            mainColors.highlightedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f);
            mainColors.pressedColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1.0f);
            mainColors.highlightedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f);
            mainInput.colors = mainColors;

            VerticalLayoutGroup mainGroup = mainObj.AddComponent<VerticalLayoutGroup>();
            mainGroup.childControlHeight = true;
            mainGroup.childControlWidth = true;
            mainGroup.childForceExpandWidth = true;
            mainGroup.childForceExpandHeight = true;

            GameObject textArea = CreateUIObject("TextArea", mainObj);
            textArea.AddComponent<RectMask2D>();

            RectTransform textAreaRect = textArea.GetComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = Vector2.zero;
            textAreaRect.offsetMax = Vector2.zero;

            // mainInput.textViewport = textArea.GetComponent<RectTransform>();

            GameObject placeHolderObj = CreateUIObject("Placeholder", textArea);
            Text placeholderText = placeHolderObj.AddComponent<Text>();
            SetDefaultTextValues(placeholderText);
            placeholderText.text = "...";
            placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            placeholderText.horizontalOverflow = (HorizontalWrapMode)wrap;
            placeholderText.alignment = (TextAnchor)alignment;
            placeholderText.fontSize = fontSize;

            RectTransform placeHolderRect = placeHolderObj.GetComponent<RectTransform>();
            placeHolderRect.anchorMin = Vector2.zero;
            placeHolderRect.anchorMax = Vector2.one;
            placeHolderRect.offsetMin = Vector2.zero;
            placeHolderRect.offsetMax = Vector2.zero;

            LayoutElement placeholderLayout = placeHolderObj.AddComponent<LayoutElement>();
            placeholderLayout.minWidth = 500;
            placeholderLayout.flexibleWidth = 5000;

            mainInput.placeholder = placeholderText;

            GameObject inputTextObj = CreateUIObject("Text", textArea);
            Text inputText = inputTextObj.AddComponent<Text>();
            SetDefaultTextValues(inputText);
            inputText.text = "";
            inputText.color = new Color(1f, 1f, 1f, 1f);
            inputText.horizontalOverflow = (HorizontalWrapMode)wrap;
            inputText.alignment = (TextAnchor)alignment;
            inputText.fontSize = fontSize;

            RectTransform inputTextRect = inputTextObj.GetComponent<RectTransform>();
            inputTextRect.anchorMin = Vector2.zero;
            inputTextRect.anchorMax = Vector2.one;
            inputTextRect.offsetMin = Vector2.zero;
            inputTextRect.offsetMax = Vector2.zero;

            LayoutElement inputTextLayout = inputTextObj.AddComponent<LayoutElement>();
            inputTextLayout.minWidth = 500;
            inputTextLayout.flexibleWidth = 5000;

            mainInput.textComponent = inputText;

            return mainObj;
        }
    }
}
