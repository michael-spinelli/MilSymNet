using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace MilSymUwp
{
    
    public class ImageInfo: IDisposable
    {
        private CanvasRenderTarget _crt;
        private Point _offset = new Point(0,0);
        private Rect _symbolBounds = Rect.Empty;
        private Rect _imageBounds = Rect.Empty;
        // Track whether Dispose has been called.
        private bool disposed = false;

        public ImageInfo(CanvasRenderTarget image)
        {
            _crt = image;
            BitmapSize bsize = image.SizeInPixels;
            Size size = image.Size;
            _offset = new Point(size.Width / 2.0f, size.Height / 2.0f);
            using (CanvasDrawingSession ds = image.CreateDrawingSession())
            {
                _symbolBounds = image.GetBounds(ds);
                _imageBounds = ShapeUtilities.clone(_symbolBounds);
            }
        }

        public ImageInfo(CanvasRenderTarget image, Point offset, Rect symbolBounds, Rect imageBounds)
        {
            _crt = image;
            _offset = offset;
            _symbolBounds = symbolBounds;
            _imageBounds = imageBounds;
        }

        public CanvasRenderTarget getCanvasRenderTarget()
        {
            return _crt;
        }

        public WriteableBitmap getWriteableBitmap()
        {
            WriteableBitmap wbmp = new WriteableBitmap((int)(_symbolBounds.Width), (int)(_symbolBounds.Height));
            _crt.GetPixelBytes(wbmp.PixelBuffer);
            return wbmp;
        }

        public byte[] getPixelBytes()
        {
            return _crt.GetPixelBytes();
        }

        public Point getAnchorPoint()
        {
            return new Point((int)_offset.X, (int)_offset.Y);
        }

        public Rect getSymbolBounds()
        {
            return new Rect((int)_symbolBounds.X, (int)_symbolBounds.Y, (int)_symbolBounds.Width, (int)_symbolBounds.Height);
        }

        ~ImageInfo()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing == true && _crt != null)
                {
                    _crt.Dispose();
                    _crt = null;
                }
                disposed = true;
            }
        }
    }
}
