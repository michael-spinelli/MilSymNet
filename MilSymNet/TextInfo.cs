using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MilSymNet
{
    public class TextInfo
    {
        private String _text = "";
        private Point _location = Point.Empty;
        private Rectangle _bounds = Rectangle.Empty;

        public TextInfo(String text, int x, int y, Font font, Graphics g)
        {
            if(text != null)
            {
                _text = text;
            }
            _location = new Point(x, y);
            _bounds = new Rectangle();

            SizeF size = SizeF.Empty;
            if(g != null)
                size = g.MeasureString(text, font);
            else
                size = TextRenderer.MeasureText(text, font);
            

            _bounds = new Rectangle(x,y, (int)(size.Width + 0.5),(int)(size.Height + 0.5));
        }

        public void setLocation(int x, int y)
        {
            _location.X = x;
            _location.Y = y;
            _bounds.Location = new Point(x, y);
        }

        public Point getLocation()
        {
            return _location;
        }

        public void shift(int x, int y)
        {
            _location.Offset(x, y);
            _bounds.Offset(x, y);
        }

        public String getText() 
        { 
            return _text; 
        }

        public Rectangle getTextBounds()
        {
            return _bounds;
        }

        public Rectangle getTextOutlineBounds()
        {
            RendererSettings RS = RendererSettings.getInstance();
            int outlineOffset = RS.getTextOutlineWidth();
            Rectangle bounds = ShapeUtilities.clone(_bounds);
            bounds.Inflate(outlineOffset, outlineOffset);
            return bounds;
        }
    }
}
