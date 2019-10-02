using Newtonsoft.Json.Linq;
using ReactNative.UIManager.Events;

namespace RNInkCanvas
{
    class RNInkCanvasExportEvent : Event
    {
        private string _base64;

        public RNInkCanvasExportEvent(int viewTag, string base64) : base(viewTag)
        {
            _base64 = base64;
        }

        public override string EventName
        {
            get
            {
                return "topExport";
            }
        }

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            eventEmitter.receiveEvent(ViewTag, EventName, new JObject
            {
                { "base64", _base64 }
            });
        }
    }
}
