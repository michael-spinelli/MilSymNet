using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MilSymNetUtilities
{
    public class UnitFontLookup
    {

        #region Fill indexes
          //UNKNOWN FILL Indexes
          private static int FillIndexUZ = 800;
          private static int FillIndexUP = 849;
          private static int FillIndexUA = 825;
          private static int FillIndexUG = 800;
          private static int FillIndexUGE = 800;
          private static int FillIndexUS = 800;
          private static int FillIndexUU = 837;
          private static int FillIndexUF = 800;
           //FRIENDLY FILL Indexes
          private static int FillIndexFZ = 812;
          private static int FillIndexFP = 843;
          private static int FillIndexFA = 819;
          private static int FillIndexFG = 803;
          private static int FillIndexFGE = 812;
          private static int FillIndexFS = 812;
          private static int FillIndexFU = 831;
          private static int FillIndexFF = 803;
           //NEUTRAL FILL Indexes
          private static int FillIndexNZ = 809;
          private static int FillIndexNP = 846;
          private static int FillIndexNA = 822;
          private static int FillIndexNG = 809;
          private static int FillIndexNGE = 809;
          private static int FillIndexNS = 809;
          private static int FillIndexNU = 834;
          private static int FillIndexNF = 809;
           //HOSTILE FILL Indexes
          private static int FillIndexHZ = 806;
          private static int FillIndexHP = 840;
          private static int FillIndexHA = 816;
          private static int FillIndexHG = 806;
          private static int FillIndexHGE = 806;
          private static int FillIndexHS = 806;
          private static int FillIndexHU = 828;
          private static int FillIndexHF = 806;

          //Font positions for new layout
          //770-799: small fills, inside the frame
          //800-900: regular fills with solid & dashed frame
          //1000 - 3000?: Warfighting - A -
          //3000 - 3200?: SigInt - D - 80 only need fill/frame/symbol
          //3200? - 3400?: Stability Operations Symbology - E - 60 w/ 6 that need 4 symbols & 6 with secondary symbol / fill
          //4000+: Emergency Management Symbols - G -
        #endregion

        private static Dictionary<String, UnitFontLookupInfo> _UnitMappings = null;

        private static UnitFontLookup _instance = null;
        private static object syncLock = new object();

        public static UnitFontLookup getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new UnitFontLookup();
                }
            }

            return _instance;
        }

        private UnitFontLookup()
        {
            Init();
        }

        private void Init()
        {
            _UnitMappings = new Dictionary<String, UnitFontLookupInfo>();
            //XDocument lookupXml = XDocument.Parse(Properties.Resources.UnitFontMappingsC.Replace("&","&amp;"));

            var s = Assembly.Load(new AssemblyName("MilSymNetUtilities")).GetManifestResourceStream(@"MilSymNetUtilities.XML.UnitFontMappingsC.xml");
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

            if (xml.Element("UNITFONTMAPPINGS").HasElements)
            {
                elements = xml.Elements();

                IEnumerator<XElement> itr = null;// = xml.DescendantNodes().GetEnumerator();
                //XElement unitConstants = XElement.Load(ofd1.FileName, LoadOptions.None);
                itr = elements.GetEnumerator();


                XElement child = null;
                itr.MoveNext();
                child = itr.Current;//SYMBOLCONSTANTS
                if (child.HasElements)
                {
                    elements = child.Elements();
                    itr = elements.GetEnumerator();//SYMBOLS
                    while (itr.MoveNext())
                    {
                        child = itr.Current;
                        UnitFontLookupInfo temp = UnitFontLookupInfo.XNodeToUnitFontLookupInfo(child);
                        if ((temp != null) && _UnitMappings.ContainsKey(temp.getBasicSymbolID()) == false)//temp will be null if node is an XCOMMENT
                            _UnitMappings[temp.getBasicSymbolID()] = temp;

                    }//end while
                }
            }//end if
        }//end populate lookup

               /**
          * we only have font lookups for F,H,N,U.  But the shapes match one of these
          * four for the remaining affiliations.  So we convert the string to a base
          * affiliation before we do the lookup.
          * @param symbolID
          * @return
          */
               private String resolveAffiliation(String symbolID)
               {
                   String code = symbolID.Substring(0,15);
                   String affiliation = symbolID.Substring(1, 1);

                   if (affiliation==("F") ||//friendly
                           affiliation==("H") ||//hostile
                           affiliation==("U") ||//unknown
                           affiliation==("N"))//neutral
                       return code;
                   else if (affiliation==("S"))//suspect
                       code = code.Substring(0, 1) + "H" + code.Substring(2, 13);
                   else if (affiliation==("L"))//exercise neutral
                       code = code.Substring(0, 1) + "N" + code.Substring(2, 13);
                   else if (affiliation==("A") ||//assumed friend
                           affiliation==("D") ||//exercise friend
                           affiliation==("M") ||//exercise assumed friend
                           affiliation==("K") ||//faker
                           affiliation==("J"))//joker
                       code = code.Substring(0, 1) + "F" + code.Substring(2, 13);
                   else if (affiliation==("P") ||//pending
                           affiliation==("G") ||//exercise pending
                           affiliation==("O") ||//? brought it over from mitch's code
                           affiliation==("W"))//exercise unknown
                       code = code.Substring(0, 1) + "U" + code.Substring(2, 13);
                   else
                       code = code.Substring(0, 1) + "U" + code.Substring(2, 13);

                   return code;
               }




               /**
                * 2525C
                * returns the character index for the fill frame based on the symbol code.
                * @param SymbolID 15 character symbol ID
                * @return fill character index
                */
               public static int getFillCode(String SymbolID)
               {
                   int returnVal = -1;

                   char scheme = '0';
                   char battleDimension = '0';
                   char status = '0';
                   char affiliation = '0';
                   char grdtrkSubset = '0';
                   //char foo = 'a';


                   try
                   {
                       //to upper
                       if (SymbolID != null && SymbolID.Length  == 15)
                       {
                           char[] arrID = SymbolID.ToCharArray();
                           scheme = arrID[0];//S,O,E,I,etc...
                           affiliation = arrID[1];//F,H,N,U,etc...
                           battleDimension = arrID[2];//P,A,G,S,U,F,X,Z
                           status = arrID[3];//A,P,C,D,X,F
                           grdtrkSubset = arrID[4];

                           if (scheme == 'S')//Warfighting symbols
                           {
                               if (affiliation == 'F' ||
                                       affiliation == 'A' ||
                                       affiliation == 'D' ||
                                       affiliation == 'M' ||
                                       affiliation == 'J' ||
                                       affiliation == 'K')
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 812;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       if (battleDimension == 'F' ||
                                               (battleDimension == 'G' &&
                                                 (grdtrkSubset == 'U' || grdtrkSubset == 'I' || grdtrkSubset == '0' || grdtrkSubset == '-')))
                                       {
                                           returnVal = 803;
                                       }
                                       else if (battleDimension == 'S' ||
                                               (battleDimension == 'G' && grdtrkSubset == 'E'))
                                       {
                                           returnVal = 812;
                                       }
                                       else
                                           returnVal = 803;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 819;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = getSubSurfaceFill(SymbolID);
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 843;
                                   }
                               }
                               else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 806;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       returnVal = 806;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 816;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = getSubSurfaceFill(SymbolID);
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 840;
                                   }
                                   else
                                       returnVal = 806;
                               }
                               else if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 809;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       returnVal = 809;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 822;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = getSubSurfaceFill(SymbolID);
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 846;
                                   }
                                   else
                                       returnVal = 809;
                               }
                               else if (affiliation == 'P' ||
                                  affiliation == 'U' ||
                                  affiliation == 'G' ||
                                  affiliation == 'W')
                               {

                                   if (battleDimension == 'Z' ||//unknown
                                         battleDimension == 'G' ||//ground
                                         battleDimension == 'S' ||//sea surface
                                         battleDimension == 'F')//SOF
                                   {
                                       returnVal = 800;//index in font file
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 825;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = getSubSurfaceFill(SymbolID);
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 849;
                                   }
                                   else
                                       returnVal = 800;
                               }
                           }//end if scheme == 's'
                           else if (scheme == 'E')//Emergency Management Symbols
                           {
                               if (affiliation == 'F' ||
                                       affiliation == 'A' ||
                                       affiliation == 'D' ||
                                       affiliation == 'M' ||
                                       affiliation == 'J' ||
                                       affiliation == 'K')
                               {

                                   //EMS symbols break some rules about symbol codes
                                   if (SymbolUtilities.isEMSEquipment(SymbolID))
                                       returnVal = 812;
                                   else if (battleDimension != 'N')//natural events
                                       returnVal = 803;
                                   else
                                       returnVal = -1;//natural events do not have a fill/frame
                               }
                               else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                               {
                                   returnVal = 806;//index in font file

                               }
                               else if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                               {
                                   returnVal = 809;
                               }
                               else /*if(affiliation == 'P' ||
                                     affiliation == 'U' ||
                                     affiliation == 'G' ||
                                     affiliation == 'W')*/
                               {
                                   returnVal = 800;//index in font file
                               }
                           }//end if scheme == 'E'
                           else if (scheme == 'I')//Also default behavior
                           {
                               if (affiliation == 'F' ||
                                       affiliation == 'A' ||
                                       affiliation == 'D' ||
                                       affiliation == 'M' ||
                                       affiliation == 'J' ||
                                       affiliation == 'K')
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 812;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       if (scheme == 'I')
                                           returnVal = 812;
                                       else
                                           returnVal = 803;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 819;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = 831;
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 843;
                                   }
                                   else
                                   {
                                       if (scheme == 'I')
                                           returnVal = 812;
                                       else
                                           returnVal = 803;
                                   }
                               }
                               if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 806;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       returnVal = 806;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 816;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = 828;
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 840;
                                   }
                                   else
                                   {
                                       returnVal = 806;
                                   }
                               }
                               if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                               {
                                   if (battleDimension == 'Z')//unknown
                                   {
                                       returnVal = 809;//index in font file
                                   }
                                   else if (battleDimension == 'F' || battleDimension == 'G' || battleDimension == 'S')//ground & SOF & sea surface
                                   {
                                       returnVal = 809;
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 822;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = 834;
                                   }
                                   else if (battleDimension == 'P')//space
                                   {
                                       returnVal = 846;
                                   }
                                   else
                                   {
                                       returnVal = 809;
                                   }
                               }
                               else if (affiliation == 'P' ||
                                  affiliation == 'U' ||
                                  affiliation == 'G' ||
                                  affiliation == 'W')
                               {

                                   if (battleDimension == 'Z' ||//unknown
                                         battleDimension == 'G' ||//ground
                                         battleDimension == 'S' ||//sea surface
                                         battleDimension == 'F')//SOF
                                   {
                                       returnVal = 800;//index in font file
                                   }
                                   else if (battleDimension == 'A')//Air
                                   {
                                       returnVal = 825;
                                   }
                                   else if (battleDimension == 'U')//Subsurface
                                   {
                                       returnVal = 837;
                                   }
                                   else if (battleDimension == 'P')//Subsurface
                                   {
                                       returnVal = 849;
                                   }
                                   else
                                   {
                                       returnVal = 800;
                                   }
                               }
                           }//end if scheme == 'I'
                           else//scheme = 'O' and anything else
                           {
                               if (affiliation == 'F' ||
                                       affiliation == 'A' ||
                                       affiliation == 'D' ||
                                       affiliation == 'M' ||
                                       affiliation == 'J' ||
                                       affiliation == 'K')
                               {

                                   //EMS symbols break some rules about symbol codes
                                   if (SymbolUtilities.isEMSEquipment(SymbolID))
                                       returnVal = 812;
                                   else if (battleDimension != 'N')//natural events
                                       returnVal = 803;
                                   else
                                       returnVal = -1;//natural events do not have a fill/frame
                               }
                               else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                               {
                                   returnVal = 806;//index in font file

                               }
                               else if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                               {
                                   returnVal = 809;
                               }
                               else /*if(affiliation == 'P' ||
                     affiliation == 'U' ||
                     affiliation == 'G' ||
                     affiliation == 'W')*/
                               {
                                   returnVal = 800;//index in font file
                               }
                           }//end default

                       }

                   }
                   catch (Exception exc)
                   {
                       ErrorLogger.LogException("UnitFontLookup", "getFillCode", exc, ErrorLevel.SEVERE);
                   }

                   return returnVal;
               }

               public static int getFrameCode(String SymbolID, int FillCode)
               {
                   int returnVal = 0;
                   char status = SymbolID.ToCharArray()[3];

                   if (status == 'A')
                       returnVal = FillCode + 2;
                   else//P, C, D, X, F
                       returnVal = FillCode + 1;

                   if (SymbolUtilities.isSubSurface(SymbolID))
                   {
                       returnVal = getSubSurfaceFrame(SymbolID, FillCode);
                   }

                   return returnVal;

               }

               private static int getSubSurfaceFill(String SymbolID)
               {
                   char affiliation = '0';
                   char status = '0';
                   int returnVal = '0';

                   returnVal = 831;

                   try
                   {
                       char[] arrID = SymbolID.ToCharArray();
                       affiliation = arrID[1];//F,H,N,U,etc...
                       status = arrID[3];//A,P,C,D,X,F

                       if (affiliation == 'F' ||
                               affiliation == 'A' ||
                               affiliation == 'D' ||
                               affiliation == 'M' ||
                               affiliation == 'J' ||
                               affiliation == 'K')
                       {
                           returnVal = 831;//
                       }
                       else if (affiliation == 'H' || affiliation == 'S')//hostile,suspect
                       {
                           returnVal = 828;//index in font file

                       }
                       else if (affiliation == 'N' || affiliation == 'L')//neutral,exercise neutral
                       {
                           returnVal = 834;
                       }
                       else if (affiliation == 'P' ||
                          affiliation == 'U' ||
                          affiliation == 'G' ||
                          affiliation == 'W')
                       {
                           returnVal = 837;//index in font file
                       }

                       //2525C/////////////////////////////////////////////////////////////////////////////////
                       /*if(SymbolID.indexOf("WM")==4 || //Sea Mine
                               SymbolID.indexOf("WDM")==4 ||//Sea Mine Decoy
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*E-----*****") ||
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*V-----*****") ||
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*X-----*****"))
                       {
                           returnVal++;


              

                           if(status == 'A')
                               returnVal++;

                       }
                       else if(SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*ND----*****"))
                       {
                           returnVal = 2121;
                       }//*/
                       //////////////////////////////////////////////////////////////////////////////////////////
                       //2525Bch2
                       if (SymbolID.IndexOf("WM") == 4)//Sea Mine
                       {
                           if (SymbolID.IndexOf("----", 6) == 6 || SymbolID.IndexOf("D---", 6) == 6)
                               returnVal = 2059;//
                           else if (SymbolID.IndexOf("G---", 6) == 6)
                               returnVal = 2062;
                           else if (SymbolID.IndexOf("GD--", 6) == 6)
                               returnVal = 2064;
                           else if (SymbolID.IndexOf("M---", 6) == 6)
                               returnVal = 2073;
                           else if (SymbolID.IndexOf("MD--", 6) == 6)
                               returnVal = 2075;
                           else if (SymbolID.IndexOf("F---", 6) == 6)
                               returnVal = 2084;
                           else if (SymbolID.IndexOf("FD--", 6) == 6)
                               returnVal = 2086;
                           else if (SymbolID.IndexOf("O---", 6) == 6 ||
                                   SymbolID.IndexOf("OD--", 6) == 6)
                               returnVal = 2094;

                       }
                       else if (SymbolID.IndexOf("WDM") == 4)//Sea Mine Decoy
                       {
                           returnVal = 2115;
                       }
                       else if (SymbolUtilities.getBasicSymbolID(SymbolID).ToUpper()==("S*U*ND----*****"))
                       {
                           returnVal = 2121;
                       }//*/
                   }
                   catch (Exception exc)
                   {
                       ErrorLogger.LogException("UnitFontLookupC", "getSubSurfaceFill", exc);
                       return 831;
                   }

                   return returnVal;
               }

               private static int getSubSurfaceFrame(String SymbolID, int fillCode)
               {
                   int returnVal = 0;

                   returnVal = 831;

                   try
                   {
                       //2525C/////////////////////////////////////////////////////////////////////////////////
                       /*if(SymbolID.indexOf("WM")==4 || //Sea Mine
                               SymbolID.indexOf("WDM")==4 ||//Sea Mine Decoy
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*E-----*****") ||
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*V-----*****") ||
                               SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*X-----*****"))
                       {
                           returnVal = -1;
                       }
                       else if(SymbolUtilities.getBasicSymbolID(SymbolID).equalsIgnoreCase("S*U*ND----*****"))
                       {
                           returnVal = -1;
                       }
                       else
                       {
                           if(SymbolID.charAt(3)=='A' || SymbolID.charAt(3)=='a')
                               return fillCode + 2;
                           else
                               return fillCode + 1;
                       }//*/
                       //////////////////////////////////////////////////////////////////////////////////////////
                       //2525Bch2

                       if (SymbolID.IndexOf("WM") == 4)//Sea Mine
                       {
                           returnVal = -1;

                       }
                       else if (SymbolID.IndexOf("WDM") == 4)//Sea Mine Decoy
                       {
                           returnVal = -1;
                       }
                       else if (SymbolUtilities.getBasicSymbolID(SymbolID).ToUpper()==("S*U*ND----*****"))
                       {
                           returnVal = -1;
                       }//*/
                       else
                       {
                           if (SymbolID.Substring(3,1).ToUpper() == "A")
                               return fillCode + 2;
                           else
                               return fillCode + 1;
                       }
                   }
                   catch (Exception exc)
                   {
                       ErrorLogger.LogException("UnitFontLookupC", "getSubSurfaceFrame", exc);
                       return fillCode;
                   }

                   return returnVal;
               }

               public UnitFontLookupInfo getLookupInfo(String SymbolID)
               {
                   try
                   {
                       String code = SymbolUtilities.getBasicSymbolID(SymbolID);
                       //        if(SymbolUtilities.isSIGINT(SymbolID))
                       //            code = code.substring(0, 10) + "--***";
                       //        else
                       code = code.Substring(0, 10) + "*****";
                       UnitFontLookupInfo data = null;
                       if (_UnitMappings.ContainsKey(code))
                           data = _UnitMappings[code];

                       return data;
                   }
                   catch (Exception exc)
                   {
                       ErrorLogger.LogException("UnitFontLookupC", "getLookupInfo()", exc, ErrorLevel.WARNING);
                       return null;
                   }
               }

               /**
                *
                * @param characterIndex - Fill Character Index
                * @return
                */
               public static double getUnitRatioHeight(int fillCharIndex)
               {
                   if (fillCharIndex == FillIndexHP ||
                           fillCharIndex == FillIndexHA ||
                           fillCharIndex == FillIndexHU ||
                           fillCharIndex == (FillIndexHU + 1) ||
                           fillCharIndex == (FillIndexHU + 2) ||
                           fillCharIndex == FillIndexUP ||
                           fillCharIndex == FillIndexUA ||
                           fillCharIndex == FillIndexUU ||
                           fillCharIndex == (FillIndexUU + 1) ||
                           fillCharIndex == (FillIndexUU + 2))
                   {
                       return 1.3;
                   }
                   else if (fillCharIndex == FillIndexHZ ||
                           fillCharIndex == FillIndexHG ||
                           fillCharIndex == FillIndexHGE ||
                           fillCharIndex == FillIndexHS ||
                           fillCharIndex == FillIndexHF ||
                           fillCharIndex == FillIndexUZ ||
                           fillCharIndex == FillIndexUG ||
                           fillCharIndex == FillIndexUGE ||
                           fillCharIndex == FillIndexUS ||
                           fillCharIndex == FillIndexUF)
                   {
                       return 1.44;
                   }
                   else if (fillCharIndex == FillIndexFGE ||
                           fillCharIndex == FillIndexFP ||
                           fillCharIndex == FillIndexFA ||
                           fillCharIndex == FillIndexFU ||
                           fillCharIndex == (FillIndexFU + 1) ||
                           fillCharIndex == (FillIndexFU + 2) ||
                           fillCharIndex == FillIndexFZ ||
                           fillCharIndex == FillIndexFS ||
                           fillCharIndex == FillIndexNP ||
                           fillCharIndex == FillIndexNA ||
                           fillCharIndex == FillIndexNU ||
                           fillCharIndex == (FillIndexNU + 1) ||
                           fillCharIndex == (FillIndexNU + 2))
                   {
                       return 1.2;
                   }
                   else if (fillCharIndex == FillIndexNZ ||
                           fillCharIndex == FillIndexNG ||
                           fillCharIndex == FillIndexNGE ||
                           fillCharIndex == FillIndexNS ||
                           fillCharIndex == FillIndexNF)
                   {
                       return 1.1;
                   }
                   else if (fillCharIndex == FillIndexFG ||
                           fillCharIndex == FillIndexFGE)
                   {
                       return 1.0;
                   }
                   else
                   {
                       return 1.2;
                   }
               }

               /**
                *
                * @param characterIndex - Fill Character Index
                * @return
                */
               public static double getUnitRatioWidth(int characterIndex)
               {
                   if (characterIndex == FillIndexUP ||
                           characterIndex == FillIndexUA ||
                           characterIndex == FillIndexUU ||
                           characterIndex == FillIndexUU + 1 ||
                           characterIndex == FillIndexUU + 2 ||
                           characterIndex == FillIndexFG ||
                           characterIndex == FillIndexFF)
                   {
                       return 1.5;
                   }
                   else if (characterIndex == FillIndexHZ ||
                           characterIndex == FillIndexHG ||
                           characterIndex == FillIndexHGE ||
                           characterIndex == FillIndexHS ||
                           characterIndex == FillIndexHF ||
                           characterIndex == FillIndexUZ ||
                           characterIndex == FillIndexUG ||
                           characterIndex == FillIndexUGE ||
                           characterIndex == FillIndexUS ||
                           characterIndex == FillIndexUF)
                   {
                       return 1.44;
                   }
                   else if (characterIndex == FillIndexFZ ||
                           characterIndex == FillIndexFGE ||
                           characterIndex == FillIndexFS)
                   {
                       return 1.2;
                   }
                   else
                   {
                       return 1.1;
                   }
               }
    }
}
