using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class UnitDef : IComparable<UnitDef>
    {


        /**
 * The basic 15 character basic symbol Id.
 */
        private String _strBasicSymbolId = "";
        public String getBasicSymbolId()
        {
            return _strBasicSymbolId;
        }

        /**
         * @private
         */
        public void setBasicSymbolId(String value)
        {
            _strBasicSymbolId = value;
        }

        /**
         * The description of this tactical graphic.  Typically the name of the tactical graphic in MIL-STD-2525B.
         */
        private String _strDescription;
        public String getDescription()
        {
            return _strDescription;
        }

        /**
         * @private
         */
        public void setDescription(String value)
        {
            _strDescription = value;
        }

        private int _intDrawCategory = 0;
        /**
         * 8 is singlepoint unit, 0 is category
         * (do not draw because it's just a category node in the tree)
         * @return
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
         * Defines where the symbol goes in the ms2525 hierarchy.
         * 2.X.whatever
         */
        private String _strHierarchy;
        public String getHierarchy()
        {
            return _strHierarchy;
        }

        /**
         * @private
         */
        public void setHierarchy(String value)
        {
            _strHierarchy = value;
        }

        /**
* Defines where the symbol goes in the ms2525 hierarchy.
* STBOPS.INDIV.WHATEVER
*/
        private String _strAlphaHierarchy;
        public String getAlphaHierarchy()
        {
            return _strAlphaHierarchy;
        }

        /**
         * @private
         */
        public void setAlphaHierarchy(String value)
        {
            _strAlphaHierarchy = value;
        }

        private String _strFullPath = "";
        /**
* Defines where the symbol goes in the ms2525 hierarchy.
 * Warfighting/something/something
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

        private UnitDef()
        {
            //Set fields to their default values.
            _strBasicSymbolId = "";
            _strDescription = "";
        }

        public UnitDef(String symbolID, String description, String hierarchy, int drawCategory, String alphaHierarchy, String path)
        {
            //Set fields to their default values.
            _strBasicSymbolId = symbolID;
            _strDescription = description;
            _strHierarchy = hierarchy;
            _intDrawCategory = drawCategory;
            _strAlphaHierarchy = alphaHierarchy;
            _strFullPath = path;
        }

        /**
         * @name Clone
         *
         * @desc Gets around the fact that Flex passes non-primitive data types (e.g. int, string) as by ref. So
         * clone creates a new object of this instance of UnitDef
         *
         * @param none
         * @return The newly cloned UnitDef
         */
        public UnitDef Clone()
        {
            UnitDef defReturn = new UnitDef();
            defReturn.setBasicSymbolId(this.getBasicSymbolId());
            defReturn.setDescription(this.getDescription());
            defReturn.setDrawCategory(this.getDrawCategory());
            defReturn.setHierarchy(this.getHierarchy());
            defReturn.setAlphaHierarchy(this.getAlphaHierarchy());
            defReturn.setFullPath(this.getFullPath());

            return defReturn;
        }

        public int CompareTo(UnitDef other)
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
                        Debug.WriteLine("One of these has a bad hierarchy");
                        Debug.WriteLine(this._strBasicSymbolId + " - " + other._strBasicSymbolId);
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
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }

            return returnVal;
        }

        /**
  *passed node is a child node
  **/
        public bool IsChild(UnitDef ud)
        {
            String str1 = this._strHierarchy;
            String str2 = ud._strHierarchy;
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
        public bool IsSibling(UnitDef ud)
        {
            int index = -1;
            String str1 = this._strHierarchy;
            index = str1.LastIndexOf(".");
            str1 = str1.Substring(0, index);

            String str2 = ud._strHierarchy;
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
        public bool IsParent(UnitDef ud)
        {
            String str1 = this._strHierarchy;
            String str2 = ud._strHierarchy.Substring(0, ud._strHierarchy.Length - 2);
            if (str1 == str2)
                return true;
            else
                return false;
        }

        public static UnitDef XNodeToUnitDef(XNode node)
        {
            UnitDef ud = null;
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
                        ud = new UnitDef();
                        subElement = null;
                        if (element.FirstNode is XElement)
                            subElement = (XElement)element.FirstNode;
                        while (subElement != null)
                        {

                            if (subElement.Name == "DESCRIPTION")
                            {
                                ud._strDescription = subElement.FirstNode.ToString();//get element value
                                ud._strDescription = ud._strDescription.Replace("&amp;", "&");
                            }
                            if (subElement.Name.LocalName == "SYMBOLID")
                            {
                                ud._strBasicSymbolId = subElement.FirstNode.ToString();
                                //if (ud.BasicSymbolCode == "S*A*MFQH--*****")
                                //{
                                //    Console.WriteLine("");
                                //}
                            }
                            if (subElement.Name.LocalName == "HIERARCHY")
                                ud._strHierarchy = subElement.FirstNode.ToString();
                            if (subElement.Name.LocalName == "ALPHAHIERARCHY")
                            {
                                if (subElement.FirstNode != null)
                                    ud._strAlphaHierarchy = subElement.FirstNode.ToString();
                                else
                                    ud._strAlphaHierarchy = "";
                            }
                            if (subElement.Name.LocalName == "DRAWCATEGORY")
                            {
                                if (subElement.FirstNode != null)
                                    ud._intDrawCategory = int.Parse(subElement.FirstNode.ToString());
                                else
                                    ud._intDrawCategory = 0;
                            }
                            if (subElement.Name.LocalName == "PATH")
                            {
                                if (subElement.FirstNode != null)
                                {
                                    ud._strFullPath = subElement.FirstNode.ToString();
                                    ud._strFullPath = ud._strFullPath.Replace("&amp;", "&");
                                }
                                else
                                    ud._strFullPath = "";

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
                Debug.WriteLine(xe.Message);
                Debug.WriteLine(xe.StackTrace);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }
            if (ud.getBasicSymbolId() != null && ud.getBasicSymbolId() != "")
                return ud;
            else
                return null;
        }

        public static XElement UnitDefToXElement(UnitDef ud)
        {
            //if (ud.BasicSymbolCode == "S*A*MFQH--*****")
            //{
            //    Console.WriteLine("");
            //} 
            return new XElement("SYMBOL",
                            new XElement("SYMBOLID", ud._strBasicSymbolId),
                            new XElement("DESCRIPTION", ud._strDescription),
                            new XElement("DRAWCATEGORY", ud._intDrawCategory),
                            new XElement("HIERARCHY", ud._strHierarchy),
                            new XElement("ALPHAHIERARCHY", ud._strAlphaHierarchy),
                            new XElement("PATH", ud._strFullPath));
        }
    }
}
