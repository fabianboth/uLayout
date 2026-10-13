using NUnit.Framework;
using UnityEngine;

namespace Poke.UI.Tests
{
    public class EmptyLayoutTests
    {
        private const float Tolerance = 0.5f;

        [TestCase(Layout.LayoutDirection.Row)]
        [TestCase(Layout.LayoutDirection.Column)]
        public void ALayoutWhoseOnlyChildIsInactiveFitsToNothing(Layout.LayoutDirection direction) {
            using Stage stage = new Stage();
            Layout box = Build.Layout(stage.Content, "Box", direction)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", 8f);
            Build.Item(box, "Hidden").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(50, 20).gameObject.SetActive(false);

            stage.Show();

            Rect area = stage.Box(box);
            Assert.That(area.width, Is.EqualTo(0f).Within(Tolerance), "across");
            Assert.That(area.height, Is.EqualTo(0f).Within(Tolerance), "down");
        }
    }
}
