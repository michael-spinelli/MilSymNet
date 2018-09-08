using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MilSymUwp
{
    public class ShapeUtilities
    {
        public static Rect clone(Rect rect)
        {
            return new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rect cloneToRect(Rect rect)
        {
            return new Rect((int)(rect.X + 0.5), (int)(rect.Y + 0.5), (int)(rect.Width + 0.5), (int)(rect.Height + 0.5));
        }

        public static Point offset(Point point, double x, double y)
        {
            point.X += x;
            point.Y += y;
            return point;
        }

        public static Rect offset(Rect rect, double x, double y)
        {
            rect.X += x;
            rect.Y += y;
            return rect;
        }

        public static Point clone(Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static Rect inflate(Rect r1, int x, int y)
        {
            r1 = new Rect(r1.X - x, r1.Y - y, r1.Width + (x*2), r1.Height + (y*2));
            return r1;
        }

        public static Rect union(Rect r1, Rect r2)
        {
            r1.Union(r2);
            return r1;
        }


        public static Rect union(Rect r, Point p)
        {
            r.Union(p);
            return r;
        }


        public static int getCenterX(Rect rect)
        {
            return (int)(rect.X + (rect.Width / 2) + 0.5);
        }

        public static float getCenterXf(Rect rect)
        {
            return (float)(rect.X + (rect.Width / 2f));
        }

        public static int getCenterY(Rect rect)
        {
            return (int)(rect.Y + (rect.Height / 2) + 0.5);
        }

        public static float getCenterYf(Rect rect)
        {
            return (float)(rect.Y + (rect.Height / 2f));
        }

        public static int round(float number)
        {
            return (int)(number + 0.5);
        }

        public static int round(double number)
        {
            return (int)(number + 0.5);
        }
    }
}
