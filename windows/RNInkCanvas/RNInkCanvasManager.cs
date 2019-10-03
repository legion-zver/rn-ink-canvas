using Newtonsoft.Json.Linq;
using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using Windows.UI.Input.Inking;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.Generic;
using ReactNative.Bridge;
using Microsoft.Graphics.Canvas;
using System;
using System.IO;

namespace RNInkCanvas
{
    class RNInkCanvasManager : SimpleViewManager<AGInkCanvas>
    {
        private const int CommandExportImageBase64 = 1;
        private const int CommandSetStrokes = 2;
        private const int CommandClear = 3;

        private ReactContext _reactContext;

        public override string Name {
            get
            {
                return "RNInkCanvas";
            }
        }

        public override JObject ViewCommandsMap
        {
            get
            {
                return new JObject
                {
                    { "clear", CommandClear },
                    { "setStrokes", CommandSetStrokes },
                    { "exportImageBytes", CommandExportImageBase64 }
                };
            }
        }

        public RNInkCanvasManager(ReactContext reactContext)
        {
            _reactContext = reactContext;
        }

        public ReactContext GetReactContext()
        {
            return _reactContext;
        }

        protected override AGInkCanvas CreateViewInstance(ThemedReactContext reactContext)
        {
            var canvas = new AGInkCanvas();
            // Set supported inking device types.
            canvas.InkPresenter.InputDeviceTypes =
                Windows.UI.Core.CoreInputDeviceTypes.Mouse |
                Windows.UI.Core.CoreInputDeviceTypes.Touch |
                Windows.UI.Core.CoreInputDeviceTypes.Pen;

            // Set initial ink stroke attributes.
            InkDrawingAttributes drawingAttributes = new InkDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Colors.Black;
            drawingAttributes.IgnorePressure = false;
            drawingAttributes.FitToCurve = true;
            canvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);

            // Return canvas
            return canvas;
        }

        [ReactProp("lineWidth")]
        public void SetLineWidth(AGInkCanvas view, double width)
        {
            InkDrawingAttributes drawingAttributes = view.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Windows.Foundation.Size(width, width);
            view.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        [ReactProp("lineColor")]
        public void SetLineColor(AGInkCanvas view, string color)
        {
            InkDrawingAttributes drawingAttributes = view.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = ColorHelper.ToColor(color);
            view.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        [ReactProp("readOnly")]
        public void SetLineWidth(AGInkCanvas view, bool value)
        {
            view.InkPresenter.IsInputEnabled = !value;
        }

        public override async void ReceiveCommand(AGInkCanvas view, int commandId, JArray args)
        {
            switch (commandId)
            {
                case CommandClear:
                    view.InkPresenter.StrokeContainer.Clear();
                    return;
                case CommandSetStrokes:
                    var arrayStrokes = args[0].Value<JArray>();
                    if (arrayStrokes == null)
                    {
                        return;
                    }
                    view.InkPresenter.StrokeContainer.Clear();
                    var strokes = new List<InkStroke>();
                    var builder = new InkStrokeBuilder();
                    foreach (JObject objStroke in arrayStrokes)
                    {
                        var arrayPoints = objStroke.Value<JArray>("points");
                        if (arrayPoints == null || (arrayPoints != null && arrayPoints.Count < 1))
                        {
                            continue;
                        }
                        var points = new List<InkPoint>();
                        foreach (JObject objPoint in arrayPoints)
                        {
                            points.Add(new InkPoint(new Windows.Foundation.Point(
                                objPoint.Value<double>("x"),
                                objPoint.Value<double>("y")
                            ), objPoint.Value<float>("p")));
                        }
                        var w = objStroke.Value<double>("w");
                        var stroke = builder.CreateStrokeFromInkPoints(points, System.Numerics.Matrix3x2.Identity);
                        stroke.DrawingAttributes.Size = new Windows.Foundation.Size(w, w);
                        stroke.DrawingAttributes.Color = ColorHelper.ToColor(objStroke.Value<string>("c") ?? "#000000");
                        strokes.Add(stroke);
                    }
                    view.InkPresenter.StrokeContainer.AddStrokes(strokes);
                    return;
                case CommandExportImageBase64:
                    if (!view.HasTag())
                    {
                        return;
                    }
                    var viewTag = view.GetTag();
                    var canvasStrokes = view.InkPresenter.StrokeContainer.GetStrokes();
                    if (canvasStrokes.Count > 0)
                    {
                        var width = (int)view.ActualWidth;
                        var height = (int)view.ActualHeight;
                        var device = CanvasDevice.GetSharedDevice();
                        var renderTarget = new CanvasRenderTarget(device, width, height, 96);
                        using (var ds = renderTarget.CreateDrawingSession())
                        {
                            ds.Clear(Windows.UI.Colors.White);
                            ds.DrawInk(canvasStrokes);
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await renderTarget.SaveAsync(ms.AsRandomAccessStream(), CanvasBitmapFileFormat.Png);
                            this.GetReactContext()
                                .GetNativeModule<UIManagerModule>()
                                .EventDispatcher
                                .DispatchEvent(new RNInkCanvasExportEvent(viewTag, Convert.ToBase64String(ms.ToArray())));
                        }
                    }
                    return;
                default:
                    break;
            }
            base.ReceiveCommand(view, commandId, args);
        }

        protected override void AddEventEmitters(ThemedReactContext reactContext, AGInkCanvas view)
        {
            base.AddEventEmitters(reactContext, view);
            view.InitEventSystem();

            view.EndChanging += AGInkCanvas_EndChanging;

        }


        public override void OnDropViewInstance(ThemedReactContext reactContext, AGInkCanvas view)
        {
            base.OnDropViewInstance(reactContext, view);
            view.DeinitEventSystem();

            view.EndChanging -= AGInkCanvas_EndChanging;
        }

        private void AGInkCanvas_EndChanging(AGInkCanvas view, JArray strokes)
        {
            if (!view.HasTag())
            {
                return;
            }
            var context = this.GetReactContext();
            context.GetNativeModule<UIManagerModule>().EventDispatcher
                .DispatchEvent(new RNInkCanvasChangeEvent(view.GetTag(), strokes));
        }
    }
}
