using Newtonsoft.Json.Linq;
using ReactNative.UIManager.Events;
using System.Collections.Generic;
using Windows.UI.Input.Inking;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Ink.Canvas
{
    class RNInkCanvasChangeEvent : Event
    {
        private IReadOnlyList<InkStroke> _strokes;

        public RNInkCanvasChangeEvent(int viewTag, IReadOnlyList<InkStroke> strokes) : base(viewTag)
        {
            _strokes = strokes;
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
            var arrayStrokes = new JArray();
            foreach (InkStroke stroke in _strokes)
            {
                var arrayPoints = new JArray();
                foreach (InkPoint point in stroke.GetInkPoints())
                {
                    arrayPoints.Add(new JObject()
                    {
                        { "x", point.Position.X },
                        { "y", point.Position.Y },
                        { "p", point.Pressure },
                    });
                }
                arrayStrokes.Add(new JObject()
                {
                    { "c", stroke.DrawingAttributes.Color.ToHex() },
                    { "w", stroke.DrawingAttributes.Size.Width },
                    { "points", arrayPoints }
                });
            }
            eventEmitter.receiveEvent(ViewTag, EventName, new JObject
            {
                { "strokes", arrayStrokes }
            });
        }
    }
}
