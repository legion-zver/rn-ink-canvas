using ReactNative.Bridge;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Ink.Canvas.RNInkCanvas
{
    /// <summary>
    /// A module that allows JS to share data.
    /// </summary>
    class RNInkCanvasModule : NativeModuleBase
    {
        /// <summary>
        /// Instantiates the <see cref="RNInkCanvasModule"/>.
        /// </summary>
        internal RNInkCanvasModule()
        {

        }

        /// <summary>
        /// The name of the native module.
        /// </summary>
        public override string Name
        {
            get
            {
                return "RNInkCanvas";
            }
        }
    }
}
