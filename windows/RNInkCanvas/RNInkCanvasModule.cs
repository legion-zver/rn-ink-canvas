using ReactNative.Bridge;

namespace RNInkCanvas
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
                return "RNInkCanvasModule";
            }
        }
    }
}
