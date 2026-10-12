using NUnit.Framework;
using UnityEngine;

namespace Poke.UI.Tests
{
    public class PaddingTests
    {
        private const float Tolerance = 0.5f;

        [TestCase(Layout.LayoutDirection.Row)]
        [TestCase(Layout.LayoutDirection.RowReverse)]
        [TestCase(Layout.LayoutDirection.Column)]
        [TestCase(Layout.LayoutDirection.ColumnReverse)]
        public void ACentredChildSitsInTheMiddleOfThePaddedArea(Layout.LayoutDirection direction) {
            using Stage stage = new Stage();
            Layout box = Build.Layout(stage.Content, "Box", direction)
                .Sized(SizingMode.Fixed, SizingMode.Fixed)
                .Set("m_padding", new Margins { left = 40, right = 0, top = 30, bottom = 0 })
                .Set("m_justifyContent", Layout.Justification.Center)
                .Set("m_alignContent", Layout.Alignment.Center)
                .Set("m_alignItems", Layout.Alignment.Center)
                .Size(300, 200);
            LayoutItem child = Build.Item(box, "Child").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(50, 20);

            stage.Show();

            Rect area = stage.Box(box);
            Rect placed = stage.Box(child);
            Assert.That(placed.center.x - area.xMin, Is.EqualTo(40 + 260 / 2f).Within(Tolerance), "across");
            Assert.That(area.yMax - placed.center.y, Is.EqualTo(30 + 170 / 2f).Within(Tolerance), "down");
        }
    }
}
