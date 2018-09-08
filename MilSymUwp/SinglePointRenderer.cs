using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.Foundation;
using MilSymNetUtilities;

namespace MilSymUwp
{
    public class SinglePointRenderer: IDisposable
    {
        
        private static object syncLock = new object();
        private static SinglePointRenderer _instance = null;

        private CanvasRenderTarget coreUnitBuffer = null;
        private static float coreBufferSize = 100;

        // Track whether Dispose has been called.
        private static bool disposed = false;

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

        public CanvasRenderTarget DrawTest()
        {/*
            CanvasRenderTarget myBmp = null;
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
            }//*/
            return null;
        }

        public ImageInfo RenderSymbol(String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes)
        {
            ImageInfo returnVal = null;
            int symStd = 1;

            
            if (SymbolUtilities.isTacticalGraphic(symbolID))
            {
                if (modifiers != null && modifiers[MilStdAttributes.SymbologyStandard] != null)
                {
                    symStd = Convert.ToInt32(modifiers[MilStdAttributes.SymbologyStandard]);
                }
                else
                {
                    if (modifiers == null)
                        modifiers = new Dictionary<int, String>();
                    modifiers[MilStdAttributes.SymbologyStandard] = Convert.ToString(RendererSettings.getInstance().getSymbologyStandard());
                }
                SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(SymbolUtilities.getBasicSymbolIDStrict(symbolID), symStd);
                if (sd.getMaxPoints() == 1 && sd.HasWidth() == false)
                    returnVal = RenderSPTG(symbolID, modifiers, attributes);
                else
                    returnVal = RenderMPTG(symbolID, modifiers, attributes);
            }
            else
            {
                returnVal =  RenderUnit(symbolID, modifiers, attributes, null);
            }
            return returnVal;
        }

