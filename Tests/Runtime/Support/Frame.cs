using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace Poke.UI.Tests
{
    public static class Frame
    {
        public static IEnumerator Rendered(Action check) {
            bool rendered = false;
            Exception failure = null;

            void Check() {
                Canvas.willRenderCanvases -= Check;
                rendered = true;
                try {
                    check();
                }
                catch(Exception e) {
                    failure = e;
                }
            }

            Canvas.willRenderCanvases += Check;
            yield return new WaitUntil(() => rendered);
            if(failure != null) ExceptionDispatchInfo.Capture(failure).Throw();
        }
    }
}
