using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using MilSymNetUtilities;

namespace MilSymUwp
{
    class ModifierRenderer
    {

        private static RendererSettings RS = RendererSettings.getInstance();
        //private static Bitmap _textBMP = new Bitmap(2, 2);
        //private static Graphics _g = Graphics.FromImage(_textBMP);
        //private static Matrix _identityMatrix = new Matrix();
        //private static int _fontSize = RS.getModifierFontSize();
        //private static FontStyle _fontStyle = RS.getModifierFontStyle();
        //private static String _fontName = RS.getModifierFontName();
        //private static int _modifierFontHeight = ShapeUtilities.round(_g.MeasureString("Hj", RS.getLabelFont()).Height);
        //private static Font _modifierFont = RS.getLabelFont();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolID"></param>
        /// <param name="msb"></param>
        /// <param name="modifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static ImageInfo ProcessUnitDisplayModifiers(String symbolID, ImageInfo ii, Dictionary<int, String> modifiers, Dictionary<int, String> attributes, Boolean hasTextModifiers, CanvasDevice device)
        {
            ImageInfo newii = null;
            Rect symbolBounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Rect imageBounds = ShapeUtilities.clone(ii.getCanvasRenderTarget().GetBounds(device));
            Point centerPoint = ShapeUtilities.clone(ii.getAnchorPoint());
            TextInfo tiEchelon = null;
            TextInfo tiAM = null;
            Rect echelonBounds = Rect.Empty;
            Rect amBounds = Rect.Empty;
            Color textColor = Colors.Black;
            Color textBackgroundColor = Colors.Transparent;
            int buffer = 0;
            //ctx = null;
            int offsetX = 0;
            int offsetY = 0;
            int symStd = RS.getSymbologyStandard();
            CanvasTextFormat font = RS.getLabelFont();
            /*Pen mobilityPen = new Pen(Colors.Black, 2f);
            mobilityPen.MiterLimit = 2;//*/

            try
            {
                if(device == null)
                {
                    device = CanvasDevice.GetSharedDevice();
                }

                if (attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
                {
                    symStd = Convert.ToInt16(attributes[MilStdAttributes.SymbologyStandard]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextColor))
                {
                    textColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextColor]));
                }
                if (attributes.ContainsKey(MilStdAttributes.TextBackgroundColor))
                {
                    textBackgroundColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextBackgroundColor]));
                }

                #region Build Mobility Modifiers

                Rect mobilityBounds = Rect.Empty;

                List<CanvasGeometry> shapes = new List<CanvasGeometry>();
                CanvasPathBuilder mobilityPath = null;
                CanvasPathBuilder mobilityPathFill = null;
                CanvasGeometry cgMobilityPath = null;
                CanvasGeometry cgMobilityPathFill = null;
                if (symbolID[10] == ('M') || symbolID[10] == ('N'))
                {

                    //Draw Mobility
                    int fifth = (int)((symbolBounds.Width * 0.2) + 0.5f);
                    mobilityPath = new CanvasPathBuilder(device);
                    int x = 0;
                    int y = 0;
                    int centerX = 0;
                    int bottomY = 0;
                    int height = 0;
                    int width = 0;
                    int middleY = 0;
                    int wheelOffset = 2;
                    int wheelSize = fifth;//10;
                    int wheelRadius = fifth/2;//10;
                    int rrHeight = fifth;//10;
                    int rrArcWidth = (int)((fifth * 1.5) + 0.5f);//16;

                    String mobility = symbolID.Substring(10, 2);
                    x = (int)symbolBounds.Left;
                    y = (int)symbolBounds.Top;
                    height = (int)(symbolBounds.Height);
                    width = (int)(symbolBounds.Width);
                    bottomY = y + height + 2;

                    if (symbolID[10] == ('M'))
                    {
                        bottomY = y + height + 2;

                        //wheelSize = width / 7;
                        //rrHeight = width / 7;
                        //rrArcWidth = width / 7;
                        if (mobility.Equals("MO"))
                        {
                            //line
                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddLine(x + width, bottomY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device,new Vector2(x + 1 + wheelRadius, bottomY + wheelRadius + wheelOffset), wheelRadius, wheelRadius));
                            //mobilityPath.AddEllipse(x, bottomY + wheelOffset, wheelSize, wheelSize);


                            //right circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new System.Numerics.Vector2(x + width - wheelRadius, bottomY + wheelRadius + wheelOffset), wheelRadius, wheelRadius));
                            //mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                        }
                        else if (mobility.Equals("MP"))
                        {
                            //line
                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddLine(x + width, bottomY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //PathUtilties.AddLine(mobilityPath, x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x, bottomY + wheelOffset), wheelSize, wheelSize));

                            //right circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + width - wheelSize, bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //center wheel
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + (width / 2) - (wheelSize / 2), bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + (width / 2) - (wheelSize / 2), bottomY + wheelOffset, wheelSize, wheelSize);
                            
                        }
                        else if (mobility.Equals("MQ"))
                        {
                            //round Rect
                            mobilityPath.AddGeometry(CanvasGeometry.CreateRoundedRectangle(device, x, bottomY, width, rrHeight, rrHeight / 2, rrHeight / 2));
                            //mobilityPath.AddPath(ShapeUtilities.createRoundedRect(new Rect(x, bottomY, width, rrHeight), rrHeight / 2), false);

                        }
                        else if (mobility.Equals("MR"))
                        {
                            //round Rect
                            mobilityPath.AddGeometry(CanvasGeometry.CreateRoundedRectangle(device, x, bottomY, width, rrHeight, rrHeight / 2, rrHeight / 2));
                            //mobilityPath.AddPath(ShapeUtilities.createRoundedRect(new Rect(x, bottomY, width, rrHeight), wheelSize / 2), false);

                            //left circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x - wheelSize - wheelSize, bottomY), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x - wheelSize - wheelSize, bottomY, wheelSize, wheelSize);
                            
                        }
                        else if (mobility.Equals("MS"))
                        {
                            //line
                            mobilityPath.BeginFigure(x + wheelSize, bottomY + (wheelSize / 2));
                            mobilityPath.AddLine(x + width - wheelSize, bottomY + (wheelSize / 2));
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            /*mobilityPath.AddLine(x + wheelSize, bottomY + (wheelSize / 2),
                                    x + width - wheelSize, bottomY + (wheelSize / 2));//*/


                            //left circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x, bottomY), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x, bottomY, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + width - wheelSize, bottomY), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + width - wheelSize, bottomY, wheelSize, wheelSize);

                        }
                        else if (mobility.Equals("MT"))
                        {

                            //line
                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddLine(x + width, bottomY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + wheelSize, bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //left circle2
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x, bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + width - wheelSize, bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle2
                            mobilityPath.AddGeometry(CanvasGeometry.CreateEllipse(device, new Vector2(x + width - wheelSize - wheelSize, bottomY + wheelOffset), wheelSize, wheelSize));
                            //mobilityPath.AddEllipse(x + width - wheelSize - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                        }
                        else if (mobility.Equals("MU"))
                        {
                            float halfWidth = (rrArcWidth * 0.5f);
                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddLine(x + halfWidth, bottomY + halfWidth);
                            mobilityPath.AddLine(x + width, bottomY + halfWidth);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                           /* mobilityPath.AddLine(x, bottomY, x + halfWidth, bottomY + halfWidth);
                            mobilityPath.AddLine(x + halfWidth, bottomY + halfWidth, x + width, bottomY + halfWidth);//*/
                        }
                        else if (mobility.Equals("MV"))
                        {
                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddCubicBezier(new Vector2(x, bottomY), new Vector2(x - rrHeight, bottomY + rrHeight / 2), new Vector2(x, bottomY + rrHeight));
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddBezier(x, bottomY, x, bottomY, x - rrHeight, bottomY + rrHeight / 2, x, bottomY + rrHeight);

                            mobilityPath.BeginFigure(x, bottomY + rrHeight);
                            mobilityPath.AddLine(x + width, bottomY + rrHeight);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(x, bottomY + rrHeight, x + width, bottomY + rrHeight);


                            mobilityPath.BeginFigure(x + width, bottomY + rrHeight);
                            mobilityPath.AddCubicBezier(new Vector2(x + width, bottomY + rrHeight), new Vector2(x + width + rrHeight, bottomY + rrHeight / 2), new Vector2(x + width, bottomY));
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddBezier(x + width, bottomY + rrHeight, x + width, bottomY + rrHeight, x + width + rrHeight, bottomY + rrHeight / 2, x + width, bottomY);

                        }
                        else if (mobility.Equals("MW"))
                        {
                            centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);
                            int angleWidth = rrHeight / 2;

                            mobilityPath.BeginFigure(centerX, bottomY + rrHeight + 2);
                            mobilityPath.AddLine(centerX - angleWidth, bottomY);
                            mobilityPath.AddLine(centerX - angleWidth * 2, bottomY + rrHeight + 2);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(centerX, bottomY + rrHeight + 2, centerX - angleWidth, bottomY);
                            //mobilityPath.AddLine(centerX - angleWidth, bottomY, centerX - angleWidth * 2, bottomY + rrHeight + 2);

                            mobilityPath.BeginFigure(centerX, bottomY + rrHeight + 2);
                            mobilityPath.AddLine(centerX + angleWidth, bottomY);
                            mobilityPath.AddLine(centerX + angleWidth * 2, bottomY + rrHeight + 2);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.StartFigure();
                            //mobilityPath.AddLine(centerX, bottomY + rrHeight + 2, centerX + angleWidth, bottomY);
                            //mobilityPath.AddLine(centerX + angleWidth, bottomY, centerX + angleWidth * 2, bottomY + rrHeight + 2);

                        }
                        else if (mobility.Equals("MX"))
                        {
                            centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);

                            mobilityPath.BeginFigure(x + width, bottomY);
                            mobilityPath.AddLine(x, bottomY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(x + width, bottomY, x, bottomY);


                            float quarterX = (centerX - x) / 2;
                            ////var quarterY = (((bottomY + rrHeight) - bottomY)/2);

                            mobilityPath.BeginFigure(x, bottomY);
                            mobilityPath.AddCubicBezier(new Vector2(x + quarterX, bottomY + rrHeight), new Vector2(centerX + quarterX, bottomY + rrHeight), new Vector2(x + width, bottomY));
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddBezier(x, bottomY, x + quarterX, bottomY + rrHeight, centerX + quarterX, bottomY + rrHeight, x + width, bottomY);

                        }

                        else if (mobility.Equals("MY"))
                        {
                            float incrementX = width / 7f;

                            x = (int)Math.Floor(symbolBounds.X);
                            float r = incrementX;

                            //mobilityPath.arcTo(oval, sAngle, sAngle, moveTo);
                            mobilityPath.AddArc(new Vector2(x, bottomY), r, r, 180, 180);
                            mobilityPath.AddArc(new Vector2(x + incrementX, bottomY), r, r, 180, -180);
                            mobilityPath.AddArc(new Vector2(x + incrementX * 2, bottomY), r, r, 180, 180);
                            mobilityPath.AddArc(new Vector2(x + incrementX * 3, bottomY), r, r, 180, -180);
                            mobilityPath.AddArc(new Vector2(x + incrementX * 4, bottomY), r, r, 180, 180);
                            mobilityPath.AddArc(new Vector2(x + incrementX * 5, bottomY), r, r, 180, -180);
                            mobilityPath.AddArc(new Vector2(x + incrementX * 6, bottomY), r, r, 180, 180);

                        }

                    }
                    //Draw Towed Array Sonar
                    else if (symbolID[10] == ('N'))
                    {

                        int boxHeight = (int)((rrHeight * 0.8f) + 0.5f);
                        bottomY = y + height + (boxHeight / 7);
                        mobilityPathFill = new CanvasPathBuilder(device);
                        offsetY = ShapeUtilities.round(boxHeight / 6);//1;
                        centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);
                        int squareOffset = (int)((boxHeight * 0.5f) + 0.5);
                        middleY = ((boxHeight / 2) + bottomY) + offsetY;//+1 for offset from symbol
                        if (symbolID.Substring(10, 2).Equals("NS"))
                        {
                            //subtract 0.5 becase lines 1 pixel thick get aliased into
                            //a line two pixels wide.
                            //line
                            mobilityPath.BeginFigure(centerX - 1, bottomY - 1);
                            mobilityPath.AddLine(centerX - 1, bottomY + boxHeight + offsetY + 2);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(centerX - 1, bottomY - 1, centerX - 1, bottomY + boxHeight + offsetY + 2);


                            //line
                            mobilityPath.BeginFigure(x, middleY);
                            mobilityPath.AddLine(x + width, middleY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.StartFigure();
                            //mobilityPath.AddLine(x, middleY, x + width, middleY);


                            //square
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));


                            //square
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                            //square
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                        }
                        else if (symbolID.Substring(10, 2).Equals("NL"))
                        {
                            int leftX = x + (centerX - x) / 2,
                                    rightX = centerX + (x + width - centerX) / 2;

                            //line vertical left
                            mobilityPath.BeginFigure(leftX, bottomY - 1);
                            mobilityPath.AddLine(leftX, bottomY + offsetY + boxHeight + offsetY + 2);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.AddLine(leftX, bottomY - 1, leftX, bottomY + offsetY + boxHeight + offsetY + 2);


                            //line vertical right
                            mobilityPath.BeginFigure(rightX, bottomY - 1);
                            mobilityPath.AddLine(rightX, bottomY + offsetY + boxHeight + offsetY + 2);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.StartFigure();
                            //mobilityPath.AddLine(rightX, bottomY - 1, rightX, bottomY + offsetY + boxHeight + offsetY + 2);


                            //line horizontal
                            mobilityPath.BeginFigure(x, middleY);
                            mobilityPath.AddLine(x + width, middleY);
                            mobilityPath.EndFigure(CanvasFigureLoop.Open);
                            //mobilityPath.StartFigure();
                            //mobilityPath.AddLine(x, middleY, x + width, middleY);
                            

                            //square left
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                            //square middle
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                            //square right
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                            //square middle left
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, leftX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(leftX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));

                            //square middle right
                            mobilityPathFill.AddGeometry(CanvasGeometry.CreateRectangle(device, rightX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(new Rect(rightX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));


                        }
                    }

                    //get mobility bounds
                    if (mobilityPath != null)
                    {

                        //build mobility bounds
                        cgMobilityPath = CanvasGeometry.CreatePath(mobilityPath);
                        mobilityBounds = cgMobilityPath.ComputeBounds();


                        Rect mobilityFillBounds = new Rect();
                        if (mobilityPathFill != null)
                        {
                            cgMobilityPathFill = CanvasGeometry.CreatePath(mobilityPathFill);
                            mobilityFillBounds = cgMobilityPathFill.ComputeBounds();
                        }

                        //grow because we use a line thickness of 2.
                        mobilityBounds = new Rect(mobilityBounds.X - 1, mobilityBounds.Y - 1, mobilityBounds.Width+2, mobilityBounds.Height+2);
                        //mobilityBounds.Inflate(1f, 1f);
                        imageBounds = ShapeUtilities.union(imageBounds, mobilityBounds);
                    }
                }

                #endregion

                #region Build Echelon
                //Draw Echelon
                String strEchelon = SymbolUtilities.getEchelon(symbolID);//symbolID.substring(11, 12);
                if (strEchelon != null)
                {
                    strEchelon = SymbolUtilities.getEchelonText(strEchelon);
                }
                if (strEchelon != null && SymbolUtilities.hasInstallationModifier(symbolID) == false
                        && SymbolUtilities.canUnitHaveModifier(symbolID, ModifiersUnits.B_ECHELON))
                {

                    if (strEchelon != null)
                    {
                        int outlineOffset = RS.getTextOutlineWidth();

                        //tiEchelon = new TextInfo(strEchelon, 0, 0, font, _g);
                        tiEchelon = new TextInfo(device, strEchelon);
                        
                        echelonBounds = tiEchelon.getTextBounds();

                        int y = (int)Math.Round(symbolBounds.Top - echelonBounds.Height);
                        int x = (int)Math.Round(symbolBounds.Left + (symbolBounds.Width / 2) - (echelonBounds.Width / 2));
                        tiEchelon.setLocation(x, y);
                        echelonBounds = tiEchelon.getTextBounds();

                        //There will never be lowercase characters in an echelon so trim that fat.    
                        //Remove the descent from the bounding box.
                        //tiEchelon.getTextOutlineBounds();//.shiftBR(0,ShapeUtilities.round(-(echelonBounds.Height*0.3)));                         

                        //make echelon bounds a little more spacious for things like nearby labels and Task Force.
                        //echelonBounds.Inflate(outlineOffset, outlineOffset);// ShapeUtilities.grow(echelonBounds, outlineOffset);
                        //tiEchelon.getTextOutlineBounds();
                        //                RectUtilities.shift(echelonBounds, x, -outlineOffset);
                        //echelonBounds.shift(0,-outlineOffset);// - ShapeUtilities.round(echelonOffset/2));
                        //tiEchelon.setLocation(x, y - outlineOffset);

                        imageBounds = ShapeUtilities.union(imageBounds, echelonBounds);

                    }
                }
                #endregion

                #region Build Affiliation Modifier
                String affiliationModifier = null;
                if (RS.getDrawAffiliationModifierAsLabel() == false)
                {
                    affiliationModifier = SymbolUtilities.getUnitAffiliationModifier(symbolID, symStd);
                }
                if (affiliationModifier != null)
                {

                    int amOffset = 2;
                    int outlineOffset = RS.getTextOutlineWidth();

                    tiAM = new TextInfo(device,affiliationModifier);
                    amBounds = tiAM.getTextBounds();

                    double x, y;

                    if (echelonBounds != Rect.Empty
                            && ((echelonBounds.Left + echelonBounds.Width > symbolBounds.Left + symbolBounds.Width)))
                    {
                        y = (symbolBounds.Top - amOffset);
                        x = echelonBounds.Left + echelonBounds.Width;
                    }
                    else
                    {
                        y = (symbolBounds.Top - amOffset);
                        x = (symbolBounds.Left + symbolBounds.Width);
                    }
                    tiAM.setLocation(x, y);

                    //adjust for outline.
                    amBounds = tiAM.getTextBoundsWithOutline();
                    tiAM.shift(-outlineOffset, -outlineOffset);

                    imageBounds = ShapeUtilities.union(imageBounds, amBounds);
                }
                #endregion

                #region Build Task Force
                Rect tfBounds = Rect.Empty;
                Rect tfRect = Rect.Empty;
                if (SymbolUtilities.isTaskForce(symbolID))
                {
                    if (echelonBounds != Rect.Empty)
                    {
                        tfRect = new Rect(echelonBounds.X, echelonBounds.Y, echelonBounds.Width, symbolBounds.Y - echelonBounds.Y);
                        tfBounds = ShapeUtilities.clone(tfRect);
                    }
                    else
                    {
                        double height = (symbolBounds.Height / 4);
                        double width = (symbolBounds.Width / 3);

                        tfRect = new Rect(symbolBounds.Left + width,
                                symbolBounds.Top - height,
                                width,
                                height);

                        tfBounds = new Rect(tfRect.Left + -1,
                                tfRect.Top - 1,
                                tfRect.Width + 2,
                                tfRect.Height + 2);

                    }
                    imageBounds = ShapeUtilities.union(imageBounds, tfBounds);
                }
                #endregion

                #region Build Feint Dummy Indicator
                Rect fdiBounds = Rect.Empty;
                Point fdiTop = new Point();// Point.Empty;
                Point fdiLeft = new Point();
                Point fdiRight = new Point();

                if (SymbolUtilities.isFeintDummy(symbolID)
                        || SymbolUtilities.isFeintDummyInstallation(symbolID))
                {
                    //create feint indicator /\
                    fdiLeft = new Point(symbolBounds.Left, symbolBounds.Top);
                    fdiRight = new Point((symbolBounds.Left + symbolBounds.Width), symbolBounds.Top);

                    char affiliation = symbolID[1];
                    if (affiliation == ('F')
                            || affiliation == ('A')
                            || affiliation == ('D')
                            || affiliation == ('M')
                            || affiliation == ('J')
                            || affiliation == ('K'))
                    {
                        fdiTop = new Point((ShapeUtilities.getCenterX(symbolBounds)), ShapeUtilities.round(symbolBounds.Top - (symbolBounds.Height * .75f)));
                    }
                    else
                    {
                        fdiTop = new Point((ShapeUtilities.getCenterX(symbolBounds)), ShapeUtilities.round(symbolBounds.Top - (symbolBounds.Height * .54f)));
                    }

                    fdiBounds = new Rect(fdiLeft.X, fdiLeft.Y, 1, 1);
                    fdiBounds = ShapeUtilities.union(fdiBounds, fdiTop);
                    fdiBounds = ShapeUtilities.union(fdiBounds, fdiRight);

                    if (echelonBounds != null)
                    {
                        double shiftY = (symbolBounds.Top - echelonBounds.Height - 2);
                        fdiLeft.Y = fdiLeft.Y + shiftY;
                        fdiTop.Y = fdiLeft.Y + shiftY;
                        fdiRight.Y = fdiLeft.Y + shiftY;
                        fdiBounds.Y = fdiLeft.Y + shiftY;
                        /*(fdiLeft.Offset(0, shiftY);
                        fdiTop.Offset(0, shiftY);
                        fdiRight.Offset(0, shiftY);
                        fdiBounds.Offset(0, shiftY);//*/
                    }

                    imageBounds = ShapeUtilities.union(imageBounds, fdiBounds);

                }
                #endregion

                #region Build Installation
                Rect instRect = Rect.Empty;
                Rect instBounds = Rect.Empty;
                if (SymbolUtilities.hasInstallationModifier(symbolID))
                {//the actual installation symbols have the modifier
                    //built in.  everything else, we have to draw it.
                    //
                    ////get indicator dimensions////////////////////////////////
                    int width;
                    int height;
                    char affiliation = SymbolUtilities.getAffiliation(symbolID);

                    if (affiliation == 'F'
                            || affiliation == 'A'
                            || affiliation == 'D'
                            || affiliation == 'M'
                            || affiliation == 'J'
                            || affiliation == 'K')
                    {
                        //4th height, 3rd width
                        height = ShapeUtilities.round(symbolBounds.Height / 4);
                        width = ShapeUtilities.round(symbolBounds.Width / 3);
                    }
                    else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                    {
                        //6th height, 3rd width
                        height = ShapeUtilities.round(symbolBounds.Height / 6);
                        width = ShapeUtilities.round(symbolBounds.Width / 3);
                    }
                    else if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                    {
                        //6th height, 3rd width
                        height = ShapeUtilities.round(symbolBounds.Height / 6);
                        width = ShapeUtilities.round(symbolBounds.Width / 3);
                    }
                    else if (affiliation == 'P'
                            || affiliation == 'U'
                            || affiliation == 'G'
                            || affiliation == 'W')
                    {
                        //6th height, 3rd width
                        height = ShapeUtilities.round(symbolBounds.Height / 6);
                        width = ShapeUtilities.round(symbolBounds.Width / 3);
                    }
                    else
                    {
                        //6th height, 3rd width
                        height = ShapeUtilities.round(symbolBounds.Height / 6);
                        width = ShapeUtilities.round(symbolBounds.Width / 3);
                    }

                    //                    if(width * 3 < symbolBounds.Width)
                    //                        width++;
                    //set installation position/////////////////////////////////
                    //set position of indicator
                    if (affiliation == 'F'
                            || affiliation == 'A'
                            || affiliation == 'D'
                            || affiliation == 'M'
                            || affiliation == 'J'
                            || affiliation == 'K'
                            || affiliation == 'N'
                            || affiliation == 'L')
                    {
                        instRect = new Rect((int)(symbolBounds.Left + width),
                                (int)(symbolBounds.Top - height),
                                width,
                                height);
                    }
                    else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                    {
                        instRect = new Rect((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.15f)),
                                width,
                                height);
                    }
                    else if (affiliation == 'P'
                            || affiliation == 'U'
                            || affiliation == 'G'
                            || affiliation == 'W')
                    {
                        instRect = new Rect((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.3f)),
                                width,
                                height);
                    }
                    else
                    {
                        instRect = new Rect((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.3f)),
                                width,
                                height);
                    }

                    /*instRect = new SO.Rect(symbolBounds.Left + width,
                     symbolBounds.Top - height,
                     width,
                     height);//*/
                    //generate installation bounds//////////////////////////////
                    instBounds = new Rect(instRect.Left + -1,
                            instRect.Top - 1,
                            instRect.Width + 2,
                            instRect.Height + 2);

                    imageBounds = ShapeUtilities.union(imageBounds, instBounds);

                }
                #endregion

                #region Build HQ Staff
                Point pt1HQ = new Point();
                Point pt2HQ = new Point();
                Rect hqBounds = Rect.Empty;
                //Draw HQ Staff
                if (SymbolUtilities.isHQ(symbolID))
                {

                    char affiliation = symbolID[1];
                    //get points for the HQ staff
                    if (affiliation == ('F')
                            || affiliation == ('A')
                            || affiliation == ('D')
                            || affiliation == ('M')
                            || affiliation == ('J')
                            || affiliation == ('K')
                            || affiliation == ('N')
                            || affiliation == ('L'))
                    {
                        pt1HQ = new Point((int)symbolBounds.Left + 1,
                                (int)(symbolBounds.Top + symbolBounds.Height - 1));
                        pt2HQ = new Point((int)pt1HQ.X, (int)(pt1HQ.Y + symbolBounds.Height));
                    }
                    else
                    {
                        pt1HQ = new Point((int)symbolBounds.Left + 1,
                                (int)(symbolBounds.Top + (symbolBounds.Height / 2)));
                        pt2HQ = new Point((int)pt1HQ.X, (int)(pt1HQ.Y + symbolBounds.Height));
                    }

                    //create bounding Rect for HQ staff.
                    hqBounds = new Rect(pt1HQ.X, pt1HQ.Y, 2, pt2HQ.Y - pt1HQ.Y);
                    //adjust the image bounds accordingly.
                    imageBounds.Height = imageBounds.Height + (pt2HQ.Y - imageBounds.Bottom);
                    imageBounds = ShapeUtilities.union(imageBounds, hqBounds);
                    //adjust symbol center
                    centerPoint.X = pt2HQ.X;
                    centerPoint.Y = pt2HQ.Y;
                }
                #endregion

                #region Build DOM Arrow
                Point[] domPoints = null;
                Rect domBounds = Rect.Empty;
                if (modifiers != null && modifiers.ContainsKey(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT))
                {
                    String strQ = modifiers[ModifiersUnits.Q_DIRECTION_OF_MOVEMENT];

                    if (strQ != null && SymbolUtilities.isNumber(strQ))
                    {
                        float q = (float)Convert.ToDouble(strQ);

                        Boolean isY = (modifiers.ContainsKey(ModifiersUnits.Y_LOCATION));

                        TextInfo tiY = new TextInfo(device, "Y");

                        domPoints = createDOMArrowPoints(symbolID, symbolBounds, centerPoint, q, tiY.getTextBounds().Height);

                        domBounds = new Rect(domPoints[0].X, domPoints[0].Y, 1, 1);

                        Point temp = new Point();
                        for (int i = 1; i < 6; i++)
                        {
                            temp = domPoints[i];
                            if (temp.X != 0 && temp.Y != 0)
                            {
                                domBounds = ShapeUtilities.union(domBounds, temp);
                            }
                        }
                        domBounds = ShapeUtilities.inflate(domBounds,1, 1);
                        imageBounds = ShapeUtilities.union(imageBounds, domBounds);
                    }
                }
                #endregion

                #region Build Operational Condition Indicator
                Rect ociBounds = Rect.Empty;
                int ociOffset = 4;
                if (mobilityBounds != Rect.Empty)
                {
                    ociOffset = ShapeUtilities.round(mobilityBounds.Height);
                }
                Rect ociShape = processOperationalConditionIndicator(symbolID, symbolBounds, ociOffset);
                if (ociShape != Rect.Empty)
                {
                    Rect temp = ShapeUtilities.clone(ociShape);
                    temp = ShapeUtilities.inflate(temp,2, 3);
                    ociBounds = temp;
                    imageBounds = ShapeUtilities.union(imageBounds, ociBounds);
                }
                #endregion

                #region Shift Modifiers
                if (imageBounds.Left < 0 || imageBounds.Top < 0)
                {
                    double shiftX = Math.Abs(imageBounds.Left);
                    double shiftY = Math.Abs(imageBounds.Top);

                    if (hqBounds != Rect.Empty)
                    {
                        pt1HQ = ShapeUtilities.offset(pt1HQ,shiftX, shiftY);
                        pt2HQ = ShapeUtilities.offset(pt2HQ, shiftX, shiftY);
                    }
                    if (echelonBounds != Rect.Empty)
                    {
                        tiEchelon.setLocation(tiEchelon.getLocation().X + shiftX, tiEchelon.getLocation().Y + shiftY);
                    }
                    if (amBounds != Rect.Empty)
                    {
                        tiAM.setLocation(tiAM.getLocation().X + shiftX, tiAM.getLocation().Y + shiftY);
                    }
                    if (tfBounds != Rect.Empty)
                    {
                        tfRect = ShapeUtilities.offset(tfRect, shiftX, shiftY);
                        tfBounds = ShapeUtilities.offset(tfBounds,shiftX, shiftY);
                    }
                    if (instBounds != Rect.Empty)
                    {
                        instRect = ShapeUtilities.offset(instRect,shiftX, shiftY);
                        instBounds = ShapeUtilities.offset(instBounds,shiftX, shiftY);
                    }
                    if (fdiBounds != Rect.Empty)
                    {
                        fdiBounds = ShapeUtilities.offset(fdiBounds,shiftX, shiftY);
                        fdiLeft = ShapeUtilities.offset(fdiLeft, shiftX, shiftY);
                        fdiTop = ShapeUtilities.offset(fdiTop, shiftX, shiftY);
                        fdiRight = ShapeUtilities.offset(fdiRight,shiftX, shiftY);
                    }
                    if (ociBounds != Rect.Empty)
                    {
                        ociBounds = ShapeUtilities.offset(ociBounds,shiftX, shiftY);
                        ociShape = ShapeUtilities.offset(ociShape,shiftX, shiftY);
                    }
                    if (domBounds != Rect.Empty)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Point temp = domPoints[i];

                            if (temp.X != 0 && temp.Y != 0)
                                temp = ShapeUtilities.offset(temp,shiftX, shiftY);

                            domPoints[i] = temp;

                        }
                        domBounds = ShapeUtilities.offset(domBounds,shiftX, shiftY);
                    }
                    if (mobilityBounds != Rect.Empty)
                    {
                        Matrix3x2 translation = Matrix3x2.CreateTranslation((float)shiftX, (float)shiftY);

                        //shift mobility points
                        cgMobilityPath.Transform(translation);
                        if(cgMobilityPathFill != null)
                        {
                            cgMobilityPathFill.Transform(translation);
                        }

                        mobilityBounds = ShapeUtilities.offset(mobilityBounds,shiftX, shiftY);
                    }

                    centerPoint = ShapeUtilities.offset(centerPoint,shiftX, shiftY);
                    symbolBounds = ShapeUtilities.offset(symbolBounds, shiftX, shiftY);
                    imageBounds = ShapeUtilities.offset(imageBounds, shiftX, shiftY);
                }
                #endregion

                #region Draw Modifiers
                CanvasRenderTarget bmp = new CanvasRenderTarget(device, (float)imageBounds.Width, (float)imageBounds.Height, 96);
                //Bitmap bmp = new Bitmap(imageBounds.Width, imageBounds.Height);
                //Graphics g = Graphics.FromImage(bmp);


                //render////////////////////////////////////////////////////////
                using (CanvasDrawingSession ds = bmp.CreateDrawingSession())
                {
                    ds.Antialiasing = CanvasAntialiasing.Antialiased;
                    ds.TextAntialiasing = CanvasTextAntialiasing.ClearType;
                    //Pen pen = new Pen(Color.Black, 2f);
                    //g.SmoothingMode = SmoothingMode.AntiAlias;
                    if (hqBounds != Rect.Empty)
                    {
                        ds.DrawLine(pt1HQ.ToVector2(), pt2HQ.ToVector2(), Colors.Black, 2f);
                        //g.DrawLine(pen, pt1HQ.X, pt1HQ.Y, pt2HQ.X, pt2HQ.Y);
                    }

                    if (tfBounds != Rect.Empty)
                    {
                        ds.DrawRectangle(tfRect, Colors.Black,2f);
                        //g.DrawRect(pen, tfRect);
                    }

                    if (instBounds != Rect.Empty)
                    {
                        ds.FillRectangle(tfRect,Colors.Black);
                        //g.FillRect(Brushes.Black, instRect);
                    }

                    if (echelonBounds != Rect.Empty)
                    {
                        /*TextInfo[] aTiEchelon =
                        {
                            tiEchelon
                        };
                        renderText(g, aTiEchelon, textColor, textBackgroundColor);//*/
                        tiEchelon.drawText(ds, textColor);
                        echelonBounds = Rect.Empty;
                        tiEchelon = null;
                    }

                    if (amBounds != Rect.Empty)
                    {
                        /*TextInfo[] aTiAM =
                        {
                            tiAM
                        };
                        renderText(g, aTiAM, textColor, textBackgroundColor);//*/
                        tiAM.drawText(ds, textColor);
                        amBounds = Rect.Empty;
                        tiAM = null;
                    }

                    if (fdiBounds != Rect.Empty)
                    {
                        CanvasStrokeStyle style = new CanvasStrokeStyle();
                        
                        if (symbolBounds.Width > 19)
                        {
                            style.CustomDashStyle = new float[] { 6f, 4f };
                        }
                        else
                        {
                            style.CustomDashStyle = new float[] { 5f, 3f };
                        }

                        style.LineJoin = CanvasLineJoin.Miter;
                        style.MiterLimit = 3;
                        
                        //pen.LineJoin = LineJoin.Miter;
                        //pen.MiterLimit = 3;
                        //pen.Width = 2;
                        
                        //GraphicsPath fdiPath = new GraphicsPath();

                        //fdiPath.AddLine(fdiLeft.X, fdiLeft.Y, fdiTop.X, fdiTop.Y);
                        //fdiPath.AddLine(fdiTop.X, fdiTop.Y, fdiRight.X, fdiRight.Y);

                        ds.DrawLine(fdiLeft.ToVector2(), fdiTop.ToVector2(), Colors.Black, 2f);

                        //g.DrawPath(pen, fdiPath);

                        fdiBounds = Rect.Empty;

                    }

                    if (mobilityBounds != Rect.Empty)
                    {


                        //ctx.lineCap = "butt";
                        //ctx.lineJoin = "miter";
                        if (symbolID[10] == ('N'))
                        {
                            ds.Antialiasing = CanvasAntialiasing.Aliased;
                            //mobilityPaint.setAntiAlias(false);
                            //g.SmoothingMode = SmoothingMode.None;
                        }
                        ds.DrawGeometry(cgMobilityPath, Colors.Black, 2f);
                       // g.DrawPath(mobilityPen, mobilityPath);

                        if (cgMobilityPathFill != null)
                        {
                            ds.FillGeometry(cgMobilityPathFill,Colors.Black);
                            //g.FillPath(Brushes.Black, mobilityPathFill);
                        }

                        mobilityBounds = Rect.Empty;

                    }

                    if (ociBounds != Rect.Empty)
                    {
                        Color statusColor = Colors.Black;
                        char status = symbolID[3];
                        if (status == ('C'))//Fully Capable
                        {
                            statusColor = Colors.Green;
                        }
                        else if (status == ('D'))//Damage
                        {
                            statusColor = Colors.Yellow;
                        }
                        else if (status == ('X'))
                        {
                            statusColor = Colors.Red;
                        }
                        else if (status == ('F'))//full to capacity(hospital)
                        {
                            statusColor = Colors.Blue;
                        };

                        ds.FillRectangle(ociBounds, Colors.Black);
                        //g.FillRect(Brushes.Black, ociBounds);
                        ds.FillRectangle(ociShape, statusColor);
                        //g.FillRect(new SolidBrush(statusColor), ociShape);

                        ociBounds = Rect.Empty;
                        ociShape = Rect.Empty;
                    }

                    //draw original icon.        
                    //ctx.drawBitmap(ii.getImage(), null, symbolBounds, null);
                    ds.DrawImage(ii.getCanvasRenderTarget(), (float)symbolBounds.X, (float)symbolBounds.Y);
                    //g.DrawImageUnscaled(ii.getBitmap(), symbolBounds.X, symbolBounds.Y);

                    if (domBounds != Rect.Empty)
                    {
                        drawDOMArrow(device, ds, domPoints);

                        domBounds = Rect.Empty;
                        domPoints = null; ;
                    }

                    #endregion
                }
                
               
                /*GraphicsUnit pixel = GraphicsUnit.Pixel;
                Rect outline = ShapeUtilities.cloneToRect(bmp.GetBounds(ref pixel));
                outline.Width = outline.Width - 1;
                outline.Height = outline.Height - 1;
                g.DrawRect(Pens.Red, outline);//*/

                newii = new ImageInfo(bmp, centerPoint, symbolBounds,bmp.GetBounds(device));

                #region Cleanup
                if (newii != null)
                {
                    return newii;
                }
                else
                {
                    return null;
                }
                #endregion
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "ProcessUnitDisplayModifiers", exc);
                return null;
            }
        }


        private static double getYPositionForSCC(String symbolID)
        {
            double yPosition = 0.32;
            String temp = symbolID.Substring(4, 6);
            char affiliation = symbolID[1];

            if (temp.Equals("WMGC--"))//GROUND (BOTTOM) MILCO
            {
                if (affiliation == 'H' ||
                        affiliation == 'S')//suspect
                    yPosition = 0.29;
                else if (affiliation == 'N' ||
                        affiliation == 'L')//exercise neutral
                    yPosition = 0.32;
                else if (affiliation == 'F' ||
                        affiliation == 'A' ||//assumed friend
                        affiliation == 'D' ||//exercise friend
                        affiliation == 'M' ||//exercise assumed friend
                        affiliation == 'K' ||//faker
                        affiliation == 'J')//joker
                    yPosition = 0.32;
                else
                    yPosition = 0.34;
            }
            else if (temp.Equals("WMMC--"))//MOORED MILCO
            {
                if (affiliation == 'H' ||
                        affiliation == 'S')//suspect
                    yPosition = 0.25;
                else if (affiliation == 'N' ||
                        affiliation == 'L')//exercise neutral
                    yPosition = 0.25;
                else if (affiliation == 'F' ||
                        affiliation == 'A' ||//assumed friend
                        affiliation == 'D' ||//exercise friend
                        affiliation == 'M' ||//exercise assumed friend
                        affiliation == 'K' ||//faker
                        affiliation == 'J')//joker
                    yPosition = 0.25;
                else
                    yPosition = 0.28;
            }
            else if (temp.Equals("WMFC--"))//FLOATING MILCO
            {
                if (affiliation == 'H' ||
                        affiliation == 'S')//suspect
                    yPosition = 0.29;
                else if (affiliation == 'N' ||
                        affiliation == 'L')//exercise neutral
                    yPosition = 0.32;
                else if (affiliation == 'F' ||
                        affiliation == 'A' ||//assumed friend
                        affiliation == 'D' ||//exercise friend
                        affiliation == 'M' ||//exercise assumed friend
                        affiliation == 'K' ||//faker
                        affiliation == 'J')//joker
                    yPosition = 0.32;
                else
                    yPosition = 0.34;
            }
            else if (temp.Equals("WMC---"))//GENERAL MILCO
            {
                if (affiliation == 'H' ||
                        affiliation == 'S')//suspect
                    yPosition = 0.33;
                else if (affiliation == 'N' ||
                        affiliation == 'L')//exercise neutral
                    yPosition = 0.36;
                else if (affiliation == 'F' ||
                        affiliation == 'A' ||//assumed friend
                        affiliation == 'D' ||//exercise friend
                        affiliation == 'M' ||//exercise assumed friend
                        affiliation == 'K' ||//faker
                        affiliation == 'J')//joker
                    yPosition = 0.36;
                else
                    yPosition = 0.36;
            }

            return yPosition;
        }

        /**
         *
         * @param {type} symbolID
         * @param {type} bounds symbolBounds SO.Rect
         * @param {type} center SO.Point Location where symbol is centered.
         * @param {type} angle in degrees
         * @param {Boolean} isY Boolean.
         * @returns {Array} of SO.Point. First 3 items are the line. Last three are
         * the arrowhead.
         */
        private static Point[] createDOMArrowPoints(String symbolID, Rect bounds, Point center, float angle, double yOffset)
        {
            Point[] arrowPoints = new Point[6];
            Point pt1 = new Point(0,0);
            Point pt2 = new Point(0,0);
            Point pt3 = new Point(0,0);

            double length = 40;
            if (SymbolUtilities.isNBC(symbolID))
            {
                length = ShapeUtilities.round(bounds.Height / 2);
            }
            else
            {
                length = bounds.Height;
            }

            //get endpoint
            double dx2, dy2,
                    x1, y1,
                    x2, y2;

            x1 = (center.X);
            y1 = (center.Y);

            pt1 = new Point(x1, y1);

            if (SymbolUtilities.isNBC(symbolID)
                    || (SymbolUtilities.isWarfighting(symbolID) && symbolID[2] == ('G')))
            {
                y1 = bounds.Top + bounds.Height;
                pt1 = new Point(x1, y1);

                if (yOffset > 0 && SymbolUtilities.isNBC(symbolID))//make room for y modifier
                {
                    yOffset += RS.getTextOutlineWidth();
                    pt1 = ShapeUtilities.offset(pt1,0, yOffset + RS.getTextOutlineWidth());
                }

                y1 = y1 + length;
                pt2 = new Point(x1, y1);
            }

            //get endpoint given start point and an angle
            //x2 = x1 + (length * Math.cos(radians)));
            //y2 = y1 + (length * Math.sin(radians)));
            angle = angle - 90;//in java, east is zero, we want north to be zero
            double radians = 0;
            radians = (angle * (Math.PI / 180));//convert degrees to radians

            dx2 = x1 + (int)(length * Math.Cos(radians));
            dy2 = y1 + (int)(length * Math.Sin(radians));
            x2 = (dx2);
            y2 = (dy2);

            //create arrowhead//////////////////////////////////////////////////////
            //Edit arrowWidth and theta to change the shape of the arrowhead.
            float arrowWidth = 16.0f,//8.0f,//6.5f;//7.0f;//6.5f;//10.0f//default
                    theta = 0.673f;//higher value == shorter arrow head//*/

            if (length < 50)
            {
                theta = 0.95f;
                arrowWidth = 12.0f;
            }
            /*float arrowWidth = length * .09f,// 16.0f,//8.0f,//6.5f;//7.0f;//6.5f;//10.0f//default
             theta = length * .0025f;//0.423f;//higher value == shorter arrow head
             if(arrowWidth < 8)
             arrowWidth = 8f;//*/

            double[] xPoints = new double[3];//3
            double[] yPoints = new double[3];//3
            double[] vecLine = new double[2];//2
            double[] vecLeft = new double[2];//2
            double fLength;
            double th;
            double ta;
            double baseX, baseY;

            xPoints[0] = x2;
            yPoints[0] = y2;

            //build the line vector
            vecLine[0] = (xPoints[0] - x1);
            vecLine[1] = (yPoints[0] - y1);

            //build the arrow base vector - normal to the line
            vecLeft[0] = -vecLine[1];
            vecLeft[1] = vecLine[0];

            //setup length parameters
            fLength = Math.Sqrt(vecLine[0] * vecLine[0] + vecLine[1] * vecLine[1]);
            th = arrowWidth / (2.0 * fLength);
            ta = arrowWidth / (2.0 * (Math.Tan(theta) / 2.0) * fLength);

            //find base of the arrow
            baseX = (xPoints[0] - ta * vecLine[0]);
            baseY = (yPoints[0] - ta * vecLine[1]);

            //build the points on the sides of the arrow
            xPoints[1] = (int)ShapeUtilities.round(baseX + th * vecLeft[0]);
            yPoints[1] = (int)ShapeUtilities.round(baseY + th * vecLeft[1]);
            xPoints[2] = (int)ShapeUtilities.round(baseX - th * vecLeft[0]);
            yPoints[2] = (int)ShapeUtilities.round(baseY - th * vecLeft[1]);

            //line.lineTo((int)baseX, (int)baseY);
            pt3 = new Point((int)ShapeUtilities.round(baseX), (int)ShapeUtilities.round(baseY));

            //arrowHead = new Polygon(xPoints, yPoints, 3);
            arrowPoints[0] = pt1;
            arrowPoints[1] = pt2;
            arrowPoints[2] = pt3;
            arrowPoints[3] = new Point(xPoints[0], yPoints[0]);
            arrowPoints[4] = new Point(xPoints[1], yPoints[1]);
            arrowPoints[5] = new Point(xPoints[2], yPoints[2]);

            return arrowPoints;

        }

        private static void drawDOMArrow(ICanvasResourceCreator device, CanvasDrawingSession ds, Point[] domPoints)
        {
           // Pen pen = new Pen(Color.Black, 3f);

            //pen.MiterLimit = 3;
            
            CanvasPathBuilder domPath = new CanvasPathBuilder(device);
            CanvasPathBuilder domArrow = new CanvasPathBuilder(device);

            if (domPoints[1].X == 0 && domPoints[1].Y == 0)
            {
                domPath.BeginFigure(domPoints[0].ToVector2());
                domPath.AddLine(domPoints[2].ToVector2());
                domPath.EndFigure(CanvasFigureLoop.Open);
            }
            else
            {
                domPath.BeginFigure(domPoints[0].ToVector2());
                domPath.AddLine(domPoints[1].ToVector2());
                domPath.AddLine(domPoints[2].ToVector2());
                domPath.EndFigure(CanvasFigureLoop.Open);
            }

            domArrow.BeginFigure(domPoints[3].ToVector2());
            domArrow.AddLine(domPoints[4].ToVector2());
            domArrow.AddLine(domPoints[5].ToVector2());
            domArrow.EndFigure(CanvasFigureLoop.Closed);

            CanvasGeometry cgPath = CanvasGeometry.CreatePath(domPath);
            CanvasGeometry cgArrow = CanvasGeometry.CreatePath(domArrow);

            ds.DrawGeometry(cgPath, Colors.Black,3f);
            ds.FillGeometry(cgArrow, Colors.Black);
        }

        private static Rect processOperationalConditionIndicator(String symbolID, Rect symbolBounds, int offsetY)
        {
            // <editor-fold defaultstate="collapsed" desc="Operational Condition Indicator">
            //create Operational Condition Indicator
            //set color
            
            Rect bar = Rect.Empty;
            char status;
            int barSize = 0;
            double pixelSize = symbolBounds.Height;

            status = symbolID[3];

            /*if(_statusColorMap[status] !== undefined)
             statusColor = _statusColorMap[status];
             else
             statusColor = null;*/
            if (status == 'C' || status == 'D' || status == 'X' || status == 'F')
            {
                if (pixelSize > 0)
                {
                    barSize = ShapeUtilities.round(pixelSize / 5);
                }

                if (barSize < 2)
                {
                    barSize = 2;
                }

                offsetY += ShapeUtilities.round(symbolBounds.Top + symbolBounds.Height);

                bar = new Rect(symbolBounds.Left + 2, offsetY, ShapeUtilities.round(symbolBounds.Width) - 4, barSize);
                //bar = new SO.Rect(symbolBounds.Left+1, offsetY, ShapeUtilities.round(symbolBounds.Width)-2,barSize);
                /*ctx.lineColor = '#000000';
                 ctx.lineWidth = 1;
                 ctx.fillColor = statusColor;
                 bar.fill(ctx);
                 bar.grow(1);
                 bar.stroke(ctx);
            
                 imageBounds.union(bar.getBounds());//*/
            }

            return bar;

            // </editor-fold>
        }

        /// <summary>
        /// DEPRECATED, probably not necessary since it's not a font object, just a font descriptor.
        /// </summary>
        private static void checkModifierFont()
        {
            /*
            if (_fontSize != RS.getModifierFontSize() || _fontName.Equals(RS.getModifierFontName()) == false || _fontStyle != RS.getModifierFontStyle())
            {
                _modifierFontHeight = ShapeUtilities.round(_g.MeasureString("Hj", RS.getLabelFont()).Height);
            }//*/

            /*private readonly object syncLock = new object();
            public void SomeMethod() {
            lock(syncLock) { code }
            }*/

        }

        public static Boolean hasDisplayModifiers(String symbolID, Dictionary<int, String> modifiers)
        {
            Boolean hasModifiers = false;
            char scheme = symbolID[0];
            char status = symbolID[3];
            char affiliation = symbolID[1];
            if (scheme != 'W')
            {
                if (scheme != 'G' && (SymbolUtilities.isEMSNaturalEvent(symbolID) == false))
                {
                    switch (status)
                    {
                        case 'C':
                        case 'D':
                        case 'X':
                        case 'F':
                            hasModifiers = true;
                            break;

                        default:
                            break;
                    }

                    if ((symbolID.Substring(10, 2).Equals("--") == false && symbolID.Substring(10, 2).Equals("**") == false) || (modifiers != null && modifiers.ContainsKey(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT)))
                    {
                        hasModifiers = true;
                    }
                    else if (modifiers != null && modifiers.ContainsKey(ModifiersTG.Q_DIRECTION_OF_MOVEMENT))
                        hasModifiers = true;
                }
                else
                {
                    if (SymbolUtilities.isNBC(symbolID) == true && modifiers != null && modifiers.ContainsKey(ModifiersTG.Q_DIRECTION_OF_MOVEMENT))
                    {
                        hasModifiers = true;
                    }
                }

            }

            return hasModifiers;
        }

        public static Boolean hasTextModifiers(String symbolID, Dictionary<int, String> modifiers, Dictionary<int, String> attributes)
        {

            int symStd = RS.getSymbologyStandard();
            if (attributes != null && attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
            {
                symStd = Convert.ToInt32(attributes[MilStdAttributes.SymbologyStandard]);
            }
            else
            {
                symStd = RS.getSymbologyStandard();
            }
            char scheme = symbolID[0];
            if (scheme == 'W')
            {
                return false;
            }
            if (scheme == 'G')
            {
                if (modifiers != null)
                {
                    if (modifiers.ContainsKey(ModifiersTG.Q_DIRECTION_OF_MOVEMENT))
                    {
                        if (modifiers.Count > 1)
                        {
                            return true;
                        }
                    }
                    else if (modifiers.Count > 0)
                    {
                        return true;
                    }
                }

            }
            else if (SymbolUtilities.isEMSNaturalEvent(symbolID) == false)
            {

                if (SymbolUtilities.getUnitAffiliationModifier(symbolID, symStd) != null)
                {
                    return true;
                }

                if (SymbolUtilities.hasValidCountryCode(symbolID))
                {
                    return true;
                }

                if (SymbolUtilities.isEMSNaturalEvent(symbolID))
                {
                    return false;
                }

                if (modifiers != null)
                {
                    if (modifiers.ContainsKey(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT))
                    {
                        if (modifiers.Count > 1)
                        {
                            return true;
                        }
                    }
                    else if (modifiers != null && modifiers.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static Rect measureText(String text, CanvasDevice device)
        {
            Rect rect = Rect.Empty;

            if (device == null)
                device = CanvasDevice.GetSharedDevice();
            //coreUnitBuffer = new CanvasRenderTarget(device, coreBufferSize, coreBufferSize, 96);//new Bitmap(w, h);

            float xLoc = 100.0f;
            float yLoc = 100.0f;
            CanvasTextFormat format = RS.getLabelFont();//new CanvasTextFormat { FontSize = 30.0f, WordWrapping = CanvasWordWrapping.NoWrap };
            format.HorizontalAlignment = CanvasHorizontalAlignment.Left;
            //format.VerticalAlignment = CanvasVerticalAlignment.Bottom;

            CanvasTextLayout textLayout = new CanvasTextLayout(device, text, format, 0.0f, 0.0f);
            rect = new Rect(xLoc + textLayout.DrawBounds.X, yLoc + textLayout.DrawBounds.Y, textLayout.DrawBounds.Width, textLayout.DrawBounds.Height);

            return rect;

        }


    }
}
