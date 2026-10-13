using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace Poke.UI.Tests
{
    public class RegressionTests
    {
        private const float Tolerance = 0.5f;

        private Stage _stage;

        [SetUp]
        public void SetUp() {
            _stage = new Stage();
        }

        [TearDown]
        public void TearDown() {
            _stage.Dispose();
        }

        [Test]
        public void ALayoutWithNothingAboveItLogsNoError() {
            GameObject root = new GameObject("Root", typeof(RectTransform));
            root.AddComponent<Layout>();
            Object.Destroy(root);

            LogAssert.NoUnexpectedReceived();
        }

        [TestCase("Short")]
        [TestCase("A considerably longer heading than the first")]
        public void TheHintFollowsTheTitleWhateverItsLength(string heading) {
            Layout header = Build.Layout(_stage.Content, "Header", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", 24f);
            LayoutText title = Build.Text(header, "Title", heading);
            LayoutText hint = Build.Text(header, "Hint", "Back");

            _stage.Show();

            Assert.That(_stage.Box(title).width, Is.EqualTo(title.Tmp().GetPreferredValues().x).Within(Tolerance));
            Assert.That(_stage.Box(hint).xMin - _stage.Box(title).xMax, Is.EqualTo(24).Within(Tolerance));
        }

        [Test]
        public void AGrowSpacerPushesTheLastItemToTheEnd() {
            Layout row = Build.Layout(_stage.Content, "Row", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.FitContent)
                .Padded(20, 20)
                .Size(800, 40);
            LayoutText title = Build.Text(row, "Title", "Heading");
            Build.Item(row, "Spacer").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 0);
            LayoutText end = Build.Text(row, "End", "End");

            _stage.Show();

            Assert.That(_stage.Box(title).xMin - _stage.Box(row).xMin, Is.EqualTo(20).Within(Tolerance));
            Assert.That(_stage.Box(row).xMax - _stage.Box(end).xMax, Is.EqualTo(20).Within(Tolerance));
        }

        [TestCase("OK")]
        [TestCase("A longer caption")]
        public void AButtonKeepsItsMinimumWidthAndGrowsPastIt(string caption) {
            Layout button = Build.Layout(_stage.Content, "Button", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.Fixed)
                .Padded(28, 28)
                .Set("m_useMinWidth", true)
                .Set("m_minWidth", 120f)
                .Size(0, 48);
            LayoutText label = Build.Text(button, "Label", caption);

            _stage.Show();

            float expected = Mathf.Max(120, label.Tmp().GetPreferredValues().x + 56);
            Assert.That(_stage.Box(button).width, Is.EqualTo(expected).Within(Tolerance));
        }

        [Test]
        public void AWrappedTextTakesTheHeightOfItsLines() {
            Layout column = Build.Layout(_stage.Content, "Column", Layout.LayoutDirection.Column)
                .Sized(SizingMode.FitContent, SizingMode.FitContent);
            LayoutText description = Build.Text(column, "Description", "The quick brown fox jumps over the lazy dog and keeps on running far away.")
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_useMaxWidth", true)
                .Set("m_maxWidth", 200f);
            description.Tmp().textWrappingMode = TextWrappingModes.Normal;

            _stage.Show();

            Rect box = _stage.Box(description);
            Assert.That(box.width, Is.EqualTo(200).Within(Tolerance));
            Assert.That(box.height, Is.EqualTo(description.Tmp().GetPreferredValues(description.Tmp().text, 200, float.PositiveInfinity).y).Within(Tolerance));
            Assert.That(box.height, Is.GreaterThan(2 * description.Tmp().GetPreferredValues("A", 200, float.PositiveInfinity).y));
        }
    }
}
