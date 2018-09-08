using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class TacticalGraphicLookupInfo
    {
        private String _SymbolID = "";
        private int _mapping = -1;

        public TacticalGraphicLookupInfo(String basicSymbolID, String mapping)
        {
            _SymbolID = basicSymbolID;
            if (mapping != null && mapping!=(""))
                _mapping = Convert.ToInt16(mapping);
        }
    
        public String getBasicSymbolID()
        {
                return _SymbolID;
        }

        public int getMapping()
        {
                return _mapping;
        }

        public static TacticalGraphicLookupInfo XNodeToTGLookupInfo(XNode node)
        {
            XElement element = null;
            XElement subElement = null;
            //XAttribute attribute = null;
            XNode nextNode = null;
            
            string symbolID = "";
            string mapping = "-1";
            try
            {
                if (node is XElement)
                {
                    element = (XElement)node;

                    if (element.HasElements)
                    {
                        
                        subElement = null;
                        if (element.FirstNode is XElement)
                            subElement = (XElement)element.FirstNode;
                        while (subElement != null)
                        {
                            if (subElement.Name == "SYMBOLID")
                            {
                                symbolID = subElement.FirstNode.ToString();//get element value
                            }
                            else if (subElement.Name == "MAPPING")
                            {
                                mapping = subElement.FirstNode.ToString();//get element value
                                mapping = Convert.ToString(Convert.ToInt32(mapping));
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


            return new TacticalGraphicLookupInfo(symbolID, mapping);
        }
    }
}
