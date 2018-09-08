using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MilSymNetUtilities;

namespace MilSymNet
{
    public class ModifierRenderer
    {
        private static RendererSettings RS = RendererSettings.getInstance();
        private static Bitmap _textBMP = new Bitmap(2,2);
        private static Graphics _g = Graphics.FromImage(_textBMP);
        private static Matrix _identityMatrix = new Matrix();
        private static int _fontSize = RS.getModifierFontSize();
        private static FontStyle _fontStyle = RS.getModifierFontStyle();
        private static String _fontName = RS.getModifierFontName();
        private static int _modifierFontHeight = ShapeUtilities.round(_g.MeasureString("Hj",RS.getLabelFont()).Height);
        private static Font _modifierFont = RS.getLabelFont();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolID"></param>
        /// <param name="msb"></param>
        /// <param name="modifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static ImageInfo ProcessUnitDisplayModifiers(String symbolID, ImageInfo ii, Dictionary<int, String> modifiers, Dictionary<int, String> attributes, Boolean hasTextModifiers)
        {
            ImageInfo newii = null;
            Rectangle symbolBounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Rectangle imageBounds = ShapeUtilities.cloneToRectangle(ii.getImageBounds());
            Point centerPoint = ShapeUtilities.clone(ii.getCenterPoint());
            TextInfo tiEchelon = null;
            TextInfo tiAM = null;
            Rectangle echelonBounds = Rectangle.Empty;
            Rectangle amBounds = Rectangle.Empty;
            Color textColor = Color.Black;
            Color textBackgroundColor = Color.Empty;
            int buffer = 0;
            //ctx = null;
            int offsetX = 0;
            int offsetY = 0;
            int symStd = RS.getSymbologyStandard();
            Font font = RS.getLabelFont();
            Pen mobilityPen = new Pen(Color.Black, 2f);
            mobilityPen.MiterLimit = 2;

            try
            {
                if (attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
                {
                    symStd = Convert.ToInt16(attributes[MilStdAttributes.SymbologyStandard]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextColor))
                {
                    textColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextColor]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextBackgroundColor))
                {
                    textBackgroundColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextBackgroundColor]);
                }

                #region Build Mobility Modifiers

                RectangleF mobilityBounds = RectangleF.Empty;

