using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class SymbolDef : IComparable<SymbolDef>
    {
        String _strBasicSymbolId = "";
        String _strDescription = "";
        String _strSymbolType = "";
        String _strGeometry = "";
        //String _strDrawCategory = "";
        int _intDrawCategory = 99;
        int _intMinPoints = 0;
        int _intMaxPoints = 0;
        int _intWidth = 0;
        int _intHeight = 0;
        String _strModifiers = "";
        String _strHierarchy = "";
        String _strAlphaHierarchy = "";
        String _strFullPath = "";
         Boolean _hasWidth = false;

        int _intUpperleftX = 0;
        int _intUpperleftY = 0;
        int _intLowerrightX = 0;
        int _intLowerrightY = 0;


       /**
        * Just a category in the milstd hierarchy.
        * Not something we draw.
        * WILL NOT RENDER
        */
       public const int DRAW_CATEGORY_DONOTDRAW = 0;

       /**
        * A polyline, a line with n number of points.
        * 0 control points
        */
       public const int DRAW_CATEGORY_LINE = 1;

       /**
        * An animated shape, uses the animate function to draw.
        * 0 control points (every point shapes symbol)
        */
       public const int DRAW_CATEGORY_AUTOSHAPE = 2;

       /**
        * An enclosed polygon with n points
        * 0 control points
        */
       public const int DRAW_CATEGORY_POLYGON = 3;
       /**
        * A polyline with n points (entered in reverse order)
        * 0 control points
        */
       public const int DRAW_CATEGORY_ARROW = 4;
       /**
        * A graphic with n points whose last point defines the width of the graphic.
        * 1 control point
        */
       public const int DRAW_CATEGORY_ROUTE = 5;
       /**
        * A line defined only by 2 points, and cannot have more.
        * 0 control points
        */
       public const int DRAW_CATEGORY_TWOPOINTLINE = 6;
       /**
        * Shape is defined by a single point
        * 0 control points
        */
       public const int DRAW_CATEGORY_POINT = 8;
       /**
        * A polyline with 2 points (entered in reverse order).
        * 0 control points
        */
       public const int DRAW_CATEGORY_TWOPOINTARROW = 9;
       /**
        * An animated shape, uses the animate function to draw. Super Autoshape draw
        * in 2 phases, usually one to define length, and one to define width.
        * 0 control points (every point shapes symbol)
        *
        */
       public const int DRAW_CATEGORY_SUPERAUTOSHAPE = 15;
        /**
        * Circle that requires 1 AM modifier value.
        * See ModifiersTG.java for modifier descriptions and constant key strings.
        */
       public const int DRAW_CATEGORY_CIRCULAR_PARAMETERED_AUTOSHAPE = 16;
       /**
        * Rectangle that requires 2 AM modifier values and 1 AN value.";
        * See ModifiersTG.java for modifier descriptions and constant key strings.
        */
       public const int DRAW_CATEGORY_RECTANGULAR_PARAMETERED_AUTOSHAPE = 17;
       /**
        * Requires 2 AM values and 2 AN values per sector.  
        * The first sector can have just one AM value although it is recommended 
        * to always use 2 values for each sector.  X values are not required
        * as our rendering is only 2D for the Sector Range Fan symbol.
        * See ModifiersTG.java for modifier descriptions and constant key strings.
        */
       public const int DRAW_CATEGORY_SECTOR_PARAMETERED_AUTOSHAPE = 18;
       /**
        *  Requires at least 1 distance/AM value"
        *  See ModifiersTG.java for modifier descriptions and constant key strings.
        */
       public const int DRAW_CATEGORY_CIRCULAR_RANGEFAN_AUTOSHAPE = 19;
       /**
        * Requires 1 AM value.
        * See ModifiersTG.java for modifier descriptions and constant key strings.
        */
       public const int DRAW_CATEGORY_TWO_POINT_RECT_PARAMETERED_AUTOSHAPE = 20;
   
       /**
        * 3D airspace, not a milstd graphic.
        */
       public const int DRAW_CATEGORY_3D_AIRSPACE = 40;
   
       /**
        * UNKNOWN.
        */
       public const int DRAW_CATEGORY_UNKNOWN = 99;

        public SymbolDef()
        {

        }


        /**
         * The basic 15 character basic symbol Id.
         */

        public String getBasicSymbolId()
        {
            return _strBasicSymbolId;
        }

        /**
         * 
         */
        public void setBasicSymbolId(String value)
        {
            _strBasicSymbolId = value;
        }

        /**
         * The description of this tactical graphic.  Typically the name of the tactical graphic in MIL-STD-2525B.
         */
        public String getDescription()
        {
            return _strDescription;
        }

        /**
         * 
         */
        public void setDescription(String value)
        {
            _strDescription = value;
        }

        /**
         * What kind of symbol is it? (Bridge, Critical Point, Check Point, Road, Route, etc)
         */
        public String getSymbolType()
        {
            return _strSymbolType;
        }

        /**
         * What kind of symbol is it? (Bridge, Critical Point, Check Point, Road, Route, etc)
         */
        public String setSymbolType(String value)
        {
            return _strSymbolType = value;
        }

        /**
         * 
         */
        public String getGeometry()
        {
            return _strGeometry;
        }

        /**
         * 
         */
        public void setGeometry(String value)
        {
            _strGeometry = value;
        }

        /**
         * How does this draw? (autoshape, superautoshape, polygon)
         * 
         */
        public int getDrawCategory()
        {
            return _intDrawCategory;
        }

        /**
         * 
         */
        public void setDrawCategory(int value)
        {
            _intDrawCategory = value;
        }

        /**
         * Defines the minimum points fields.
         */
        public int getMinPoints()
        {
            return _intMinPoints;
        }

        /**
         * 
         */
        public void setMinPoints(int value)
        {
            _intMinPoints = value;
        }

        /**
         * Defines the maximum points fields.
         */
        public int getMaxPoints()
        {
            return _intMaxPoints;
        }

        /**
         * 
         */
        public void setMaxPoints(int value)
        {
            _intMaxPoints = value;
        }

        public Boolean HasWidth()
        {
            return _hasWidth;
        }

        public void HasWidth(Boolean value)
        {
            _hasWidth = value;
        }

        /**
         *
         */
        public String getModifiers()
        {
            return _strModifiers;
        }

        /**
         * 
         */
        public void setModifiers(String value)
        {
            _strModifiers = value;
        }

        /**
         * Defines where the symbol goes in the ms2525 hierarchy.
         * 2.X.etc...
         */
        public String getHierarchy()
        {
            return _strHierarchy;
        }

        /**
         *
         */
        public void setHierarchy(String value)
        {
            _strHierarchy = value;
        }

        /**
 * Defines where the symbol goes in the ms2525 hierarchy.
 * WAR.GRDTRK.UNT.etc...
 */
        public String getAlphaHierarchy()
        {
            return _strAlphaHierarchy;
        }

        /**
         *
         */
        public void setAlphaHierarchy(String value)
        {
            _strAlphaHierarchy = value;
        }

        /**
        * Defines where the symbol goes in the ms2525 hierarchy.
         * TacticalGraphics/Areas/stuff...
        */
        public String getFullPath()
        {
            return _strFullPath;
        }

        /**
         *
         */
        public void setFullPath(String value)
        {
            _strFullPath = value;
        }

        /**
         * Defines the upperleftX fields.
         */
        public int getUpperleftX()
        {
            return _intUpperleftX;
        }

        /**
         * 
         */
        public void setUpperleftX(int value)
        {
            _intUpperleftX = value;
        }

        /**
         * Defines the upperleftY fields.
         */
        public int getUpperleftY()
        {
            return _intUpperleftY;
        }

        /**
         * 
         */
        public void setUpperleftY(int value)
        {
            _intUpperleftY = value;
        }

        /**
         * Defines the lowerrightY fields.
         */
        public int getLowerrightY()
        {
            return _intLowerrightY;
        }

        /**
         * 
         */
        public void setLowerrightY(int value)
        {
            _intLowerrightY = value;
        }

        /**
         * Defines the lowerrightX fields.
         */

        public int getLowerrightX()
        {
            return _intLowerrightX;
        }

        /**
         * 
         */
        public void setLowerrightX(int value)
        {
            _intLowerrightX = value;
        }

        public int getHeight()
        {
            return _intHeight;
        }
        public int getWidth()
        {
            return _intWidth;
        }

        public int CompareTo(SymbolDef other)
        {
            int returnVal = -99;

            try
            {
                if (other == null)
                {
                    throw new ArgumentException("Passed object is null");
                }

                String str1 = _strHierarchy;
                String str2 = other._strHierarchy;
                int xLocation = 0;

                xLocation = str1.IndexOf("X");
                if (xLocation > 0)
                    str1 = str1.Remove(xLocation - 1, 2);//get rid of the ".X"

                xLocation = str2.IndexOf("X");
                if (str2.IndexOf("X") > 0)
                    str2 = str2.Remove(xLocation - 1, 2);//get rid of the ".X"

                String[] arr1 = null;
                String[] arr2 = null;

                arr1 = str1.Split(".".ToCharArray());
                arr2 = str2.Split(".".ToCharArray());

                int code1 = 0;
                int code2 = 0;

                int count1 = arr1.Length;
                int count2 = arr2.Length;

                int lcv = 0;
                while (returnVal == -99)
                {
                    code1 = -99;
                    code2 = -99;

                    code1 = Convert.ToInt32(arr1[lcv]);

                    code2 = Convert.ToInt32(arr2[lcv]);



                    if (str1 == null || str1 == "" && str2 == null || str2 == "")
                    {
                        returnVal = 0;//should never happen
                        Console.WriteLine("One of these has a bad hierarchy");
                        Console.WriteLine(this._strBasicSymbolId + " - " + other._strBasicSymbolId);
                        return 0;
                    }
                    else if (code1 < code2)
                        returnVal = -1;
                    else if (code1 > code2)
                        returnVal = 1;
                    else if (code1 == code2 && count1 == lcv + 1 && count2 == lcv + 1)
                        returnVal = 0;//should never happen if hierarchies are correct
                    else if (code1 == code2 && count1 == lcv + 1 && count2 >= lcv)//equal but hierarchy1 has ended
                        returnVal = -1;
                    else if (code1 == code2 && count1 >= lcv && lcv == count2 - 1)//equal but hierarchy2 has ended
                        returnVal = 1;
                    lcv++;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }

            return returnVal;
        }

        /**
  *passed node is a child node
  **/
        public bool IsChild(SymbolDef sd)
        {
            String str1 = this._strHierarchy;
            String str2 = sd._strHierarchy;
            int xLocation = 0;

            xLocation = str1.IndexOf("X");
            if (xLocation > 0)
                str1 = str1.Remove(xLocation - 1, 2);//get rid of the ".X"

            xLocation = str2.IndexOf("X");
            if (str2.IndexOf("X") > 0)
                str2 = str2.Remove(xLocation - 1, 2);//get rid of the ".X"

            String[] arr1 = null;
            String[] arr2 = null;

            arr1 = str1.Split(".".ToCharArray());
            arr2 = str2.Split(".".ToCharArray());


            int count1 = arr1.Length;
            int count2 = arr2.Length;

            if (count2 == count1 + 1)
            {
                int index = str2.LastIndexOf(".");
                str2 = str2.Substring(0, index);
                if (str1 == str2)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /**
        *passed node is a sibling
        **/
        public bool IsSibling(SymbolDef sd)
        {
            int index = -1;
            String str1 = this._strHierarchy;
            index = str1.LastIndexOf(".");
            str1 = str1.Substring(0, index);

            String str2 = sd._strHierarchy;
            index = str2.LastIndexOf(".");
            str2 = str2.Substring(0, index);

            //parent strings equal or they're both root nodes
            if (str1 == str2 || (str1.IndexOf(".") == -1 && str2.IndexOf(".") == -1))
                return true;
            else
                return false;
        }

        /**
        *passed node is the parent
        **/
        public bool IsParent(SymbolDef sd)
        {
            String str1 = this._strHierarchy;
            String str2 = sd._strHierarchy.Substring(0, sd._strHierarchy.Length - 2);
            if (str1 == str2)
                return true;
            else
                return false;
        }

        public static SymbolDef XNodeToSymbolDef(XNode node)
        {
            SymbolDef sd = null;
            XElement element = null;
            XElement subElement = null;
            //XAttribute attribute = null;
            XNode nextNode = null;
            try
            {
                if (node is XElement)
                {
                    element = (XElement)node;

                    if (element.HasElements)
                    {
                        sd = new SymbolDef();
                        subElement = null;
                        if (element.FirstNode is XElement)
                            subElement = (XElement)element.FirstNode;
                        while (subElement != null)
                        {

                            if (subElement.Name == "DESCRIPTION")
                            {
                                sd._strDescription = subElement.FirstNode.ToString();//get element value
                                sd._strDescription = sd._strDescription.Replace("&amp;", "&");
                            }
                            if (subElement.Name.LocalName == "SYMBOLID")
                            {
                                sd._strBasicSymbolId = subElement.FirstNode.ToString();
                            }
                            if (subElement.Name.LocalName == "HIERARCHY")
                                sd._strHierarchy = subElement.FirstNode.ToString();
                            if (subElement.Name.LocalName == "ALPHAHIERARCHY")
                            {
                                if (subElement.FirstNode != null)
                                    sd._strAlphaHierarchy = subElement.FirstNode.ToString();
                                else
                                    sd._strAlphaHierarchy = "";
                            }
                            if (subElement.Name.LocalName == "DRAWCATEGORY")
                            {
                                if (subElement.FirstNode != null)
                                    sd._intDrawCategory = int.Parse(subElement.FirstNode.ToString());
                                else
                                    sd._intDrawCategory = 0;
                            }
                            if (subElement.Name.LocalName == "PATH")
                            {
                                if (subElement.FirstNode != null)
                                {
                                    sd._strFullPath = subElement.FirstNode.ToString();
                                    sd._strFullPath = sd._strFullPath.Replace("&amp;", "&");
                                }
                                else
                                    sd._strFullPath = "";

                            }
                            if (subElement.Name.LocalName == "MODIFIERS")
                            {
                                if (subElement.FirstNode != null)
                                {
                                    sd._strModifiers = subElement.FirstNode.ToString();
                                }
                                else
                                    sd._strModifiers = "";
                            }
                            if (subElement.Name.LocalName == "MAXPOINTS")
                                sd._intMaxPoints = int.Parse(subElement.FirstNode.ToString());
                            if (subElement.Name.LocalName == "MINPOINTS")
                                sd._intMinPoints = int.Parse(subElement.FirstNode.ToString());
                            if (subElement.Name.LocalName == "GEOMETRY")
                                sd._strGeometry = subElement.FirstNode.ToString();
                            if (subElement.Name.LocalName == "HASWIDTH")
                            {
                                if (subElement.FirstNode.ToString() == "yes" || subElement.FirstNode.ToString() == "true")
                                    sd._hasWidth = true;
                                else
                                    sd._hasWidth = false;
                            }


                            nextNode = subElement.NextNode;
                            subElement = null;
                            while (subElement == null && nextNode != null)
                            {
                                if (nextNode is XElement)
                                    subElement = (XElement)nextNode;
                                else
                                    nextNode = nextNode.NextNode;
                            }
                        }
                    }
                }
                else
                    return null;
            }
            catch (System.Xml.XmlException xe)
            {
                Console.WriteLine(xe.Message);
                Console.WriteLine(xe.StackTrace);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }

            return sd;
        }

        public static XElement SymbolDefToXElement(SymbolDef sd)
        {
            String hw = "no";
            if (sd._hasWidth)
                hw = "yes";
            return new XElement("SYMBOL",
                            new XElement("SYMBOLID", sd._strBasicSymbolId),
                            new XElement("GEOMETRY", sd._strGeometry),
                            new XElement("DRAWCATEGORY", sd._intDrawCategory.ToString()),
                            new XElement("MAXPOINTS", sd._intMaxPoints.ToString()),
                            new XElement("MINPOINTS", sd._intMinPoints.ToString()),
                            new XElement("HASWIDTH", hw),
                            new XElement("MODIFIERS", sd._strModifiers),
                            new XElement("DESCRIPTION", sd._strModifiers),
                            new XElement("HIERARCHY", sd._strHierarchy),
                            new XElement("ALPHAHIERARCHY", sd._strAlphaHierarchy),
                            new XElement("PATH", sd._strFullPath));
        }
    }
}
