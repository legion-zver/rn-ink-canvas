using Newtonsoft.Json.Linq;
using ReactNative.UIManager.Events;

namespace RNInkCanvas
{
    class RNInkCanvasChangeEvent : Event
    {
        private JArray _jArray;

        public RNInkCanvasChangeEvent(int viewTag, JArray array) : base(viewTag)
        {
            _jArray = array;
        }

        public override string EventName
        {
            get
            {
                return "topChange";
            }
        }

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            eventEmitter.receiveEvent(ViewTag, EventName, new JObject
            {
                { "strokes", _jArray }
            });
        }
    }
}