                List<GraphicsPath> shapes = new List<GraphicsPath>();
                GraphicsPath mobilityPath = null;
                GraphicsPath mobilityPathFill = null;
                if (symbolID[10] == ('M') || symbolID[10] == ('N'))
                {

                    //Draw Mobility
                    int fifth = (int)((symbolBounds.Width * 0.2) + 0.5f);
                    mobilityPath = new GraphicsPath();
                    int x = 0;
                    int y = 0;
                    int centerX = 0;
                    int bottomY = 0;
                    int height = 0;
                    int width = 0;
                    int middleY = 0;
                    int wheelOffset = 2;
                    int wheelSize = fifth;//10;
                    int rrHeight = fifth;//10;
                    int rrArcWidth = (int)((fifth * 1.5) + 0.5f);//16;

                    String mobility = symbolID.Substring(10, 2);
                    x = (int)symbolBounds.Left;
                    y = (int)symbolBounds.Top;
                    height = (symbolBounds.Height);
                    width = (symbolBounds.Width);
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
                            mobilityPath.AddLine(x, bottomY, x + width, bottomY);
                            //PathUtilties.AddLine(mobilityPath, x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddEllipse(x, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                        }
                        else if (mobility.Equals("MP"))
                        {
                            //line
                            mobilityPath.AddLine(x, bottomY, x + width, bottomY);
                            //PathUtilties.AddLine(mobilityPath, x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddEllipse(x, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //center wheel
                            mobilityPath.AddEllipse(x + (width / 2) - (wheelSize / 2), bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + (width / 2) - (wheelSize / 2), bottomY + wheelOffset, wheelSize, wheelSize);
                        }
                        else if (mobility.Equals("MQ"))
                        {
                            //round rectangle
                            mobilityPath.AddPath(ShapeUtilities.createRoundedRectangle(new Rectangle(x, bottomY, width, rrHeight),rrHeight / 2),false);
                            //PathUtilties.AddRoundedRect(mobilityPath, x, bottomY, width, rrHeight, rrHeight / 2, rrHeight);

                        }
                        else if (mobility.Equals("MR"))
                        {
                            //round rectangle
                            mobilityPath.AddPath(ShapeUtilities.createRoundedRectangle(new Rectangle(x, bottomY, width, rrHeight), wheelSize / 2), false);
                            //PathUtilties.AddRoundedRect(mobilityPath, x, bottomY, width, rrHeight, wheelSize / 2, rrHeight);

                            //left circle
                            mobilityPath.AddEllipse(x - wheelSize - wheelSize, bottomY, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x - wheelSize - wheelSize, bottomY, wheelSize, wheelSize);
                        }
                        else if (mobility.Equals("MS"))
                        {
                            //line
                            mobilityPath.AddLine(x + wheelSize, bottomY + (wheelSize / 2),
                                    x + width - wheelSize, bottomY + (wheelSize / 2));
                            //PathUtilties.AddLine(mobilityPath, x + wheelSize, bottomY + (wheelSize / 2),
                            //        x + width - wheelSize, bottomY + (wheelSize / 2));

                            //left circle
                            mobilityPath.AddEllipse(x, bottomY, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x, bottomY, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddEllipse(x + width - wheelSize, bottomY, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + width - wheelSize, bottomY, wheelSize, wheelSize);
                        }
                        else if (mobility.Equals("MT"))
                        {

                            //line
                            mobilityPath.AddLine(x, bottomY, x + width, bottomY);
                            //PathUtilties.AddLine(mobilityPath, x, bottomY, x + width, bottomY);

                            //left circle
                            mobilityPath.AddEllipse(x + wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //left circle2
                            mobilityPath.AddEllipse(x, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle
                            mobilityPath.AddEllipse(x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + width - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                            //right circle2
                            mobilityPath.AddEllipse(x + width - wheelSize - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);
                            //PathUtilties.AddEllipse(mobilityPath, x + width - wheelSize - wheelSize, bottomY + wheelOffset, wheelSize, wheelSize);

                        }
                        else if (mobility.Equals("MU"))
                        {
                            float halfWidth = (rrArcWidth * 0.5f);
                            mobilityPath.AddLine(x, bottomY, x + halfWidth, bottomY + halfWidth);
                            mobilityPath.AddLine(x + halfWidth, bottomY + halfWidth, x + width, bottomY + halfWidth);
                            //mobilityPath.moveTo(x, bottomY);
                            //mobilityPath.lineTo(x + halfWidth, bottomY + halfWidth);
                            //mobilityPath.lineTo(x + width, bottomY + halfWidth);
                        }
                        else if (mobility.Equals("MV"))
                        {
                            mobilityPath.AddBezier(x, bottomY, x, bottomY, x - rrHeight, bottomY + rrHeight / 2, x, bottomY + rrHeight);

                            //mobilityPath.moveTo(x, bottomY);
                            //mobilityPath.cubicTo(x, bottomY, x - rrHeight, bottomY + rrHeight / 2, x, bottomY + rrHeight);
                            ////mobilityPath.bezierCurveTo(x, bottomY, x-rrArcWidth, bottomY+3, x, bottomY+rrHeight);

                            mobilityPath.AddLine(x, bottomY + rrHeight, x + width, bottomY + rrHeight);
                            //mobilityPath.lineTo(x + width, bottomY + rrHeight);

                            mobilityPath.AddBezier(x + width, bottomY + rrHeight, x + width, bottomY + rrHeight, x + width + rrHeight, bottomY + rrHeight / 2, x + width, bottomY);
                            //mobilityPath.cubicTo(x + width, bottomY + rrHeight, x + width + rrHeight, bottomY + rrHeight / 2, x + width, bottomY);
                            ////shapeMobility.curveTo(x + width, bottomY + rrHeight, x+ width + rrArcWidth, bottomY+3, x + width, bottomY);
                        }
                        else if (mobility.Equals("MW"))
                        {
                            centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);
                            int angleWidth = rrHeight / 2;

                            mobilityPath.AddLine(centerX, bottomY + rrHeight + 2, centerX - angleWidth, bottomY);
                            //mobilityPath.moveTo(centerX, bottomY + rrHeight + 2);
                            //mobilityPath.lineTo(centerX - angleWidth, bottomY);
                            mobilityPath.AddLine(centerX - angleWidth, bottomY,centerX - angleWidth * 2, bottomY + rrHeight + 2);
                            //mobilityPath.lineTo(centerX - angleWidth * 2, bottomY + rrHeight + 2);

                            mobilityPath.StartFigure();
                            mobilityPath.AddLine(centerX, bottomY + rrHeight + 2, centerX + angleWidth, bottomY);
                            //mobilityPath.moveTo(centerX, bottomY + rrHeight + 2);
                            //mobilityPath.lineTo(centerX + angleWidth, bottomY);
                            mobilityPath.AddLine(centerX + angleWidth, bottomY,centerX + angleWidth * 2, bottomY + rrHeight + 2);
                            //mobilityPath.lineTo(centerX + angleWidth * 2, bottomY + rrHeight + 2);
                        }
                        else if (mobility.Equals("MX"))
                        {
                            centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);
                            mobilityPath.AddLine(x + width, bottomY, x, bottomY);
                            //PathUtilties.AddLine(mobilityPath, x + width, bottomY, x, bottomY);
                            ////var line = new SO.Line(x + width, bottomY,x, bottomY);

                            float quarterX = (centerX - x) / 2;
                            ////var quarterY = (((bottomY + rrHeight) - bottomY)/2);

                            mobilityPath.AddBezier(x, bottomY, x + quarterX, bottomY + rrHeight, centerX + quarterX, bottomY + rrHeight, x + width, bottomY);
                            //mobilityPath.moveTo(x, bottomY);
                            //mobilityPath.cubicTo(x + quarterX, bottomY + rrHeight, centerX + quarterX, bottomY + rrHeight, x + width, bottomY);
                            ////shapes.push(new SO.BCurve(x, bottomY,x+quarterX, bottomY+rrHeight, centerX + quarterX, bottomY + rrHeight, x + width, bottomY));
                        }

                        else if (mobility.Equals("MY"))
                        {
                            float incrementX = width / 7f;

                            x = symbolBounds.X;
                            float r = incrementX;

                            //mobilityPath.arcTo(oval, sAngle, sAngle, moveTo);
                            mobilityPath.AddArc(x, bottomY, r, r, 180, 180);
                            mobilityPath.AddArc(x + incrementX, bottomY, r, r, 180, -180);
                            mobilityPath.AddArc(x + incrementX * 2, bottomY, r, r, 180, 180);
                            mobilityPath.AddArc(x + incrementX * 3, bottomY, r, r, 180, -180);
                            mobilityPath.AddArc(x + incrementX * 4, bottomY, r, r, 180, 180);
                            mobilityPath.AddArc(x + incrementX * 5, bottomY, r, r, 180, -180);
                            mobilityPath.AddArc(x + incrementX * 6, bottomY, r, r, 180, 180);

                        }

                    }
                    //Draw Towed Array Sonar
                    else if (symbolID[10] == ('N'))
                    {

                        int boxHeight = (int)((rrHeight * 0.8f) + 0.5f);
                        bottomY = y + height + (boxHeight / 7);
                        mobilityPathFill = new GraphicsPath();
                        offsetY = ShapeUtilities.round(boxHeight / 6);//1;
                        centerX = (int)((symbolBounds.X + (symbolBounds.Width / 2)) + 0.5);
                        int squareOffset = (int)((boxHeight * 0.5f) + 0.5);
                        middleY = ((boxHeight / 2) + bottomY) + offsetY;//+1 for offset from symbol
                        if (symbolID.Substring(10, 2).Equals("NS"))
                        {
                            //subtract 0.5 becase lines 1 pixel thick get aliased into
                            //a line two pixels wide.
                            //line
                            mobilityPath.AddLine(centerX - 1, bottomY - 1, centerX - 1, bottomY + boxHeight + offsetY + 2);
                            //PathUtilties.AddLine(mobilityPath, centerX - 1, bottomY - 1, centerX - 1, bottomY + boxHeight + offsetY);

                            //line
                            mobilityPath.StartFigure();
                            mobilityPath.AddLine(x, middleY, x + width, middleY);
                            //PathUtilties.AddLine(mobilityPath, x, middleY, x + width, middleY);

                            //square
                            mobilityPathFill.AddRectangle(new Rectangle(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);

                            //square
                            mobilityPathFill.AddRectangle(new Rectangle(centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(ShapeUtilities.round(centerX - squareOffset), bottomY + offsetY, boxHeight, boxHeight), Direction.CW);

                            //square
                            mobilityPathFill.AddRectangle(new Rectangle(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);

                        }
                        else if (symbolID.Substring(10, 2).Equals("NL"))
                        {
                            int leftX = x + (centerX - x) / 2,
                                    rightX = centerX + (x + width - centerX) / 2;

                            //line vertical left
                            mobilityPath.AddLine(leftX, bottomY - 1, leftX, bottomY + offsetY + boxHeight + offsetY + 2);
                            //PathUtilties.AddLine(mobilityPath, leftX, bottomY - 1, leftX, bottomY + offsetY + boxHeight + offsetY);

                            //line vertical right
                            mobilityPath.StartFigure();
                            mobilityPath.AddLine(rightX, bottomY - 1, rightX, bottomY + offsetY + boxHeight + offsetY + 2);
                            //PathUtilties.AddLine(mobilityPath, rightX, bottomY - 1, rightX, bottomY + offsetY + boxHeight + offsetY);

                            //line horizontal
                            mobilityPath.StartFigure();
                            mobilityPath.AddLine(x, middleY, x + width, middleY);
                            //PathUtilties.AddLine(mobilityPath, x, middleY, x + width, middleY);

                            //square left
                            mobilityPathFill.AddRectangle(new Rectangle(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(x - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);

                            //square middle
                            mobilityPathFill.AddRectangle(new Rectangle(centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(centerX - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);
                            
                            //square right
                            mobilityPathFill.AddRectangle(new Rectangle(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(x + width - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);
                            
                            //square middle left
                            mobilityPathFill.AddRectangle(new Rectangle(leftX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(leftX - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);

                            //square middle right
                            mobilityPathFill.AddRectangle(new Rectangle(rightX - squareOffset, bottomY + offsetY, boxHeight, boxHeight));
                            //mobilityPathFill.AddRect(PathUtilties.makeRectF(rightX - squareOffset, bottomY + offsetY, boxHeight, boxHeight), Direction.CW);


                        }
                    }

                    //get mobility bounds
                    if (mobilityPath != null)
                    {

                        //build mobility bounds
                        mobilityBounds = new Rectangle();
                        mobilityBounds = mobilityPath.GetBounds(_identityMatrix,mobilityPen);


                        RectangleF mobilityFillBounds = new RectangleF();
                        if (mobilityPathFill != null)
                        {
                            mobilityFillBounds = mobilityPathFill.GetBounds();
                            mobilityBounds = RectangleF.Union(mobilityBounds,mobilityFillBounds);
                        }

                        //grow because we use a line thickness of 2.
                        mobilityBounds.Inflate(1f, 1f);
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
                        int echelonOffset = 2,
                                outlineOffset = RS.getTextOutlineWidth();

                        tiEchelon = new TextInfo(strEchelon, 0, 0, font,_g);
                        echelonBounds = tiEchelon.getTextBounds();

                        int y = (symbolBounds.Top - echelonBounds.Height);
                        int x = (symbolBounds.Left + (symbolBounds.Width / 2) - (echelonBounds.Width / 2));
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
                        
                        imageBounds = Rectangle.Union(imageBounds, echelonBounds);

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

                    tiAM = new TextInfo(affiliationModifier, 0, 0, font,_g);
                    amBounds = tiAM.getTextBounds();

                    int x, y;

                    if (echelonBounds != Rectangle.Empty
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
                    amBounds.Inflate(outlineOffset, outlineOffset);
                    tiAM.shift(-outlineOffset, -outlineOffset);

                    imageBounds = Rectangle.Union(imageBounds,amBounds);
                }
                #endregion

                #region Build Task Force
                Rectangle tfBounds = Rectangle.Empty;
                Rectangle tfRectangle = Rectangle.Empty;
                if (SymbolUtilities.isTaskForce(symbolID))
                {
                    if (echelonBounds != Rectangle.Empty)
                    {
                        tfRectangle = new Rectangle(echelonBounds.X, echelonBounds.Y, echelonBounds.Width, symbolBounds.Y - echelonBounds.Y);
                        tfBounds = ShapeUtilities.clone(tfRectangle);
                    }
                    else
                    {
                        int height = (symbolBounds.Height / 4);
                        int width = (symbolBounds.Width / 3);

                        tfRectangle = new Rectangle(symbolBounds.Left + width,
                                symbolBounds.Top - height,
                                width,
                                height);

                        tfBounds = new Rectangle(tfRectangle.Left + -1,
                                tfRectangle.Top - 1,
                                tfRectangle.Width + 2,
                                tfRectangle.Height + 2);

                    }
                    imageBounds = Rectangle.Union(imageBounds,tfBounds);
                }
                #endregion

                #region Build Feint Dummy Indicator
                Rectangle fdiBounds = Rectangle.Empty;
                Point fdiTop = Point.Empty;
                Point fdiLeft = Point.Empty;
                Point fdiRight = Point.Empty;

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

                    fdiBounds = new Rectangle(fdiLeft.X, fdiLeft.Y, 1, 1);
                    fdiBounds = ShapeUtilities.union(fdiBounds,fdiTop);
                    fdiBounds = ShapeUtilities.union(fdiBounds,fdiRight);

                    if (echelonBounds != null)
                    {
                        int shiftY = (symbolBounds.Top - echelonBounds.Height - 2);
                        fdiLeft.Offset(0, shiftY);
                        fdiTop.Offset(0, shiftY);
                        fdiRight.Offset(0, shiftY);
                        fdiBounds.Offset(0, shiftY);
                    }

                    imageBounds = Rectangle.Union(imageBounds,fdiBounds);

                }
                #endregion

                #region Build Installation
                Rectangle instRectangle = Rectangle.Empty;
                Rectangle instBounds = Rectangle.Empty;
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
                        instRectangle = new Rectangle((int)(symbolBounds.Left + width),
                                (int)(symbolBounds.Top - height),
                                width,
                                height);
                    }
                    else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                    {
                        instRectangle = new Rectangle((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.15f)),
                                width,
                                height);
                    }
                    else if (affiliation == 'P'
                            || affiliation == 'U'
                            || affiliation == 'G'
                            || affiliation == 'W')
                    {
                        instRectangle = new Rectangle((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.3f)),
                                width,
                                height);
                    }
                    else
                    {
                        instRectangle = new Rectangle((int)symbolBounds.Left + width,
                                ShapeUtilities.round(symbolBounds.Top - (height * 0.3f)),
                                width,
                                height);
                    }

                    /*instRectangle = new SO.Rectangle(symbolBounds.Left + width,
                     symbolBounds.Top - height,
                     width,
                     height);//*/
                    //generate installation bounds//////////////////////////////
                    instBounds = new Rectangle(instRectangle.Left + -1,
                            instRectangle.Top - 1,
                            instRectangle.Width + 2,
                            instRectangle.Height + 2);

                    imageBounds = Rectangle.Union(imageBounds,instBounds);

                }
                #endregion

                #region Build HQ Staff
                Point pt1HQ = Point.Empty;
                Point pt2HQ = Point.Empty;
                Rectangle hqBounds = Rectangle.Empty;
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

                    //create bounding rectangle for HQ staff.
                    hqBounds = new Rectangle(pt1HQ.X, pt1HQ.Y, 2, pt2HQ.Y - pt1HQ.Y);
                    //adjust the image bounds accordingly.
                    imageBounds.Height = imageBounds.Height + (pt2HQ.Y - imageBounds.Bottom);
                    imageBounds = Rectangle.Union(imageBounds, hqBounds);
                    //adjust symbol center
                    centerPoint.X = pt2HQ.X;
                    centerPoint.Y = pt2HQ.Y;
                }
                #endregion

                #region Build DOM Arrow
                Point[] domPoints = null;
                Rectangle domBounds = Rectangle.Empty;
                if (modifiers != null && modifiers.ContainsKey(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT))
                {
                    String strQ = modifiers[ModifiersUnits.Q_DIRECTION_OF_MOVEMENT];

                    if (strQ != null && SymbolUtilities.isNumber(strQ))
                    {
                        float q = (float)Convert.ToDouble(strQ);

                        Boolean isY = (modifiers.ContainsKey(ModifiersUnits.Y_LOCATION));

                        domPoints = createDOMArrowPoints(symbolID, symbolBounds, centerPoint, q, isY);

                        domBounds = new Rectangle(domPoints[0].X, domPoints[0].Y, 1, 1);

                        Point temp = Point.Empty;
                        for (int i = 1; i < 6; i++)
                        {
                            temp = domPoints[i];
                            if (temp.IsEmpty == false)
                            {
                                domBounds = ShapeUtilities.union(domBounds, temp);
                            }
                        }
                        domBounds.Inflate(1, 1);
                        imageBounds = Rectangle.Union(imageBounds, domBounds);
                    }
                }
                #endregion

                #region Build Operational Condition Indicator
                Rectangle ociBounds = Rectangle.Empty;
                int ociOffset = 4;
                if (mobilityBounds != Rectangle.Empty)
                {
                    ociOffset = ShapeUtilities.round(mobilityBounds.Height);
                }
                Rectangle ociShape = processOperationalConditionIndicator(symbolID, symbolBounds, ociOffset);
                if (ociShape != Rectangle.Empty)
                {
                    Rectangle temp = ShapeUtilities.clone(ociShape);
                    temp.Inflate(2, 3);
                    ociBounds = temp;
                    imageBounds = Rectangle.Union(imageBounds,ociBounds);
                }
                #endregion

                #region Shift Modifiers
                if (imageBounds.Left < 0 || imageBounds.Top < 0)
                {
                    int shiftX = Math.Abs(imageBounds.Left);
                    int shiftY = Math.Abs(imageBounds.Top);

                    if (hqBounds != Rectangle.Empty)
                    {
                        pt1HQ.Offset(shiftX, shiftY);
                        pt2HQ.Offset(shiftX, shiftY);
                    }
                    if (echelonBounds != Rectangle.Empty)
                    {
                        tiEchelon.setLocation(tiEchelon.getLocation().X + shiftX, tiEchelon.getLocation().Y + shiftY);
                    }
                    if (amBounds != Rectangle.Empty)
                    {
                        tiAM.setLocation(tiAM.getLocation().X + shiftX, tiAM.getLocation().Y + shiftY);
                    }
                    if (tfBounds != Rectangle.Empty)
                    {
                        tfRectangle.Offset(shiftX, shiftY);
                        tfBounds.Offset(shiftX, shiftY);
                    }
                    if (instBounds != Rectangle.Empty)
                    {
                        instRectangle.Offset(shiftX, shiftY);
                        instBounds.Offset(shiftX, shiftY);
                    }
                    if (fdiBounds != Rectangle.Empty)
                    {
                        fdiBounds.Offset(shiftX, shiftY);
                        fdiLeft.Offset(shiftX, shiftY);
                        fdiTop.Offset(shiftX, shiftY);
                        fdiRight.Offset(shiftX, shiftY);
                    }
                    if (ociBounds != Rectangle.Empty)
                    {
                        ociBounds.Offset(shiftX, shiftY);
                        ociShape.Offset(shiftX, shiftY);
                    }
                    if (domBounds != Rectangle.Empty)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Point temp = domPoints[i];
    
                            if(temp.IsEmpty==false)
                                temp.Offset(shiftX, shiftY);

                            domPoints[i] = temp;
                            
                        }
                        domBounds.Offset(shiftX, shiftY);
                    }
                    if (mobilityBounds != Rectangle.Empty)
                    {
                        Matrix translation = new Matrix();
                        translation.Translate(shiftX,shiftY);
                        //shift mobility points
                        mobilityPath.Transform(translation);
                        if (mobilityPathFill != null)
                        {
                            mobilityPathFill.Transform(translation);
                        }

                        mobilityBounds.Offset(shiftX, shiftY);
                    }

                    centerPoint.Offset(shiftX, shiftY);
                    symbolBounds.Offset(shiftX, shiftY);
                    imageBounds.Offset(shiftX, shiftY);
                }
                #endregion

                #region Draw Modifiers

                Bitmap bmp = new Bitmap(imageBounds.Width, imageBounds.Height);
                Graphics g = Graphics.FromImage(bmp);


                //render////////////////////////////////////////////////////////

                Pen pen = new Pen(Color.Black,2f);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (hqBounds != Rectangle.Empty)
                {
                    g.DrawLine(pen,pt1HQ.X, pt1HQ.Y, pt2HQ.X, pt2HQ.Y);
                }

                if (tfBounds != Rectangle.Empty)
                {
                    g.DrawRectangle(pen, tfRectangle);
                }

                if (instBounds != Rectangle.Empty)
                {
                    g.FillRectangle(Brushes.Black, instRectangle);
                }

                if (echelonBounds != Rectangle.Empty)
                {
                    TextInfo[] aTiEchelon =
                    {
                        tiEchelon
                    };
                    renderText(g, aTiEchelon, textColor, textBackgroundColor);

                    echelonBounds = Rectangle.Empty;
                    tiEchelon = null;
                }

                if (amBounds != Rectangle.Empty)
                {
                    TextInfo[] aTiAM =
                    {
                        tiAM
                    };
                    renderText(g, aTiAM, textColor, textBackgroundColor);
                    amBounds = Rectangle.Empty;
                    tiAM = null;
                }

                if (fdiBounds != Rectangle.Empty)
                {

                    if (symbolBounds.Width > 19)
                    {
                        pen.DashPattern = new float[] { 6f, 4f };
                    }
                    else
                    {
                        pen.DashPattern = new float[] { 5f, 3f };
                    }
                    
                    pen.LineJoin = LineJoin.Miter;
                    pen.MiterLimit = 3;
                    pen.Width = 2;

                    GraphicsPath fdiPath = new GraphicsPath();

                    fdiPath.AddLine(fdiLeft.X, fdiLeft.Y, fdiTop.X, fdiTop.Y);
                    fdiPath.AddLine(fdiTop.X, fdiTop.Y, fdiRight.X, fdiRight.Y);

                    g.DrawPath(pen, fdiPath);

                    fdiBounds = Rectangle.Empty;

                }

                if (mobilityBounds != Rectangle.Empty)
                {
                    

                    //ctx.lineCap = "butt";
                    //ctx.lineJoin = "miter";
                    if (symbolID[10] == ('M'))
                    {
                        //mobilityPaint.setAntiAlias(true);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                    }
                    else //NS or NL
                    {
                        //mobilityPaint.setAntiAlias(false);
                        g.SmoothingMode = SmoothingMode.None;
                    }

                    g.DrawPath(mobilityPen, mobilityPath);

                    if (mobilityPathFill != null)
                    {
                        g.FillPath(Brushes.Black, mobilityPathFill);

                    }

                    mobilityBounds = Rectangle.Empty;

                }

                if (ociBounds != Rectangle.Empty)
                {
                    Color statusColor = Color.Black;
                    char status = symbolID[3];
                    if (status == ('C'))//Fully Capable
                    {
                        statusColor = Color.Green;
                    }
                    else if (status == ('D'))//Damage
                    {
                        statusColor = Color.Yellow;
                    }
                    else if (status == ('X'))
                    {
                        statusColor = Color.Red;
                    }
                    else if (status == ('F'))//full to capacity(hospital)
                    {
                        statusColor = Color.Blue;
                    };

                    g.FillRectangle(Brushes.Black, ociBounds);
                    g.FillRectangle(new SolidBrush(statusColor), ociShape);

                    ociBounds = Rectangle.Empty;
                    ociShape = Rectangle.Empty;
                }

                //draw original icon.        
                //ctx.drawBitmap(ii.getImage(), null, symbolBounds, null);
                g.DrawImageUnscaled(ii.getBitmap(), symbolBounds.X, symbolBounds.Y);
                
                if (domBounds != Rectangle.Empty)
                {
                    drawDOMArrow(g, domPoints);

                    domBounds = Rectangle.Empty;
                    domPoints = null; ;
                }

                #endregion
                /*GraphicsUnit pixel = GraphicsUnit.Pixel;
                Rectangle outline = ShapeUtilities.cloneToRectangle(bmp.GetBounds(ref pixel));
                outline.Width = outline.Width - 1;
                outline.Height = outline.Height - 1;
                g.DrawRectangle(Pens.Red, outline);//*/

                newii = new ImageInfo(bmp, centerPoint, symbolBounds);

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
         * @param {type} bounds symbolBounds SO.Rectangle
         * @param {type} center SO.Point Location where symbol is centered.
         * @param {type} angle in degrees
         * @param {Boolean} isY Boolean.
         * @returns {Array} of SO.Point. First 3 items are the line. Last three are
         * the arrowhead.
         */
        private static Point[] createDOMArrowPoints(String symbolID, Rectangle bounds, Point center, float angle, Boolean isY)
        {
            Point[] arrowPoints = new Point[6];
            Point pt1 = Point.Empty;
            Point pt2 = Point.Empty;
            Point pt3 = Point.Empty;

            int length = 40;
            if (SymbolUtilities.isNBC(symbolID))
            {
                length = ShapeUtilities.round(bounds.Height / 2);
            }
            else
            {
                length = bounds.Height;
            }

            //get endpoint
            int dx2, dy2,
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

                if (isY == true && SymbolUtilities.isNBC(symbolID))//make room for y modifier
                {
                    //TODO: calculate this value
                    int yModifierOffset = TextRenderer.MeasureText("Y", RS.getLabelFont()).Height;

                    yModifierOffset += RS.getTextOutlineWidth();

                    pt1.Offset(0, yModifierOffset);
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

            int[] xPoints = new int[3];//3
            int[] yPoints = new int[3];//3
            int[] vecLine = new int[2];//2
            int[] vecLeft = new int[2];//2
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

        private static void drawDOMArrow(Graphics g, Point[] domPoints)
        {
            Pen pen = new Pen(Color.Black,3f);

            pen.MiterLimit = 3;

            GraphicsPath domPath = new GraphicsPath();
            
            if (domPoints[1].IsEmpty == true)
            {
                domPath.AddLine(domPoints[0].X, domPoints[0].Y, domPoints[2].X, domPoints[2].Y);
            }
            else
            {
                domPath.AddLine(domPoints[0].X, domPoints[0].Y, domPoints[1].X, domPoints[1].Y);
                domPath.AddLine(domPoints[1].X, domPoints[1].Y, domPoints[2].X, domPoints[2].Y);
            }
            g.DrawPath(pen, domPath);

            domPath.Reset();
            domPath.AddLine(domPoints[3].X, domPoints[3].Y,domPoints[4].X, domPoints[4].Y);
            domPath.AddLine(domPoints[4].X, domPoints[4].Y, domPoints[5].X, domPoints[5].Y);
            domPath.CloseFigure();
            g.FillPath(Brushes.Black, domPath);
        }

        private static Rectangle processOperationalConditionIndicator(String symbolID, Rectangle symbolBounds, int offsetY)
        {
            // <editor-fold defaultstate="collapsed" desc="Operational Condition Indicator">
            //create Operational Condition Indicator
            //set color
            Rectangle bar = Rectangle.Empty;
            char status;
            int barSize = 0;
            int pixelSize = symbolBounds.Height;

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

                bar = new Rectangle(symbolBounds.Left + 2, offsetY, ShapeUtilities.round(symbolBounds.Width) - 4, barSize);
                //bar = new SO.Rectangle(symbolBounds.Left+1, offsetY, ShapeUtilities.round(symbolBounds.Width)-2,barSize);
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

        private static void checkModifierFont()
        {
            if (_fontSize != RS.getModifierFontSize() || _fontName.Equals(RS.getModifierFontName()) == false || _fontStyle != RS.getModifierFontStyle())
            {
                _modifierFontHeight = ShapeUtilities.round(_g.MeasureString("Hj", RS.getLabelFont()).Height);
            }

            /*private readonly object syncLock = new object();
            public void SomeMethod() {
            lock(syncLock) { code }
            }*/

        }

        public static ImageInfo ProcessUnitTextModifiers(ImageInfo ii, String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes)
        {
            checkModifierFont();
            int bufferXL = 5;
            int bufferXR = 5;
            int bufferY = 2;
            int bufferText = 2;
            int x = 0;
            int y = 0;//best y
            int cpofNameX = 0;
            ImageInfo newii = null;

            Color textColor = Color.Black;
            Color textBackgroundColor = Color.Empty;

            List<TextInfo> tiArray = new List<TextInfo>(modifiers.Count);

            int descent = 0;//(int)(_modifierFontDescent + 0.5);

            int symStd = RS.getSymbologyStandard();
            if (attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
            {
                symStd = Convert.ToInt32(attributes[MilStdAttributes.SymbologyStandard]);
            }

            Rectangle labelBounds = Rectangle.Empty;
            int labelWidth, labelHeight;

            Rectangle bounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Rectangle symbolBounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Point centerPoint = ShapeUtilities.clone(ii.getCenterPoint());
            Rectangle imageBounds = ShapeUtilities.cloneToRectangle(ii.getImageBounds());
            Rectangle imageBoundsOld = ShapeUtilities.cloneToRectangle(ii.getImageBounds());

            String echelon = SymbolUtilities.getEchelon(symbolID);
            String echelonText = SymbolUtilities.getEchelonText(echelon);
            String amText = SymbolUtilities.getUnitAffiliationModifier(symbolID, symStd);

            //make room for echelon & mobility.
            if (modifiers.ContainsKey(ModifiersUnits.Q_DIRECTION_OF_MOVEMENT)==false)
            {
                //if no DOM, we can just use the image bounds
                bounds = new Rectangle(imageBounds.Left, symbolBounds.Top,
                        imageBounds.Width, symbolBounds.Height);
            }
            else //dom exists so we need to change our math
            {
                if (echelonText != null || amText != null)
                {
                    bounds = new Rectangle(imageBounds.Left, bounds.Top,
                            imageBounds.Width, bounds.Height);
                }
                else if (symbolID.Substring(10, 2).Equals("MR"))
                {
                    x = -(ShapeUtilities.round((symbolBounds.Width - 1) / 7) * 2);
                    if (x < bounds.Left)
                    {
                        int w = (bounds.Left - x) + bounds.Width;
                        bounds.X = x;
                        bounds.Y = 0;
                    }
                }
            }

            cpofNameX = bounds.Left + bounds.Width + bufferXR;

            //check if text is too tall:
            Boolean byLabelHeight = false;
            labelHeight = (int)(_modifierFontHeight + 0.5);/* RendererUtilities.measureTextHeight(RendererSettings.getModifierFontName(),
         RendererSettings.getModifierFontSize(),
         RendererSettings.getModifierFontStyle()).fullHeight;*/

            int maxHeight = (bounds.Height);
            if ((labelHeight * 3) > maxHeight)
            {
                byLabelHeight = true;
            }

            //Affiliation Modifier being drawn as a display modifier
            String affiliationModifier = null;
            if (RS.getDrawAffiliationModifierAsLabel() == true)
            {
                affiliationModifier = SymbolUtilities.getUnitAffiliationModifier(symbolID, symStd);
            }
            if (affiliationModifier != null)
            {   //Set affiliation modifier
                modifiers.Add(ModifiersUnits.E_FRAME_SHAPE_MODIFIER, affiliationModifier);
                //modifiers[ModifiersUnits.E_FRAME_SHAPE_MODIFIER] = affiliationModifier;
            }//*/

            //Check for Valid Country Code
            if (SymbolUtilities.hasValidCountryCode(symbolID))
            {
                modifiers.Add(ModifiersUnits.CC_COUNTRY_CODE, symbolID.Substring(12, 2));
                //modifiers[ModifiersUnits.CC_COUNTRY_CODE] = symbolID.substring(12,14);
            }

            //            int y0 = 0;//W    E/F
            //            int y1 = 0;//X/Y  G
            //            int y2 = 0;//V    H 
            //            int y3 = 0;//T    M CC
            //            int y4 = 0;//Z    J/K/L/N/P
            //
            //            y0 = bounds.y - 0;
            //            y1 = bounds.y - labelHeight;
            //            y2 = bounds.y - (labelHeight + (int)bufferText) * 2;
            //            y3 = bounds.y - (labelHeight + (int)bufferText) * 3;
            //            y4 = bounds.y - (labelHeight + (int)bufferText) * 4;
            // <editor-fold defaultstate="collapsed" desc="Build Modifiers">
            String modifierValue = null;
            TextInfo tiTemp = null;
            //if(ModifiersUnits.C_QUANTITY in modifiers 
            if (modifiers.ContainsKey(ModifiersUnits.C_QUANTITY) == true
                    && SymbolUtilities.canUnitHaveModifier(symbolID, ModifiersUnits.C_QUANTITY))
            {
                String text = modifiers[ModifiersUnits.C_QUANTITY];
                if (text != null)
                {
                    
                    tiTemp = new TextInfo(text, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;
                    x = ShapeUtilities.round((symbolBounds.Left + (symbolBounds.Width * 0.5f)) - (labelWidth * 0.5f));
                    y = ShapeUtilities.round(symbolBounds.Top - bufferY - descent);
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            //if(ModifiersUnits.X_ALTITUDE_DEPTH in modifiers || ModifiersUnits.Y_LOCATION in modifiers)
            if (modifiers.ContainsKey(ModifiersUnits.X_ALTITUDE_DEPTH) || modifiers.ContainsKey(ModifiersUnits.Y_LOCATION))
            {
                modifierValue = null;

                String xm = null,
                        ym = null;

                if (modifiers.ContainsKey(ModifiersUnits.X_ALTITUDE_DEPTH))
                {
                    xm = modifiers[ModifiersUnits.X_ALTITUDE_DEPTH];// xm = modifiers.X;
                }
                if (modifiers.ContainsKey(ModifiersUnits.Y_LOCATION))
                {
                    ym = modifiers[ModifiersUnits.Y_LOCATION];// ym = modifiers.Y;
                }
                if (xm == null && ym != null)
                {
                    modifierValue = ym;
                }
                else if (xm != null && ym == null)
                {
                    modifierValue = xm;
                }
                else if (xm != null && ym != null)
                {
                    modifierValue = xm + "  " + ym;
                }

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    if (!byLabelHeight)
                    {
                        x = bounds.Left - labelBounds.Width - bufferXL;
                        y = bounds.Top + labelHeight - descent;
                    }
                    else
                    {
                        x = bounds.Left - labelBounds.Width - bufferXL;

                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y - ((labelHeight + bufferText));
                        y = bounds.Top + y;
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.G_STAFF_COMMENTS))
            {
                modifierValue = modifiers[ModifiersUnits.G_STAFF_COMMENTS];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = bounds.Left + bounds.Width + bufferXR;
                    if (!byLabelHeight)
                    {
                        y = bounds.Top + labelHeight - descent;
                    }
                    else
                    {
                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y - ((labelHeight + bufferText));
                        y = bounds.Top + y;
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);

                    //Concession for cpof name label
                    if ((x + labelWidth + 3) > cpofNameX)
                    {
                        cpofNameX = x + labelWidth + 3;
                    }
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.V_EQUIP_TYPE))
            {
                modifierValue = modifiers[ModifiersUnits.V_EQUIP_TYPE];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = bounds.Left - labelBounds.Width - bufferXL;

                    y = (bounds.Height);
                    y = (int)((y * 0.5f) + ((labelHeight - descent) * 0.5f));
                    y = bounds.Top + y;

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.H_ADDITIONAL_INFO_1))
            {
                modifierValue = modifiers[ModifiersUnits.H_ADDITIONAL_INFO_1];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = bounds.Left + bounds.Width + bufferXR;

                    y = (bounds.Height);
                    y = (int)((y * 0.5) + ((labelHeight - descent) * 0.5));
                    y = bounds.Top + y;

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);

                    //Concession for cpof name label
                    if ((x + labelWidth + 3) > cpofNameX)
                    {
                        cpofNameX = x + labelWidth + 3;
                    }
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.T_UNIQUE_DESIGNATION_1))
            {
                modifierValue = modifiers[ModifiersUnits.T_UNIQUE_DESIGNATION_1];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    if (!byLabelHeight)
                    {
                        x = bounds.Left - labelWidth - bufferXL;
                        y = bounds.Top + bounds.Height;
                    }
                    else
                    {
                        x = bounds.Left - labelWidth - bufferXL;

                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y + ((labelHeight + bufferText) - descent);
                        y = bounds.Top + y;
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.M_HIGHER_FORMATION) || modifiers.ContainsKey(ModifiersUnits.CC_COUNTRY_CODE))
            {
                modifierValue = "";

                if (modifiers.ContainsKey(ModifiersUnits.M_HIGHER_FORMATION))
                {
                    modifierValue += modifiers[ModifiersUnits.M_HIGHER_FORMATION];
                }
                if (modifiers.ContainsKey(ModifiersUnits.CC_COUNTRY_CODE))
                {
                    if (modifierValue.Length > 0)
                    {
                        modifierValue += " ";
                    }
                    modifierValue += modifiers[ModifiersUnits.CC_COUNTRY_CODE];
                }

                tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                labelBounds = tiTemp.getTextBounds();
                labelWidth = labelBounds.Width;

                x = bounds.Left + bounds.Width + bufferXR;
                if (!byLabelHeight)
                {
                    y = bounds.Top + bounds.Height;
                }
                else
                {
                    y = (bounds.Height);
                    y = (int)((y * 0.5) + (labelHeight * 0.5));

                    y = y + ((labelHeight + bufferText - descent));
                    y = bounds.Top + y;
                }

                y = y - labelHeight;
                tiTemp.setLocation(x, y);
                tiArray.Add(tiTemp);

                //Concession for cpof name label
                if ((x + labelWidth + 3) > cpofNameX)
                {
                    cpofNameX = x + labelWidth + 3;
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.Z_SPEED))
            {
                modifierValue = modifiers[ModifiersUnits.Z_SPEED];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = bounds.Left - labelWidth - bufferXL;
                    if (!byLabelHeight)
                    {
                        y = ShapeUtilities.round(bounds.Top + bounds.Height + labelHeight + bufferText);
                    }
                    else
                    {
                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y + ((labelHeight + bufferText) * 2) - (descent * 2);
                        y = ShapeUtilities.round(bounds.Top + y);
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.J_EVALUATION_RATING)
                    || modifiers.ContainsKey(ModifiersUnits.K_COMBAT_EFFECTIVENESS)
                    || modifiers.ContainsKey(ModifiersUnits.L_SIGNATURE_EQUIP)
                    || modifiers.ContainsKey(ModifiersUnits.N_HOSTILE)
                    || modifiers.ContainsKey(ModifiersUnits.P_IFF_SIF))
            {
                modifierValue = null;

                String jm = null,
                        km = null,
                        lm = null,
                        nm = null,
                        pm = null;

                if (modifiers.ContainsKey(ModifiersUnits.J_EVALUATION_RATING))
                {
                    jm = modifiers[ModifiersUnits.J_EVALUATION_RATING];
                }
                if (modifiers.ContainsKey(ModifiersUnits.K_COMBAT_EFFECTIVENESS))
                {
                    km = modifiers[ModifiersUnits.K_COMBAT_EFFECTIVENESS];
                }
                if (modifiers.ContainsKey(ModifiersUnits.L_SIGNATURE_EQUIP))
                {
                    lm = modifiers[ModifiersUnits.L_SIGNATURE_EQUIP];
                }
                if (modifiers.ContainsKey(ModifiersUnits.N_HOSTILE))
                {
                    nm = modifiers[ModifiersUnits.N_HOSTILE];
                }
                if (modifiers.ContainsKey(ModifiersUnits.P_IFF_SIF))
                {
                    pm = modifiers[ModifiersUnits.P_IFF_SIF];
                }

                modifierValue = "";
                if (jm != null && jm.Equals("") == false)
                {
                    modifierValue = modifierValue + jm;
                }
                if (km != null && km.Equals("") == false)
                {
                    modifierValue = modifierValue + " " + km;
                }
                if (lm != null && lm.Equals("") == false)
                {
                    modifierValue = modifierValue + " " + lm;
                }
                if (nm != null && nm.Equals("") == false)
                {
                    modifierValue = modifierValue + " " + nm;
                }
                if (pm != null && pm.Equals("") == false)
                {
                    modifierValue = modifierValue + " " + pm;
                }

                if (modifierValue.Length > 2 && modifierValue[0] == ' ')
                {
                    modifierValue = modifierValue.Substring(1);
                }

                tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                labelBounds = tiTemp.getTextBounds();
                labelWidth = labelBounds.Width;

                x = bounds.Left + bounds.Width + bufferXR;
                if (!byLabelHeight)
                {
                    y = ShapeUtilities.round(bounds.Top + bounds.Height + labelHeight + bufferText);
                }
                else
                {
                    y = (bounds.Height);
                    y = (int)((y * 0.5) + (labelHeight * 0.5));

                    y = y + ((labelHeight + bufferText) * 2) - (descent * 2);
                    y = ShapeUtilities.round(bounds.Top + y);
                }

                y = y - labelHeight;
                tiTemp.setLocation(x, y);
                tiArray.Add(tiTemp);

                //Concession for cpof name label
                if ((x + labelWidth + 3) > cpofNameX)
                {
                    cpofNameX = x + labelWidth + 3;
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.W_DTG_1))
            {
                modifierValue = modifiers[ModifiersUnits.W_DTG_1];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    if (!byLabelHeight)
                    {
                        x = bounds.Left - labelWidth - bufferXL;
                        y = bounds.Top - bufferY - descent;
                    }
                    else
                    {
                        x = bounds.Left - labelWidth - bufferXL;

                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y - ((labelHeight + bufferText) * 2);
                        y = bounds.Top + y;
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.F_REINFORCED_REDUCED) || modifiers.ContainsKey(ModifiersUnits.E_FRAME_SHAPE_MODIFIER))
            {
                modifierValue = null;
                String E = null,
                        F = null;

                if (modifiers.ContainsKey(ModifiersUnits.E_FRAME_SHAPE_MODIFIER))
                {
                    E = modifiers[ModifiersUnits.E_FRAME_SHAPE_MODIFIER];
                }
                if (modifiers.ContainsKey(ModifiersUnits.F_REINFORCED_REDUCED))
                {
                    F = modifiers[ModifiersUnits.F_REINFORCED_REDUCED];
                }

                if (E != null && E.Equals("") == false)
                {
                    modifierValue = E;
                }

                if (F != null && F.Equals("") == false)
                {
                    if (F.ToUpper(new CultureInfo("en-US",false)).Equals("R"))
                    {
                        F = "(+)";
                    }
                    else if (F.ToUpper(new CultureInfo("en-US",false)).Equals("D"))
                    {
                        F = "(-)";
                    }
                    else if (F.ToUpper(new CultureInfo("en-US",false)).Equals("RD"))
                    {
                        F = "(" + (char)(177) + ")";
                    }
                }

                if (F != null && F.Equals("") == false)
                {
                    if (modifierValue != null && modifierValue.Equals("") == false)
                    {
                        modifierValue = modifierValue + " " + F;
                    }
                    else
                    {
                        modifierValue = F;
                    }
                }

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    if (!byLabelHeight)
                    {
                        x = bounds.Left + bounds.Width + bufferXR;
                        y = bounds.Top - bufferY - descent;
                    }
                    else
                    {
                        x = bounds.Left + bounds.Width + bufferXR;

                        y = (bounds.Height);
                        y = (int)((y * 0.5) + (labelHeight * 0.5));

                        y = y - ((labelHeight + bufferText) * 2);
                        y = bounds.Top + y;
                    }

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);

                    //Concession for cpof name label
                    if ((x + labelWidth + 3) > cpofNameX)
                    {
                        cpofNameX = x + labelWidth + 3;
                    }
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.AA_SPECIAL_C2_HQ))
            {
                modifierValue = modifiers[ModifiersUnits.AA_SPECIAL_C2_HQ];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = (int)((symbolBounds.Left + (symbolBounds.Width * 0.5f)) - (labelWidth * 0.5f));

                    y = (symbolBounds.Height);//checkpoint, get box above the point
                    y = (int)((y * 0.5) + ((labelHeight - descent) * 0.5));
                    y = symbolBounds.Top + y;

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.SCC_SONAR_CLASSIFICATION_CONFIDENCE))
            {
                modifierValue = modifiers[ModifiersUnits.SCC_SONAR_CLASSIFICATION_CONFIDENCE];

                if (modifierValue != null && SymbolUtilities.isNumber(modifierValue) && SymbolUtilities.hasModifier(symbolID, ModifiersUnits.SCC_SONAR_CLASSIFICATION_CONFIDENCE,symStd))
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = (int)((symbolBounds.Left + (symbolBounds.Width * 0.5f)) - (labelWidth * 0.5f));

                    double yPosition = getYPositionForSCC(symbolID);
                    y = (bounds.Height);//checkpoint, get box above the point
                    y = (int)(((y * yPosition) + ((labelHeight - descent) * 0.5)));
                    y = bounds.Top + y;

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            if (modifiers.ContainsKey(ModifiersUnits.CN_CPOF_NAME_LABEL))
            {
                modifierValue = modifiers[ModifiersUnits.CN_CPOF_NAME_LABEL];

                if (modifierValue != null)
                {
                    tiTemp = new TextInfo(modifierValue, 0, 0, _modifierFont,_g);
                    labelBounds = tiTemp.getTextBounds();
                    labelWidth = labelBounds.Width;

                    x = cpofNameX;

                    y = (bounds.Height);//checkpoint, get box above the point
                    y = (int)((y * 0.5) + (labelHeight * 0.5));
                    y = bounds.Top + y;

                    y = y - labelHeight;
                    tiTemp.setLocation(x, y);
                    tiArray.Add(tiTemp);
                }
            }

            // </editor-fold>
            // <editor-fold defaultstate="collapsed" desc="Shift Points and Draw">
            Rectangle modifierBounds = Rectangle.Empty;
            if (tiArray != null && tiArray.Count > 0)
            {

                //build modifier bounds/////////////////////////////////////////
                modifierBounds = tiArray[0].getTextOutlineBounds();
                int size = tiArray.Count;
                TextInfo tempShape = null;
                for (int i = 1; i < size; i++)
                {
                    tempShape = tiArray[i];

                    modifierBounds = Rectangle.Union(modifierBounds, tempShape.getTextOutlineBounds());
                }

            }

            if (modifierBounds != null)
            {

                imageBounds = Rectangle.Union(imageBounds, modifierBounds);

                //shift points if needed////////////////////////////////////////
                if (imageBounds.Left < 0 || imageBounds.Top < 0)
                {
                    int shiftX = ShapeUtilities.round(Math.Abs(imageBounds.Left)),
                            shiftY = ShapeUtilities.round(Math.Abs(imageBounds.Top));

                    //shift mobility points
                    int size = tiArray.Count;
                    TextInfo tempShape = null;
                    for (int i = 0; i < size; i++)
                    {
                        tempShape = tiArray[i];
                        tempShape.shift(shiftX, shiftY);
                    }
                    modifierBounds.Offset(shiftX, shiftY);

                    //shift image points
                    centerPoint.Offset(shiftX, shiftY);
                    symbolBounds.Offset(shiftX, shiftY);
                    imageBounds.Offset(shiftX, shiftY);
                    imageBoundsOld.Offset(shiftX, shiftY);
                    /*centerPoint.shift(shiftX, shiftY);
                     symbolBounds.shift(shiftX, shiftY);
                     imageBounds.shift(shiftX, shiftY);
                     imageBoundsOld.shift(shiftX, shiftY);//*/
                }

                Bitmap bmp = new Bitmap(imageBounds.Width, imageBounds.Height);
                Graphics g = Graphics.FromImage(bmp);

                //render////////////////////////////////////////////////////////
                //draw original icon with potential modifiers.
                g.DrawImageUnscaled(ii.getBitmap(), imageBoundsOld.X, imageBoundsOld.Y);
                //ctx.drawImage(ii.getImage(),imageBoundsOld.Left,imageBoundsOld.Top);


                if (attributes.ContainsKey(MilStdAttributes.TextColor))
                {
                    textColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextColor]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextBackgroundColor))
                {
                    textBackgroundColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextBackgroundColor]);
                }

                renderText(g, tiArray, textColor, textBackgroundColor);

                newii = new ImageInfo(bmp, centerPoint, symbolBounds);

            }
            // </editor-fold>

            // <editor-fold defaultstate="collapsed" desc="Cleanup">
            tiArray = null;
            tiTemp = null;
            //tempShape = null;
            imageBoundsOld = Rectangle.Empty;
            //ctx = null;
            //buffer = null;
            // </editor-fold>

            return newii;
        }


        public static ImageInfo ProcessTGSPWithSpecialModifierLayout(ImageInfo ii, String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes, Color lineColor)
        {
            int bufferXL = 6;
            int bufferXR = 4;
            int bufferY = 2;
            int bufferText = 2;
            int centerOffset = 1; //getCenterX/Y function seems to go over by a pixel
            int x = 0;
            int y = 0;
            int x2 = 0;
            int y2 = 0;
            int symStd = RS.getSymbologyStandard();
            int outlineOffset = RS.getTextOutlineWidth();
            int labelHeight = 0;
            int labelWidth = 0;
            ImageInfo newii = null;
            Color textColor = lineColor;
            Color textBackgroundColor = Color.Empty;

            List<TextInfo> arrMods = new List<TextInfo>();
            Boolean duplicate = false;

            Rectangle bounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Rectangle symbolBounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Point centerPoint = ShapeUtilities.clone(ii.getCenterPoint());
            Rectangle imageBounds = ShapeUtilities.cloneToRectangle(ii.getImageBounds());

            if (attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
            {
                symStd = Convert.ToInt32(attributes[MilStdAttributes.SymbologyStandard]);
            }

            Boolean byLabelHeight = false;
            labelHeight = (int)(_modifierFontHeight + 0.5f);

            int maxHeight = (symbolBounds.Height);
            if ((labelHeight * 3) > maxHeight)
            {
                byLabelHeight = true;
            }

            int descent = 0;//(int)(_modifierFontDescent + 0.5f);
            int yForY = -1;

            Rectangle labelBounds1 = Rectangle.Empty;//text.getPixelBounds(null, 0, 0);
            Rectangle labelBounds2 = Rectangle.Empty;
            String strText = "";
            String strText1 = "";
            String strText2 = "";
            TextInfo text1 = null;
            TextInfo text2 = null;

            String basicID = SymbolUtilities.getBasicSymbolID(symbolID);

            if (outlineOffset > 2)
            {
                outlineOffset = ((outlineOffset - 1) / 2);
            }
            else
            {
                outlineOffset = 0;
            }

            // <editor-fold defaultstate="collapsed" desc="Process Integral Text">
            if (basicID.Equals("G*G*GPRD--****X"))//DLRP (D)
            {

                strText1 = "D";

                text1 = new TextInfo(strText1, 0, 0, _modifierFont,_g);

                labelBounds1 = text1.getTextBounds();
                if (symStd == MilStd.Symbology_2525Bch2_USAS_13_14)
                {
                    y = symbolBounds.Top + symbolBounds.Height;
                    x = symbolBounds.Left - labelBounds1.Width - bufferXL;
                    
                    y = y - labelHeight;
                    text1.setLocation(x, y);
                }
                else//2525C built in
                {
                    text1 = null;
                    //y = symbolBounds.Top + symbolBounds.getHeight() - bufferY;
                    //x = symbolBounds.Left + symbolBounds.Width/2 - labelBounds1.getWidth()/2;
                }

                //ErrorLogger.LogMessage("D: " + String.valueOf(x)+ ", " + String.valueOf(y));
            }
            else if (basicID.Equals("G*G*APU---****X")) //pull-up point (PUP)
            {
                strText1 = "PUP";
                text1 = new TextInfo(strText1, 0, 0, _modifierFont,_g);

                labelBounds1 = text1.getTextBounds();
                y = ShapeUtilities.getCenterY(symbolBounds) + ((labelBounds1.Height - descent) / 2);
                x = symbolBounds.Left + symbolBounds.Width + bufferXR;

                y = y - labelHeight;
                text1.setLocation(x, y);
            }
            else if (basicID.Equals("G*M*NZ----****X")) //Nuclear Detonation Ground Zero (N)
            {
                //                strText1 = "N";
                //                text1 = new TextLayout(strText1, labelFont, frc);
                //                labelBounds1 = text1.getPixelBounds(null, 0, 0);
                //                y = symbolBounds.Top + (symbolBounds.getHeight() * 0.8) - centerOffset;
                //                x = symbolBounds.getCenterX() - centerOffset - (labelBounds1.getWidth()/2);
            }
            else if (basicID.Equals("G*M*NF----****X"))//Fallout Producing (N)
            {
                //                strText1 = "N";
                //                text1 = new TextLayout(strText1, labelFont, frc);
                //                descent = text1.getDescent();
                //                labelBounds1 = text1.getPixelBounds(null, 0, 0);
                //                y = symbolBounds.Top + (symbolBounds.getHeight() * 0.8) - centerOffset;
                //                x = symbolBounds.getCenterX() - centerOffset - (labelBounds1.getWidth()/2);
            }
            else if (basicID.Equals("G*M*NEB---****X"))//Release Events Biological (BIO, B)
            {
                //strText1 = "B";
                //text1 = new TextLayout(strText1, labelFont, frc);
                int offset = 1;
                strText2 = "BIO";
                text2 = new TextInfo(strText2, 0, 0, _modifierFont,_g);

                labelBounds2 = text2.getTextBounds();
                //y = symbolBounds.Top + (symbolBounds.getHeight() * 0.9);
                //x = symbolBounds.getCenterX() - centerOffset - (labelBounds1.getWidth()/2);

                y2 = (int)(ShapeUtilities.getCenterY(symbolBounds) + ((labelBounds2.Height - descent) * 0.5f));

                x2 = symbolBounds.Left - labelBounds2.Width - bufferXL;

                y2 = y2 - labelHeight;
                text2.setLocation(x2, y2 - offset);
                //ErrorLogger.LogMessage("BIO: " + String.valueOf(x2)+ ", " + String.valueOf(y2));
            }
            else if (basicID.Equals("G*M*NEC---****X"))//Release Events Chemical (CML, C)
            {
                //strText1 = "C";
                //text1 = new TextLayout(strText1, labelFont, frc);
                int offset = 1;
                strText2 = "CML";
                text2 = new TextInfo(strText2, 0, 0, _modifierFont,_g);

                labelBounds2 = text2.getTextBounds();
                //y = symbolBounds.Top + (symbolBounds.getHeight() * 0.9);
                //x = symbolBounds.getCenterX() - centerOffset - (labelBounds1.getWidth()/2);

                y2 = ShapeUtilities.getCenterY(symbolBounds) + ((labelBounds2.Height - descent) / 2);

                x2 = symbolBounds.Left - labelBounds2.Width - bufferXL;

                y2 = y2 - labelHeight;
                text2.setLocation(x2, (y2 - offset));
            }
            if (text1 != null)
            {
                arrMods.Add(text1);
            }
            if (text2 != null)
            {
                arrMods.Add(text2);
            }

            // </editor-fold>
            // <editor-fold defaultstate="collapsed" desc="Process Special Modifiers">
            TextInfo ti = null;
            if (basicID.Equals("G*M*NZ----****X") ||//ground zero
                    basicID.Equals("G*M*NEB---****X") ||//biological
                    basicID.Equals("G*M*NEC---****X"))//chemical
            {
                if ((labelHeight * 3) > bounds.Height)
                {
                    byLabelHeight = true;
                }
            }

            if (basicID.Equals("G*G*GPPC--****X")
                    || basicID.Equals("G*G*GPPD--****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = (ti.getTextBounds().Width);
                        //One modifier symbols and modifier goes in center
                        x = bounds.Left + (int)(bounds.Width * 0.5f);
                        x = x - (int)(labelWidth * 0.5f);
                        y = bounds.Top + (int)(bounds.Height * 0.4f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
            }
            else if (basicID.Equals("G*G*GPH---****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                {
                    strText = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = (ti.getTextBounds().Width);
                        //One modifier symbols and modifier goes in center
                        x = bounds.Left + (int)(bounds.Width * 0.5f);
                        x = x - (int)(labelWidth * 0.5f);
                        y = bounds.Top + (int)(bounds.Height * 0.5f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
            }
            else if (basicID.Equals("G*G*GPRI--****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        //One modifier symbols, top third & center
                        x = bounds.Left + (int)(bounds.Width * 0.5f);
                        x = x - (int)(labelWidth * 0.5f);
                        y = bounds.Top + (int)(bounds.Height * 0.25f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
            }
            else if (basicID.Equals("G*G*GPPW--****X")
                    || basicID.Equals("G*F*PCF---****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        //One modifier symbols and modifier goes right of center
                        x = bounds.Left + (int)(bounds.Width * 0.75f);
                        y = bounds.Top + (int)(bounds.Height * 0.5f);
                        y = y + (int)((labelHeight - descent) * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
            }
            else if (basicID.Equals("G*G*APP---****X")
                    || basicID.Equals("G*G*APC---****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ti.getTextBounds().Width;
                        //One modifier symbols and modifier goes just below of center
                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        y = bounds.Top + (int)(bounds.Height * 0.5f);
                        y = y + (int)(((bounds.Height * 0.5f) - labelHeight) / 2) + labelHeight - descent;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
            }
            else if (basicID.Equals("G*G*DPT---****X") || //T (target reference point)
                    basicID.Equals("G*F*PTS---****X") || //t,h,h1 (Point/Single Target)
                    basicID.Equals("G*F*PTN---****X")) //T (nuclear target)
            { //Targets with special modifier positions
                if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1)
                        && basicID.Equals("G*F*PTS---****X"))//H
                {
                    strText = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        x = ShapeUtilities.getCenterX(bounds) + (int)(bounds.Width * 0.15f);
                        y = bounds.Top + (int)(bounds.Height * 0.75f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.H1_ADDITIONAL_INFO_2)
                        && basicID.Equals("G*F*PTS---****X"))//H1
                {
                    strText = modifiers[ModifiersTG.H1_ADDITIONAL_INFO_2];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = ShapeUtilities.getCenterX(bounds) - (int)(bounds.Width * 0.15f);
                        x = x - (labelWidth);
                        y = bounds.Top + (int)(bounds.Height * 0.75f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        x = ShapeUtilities.getCenterX(bounds) + (int)(bounds.Width * 0.15f);
                        //                  x = x - (labelBounds.width * 0.5);
                        y = bounds.Top + (int)(bounds.Height * 0.25f);
                        y = y + (int)(labelHeight * 0.5f);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }

            }
            else if (basicID.Equals("G*M*NZ----****X") ||//ground zero
                    basicID.Equals("G*M*NEB---****X") ||//biological
                    basicID.Equals("G*M*NEC---****X"))//chemical
            {//NBC
                if (modifiers.ContainsKey(ModifiersTG.N_HOSTILE))
                {
                    strText = modifiers[ModifiersTG.N_HOSTILE];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        x = bounds.Left + bounds.Width + bufferXR;

                        if (!byLabelHeight)
                        {
                            y = bounds.Top + bounds.Height;
                        }
                        else
                        {
                            y = bounds.Top + (int)((bounds.Height * 0.5f) + ((labelHeight - descent) * 0.5) + (labelHeight - descent + bufferText));
                        }

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }

                }
                if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                {
                    strText = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        x = bounds.Left + bounds.Width + bufferXR;
                        if (!byLabelHeight)
                        {
                            y = bounds.Top + labelHeight - descent;
                        }
                        else
                        {
                            //y = bounds.y + ((bounds.height * 0.5) + (labelHeight * 0.5) - (labelHeight + bufferText));
                            y = bounds.Top + (int)((bounds.Height * 0.5f) - ((labelHeight - descent) * 0.5) + (-descent - bufferText));
                        }

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.W_DTG_1))
                {
                    strText = modifiers[ModifiersTG.W_DTG_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        x = bounds.Left - labelWidth - bufferXL;
                        if (!byLabelHeight)
                        {
                            y = bounds.Top + labelHeight - descent;
                        }
                        else
                        {
                            //y = bounds.y + ((bounds.height * 0.5) + (labelHeight * 0.5) - (labelHeight + bufferText));
                            y = bounds.Top + (int)((bounds.Height * 0.5) - ((labelHeight - descent) * 0.5) + (-descent - bufferText));
                        }

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (basicID.Equals("G*M*NZ----****X") == true && modifiers.ContainsKey(ModifiersTG.V_EQUIP_TYPE))
                {
                    strText = modifiers[ModifiersTG.V_EQUIP_TYPE];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);

                        //subset of nbc, just nuclear
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = bounds.Left - labelWidth - bufferXL;
                        y = bounds.Top + (int)((bounds.Height * 0.5) + ((labelHeight - descent) * 0.5));//((bounds.height / 2) - (labelHeight/2));

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = bounds.Left - labelWidth - bufferXL;
                        if (!byLabelHeight)
                        {
                            y = bounds.Top + bounds.Height;
                        }
                        else
                        {
                            //y = bounds.y + ((bounds.height * 0.5) + ((labelHeight-descent) * 0.5) + (labelHeight + bufferText));
                            y = bounds.Top + (int)((bounds.Height * 0.5) + ((labelHeight - descent) * 0.5) + (labelHeight - descent + bufferText));
                        }
                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.Y_LOCATION))
                {
                    strText = modifiers[ModifiersTG.Y_LOCATION];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        //just NBC
                        //x = bounds.getX() + (bounds.getWidth() * 0.5);
                        //x = x - (labelWidth * 0.5);
                        x = bounds.Left + (int)(bounds.Width * 0.5f);
                        x = x - (int)(labelWidth * 0.5f);

                        if (!byLabelHeight)
                        {
                            y = bounds.Top + bounds.Height + labelHeight - descent + bufferY;
                        }
                        else
                        {
                            y = bounds.Top + (int)((bounds.Height * 0.5) + ((labelHeight - descent) * 0.5) + ((labelHeight + bufferText) * 2) - descent);

                        }
                        yForY = y + descent; //so we know where to start the DOM arrow.

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }

                }
                if (modifiers.ContainsKey(ModifiersTG.C_QUANTITY))
                {
                    strText = modifiers[ModifiersTG.C_QUANTITY];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        //subset of NBC, just nuclear
                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        y = bounds.Top - descent;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }

                }
            }
            else if (basicID.Equals("G*M*OFS---****X"))
            {
                if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                {
                    strText = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        y = bounds.Top - descent;// + (bounds.height * 0.5);
                        //y = y + (labelHeight * 0.5);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }

                }
                if (modifiers.ContainsKey(ModifiersTG.W_DTG_1))
                {
                    strText = modifiers[ModifiersTG.W_DTG_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        y = bounds.Top + (bounds.Height);
                        y = y + (labelHeight);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.N_HOSTILE))
                {
                    strText = modifiers[ModifiersTG.N_HOSTILE];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        TextInfo ti2 = new TextInfo(strText, 0, 0, _modifierFont,_g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);
                        x = bounds.Left + (bounds.Width) + bufferXR;//right
                        //x = x + labelWidth;//- (labelBounds.width * 0.75);

                        duplicate = true;

                        x2 = bounds.Left;//left
                        x2 = x2 - labelWidth - bufferXL;// - (labelBounds.width * 0.25);

                        y = bounds.Top + (int)(bounds.Height * 0.5);//center
                        y = y + (int)((labelHeight - descent) * 0.5);

                        y2 = y;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        ti2.setLocation(ShapeUtilities.round(x2), ShapeUtilities.round(y2));
                        arrMods.Add(ti);
                        arrMods.Add(ti2);
                    }
                }

            }
            // </editor-fold>

            // <editor-fold defaultstate="collapsed" desc="DOM Arrow">
            Point[] domPoints = null;
            Rectangle domBounds = Rectangle.Empty;
            if (modifiers.ContainsKey(ModifiersTG.Q_DIRECTION_OF_MOVEMENT)
                    && (basicID.Equals("G*M*NZ----****X") ||//ground zero
                    basicID.Equals("G*M*NEB---****X") ||//biological
                    basicID.Equals("G*M*NEC---****X")))//chemical)
            {
                strText = modifiers[ModifiersTG.Q_DIRECTION_OF_MOVEMENT];
                if (strText != null && SymbolUtilities.isNumber(strText))
                {
                    float q = (float)Convert.ToDouble(strText);
                    Rectangle tempBounds = ShapeUtilities.clone(bounds);
                    tempBounds = ShapeUtilities.union(tempBounds,new Point(ShapeUtilities.getCenterX(bounds), yForY));

                    domPoints = createDOMArrowPoints(symbolID, tempBounds, ii.getCenterPoint(), q, false);

                    domBounds = new Rectangle(domPoints[0].X, domPoints[0].Y, 1, 1);

                    Point temp = Point.Empty;
                    for (int i = 1; i < 6; i++)
                    {
                        temp = domPoints[i];
                        if (temp != null)
                        {
                            domBounds = ShapeUtilities.union(domBounds, temp);
                        }
                    }
                    imageBounds = ShapeUtilities.union(imageBounds,domBounds);
                }
            }
            // </editor-fold>

            // <editor-fold defaultstate="collapsed" desc="Shift Points and Draw">
            Rectangle modifierBounds = Rectangle.Empty;
            if (arrMods != null && arrMods.Count > 0)
            {

                //build modifier bounds/////////////////////////////////////////
                modifierBounds = arrMods[0].getTextOutlineBounds();
                int size = arrMods.Count;
                TextInfo tempShape = null;
                for (int i = 1; i < size; i++)
                {
                    tempShape = arrMods[i];
                    modifierBounds = ShapeUtilities.union(modifierBounds, tempShape.getTextBounds());
                }

            }

            if (modifierBounds != Rectangle.Empty || domBounds != Rectangle.Empty)
            {

                if (modifierBounds != Rectangle.Empty)
                {
                    imageBounds = ShapeUtilities.union(imageBounds, modifierBounds);
                }
                if (domBounds != Rectangle.Empty)
                {
                    imageBounds = ShapeUtilities.union(imageBounds, domBounds);
                }

                //shift points if needed////////////////////////////////////////
                if (imageBounds.Left < 0 || imageBounds.Top < 0)
                {
                    int shiftX = Math.Abs(imageBounds.Left);
                    int shiftY = Math.Abs(imageBounds.Top);

                    //shift mobility points
                    int size = arrMods.Count;
                    TextInfo tempShape = null;
                    for (int i = 0; i < size; i++)
                    {
                        tempShape = arrMods[i];
                        tempShape.shift(shiftX, shiftY);
                    }
                    modifierBounds.Offset(shiftX, shiftY);

                    if (domBounds != Rectangle.Empty)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            Point temp = domPoints[i];
                            if (temp != null)
                            {
                                temp.Offset(shiftX, shiftY);
                                domPoints[i] = temp;
                            }
                        }
                        domBounds.Offset(shiftX, shiftY);
                    }

                    //shift image points
                    centerPoint.Offset(shiftX, shiftY);
                    symbolBounds.Offset(shiftX, shiftY);
                    imageBounds.Offset(shiftX, shiftY);
                }

                //Render modifiers//////////////////////////////////////////////////
                Bitmap bmp = new Bitmap(imageBounds.Width, imageBounds.Height);
                Graphics g = Graphics.FromImage(bmp);

                //render////////////////////////////////////////////////////////
                //draw original icon with potential modifiers.
                g.DrawImage(ii.getBitmap(), symbolBounds.Left, symbolBounds.Top);
                //ctx.drawImage(ii.getBitmap(),imageBoundsOld.Left,imageBoundsOld.Top);

                if (attributes.ContainsKey(MilStdAttributes.TextColor))
                {
                    textColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextColor]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextBackgroundColor))
                {
                    textBackgroundColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextBackgroundColor]);
                }

                renderText(g, arrMods, textColor, textBackgroundColor);

                newii = new ImageInfo(bmp, centerPoint, symbolBounds);

                //draw DOM arrow
                if (domBounds != Rectangle.Empty)
                {
                    drawDOMArrow(g, domPoints);
                }

                newii = new ImageInfo(bmp, centerPoint, symbolBounds);

                // <editor-fold defaultstate="collapsed" desc="Cleanup">
                g = null;
                // </editor-fold>

                return newii;

            }
            else
            {
                return null;
            }
            // </editor-fold>

        }


        public static ImageInfo ProcessTGSPModifiers(ImageInfo ii, String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes, Color lineColor)
        {

            // <editor-fold defaultstate="collapsed" desc="Variables">
            int bufferXL = 6;
            int bufferXR = 4;
            int bufferY = 2;
            int bufferText = 2;
            int centerOffset = 1; //getCenterX/Y function seems to go over by a pixel
            int x = 0;
            int y = 0;
            int x2 = 0;
            int y2 = 0;
            int symStd = RS.getSymbologyStandard();
            int outlineOffset = RS.getTextOutlineWidth();
            int labelHeight = 0;
            int labelWidth = 0;
            ImageInfo newii = null;

            Color textColor = lineColor;
            Color textBackgroundColor = Color.Empty;

            List<TextInfo> arrMods = new List<TextInfo>();

            if (attributes.ContainsKey(MilStdAttributes.SymbologyStandard))
            {
                symStd = Convert.ToInt32(attributes[MilStdAttributes.SymbologyStandard]);
            }

            Rectangle bounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Rectangle symbolBounds = ShapeUtilities.clone(ii.getSymbolBounds());
            Point centerPoint = ShapeUtilities.clone(ii.getCenterPoint());
            Rectangle imageBounds = ShapeUtilities.cloneToRectangle(ii.getImageBounds());

            centerPoint = new Point((ii.getCenterPoint().X),(ii.getCenterPoint().Y));

            Boolean byLabelHeight = false;

            labelHeight = ShapeUtilities.round(_modifierFontHeight + 0.5f);
            int maxHeight = (symbolBounds.Height);
            if ((labelHeight * 3) > maxHeight)
            {
                byLabelHeight = true;
            }

            int descent = 0;// (int)(_modifierFontDescent + 0.5f);

            Rectangle labelBounds1 = Rectangle.Empty;//text.getPixelBounds(null, 0, 0);
            Rectangle labelBounds2 = Rectangle.Empty;
            String strText = "";


            String basicID = SymbolUtilities.getBasicSymbolID(symbolID);

            if (outlineOffset > 2)
            {
                outlineOffset = ((outlineOffset - 1) / 2);
            }
            else
            {
                outlineOffset = 0;
            }

            /*bufferXL += outlineOffset;
             bufferXR += outlineOffset;
             bufferY += outlineOffset;
             bufferText += outlineOffset;*/
            // </editor-fold>
            // <editor-fold defaultstate="collapsed" desc="Process Modifiers">
            TextInfo ti = null;

            {
                if (modifiers.ContainsKey(ModifiersTG.N_HOSTILE))
                {
                    strText = modifiers[ModifiersTG.N_HOSTILE];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);

                        x = bounds.Left + bounds.Width + bufferXR;

                        if (!byLabelHeight)
                        {
                            y = ((bounds.Height / 3) * 2);//checkpoint, get box above the point
                            y = bounds.Top + y;
                        }
                        else
                        {
                            //y = ((labelHeight + bufferText) * 3);
                            //y = bounds.y + y - descent;
                            y = bounds.Top + bounds.Height;
                        }

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }

                }
                if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                {
                    strText = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        x = bounds.Left + (int)(bounds.Width * 0.5f);
                        x = x - (int)(labelWidth * 0.5f);
                        y = bounds.Top - descent;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.H1_ADDITIONAL_INFO_2))
                {
                    strText = modifiers[ModifiersTG.H1_ADDITIONAL_INFO_2];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        y = bounds.Top + labelHeight + (int)(bounds.Height * 0.2);

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.W_DTG_1))
                {
                    strText = modifiers[ModifiersTG.W_DTG_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        x = bounds.Left - labelWidth - bufferXL;
                        y = bounds.Top + labelHeight - descent;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.W1_DTG_2))
                {
                    strText = modifiers[ModifiersTG.W1_DTG_2];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        x = bounds.Left - labelWidth - bufferXL;

                        y = ((labelHeight - descent + bufferText) * 2);
                        y = bounds.Top + y;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if (modifiers.ContainsKey(ModifiersTG.T_UNIQUE_DESIGNATION_1))
                {
                    strText = modifiers[ModifiersTG.T_UNIQUE_DESIGNATION_1];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);

                        x = bounds.Left + bounds.Width + bufferXR;
                        y = bounds.Top + labelHeight - descent;
                        
                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }
                if ((modifiers.ContainsKey(ModifiersTG.T1_UNIQUE_DESIGNATION_2)) &&//T1
                        (basicID.Equals("G*O*ES----****X") || //emergency distress call
                        basicID.Equals("G*S*PP----****X") || //medevac pick-up point
                        basicID.Equals("G*S*PX----****X")))//ambulance exchange point
                {
                    strText = modifiers[ModifiersTG.T1_UNIQUE_DESIGNATION_2];
                    if (strText != null)
                    {
                        ti = new TextInfo(strText, 0, 0, _modifierFont, _g);
                        labelWidth = ShapeUtilities.round(ti.getTextBounds().Width);

                        //points
                        x = bounds.Left + (int)(bounds.Width * 0.5);
                        x = x - (int)(labelWidth * 0.5);
                        //y = bounds.y + (bounds.height * 0.5);

                        y = (int)((bounds.Height * 0.60));//633333333
                        y = bounds.Top + y;

                        y = y - labelHeight;
                        ti.setLocation(x, y);
                        arrMods.Add(ti);
                    }
                }

            }

            // </editor-fold>
            // <editor-fold defaultstate="collapsed" desc="Shift Points and Draw">
            Rectangle modifierBounds = Rectangle.Empty;
            if (arrMods != null && arrMods.Count > 0)
            {

                //build modifier bounds/////////////////////////////////////////
                modifierBounds = arrMods[0].getTextOutlineBounds();
                int size = arrMods.Count;
                TextInfo tempShape = null;
                for (int i = 1; i < size; i++)
                {
                    tempShape = arrMods[i];
                    modifierBounds = Rectangle.Union(modifierBounds, tempShape.getTextBounds());
                }

            }

            if (modifierBounds != null)
            {
                imageBounds = Rectangle.Union(imageBounds, modifierBounds);

                //shift points if needed////////////////////////////////////////
                if (imageBounds.Left < 0 || imageBounds.Top < 0)
                {
                    int shiftX = Math.Abs(imageBounds.Left);
                    int shiftY = Math.Abs(imageBounds.Top);

                    //shift mobility points
                    int size = arrMods.Count;
                    TextInfo tempShape = null;
                    for (int i = 0; i < size; i++)
                    {
                        tempShape = arrMods[i];
                        tempShape.shift(shiftX, shiftY);
                    }
                    modifierBounds.Offset(shiftX, shiftY);

                    //shift image points
                    centerPoint.Offset(shiftX, shiftY);
                    symbolBounds.Offset(shiftX, shiftY);
                    imageBounds.Offset(shiftX, shiftY);
                }

                //Render modifiers//////////////////////////////////////////////////
                Bitmap bmp = new Bitmap(imageBounds.Width, imageBounds.Height);
                Graphics g = Graphics.FromImage(bmp);

                //draw original icon with potential modifiers.
                g.DrawImage(ii.getBitmap(), symbolBounds.Left, symbolBounds.Top);
                //ctx.drawImage(ii.getBitmap(),imageBoundsOld.Left,imageBoundsOld.Top);

                if (attributes.ContainsKey(MilStdAttributes.TextColor))
                {
                    textColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextColor]);
                }
                if (attributes.ContainsKey(MilStdAttributes.TextBackgroundColor))
                {
                    textBackgroundColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.TextBackgroundColor]);
                }
                renderText(g, arrMods, textColor, textBackgroundColor);

                newii = new ImageInfo(bmp, centerPoint, symbolBounds);

                // <editor-fold defaultstate="collapsed" desc="Cleanup">
                g = null;

                // </editor-fold>
            }
            // </editor-fold>
            return newii;

        }


        private static void renderText(Graphics g, List<TextInfo> tiArray, Color color, Color backgroundColor)
        {
            ModifierRenderer.renderText(g, tiArray.ToArray(), color, backgroundColor);
        }

        /**
         * 
         * @param ctx
         * @param tiArray
         * @param color
         * @param backgroundColor 
         */
        public static void renderText(Graphics g, TextInfo[] tiArray, Color color, Color backgroundColor)
        {
            /*for (TextInfo textInfo : tiArray) 
             {
             ctx.drawText(textInfo.getText(), textInfo.getLocation().x, textInfo.getLocation().y, _modifierFont);	
             }*/
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            int size = tiArray.Length;

            int tbm = RS.getTextBackgroundMethod();
            int outlineWidth = RS.getTextOutlineWidth();
            Font font = RS.getLabelFont();
            SolidBrush brush = null;
            SolidBrush outlineBrush = null;

            if (color != Color.Empty)
            {
                brush = new SolidBrush(color);
                outlineBrush = new SolidBrush(Color.White);
            }
            else
            {
                brush = new SolidBrush(Color.Black);
                outlineBrush = new SolidBrush(Color.White);
            }
            
            Color outlineColor = Color.Empty;

            if (backgroundColor != Color.Empty)
            {
                outlineColor = backgroundColor;
                outlineBrush.Color = backgroundColor;
            }
            else
            {
                outlineColor = RendererUtilities.getIdealOutlineColor(color);
                outlineBrush.Color = outlineColor;
            }
            TextFormatFlags tff = TextFormatFlags.Top;
            if (tbm == RendererSettings.TextBackgroundMethod_OUTLINE_QUICK)
            {
                for (int i = 0; i < size; i++)//better looking
                {
                    TextInfo textInfo = tiArray[i];
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X - 1, textInfo.getLocation().Y);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X + 1, textInfo.getLocation().Y);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X, textInfo.getLocation().Y + 1);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X, textInfo.getLocation().Y - 1);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X - 1, textInfo.getLocation().Y - 1);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X + 1, textInfo.getLocation().Y - 1);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X - 1, textInfo.getLocation().Y + 1);
                    g.DrawString(textInfo.getText(), font, outlineBrush, textInfo.getLocation().X + 1, textInfo.getLocation().Y + 1);

                    g.DrawString(textInfo.getText(), font, brush, textInfo.getLocation().X, textInfo.getLocation().Y);
                }//*/
                /*for (int j = 0; j < size; j++)//faster
                {
                    TextInfo textInfo = tiArray[j];
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X - 1, textInfo.getLocation().Y), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X + 1, textInfo.getLocation().Y), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X, textInfo.getLocation().Y + 1), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X, textInfo.getLocation().Y - 1), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X - 1, textInfo.getLocation().Y - 1), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X + 1, textInfo.getLocation().Y - 1), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X - 1, textInfo.getLocation().Y + 1), outlineColor, tff);
                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X + 1, textInfo.getLocation().Y + 1), outlineColor, tff);

                    TextRenderer.DrawText(g, textInfo.getText(), font, new Point(textInfo.getLocation().X, textInfo.getLocation().Y), color, tff);
                }//*/
                
                //draw text
                
            }
            else if (tbm == RendererSettings.TextBackgroundMethod_OUTLINE)
            {

                //draw text outline
                /*_modifierFont.setStyle(Style.STROKE);
                _modifierFont.setStrokeWidth(RS.getTextOutlineWidth());
                _modifierFont.setColor(outlineColor.toInt());
                if (outlineWidth > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        TextInfo textInfo = tiArray[i];
                        ctx.drawText(textInfo.getText(), textInfo.getLocation().x, textInfo.getLocation().y, _modifierFont);
                    }
                }
                //draw text
                _modifierFont.setColor(color.toInt());
                _modifierFont.setStyle(Style.FILL);
                for (int j = 0; j < size; j++)
                {
                    TextInfo textInfo = tiArray[j];
                    ctx.drawText(textInfo.getText(), textInfo.getLocation().x, textInfo.getLocation().y, _modifierFont);
                }//*/

                for (int j = 0; j < size; j++)
                {
                    TextInfo textInfo = tiArray[j];
                    TextRenderer.DrawText(g, textInfo.getText(), font, textInfo.getLocation(), color, tff);
                    //g.DrawString(textInfo.getText(), font, Brushes.Black, textInfo.getLocation().X, textInfo.getLocation().Y);
                }
                
            }
            else if (tbm == RendererSettings.TextBackgroundMethod_COLORFILL)
            {
                SolidBrush fillColor = new SolidBrush(RendererUtilities.getIdealOutlineColor(color));

                //draw rectangle
                for (int k = 0; k < size; k++)
                {
                    TextInfo textInfo = tiArray[k];
                    Rectangle fillRect = textInfo.getTextOutlineBounds();
                    //fillRect.Width = (int)(fillRect.Width * 0.7f);
                    fillRect.Height = (int)(fillRect.Height * 0.8f);
                    g.FillRectangle(fillColor, fillRect);
                }
                //draw text
                for (int j = 0; j < size; j++)
                {
                    TextInfo textInfo = tiArray[j];
                    //TextRenderer.DrawText(g, textInfo.getText(), font, textInfo.getLocation(), color, tff);
                    g.DrawString(textInfo.getText(), font, brush, textInfo.getLocation().X, textInfo.getLocation().Y);
                }
            }
            else if (tbm == RendererSettings.TextBackgroundMethod_NONE)
            {
                for (int j = 0; j < size; j++)
                {
                    tff = TextFormatFlags.Top;
                    TextInfo textInfo = tiArray[j];
                    TextRenderer.DrawText(g, textInfo.getText(), font, textInfo.getLocation(), color, tff);
                    //g.DrawString(textInfo.getText(), font, brush, textInfo.getLocation().X, textInfo.getLocation().Y);
                }
            }
        }

        public static Boolean hasDisplayModifiers(String symbolID, Dictionary<int,String> modifiers)
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
    }
}
