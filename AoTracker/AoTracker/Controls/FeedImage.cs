using System;
using System.Collections.Generic;
using System.Text;
using FFImageLoading.Forms;

namespace AoTracker.Controls
{
    public class FeedImage : CachedImage
    {
        public FeedImage()
        {
            Finish += OnFinish;
        }

        private void OnFinish(object sender, CachedImageEvents.FinishEventArgs e)
        {
            NativeSizeChanged();
        }
    }
}
