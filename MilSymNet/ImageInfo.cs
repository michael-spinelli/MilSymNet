using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MilSymNet
{
    
    public class ImageInfo
    {
        private Bitmap _bitmap = null;
        private PointF _centerPoint = new PointF(0, 0);
        private RectangleF _symbolBounds = new RectangleF();

        public ImageInfo(Bitmap image)
        {
            _centerPoint = new PointF(image.Width / 2.0f, image.Height / 2.0f);
            GraphicsUnit guPixel = GraphicsUnit.Pixel;
            _symbolBounds = image.GetBounds(ref guPixel);

        }

        public ImageInfo(Bitmap image, PointF centerPoint, RectangleF symbolBounds)
        {
            _bitmap = image;
            _centerPoint = centerPoint;
            _symbolBounds = symbolBounds;
        }

        public Bitmap getBitmap()
        {
            return _bitmap;
        }

        public PointF getCenterPointF()
        {
            return _centerPoint;
        }

        public Point getCenterPoint()
        {
            return new Point((int)_centerPoint.X, (int)_centerPoint.Y);
        }

        public RectangleF getSymbolBoundsF()
        {
            return _symbolBounds;
        }

        public Rectangle getSymbolBounds()
        {
            return new Rectangle((int)_symbolBounds.X, (int)_symbolBounds.Y, (int)_symbolBounds.Width, (int)_symbolBounds.Height);
        }

        public RectangleF getImageBounds()
        {
            GraphicsUnit pixels = GraphicsUnit.Pixel;
            return _bitmap.GetBounds(ref pixels);
        }


    }
}
