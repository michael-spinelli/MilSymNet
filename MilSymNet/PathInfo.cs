using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MilSymNet
{
    public class PathInfo
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

        private GraphicsPath _gp = null;
        private Pen _pen = null;
        private bool _fill = false;
        private int _type = -1;

        public PathInfo(GraphicsPath gp, Pen pen, bool fill, int shapeType)
        {
            _gp = gp;
            _pen = pen;
            _fill = fill;
            _type = shapeType;
        }

        public RectangleF getBounds()
        {
            return _gp.GetBounds(new Matrix(), _pen);
        }
    }
}
