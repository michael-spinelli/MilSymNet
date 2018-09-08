using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Drawing;
using MilSymNetUtilities;

namespace MilSymNet
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

        private GraphicsPath _gp = null;
        // Track whether Dispose has been called.
        private bool disposed = false;

        public int getID()
        {
            return _ID;
        }

        /**
         * Returns bounds of the core symbol
         * */
        public RectangleF getBounds()
        {
            return _gp.GetBounds();
        }

        public RectangleF getBounds(Pen pen)
        {
            return _gp.GetBounds(new Matrix(), pen);
        }

        /*
         * Returns bounds of the symbol when it's being outlined.
         * */
        public RectangleF getBounds(float outlineWidth)
        {
            Pen testPen = new Pen(Color.Red, outlineWidth);
            testPen.MiterLimit = 1f;
            testPen.LineJoin = LineJoin.Miter;
            return _gp.GetBounds(new Matrix(), testPen);
        }

        private SVGPath()
        {
        }

        public SVGPath(SVGPath path)
        {
            _gp = (GraphicsPath)path._gp.Clone();
            _ID = path._ID;
        }

        public SVGPath(String unicodeHex, String path)
        {
            _ID = int.Parse(unicodeHex.Replace(";", ""), System.Globalization.NumberStyles.HexNumber);
            _gp = new GraphicsPath();
            parsePath(path);
        }

        /*public override object Clone()
        {
            SVGPath clone = new SVGPath();
            clone._gp = (GraphicsPath)_gp.Clone();
            clone._ID = _ID;
            clone._strPath = (String)_strPath.Clone();
            return clone;
        }*/

        private void parsePath(String path)
        {
            char[] delimiter = { ' ' };
            string[] commands = Regex.Split(path, _regex1);
            string[] values = null;
            float[] points = new float[7];
            PointF[] pointFs = new PointF[4];
            PointF lastPoint = new PointF(0, 0);
            PointF lastControlPoint = new PointF(0, 0);
            //PointF firstPoint = new PointF(0, 0);
            //PointF firstPoint = new PointF(0, 0);

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
                        lastPoint = new PointF(points[0], points[1]);
                        //firstPoint = new PointF(points[0], points[1]);
                        _gp.StartFigure();
                    }
                    else if (action == 'm')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;

                        lastPoint = new PointF(points[0], points[1]);
                        _gp.StartFigure();
                    }
                    else if (action == 'L')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        _gp.AddLine(lastPoint.X, lastPoint.Y, points[0], points[1]);
                        lastPoint = _gp.GetLastPoint();

                    }
                    else if (action == 'l')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        _gp.AddLine(lastPoint.X, lastPoint.Y, points[0], points[1]);
                        lastPoint = _gp.GetLastPoint();
                    }
                    else if (action == 'H')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        _gp.AddLine(lastPoint.X, lastPoint.Y, points[0], lastPoint.Y);
                        lastPoint = _gp.GetLastPoint();

                    }
                    else if (action == 'h')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        _gp.AddLine(lastPoint.X, lastPoint.Y, points[0], lastPoint.Y);
                        lastPoint = _gp.GetLastPoint();
                    }
                    else if (action == 'V')
                    {
                        points[0] = -(float)Convert.ToDouble(values[0].Substring(1));
                        _gp.AddLine(lastPoint.X, lastPoint.Y, lastPoint.X, points[0]);
                        lastPoint = _gp.GetLastPoint();

                    }
                    else if (action == 'v')
                    {
                        points[0] = -(float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.Y;
                        _gp.AddLine(lastPoint.X, lastPoint.Y, lastPoint.X, points[0]);
                        lastPoint = _gp.GetLastPoint();
                    }
                    else if (action == 'C')//cubic bezier, 2 control points
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);
                        points[4] = (float)Convert.ToDouble(values[4]);
                        points[5] = -(float)Convert.ToDouble(values[5]);

                        pointFs[0] = lastPoint;
                        pointFs[1] = new PointF(points[0], points[1]);
                        pointFs[2] = new PointF(points[2], points[3]);
                        pointFs[3] = new PointF(points[4], points[5]);
                        //_gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);
                        //lastPoint = new PointF(points[4], points[5]);

                        _gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);

                        //SvgCubicCurveSegment sccs = new SvgCubicCurveSegment(_gp.GetLastPoint(), pointFs[1], pointFs[2], pointFs[3]);
                        //sccs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();
                        lastControlPoint = pointFs[2];
                    }
                    else if (action == 'c')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;
                        points[4] = (float)Convert.ToDouble(values[4]) + lastPoint.X;
                        points[5] = -(float)Convert.ToDouble(values[5]) + lastPoint.Y;

                        pointFs[0] = lastPoint;
                        pointFs[1] = new PointF(points[0], points[1]);
                        pointFs[2] = new PointF(points[2], points[3]);
                        pointFs[3] = new PointF(points[4], points[5]);
                        //_gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);

                        _gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);

                        //SvgCubicCurveSegment sccs = new SvgCubicCurveSegment(_gp.GetLastPoint(), pointFs[1], pointFs[2], pointFs[3]);
                        //sccs.AddToPath(_gp);
                        //lastPoint = new PointF(points[4], points[5]);
                        lastPoint = _gp.GetLastPoint();
                        lastControlPoint = pointFs[2];

                    }
                    else if (action == 'S')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);

                        pointFs[0] = lastPoint;
                        pointFs[1] = mirrorControlPoint(lastControlPoint, lastPoint);
                        pointFs[2] = new PointF(points[0], points[1]);
                        pointFs[3] = new PointF(points[2], points[3]);
                        //_gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);
                        //lastPoint = new PointF(points[4], points[5]);

                        _gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);

                        //SvgCubicCurveSegment sccs = new SvgCubicCurveSegment(_gp.GetLastPoint(), pointFs[1], pointFs[2], pointFs[3]);
                        //sccs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();
                        lastControlPoint = pointFs[2];
                    }
                    else if (action == 's')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;

                        pointFs[0] = lastPoint;
                        pointFs[1] = mirrorControlPoint(lastControlPoint, lastPoint);
                        pointFs[2] = new PointF(points[0], points[1]);
                        pointFs[3] = new PointF(points[2], points[3]);
                        //_gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);
                        //lastPoint = new PointF(points[4], points[5]);

                        _gp.AddBezier(pointFs[0], pointFs[1], pointFs[2], pointFs[3]);

                        //SvgCubicCurveSegment sccs = new SvgCubicCurveSegment(_gp.GetLastPoint(), pointFs[1], pointFs[2], pointFs[3]);
                        //sccs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();
                        lastControlPoint = pointFs[2];
                    }
                    else if (action == 'Q')//quadratic bezier, 1 control point
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = -(float)Convert.ToDouble(values[3]);


                        //convert quadratic to cubic bezier
                        PointF QP0 = lastPoint;
                        PointF QP1 = new PointF(points[0], points[1]);
                        PointF QP2 = new PointF(points[2], points[3]);

                        PointF CP0 = QP0;
                        PointF CP3 = QP2;

                        //PointF CP1 = new PointF((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //PointF CP2 = new PointF((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        PointF CP1 = new PointF(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        PointF CP2 = new PointF(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));

                        _gp.AddBezier(CP0, CP1, CP2, CP3);

                        //SvgQuadraticCurveSegment qcs = new SvgQuadraticCurveSegment(QP0, QP1, QP2);
                        //qcs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();

                        lastControlPoint = QP1;
                    }
                    else if (action == 'q')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;
                        points[2] = (float)Convert.ToDouble(values[2]) + lastPoint.X;
                        points[3] = -(float)Convert.ToDouble(values[3]) + lastPoint.Y;


                        //convert quadratic to cubic bezier
                        PointF QP0 = lastPoint;
                        PointF QP1 = new PointF(points[0], points[1]);
                        PointF QP2 = new PointF(points[2], points[3]);

                        PointF CP0 = QP0;
                        PointF CP3 = QP2;

                        //PointF CP1 = new PointF((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //PointF CP2 = new PointF((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        PointF CP1 = new PointF(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        PointF CP2 = new PointF(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));

                        _gp.AddBezier(CP0, CP1, CP2, CP3);


                        //SvgQuadraticCurveSegment qcs = new SvgQuadraticCurveSegment(QP0, QP1, QP2);
                        //qcs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();

                        lastControlPoint = QP1;
                    }
                    else if (action == 'T')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = -(float)Convert.ToDouble(values[1]);

                        //convert quadratic to bezier
                        PointF QP0 = lastPoint;
                        PointF QP1 = mirrorControlPoint(lastControlPoint, QP0);
                        PointF QP2 = new PointF(points[0], points[1]);

                        PointF CP0 = QP0;
                        PointF CP3 = QP2;

                        //PointF CP1 = new PointF((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //PointF CP2 = new PointF((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        PointF CP1 = new PointF(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        PointF CP2 = new PointF(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));


                        _gp.AddBezier(CP0, CP1, CP2, CP3);

                        //SvgQuadraticCurveSegment qcs = new SvgQuadraticCurveSegment(QP0, QP1, QP2);
                        //qcs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();

                        lastControlPoint = QP1;
                    }
                    else if (action == 't')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1)) + lastPoint.X;
                        points[1] = -(float)Convert.ToDouble(values[1]) + lastPoint.Y;

                        //convert quadratic to bezier
                        PointF QP0 = lastPoint;
                        PointF QP1 = mirrorControlPoint(lastControlPoint, QP0);
                        PointF QP2 = new PointF(points[0], points[1]);

                        PointF CP0 = QP0;
                        PointF CP3 = QP2;

                        //PointF CP1 = new PointF((QP0.X + 2.0f * QP1.X) / 3.0f, (QP0.Y + 2.0f * QP1.Y) / 3.0f);
                        //PointF CP2 = new PointF((QP2.X + 2.0f * QP1.X) / 3.0f, (QP2.Y + 2.0f * QP1.Y) / 3.0f);

                        PointF CP1 = new PointF(QP0.X + 2.0f / 3.0f * (QP1.X - QP0.X), QP0.Y + 2.0f / 3.0f * (QP1.Y - QP0.Y));
                        PointF CP2 = new PointF(QP2.X + 2.0f / 3.0f * (QP1.X - QP2.X), QP2.Y + 2.0f / 3.0f * (QP1.Y - QP2.Y));


                        _gp.AddBezier(CP0, CP1, CP2, CP3);

                        //SvgQuadraticCurveSegment qcs = new SvgQuadraticCurveSegment(QP0, QP1, QP2);
                        //qcs.AddToPath(_gp);
                        lastPoint = _gp.GetLastPoint();

                        lastControlPoint = QP1;
                    }
                    else if (action == 'A')
                    {
                        points[0] = (float)Convert.ToDouble(values[0].Substring(1));
                        points[1] = (float)Convert.ToDouble(values[1]);
                        points[2] = (float)Convert.ToDouble(values[2]);
                        points[3] = (float)Convert.ToDouble(values[3]);
                        points[4] = (float)Convert.ToDouble(values[4]);
                        points[5] = (float)Convert.ToDouble(values[5]);
                        points[6] = -(float)Convert.ToDouble(values[6]);

                        //SvgArcSize sas = points[3] == 0 ? SvgArcSize.Small : SvgArcSize.Large;
                        //SvgArcSweep sasw = points[4] == 0 ? SvgArcSweep.Negative : SvgArcSweep.Positive;
                        PointF endPoint = new PointF(points[5], points[6]);

                        //SvgArcSegment arc = new SvgArcSegment(lastPoint, points[0], points[1], points[2], sas, sasw, endPoint);
                        //arc.AddToPath(_gp);

                        AddArcToPath(_gp, lastPoint, points[0], points[1], points[2], (int)points[3], (int)points[4], endPoint);

                        lastPoint = _gp.GetLastPoint();
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
                        PointF endPoint = new PointF(points[5], points[6]);

                        //SvgArcSegment arc = new SvgArcSegment(lastPoint, points[0], points[1], points[2], sas, sasw, endPoint);
                        //arc.AddToPath(_gp);

                        AddArcToPath(_gp, lastPoint, points[0], points[1], points[2], (int)points[3], (int)points[4], endPoint);

                        lastPoint = _gp.GetLastPoint();
                    }
                    else if (action == 'Z' || action == 'z')
                    {
                        _gp.CloseFigure();
                    }

                    //Matrix verticalFlip = new Matrix();
                    //verticalFlip.Scale(1, -1);
                    //_gp.Transform(verticalFlip);
                    

                }
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SVGPath", "parsePath", exc);
            }



        }

        private PointF mirrorControlPoint(PointF cp, PointF endPoint)
        {
            
            float xOffset = endPoint.X - cp.X;
            float yOffset = endPoint.Y - cp.Y;

            PointF mirror1 = new PointF(endPoint.X + xOffset, endPoint.Y + yOffset);

            //return mirror;//*/
            ///////////////////////////////////////////////////////////////////////
            PointF point = cp;
            PointF mirror = endPoint;

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
            PointF mirror2 = new PointF(x, y * -1f);//*/

            //Console.Out.WriteLine("mirror1: " + mirror1.ToString());
            //Console.Out.WriteLine("mirror2: " + mirror2.ToString());

            return mirror1;

        }

        /**
         * taken from https://svg.codeplex.com/ SvgArcSegment.cs
         * */
        private void AddArcToPath(GraphicsPath graphicsPath, PointF start, float radiusX, float radiusY, float angle, int size, int sweep, PointF end)
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

        }

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
        public Matrix TransformToFitDimensions(int width, int height)
        {
            Pen testPen = new Pen(Color.Red, 0f);
            testPen.MiterLimit = 1f;
            testPen.LineJoin = LineJoin.Round;
            Matrix testMatrix = new Matrix();

            RectangleF rect = _gp.GetBounds();//(testMatrix, testPen);
            Matrix m = new Matrix();
            Matrix mScale = new Matrix();
            Matrix mTranslate = new Matrix();


            float sx = width / rect.Width;
            float sy = height / rect.Height;
            if (sx < sy)
            {
                mScale.Scale(sx, sx);
                m.Scale(sx, sx);
            }
            else
            {
                mScale.Scale(sy, sy);
                m.Scale(sy, sy);
            }

            _gp.Transform(mScale);
            rect = _gp.GetBounds();//(testMatrix, testPen);

            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = rect.X * -1.0f;
            if (rect.Y < 0)
                transy = rect.Y * -1.0f;
            mTranslate.Translate(transx, transy);
            m.Translate(transx, transy, MatrixOrder.Append);
            _gp.Transform(mTranslate);

            return m;
        }

        public void Transform(Matrix m)
        {
            _gp.Transform(m);
        }

        public void Draw(Graphics g, Color lineColor, float lineWidth, Color fillColor, Matrix m)
        {
            if (m != null)
                _gp.Transform(m);
            if (lineColor.IsEmpty == false)
            {
                Pen pen = new Pen(lineColor, lineWidth);
                pen.MiterLimit = 3;
                g.DrawPath(pen, _gp);
            }

            if (fillColor != null)
                g.FillPath(new SolidBrush(fillColor), _gp);
        }

        /**
         * Draws SVG to fit into a image of the specified dimensions
         * */
        public Image Draw(int width, int height, Pen lineColor, Pen fillColor)
        {
            Image foo = null;

            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);

            RectangleF rect = _gp.GetBounds();
            Matrix m = new Matrix();

            float sx = width / rect.Width;
            float sy = height / rect.Height;
            if (sx < sy)
                m.Scale(sx, sx);
            else
                m.Scale(sy, sy);

            _gp.Transform(m);
            rect = _gp.GetBounds();
            m = new Matrix();
            float transx = 0;
            float transy = 0;
            if (rect.X < 0)
                transx = rect.X * -1.0f;
            if (rect.Y < 0)
                transy = rect.Y * -1.0f;
            m.Translate(transx, transy);
            //m.Translate(300,300);

            _gp.Transform(m);
            rect = _gp.GetBounds();

            //Console.WriteLine(rect.ToString());

            g.DrawPath(new Pen(Color.Red), _gp);
            Color c = Color.FromArgb(128, 0, 255, 255);
            Pen p = new Pen(c);
            Brush b = new SolidBrush(c);
            g.FillPath(b, _gp);
            g.DrawRectangle(new Pen(Color.Green), 0, 0, width - 1, height - 1);
            foo = (Image)bmp;
            return foo;
        }

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
                if (disposing == true && _gp != null)
                {
                    _gp.Dispose();
                    _gp = null;
                }
                disposed = true;
            }
        }
    }
}
