using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using MilSymNetUtilities;

namespace MilSymNet
{
    /**
     * Static class that holds the setting for the JavaRenderer.
     * Allows different parts of the renderer to know what
     * values are being used.
     */

    public class RendererSettings
    {


        private static RendererSettings _instance = null;

        //outline approach.  none, filled rectangle, outline (default),
        //outline quick (outline will not exceed 1 pixels).
        private static int _TextBackgroundMethod = 0;
        /**
         * There will be no background for text
         */
        public static readonly int TextBackgroundMethod_NONE = 0;

        /**
         * There will be a colored box behind the text
         */
        public static readonly int TextBackgroundMethod_COLORFILL = 1;

        /**
         * There will be an adjustable outline around the text (expensive)
         * Outline width of 4 is recommended.
         */
        public static readonly int TextBackgroundMethod_OUTLINE = 2;

        /**
         * A different approach for outline which is quicker and seems to use
         * less memory.  Also, you may do well with a lower outline thickess setting
         * compared to the regular outlining approach.  Outline Width of 2 is
         * recommended.  Only works with RenderMethod_NATIVE.
         */
        public static readonly int TextBackgroundMethod_OUTLINE_QUICK = 3;

        /**
         * Value from 0 to 255. The closer to 0 the lighter the text color has to be
         * to have the outline be black. Default value is 160.
         */
        private static int _TextBackgroundAutoColorThreshold = 160;

        //if TextBackgroundMethod_OUTLINE is set, This value determnies the width of that outline.
        private static int _TextOutlineWidth = 4;

        private static int _SymbolOutlineWidth = 2;

        //label foreground color, uses line color of symbol if null.
        private static Color _ColorLabelForeground = Color.Empty; //Color.BLACK; 
        //label background color, used if TextBackGroundMethod = TextBackgroundMethod_COLORFILL && not null
        private static Color _ColorLabelBackground = Color.White;

        
        /**
         * If true (default), when HQ Staff is present, location will be indicated by the free
         * end of the staff
         */
        private static Boolean _CenterOnHQStaff = true;

        /**
         * Everything that comes back from the Renderer is a Java Shape.  Simpler,
         * but can be slower when rendering modifiers or a large number of single
         * point symbols. Not recommended
         */
        public static readonly int RenderMethod_SHAPES = 0;
        /**
         * Adds a level of complexity to the rendering but is much faster for 
         * certain objects.  Modifiers and single point graphics will render faster.
         * MultiPoints will still be shapes.  Recommended
         */
        public static readonly int RenderMethod_NATIVE = 1;


        private static int _SymbologyStandard = MilStd.Symbology_2525C;



        //private static Font _ModifierFont = new Font("arial", Font.TRUETYPE_FONT, 12);
        private static String _ModifierFontName = "arial";
        //private static int _ModifierFontType = Font.TRUETYPE_FONT;
        private static System.Drawing.FontStyle _ModifierFontStyle = System.Drawing.FontStyle.Bold;  // private static int _ModifierFontType = Font.BOLD;
        private static float _ModifierFontSize = 12;
        private Boolean _scaleEchelon = false;
        private Boolean _DrawAffiliationModifierAsLabel = true;

        private static object syncLock = new object();

        private RendererSettings()
        {
            Init();

        }
        public static RendererSettings getInstance() //synchronized
        {

            if (_instance == null)
                lock (syncLock)
                {
                    _instance = new RendererSettings();
                }

            return _instance;
        }

        private void Init()
        {

        }

        /**
         * None, outline (default), or filled background.
         * If set to OUTLINE, TextOutlineWidth changed to default of 4.
         * If set to OUTLINE_QUICK, TextOutlineWidth changed to default of 2.
         * Use setTextOutlineWidth if you'd like a different value.
         * @param method like RenderSettings.TextBackgroundMethod_NONE
         */
        public void setTextBackgroundMethod(int textBackgroundMethod) //synchronized
        {
            _TextBackgroundMethod = textBackgroundMethod;
            if (_TextBackgroundMethod == TextBackgroundMethod_OUTLINE)
                _TextOutlineWidth = 4;
            else if (_TextBackgroundMethod == TextBackgroundMethod_OUTLINE_QUICK)
                _TextOutlineWidth = 2;

        }

        /**
         * None, outline (default), or filled background.
         * @return method like RenderSettings.TextBackgroundMethod_NONE
         */
        public int getTextBackgroundMethod() //synchronized
        {
            return _TextBackgroundMethod;
        }

        
        /**
         * Controls what symbols are supported.
         * Set this before loading the renderer.
         * @param symbologyStandard
         * Like MilStd.Symbology_2525Bch2_USAS_13_14
         */
        public void setSymbologyStandard(int standard)
        {
            _SymbologyStandard = standard;
        }

        /**
         * Current symbology standard
         * @return symbologyStandard
         * Like MilStd.Symbology_2525Bch2_USAS_13_14
         */
        public int getSymbologyStandard()
        {
            return _SymbologyStandard;
        }

       
        /**
         * if true (default), when HQ Staff is present, location will be indicated by the free
         * end of the staff
         * @param value
         */
        public void setCenterOnHQStaff(Boolean value)
        {
            _CenterOnHQStaff = value;
        }

