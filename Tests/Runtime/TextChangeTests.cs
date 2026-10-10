using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TextCore;

namespace Poke.UI.Tests
{
    public class TextChangeTests
    {
        private const float Gap = 24;
        private const float Tolerance = 0.5f;

        private Stage _stage;
        private LayoutText _title;
        private LayoutText _hint;
        private TMP_FontAsset _wideFont;

        [SetUp]
        public void SetUp() {
            _stage = new Stage();
            Layout header = Build.Layout(_stage.Content, "Header", Layout.LayoutDirection.Row)
                .Sized(SizingMode.FitContent, SizingMode.FitContent)
                .Set("m_innerSpacing", Gap);
            _title = Build.Text(header, "Title", "Short");
            _hint = Build.Text(header, "Hint", "Back");
            _stage.Show();
        }

        [TearDown]
        public void TearDown() {
            _stage.Dispose();
            if(_wideFont) {
                _wideFont.atlasTextures = null;
                _wideFont.material = null;
                Object.Destroy(_wideFont);
            }
        }

        [UnityTest]
        public IEnumerator ANewTextIsLaidOutInItsFrame() {
            _title.Tmp().text = "A considerably longer heading";
            yield return Frame.Rendered(TheHintFollowsTheTitle);
        }

        [UnityTest]
        public IEnumerator ANewFontSizeIsLaidOutInItsFrame() {
            _title.Tmp().fontSize = 40;
            yield return Frame.Rendered(TheHintFollowsTheTitle);
        }

        [UnityTest]
        public IEnumerator ANewStyleIsLaidOutInItsFrame() {
            _title.Tmp().fontStyle = FontStyles.Bold | FontStyles.UpperCase;
            yield return Frame.Rendered(TheHintFollowsTheTitle);
        }

        [UnityTest]
        public IEnumerator ANewFontAssetIsLaidOutInItsFrame() {
            _wideFont = Object.Instantiate(_title.Tmp().font);
            FaceInfo face = _wideFont.faceInfo;
            face.scale *= 1.5f;
            _wideFont.faceInfo = face;

            _title.Tmp().font = _wideFont;
            yield return Frame.Rendered(TheHintFollowsTheTitle);
        }

        [UnityTest]
        public IEnumerator AStandaloneTextFitsItsNewTextInItsFrame() {
            LayoutText name = Build.Text(_stage.Content, "Name", "Name")
                .Sized(SizingMode.FitContent, SizingMode.FitContent);
            yield return Frame.Rendered(() => { });

            name.Tmp().text = "A much longer name than before";
            yield return Frame.Rendered(() =>
                Assert.That(_stage.Box(name).width, Is.EqualTo(name.Tmp().GetPreferredValues().x).Within(Tolerance)));
        }

        private void TheHintFollowsTheTitle() {
            Assert.That(_stage.Box(_title).width, Is.EqualTo(_title.Tmp().GetPreferredValues().x).Within(Tolerance));
            Assert.That(_stage.Box(_hint).xMin - _stage.Box(_title).xMax, Is.EqualTo(Gap).Within(Tolerance));
        }
    }
}
