using NUnit.Framework;
using UnityEngine;

namespace Poke.UI.Tests
{
    public class UnityCallbackTests
    {
        [ExecuteAlways]
        private class ResizeListener : MonoBehaviour
        {
            public int Calls;

            private void OnRectTransformDimensionsChange() {
                Calls++;
            }
        }

        [Test]
        public void APlainBehaviourHearsItsOwnResizeAtOnce() {
            using Stage stage = new Stage();
            ResizeListener item = new GameObject("Item", typeof(RectTransform)).AddComponent<ResizeListener>();
            item.transform.SetParent(stage.Content, false);
            stage.Show();
            item.Calls = 0;

            ((RectTransform)item.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 321);

            Assert.That(item.Calls, Is.EqualTo(1));
        }
    }
}
