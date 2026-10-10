using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Constraints;
using UnityEngine.UI;
using Is = UnityEngine.TestTools.Constraints.Is;

namespace Poke.UI.Tests
{
    public class AllocationTests
    {
        [Test]
        public void ARebuildWithLoggingOffAllocatesNothing() {
            using Stage stage = new Stage();
            Layout root = Build.Layout(stage.Content, "Root", Layout.LayoutDirection.Column)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", 8f);
            Layout row = Build.Layout(root, "Row", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.FitContent)
                .Padded(10, 10)
                .Size(400, 0);
            Build.Item(row, "Start").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(50, 20);
            Build.Item(row, "Spacer").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 0);
            Build.Item(row, "End").Sized(SizingMode.Fixed, SizingMode.Fixed).Size(70, 30);
            Build.Item(root, "Footer").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 12);
            stage.Show();
            RectTransform rect = (RectTransform)root.transform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
#if UNITY_EDITOR
            Layout.RefreshedThisFrame.Clear();
#endif

            Assert.That(() => LayoutRebuilder.ForceRebuildLayoutImmediate(rect), Is.Not.AllocatingGCMemory());
        }
    }
}
