using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input.Inking.Core;
using Windows.UI.Xaml.Controls;

namespace RNInkCanvas
{
    class AGInkCanvas : InkCanvas
    {
        private CoreInkIndependentInputSource eventsInputSource = null;

        // Событие изменения
        public event TypedEventHandler<AGInkCanvas, PointerEventArgs> EndChanging;

        public void InitEventSystem()
        {
            DeinitEventSystem();

            eventsInputSource = CoreInkIndependentInputSource.Create(InkPresenter);
            eventsInputSource.PointerReleasing += Core_PointerReleasing;
        }

        public void DeinitEventSystem()
        {
            if (eventsInputSource == null)
            {
                return;
            }
            eventsInputSource.PointerReleasing -= Core_PointerReleasing;
            eventsInputSource = null;
        }

        private void Core_PointerReleasing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            EndChanging.Invoke(this, args);
        }
    }
}
