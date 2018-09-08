using System;
using System.Xml.Linq;
using System.Drawing;

namespace MilSymNetUtilities
{
    /*
    <SYMBOL>
      <SYMBOLID>S*G*UCRX--*****</SYMBOLID>
      <DESCRIPTION>Reconnaissance Long Range Surveillance (LRS)</DESCRIPTION>
      <MAPPING1U>1457</MAPPING1U>
      <MAPPING1F>1458</MAPPING1F>
      <MAPPING1N>1459</MAPPING1N>
      <MAPPING1H>1460</MAPPING1H>
      <MAPPING1COLOR/>
      <MAPPING2/>
      <MAPPING2COLOR/>
    </SYMBOL>
    */
    public class UnitFontLookupInfo
    {
        public String _SymbolID = "";
        public String _Description = "";
        public int _mapping1 = -1;
        public int _mapping1U = -1;
        public int _mapping1F = -1;
        public int _mapping1N = -1;
        public int _mapping1H = -1;
        public int _mapping2 = -1;
        public Color _color1 = Color.Black;
        public Color _color2 = Color.Transparent;//Color.Black;

        public UnitFontLookupInfo()
        {
        }

        public UnitFontLookupInfo(String SymbolID, String Description,
        int M1U, int M1F, int M1N, int M1H, Color Color1, int M2, Color Color2)
        {
            _SymbolID = SymbolID;
            _Description = Description;

            _mapping1U = M1U;
            _mapping1F = M1F;
            _mapping1N = M1N;
            _mapping1H = M1H;
            _mapping2 = M2;

            _color1 = Color1;
            _color2 = Color2;
        }

        public UnitFontLookupInfo(String SymbolID, String Description,
            String M1U, String M1F, String M1N, String M1H, String Color1,
            String M2, String Color2)
        {
            _SymbolID = SymbolID;
            _Description = Description;

            if (M1U != null && M1U!=(""))
                _mapping1U = Convert.ToInt16(M1U);
            if (M1F != null && M1F!=(""))
                _mapping1F = Convert.ToInt16(M1F);
            if (M1N != null && M1N!=(""))
                _mapping1N = Convert.ToInt16(M1N);
            if (M1H != null && M1H!=(""))
                _mapping1H = Convert.ToInt16(M1H);
            if (M2 != null && M2!=(""))
                _mapping2 = Convert.ToInt16(M2);

            Color temp = Color.Transparent;
            if (Color1 != null && Color1!=(""))
                temp = SymbolUtilities.getColorFromHexString(Color1);
            if (temp != null)
                _color1 = temp;
            if (Color2 != null && Color2!=(""))
                _color2 = SymbolUtilities.getColorFromHexString(Color2);
        }

        public String getBasicSymbolID()
        {
            return _SymbolID;
        }

        public String getDescription()
        {
            return _Description;
        }

        /**
         * gives correct mapping given the full
         * @param SymbolID
         * @return
         */
        public int getMapping1(String SymbolID)
        {
            char affiliation = SymbolID.ToCharArray()[1];
            if (affiliation == 'F' ||
                              affiliation == 'A' ||
                              affiliation == 'D' ||
                              affiliation == 'M' ||
                              affiliation == 'J' ||
                              affiliation == 'K')
                return _mapping1F;
            else if (affiliation == 'H' || affiliation == 'S')
                return _mapping1H;
            if (affiliation == 'N' || affiliation == 'L')
                return _mapping1N;
            else /*(affiliation == 'P' ||
                     affiliation == 'U' ||
                     affiliation == 'G' ||
                     affiliation == 'W')*/
                return _mapping1U;
        }

        public int getMapping2()
        {
            return _mapping2;
        }

        public Color getColor1()
        {
            return _color1;
        }

        public Color getColor2()
        {
            return _color2;
        }

        public static UnitFontLookupInfo XNodeToUnitFontLookupInfo(XNode node)
        {
            UnitFontLookupInfo ufli = null;
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
                        ufli = new UnitFontLookupInfo();
                        subElement = null;
                        if (element.FirstNode is XElement)
                            subElement = (XElement)element.FirstNode;
                        while (subElement != null)
                        {

                            if (subElement.Name == "DESCRIPTION")
                            {
                                ufli._Description = subElement.FirstNode.ToString();//get element value
                                //ufli._strDescription = ufli._strDescription.Replace("&amp;", "&");
                            }
                            else if (subElement.Name.LocalName == "SYMBOLID")
                            {
                                ufli._SymbolID = subElement.FirstNode.ToString();
                            }
                            else if (subElement.Name.LocalName == "MAPPING1U")
                            {
                                if(subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._mapping1U = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING1F")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._mapping1F = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING1N")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._mapping1N = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING1H")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._mapping1H = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING1COLOR")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._color1 = SymbolUtilities.getColorFromHexString(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING2")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._mapping2 = int.Parse(subElement.FirstNode.ToString());
                            }
                            else if (subElement.Name.LocalName == "MAPPING2COLOR")
                            {
                                if (subElement.FirstNode != null && subElement.FirstNode.ToString() != "")
                                    ufli._color2 = SymbolUtilities.getColorFromHexString(subElement.FirstNode.ToString());
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
            if (ufli.getBasicSymbolID() != null && ufli.getBasicSymbolID() != "")
                return ufli;
            else
                return null;
        }
    }
}
