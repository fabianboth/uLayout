using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Poke.UI.Tests
{
    public class LayoutElementTests
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
        public void AUnityGroupGivesALayoutItsLaidOutWidth() {
            HorizontalLayoutGroup group = UnityRow(600);
            Layout button = Build.Layout(group, "Button", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.Fixed)
                .Padded(28, 28)
                .Size(0, 48);
            LayoutText label = Build.Text(button, "Label", "A caption");

            _stage.Show();

            Assert.That(_stage.Box(button).width, Is.EqualTo(label.Tmp().GetPreferredValues().x + 56).Within(Tolerance));
        }

        [Test]
        public void AUnityGroupDoesNotStretchAFixedItem() {
            HorizontalLayoutGroup group = UnityRow(600);
            LayoutItem item = Build.Item(group, "Item").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(80, 20);

            _stage.Show();

            Assert.That(_stage.Box(item).width, Is.EqualTo(80).Within(Tolerance));
        }

        [Test]
        public void UnusedBoundsAndWeightsReportNothing() {
            LayoutItem item = Build.Item(_stage.Content, "Item")
                .Sized(SizingMode.Fixed, SizingMode.Grow)
                .Set("m_minWidth", 50f)
                .Set("m_maxWidth", 70f)
                .Set("m_flexWidth", 3f)
                .Set("m_flexHeight", 2f);

            Assert.That(item.minWidth, Is.Zero);
            Assert.That(item.flexibleWidth, Is.Zero);
            Assert.That(item.flexibleHeight, Is.EqualTo(2));
        }

        [Test]
        public void UsedBoundsAreReported() {
            LayoutItem item = Build.Item(_stage.Content, "Item")
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_useMinWidth", true)
                .Set("m_minWidth", 50f)
                .Set("m_useMinHeight", true)
                .Set("m_minHeight", 30f);

            Assert.That(item.minWidth, Is.EqualTo(50));
            Assert.That(item.minHeight, Is.EqualTo(30));
        }

        private HorizontalLayoutGroup UnityRow(float width) {
            HorizontalLayoutGroup group = new GameObject("Group", typeof(RectTransform)).AddComponent<HorizontalLayoutGroup>();
            group.transform.SetParent(_stage.Content, false);
            ((RectTransform)group.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            group.childControlWidth = true;
            group.childControlHeight = false;
            group.childForceExpandWidth = false;
            group.childForceExpandHeight = false;
            return group;
        }
    }
}
