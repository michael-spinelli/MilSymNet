using System;
using System.Collections.Generic;
using System.Collections;
using Windows.Foundation;
using System.Linq;
using System.Text;
using Windows.UI;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;


namespace MilSymUwp
{
    /**
     * Static class that holds the setting for the JavaRenderer.
     * Allows different parts of the renderer to know what
     * values are being used.
     */

    public class RendererSettings : IDisposable
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
        private static Color _ColorLabelForeground = Color.FromArgb(0, 0, 0, 0);
        //label background color, used if TextBackGroundMethod = TextBackgroundMethod_COLORFILL && not null
        private static Color _ColorLabelBackground = Colors.White;

        
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

        /**
         * 2525Bch2 and USAS 13/14 symbology
         */
        public static readonly int Symbology_2525Bch2_USAS_13_14 = 0;
        /**
         * 2525C, which includes 2525Bch2 & USAS 13/14
         */
        public static readonly int Symbology_2525C = 1;

        private static int _SymbologyStandard = 0;



        

        private Boolean _scaleEchelon = false;
        private Boolean _DrawAffiliationModifierAsLabel = true;
        private CanvasTextFormat _ModifierTextFormat = new CanvasTextFormat();
        private CanvasTextFormat _MPModifierTextFormat = new CanvasTextFormat();


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
            _ModifierTextFormat.FontFamily = "Arial";
            _ModifierTextFormat.FontSize = 10;
            _ModifierTextFormat.FontStyle = FontStyle.Normal;
            _ModifierTextFormat.FontWeight = FontWeights.Bold;

            _MPModifierTextFormat.FontFamily = "Arial";
            _MPModifierTextFormat.FontSize = 12;
            _MPModifierTextFormat.FontStyle = FontStyle.Normal;
            _MPModifierTextFormat.FontWeight = FontWeights.Bold;
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
         * Like RendererSettings.Symbology_2525Bch2_USAS_13_14
         */
        public void setSymbologyStandard(int standard)
        {
            _SymbologyStandard = standard;
        }

        /**
         * Current symbology standard
         * @return symbologyStandard
         * Like RendererSettings.Symbology_2525Bch2_USAS_13_14
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
         * Sets the font to be used for modifier labels
         * @param name Like "arial"
         * @param type Like Font.TRUETYPE_FONT
         * @param size Like 12
         */
        public void setLabelFont(String name, int size, FontWeight type)
        {
            _ModifierTextFormat.FontFamily = name;
            _ModifierTextFormat.FontSize = size;
            _ModifierTextFormat.FontStyle = FontStyle.Normal;
            _ModifierTextFormat.FontWeight = type;
        }

        /**
         * Sets the font to be used for modifier labels
         * @param name Like "arial"
         * @param type Like Font.TRUETYPE_FONT
         * @param size Like 12
         */
        public void setMPLabelFont(String name, int size, FontWeight type)
        {
            _MPModifierTextFormat.FontFamily = name;
            _MPModifierTextFormat.FontSize = size;
            _MPModifierTextFormat.FontStyle = FontStyle.Normal;
            _MPModifierTextFormat.FontWeight = type;
        }


        public CanvasTextFormat getLabelFont()
        {
            return _ModifierTextFormat;
        }

        public CanvasTextFormat getMPLabelFont()
        {
            return _MPModifierTextFormat;
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if(_ModifierTextFormat != null)
                        _ModifierTextFormat.Dispose();
                    if(_MPModifierTextFormat != null)
                        _MPModifierTextFormat.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RendererSettings() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
