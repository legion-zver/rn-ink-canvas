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
            InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        public void DeinitEventSystem()
        {
            InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
            InkPresenter.StrokesErased -= InkPresenter_StrokesErased;
        }

        private void DidUpdateInkPresenter(InkPresenter inkPresenter)
        {
            // INFO: Generate JSON it! Not change it!
            var arrayStrokes = new JArray();
            foreach (InkStroke stroke in inkPresenter.StrokeContainer.GetStrokes())
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

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            DidUpdateInkPresenter(sender);
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            DidUpdateInkPresenter(sender);
        }
    }
}
