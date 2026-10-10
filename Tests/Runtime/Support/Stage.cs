using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poke.UI.Tests
{
    public sealed class Stage : IDisposable
    {
        private readonly GameObject _canvas;

        public RectTransform Content { get; }

        public Stage() {
            _canvas = new GameObject("Stage", typeof(RectTransform), typeof(Canvas));
            _canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            Content = new GameObject("Content", typeof(RectTransform)).GetComponent<RectTransform>();
            Content.gameObject.SetActive(false);
            Content.SetParent(_canvas.transform, false);
            Content.anchorMin = Vector2.zero;
            Content.anchorMax = Vector2.one;
            Content.sizeDelta = Vector2.zero;
        }

        public void Show() {
            Content.gameObject.SetActive(true);
            Canvas.ForceUpdateCanvases();
        }

        public Rect Box(Component item) {
            Vector3[] corners = new Vector3[4];
            ((RectTransform)item.transform).GetWorldCorners(corners);
            Vector2 min = Content.InverseTransformPoint(corners[0]);
            Vector2 max = Content.InverseTransformPoint(corners[2]);
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        public void Dispose() {
            Object.Destroy(_canvas);
        }
    }
}
