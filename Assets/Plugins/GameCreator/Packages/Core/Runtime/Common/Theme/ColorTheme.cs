using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Common
{
    public static class ColorTheme
    {
        public enum Type
        {
            Background,
            TextNormal,
            TextLight,
            White,
            Black,
            Light,
            Gray,
            Dark,
            Red,
            Green,
            Blue,
            Yellow,
            Purple,
            Pink,
            Teal,
        }

        // DATA CLASS: ----------------------------------------------------------------------------

        private class Colors
        {
            private readonly Color light;
            private readonly Color dark;

            public Color Color
            {
                get
                {
                    #if UNITY_EDITOR
                    return EditorGUIUtility.isProSkin ? this.dark : this.light;
                    #else
                    return this.light;
                    #endif
                }
            }

            public Colors(string hexLight, string hexDark)
            {
                this.light = this.Get(hexLight);
                this.dark = this.Get(hexDark);
            }

            private Color Get(string hex) => ColorUtility.TryParseHtmlString(hex, out Color color)
                ? color
                : default;
        }

        // PROPERTIES: ----------------------------------------------------------------------------

        private static readonly Dictionary<Type, Colors> Data = new Dictionary<Type, Colors>
        {
            { Type.Background, new Colors("#c2c2c2", "#383838") },
            { Type.TextNormal, new Colors("#090909", "#ffffff") },
            { Type.TextLight,  new Colors("#565656", "#AAAAAA") },
            { Type.White,      new Colors("#ffffff", "#ffffff") },
            { Type.Black,      new Colors("#000000", "#000000") },
            { Type.Light,      new Colors("#f0f0f0", "#f6f6f6") },
            { Type.Gray,       new Colors("#535353", "#c2c2c2") },
            { Type.Dark,       new Colors("#313131", "#1e1e1e") },
            { Type.Red,        new Colors("#9f251a", "#e9754c") },
            { Type.Green,      new Colors("#43793b", "#c2f771") },
            { Type.Blue,       new Colors("#2c6cc3", "#87d8f6") },
            { Type.Yellow,     new Colors("#c09431", "#f1c437") },
            { Type.Purple,     new Colors("#6040af", "#a692e9") },
            { Type.Pink,       new Colors("#bd377c", "#d790d4") },
            { Type.Teal,       new Colors("#347480", "#a2f7e4") },
        };

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static Color Get(Type type) => Data[type].Color;
    }
}