        /**
         * if true (default), when HQ Staff is present, location will be indicated by the free
         * end of the staff
         * @param value
         */
        public Boolean getCenterOnHQStaff()
        {
            return _CenterOnHQStaff;
        }

        
        /**
         * if RenderSettings.TextBackgroundMethod_OUTLINE is used,
         * the outline will be this many pixels wide.
         * @param method
         */
        public void setTextOutlineWidth(int width) //synchronized
        {
            _TextOutlineWidth = width;
        }

        /**
         * if RenderSettings.TextBackgroundMethod_OUTLINE is used,
         * the outline will be this many pixels wide.
         * @param method
         * @return
         */
        public int getTextOutlineWidth() //synchronized
        {
            return _TextOutlineWidth;
        }

        /**
         * if greater than zero, single point Tactical Graphics
         * will be drawn with an outline
         * @param width
         */
        public void setSymbolOutlineWidth(int width) //synchronized
        {
            _SymbolOutlineWidth = width;
        }

        /**
         * if greater than zero, single point Tactical Graphics
         * will be drawn with an outline
         * @return
         */
        public int getSymbolOutlineWidth() //synchronized
        {
            return _SymbolOutlineWidth;
        }

        /**
        * Refers to text color of modifier labels
        * @return
         *  
        */
        public Color getLabelForegroundColor()
        {
            return _ColorLabelForeground;
        }

        /**
         * Refers to text color of modifier labels
         * Default Color is Black.  If NULL, uses line color of symbol
         * @param value
         * 
         */
        public void setLabelForegroundColor(Color value) //synchronized
        {
            _ColorLabelForeground = value;
        }

        /**
         * Refers to background color of modifier labels
         * @return
         * 
         */
        public Color getLabelBackgroundColor()
        {
            return _ColorLabelBackground;
        }

        /**
         * Refers to text color of modifier labels
         * Default Color is White.
         * Null value means the optimal background color (black or white)
         * will be chose based on the color of the text.
         * @param value
         * 
         */
        public void setLabelBackgroundColor(Color value) //synchronized
        {
            _ColorLabelBackground = value;
        }

        /**
         * Value from 0 to 255. The closer to 0 the lighter the text color has to be
         * to have the outline be black. Default value is 160.
         * @param value
         */
        public void setTextBackgroundAutoColorThreshold(int value)
        {
            _TextBackgroundAutoColorThreshold = value;
        }

        /**
         * Value from 0 to 255. The closer to 0 the lighter the text color has to be
         * to have the outline be black. Default value is 160.
         * @return
         */
        public int getTextBackgroundAutoColorThreshold()
        {
            return _TextBackgroundAutoColorThreshold;
        }

        /**
         * false to use label font size
         * true to scale it using symbolPixelBounds / 3.5
         * @param value 
         */
        public void setScaleEchelon(Boolean value)
        {
            _scaleEchelon = value;
        }
        /**
         * Returns the value determining if we scale the echelon font size or
         * just match the font size specified by the label font.
         * @return true or false
         */
        public Boolean getScaleEchelon()
        {
            return _scaleEchelon;
        }

        /**
        * Determines how to draw the Affiliation modifier.
        * True to draw as modifier label in the "E/F" location.
        * False to draw at the top right corner of the symbol
        */
        public void setDrawAffiliationModifierAsLabel(Boolean value)
        {
            _DrawAffiliationModifierAsLabel = value;
        }
        /**
         * True to draw as modifier label in the "E/F" location.
         * False to draw at the top right corner of the symbol
         */
        public Boolean getDrawAffiliationModifierAsLabel()
        {
            return _DrawAffiliationModifierAsLabel;
        }

        /**
         * get font object used for labels
         * @return Font object
         */
        public System.Drawing.Font getLabelFont()
        {
            try
            {

                return new System.Drawing.Font(_ModifierFontName, _ModifierFontSize, _ModifierFontStyle);
            }
            catch (Exception exc)
            {
                String message = "font creation error, returning \"" + _ModifierFontName + "\" font, " + _ModifierFontSize + "pt. Check font name and type.";
                ErrorLogger.LogMessage("RendererSettings", "getLabelFont", message);
                ErrorLogger.LogMessage("RendererSettings", "getLabelFont", exc.Message);
                return new System.Drawing.Font("arial", 12, System.Drawing.FontStyle.Bold);
            }
        }

        public void setModifierFont(String fontName, float fontSize, FontStyle fontStyle)
        {
            _ModifierFontName = fontName;
            _ModifierFontSize = fontSize;
            _ModifierFontStyle = fontStyle;
        }
        
          private static String _modifierFontName = "Arial";
          public String getModifierFontName()
          {
              return _modifierFontName;
          }


          private static FontStyle _modifierFontStyle = FontStyle.Bold;
          public FontStyle getModifierFontStyle()
          {
              return _modifierFontStyle;
          }
		
		
          private static int _modifierFontSize = 12;
          public int getModifierFontSize()
          {

              return _modifierFontSize;
          }
		
          private static String _modifierFontColor = "0x000000";
          public String getModifierFontColor()
          {
              return _modifierFontColor;
          }
          //public function setModifierFontColor(value:uint):void
          //{
          //	_modifierFontColor = value;
          //}
		
          private static int _modifierOutlineSize = 1;
          public int getModifierOutlineSize()
          {
              return _modifierOutlineSize;
          }
          public void setModifierOutlineSize(int value)
          {
              _modifierOutlineSize = value;
          }

    }
}
