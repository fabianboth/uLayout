using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace Poke.UI.Tests
{
    public class IdleTests
    {
        private const int IdleFrames = 10;

        [UnityTest]
        public IEnumerator ASettledTreeLaysNothingOutWhileIdle() {
            using Stage stage = new Stage();
            Layout root = Build.Layout(stage.Content, "Root", Layout.LayoutDirection.Column)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", 12f);
            Layout header = Build.Layout(root, "Header", Layout.LayoutDirection.Row)
                .Sized(SizingMode.Fixed, SizingMode.FitContent)
                .Size(600, 0);
            Build.Text(header, "Title", "Heading");
            Build.Item(header, "Spacer").Sized(SizingMode.Grow, SizingMode.Fixed).Size(0, 0);
            Build.Text(header, "End", "End");
            LayoutText body = Build.Text(root, "Body", "The quick brown fox jumps over the lazy dog and keeps on running far away.")
                .Sized(SizingMode.Grow, SizingMode.FitContent);
            body.Tmp().textWrappingMode = TextWrappingModes.Normal;
            Build.Item(stage.Content, "Backdrop").Sized(SizingMode.Grow, SizingMode.Grow);
            stage.Show();
            yield return Frame.Rendered(() => { });

            int passes = 0;
            Action count = () => passes++;
            Layout[] layouts = stage.Content.GetComponentsInChildren<Layout>();
            foreach(Layout layout in layouts) layout.OnLayoutChanged += count;
            for(int frame = 0; frame < IdleFrames; frame++) yield return Frame.Rendered(() => { });
            foreach(Layout layout in layouts) layout.OnLayoutChanged -= count;

            Assert.That(passes, Is.Zero);
        }

        [Test]
        public void NoLayoutComponentWorksEveryFrame() {
            string[] perFrame = { "Update", "LateUpdate", "FixedUpdate" };
            string[] offenders = typeof(LayoutItem).Assembly.GetTypes()
                .Where(type => typeof(LayoutItem).IsAssignableFrom(type))
                .SelectMany(type => perFrame
                    .Where(name => type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) != null)
                    .Select(name => $"{type.Name}.{name}"))
                .ToArray();

            Assert.That(offenders, Is.Empty);
        }
    }
}
