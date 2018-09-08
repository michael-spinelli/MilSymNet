using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
   public class SinglePointLookupInfo
    {

        private String _SymbolID = "";
        private String _Description = "";
        private int _mappingP = 0;
        private int _mappingA = 0;
        private int _height = 0;
        private int _width = 0;

        public SinglePointLookupInfo()      
        {
        }


        public SinglePointLookupInfo(String basicSymbolID, String description,
                                    String mappingP, String mappingA, String width, String height)
        {
            _SymbolID = basicSymbolID;
            _Description = description;
            if (mappingP != null && mappingP!=(""))
                _mappingP = Convert.ToInt16(mappingP);
            if (mappingA != null && mappingA !=(""))
                _mappingA =Convert.ToInt16(mappingA);
            if (height != null && height != (""))
                _height = Convert.ToInt16(height);
            if (width != null && width != (""))
                _width = Convert.ToInt16(width);
        }
    
        public String getBasicSymbolID()
        {
                return _SymbolID;
        }

   
        public String getDescription()
        {
                return _Description;
        }

        public int getMappingP()
        {
                return _mappingP;
        }

        public int getMappingA()
        {
                return _mappingA;
        }

        public int getHeight()
        {
                return _height;
        }

        public int getWidth()
        {
                return _width;
        }
    
       /**
       * 
       * return The newly cloned SPSymbolDef
       */
      //@Override
 
        public SinglePointLookupInfo clone()
          {
            SinglePointLookupInfo defReturn;
            defReturn = new SinglePointLookupInfo(_SymbolID, _Description, 
                    getMappingP().ToString(), 
                    getMappingA().ToString(),
                    getWidth().ToString(), 
                    getHeight().ToString());
            return defReturn;
          }

  
        public String toXML()
        {
        String symbolId = "<SYMBOLID>" +  getBasicSymbolID() + "</SYMBOLID>";
        String mappingP = "<MAPPINGP>" + getMappingP().ToString() + "</MAPPINGP>";
        String mappingA = "<MAPPINGA>" + getMappingA().ToString() + "</MAPPINGA>";
        String description = "<DESCRIPTION>" + getDescription() + "</DESCRIPTION>";
        String width = "<WIDTH>" + getWidth().ToString() + "</WIDTH>";
        String height = "<HEIGHT>" + getHeight().ToString() + "</HEIGHT>";
        String xml = symbolId + mappingP + mappingA + description + width + height;
        return xml;
        }

        public static SinglePointLookupInfo XNodeToSinglePointFontLookupInfo(XNode node)
        {
            SinglePointLookupInfo spli = null;
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
                        spli = new SinglePointLookupInfo();
                        subElement = null;
                        if (element.FirstNode is XElement)
                            subElement = (XElement)element.FirstNode;
                        while (subElement != null)
                        {

                            if (subElement.Name == "DESCRIPTION")
                            {
                                spli._Description = subElement.FirstNode.ToString();//get element value
                                //ufli._strDescription = ufli._strDescription.Replace("&amp;", "&");
                            }
                            else if (subElement.Name.LocalName == "SYMBOLID")
                            {
                                spli._SymbolID = subElement.FirstNode.ToString();
                            }
                            else if (subElement.Name.LocalName == "MAPPINGA")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    spli._mappingA = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPINGP")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    spli._mappingP = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "HEIGHT")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    spli._height = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "WIDTH")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    spli._width = int.Parse(subElement.FirstNode.ToString());
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
            if (spli.getBasicSymbolID() != null && spli.getBasicSymbolID() != "")
                return spli;
            else
                return null;
        }

    }
}
