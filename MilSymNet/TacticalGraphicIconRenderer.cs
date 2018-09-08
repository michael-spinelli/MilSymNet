using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MilSymNetUtilities;
using System.Drawing.Drawing2D;

namespace MilSymNet
{
    public class TacticalGraphicIconRenderer
    {
        private static TacticalGraphicLookup _tgl = null;
        public static ImageInfo getIcon(String symbolID, int size, Color color, int outlineSize)
        {
            ImageInfo returnVal = null;
            if (_tgl == null)
                _tgl = TacticalGraphicLookup.getInstance();

            int mapping = _tgl.getCharCodeFromSymbol(symbolID);

            Bitmap coreBMP = null;

            SVGPath svgTG = null;
            //SVGPath svgFrame = null;

            if (mapping > 0)
                svgTG = TGSVGTable.getInstance().getSVGPath(mapping);

            //float scale = 1;

            svgTG.TransformToFitDimensions(size, size);
            RectangleF rr = svgTG.getBounds();

            coreBMP = new Bitmap((int)(rr.Width + 0.5), (int)(rr.Height + 0.5));

            Graphics g = Graphics.FromImage(coreBMP);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            svgTG.Draw(g, Color.Empty, 0, color, null);

            returnVal = new ImageInfo(coreBMP,new PointF(coreBMP.Width/2f,coreBMP.Height/2.0f),new RectangleF(0,0,coreBMP.Width,coreBMP.Height));

            return returnVal;
        }
    }
}
