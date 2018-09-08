using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Windows.UI;

namespace MilSymUwp
{
    public class PathInfo: IDisposable
    {
        public static int SHAPE_TYPE_POLYLINE = 0;
        //public static int SHAPE_TYPE_POLYGON=1;
        public static int SHAPE_TYPE_FILL = 1;
        public static int SHAPE_TYPE_MODIFIER = 2;
        public static int SHAPE_TYPE_MODIFIER_FILL = 3;
        public static int SHAPE_TYPE_UNIT_FRAME = 4;
        public static int SHAPE_TYPE_UNIT_FILL = 5;
        public static int SHAPE_TYPE_UNIT_SYMBOL1 = 6;
        public static int SHAPE_TYPE_UNIT_SYMBOL2 = 7;
        public static int SHAPE_TYPE_UNIT_DISPLAY_MODIFIER = 8;
        public static int SHAPE_TYPE_UNIT_ECHELON = 9;
        public static int SHAPE_TYPE_UNIT_AFFILIATION_MODIFIER = 10;
        public static int SHAPE_TYPE_UNIT_HQ_STAFF = 11;
        public static int SHAPE_TYPE_TG_SP_FILL = 12;
        public static int SHAPE_TYPE_TG_SP_FRAME = 13;
        public static int SHAPE_TYPE_TG_Q_MODIFIER = 14;
        public static int SHAPE_TYPE_TG_SP_OUTLINE = 15;
        public static int SHAPE_TYPE_SINGLE_POINT_OUTLINE = 16;
        public static int SHAPE_TYPE_UNIT_OUTLINE = 17;
        //

        /*public void DrawGeometry(
    CanvasGeometry geometry,
    float x,
    float y,
    Color color,
    float strokeWidth,
    CanvasStrokeStyle strokeStyle)//*/

        private CanvasPathBuilder _cpb = null;// new CanvasPathBuilder(ICanvasResourceCreator resourceCreator);
        // Track whether Dispose has been called.
        private bool disposed = false;

        //private GraphicsPath _gp = null;

        private Color _color = Colors.Transparent;
        private bool _fill = false;
        private int _type = -1;

        public PathInfo(ICanvasResourceCreator rc, Windows.UI.Color color, System.Single strokeWidth, CanvasStrokeStyle strokeStyle) //Pen pen, bool fill, int shapeType)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            _cpb = new CanvasPathBuilder(rc);
            //_cpb.SetSegmentOptions(CanvasFigureSegmentOptions.ForceRoundLineJoin);
            _color = color;
            //_fill = fill;
            //_type = shapeType;
        }
        public PathInfo(ICanvasResourceCreator rc, Windows.UI.Color color, System.Single strokeWidth, bool fill, int shapeType) //Pen pen, bool fill, int shapeType)
        {
            Vector2 offset = new Vector2(0.0f, 0.0f);
            _cpb = new CanvasPathBuilder(rc);

            _color = color;
            _fill = fill;
            _type = shapeType;
            CanvasGeometry path = CanvasGeometry.CreatePath(_cpb);
            //path.ComputeBounds

            //var geometry = CanvasGeometry.CreatePath(pathBuilder);
        }

        ~PathInfo()
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
                if (disposing == true && _cpb != null)
                {
                    _cpb.Dispose();
                    _cpb = null;
                }
                disposed = true;
            }
        }

        /*
public RectangleF getBounds()
{
   return _gp.GetBounds(new Matrix(), _pen);
}//*/
    }
}
