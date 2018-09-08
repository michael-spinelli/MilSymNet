using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MilSymNetUtilities;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace MilSymNet
{
    public class SinglePointRenderer
    {

        private static object syncLock = new object();
        private static SinglePointRenderer _instance = null;
        private static Bitmap dimensionsBMP = new Bitmap(10, 10);
        private static Font _unitFont = null;
        private static Font _spFont = null;
        private static float _unitFontSize = 50;
        private static float _spFontSize = 60;

        private SinglePointRenderer()
        {
            Init();
        }

        private void Init()
        {
            //float dpi = 72;
            //float fontSize = 50;
            //float windowsFontSize = fontSize;
            //float unitFontSize = 40;
            //float spFontSize = 60;
            //float javaDPI = 96;//72;
            //float windowsDPI = 72;

            //adjust for windowsDPI vs Java
            /*float adjustedUnitFontSize = _unitFontSize;//(float)(_unitFontSize * (72.0f / 96.0f));//
            float adjustedSPFontSize = (float)(_spFontSize * (72.0f / 96.0f));//
            _unitFont = SECRendererUtils.ResourceLoader.getInstance().getFont(ResourceLoader.FontIndexUnits, adjustedUnitFontSize);
            _spFont = SECRendererUtils.ResourceLoader.getInstance().getFont(ResourceLoader.FontIndexSinglePoints, adjustedSPFontSize);//*/
        }



        public static SinglePointRenderer getInstance()
        {
            lock (syncLock)
            {
                if (_instance == null)
                    _instance = new SinglePointRenderer();
            }

            return _instance;
        }

        public Bitmap DrawTest()
        {
            Bitmap myBmp = null;
            try
            {
                float dpi = 72;
                float fontSize = 50;
                float windowsFontSize = fontSize;
                float javaDPI = 96;//72;
                float windowsDPI = 72;

                //get symbol dimensions
                Graphics g = Graphics.FromImage(dimensionsBMP);
               // g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //g.MeasureString("text", font);
                dpi = g.DpiX;

                //scaled the font for the C# renderer down by 0.75 so this calculation not necessary
                //had to scale it down also because bottom of symbols were getting clipped.
                //windowsFontSize = fontSize * windowsDPI / javaDPI;

                String spaceStation = "SUPPT-----*****";//space station
                String airTrack = "SUAPM-----*****";//air track, militray
                String AtoSMissle = "SFAPWMAP--*****";//air track, air to space missile
                String armor = "SUGPUCA---*****";//ground
                String mLauncher = "SFGPEWM---*****";//ground equipment
                String SSGN = "SUUPSNG---*****";//sub surface, guided missile (SSGN)

                String sar = "SHAPMFQH--*****";//search and rescue
                String sensor = "SHGPUUMRS-*****"; //sensor

                String symbolID = SSGN;


                int fillIndex = -1;
                int frameIndex = -1;
                int symbol1Index = -1;
                int symbol2Index = -1;
                char[] fillString = new char[1];
                char[] frameString = new char[1];
                char[] symbol1 = new char[1];
                char[] symbol2 = new char[1];

                UnitFontLookupInfo ufli = UnitFontLookup.getInstance().getLookupInfo(symbolID);

                fillIndex = UnitFontLookup.getFillCode(symbolID);
                frameIndex = UnitFontLookup.getFrameCode(symbolID, fillIndex);
                symbol1Index = ufli.getMapping1(symbolID);
                symbol2Index = ufli.getMapping2();

                fillString[0] = (char)fillIndex;
                frameString[0] = (char)frameIndex;
                symbol1[0] = (char)symbol1Index;
                symbol2[0] = (char)symbol2Index;

                Color fillColor = SymbolUtilities.getFillColorOfAffiliation(symbolID);
                Color frameColor = SymbolUtilities.getLineColorOfAffiliation(symbolID);

                PointF center = new PointF(150f, 150f);
                PointF location = new PointF(center.X, center.Y);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;//horizontal
                sf.LineAlignment = StringAlignment.Center;//vertical
                sf.FormatFlags = StringFormatFlags.NoClip;

                sf = StringFormat.GenericTypographic;

                RectangleF rectF = SymbolDimensions.getUnitBounds(fillIndex, fontSize);
                RectangleF rectLayout = new RectangleF(100f, 100f, 100f, 100f);

                //location.Y = location.Y + (rectF.Height / 2);// +rectF.Y;

                
                //create bitmap to draw to.
                //myBmp = new Bitmap(500,500);
                String temp = new String(frameString);

                
                int w = (int)Math.Round(rectF.Width);
                int h = (int)Math.Round(rectF.Height);
                myBmp = new Bitmap(w, h);
                location.X = 0;//rectF.Width / 2;
                location.Y = 0;
                
                location.X = rectF.Width / 2.0f;
                //location.Y = rectF.Height * 0.3f;
                location.Y = -(rectF.Height / 2.0f);
                RectangleF rectL = new RectangleF(0,0,rectF.Width,rectF.Height);

                g = Graphics.FromImage(myBmp);
                g.Transform = new Matrix();
                //g.TranslateTransform(rectF.Width / 2, rectF.Height * 0.25f);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //g.DrawString(new String(frameString), unitFont, new SolidBrush(frameColor), location, sf);
                


                location.Y += rectF.Y;



                //g.DrawString(Convert.ToString("fillString"), new Font("Arial",35f), new SolidBrush(frameColor), new PointF(100f, 100f));
                g.Transform = new Matrix();
                Pen RectanglePen = new Pen(Color.Red, 1.0f);
                RectanglePen.DashPattern = new float[] { 4.0f, 2.0f, 1.0f, 3.0f };
                g.DrawRectangle(RectanglePen, 0, 0, w-1, h-1);
                //g.DrawRectangle(Pens.Red, rectLayout.X,rectLayout.Y,rectLayout.Width,rectLayout.Height);
                //g.FillRectangle(Brushes.Red,center.X-1f,center.Y-1f, 2f, 2f);

                sf.LineAlignment = StringAlignment.Far;

                g.DrawString("T", new Font("arial", 24f), Brushes.Orange, center,sf);
                //g.DrawString("g", new Font("arial", 24f), Brushes.Purple, center, sf);
                //g.Dispose();
                //g = null;
                //g.FillRectangle(Brushes.Red, center.X - 1f, center.Y - 1f, 2f, 2f);

                
                return myBmp;
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "DrawTest", exc);
                return null;
            }
        }

        public ImageInfo RenderSymbol(String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes)
        {
            ImageInfo returnVal = null;
            int symStd = 0;
            
            if (SymbolUtilities.isTacticalGraphic(symbolID))
            {
                SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(SymbolUtilities.getBasicSymbolID(symbolID),symStd);
                if (sd.getMaxPoints() == 1 && sd.HasWidth() == false)
                    returnVal = RenderSPTG(symbolID, modifiers, attributes);
                else
                    returnVal = RenderMPTG(symbolID, modifiers, attributes);
            }
            else
            {
                returnVal =  RenderUnit(symbolID, modifiers, attributes);
            }
            return returnVal;
        }

        public ImageInfo RenderMPTG(String symbolID, Dictionary<int, String> modifiers, Dictionary<int, String> attributes)
        {
            ImageInfo returnVal = null;

            Color lineColor = Color.Empty;

            int pixelSize = 35;

            if (attributes.ContainsKey(MilStdAttributes.LineColor))
                lineColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]);
            else
                lineColor = SymbolUtilities.getLineColorOfAffiliation(symbolID);

            if (attributes.ContainsKey(MilStdAttributes.PixelSize))
                pixelSize = Convert.ToInt32(attributes[MilStdAttributes.PixelSize]);

            returnVal = TacticalGraphicIconRenderer.getIcon(symbolID, pixelSize, lineColor, 0);

            return returnVal;
        }


        public ImageInfo RenderSPTG(String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes)
        {
            if (modifiers == null)
                modifiers = new Dictionary<int, string>();
            if (attributes == null)
                attributes = new Dictionary<int, string>();
            Bitmap finalBmp = null;

            Bitmap coreBMP = null;
            try
            {
                //Graphics g = Graphics.FromImage(dimensionsBMP);

                //get unit font
                Font spFont = _spFont;

                //get font character indexes
                int fillIndex = -1;
                int frameIndex = -1;

                int alpha = 255;

                int symbolOutlineSize = 0;
                Color symbolOutlineColor = Color.Empty;
                Boolean drawAsIcon = false;
                Boolean keepUnitRatio = true;
                int pixelSize = 35;
                int w = pixelSize;
                int h = pixelSize;

                char[] fillString = new char[1];
                char[] frameString = new char[1];
                char[] symbol1 = new char[1];
                char[] symbol2 = new char[1];

                
                //_spFontSize;
                Color lineColor = Color.Empty;
                Color fillColor = Color.Empty;

                if (attributes.ContainsKey(MilStdAttributes.LineColor))
                    lineColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]);
                else
                    lineColor = SymbolUtilities.getLineColorOfAffiliation(symbolID);

                if(attributes.ContainsKey(MilStdAttributes.FillColor))
                    fillColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.FillColor]);

                if (attributes.ContainsKey(MilStdAttributes.Alpha))
                    alpha = Convert.ToInt32(attributes[MilStdAttributes.Alpha]);

                /*if (attributes.ContainsKey(MilStdAttributes.SymbolOutlineColor))
                    symbolOutlineColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.SymbolOutlineColor]);
                else
                    symbolOutlineColor = RenderUtilities.getIdealTextBackgroundColor(lineColor);

                if (attributes.ContainsKey(MilStdAttributes.SymbolOutlineSize))
                    symbolOutlineSize = Convert.ToInt32(attributes[MilStdAttributes.SymbolOutlineSize]);
                else 
                    symbolOutlineSize = RendererSettings.getInstance().getSymbolOutlineWidth();//*/

                symbolOutlineColor = RendererUtilities.getIdealOutlineColor(lineColor);
                symbolOutlineSize = RendererSettings.getInstance().getSymbolOutlineWidth();
                
                if (symbolOutlineSize <= 0)
                    symbolOutlineColor = Color.Empty;

                if (attributes.ContainsKey(MilStdAttributes.DrawAsIcon))
                    drawAsIcon = Convert.ToBoolean(attributes[MilStdAttributes.DrawAsIcon]);

                if (attributes.ContainsKey(MilStdAttributes.PixelSize))
                    pixelSize = Convert.ToInt32(attributes[MilStdAttributes.PixelSize]);

                if (attributes.ContainsKey(MilStdAttributes.KeepUnitRatio))
                    keepUnitRatio = Convert.ToBoolean(attributes[MilStdAttributes.KeepUnitRatio]);

                SinglePointLookupInfo spli = SinglePointLookup.getInstance().getSPLookupInfo(SymbolUtilities.getBasicSymbolID(symbolID));

                if (spli == null)//default to action point on bad symbolID
                {
                    if (modifiers == null)
                        modifiers = new Dictionary<int, String>();
                    if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                        modifiers[ModifiersTG.H1_ADDITIONAL_INFO_2] = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    modifiers[ModifiersTG.H_ADDITIONAL_INFO_1] = symbolID.Substring(0, 10);

                    symbolID = "G" + SymbolUtilities.getAffiliation(symbolID) +
                        "G" + SymbolUtilities.getStatus(symbolID) + "GPP---****X";
                    spli = SinglePointLookup.getInstance().getSPLookupInfo(symbolID);
                    lineColor = SymbolUtilities.getLineColorOfAffiliation(symbolID);
                    //fillColor = SymbolUtilities.getFillColorOfAffiliation(symbolID);
                }

                //Check if we need to set 'N' to ENY

                if (symbolID[1] == 'H' && drawAsIcon == false)
                {
                    if (modifiers == null)
                        modifiers = new Dictionary<int, string>();
                    modifiers[ModifiersTG.N_HOSTILE] = "ENY";
                }

                if (SymbolUtilities.getStatus(symbolID) == "A")
                    frameIndex = spli.getMappingA();
                else
                    frameIndex = spli.getMappingP();

                if (SymbolUtilities.hasDefaultFill(symbolID))
                {
                    fillColor = SymbolUtilities.getFillColorOfAffiliation(symbolID);
                }
                if (SymbolUtilities.isTGSPWithFill(symbolID))
                {
                    String fillID = SymbolUtilities.getTGFillSymbolCode(symbolID);
                    if (fillID != null)
                        fillIndex = SinglePointLookup.getInstance().getCharCodeFromFillID(fillID);
                }
                else if (SymbolUtilities.isWeatherSPWithFill(symbolID))
                {
                    fillIndex = frameIndex + 1;
                    fillColor = SymbolUtilities.getFillColorOfWeather(symbolID);

                }
                   

                fillString[0] = (char)fillIndex;
                frameString[0] = (char)frameIndex;


                SVGPath svgFill = null;
                SVGPath svgFrame = null;

                if(fillIndex > 0)
                    svgFill = SymbolSVGTable.getInstance().getSVGPath(fillIndex);
                if(frameIndex > 0)
                    svgFrame = SymbolSVGTable.getInstance().getSVGPath(frameIndex);
                
                float scale = 1;
                RectangleF rr = svgFrame.getBounds();

                if (keepUnitRatio)
                {
                    scale = pixelSize * .00095f;
                    if (rr.Height > rr.Width)
                        pixelSize = (int)((scale * rr.Height) + 0.5);
                    else
                        pixelSize = (int)((scale * rr.Width) + 0.5);
                }
                

                Matrix m = svgFrame.TransformToFitDimensions(pixelSize, pixelSize);
                //Matrix m = svgFrame.TransformToFitDimensions(w, h);
                rr = svgFrame.getBounds();
                w = (int)((rr.Width) + 0.5f);
                h = (int)((rr.Height) + 0.5f);
                //draw location
                PointF centerPoint = SymbolDimensions.getSymbolCenter(spli.getBasicSymbolID(), rr);
                PointF location = new PointF(0, 0);

                location.X = centerPoint.X;
                location.Y = 0;// centerPoint.Y;
                //location.Y = (h * 1.5f);

                float outlineOffsetX = 0;
                float outlineOffsetY = 0;
                Matrix mOutline = new Matrix();
                if (symbolOutlineSize > 0)
                {
                    RectangleF rectOutline = svgFrame.getBounds(symbolOutlineSize);
                    outlineOffsetX = (rectOutline.Width - w) / 2f - 1;
                    outlineOffsetY = (rectOutline.Height - h) / 2f - 1;//too much added for AA
                    w = (int)(rectOutline.Width + 0.5f);
                    h = (int)(rectOutline.Height + 0.5f);
                    mOutline.Translate(outlineOffsetX, outlineOffsetY);
                    centerPoint.X += outlineOffsetX;
                    centerPoint.Y += outlineOffsetY;
                    coreBMP = new Bitmap(w-1, h-1);//getBounds adds too much for AA
                }
                else
                {
                    coreBMP = new Bitmap(w + 1, h + 1);//add for AA
                }
                //coreBMP = new Bitmap(w, h);
                
                //get & setup graphics object for destination BMP
                Graphics g = Graphics.FromImage(coreBMP);

                
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                //draw test outline
                //g.DrawRectangle(new Pen(Color.LightSkyBlue), 0, 0, coreBMP.Width - 1, coreBMP.Height - 1);

                if (svgFill != null)
                {
                    svgFill.Transform(m);
                    svgFill.Draw(g, Color.Empty, 0, fillColor, mOutline);
                }
                
                if(svgFrame != null)
                    svgFrame.Draw(g, symbolOutlineColor, symbolOutlineSize, lineColor, mOutline);



                RectangleF coreDimensions = new RectangleF(0, 0, coreBMP.Width, coreBMP.Height);

                ImageInfo ii = new ImageInfo(coreBMP, new PointF(centerPoint.X, centerPoint.Y), coreDimensions);

                //process display modifiers
                ImageInfo iinew = null;

                Boolean hasDisplayModifiers = ModifierRenderer.hasDisplayModifiers(symbolID, modifiers);
                Boolean hasTextModifiers = ModifierRenderer.hasTextModifiers(symbolID, modifiers, attributes);
                ImageInfo iiNew = null;

                if (drawAsIcon == false && (hasTextModifiers || hasDisplayModifiers || SymbolUtilities.isTGSPWithIntegralText(symbolID)))
                {
                    if (SymbolUtilities.isTGSPWithSpecialModifierLayout(symbolID)
                            || SymbolUtilities.isTGSPWithIntegralText(symbolID))
                    {
                        iiNew = ModifierRenderer.ProcessTGSPWithSpecialModifierLayout(ii, symbolID, modifiers, attributes, lineColor);
                    }
                    else
                    {
                        iiNew = ModifierRenderer.ProcessTGSPModifiers(ii, symbolID, modifiers, attributes, lineColor);
                    }

                }

                if (iiNew != null)
                {
                    ii = iiNew;
                }

                iinew = null;

                g.Dispose();
                g = null;

                return ii;
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "RenderSPTG", exc);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolID"></param>
        /// <param name="modifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public ImageInfo RenderUnit(String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes)
        {
            //L 1.5 = 2650 pixel units in the svg font file
            double L1_5 = 2650;

            Bitmap coreBMP = null;
            try
            {
                Graphics g = Graphics.FromImage(dimensionsBMP);

                //get unit font
                Font unitFont = _unitFont;

                //get font character indexes
                int fillIndex = -1;
                int frameIndex = -1;
                int symbol1Index = -1;
                int symbol2Index = -1;
                SVGPath svgFill = null;
                SVGPath svgFrame = null;
                SVGPath svgSymbol1 = null;
                SVGPath svgSymbol2 = null;
                
                //get attributes
                int alpha = 255;
                Boolean drawAsIcon = false;
                Boolean keepUnitRatio = true;
                int pixelSize = 0;
                Color fillColor = SymbolUtilities.getFillColorOfAffiliation(symbolID);
                Color frameColor = SymbolUtilities.getLineColorOfAffiliation(symbolID);

                if (attributes == null)
                    attributes = new Dictionary<int, string>();
                if (attributes.ContainsKey(MilStdAttributes.LineColor))
                    frameColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]);

                if (attributes.ContainsKey(MilStdAttributes.FillColor))
                    fillColor = SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.FillColor]);

                if (attributes.ContainsKey(MilStdAttributes.Alpha))
                    alpha = Convert.ToInt32(attributes[MilStdAttributes.Alpha]);

                if (attributes.ContainsKey(MilStdAttributes.DrawAsIcon))
                    drawAsIcon = Convert.ToBoolean(attributes[MilStdAttributes.DrawAsIcon]);

                if (attributes.ContainsKey(MilStdAttributes.PixelSize))
                    pixelSize = Convert.ToInt32(attributes[MilStdAttributes.PixelSize]);
                else
                    pixelSize = 35;

                if (attributes.ContainsKey(MilStdAttributes.KeepUnitRatio))
                    keepUnitRatio = Convert.ToBoolean(attributes[MilStdAttributes.KeepUnitRatio]);

                UnitFontLookupInfo ufli = UnitFontLookup.getInstance().getLookupInfo(symbolID);
                fillIndex = UnitFontLookup.getFillCode(symbolID);
                frameIndex = UnitFontLookup.getFrameCode(symbolID, fillIndex);
                if (ufli != null)
                {
                    symbol1Index = ufli.getMapping1(symbolID);
                    symbol2Index = ufli.getMapping2();
                }

                if (fillIndex > 0)
                    svgFill = UnitSVGTable.getInstance().getSVGPath(fillIndex);
                if (frameIndex > 0)
                    svgFrame = UnitSVGTable.getInstance().getSVGPath(frameIndex);
                if (symbol1Index > 0)
                    svgSymbol1 = UnitSVGTable.getInstance().getSVGPath(symbol1Index);
                if (symbol2Index > 0)
                    svgSymbol2 = UnitSVGTable.getInstance().getSVGPath(symbol2Index);

                
                //get dimensions for this symbol given the font size & fill index
                

                Matrix matrix = null;
                double heightL = 1;
                double widthL = 1;
                
                if (keepUnitRatio)
                {
                    RectangleF rectFrame = svgFrame.getBounds();
                    double ratio = pixelSize / L1_5 / 1.5;
                    widthL = UnitFontLookup.getUnitRatioWidth(fillIndex);
                    heightL = UnitFontLookup.getUnitRatioHeight(fillIndex);
                    if (widthL > heightL)
                        ratio = ratio * widthL;
                    else
                        ratio = ratio * heightL;
                    pixelSize = (int)((ratio * L1_5) + 0.5);

                }

                matrix = svgFrame.TransformToFitDimensions(pixelSize, pixelSize);

                RectangleF rectF = svgFrame.getBounds();

                int w = (int)(rectF.Width + 1.5f);
                int h = (int)(rectF.Height + 1.5f);
                coreBMP = new Bitmap(w, h);
                Point centerPoint = new Point(w / 2, h / 2);

                //draw location
                PointF location = new PointF(0, 0);
                location.X = (rectF.Width / 2.0f);// +0.5f;//use 0.5f to round up
                location.Y = -(rectF.Height / 2.0f);

                //get & setup graphics object for destination BMP
                g = Graphics.FromImage(coreBMP);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                
                //draw symbol to BMP
                if (svgFill != null)
                    svgFill.Draw(g,Color.Empty,0,fillColor,matrix);
                if (svgFrame != null)
                    svgFrame.Draw(g, Color.Empty, 0, frameColor, null);
                if (svgSymbol2 != null)
                    svgSymbol2.Draw(g, Color.Empty, 0, ufli.getColor2(), matrix);
                if(svgSymbol1 != null)
                    svgSymbol1.Draw(g, Color.Empty, 0, ufli.getColor1(), matrix);
                

                RectangleF coreDimensions = new RectangleF(0, 0, w, h);
                Rectangle finalDimensions = new Rectangle(0, 0, w, h);

                //adjust centerpoint for HQStaff if present
				/*if(SymbolUtilities.isHQ(symbolID))
				{
					Point point1 = new Point();
					Point point2 = new Point();
					string affiliation = symbolID.Substring(1, 2);
					if(affiliation==("F") ||
						affiliation==("A") ||
						affiliation==("D") ||
						affiliation==("M") ||
						affiliation==("J") ||
						affiliation==("K") ||
						affiliation==("N") ||
						affiliation==("L"))
					{
						point1.X = 0;
                        point1.Y = (coreBMP.Height);
						point2.X = point1.X;
                        point2.Y = point1.Y + coreBMP.Height;
					}
					else
					{
						point1.X = 1;
                        point1.Y = (coreBMP.Height / 2);
						point2.X = point1.X;
                        point2.Y = point1.Y + coreBMP.Height;
					}
					centerPoint = point2;
				}//*/

                ImageInfo ii = new ImageInfo(coreBMP, centerPoint, coreDimensions);   

                //process display modifiers
                ImageInfo iinew = null;

                Boolean hasDisplayModifiers = ModifierRenderer.hasDisplayModifiers(symbolID, modifiers);
                Boolean hasTextModifiers = ModifierRenderer.hasTextModifiers(symbolID, modifiers, attributes);
                if(hasDisplayModifiers)
                { 
                    iinew = ModifierRenderer.ProcessUnitDisplayModifiers(symbolID, ii, modifiers, attributes,true);
                }   

                if(iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;

                //process text modifiers
                if(hasTextModifiers)
                {
                    //iinew = ModifierRenderer.ProcessUnitTextModifiers(symbolID, ii, modifiers, attributes);
                    iinew = ModifierRenderer.ProcessUnitTextModifiers(ii, symbolID, modifiers, attributes);
                }

                if (iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;

                g.Dispose();
                g = null;

                return ii;
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "RenderUnit", exc);
                return null;
            }
        }



    }
}
