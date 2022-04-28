using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProtoGUI
{
    public static class ProtoGUILayout
    {
        #region Properties

        public const float messageBoxIconSize = 30;

        public static readonly Color infoBoxColor = new Color(0.25f, 0.25f, 1.0f, 1.0f);
        
        public static readonly Color warningBoxColor = new Color(1.0f, 0.7f, 0.25f, 1.0f);
        
        public static readonly Color errorBoxColor = new Color(1.0f, 0.5f, 0.5f, 1.0f);

        public static readonly Sprite infoBoxSprite, warningBoxSprite, errorBoxSprite;

        #endregion

        #region Constructors
        static ProtoGUILayout()
        {
            infoBoxSprite = Resources.Load<Sprite>("protogui_infobox_icon");
            warningBoxSprite = Resources.Load<Sprite>("protogui_warningbox_icon");
            errorBoxSprite = Resources.Load<Sprite>("protogui_errorbox_icon");
        }

        #endregion
        
        #region Drawing Menus
        
        /// <summary>
        /// Draws a series of <see cref="ProtoMenuButton"/> objects vertically using the default GUI
        /// <see cref="GUI.skin"/> <c>box</c>. 
        /// </summary>
        /// <param name="buttons"></param>
        public static void DrawVerticalMenuOptions(params ProtoMenuButton[] buttons)
        {
            DrawVerticalMenuOptions(GUI.skin.box, buttons);
        }

        /// <summary>
        /// Draws a series of <see cref="ProtoMenuButton"/> objects vertically with the given <see cref="style"/>.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="buttons"></param>
        public static void DrawVerticalMenuOptions(GUIStyle style, params ProtoMenuButton[] buttons)
        {
            if (buttons == null)
            {
                return;
            }

            using (new GUILayout.VerticalScope(style))
            {
                var originalEnabledState = GUI.enabled;
                var originalColor = GUI.color;
                
                foreach (var button in buttons)
                {
                    GUI.enabled = button.enabled;
                    GUI.color = button.color ?? originalColor;

                    if (GUILayout.Button(button.text))
                    {
                        button.onClick?.Invoke();
                    }
                }

                GUI.enabled = originalEnabledState;
                GUI.color = originalColor;
            }
        }
        
        #endregion
        
        #region Containers
        
        /// <summary>
        /// Draws content surrounded by <see cref="GUILayout.FlexibleSpace"/> calls in both a horizontal and vertical
        /// scope. This is useful for creating centered content. 
        /// </summary>
        /// <param name="drawContent">The invokable action used to draw content within the horizontal and vertical flexed space.</param>
        public static void DrawFullyFlexibleContent(Action drawContent)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                using (new GUILayout.VerticalScope())
                {
                    GUILayout.FlexibleSpace();

                    drawContent?.Invoke();
                    
                    GUILayout.FlexibleSpace();
                }
                
                GUILayout.FlexibleSpace();
            }
        }

        /// <summary>
        /// Draws content surrounded by <see cref="GUILayout.FlexibleSpace"/> calls in both a horizontal and vertical
        /// scope. This is useful for creating centered content. 
        /// </summary>
        /// <param name="drawContent">The invokable action used to draw content within the horizontal and vertical flexed space.</param>
        /// <param name="horizontalBoxStyle"></param>
        /// <param name="verticalBoxStyle"></param>
        public static void DrawFullyFlexibleContent(Action drawContent, GUIStyle horizontalBoxStyle, GUIStyle verticalBoxStyle)
        {
            using (new GUILayout.HorizontalScope(horizontalBoxStyle))
            {
                GUILayout.FlexibleSpace();

                using (new GUILayout.VerticalScope(verticalBoxStyle))
                {
                    GUILayout.FlexibleSpace();

                    drawContent?.Invoke();
                    
                    GUILayout.FlexibleSpace();
                }
                
                GUILayout.FlexibleSpace();
            }
        }

        /// <summary>
        /// Draws content surrounded by <see cref="GUILayout.FlexibleSpace"/> calls in a horizontal scope.
        /// </summary>
        /// <param name="drawContent"></param>
        /// <param name="horizontalBoxStyle"></param>
        public static void DrawHorizontallyFlexibleContent(Action drawContent, GUIStyle horizontalBoxStyle = null)
        {
            using (new GUILayout.HorizontalScope(horizontalBoxStyle))
            {
                GUILayout.FlexibleSpace();

                drawContent?.Invoke();
                
                GUILayout.FlexibleSpace();
            }
        }

        /// <summary>
        /// Draws content surrounded by <see cref="GUILayout.FlexibleSpace"/> calls in a vertical scope.
        /// </summary>
        /// <param name="drawContent"></param>
        /// <param name="verticalBoxStyle"></param>
        public static void DrawVerticallyFlexibleContent(Action drawContent, GUIStyle verticalBoxStyle = null)
        {
            using (new GUILayout.VerticalScope())
            {
                GUILayout.FlexibleSpace();

                drawContent?.Invoke();
                
                GUILayout.FlexibleSpace();
            }
        }

        /// <summary>
        /// Draws a vertical-only scroll box, disabling horizontal scrolling.
        /// </summary>
        /// <param name="scrollPosition"></param>
        /// <param name="drawContent"></param>
        public static void DrawVerticalScrollArea(ref Vector2 scrollPosition, Action drawContent)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            drawContent?.Invoke();
            GUILayout.EndScrollView();
        }
        
        #endregion

        #region Special Containers

        /// <summary>
        /// Draws a content box with the given <see cref="icon"/> and <see cref="backgroundColor"/>.
        /// </summary>
        /// <param name="icon">The icon that appear on the left side of the box.</param>
        /// <param name="backgroundColor">The background color of the box.</param>
        /// <param name="drawContent">The action which draws additional content.</param>
        public static void DrawIconContentBox(Texture icon, Color backgroundColor, Action drawContent)
        {
            var originalGuiColor = GUI.color;
            GUI.backgroundColor = backgroundColor;
            
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = originalGuiColor;
                
                GUILayout.Space(15);

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(15);
                    
                    GUILayout.Label(
                        icon, 
                        GUILayout.Width(messageBoxIconSize), 
                        GUILayout.Height(messageBoxIconSize));

                    GUILayout.Space(15);

                    using (new GUILayout.VerticalScope())
                    {
                        drawContent?.Invoke();
                    }
                    
                    GUILayout.Space(15); 
                }

                GUILayout.Space(15); 
            }
        }
        
        /// <summary>
        /// Draws an "info box" with <see cref="infoBoxColor"/> and <see cref="infoBoxSprite"/>.
        /// </summary>
        /// <param name="drawContent">The action which draws additional content.</param>
        public static void DrawInfoBox(Action drawContent)
        {
            DrawIconContentBox(infoBoxSprite?.texture, infoBoxColor, drawContent);
        }
        
        /// <summary>
        /// Draws an "info box" with <see cref="warningBoxColor"/> and <see cref="warningBoxSprite"/>.
        /// </summary>
        /// <param name="drawContent">The action which draws additional content.</param>
        public static void DrawWarningBox(Action drawContent)
        {
            DrawIconContentBox(warningBoxSprite?.texture, warningBoxColor, drawContent);
        }
        
        /// <summary>
        /// Draws an "error box" with <see cref="errorBoxColor"/> and <see cref="errorBoxSprite"/>.
        /// </summary>
        /// <param name="drawContent">The action which draws additional content.</param>
        public static void DrawErrorBox(Action drawContent)
        {
            DrawIconContentBox(errorBoxSprite?.texture, errorBoxColor, drawContent);
        }

        #endregion
        
        #region Sliders

        /// <summary>
        /// Draws a horizontal slider with a label.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">Current value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="labelWidth">Width of the label defaults to 150.</param>
        /// <returns></returns>
        public static float DrawHorizontalSliderField(string label, float value, float min, float max, float labelWidth = 150)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(labelWidth));
                GUILayout.Space(15);

                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Space(9);
                    value = GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true));
                    GUILayout.Space(9);
                }
            }

            return value;
        }

        /// <summary>
        /// Draws a fancy horizontal slider with a label, min text, max text, and value text.
        /// </summary>
        /// <param name="label">The label to display.</param>
        /// <param name="value">Current value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="showMinText">Shows the "min" text to the left of the slider. Defaults to true.</param>
        /// <param name="showMaxText">Shows the "max" text to the right of the slider. Defaults to true.</param>
        /// <param name="showValueText">Shows the value in the label. Defaults to true.</param>
        /// <param name="valueFormatting">The string format to use for the min, max, and value when printing the string.
        /// Defaults to "0.000"</param>
        /// <param name="labelWidth">Width of the label defaults to 150.</param>
        /// <returns>The changed value.</returns>
        public static float DrawFancyHorizontalSlider(string label, float value, float min, float max, bool showValueText = true, string valueFormatting = "0.000", bool showMinText = true, bool showMaxText = true, float labelWidth = 150)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label($"{label} {(showValueText ? $"({value.ToString(valueFormatting)})" : "")}", GUILayout.Width(labelWidth));
                GUILayout.Space(15);

                if (showMinText)
                {
                    GUILayout.Label(min.ToString(valueFormatting), GUILayout.ExpandWidth(false), GUILayout.Width(50));
                    GUILayout.Space(5);
                }

                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Space(9);
                    value = GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true));
                    GUILayout.Space(9);
                }

                if (showMaxText)
                {
                    GUILayout.Space(5);
                    GUILayout.Label(max.ToString(valueFormatting), GUILayout.ExpandWidth(false), GUILayout.Width(50));
                }
            }

            return value;
        }
        
        #endregion

        #region Labels
        
        /// <summary>
        /// Draws a label that's been completely wrapped by <see cref="GUILayout.FlexibleSpace"/>.
        /// </summary>
        /// <param name="label">The string label to use.</param>
        public static void DrawFullyFlexibleLabel(string label)
        {
            DrawFullyFlexibleContent(() => GUILayout.Label(label));
        }

        /// <summary>
        /// Draws a label that's been completely wrapped by <see cref="GUILayout.FlexibleSpace"/>.
        /// </summary>
        /// <param name="guiContent">The GUI Content to use.</param>
        public static void DrawFullyFlexibleLabel(GUIContent guiContent)
        {
            DrawFullyFlexibleContent(() => GUILayout.Label(guiContent));
        }

        #endregion
        
        #region Enum Fields

        /// <summary>
        /// Draws a "left" and "right" button to cycle through a series of enumeration values. The given type <see cref="T"/>
        /// must be an enumeration.
        /// </summary>
        /// <param name="value">The curren value.</param>
        /// <param name="previousText">The text for the button that selects the "previous" value. The default value is
        /// a less than sign.</param>
        /// <param name="nextText">The text for the button that selects the "next" value. The default value is a greater
        /// than sign.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The updated value.</returns>
        /// <exception cref="ArgumentException">Thrown when <see cref="T"/> is not an enumeration.</exception>
        public static T DrawHorizontalEnumSelector<T>(T value, string previousText = "<", string nextText = ">") where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var allValues = Enum.GetValues(typeof(T)) as T[];

            if (allValues == null)
            {
                throw new Exception($"Failed to get values for given type {typeof(T).Name}");
            }

            var names = Enum.GetNames(typeof(T));
            var indexOfSelection = GetIndexOfSelection(value, allValues);

            if (indexOfSelection <= -1 || indexOfSelection >= allValues.Length)
            {
                indexOfSelection = 0;
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button(previousText, GUILayout.ExpandWidth(false)))
                {
                    indexOfSelection--;

                    if (indexOfSelection <= -1)
                    {
                        indexOfSelection = allValues.Length - 1;
                    }
                }

                GUILayout.FlexibleSpace();
                
                GUILayout.Label(names[indexOfSelection]);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(nextText, GUILayout.ExpandWidth(false)))
                {
                    indexOfSelection++;

                    if (indexOfSelection >= allValues.Length)
                    {
                        indexOfSelection = 0;
                    }
                }
            }

            return allValues[indexOfSelection];
        }

        /// <summary>
        /// Draws a "left" and "right" button to cycle through a series of enumeration values. The given type <see cref="T"/>
        /// must be an enumeration.
        /// </summary>
        /// <param name="label">A label to display in front of the field.</param>
        /// <param name="value">The curren value.</param>
        /// <param name="previousText">The text for the button that selects the "previous" value. The default value is
        /// a less than sign.</param>
        /// <param name="nextText">The text for the button that selects the "next" value. The default value is a greater
        /// than sign.</param>
        /// <param name="labelWidth">The width of the label. The default value is 150.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The updated value.</returns>
        /// <exception cref="ArgumentException">Thrown when <see cref="T"/> is not an enumeration.</exception>
        public static T DrawHorizontalEnumSelector<T>(string label, T value, string previousText = "<", string nextText = ">", float labelWidth = 150) where T : Enum
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(labelWidth), GUILayout.ExpandWidth(false));
                GUILayout.Space(15);
                
                value = DrawHorizontalEnumSelector(value, previousText, nextText);
            }

            return value;
        }

        /// <summary>
        /// Draws a selection grid with the values of the given enumeration type. 
        /// </summary>
        /// <param name="value">The currently selected enumeration value.</param>
        /// <param name="maxRowCount">The maximum number of values that can appear in one row.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The updated value.</returns>
        /// <exception cref="ArgumentException">Thrown when the given type <see cref="T"/> isn't an enumeration.</exception>
        /// <exception cref="Exception">Thrown when the given type <see cref="T"/> has invalid values.</exception>
        public static T DrawHorizontalEnumSelectionGrid<T>(T value, int maxRowCount = -1) where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var allValues = Enum.GetValues(typeof(T)) as T[];

            if (allValues == null)
            {
                throw new Exception($"Failed to get values for given type {typeof(T).Name}");
            }

            if (maxRowCount <= 0)
            {
                maxRowCount = allValues.Length;
            }
            
            var indexOfSelection = GetIndexOfSelection(value, allValues);

            if (indexOfSelection <= -1 || indexOfSelection >= allValues.Length)
            {
                indexOfSelection = 0;
            }

            indexOfSelection = GUILayout.SelectionGrid(indexOfSelection, Enum.GetNames(typeof(T)), maxRowCount);

            return allValues[indexOfSelection];
        }

        /// <summary>
        /// Draws a selection grid with the values of the given enumeration type. 
        /// </summary>
        /// <param name="label">A label to display in front of the field.</param>
        /// <param name="value">The currently selected enumeration value.</param>
        /// <param name="maxRowCount">The maximum number of values that can appear in one row.</param>
        /// <param name="labelWidth">The width of the label. The default value is 150.</param>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <returns>The updated value.</returns>
        /// <exception cref="ArgumentException">Thrown when the given type <see cref="T"/> isn't an enumeration.</exception>
        /// <exception cref="Exception">Thrown when the given type <see cref="T"/> has invalid values.</exception>
        public static T DrawHorizontalEnumSelectionGrid<T>(string label, T value, int maxRowCount = -1, float labelWidth = 150) where T : Enum
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(label, GUILayout.Width(labelWidth), GUILayout.ExpandWidth(false));
                GUILayout.Space(15);
                
                value = DrawHorizontalEnumSelectionGrid(value, maxRowCount);
            }

            return value;
        }
        
        #endregion

        #region Text Fields

        public static string DrawTextField(string label, string value, float labelWidth = 150)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(labelWidth));
            value = GUILayout.TextField(value, GUILayout.ExpandWidth(true)); 
            GUILayout.EndHorizontal();
            return value;
        }

        #endregion

        #region Private Utilities 
        
        private static int GetIndexOfSelection<T>(T value, T[] values) where T : Enum
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(value, values[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        private static Texture2D CreateBackgroundTexture(Color color)
        {
            var result = new Texture2D(2, 2);

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    result.SetPixel(x, y, color);
                }
            }

            return result;
        }
        
        #endregion
    }

    public class ProtoMenuButton
    {
        public string text = "Button";
        public Color? color;
        public Action onClick;
        public bool enabled = true;
    }
}