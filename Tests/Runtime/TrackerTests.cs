using NUnit.Framework;
using UnityEngine;

namespace Poke.UI.Tests
{
    public class TrackerTests
    {
        private const DrivenTransformProperties Size = DrivenTransformProperties.SizeDelta;
        private const DrivenTransformProperties Placement = DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Anchors;

        [Test]
        public void AnIgnoredItemClaimsNothing() {
            using Stage stage = new Stage();
            Layout row = Build.Layout(stage.Content, "Row", Layout.LayoutDirection.Row).Sized(SizingMode.FitContent, SizingMode.FitContent);
            LayoutItem backdrop = Build.Item(row, "Backdrop").Set("m_ignoreLayout", true).Size(80, 20);
            Build.Item(backdrop, "Fill");

            stage.Show();

            Assert.That(backdrop.TrackerProps & (Size | Placement), Is.EqualTo(DrivenTransformProperties.None));
        }

        [Test]
        public void AnItemNothingSizesClaimsOnlyItsPlace() {
            using Stage stage = new Stage();
            Layout row = Build.Layout(stage.Content, "Row", Layout.LayoutDirection.Row).Sized(SizingMode.FitContent, SizingMode.FitContent);
            LayoutItem item = Build.Item(row, "Item").Sized(SizingMode.FitContent, SizingMode.FitContent).Size(80, 20);
            Build.Item(item, "Inner");

            stage.Show();

            Assert.That(item.TrackerProps & Size, Is.EqualTo(DrivenTransformProperties.None), "a plain item does not fit itself to anything");
            Assert.That(item.TrackerProps & Placement, Is.EqualTo(Placement), "its layout places it");
        }
    }
}
