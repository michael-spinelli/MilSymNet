using System;
using System.Text.RegularExpressions;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using System.Diagnostics;
using MilSymNetUtilities;

namespace MilSymUwp
{
    public class SVGPath: IDisposable
    {
        private int _ID = -1;
        private static string _regex1 = "(?=[M,m,Z,z,L,l,H,h,V,v,C,c,S,s,Q,q,T,t,A,a])";
        //private static string _regex2 = "(?=[M,m,L,l,H,h,V,v,C,c,S,s,Q,q,T,t,A,a])";
        //private static char[] svgCommands = { 'M', 'm', 'Z', 'z', 'L', 'l', 'H', 'h', 'V', 'v', 'C', 'c', 'S', 's', 'Q', 'q', 'T', 't', 'A', 'a' };
        /*
        static const int ACTION_MOVE_A = 0;//absolute
        static const int ACTION_MOVE_R = 1;//relative
        static const int ACTION_CLOSEPATH_A = 2;//absolute
        static const int ACTION_CLOSEPATH_R = 2;//relative
        static const int ACTION_LINETO_A = 3;//absolute
        static const int ACTION_LINETO_R = 4;//relative
        static const int ACTION_HLINETO_A = 5;//absolute
        static const int ACTION_HLINETO_R = 6;//relative
        static const int ACTION_VLINETO_A = 7;//absolute
        static const int ACTION_VLINETO_R = 8;//relative
        static const int ACTION_CURVETO_A = 9;//absolute
        static const int ACTION_CURVETO_R = 10;//relative
        static const int ACTION_SCURVETO_A = 11;//absolute
        static const int ACTION_SCURVETO_R = 12;//relative
        static const int ACTION_QUADTO_A = 13;//absolute
        static const int ACTION_QUADTO_R = 14;//relative
        static const int ACTION_SQUADTO_A = 15;//absolute
        static const int ACTION_SQUADTO_R = 16;//relative
        static const int ACTION_ARCTO_A = 17;//absolute
        static const int ACTION_ARCTO_R = 18;//relative*/

        
        private CanvasGeometry _cg = null;
        private CanvasCachedGeometry _ccgStroke = null;
        private CanvasCachedGeometry _ccgFill = null;
        private Rect _bounds = Rect.Empty;
        private CanvasGeometry _cgBounds = null;
        // Track whether Dispose has been called.
        private bool disposed = false;

        //CanvasGeometry geometry = CanvasGeometry.CreatePath(pathBuilder);

        public int getID()
        {
            return _ID;
        }

        public Rect computeBounds()
        {
            return _cg.ComputeBounds();
        }
        public Rect computeBounds(Matrix3x2 m)
        {
            return _cg.ComputeBounds(m);
        }
        /**
         * Returns bounds of the core symbol
         * */
        public Rect getBounds()
        {
            return _bounds;//_cg.ComputeBounds();
        }

        public Rect getBounds(float strokeWidth)
        {
            return _cg.ComputeStrokeBounds(strokeWidth);
        }

        public Rect getBounds(float strokeWidth, CanvasStrokeStyle strokeStyle)
        {
            return _cg.ComputeStrokeBounds(strokeWidth, strokeStyle);
        }

        /*
         * Returns bounds of the symbol when it's being outlined.
         * */
        /*public Rect getOutlineBounds(float outlineWidth)
        {
            Pen testPen = new Pen(Color.Red, outlineWidth);
            testPen.MiterLimit = 1f;
            testPen.LineJoin = LineJoin.Miter;
            return _gp.GetBounds(new Matrix(), testPen);
        }//*/

        private SVGPath()
        {

        }

        public SVGPath(SVGPath path)
        {
            _ID = path._ID;
            _cg = path._cg.Transform(Matrix3x2.Identity);
            _bounds = _cg.ComputeBounds();
            _ccgFill = CanvasCachedGeometry.CreateFill(_cg);
        }

