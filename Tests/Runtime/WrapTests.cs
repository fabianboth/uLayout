using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Poke.UI.Tests
{
    public class WrapTests
    {
        private const float Gap = 10;
        private const float Tolerance = 0.5f;

        private Stage _stage;
        private Layout _strip;
        private Layout _cell;
        private LayoutItem _first;
        private LayoutItem _second;

        [SetUp]
        public void SetUp() {
            _stage = new Stage();
            _strip = Build.Layout(_stage.Content, "Strip", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.Fixed)
                .Size(600, 200);
            _cell = Build.Layout(_strip, "Cell", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Grow, SizingMode.FitContent)
                .Set("m_wrap", true)
                .Set("m_innerSpacing", Gap)
                .Set("m_lineSpacing", Gap);
            _first = Build.Item(_cell, "First").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(200, 40);
            _second = Build.Item(_cell, "Second").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(200, 40);
            Build.Item(_strip, "Centre").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(100, 40);
            Build.Item(_strip, "Other").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 40);
        }

        [TearDown]
        public void TearDown() {
            _stage.Dispose();
        }

        [Test]
        public void ARowInAGrowingCellWrapsAtTheCellsWidth() {
            _stage.Show();

            Assert.That(_stage.Box(_cell).width, Is.EqualTo(250).Within(Tolerance));
            TheSecondIsBelowTheFirst();
        }

        [UnityTest]
        public IEnumerator ARowInAGrowingCellUnwrapsWhenTheCellWidens() {
            _stage.Show();

            _strip.Size(1200, 200);
            yield return Frame.Rendered(TheSecondFollowsTheFirst);
        }

        [UnityTest]
        public IEnumerator ARowInAGrowingCellWrapsWhenTheCellNarrows() {
            _strip.Size(1200, 200);
            _stage.Show();
            yield return Frame.Rendered(() => { });

            _strip.Size(600, 200);
            yield return Frame.Rendered(TheSecondIsBelowTheFirst);
        }

        [Test]
        public void AColumnInAGrowingCellWrapsAtTheCellsHeight() {
            Layout strip = Build.Layout(_stage.Content, "Tall", Layout.LayoutDirection.Column)
                .Sized(SizingMode.Fixed, SizingMode.Fixed)
                .Size(200, 300);
            Layout cell = Build.Layout(strip, "Cell", Layout.LayoutDirection.Column)
                .Sized(SizingMode.FitContent, SizingMode.Grow)
                .Set("m_wrap", true)
                .Set("m_innerSpacing", Gap)
                .Set("m_lineSpacing", Gap);
            LayoutItem first = Build.Item(cell, "First").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(40, 60);
            LayoutItem second = Build.Item(cell, "Second").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(40, 60);
            Build.Item(strip, "Centre").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(40, 100);
            Build.Item(strip, "Other").Sized(SizingMode.Fixed, SizingMode.Grow).Size(40, 0);

            _stage.Show();

            Assert.That(_stage.Box(cell).height, Is.EqualTo(100).Within(Tolerance));
            Assert.That(_stage.Box(second).yMax, Is.EqualTo(_stage.Box(first).yMax).Within(Tolerance), "the second starts its own column");
            Assert.That(_stage.Box(second).xMin - _stage.Box(first).xMax, Is.EqualTo(Gap).Within(Tolerance), "one line spacing beside");
        }

        private void TheSecondIsBelowTheFirst() {
            Rect first = _stage.Box(_first);
            Rect second = _stage.Box(_second);
            Assert.That(second.xMin, Is.EqualTo(first.xMin).Within(Tolerance), "the second starts its own line");
            Assert.That(first.yMin - second.yMax, Is.EqualTo(Gap).Within(Tolerance), "one line spacing below");
            Assert.That(_stage.Box(_cell).height, Is.EqualTo(40 + Gap + 40).Within(Tolerance), "the cell holds both lines");
        }

        private void TheSecondFollowsTheFirst() {
            Rect first = _stage.Box(_first);
            Rect second = _stage.Box(_second);
            Assert.That(second.xMin - first.xMax, Is.EqualTo(Gap).Within(Tolerance), "on one line");
            Assert.That(second.yMin, Is.EqualTo(first.yMin).Within(Tolerance));
        }
    }
}
