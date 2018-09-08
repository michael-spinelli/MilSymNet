using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace MilSymNetUtilities
{
    public class UnitDefTable
    {
            private static UnitDefTable _instance = null;
        //private static SymbolTableThingy
        private static Dictionary<String, UnitDef> _UnitDefinitions = null;

        private static String propSymbolID = "SYMBOLID";
        private static String propDrawCategory = "DRAWCATEGORY";
        private static String propModifiers = "MODIFIERS";
        private static String propDescription = "DESCRIPTION";
        private static String propHierarchy = "HIERARCHY";
        private static String propAlphaHierarchy = "ALPHAHIERARCHY";
        private static String propPath = "PATH";

        private static object syncLock = new object();

        private UnitDefTable()
        {
            Init();
        }

        public static UnitDefTable getInstance()
        {
            if(_instance == null)
            {
                lock(syncLock)
                {
                    if(_instance == null)
                        _instance = new UnitDefTable();
                }
            }

            return _instance;
        }

       /* public String[] searchByHierarchy(String hierarchy)
        {
            for(UnitDef foo : _UnitDefinitions.values() )
            {
                if(foo.getHierarchy().equalsIgnoreCase(hierarchy))
                {
                    return
                }
            }
        }*/

        private void Init()
        {
            _UnitDefinitions = new Dictionary<String, UnitDef>();
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.UnitConstantsC.Replace("&","&amp;"));
            //populateLookup(lookupXml);

            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.UnitConstantsC.xml");
            StreamReader sr = new StreamReader(s);
            string fileContentPortable = sr.ReadToEnd();
            //MessageDialog msgPortable = new MessageDialog("From PortableClassLibrary: " + fileContentPortable);
            //await msgPortable.ShowAsync();

            XDocument lookupXml = XDocument.Parse(fileContentPortable.Replace("&", "&amp;"));
            populateLookup(lookupXml);
        }

        private void populateLookup(XDocument xml)
        {
            IEnumerable<XElement> elements = null;

            if (xml.Element("UNITCONSTANTS").HasElements)
            {
                elements = xml.Elements();
                
                IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
                //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
                itr = elements.GetEnumerator();
                

                XElement child = null;
                itr.MoveNext();
                child = itr.Current;//UNITCONSTANTS
                if (child.HasElements)
                {
                    elements = child.Elements();
                    itr = elements.GetEnumerator();//SYMBOLS
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        UnitDef temp = UnitDef.XNodeToUnitDef(child);
                        if ((temp != null) && _UnitDefinitions.ContainsKey(temp.getBasicSymbolId())==false)//temp will be null if node is an XCOMMENT
                            _UnitDefinitions.Add(temp.getBasicSymbolId(), temp);

                    }//end while
                }
            }//end if
        }//end populate lookup

        /**
        * @name getSymbolDef
        *
        * @desc Returns a SymbolDef from the SymbolDefTable that matches the passed in Symbol Id
        *
        * @param strBasicSymbolID - IN - A 15 character MilStd code
        * @return SymbolDef whose Symbol Id matches what is passed in
        */
        public UnitDef getUnitDef(String basicSymbolID)
        {
            UnitDef returnVal = null;
            if(_UnitDefinitions.ContainsKey(basicSymbolID))
                returnVal = _UnitDefinitions[basicSymbolID];
            return returnVal;
        }



        /**
         *
         * @return
         */
        public Dictionary<String, UnitDef> GetAllUnitDefs()
        {
    	    return _UnitDefinitions;
        }

        /**
         * 
         * @param basicSymbolID
         * @return
         */
        public Boolean HasUnitDef(String basicSymbolID)
        {
            if(basicSymbolID != null && basicSymbolID.Length == 15)
                return _UnitDefinitions.ContainsKey(basicSymbolID);
            else
                return false;
        }
    }
}