        public SVGPath(String unicodeHex, String path)
        {
            String hex = "";
            
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            hex = unicodeHex;
                                
            _ID = int.Parse(unicodeHex.Replace(";", ""), System.Globalization.NumberStyles.HexNumber);//Convert.ToInt32(hex);

            CanvasPathBuilder pb = new CanvasPathBuilder(device);
            parsePath(pb, path);

            
            //L 1.5 = 2650 pixel units in the svg font file
            //double L1_5 = 2650;


            //_cg = _cg.Transform(Matrix3x2.CreateScale(0.2f));
            _cg = _cg.Transform(Matrix3x2.CreateScale(0.0188679245283019f));
            _bounds = _cg.ComputeBounds();

            _ccgFill = CanvasCachedGeometry.CreateFill(_cg);
            
        }

        public SVGPath(String unicodeHex, String path, ICanvasResourceCreator rc)
        {
            _ID = int.Parse(unicodeHex.Replace(";", ""), System.Globalization.NumberStyles.HexNumber);

            CanvasPathBuilder pb = new CanvasPathBuilder(rc);
            parsePath(pb, path);

            _cg = _cg.Transform(Matrix3x2.CreateScale(0.2f));
            _bounds = _cg.ComputeBounds();

            _ccgFill = CanvasCachedGeometry.CreateFill(_cg);
        }

        private bool isValidHexString(String hex)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(hex, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        private bool isValidHexString2(String hex)
        {
            bool isHex;
            char[] arr = hex.ToCharArray();
            foreach (var c in arr)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex)
                    return false;
            }
            return true;
        }


        /*public override object Clone()
        {
            SVGPath clone = new SVGPath();
            clone._gp = (GraphicsPath)_gp.Clone();
            clone._ID = _ID;
            clone._strPath = (String)_strPath.Clone();
            return clone;
        }*/

        private void parsePath(CanvasPathBuilder pb, String path)
        {
            char[] delimiter = { ' ' };
            string[] commands = Regex.Split(path, _regex1);
            string[] values = null;
            float[] points = new float[7];
            Vector2[] Vector2s = new Vector2[4];
            Vector2 lastPoint = new Vector2(0, 0);
            Vector2 lastControlPoint = new Vector2(0, 0);
            //Vector2 firstPoint = new Vector2(0, 0);
            //Vector2 firstPoint = new Vector2(0, 0);

            try
            {

                for (int i = 0; i < commands.Length; i++)
                {

                    String strCommand = commands[i];
                    char action = (strCommand != null && strCommand.Length > 0) ? strCommand[0] : ' ';
                    values = strCommand.Split(delimiter);

                    if (action == 'M')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        lastPoint = new Vector2(points[0], points[1]);
                        //firstPoint = new Vector2(points[0], points[1]);
                        
                        pb.BeginFigure(lastPoint);
                    }
                    else if (action == 'm')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;

                        pb.BeginFigure(points[0], points[1]);
                        lastPoint.X = points[0];
                        lastPoint.Y = points[1];
                        pb.BeginFigure(lastPoint);
                    }
                    else if (action == 'L')//capital letter is absolute location
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        
                        pb.AddLine(points[0], points[1]);

                        lastPoint.X = points[0];
                        lastPoint.Y = points[1];
                        

                    }
                    else if (action == 'l')//lowercase letter is relative location
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;

                        pb.AddLine(points[0], points[1]);