        public ImageInfo RenderMPTG(String symbolID, Dictionary<int, String> modifiers, Dictionary<int, String> attributes)
        {
            ImageInfo returnVal = null;

            Color lineColor = Colors.Transparent;

            int pixelSize = 35;

            if (attributes.ContainsKey(MilStdAttributes.LineColor))
                lineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]));
            else
                lineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getLineColorOfAffiliation(symbolID));

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
            CanvasRenderTarget finalBmp = null;

            CanvasRenderTarget coreBMP = null;
            try
            {
                //get font character indexes (svg file for this renderer port)
                int fillIndex = -1;
                int frameIndex = -1;

                int alpha = 255;

                int symbolOutlineSize = 0;
                Color symbolOutlineColor = Colors.Transparent;
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
                Color lineColor = Colors.Transparent;
                Color fillColor = Colors.Transparent;

                if (attributes.ContainsKey(MilStdAttributes.LineColor))
                    lineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]));
                else
                    lineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getLineColorOfAffiliation(symbolID));

                if(attributes.ContainsKey(MilStdAttributes.FillColor))
                    fillColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.FillColor]));

                if (attributes.ContainsKey(MilStdAttributes.Alpha))
                    alpha = Convert.ToInt32(attributes[MilStdAttributes.Alpha]);

                if (attributes.ContainsKey(MilStdAttributes.SymbolOutlineColor))
                    symbolOutlineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.SymbolOutlineColor]));
                else
                    symbolOutlineColor = RenderUtilities.getIdealTextBackgroundColor(lineColor);

                if (attributes.ContainsKey(MilStdAttributes.SymbolOutlineSize))
                    symbolOutlineSize = Convert.ToInt32(attributes[MilStdAttributes.SymbolOutlineSize]);
                else
                    symbolOutlineSize = RendererSettings.getInstance().getSymbolOutlineWidth();
                
                if (symbolOutlineSize <= 0)
                    symbolOutlineColor = Colors.Transparent;

                if (attributes.ContainsKey(MilStdAttributes.DrawAsIcon))
                    drawAsIcon = Convert.ToBoolean(attributes[MilStdAttributes.DrawAsIcon]);

                if (attributes.ContainsKey(MilStdAttributes.PixelSize))
                    pixelSize = Convert.ToInt32(attributes[MilStdAttributes.PixelSize]);

                if (attributes.ContainsKey(MilStdAttributes.KeepUnitRatio))
                    keepUnitRatio = Convert.ToBoolean(attributes[MilStdAttributes.KeepUnitRatio]);

                SinglePointLookupInfo spli = SinglePointLookup.getInstance().getSPLookupInfo(SymbolUtilities.getBasicSymbolIDStrict(symbolID));

                if (spli == null)//default to action point on bad symbolID
                {
                    if (modifiers == null)
                        modifiers = new Dictionary<int, string>();
                    if (modifiers.ContainsKey(ModifiersTG.H_ADDITIONAL_INFO_1))
                        modifiers[ModifiersTG.H1_ADDITIONAL_INFO_2] = modifiers[ModifiersTG.H_ADDITIONAL_INFO_1];
                    modifiers[ModifiersTG.H_ADDITIONAL_INFO_1] = symbolID.Substring(0, 10);

                    symbolID = "G" + SymbolUtilities.getAffiliation(symbolID) +
                        "G" + SymbolUtilities.getStatus(symbolID) + "GPP---****X";
                    spli = SinglePointLookup.getInstance().getSPLookupInfo(symbolID);
                    lineColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getLineColorOfAffiliation(symbolID));
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

                if (SymbolUtilities.isTGSPWithFill(symbolID))
                {
                    String fillID = SymbolUtilities.getTGFillSymbolCode(symbolID);
                    if (fillID != null)
                        fillIndex = SinglePointLookup.getInstance().getCharCodeFromFillID(fillID);
                }
                else if (SymbolUtilities.isWeatherSPWithFill(symbolID))
                {
                    fillIndex = frameIndex + 1;
                    fillColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getFillColorOfWeather(symbolID));

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
                Rect rr = svgFrame.getBounds();

                if (keepUnitRatio)
                {
                    //scale = pixelSize * .00095f;// 1f;
                    scale = pixelSize / 34f;//height of friendly unit before scaling.
                    if (rr.Height > rr.Width)
                        pixelSize = (int)((scale * rr.Height) + 0.5);
                    else
                        pixelSize = (int)((scale * rr.Width) + 0.5);
                }

                Matrix3x2 mScale, mTranslate;
                Matrix3x2 mIdentity = Matrix3x2.Identity;
                Rect rectF = new Rect();
                Matrix3x2  m = svgFrame.CreateMatrix(pixelSize, pixelSize, out rectF, out mScale, out mTranslate);
                //Matrix3x2 m = svgFrame.TransformToFitDimensions(pixelSize, pixelSize);

                rr = svgFrame.computeBounds(m);
                w = (int)((rr.Width) + 0.5f);
                h = (int)((rr.Height) + 0.5f);
                //draw location

                System.Drawing.RectangleF sdrr = new System.Drawing.RectangleF((float)rr.X,(float)rr.Y,(float)rr.Width,(float)rr.Height);
                System.Drawing.PointF sdpf = SymbolDimensions.getSymbolCenter(spli.getBasicSymbolID(), sdrr);
                Point centerPoint = new Point(sdpf.X,sdpf.Y); 
                Point location = new Point(0, 0);

                location.X = centerPoint.X;
                location.Y = 0;// centerPoint.Y;
                //location.Y = (h * 1.5f);

                float outlineOffsetX = 0;
                float outlineOffsetY = 0;
                Matrix3x2 mOutline = new Matrix3x2();

                CanvasDevice device = CanvasDevice.GetSharedDevice();
                //CanvasRenderTarget offscreen = new CanvasRenderTarget(device, width, height, 96);

                if (symbolOutlineSize > 0)
                {
                    Rect rectOutline = ShapeUtilities.inflate(rr, symbolOutlineSize, symbolOutlineSize);// svgFrame.getBounds(symbolOutlineSize);
                    outlineOffsetX = (float)((rectOutline.Width - w) / 2f );
                    outlineOffsetY = (float)((rectOutline.Height - h) / 2f );//too much added for AA
                    w = (int)(rectOutline.Width + 0.5f);
                    h = (int)(rectOutline.Height + 0.5f);
                    mOutline = Matrix3x2.CreateTranslation(outlineOffsetX, outlineOffsetY);
                    centerPoint.X += outlineOffsetX;
                    centerPoint.Y += outlineOffsetY;
                    coreBMP = new CanvasRenderTarget(device, w, h,96);// new Bitmap(w, h);//getBounds adds too much for AA
                }
                else
                {
                    coreBMP = new CanvasRenderTarget(device, w, h, 96);//add for AA
                }
                //coreBMP = new Bitmap(w, h);
                
                //draw test outline
                //g.DrawRectangle(new Pen(Color.LightSkyBlue), 0, 0, coreBMP.Width - 1, coreBMP.Height - 1);
                using (CanvasDrawingSession cds = coreBMP.CreateDrawingSession())
                {
                    if (svgFill != null)
                    {
                        svgFill.Transform(m);
                        svgFill.Draw(cds, Colors.Transparent, 0, fillColor, mOutline);
                    }

                    if (svgFrame != null)
                    {
                        svgFrame.Transform(m);
                        svgFrame.Draw(cds, symbolOutlineColor, symbolOutlineSize, lineColor, mOutline);
                    }
                        

                    cds.DrawRectangle(new Rect(0, 0, (int)(coreBMP.Size.Width + 0.5f), (int)(coreBMP.Size.Height + 0.5f)), Colors.Red);
                }


                Rect coreDimensions = new Rect(0, 0, (int)(coreBMP.Size.Width + 0.5f), (int)(coreBMP.Size.Height + 0.5f));
                
                ImageInfo ii = new ImageInfo(coreBMP, centerPoint, coreDimensions,coreBMP.GetBounds(device));

                //process display modifiers
                Boolean hasDisplayModifiers = ModifierRenderer.hasDisplayModifiers(symbolID, modifiers);
                Boolean hasTextModifiers = ModifierRenderer.hasTextModifiers(symbolID, modifiers, attributes);
                ImageInfo iinew = null;

                if (hasDisplayModifiers)
                {
                    //iinew = ModifierRenderer.ProcessUnitDisplayModifiers(symbolID, ii, modifiers, attributes, true, device);
                }//*/

                if (iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;

                //process text modifiers
                /*if (hasTextModifiers)
                {
                    //iinew = ModifierRenderer.ProcessUnitTextModifiers(symbolID, ii, modifiers, attributes);
                    iinew = ModifierRenderer.ProcessUnitTextModifiers(ii, symbolID, modifiers, attributes);
                }//*/

                if (iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;

                /*if (coreBMP != null)
                {
                    coreBMP.Dispose();
                    coreBMP = null;
                }//*/

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
        public ImageInfo RenderUnit(String symbolID, Dictionary<int,String> modifiers, Dictionary<int,String> attributes, CanvasDevice device)
        {
            //L 1.5 = 2650 pixel units in the svg font file
            double L1_5 = 2650;
            CanvasRenderTarget finalBMP = null;

            CanvasRenderTarget coreBMP = null;
            try
            {

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
                Color fillColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getFillColorOfAffiliation(symbolID));
                Color frameColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getLineColorOfAffiliation(symbolID));

                if (attributes == null)
                    attributes = new Dictionary<int, string>();
                if (attributes.ContainsKey(MilStdAttributes.LineColor))
                    frameColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.LineColor]));

                if (attributes.ContainsKey(MilStdAttributes.FillColor))
                    fillColor = RenderUtilities.DrawingColorToUIColor(SymbolUtilities.getColorFromHexString(attributes[MilStdAttributes.FillColor]));

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

                String basicID = SymbolUtilities.getBasicSymbolIDStrict(symbolID);
                UnitFontLookupInfo ufli = UnitFontLookup.getInstance().getLookupInfo(basicID);
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


                Matrix3x2 matrix = new Matrix3x2();
                double heightL = 1;
                double widthL = 1;

                Rect rectFrame = svgFrame.getBounds();
                if (keepUnitRatio)
                {
                    double ratio = pixelSize / L1_5 / 1.5;
                    widthL = UnitFontLookup.getUnitRatioWidth(fillIndex);
                    heightL = UnitFontLookup.getUnitRatioHeight(fillIndex);
                    if (widthL > heightL)
                        ratio = ratio * widthL;
                    else
                        ratio = ratio * heightL;
                    pixelSize = (int)((ratio * L1_5) + 0.5);

                }

                Matrix3x2 mScale, mTranslate;
                Matrix3x2 mIdentity = Matrix3x2.Identity;
                Rect rectF = new Rect();
                matrix = svgFrame.CreateMatrix(pixelSize, pixelSize, out rectF, out mScale, out mTranslate);


                //int w = (int)(rectF.Width + 1.5f);
                //int h = (int)(rectF.Height + 1.5f);
                int w = (int)(rectF.Width);
                int h = (int)(rectF.Height);
                if (w == pixelSize && h != pixelSize)
                    h = (int)Math.Ceiling(rectF.Height);
                else if (h == pixelSize && w != pixelSize)
                    w = (int)Math.Ceiling(rectF.Width);

                bool bufferUsed = false;
                if(w < coreBufferSize && h < coreBufferSize)
                {
                    if(coreUnitBuffer == null)
                    {
                        if (device == null)
                            device = CanvasDevice.GetSharedDevice();
                        coreUnitBuffer = new CanvasRenderTarget(device, coreBufferSize, coreBufferSize, 96);//new Bitmap(w, h);
                    }
                    coreBMP = coreUnitBuffer;
                    bufferUsed = true;
                }
                else
                {
                    if (device == null)
                        device = CanvasDevice.GetSharedDevice();
                    coreBMP = new CanvasRenderTarget(device, w, h, 96);//new Bitmap(w, h);
                }
                Point centerPoint = new Point(w / 2, h / 2);

                //get & setup graphics object for destination BMP
                
                using (CanvasDrawingSession ds = coreBMP.CreateDrawingSession())
                {
                    ds.Clear(Colors.Transparent);
                    //clear canvas
                    //ds.Transform = matrix;
                    // ds.Transform = mScale;
                    //draw symbol to BMP
                   // ds.Transform = mScale * mTranslate;
                    if (svgFill != null)
                    {
                        //svgFill.Transform(mScale);
                        //svgFill.Transform(mTranslate);
                        svgFill.Draw(ds, Colors.Transparent, 0, fillColor, matrix);
                    }
                    if (svgFrame != null)
                    {
                        svgFrame.Draw(ds, Colors.Transparent, 0, frameColor, matrix);
                    }
                    if (svgSymbol2 != null)
                    {
                        //svgSymbol2.Transform(mScale);
                        //svgSymbol2.Transform(mTranslate);
                        svgSymbol2.Draw(ds, Colors.Transparent, 0, RenderUtilities.DrawingColorToUIColor(ufli.getColor2()), matrix);
                    }
                    if (svgSymbol1 != null)
                    {
                        //svgSymbol1.Transform(mScale);
                        //svgSymbol1.Transform(mTranslate);
                        svgSymbol1.Draw(ds, Colors.Transparent, 0, RenderUtilities.DrawingColorToUIColor(ufli.getColor1()), matrix);
                    }
                    //ds.Transform = Matrix3x2.Identity;
                }//*/
                /*
                if (svgFill != null)
                {
                    svgFill.Transform(mScale);
                    svgFill.Transform(mTranslate);
                    //svgFill.Transform(matrix);
                    svgFill.Draw(coreBMP, Colors.Transparent, 0, fillColor, mIdentity);
                }
                if (svgFrame != null)
                {
                    svgFrame.Draw(coreBMP, Colors.Transparent, 0, frameColor, mIdentity);
                }
                if (svgSymbol2 != null)
                {
                    svgSymbol2.Transform(mScale);
                    svgSymbol2.Transform(mTranslate);
                    svgSymbol2.Draw(coreBMP, Colors.Transparent, 0, ufli.getColor2(), mIdentity);
                }
                if (svgSymbol1 != null)
                {
                    svgSymbol1.Transform(mScale);
                    svgSymbol1.Transform(mTranslate);
                    svgSymbol1.Draw(coreBMP, Colors.Transparent, 0, ufli.getColor1(), mIdentity);
                }//*/
                Rect coreDimensions = new Rect(0, 0, w, h);
                Rect finalDimensions = new Rect(0, 0, w, h);

                //adjust centerpoint for HQStaff if present
				if(SymbolUtilities.isHQ(symbolID))
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
                        point1.Y = (coreBMP.Size.Height);
						point2.X = point1.X;
                        point2.Y = point1.Y + coreBMP.Size.Height;
					}
					else
					{
						point1.X = 1;
                        point1.Y = (coreBMP.Size.Height / 2);
						point2.X = point1.X;
                        point2.Y = point1.Y + coreBMP.Size.Height;
					}
					centerPoint = point2;
				}


                if (device == null)
                    device = CanvasDevice.GetSharedDevice();
                finalBMP = new CanvasRenderTarget(device, (float)finalDimensions.Width, (float)finalDimensions.Height, 96);

                ImageInfo ii = new ImageInfo(coreBMP, new Point(centerPoint.X, centerPoint.Y), new Rect(0, 0, coreBMP.Size.Width, coreBMP.Size.Height),coreBMP.GetBounds(device));
                

                //process display modifiers
                ImageInfo iinew = null;

                Boolean hasDisplayModifiers = ModifierRenderer.hasDisplayModifiers(symbolID, modifiers);
                Boolean hasTextModifiers = ModifierRenderer.hasTextModifiers(symbolID, modifiers, attributes);
                if (hasDisplayModifiers)
                {
                    iinew = ModifierRenderer.ProcessUnitDisplayModifiers(symbolID, ii, modifiers, attributes, true, device);
                }//*/

                if (iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;

                //process text modifiers
                if (hasTextModifiers)
                {
                    //iinew = ModifierRenderer.ProcessUnitTextModifiers(ii, symbolID, modifiers, attributes);
                }//*/

                if (iinew != null)
                {
                    ii = iinew;
                }
                iinew = null;


                //////
                svgFill = null;
                svgFrame = null;
                svgSymbol1 = null;
                svgSymbol2 = null;
                device = null;
                //if(bufferUsed==false)
                //    coreBMP.Dispose();
                coreBMP = null;
                finalBMP = null;
                return ii;
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "RenderUnit", exc);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolID"></param>
        /// <param name="msb"></param>
        /// <param name="modifiers"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        private ImageInfo ProcessUnitDisplayModifiers(String symbolID, ImageInfo msb, Dictionary<String, String> modifiers, Dictionary<String, String> attributes)
        {
            try
            {
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SinglePointRenderer", "ProcessUnitDisplayModifiers", exc);
                return null;
            }
            return null;
        }


        ~SinglePointRenderer()
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
                if (disposing == true && coreUnitBuffer != null)
                {
                    coreUnitBuffer.Dispose();
                    coreUnitBuffer = null;
                }
                disposed = true;
            }
        }
    }
}
