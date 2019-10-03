using Newtonsoft.Json.Linq;
using Windows.Foundation;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;

namespace RNInkCanvas
{
    class AGInkCanvas : InkCanvas
    {
        // Событие изменения
        public event TypedEventHandler<AGInkCanvas, JArray> EndChanging;

        public void InitEventSystem()
        {
            DeinitEventSystem();

            InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
        }

        public void DeinitEventSystem()
        {
            InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            // INFO: Generate JSON it! Not change it!
            var arrayStrokes = new JArray();
            foreach (InkStroke stroke in args.Strokes)
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
            EndChanging.Invoke(this, arrayStrokes);
        }
    }
}
