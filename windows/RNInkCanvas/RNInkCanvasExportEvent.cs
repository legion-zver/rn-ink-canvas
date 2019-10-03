using Newtonsoft.Json.Linq;
using ReactNative.UIManager.Events;

namespace RNInkCanvas
{
    class RNInkCanvasExportEvent : Event
    {
        public static readonly string EventNameValue = "topExport";

        private string _base64;

        public RNInkCanvasExportEvent(int viewTag, string base64) : base(viewTag)
        {
            _base64 = base64;
        }

        public override string EventName
        {
            get
            {
                return RNInkCanvasExportEvent.EventNameValue;
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
