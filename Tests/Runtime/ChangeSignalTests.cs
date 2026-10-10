using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Poke.UI.Tests
{
    public class ChangeSignalTests
    {
        private const float Gap = 10;
        private const float Tolerance = 0.5f;

        private Stage _stage;
        private Layout _row;
        private LayoutItem _first;
        private LayoutItem _second;
        private LayoutText _third;

        [SetUp]
        public void SetUp() {
            _stage = new Stage();
            _row = Build.Layout(_stage.Content, "Row", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", Gap);
            _first = Build.Item(_row, "First").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(50, 20);
            _second = Build.Item(_row, "Second").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(70, 20);
            _third = Build.Text(_row, "Third", "Third");
            _stage.Show();
        }

        [TearDown]
        public void TearDown() {
            _stage.Dispose();
        }

        [UnityTest]
        public IEnumerator AChildMovedInIsLaidOutInItsFrame() {
            LayoutItem fourth = Build.Item(_stage.Content, "Fourth").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(30, 20);
            yield return Frame.Rendered(() => { });

            fourth.transform.SetParent(_row.transform, false);
            yield return Frame.Rendered(() => TheRowHolds(_first, _second, _third, fourth));
        }

        [UnityTest]
        public IEnumerator AChildMovedOutIsLaidOutInItsFrame() {
            _second.transform.SetParent(_stage.Content, false);
            yield return Frame.Rendered(() => TheRowHolds(_first, _third));
        }

        [UnityTest]
        public IEnumerator AReorderIsLaidOutInItsFrame() {
            _third.transform.SetSiblingIndex(0);
            yield return Frame.Rendered(() => TheRowHolds(_third, _first, _second));
        }

        [UnityTest]
        public IEnumerator ADisabledChildIsLaidOutInItsFrame() {
            _second.gameObject.SetActive(false);
            yield return Frame.Rendered(() => TheRowHolds(_first, _third));
        }

        [UnityTest]
        public IEnumerator AnEnabledChildIsLaidOutInItsFrame() {
            _second.gameObject.SetActive(false);
            yield return Frame.Rendered(() => { });

            _second.gameObject.SetActive(true);
            yield return Frame.Rendered(() => TheRowHolds(_first, _second, _third));
        }

        [UnityTest]
        public IEnumerator AResizedFixedChildIsLaidOutInItsFrame() {
            _first.Size(90, 20);
            yield return Frame.Rendered(() => TheRowHolds(_first, _second, _third));
        }

        [UnityTest]
        public IEnumerator AResizedRootIsLaidOutInItsFrame() {
            Layout bar = Build.Layout(_stage.Content, "Bar", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.Fixed)
                .Size(400, 40);
            Build.Item(bar, "Spacer").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 0);
            LayoutItem end = Build.Item(bar, "End").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(60, 20);
            yield return Frame.Rendered(() => { });

            bar.Size(600, 40);
            yield return Frame.Rendered(() =>
                Assert.That(_stage.Box(bar).xMax - _stage.Box(end).xMax, Is.EqualTo(0).Within(Tolerance)));
        }

        [Test]
        public void AnItemGrowingOutsideALayoutFollowsItsParentAtOnce() {
            RectTransform region = new GameObject("Region", typeof(RectTransform)).GetComponent<RectTransform>();
            region.SetParent(_stage.Content, false);
            region.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400);
            LayoutItem fill = Build.Item(region, "Fill").Sized(SizingMode.Grow, SizingMode.Fixed).Size(10, 20);
            Canvas.ForceUpdateCanvases();

            region.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);

            Assert.That(_stage.Box(fill).width, Is.EqualTo(500).Within(Tolerance));
            Assert.That(_stage.Box(fill).xMin, Is.EqualTo(_stage.Box(region).xMin).Within(Tolerance));
        }

        private void TheRowHolds(params Component[] children) {
            float width = 0;
            for(int i = 0; i < children.Length; i++) {
                Rect box = _stage.Box(children[i]);
                if(i > 0) Assert.That(box.xMin - _stage.Box(children[i - 1]).xMax, Is.EqualTo(Gap).Within(Tolerance), children[i].name);
                width += box.width + (i > 0 ? Gap : 0);
            }
            Assert.That(_stage.Box(_row).width, Is.EqualTo(width).Within(Tolerance), "row width");
        }
    }
}
