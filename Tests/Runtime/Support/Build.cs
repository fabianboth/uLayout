using System.Reflection;
using TMPro;
using UnityEngine;

namespace Poke.UI.Tests
{
    public static class Build
    {
        public static Layout Layout(Component parent, string name, Layout.LayoutDirection direction) {
            return New(parent, name).AddComponent<Layout>().Set("m_direction", direction);
        }

        public static LayoutText Text(Component parent, string name, string text) {
            GameObject go = New(parent, name);
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.fontSize = 24;
            tmp.textWrappingMode = TextWrappingModes.NoWrap;
            tmp.text = text;
            return go.AddComponent<LayoutText>();
        }

        public static LayoutItem Item(Component parent, string name) {
            return New(parent, name).AddComponent<LayoutItem>();
        }

        public static T Sized<T>(this T item, SizingMode x, SizingMode y) where T : LayoutItem {
            return item.Set("m_sizing", new LayoutItem.SizeModes { x = x, y = y });
        }

        public static T Size<T>(this T item, float width, float height) where T : Component {
            RectTransform rect = (RectTransform)item.transform;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            return item;
        }

        public static T Padded<T>(this T layout, float left, float right) where T : Layout {
            return layout.Set("m_padding", new Margins { left = left, right = right });
        }

        public static T Set<T>(this T target, string field, object value) where T : Component {
            FieldInfo info = null;
            for(System.Type type = target.GetType(); info == null && type != null; type = type.BaseType) {
                info = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }
            info.SetValue(target, value);
            return target;
        }

        public static TMP_Text Tmp(this LayoutText text) {
            return text.GetComponent<TMP_Text>();
        }

        private static GameObject New(Component parent, string name) {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent.transform, false);
            return go;
        }
    }
}
