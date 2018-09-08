using System;
using System.Collections.Generic;


namespace MilSymNetUtilities
{
    public class MilStdAttributes
    {

        /*
         * Line color of the symbol. hex value.
         */
        public static int LineColor = 0;
    
        /*
         * Fill color of the symbol. hex value
         */
        public static int FillColor = 1;

        /*
         * Color of internal icon. hex value
         */
        public static int IconColor = 17;
    
        /*
         * font size to use when rendering symbol
         */
        public static int FontSize = 2;
    
        /*
         * size of the single point image
         */
        public static int PixelSize = 3;
    
        /*
         * scale value to grow or shrink single point tactical graphics.
         */
        public static int Scale = 4;
    
        /**
         * defaults to true
         */
        public static int KeepUnitRatio = 5;
    
        /*
         * transparency value of the symbol. values from 0-255
         */
        public static int Alpha = 6;
    
        /*
         * outline the symbol, true/false
         */
        public static int OutlineSymbol = 7;
    
        /*
         * specify and outline color rather than letting renderer picking 
         * the best contrast color. hex value
         */
        public static int OutlineColor = 8;

        public static int SymbolOutlineColor = 9;

        public static int SymbolOutlineSize = 17;

        /*
         * just draws the core symbol
         */
        public static int DrawAsIcon = 10;
    
        /*
         * 2525B vs 2525C. 
         * like:
         * RendererSettings.Symbology_2525Bch2_USAS_13_14
         * OR
         * RendererSettings.Symbology_2525C
         */
        public static int SymbologyStandard = 11;
    
        public static int LineWidth = 12;
   
        public static int TextColor = 13;
    
        public static int TextBackgroundColor = 14;    
    
        /**
         * If false, the renderer will create a bunch of little lines to create
         * the "dash" effect (expensive but necessary for KML).  
         * If true, it will be on the user to create the dash effect using the
         * DashArray from the Stroke object from the ShapeInfo object.
         */
        public static int UseDashArray = 15;
    
        public static int AltitudeMode = 16;

        public static List<int> GetModifierList()
        {
            List<int> list = new List<int>();

            list.Add(LineColor);
            list.Add(FillColor);
            list.Add(IconColor);
            list.Add(FontSize);
            list.Add(PixelSize);
            list.Add(Scale);
            list.Add(KeepUnitRatio);
            list.Add(Alpha);
            list.Add(OutlineSymbol);
            list.Add(OutlineColor);
            //list.add(OutlineWidth);
            list.Add(DrawAsIcon);
            list.Add(SymbologyStandard);

            return list;
        }
    }
}
