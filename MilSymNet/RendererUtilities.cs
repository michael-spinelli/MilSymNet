using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MilSymNetUtilities;

namespace MilSymNet
{
    public class RendererUtilities
    {
        public static Color getIdealOutlineColor(Color fgColor)
        {
            try
            {
                //an array of three elements containing the
                //hue, saturation, and brightness (in that order),
                //of the color with the indicated red, green, and blue components/
                float[] hsbvals = new float[3];

                if(fgColor != Color.Empty)
                {
                    int nThreshold = RendererSettings.getInstance().getTextBackgroundAutoColorThreshold();//160;
                    int bgDelta = (int)((fgColor.R * 0.299) + (fgColor.G * 0.587) + (fgColor.B * 0.114));
                    //ErrorLogger.LogMessage("bgDelta: " + String.valueOf(255-bgDelta));
                    //if less than threshold, black, otherwise white.
                    return (255 - bgDelta < nThreshold) ? Color.Black : Color.White;
                }
            }
            catch(Exception exc)
            {
                ErrorLogger.LogException("SymbolDraw", "getIdealtextBGColor", exc);
            }
            return Color.White;
        }
    }
}