                        lastPoint.X = points[0];
                        lastPoint.Y = points[1];
                    }
                    else if (action == 'H')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));

                        pb.AddLine(points[0], lastPoint.Y);

                        lastPoint.X = points[0];

                    }
                    else if (action == 'h')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;

                        pb.AddLine(points[0], lastPoint.Y);

                        lastPoint.X = points[0];
                    }
                    else if (action == 'V')
                    {
                        points[0] = -(float)Convert.ToDouble(values[0].Substring(1));

                        pb.AddLine(lastPoint.X, points[0]);

                        lastPoint.Y = points[0]; 

                    }
                    else if (action == 'v')
                    {
                        points[0] = -(float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.Y;

                        pb.AddLine(lastPoint.X, points[0]);

                        lastPoint.Y = points[0];
                    }
                    else if (action == 'C')//cubic bezier, 2 control points
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);
                        points[4] = (float)Convert.ToDouble(values[4]);
                        points[5] = -(float)Convert.ToDouble(values[5]);

                        Vector2s[0] = lastPoint;
                        Vector2s[1] = new Vector2(points[0], points[1]);
                        Vector2s[2] = new Vector2(points[2], points[3]);
                        Vector2s[3] = new Vector2(points[4], points[5]);
                        
                        //_gp.AddBezier(Vector2s[0], Vector2s[1], Vector2s[2], Vector2s[3]);
                        pb.AddCubicBezier(Vector2s[1], Vector2s[2], Vector2s[3]);

                        lastPoint = Vector2s[3];
                        lastControlPoint = Vector2s[2];
                    }
                    else if (action == 'c')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;
                        points[4] = (float)Convert.ToDouble(values[4]) + lastPoint.X;
                        points[5] = -(float)Convert.ToDouble(values[5]) + lastPoint.Y;

                        Vector2s[0] = lastPoint;
                        Vector2s[1] = new Vector2(points[0], points[1]);
                        Vector2s[2] = new Vector2(points[2], points[3]);
                        Vector2s[3] = new Vector2(points[4], points[5]);
                        
                        //_gp.AddBezier(Vector2s[0], Vector2s[1], Vector2s[2], Vector2s[3]);
                        pb.AddCubicBezier(Vector2s[1], Vector2s[2], Vector2s[3]);

                        lastPoint = Vector2s[3];
                        lastControlPoint = Vector2s[2];
                    }
                    else if (action == 'S')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);

                        Vector2s[0] = lastPoint;
                        Vector2s[1] = mirrorControlPoint(lastControlPoint, lastPoint);
                        Vector2s[2] = new Vector2(points[0], points[1]);
                        Vector2s[3] = new Vector2(points[2], points[3]);

                        //_gp.AddBezier(Vector2s[0], Vector2s[1], Vector2s[2], Vector2s[3]);
                        pb.AddCubicBezier(Vector2s[1], Vector2s[2], Vector2s[3]);

                        lastPoint = Vector2s[3];
                        lastControlPoint = Vector2s[2];
                    }
                    else if (action == 's')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;

                        Vector2s[0] = lastPoint;
                        Vector2s[1] = mirrorControlPoint(lastControlPoint, lastPoint);
                        Vector2s[2] = new Vector2(points[0], points[1]);
                        Vector2s[3] = new Vector2(points[2], points[3]);

                        //_gp.AddBezier(Vector2s[0], Vector2s[1], Vector2s[2], Vector2s[3]);
                        pb.AddCubicBezier(Vector2s[1], Vector2s[2], Vector2s[3]);

                        lastPoint = Vector2s[3];
                        lastControlPoint = Vector2s[2];
                    }
                    else if (action == 'Q')//quadratic bezier, 1 control point
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);


                        //convert quadratic to cubic bezier
                        Vector2 QP0 = lastPoint;
                        Vector2 QP1 = new Vector2(points[0], points[1]);
                        Vector2 QP2 = new Vector2(points[2], points[3]);

                        Vector2 CP0 = QP0;
                        Vector2 CP3 = QP2;

                        //old
                        //Vector2 CP1 = new Vector2((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //Vector2 CP2 = new Vector2((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        //new
                        Vector2 CP1 = new Vector2(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        Vector2 CP2 = new Vector2(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));

                        //_gp.AddBezier(CP0, CP1, CP2, CP3);
                        //pb.AddCubicBezier(CP1, CP2, QP2);
                        pb.AddQuadraticBezier(QP1, QP2);

                        lastPoint = QP2;

                        lastControlPoint = QP1;
                    }
                    else if (action == 'q')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;


                        //convert quadratic to cubic bezier
                        Vector2 QP0 = lastPoint;
                        Vector2 QP1 = new Vector2(points[0], points[1]);
                        Vector2 QP2 = new Vector2(points[2], points[3]);

                        Vector2 CP0 = QP0;
                        Vector2 CP3 = QP2;

                        //old
                        //Vector2 CP1 = new Vector2((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //Vector2 CP2 = new Vector2((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);
                        //new
                        //Vector2 CP1 = new Vector2(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        //Vector2 CP2 = new Vector2(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));

                        //_gp.AddBezier(CP0, CP1, CP2, CP3);
                        //pb.AddCubicBezier(CP1, CP2, QP2);
                        pb.AddQuadraticBezier(QP1, QP2);


                        lastPoint = QP2;

                        lastControlPoint = QP1;
                    }
                    else if (action == 'T')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);

                        //convert quadratic to bezier
                        Vector2 QP0 = lastPoint;
                        Vector2 QP1 = mirrorControlPoint(lastControlPoint, QP0);
                        Vector2 QP2 = new Vector2(points[0], points[1]);

                        Vector2 CP0 = QP0;
                        Vector2 CP3 = QP2;

                        //Vector2 CP1 = new Vector2((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //Vector2 CP2 = new Vector2((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        //Vector2 CP1 = new Vector2(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        //Vector2 CP2 = new Vector2(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));


                        //_gp.AddBezier(CP0, CP1, CP2, CP3);
                        //pb.AddCubicBezier(CP1, CP2, QP2);
                        pb.AddQuadraticBezier(QP1, QP2);

                       
                        lastPoint = QP2;

                        lastControlPoint = QP1;
                    }
                    else if (action == 't')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;

                        //convert quadratic to bezier
                        Vector2 QP0 = lastPoint;
                        Vector2 QP1 = mirrorControlPoint(lastControlPoint, QP0);
                        Vector2 QP2 = new Vector2(points[0], points[1]);

                        Vector2 CP0 = QP0;
                        Vector2 CP3 = QP2;

                        //Vector2 CP1 = new Vector2((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //Vector2 CP2 = new Vector2((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        //Vector2 CP1 = new Vector2(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        //Vector2 CP2 = new Vector2(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));


                        //_gp.AddBezier(CP0, CP1, CP2, CP3);
                        //pb.AddCubicBezier(CP1, CP2, QP2);
                        pb.AddQuadraticBezier(QP1, QP2);


                        lastPoint = QP2;

                        lastControlPoint = QP1;
                    }
                    else if (action == 'A')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));//radiusX
                        points[1] = (float)Convert.ToDouble(values[1]);//radiusY
                        points[2] = (float)Convert.ToDouble(values[2]);//angle
                        points[3] = (float)Convert.ToDouble(values[3]);//size
                        points[4] = (float)Convert.ToDouble(values[4]);//sweep
                        points[5] = (float)Convert.ToDouble(values[5]);//endX
                        points[6] = -(float)Convert.ToDouble(values[6]);//endY

                        //SvgArcSize sas = points[3] == 0 ? SvgArcSize.Small : SvgArcSize.Large;
                        //SvgArcSweep sasw = points[4] == 0 ? SvgArcSweep.Negative : SvgArcSweep.Positive;
                        Vector2 endPoint = new Vector2(points[5], points[6]);

                        //SvgArcSegment arc = new SvgArcSegment(lastPoint, points[0], points[1], points[2], sas, sasw, endPoint);
                        //arc.AddToPath(_gp);

                        CanvasArcSize arcSize = CanvasArcSize.Large;//if size = 1, size is large.  0 for small
                        CanvasSweepDirection sweep = CanvasSweepDirection.Clockwise;//if sweep == 1, sweep is clockwise or positive

                        if(points[3]==0.0f)
                            arcSize = CanvasArcSize.Small;
                        if (points[4] == 0.0f)
                            sweep = CanvasSweepDirection.CounterClockwise;

                        pb.AddArc(new Vector2(points[5], points[6]), points[0], points[1], points[2], sweep, arcSize);


                        //AddArcToPath(_gp, lastPoint, points[0], points[1], points[2], (int)points[3], (int)points[4], endPoint);

                        lastPoint.X = points[5];
                        lastPoint.Y = points[6];
                    }
                    else if (action == 'a')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = (float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = (float)Convert.ToDouble(values[3]);
                        points[4] = (float)Convert.ToDouble(values[4]);
                        points[5] = (float)Convert.ToDouble(values[5] + lastPoint.X);
                        points[6] = -(float)Convert.ToDouble(values[6] + lastPoint.Y);

                        //SvgArcSize sas = points[3] == 0 ? SvgArcSize.Small : SvgArcSize.Large;
                        //SvgArcSweep sasw = points[4] == 0 ? SvgArcSweep.Negative : SvgArcSweep.Positive;
                        Vector2 endPoint = new Vector2(points[5], points[6]);

                        //SvgArcSegment arc = new SvgArcSegment(lastPoint, points[0], points[1], points[2], sas, sasw, endPoint);
                        //arc.AddToPath(_gp);

                        CanvasArcSize arcSize = CanvasArcSize.Large;//if size = 1, size is large.  0 for small
                        CanvasSweepDirection sweep = CanvasSweepDirection.Clockwise;//if sweep == 1, sweep is clockwise or positive

                        if (points[3] == 0.0f)
                            arcSize = CanvasArcSize.Small;
                        if (points[4] == 0.0f)
                            sweep = CanvasSweepDirection.CounterClockwise;

                        pb.AddArc(new Vector2(points[5], points[6]), points[0], points[1], points[2], sweep, arcSize);

                        //AddArcToPath(_gp, lastPoint, points[0], points[1], points[2], (int)points[3], (int)points[4], endPoint);

                        lastPoint.X = points[5];
                        lastPoint.Y = points[6];
                    }
                    else if (action == 'Z' || action == 'z')
                    {
                        pb.EndFigure(CanvasFigureLoop.Closed);
                    }
                }
                _cg = CanvasGeometry.CreatePath(pb);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
                
                ErrorLogger.LogException("SVGPath", "parsePath", exc);
            }



        }

        private Vector2 mirrorControlPoint(Vector2 cp, Vector2 endPoint)
        {
            
            float xOffset = endPoint.X - cp.X;
            float yOffset = endPoint.Y - cp.Y;

            Vector2 mirror1 = new Vector2(endPoint.X + xOffset, endPoint.Y + yOffset);

            //return mirror;//*/
            ///////////////////////////////////////////////////////////////////////
            Vector2 point = cp;
            Vector2 mirror = endPoint;

            float x, y, dx, dy;
            dx = Math.Abs(mirror.X - point.X);
            dy = Math.Abs(mirror.Y - point.Y);

            if(mirror.X >= point.X)
            {
                x = mirror.X + dx;
            }
            else
            {
                x = mirror.X - dx;
            }

            if (mirror.Y >= point.Y)
            {
                y = mirror.Y + dy;
            }
            else
            {
                y = mirror.Y - dy;
            }
            Vector2 mirror2 = new Vector2(x, y * -1f);//*/

            //Console.Out.WriteLine("mirror1: " + mirror1.ToString());
            //Console.Out.WriteLine("mirror2: " + mirror2.ToString());

            return mirror1;

        }

        /**
         * taken from https://svg.codeplex.com/ SvgArcSegment.cs
         * */
        /*private void AddArcToPath(GraphicsPath graphicsPath, Vector2 start, float radiusX, float radiusY, float angle, int size, int sweep, Vector2 end)
        {
            double RadiansPerDegree = Math.PI / 180.0;

            radiusX = Math.Abs(radiusX);
            radiusY = Math.Abs(radiusY);

            if (start == end)
            {
                return;
            }

            if (radiusX == 0.0f && radiusY == 0.0f)
            {
                graphicsPath.AddLine(start, end);
                return;
            }

            double sinPhi = Math.Sin(angle * RadiansPerDegree);
            double cosPhi = Math.Cos(angle * RadiansPerDegree);

            double x1dash = cosPhi * (start.X - end.X) / 2.0 + sinPhi * (start.Y - end.Y) / 2.0;
            double y1dash = -sinPhi * (start.X - end.X) / 2.0 + cosPhi * (start.Y - end.Y) / 2.0;

            double root;
            double numerator = radiusX * radiusX * radiusY * radiusY - radiusX * radiusX * y1dash * y1dash - radiusY * radiusY * x1dash * x1dash;

            float rx = radiusX;
            float ry = radiusY;

            if (numerator < 0.0)
            {
                float s = (float)Math.Sqrt(1.0 - numerator / (radiusX * radiusX * radiusY * radiusY));

                rx *= s;
                ry *= s;
                root = 0.0;
            }
            else
            {
                root = ((size == 1 && sweep == 1) || (size == 0 && sweep == 0) ? -1.0 : 1.0) * Math.Sqrt(numerator / (radiusX * radiusX * y1dash * y1dash + radiusY * radiusY * x1dash * x1dash));
            }

            double cxdash = root * rx * y1dash / ry;
            double cydash = -root * ry * x1dash / rx;

            double cx = cosPhi * cxdash - sinPhi * cydash + (start.X + end.X) / 2.0;
            double cy = sinPhi * cxdash + cosPhi * cydash + (start.Y + end.Y) / 2.0;

            double theta1 = CalculateVectorAngle(1.0, 0.0, (x1dash - cxdash) / rx, (y1dash - cydash) / ry);
            double dtheta = CalculateVectorAngle((x1dash - cxdash) / rx, (y1dash - cydash) / ry, (-x1dash - cxdash) / rx, (-y1dash - cydash) / ry);

            if (sweep == 0 && dtheta > 0)
            {
                dtheta -= 2.0 * Math.PI;
            }
            else if (sweep == 1 && dtheta < 0)
            {
                dtheta += 2.0 * Math.PI;
            }

            int segments = (int)Math.Ceiling((double)Math.Abs(dtheta / (Math.PI / 2.0)));
            double delta = dtheta / segments;
            double t = 8.0 / 3.0 * Math.Sin(delta / 4.0) * Math.Sin(delta / 4.0) / Math.Sin(delta / 2.0);

            double startX = start.X;
            double startY = start.Y;

            for (int i = 0; i < segments; ++i)
            {
                double cosTheta1 = Math.Cos(theta1);
                double sinTheta1 = Math.Sin(theta1);
                double theta2 = theta1 + delta;
                double cosTheta2 = Math.Cos(theta2);
                double sinTheta2 = Math.Sin(theta2);

                double endpointX = cosPhi * rx * cosTheta2 - sinPhi * ry * sinTheta2 + cx;
                double endpointY = sinPhi * rx * cosTheta2 + cosPhi * ry * sinTheta2 + cy;

                double dx1 = t * (-cosPhi * rx * sinTheta1 - sinPhi * ry * cosTheta1);
                double dy1 = t * (-sinPhi * rx * sinTheta1 + cosPhi * ry * cosTheta1);

                double dxe = t * (cosPhi * rx * sinTheta2 + sinPhi * ry * cosTheta2);
                double dye = t * (sinPhi * rx * sinTheta2 - cosPhi * ry * cosTheta2);

                graphicsPath.AddBezier((float)startX, (float)startY, (float)(startX + dx1), (float)(startY + dy1),
                    (float)(endpointX + dxe), (float)(endpointY + dye), (float)endpointX, (float)endpointY);

                theta1 = theta2;
                startX = (float)endpointX;
                startY = (float)endpointY;
            }

        }//*/

        /**
         * taken from https://svg.codeplex.com/ SvgArcSegment.cs
         * */
        private double CalculateVectorAngle(double ux, double uy, double vx, double vy)
        {
            double DoublePI = Math.PI * 2;
            double ta = Math.Atan2(uy, ux);
            double tb = Math.Atan2(vy, vx);

            if (tb >= ta)
            {
                return tb - ta;
            }

            return DoublePI - (ta - tb);
        }

        /**
         * Transforms the path to fit the given dimensions.
         * @return Matrix used to transform the path to fit the dimensions
         * */
        public Matrix3x2 TransformToFitDimensions(int width, int height, out Matrix3x2 mScale, out Matrix3x2 mTranslate)
        {
            float scale = 1f;

            Rect rect = _cg.ComputeBounds();//(testMatrix, testPen);

            mScale = new Matrix3x2();
            mTranslate = new Matrix3x2();

            float sx = (float)(width / rect.Width);
            float sy = (float)(height / rect.Height);
            if (sx < sy)
            {
                scale = sx;
            }
            else
            {
                scale = sy;
            }
            mScale = Matrix3x2.CreateScale(scale, scale);

            _cg = _cg.Transform(mScale);
            rect = _cg.ComputeBounds();//(testMatrix, testPen);

            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = (float)(rect.X * -1.0f);
            if (rect.Y < 0)
                transy = (float)(rect.Y * -1.0f);
            mTranslate = Matrix3x2.CreateTranslation(transx, transy);
            
            _cg = _cg.Transform(mTranslate);

            //return Matrix3x2.Add(mScale, mTranslate);
            return Matrix3x2.Multiply(mScale, mTranslate);

        }//

        public Matrix3x2 TransformToFitDimensions(int width, int height)
        {
            float scale = 1f;

            Rect rect = _cg.ComputeBounds();//(testMatrix, testPen);

            Matrix3x2 mScale = new Matrix3x2();
            Matrix3x2 mTranslate = new Matrix3x2();

            float sx = (float)(width / rect.Width);
            float sy = (float)(height / rect.Height);
            if (sx < sy)
            {
                scale = sx;
            }
            else
            {
                scale = sy;
            }
            mScale = Matrix3x2.CreateScale(scale, scale);

            _cg = _cg.Transform(mScale);
            rect = _cg.ComputeBounds();//(testMatrix, testPen);

            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = (float)(rect.X * -1.0f);
            if (rect.Y < 0)
                transy = (float)(rect.Y * -1.0f);
            mTranslate = Matrix3x2.CreateTranslation(transx, transy);

            _cg = _cg.Transform(mTranslate);

            //return Matrix3x2.Add(mScale, mTranslate);
            return Matrix3x2.Multiply(mScale, mTranslate);

        }//

        public void Transform(Matrix3x2 m)
        {
            _cg = _cg.Transform(m);
            _ccgFill = CanvasCachedGeometry.CreateFill(_cg);
            _bounds = _cg.ComputeBounds();
        }

        public void Draw(CanvasRenderTarget crt, Color lineColor, float lineWidth, Color fillColor, Matrix3x2 m)
        {
            //CanvasDevice device = CanvasDevice.GetSharedDevice();
            //CanvasRenderTarget offscreen = new CanvasRenderTarget(device, width, height, 96);
            using (CanvasDrawingSession ds = crt.CreateDrawingSession())
            {
                //ds.Clear(Colors.Transparent);
                //ds.DrawRectangle(100, 200, 5, 6, Colors.Red);
               // Debug.WriteLine(_cg.ComputeBounds().ToString());
                if (m.IsIdentity == false)
                {
                    _cg = _cg.Transform(m);
                    //Debug.WriteLine(_cg.ComputeBounds().ToString());
                }
                if (!(lineColor == Colors.Transparent))
                {
                    ds.DrawGeometry(_cg, lineColor,lineWidth);
                }

                if (fillColor != null)
                    ds.FillGeometry(_cg, fillColor);
            }
        }

        /// <summary>
        /// user needs to call "ds.Clear(Colors.Transparent);" before this function if necessary
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="lineColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="fillColor"></param>
        /// <param name="m"></param>
        public void Draw(CanvasDrawingSession ds, Color lineColor, float lineWidth, Color fillColor, Matrix3x2 m)
        {
            /*if (m != null)
                _cg = _cg.Transform(m);
            if (!(lineColor == Colors.Transparent))
            {
                ds.DrawGeometry(_cg, lineColor, lineWidth);
            }

            if (fillColor != null)
                ds.FillGeometry(_cg, fillColor);//*/

            ds.Transform = m;
            if (_ccgStroke != null && lineColor != Colors.Transparent)
                ds.DrawCachedGeometry(_ccgStroke, lineColor);
            if (_ccgFill != null && fillColor != Colors.Transparent)
                ds.DrawCachedGeometry(_ccgFill, fillColor);

            ds.Transform = Matrix3x2.Identity;

        }



        /**
         * Draws SVG to fit into a image of the specified dimensions
         * @deprecated
         * */
        public CanvasRenderTarget Draw(int width, int height, Color lineColor, Color fillColor)
        {
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            CanvasRenderTarget crt = new CanvasRenderTarget(device, width, height, 96);
            
            
            ////BitmapImage bmp = new BitmapImage();
            ////SoftwareBitmap sb = new SoftwareBitmap(BitmapPixelFormat.Rgba8, width, height);

            //WriteableBitmap wbmp = new WriteableBitmap(width, height);
            //crt.GetPixelBytes(wbmp.PixelBuffer);

            Rect rect = _cg.ComputeBounds();
            Matrix3x2 m = new Matrix3x2();

            float sx = (float)(width / rect.Width);
            float sy = (float)(height / rect.Height);
            if (sx < sy)
                m = Matrix3x2.CreateScale(sx, sx);
            else
                m = Matrix3x2.CreateScale(sy, sy);

            _cg.Transform(m);
            rect = _cg.ComputeBounds();
            m = new Matrix3x2();
            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = (float)rect.X * -1.0f;
            if (rect.Y < 0)
                transy = (float)rect.Y * -1.0f;
            m = Matrix3x2.CreateTranslation(transx, transy);



            _cg.Transform(m);
            rect = _cg.ComputeBounds();

            //Debug.WriteLine(rect.ToString());
            using (CanvasDrawingSession ds = crt.CreateDrawingSession())
            {
                ds.Clear(Colors.Transparent);
                ds.DrawGeometry(_cg, Colors.Red, 1f);

                Color c = Color.FromArgb(128, 0, 255, 255);
                ds.FillGeometry(_cg, c);
                ds.DrawRectangle(0, 0, width - 1, height - 1,Colors.Green);
            }

            return crt;
        }//*/

        ~SVGPath()
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
                if (disposing == true && _cg != null)
                {
                    _cg.Dispose();
                    _cg = null;
                }
                if (disposing == true && _ccgStroke != null)
                {
                    _ccgStroke.Dispose();
                    _ccgStroke = null;
                }
                if (disposing == true && _ccgFill != null)
                {
                    _ccgFill.Dispose();
                    _ccgFill = null;
                }
                disposed = true;
            }
        }

        /// <summary>
        /// Creates a scale & translation matrix
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bounds"></param>
        /// <param name="outBounds"></param>
        /// <returns></returns>
        public Matrix3x2 CreateMatrix(int width, int height, out Rect outBounds, out Matrix3x2 mScale, out Matrix3x2 mTranslate)
        {
            float scale = 1f;

            Rect rect = new Rect(_bounds.X, _bounds.Y, _bounds.Width, _bounds.Height); //_cg.ComputeBounds();//(testMatrix, testPen);

            mScale = new Matrix3x2();
            mTranslate = new Matrix3x2();

            float sx = (float)(width / rect.Width);
            float sy = (float)(height / rect.Height);
            if (sx < sy)
            {
                scale = sx;
            }
            else
            {
                scale = sy;
            }
            mScale = Matrix3x2.CreateScale(scale, scale);

            outBounds = new Rect(0, 0, _bounds.Width * scale, _bounds.Height * scale);

            _cgBounds = CanvasGeometry.CreateRectangle(_cg.Device, _bounds);
            _cgBounds = _cgBounds.Transform(mScale);
            rect = _cgBounds.ComputeBounds();


            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = (float)(rect.X * -1.0f);
            if (rect.Y < 0)
                transy = (float)(rect.Y * -1.0f);
            mTranslate = Matrix3x2.CreateTranslation(transx, transy);

            

            //return Matrix3x2.Add(mScale, mTranslate);
            return Matrix3x2.Multiply(mScale, mTranslate);

        }//
    }
}
