using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine.TestTools;

namespace Poke.UI.Tests
{
    public class SizeSetterTests
    {
        private const float Tolerance = 0.5f;
        private const string LongText = "The quick brown fox jumps over the lazy dog and keeps on running far away.";

        private Stage _stage;

        [SetUp]
        public void SetUp() {
            _stage = new Stage();
        }

        [TearDown]
        public void TearDown() {
            _stage.Dispose();
        }

        [UnityTest]
        public IEnumerator ANewMinimumWidthIsLaidOutInItsFrame() {
            Layout button = Button();
            _stage.Show();

            button.MinWidth = 240;
            yield return Frame.Rendered(() => Assert.That(_stage.Box(button).width, Is.EqualTo(240).Within(Tolerance)));
        }

        [UnityTest]
        public IEnumerator ANewMinimumHeightIsLaidOutInItsFrame() {
            Layout button = Button().Set("m_useMinHeight", true);
            _stage.Show();

            button.MinHeight = 80;
            yield return Frame.Rendered(() => Assert.That(_stage.Box(button).height, Is.EqualTo(80).Within(Tolerance)));
        }

        [UnityTest]
        public IEnumerator ANewMaximumWidthIsLaidOutInItsFrame() {
            LayoutText text = Wrapped().Set("m_useMaxWidth", true).Set("m_maxWidth", 200f);
            _stage.Show();

            text.MaxWidth = 120;
            yield return Frame.Rendered(() => Assert.That(_stage.Box(text).width, Is.EqualTo(120).Within(Tolerance)));
        }

        [UnityTest]
        public IEnumerator ANewMaximumHeightIsLaidOutInItsFrame() {
            LayoutText text = Wrapped()
                .Set("m_useMaxWidth", true).Set("m_maxWidth", 200f)
                .Set("m_useMaxHeight", true).Set("m_maxHeight", 400f);
            _stage.Show();

            text.MaxHeight = 40;
            yield return Frame.Rendered(() => Assert.That(_stage.Box(text).height, Is.EqualTo(40).Within(Tolerance)));
        }

        [UnityTest]
        public IEnumerator ANewSizingIsLaidOutInItsFrame() {
            Layout row = Build.Layout(_stage.Content, "Row", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.FitContent)
                .Size(400, 0);
            LayoutItem item = Build.Item(row, "Item").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(50, 20);
            _stage.Show();

            item.Sizing = new LayoutItem.SizeModes { x = SizingMode.Grow, y = SizingMode.Fixed };
            yield return Frame.Rendered(() => Assert.That(_stage.Box(item).width, Is.EqualTo(400).Within(Tolerance)));
        }

        private Layout Button() {
            Layout button = Build.Layout(_stage.Content, "Button", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Padded(28, 28)
                .Set("m_useMinWidth", true)
                .Set("m_minWidth", 120f);
            Build.Text(button, "Label", "OK");
            return button;
        }

        private LayoutText Wrapped() {
            Layout column = Build.Layout(_stage.Content, "Column", Layout.LayoutDirection.Column)
                .Sized(SizingMode.FitContent, SizingMode.FitContent);
            LayoutText text = Build.Text(column, "Text", LongText).Sized(SizingMode.FitContent, SizingMode.FitContent);
            text.Tmp().textWrappingMode = TextWrappingModes.Normal;
            return text;
        }
    }
}
