using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace MilSymUwp
{
    class TextInfo : IDisposable
    {
        Point location = new Point(0, 0);
        CanvasTextFormat format = null;
        CanvasTextLayout textLayout = null;
        Rect bounds = Rect.Empty;
        private bool disposed = false;

        public TextInfo(ICanvasResourceCreator rc, String text)
        {
            
            format = RendererSettings.getInstance().getLabelFont();
            textLayout = new CanvasTextLayout(rc, text, format, 0.0f, 0.0f);
            bounds = getTextBounds();
        }

        public void setLocation(double x, double y)
        {
            location.X = x;
            location.Y = y;
        }

        public Point getLocation()
        {
            return location;
        }

        public void shift(double x, double y)
        {
            location.X += x;
            location.Y += y;
        }

        public Rect getTextBounds()
        {
            Rect bounds = new Rect(location.X + textLayout.DrawBounds.X, location.Y + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);
            return bounds;
        }

        public Rect getTextBoundsWithOutline()
        {
            double outlineWidth = RendererSettings.getInstance().getTextOutlineWidth();
            Rect bounds = new Rect(location.X - outlineWidth, location.Y - outlineWidth, bounds.Width + (outlineWidth * 2), bounds.Height + (outlineWidth * 2));
            return bounds;
        }

        public void drawText(CanvasRenderTarget crt, Color color)
        {
            using (CanvasDrawingSession ds = crt.CreateDrawingSession())
            {
                ds.DrawTextLayout(textLayout, (float)location.X, (float)location.Y, color);
            }
               
        }

        public void drawText(CanvasDrawingSession ds, Color color)
        {
            ds.DrawTextLayout(textLayout, (float)location.X, (float)location.Y, color);
        }

        ~TextInfo()
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
                if (disposing == true && textLayout != null)
                {

                    textLayout.Dispose();
                    textLayout = null;
                }
                disposed = true;
            }
        }
    }
}
