﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MilSymNetUtilities;
using System.Reflection;
using System.IO;

namespace MilSymUwp
{
    public class SymbolSVGTable
    {
        private static Dictionary<int, SVGPath> _SymbolDefinitions = null;
         private static SymbolSVGTable _instance = null;
        private static object syncLock = new object();

        public static SymbolSVGTable getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new SymbolSVGTable();
                }
            }

            return _instance;
        }

        private SymbolSVGTable()
        {
            Init();
        }

        private void Init()
        {
            _SymbolDefinitions = new Dictionary<int, SVGPath>();

            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.SinglePoint.svg");
            StreamReader sr = new StreamReader(s);
            string fileContentPortable = sr.ReadToEnd();
            //MessageDialog msgPortable = new MessageDialog("From PortableClassLibrary: " + fileContentPortable);
            //await msgPortable.ShowAsync();

            //XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&#x", "").Replace("&", "&amp;"));
            XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&#x", "").Replace("&", "&amp;"));
            populateLookup(lookupXml);
        }

        private void populateLookup(XDocument xml)
        {
            IEnumerable<XElement> elements = null;
            try
            {
                //svg/def/font
                if (xml.Root.HasElements)
                {
                    //elements = xml.Root.Descendants("{http://www.w3.org/2000/svg}glyph");
                    // if(elements == null)
                    //elements = xml.Root.Descendants("glyph");
                    elements = xml.Root.Elements();
                    int ec = elements.Count();
                    IEnumerator<XElement> itr = null;

                    itr = elements.GetEnumerator();


                    XElement child = null;
                    int count = 0;
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        SVGPath temp = XNodeToSVGPath(child);
                        if ((temp != null) && _SymbolDefinitions.ContainsKey(temp.getID()) == false)//temp will be null if node is an XCOMMENT
                        {
                            int tempID = Convert.ToInt32(temp.getID());

                            //Debug.WriteLine(tempID);
                            tempID = tempID - 57000;
                            
                            if (tempID > 0 )
                                _SymbolDefinitions.Add(tempID, temp);
                            
                        }
                        //Debug.WriteLine("ID: " + temp.getID().ToString());
                        count++;
                    }//end while
                    Debug.WriteLine("# of glyphs: " + Convert.ToString(count));
                }//end if
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolDefTableSVG", "populateLookup", exc);
            }
        }//end populate lookup

        private XElement getFirstChildElement(IEnumerable<XElement> elements)
        {
            XElement element = null;

            IEnumerator<XElement> itr = elements.GetEnumerator();

            itr.MoveNext();
            element = itr.Current;
            return element;
        }

        private void printElements(IEnumerator<XElement> itr)
        {
            //IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
            //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
            //itr = elements.GetEnumerator();
            IEnumerable<XElement> elements = null;

            XElement child = null;
            itr.MoveNext();
            child = itr.Current;//SYMBOLCONSTANTS
            if (child.HasElements)
            {
                Debug.WriteLine(child.Name);
                /*
                elements = child.Elements();
                itr = elements.GetEnumerator();//SYMBOLS
                while (itr.MoveNext())
                {
                    child = itr.Current;
                    SymbolDef temp = SymbolDef.XNodeToSymbolDef(child);
                    if ((temp != null) && _SymbolDefinitions.ContainsKey(temp.getBasicSymbolId()) == false)//temp will be null if node is an XCOMMENT
                        _SymbolDefinitions[temp.getBasicSymbolId()] = temp;

                }//end while*/
            }
        }

        private SVGPath XNodeToSVGPath(XNode node)
        {
            SVGPath sp = null;
            XElement element = null;
            XAttribute attribute = null;

            string id = null;
            uint idInt = 0;
            string idHex = null;
            string path = null;
            try
            {
                if (node is XElement)
                {
                    element = (XElement)node;

                    if (element.HasAttributes)
                    {
                        IEnumerator<XAttribute> itr = null;// = xml.DescendantNodes().GetEnumerator();
                        itr = element.Attributes().GetEnumerator();

                        while (itr.MoveNext())
                        {
                            attribute = itr.Current;

                            if (attribute.Name == "unicode")
                            {
                                id = attribute.Value;//get element value
                                /* if (id.StartsWith("&#x"))
                                {
                                    idHex = id.Substring(3,4);
                                    
                                }

                                idInt = Convert.ToUInt32(idHex, 16);
                                id = Convert.ToString(idInt);*/

                            }
                            if (attribute.Name == "d")
                            {
                                path = attribute.Value;
                            }
                        }
                        if (id != null && path != null)
                            sp = new SVGPath(id, path);
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

            return sp;
        }

        /**
        * @name getSymbolDef
        *
        * @desc Returns a SymbolDef from the SymbolDefTable that matches the passed in Symbol Id
        *
        * @param index String representation of the index number
        * @return SVGPath
        */
        public SVGPath getSVGPath(int index)
        {
            SVGPath returnVal = null;
            try
            {
                if (_SymbolDefinitions.ContainsKey(index))
                    returnVal = new SVGPath(_SymbolDefinitions[index]);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                Debug.WriteLine(exc.StackTrace);
            }
            return returnVal;
        }



        /**
        *
        * @return
        */
        public Dictionary<int, SVGPath> getAllSVGPaths()
        {
            return _SymbolDefinitions;
        }

        /**
         * 
         * @param index
         * @return
         */
        public Boolean HasSVGPath(int index)
        {
            if (index > -1)
                return _SymbolDefinitions.ContainsKey(index);
            else
                return false;
        }
    }
}
