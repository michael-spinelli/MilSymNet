using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using MilSymNetUtilities;

namespace MilSymUwp
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

            CanvasRenderTarget coreBMP = null;

            SVGPath svgTG = null;
            //SVGPath svgFrame = null;

            if (mapping > 0)
                svgTG = TGSVGTable.getInstance().getSVGPath(mapping);

            //float scale = 1;
            Matrix3x2 mScale, mTranslate;
            Matrix3x2 mIdentity = Matrix3x2.Identity;
            Rect rectF = new Rect();
            Matrix3x2 m = svgTG.CreateMatrix(size, size, out rectF, out mScale, out mTranslate);
            //svgTG.TransformToFitDimensions(size, size);
            Rect rr = svgTG.computeBounds(m);
            
            CanvasDevice device = CanvasDevice.GetSharedDevice();
            //CanvasRenderTarget offscreen = new CanvasRenderTarget(device, width, height, 96);
            coreBMP = new CanvasRenderTarget(device, (int)(rr.Width + 0.5), (int)(rr.Height + 0.5),96);

            using (CanvasDrawingSession cds = coreBMP.CreateDrawingSession())
            {
                svgTG.Draw(cds, Colors.Transparent, 0, color, m);
                cds.DrawRectangle(coreBMP.GetBounds(device), Colors.Red);
            }
            returnVal = new ImageInfo(coreBMP,new Point(coreBMP.Size.Width/2f,coreBMP.Size.Height/2.0f),new Rect(0,0,coreBMP.Size.Width,coreBMP.Size.Height),coreBMP.GetBounds(device));

            return returnVal;
        }
    }
}
