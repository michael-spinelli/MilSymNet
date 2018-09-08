using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MilSymNet
{
    public class ShapeUtilities
    {
        public static Rectangle clone(Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle cloneToRectangle(RectangleF rect)
        {
            return new Rectangle((int)(rect.X + 0.5), (int)(rect.Y + 0.5), (int)(rect.Width + 0.5), (int)(rect.Height + 0.5));
        }

        public static RectangleF clone(RectangleF rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Point clone(Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static PointF clone(PointF point)
        {
            return new PointF(point.X, point.Y);
        }

        public static Rectangle union(Rectangle r1, RectangleF r2)
        {
            return Rectangle.Union(r1, cloneToRectangle(r2));
        }

        public static Rectangle union(RectangleF r1, Rectangle r2)
        {
            return Rectangle.Union(cloneToRectangle(r1), r2);
        }

        public static Rectangle union(Rectangle r1, Rectangle r2)
        {
            return Rectangle.Union(r1, r2);
        }

        public static RectangleF union(RectangleF r1, RectangleF r2)
        {
            return RectangleF.Union(r1, r2);
        }

        public static Rectangle union(Rectangle r, Point p)
        {
            return Rectangle.Union(r, new Rectangle(p.X, p.Y, 0, 0));
        }

        public static Rectangle union(RectangleF r, Point p)
        {
            return Rectangle.Union(cloneToRectangle(r), new Rectangle(p.X, p.Y, 0, 0));
        }

        public static int getCenterX(RectangleF rect)
        {
            return (int)(rect.X + (rect.Width / 2) + 0.5);
        }

        public static int getCenterX(Rectangle rect)
        {
            return (int)(rect.X + (rect.Width / 2) + 0.5);
        }

        public static float getCenterXf(RectangleF rect)
        {
            return rect.X + (rect.Width / 2f);
        }

        public static float getCenterXf(Rectangle rect)
        {
            return rect.X + (rect.Width / 2f);
        }

        public static int getCenterY(RectangleF rect)
        {
            return (int)(rect.Y + (rect.Height / 2) + 0.5);
        }

        public static int getCenterY(Rectangle rect)
        {
            return (int)(rect.Y + (rect.Height / 2) + 0.5);
        }

        public static float getCenterYf(RectangleF rect)
        {
            return rect.Y + (rect.Height / 2f);
        }

        public static float getCenterYf(Rectangle rect)
        {
            return rect.Y + (rect.Height / 2f);
        }

        public static int round(float number)
        {
            return (int)(number + 0.5);
        }

        public static int round(double number)
        {
            return (int)(number + 0.5);
        }

        public static GraphicsPath createRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
