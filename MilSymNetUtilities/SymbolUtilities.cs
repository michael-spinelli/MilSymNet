using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

namespace MilSymNetUtilities
{

   
    public class SymbolUtilities
    {
       
        //private static SimpleDateFormat dateFormatFront = new SimpleDateFormat("ddHHmmss");
        //private static SimpleDateFormat dateFormatBack = new SimpleDateFormat("MMMyy");
        //private static SimpleDateFormat dateFormatFull = new SimpleDateFormat("ddHHmmssZMMMyy");
        //private static SimpleDateFormat dateFormatZulu = new SimpleDateFormat("Z");

        /**
         * @name getBasicSymbolID
         *
         * @desc Returns a formatted string that has only the necessary static
         * characters needed to draw a symbol. For instance
         * GetBasicSymbolID("GFTPGLB----K---") returns "G*T*GLB---****X"
         *
         * @param strSymbolID - IN - A 15 character MilStd code
         * @return A properly formated basic symbol ID
         */
        public static String getBasicSymbolID(String strSymbolID)
        {

            StringBuilder sb = new StringBuilder();
            if ((strSymbolID != null) && (strSymbolID.Length == 15))
            {
                // Check to make sure it is a tactical graphic symbol.
                if ((isWeather(strSymbolID)) || (isBasicShape(strSymbolID)))
                {
                    return strSymbolID;
                }
                else if (isTacticalGraphic(strSymbolID) == true)
                {
                    sb.Append(strSymbolID[0]);
                    sb.Append("*");
                    sb.Append(strSymbolID[2]);
                    sb.Append("*");
                    sb.Append(strSymbolID.Substring(4, 6));
                    sb.Append("****");
                    sb.Append("X");

                    if (isEMSNaturalEvent(strSymbolID) == true)
                    {
                        sb.Remove(14, 1).Append("*");
                    }

                    return sb.ToString();
                }
                else if (isWarfighting(strSymbolID))
                {

                    sb.Append(strSymbolID[0]);
                    sb.Append("*");
                    sb.Append(strSymbolID[2]);
                    sb.Append("*");
                    sb.Append(strSymbolID.Substring(4, 6));

                    if (isSIGINT(strSymbolID))
                        sb.Append("--***");
                    else if (isInstallation(strSymbolID))
                        sb.Append("H****");
                    else
                    {
                        sb.Append("*****");
                        UnitDefTable udt = UnitDefTable.getInstance();
                        String temp = sb.ToString();
                        for (int i = 0; i < 2; i++)
                        {
                            if (udt.HasUnitDef(temp) == true)
                            {
                                return temp;
                            }
                            else
                            {
                                temp = temp.Substring(0, 10) + "H****";
                                if (udt.HasUnitDef(temp) == true)
                                {
                                    return temp;
                                }
                                else
                                {
                                    temp = temp.Substring(0, 10) + "MO***";
                                    if (udt.HasUnitDef(temp) == true)
                                    {
                                        return temp;
                                    }
                                }
                            }
                            temp = temp.Substring(0, 10) + "*****";
                        }

                    }

                    return sb.ToString();
                }
                else // Don't do anything for bridge symbols
                {
                    return strSymbolID;
                }
            }
            else
            {
                return strSymbolID;
            }
        }

        /**
         * Only for renderer use.  Please use getBasicSymbolID.
         * @param strSymbolID
         * @return
         */
        public static String getBasicSymbolIDStrict(String strSymbolID)
        {
            StringBuilder sb = new StringBuilder();
            char scheme = strSymbolID[0];
            if (strSymbolID != null && strSymbolID.Length == 15)
            {
                if (scheme == 'G')
                {
                    sb.Append(strSymbolID[0]);
                    sb.Append("*");
                    sb.Append(strSymbolID[2]);
                    sb.Append("*");
                    sb.Append(strSymbolID.Substring(4, 6));
                    sb.Append("****X");
                }
                else if (scheme != 'W' && scheme != 'B')
                {
                    sb.Append(strSymbolID[0]);
                    sb.Append("*");
                    sb.Append(strSymbolID[2]);
                    sb.Append("*");
                    sb.Append(strSymbolID.Substring(4, 6));
                    sb.Append("*****");
                }
                else
                {
                    return strSymbolID;
                }
                return sb.ToString();
            }
            return strSymbolID;
        }


        /**
         * Attempts to clean up any potentially malformed symbolIDs
         * @param value SymbolID
         * @return
         */
        /**
         * Attempts to clean up any potentially malformed symbolIDs
         * @param value SymbolID
         * @return
         */
        public static String reconcileSymbolID(String symbolID)
        {
            return reconcileSymbolID(symbolID, false);
        }

        public static String reconcileSymbolID(String symbolID, Boolean isMultiPoint)
        {
            StringBuilder sb = new StringBuilder("");
            char codingScheme = symbolID[0];

            if (symbolID.StartsWith("BS_") || symbolID.StartsWith("BBS_"))
            {
                return symbolID;
            }

            if (symbolID.Length < 15)
            {
                while (symbolID.Length < 15)
                {
                    symbolID += "-";
                }
            }
            if (symbolID.Length > 15)
            {
                symbolID = symbolID.Substring(0, 15);
            }

            if (symbolID != null && symbolID.Length == 15)
            {
                if (codingScheme == 'S' || //warfighting
                        codingScheme == 'I' ||//sigint
                        codingScheme == 'O' ||//stability operation
                        codingScheme == 'E')//emergency management
                {
                    sb.Append(codingScheme);

                    if (SymbolUtilities.hasValidAffiliation(symbolID) == false)
                    {
                        sb.Append('U');
                    }
                    else
                    {
                        sb.Append(symbolID[1]);
                    }

                    if (SymbolUtilities.hasValidBattleDimension(symbolID) == false)
                    {
                        sb.Append('Z');
                        sb.Remove(0, 1);
                        sb.Insert(0, "S");
                    }
                    else
                    {
                        sb.Append(symbolID[2]);
                    }

                    if (SymbolUtilities.hasValidStatus(symbolID) == false)
                    {
                        sb.Append('P');
                    }
                    else
                    {
                        sb.Append(symbolID[3]);
                    }

                    sb.Append("------");
                    sb.Append(symbolID.Substring(10, 5));

                }
                else if (codingScheme == 'G')//tactical
                {
                    sb.Append(codingScheme);

                    if (SymbolUtilities.hasValidAffiliation(symbolID) == false)
                    {
                        sb.Append('U');
                    }
                    else
                    {
                        sb.Append(symbolID[1]);
                    }

                    //if(SymbolUtilities.hasValidBattleDimension(SymbolID)==false)
                    sb.Append('G');
                    //else
                    //    sb.append(SymbolID.charAt(2));

                    if (SymbolUtilities.hasValidStatus(symbolID) == false)
                    {
                        sb.Append('P');
                    }
                    else
                    {
                        sb.Append(symbolID[3]);
                    }

                    if (isMultiPoint)
                    {
                        sb.Append("GAG---");//return a boundary
                    }
                    else
                    {
                        sb.Append("GPP---");//return an action point
                    }
                    sb.Append(symbolID.Substring(10, 5));

                }
                else if (codingScheme == 'W')//weather
                {//no default weather graphic
                    return "SUZP-----------";//unknown
                }
                else//bad codingScheme
                {
                    sb.Append('S');
                    if (SymbolUtilities.hasValidAffiliation(symbolID) == false)
                    {
                        sb.Append('U');
                    }
                    else
                    {
                        sb.Append(symbolID[1]);
                    }

                    if (SymbolUtilities.hasValidBattleDimension(symbolID) == false)
                    {
                        sb.Append('Z');
                        //sb.replace(0, 1, "S");
                    }
                    else
                    {
                        sb.Append(symbolID[2]);
                    }

                    if (SymbolUtilities.hasValidStatus(symbolID) == false)
                    {
                        sb.Append('P');
                    }
                    else
                    {
                        sb.Append(symbolID[3]);
                    }

                    sb.Append("------");
                    sb.Append(symbolID.Substring(10, 5));
                }
            }
            else
            {
                return "SUZP-----------";//unknown
            }
            return sb.ToString();
        }

        /**
         * Returns true if the SymbolID has a valid Status (4th character)
         * @param SymbolID
         * @return
         */
        public static Boolean hasValidStatus(String SymbolID)
        {
            if (SymbolID != null && SymbolID.Length == 15)
            {
                char status = SymbolID.ToCharArray()[3];

                char codingScheme = SymbolID.ToCharArray()[0];

                if (codingScheme == 'S' || //warfighting
                     codingScheme == 'I' ||//sigint
                     codingScheme == 'O' ||//stability operation
                     codingScheme == 'E')//emergency management
                {
                    if (status == 'A' ||
                        status == 'P' ||
                        status == 'C' ||
                        status == 'D' ||
                        status == 'X' ||
                        status == 'F')
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else if (codingScheme == 'G')
                {
                    if (status == 'A' ||
                        status == 'S' ||
                        status == 'P' ||
                        status == 'K')
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else if (codingScheme == 'W')
                {
                    return true;//doesn't apply
                }

                return false;
            }
            else
                return false;
        }

        /**
         * Returns true if the SymbolID has a valid Affiliation (2nd character)
         * @param SymbolID
         * @return
         */
        public static Boolean hasValidAffiliation(String SymbolID)
        {
            if (SymbolID != null && SymbolID.Length == 15)
            {
                char affiliation = SymbolID.ToCharArray()[1];
                if (affiliation == 'P' ||
                        affiliation == 'U' ||
                        affiliation == 'A' ||
                        affiliation == 'F' ||
                        affiliation == 'N' ||
                        affiliation == 'S' ||
                        affiliation == 'H' ||
                        affiliation == 'G' ||
                        affiliation == 'W' ||
                        affiliation == 'M' ||
                        affiliation == 'D' ||
                        affiliation == 'L' ||
                        affiliation == 'J' ||
                        affiliation == 'K')
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /**
         * Returns true if the SymbolID has a valid BattleDimension (3rd character)
         * "Category" for tactical graphics
         * @param SymbolID 15 character String
         * @return
         */
        public static Boolean hasValidBattleDimension(String SymbolID)
        {
            if (SymbolID != null && SymbolID.Length == 15)
            {
                char codingScheme = SymbolID.ToCharArray()[0];
                char bd = SymbolID.ToCharArray()[2];

                if (codingScheme == 'S')//warfighting
                {
                    if (bd == 'P' ||
                        bd == 'A' ||
                        bd == 'G' ||
                        bd == 'S' ||
                        bd == 'U' ||
                        bd == 'F' ||
                        //status == 'X' ||//doesn't seem to be a valid use for this one
                        bd == 'Z')
                        return true;
                    else
                        return false;
                }
                else if (codingScheme == 'O')//stability operation
                {
                    if (bd == 'V' ||
                        bd == 'L' ||
                        bd == 'O' ||
                        bd == 'I' ||
                        bd == 'P' ||
                        bd == 'G' ||
                        bd == 'R')
                        return true;
                    else
                        return false;
                }
                else if (codingScheme == 'E')//emergency management
                {
                    if (bd == 'I' ||
                        bd == 'N' ||
                        bd == 'O' ||
                        bd == 'F')
                        return true;
                    else
                        return false;
                }
                else if (codingScheme == 'G')//tactical grahpic
                {
                    if (bd == 'T' ||
                        bd == 'G' ||
                        bd == 'M' ||
                        bd == 'F' ||
                        bd == 'S' ||
                        bd == 'O')
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else if (codingScheme == 'W')//weather
                {
                    return true;//doesn't apply
                }
                else if (codingScheme == 'I')//sigint
                {
                    if (bd == 'P' ||
                        bd == 'A' ||
                        bd == 'G' ||
                        bd == 'S' ||
                        bd == 'U' ||
                        //status == 'X' ||//doesn't seem to be a valid use for this one
                        bd == 'Z')
                        return true;
                    else
                        return false;
                }
                else//bad codingScheme, can't confirm battle dimension
                    return false;
            }
            else
                return false;
        }

        public static Boolean hasValidCountryCode(String symbolID)
        {
            if (Char.IsLetter(symbolID[12])
                    && Char.IsLetter(symbolID[13]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /**
         * converts a Java Date object into a properly formated String for
         * W or W1
         * @param time
         * @return
         */
        /*
        public static String getDateLabel(Date time)
        {

            String modifierString = null;

            modifierString = dateFormatZulu.format(time);
            if (modifierString==("+0000"))
            {
                modifierString = dateFormatFront.format(time) + "Z" + dateFormatBack.format(time);
            }
            else
            {
                modifierString = dateFormatFront.format(time) + "L" + dateFormatBack.format(time);
            }

            return modifierString.ToUpper();
        }//*/


        /**
 * Given date, return character String representing
 * which NATO time zone you're in.
 * @param hour
 * @return 
 */
        /*
        private static String getZuluCharFromTimeZoneOffset(Date time)
        {
            TimeZone tz = TimeZone.getDefault();
            Date offset = new Date(tz.getOffset(time.getTime()));
            long lOffset = offset.getTime() / 3600000;//3600000 = (1000(ms)*60(s)*60(m))

            int hour = (int)lOffset;

            return getZuluCharFromTimeZoneOffset(hour);
        }
         */

        /**
         * Given hour offset from Zulu return character String representing
         * which NATO time zone you're in.
         * @param hour
         * @return 
         */
        /*
        private static String getZuluCharFromTimeZoneOffset(int hour)
        {
            if (hour == 0)
                return "Z";
            else if (hour == -1)
                return "N";
            else if (hour == -2)
                return "O";
            else if (hour == -3)
                return "P";
            else if (hour == -4)
                return "Q";
            else if (hour == -5)
                return "R";
            else if (hour == -6)
                return "S";
            else if (hour == -7)
                return "T";
            else if (hour == -8)
                return "U";
            else if (hour == -9)
                return "V";
            else if (hour == -10)
                return "W";
            else if (hour == -11)
                return "X";
            else if (hour == -12)
                return "Y";
            else if (hour == 1)
                return "A";
            else if (hour == 2)
                return "B";
            else if (hour == 3)
                return "C";
            else if (hour == 4)
                return "D";
            else if (hour == 5)
                return "E";
            else if (hour == 6)
                return "F";
            else if (hour == 7)
                return "G";
            else if (hour == 8)
                return "H";
            else if (hour == 9)
                return "I";
            else if (hour == 10)
                return "K";
            else if (hour == 11)
                return "L";
            else if (hour == 12)
                return "M";
            else
                return "-";
        }
       

        */


        /**
      * 
      * @param symbolID
      * @param unitModifier
      * @return 
      */
        public static Boolean canUnitHaveModifier(String symbolID, int unitModifier)
        {
            Boolean returnVal = false;
            try
            {
                if (unitModifier == (ModifiersUnits.B_ECHELON))
                {
                    return (SymbolUtilities.isUnit(symbolID) || SymbolUtilities.isSTBOPS(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.C_QUANTITY))
                {
                    return (SymbolUtilities.isEquipment(symbolID) ||
                            SymbolUtilities.isEMSEquipment(symbolID) ||
                            SymbolUtilities.isEMSIncident(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.D_TASK_FORCE_INDICATOR))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isSTBOPS(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.F_REINFORCED_REDUCED))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isSTBOPS(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.G_STAFF_COMMENTS))
                {
                    return (SymbolUtilities.isEMS(symbolID) == false);
                }
                else if (unitModifier == (ModifiersUnits.H_ADDITIONAL_INFO_1))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.J_EVALUATION_RATING))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.K_COMBAT_EFFECTIVENESS))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isSTBOPS(symbolID) ||
                            (SymbolUtilities.hasInstallationModifier(symbolID) && SymbolUtilities.isEMS(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.L_SIGNATURE_EQUIP))
                {
                    return (SymbolUtilities.isEquipment(symbolID) ||
                            SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.M_HIGHER_FORMATION))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.N_HOSTILE))
                {
                    return (SymbolUtilities.isEquipment(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.P_IFF_SIF))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isEquipment(symbolID) ||
                            (SymbolUtilities.hasInstallationModifier(symbolID) && SymbolUtilities.isEMS(symbolID) == false) ||
                            SymbolUtilities.isSTBOPS(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.Q_DIRECTION_OF_MOVEMENT))
                {
                    return ((SymbolUtilities.hasInstallationModifier(symbolID) == false) &&
                            (SymbolUtilities.isSIGINT(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.R_MOBILITY_INDICATOR))
                {
                    return (SymbolUtilities.isEquipment(symbolID) ||
                            SymbolUtilities.isEMSEquipment(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.R2_SIGNIT_MOBILITY_INDICATOR))
                {
                    return (SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.S_HQ_STAFF_OR_OFFSET_INDICATOR))
                {
                    return (SymbolUtilities.isSIGINT(symbolID) == false);
                }
                else if (unitModifier == (ModifiersUnits.T_UNIQUE_DESIGNATION_1))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.V_EQUIP_TYPE))
                {
                    return (SymbolUtilities.isEquipment(symbolID) ||
                            SymbolUtilities.isSIGINT(symbolID) ||
                            SymbolUtilities.isEMSEquipment(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.W_DTG_1))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.X_ALTITUDE_DEPTH))
                {
                    return (SymbolUtilities.isSIGINT(symbolID) == false);
                }
                else if (unitModifier == (ModifiersUnits.Y_LOCATION))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.Z_SPEED))
                {
                    return ((SymbolUtilities.hasInstallationModifier(symbolID) == false) &&
                            (SymbolUtilities.isSIGINT(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.AA_SPECIAL_C2_HQ))
                {
                    return (SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.isSTBOPS(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.AB_FEINT_DUMMY_INDICATOR))
                {
                    return ((SymbolUtilities.isSIGINT(symbolID) == false) &&
                            (SymbolUtilities.isEMS(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.AC_INSTALLATION))
                {
                    return (SymbolUtilities.isSIGINT(symbolID) == false);
                }
                else if (unitModifier == (ModifiersUnits.AD_PLATFORM_TYPE))
                {
                    return (SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.AE_EQUIPMENT_TEARDOWN_TIME))
                {
                    return (SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.AF_COMMON_IDENTIFIER))
                {
                    return (SymbolUtilities.isSIGINT(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.AG_AUX_EQUIP_INDICATOR))
                {
                    return (SymbolUtilities.isEquipment(symbolID));
                }
                else if (unitModifier == (ModifiersUnits.AH_AREA_OF_UNCERTAINTY) ||
                        unitModifier == (ModifiersUnits.AI_DEAD_RECKONING_TRAILER) ||
                        unitModifier == (ModifiersUnits.AJ_SPEED_LEADER))
                {
                    return ((SymbolUtilities.isSIGINT(symbolID) == false) &&
                            (SymbolUtilities.hasInstallationModifier(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.AK_PAIRING_LINE))
                {
                    return ((SymbolUtilities.isSIGINT(symbolID) == false) &&
                            (SymbolUtilities.isEMS(symbolID) == false) &&
                            (SymbolUtilities.hasInstallationModifier(symbolID) == false));
                }
                else if (unitModifier == (ModifiersUnits.AL_OPERATIONAL_CONDITION))
                {
                    return (SymbolUtilities.isUnit(symbolID) == false);
                }
                else if (unitModifier == (ModifiersUnits.AO_ENGAGEMENT_BAR))
                {
                    return ((SymbolUtilities.isEquipment(symbolID) ||
                            SymbolUtilities.isUnit(symbolID) ||
                            SymbolUtilities.hasInstallationModifier(symbolID)) &&
                            SymbolUtilities.isEMS(symbolID) == false);
                }
                //out of order because used less often
                else if (unitModifier == (ModifiersUnits.A_SYMBOL_ICON))
                {
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.E_FRAME_SHAPE_MODIFIER))
                {
                    //return (SymbolUtilities.isSIGINT(symbolID)==false);
                    //not sure why milstd say sigint don't have it.
                    //they clearly do.
                    return true;
                }
                else if (unitModifier == (ModifiersUnits.SCC_SONAR_CLASSIFICATION_CONFIDENCE))
                {
                    if (SymbolUtilities.isSubSurface(symbolID))
                    {
                        //these symbols only exist in 2525C
                        String temp = symbolID.Substring(4, 6);
                        if (temp.Equals("WMGC--") ||
                                temp.Equals("WMMC--") ||
                                temp.Equals("WMFC--") ||
                                temp.Equals("WMC---"))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else
                    return false;


            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolUtilities", "canHaveModifier", exc);
            }
            return returnVal;
        }



        /**
         *
         * @param symbolID
         * @param modifier - from the constants ModifiersUnits or ModifiersTG
         * @param symStd - 0=2525B, 1=2525C. Constants available in
         * RendererSettings.
         * @return
         */
        public static Boolean hasModifier(String symbolID, int modifier, int symStd)
        {
            Boolean returnVal = false;

            if (isTacticalGraphic(symbolID) == true)
            {
                returnVal = canSymbolHaveModifier(symbolID, modifier, symStd);
            }
            else
            {
                returnVal = canUnitHaveModifier(symbolID, modifier);
            }
            return returnVal;
        }


        /**
         * Checks if a tactical graphic has the passed modifier.
         *
         * @param symbolID - symbolID of Tactical Graphic
         * @param tgModifier - ModifiersTG.AN_AZIMUTH
         * @param symStd - like MilStd.Symbology_2525C
         * @return
         */
        public static Boolean canSymbolHaveModifier(String symbolID, int tgModifier, int symStd)
        {
            String basic = null;
            SymbolDef sd = null;
            Boolean returnVal = false;
            String modCode = ModifiersTG.getModifierLetterCode(tgModifier);
            try
            {

                basic = SymbolUtilities.getBasicSymbolID(symbolID);
                sd = SymbolDefTable.getInstance().getSymbolDef(basic, symStd);
                if (sd != null)
                {
                    int dc = sd.getDrawCategory();
                    if (tgModifier == (ModifiersTG.AM_DISTANCE))
                    {
                        switch (dc)
                        {
                            case SymbolDef.DRAW_CATEGORY_RECTANGULAR_PARAMETERED_AUTOSHAPE:
                            case SymbolDef.DRAW_CATEGORY_SECTOR_PARAMETERED_AUTOSHAPE:
                            case SymbolDef.DRAW_CATEGORY_TWO_POINT_RECT_PARAMETERED_AUTOSHAPE:
                                returnVal = true;
                                break;
                            case SymbolDef.DRAW_CATEGORY_CIRCULAR_PARAMETERED_AUTOSHAPE:
                            case SymbolDef.DRAW_CATEGORY_CIRCULAR_RANGEFAN_AUTOSHAPE:
                                returnVal = true;
                                break;
                            case SymbolDef.DRAW_CATEGORY_LINE://air corridor
                                if (sd.getModifiers().Contains(modCode + "."))
                                    returnVal = true;
                                break;
                            default:
                                returnVal = false;
                                break;
                        }
                    }
                    else if (tgModifier == (ModifiersTG.AN_AZIMUTH))
                    {
                        switch (dc)
                        {
                            case SymbolDef.DRAW_CATEGORY_RECTANGULAR_PARAMETERED_AUTOSHAPE:
                            case SymbolDef.DRAW_CATEGORY_SECTOR_PARAMETERED_AUTOSHAPE:
                                returnVal = true;
                                break;
                            default:
                                returnVal = false;
                                break;
                        }
                    }
                    else
                    {
                        if (sd.getModifiers().Contains(modCode + "."))
                        {
                            returnVal = true;
                        }
                    }
                }

                return returnVal;

            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolUtilties", "canSymbolHaveModifier", exc);
            }
            return returnVal;
        }

      
 


        public static Boolean isBasicShape(String symbolID)
        {
            if (symbolID != null && symbolID.Length >= 2)
            {
                if (symbolID.StartsWith("BS_") || symbolID.StartsWith("BBS_"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /**
         * Determines if the symbol is a tactical graphic
         * @param strSymbolID
         * @return true if symbol starts with "G", or is a weather graphic, or a bridge graphic
         */
        public static Boolean isTacticalGraphic(String strSymbolID)
          {
            try
            {
              if(strSymbolID == null) // Error handling
              {
                return false;
              }
              if ((strSymbolID.Substring(0, 1) == ("G")) || (isWeather(strSymbolID)) || (isEMSNaturalEvent(strSymbolID)))
              {
                return true;
              }
            }
            catch(Exception exc)
            {
              Debug.WriteLine(exc.StackTrace);
            }
            return false;
          }


        /**
         * Determins if symbols is a warfighting symbol.
         * @param strSymbolID
         * @return True if code starts with "O", "S", or "I". (or "E" in 2525C)
         */
        public static Boolean isWarfighting(String strSymbolID)
          {
            try
            {
              if(strSymbolID == null) // Error handling
              {
                return false;
              }
              if((strSymbolID.Substring(0, 1)==("O")) || (strSymbolID.Substring(0, 1)==("S")) ||
                      (strSymbolID.Substring(0, 1)==("I")) || (strSymbolID.Substring(0, 1)==("E")))
              {
                return true;
              }
            }
            catch(Exception exc)
            {
              Debug.WriteLine(exc.StackTrace);
            }
            return false;
          }



        /**
        * Determines if the symbol is a weather graphic
        * @param strSymbolID
        * @return true if symbolID starts with a "W"
        */
        public static Boolean isWeather(String strSymbolID)      
        {
           
            try
            {
              Boolean blRetVal = strSymbolID.Substring(0, 1)==("W");
              return blRetVal;
            }
            catch(Exception exc)
            {
              Debug.WriteLine(exc.StackTrace);
            }
            return false; 
        }



        /**
         * Determines if a String represents a valid number
         * @param text
         * @return "1.56" == true, "1ab" == false
         */
        public static Boolean isNumber(String text)
        {
            String pattern = "((-|\\+)?[0-9]+(\\.[0-9]+)?)+";
            if (System.Text.RegularExpressions.Regex.IsMatch(text, pattern))
                return true;
            else
                return false;
        }







        public static Boolean isCheckPoint(String strSymbolID)
        {
            try
            {
                String strBasicSymbolID = getBasicSymbolIDStrict(strSymbolID);
                Boolean blRetVal = false;
                if (strBasicSymbolID == ("G*G*GPPE--****X")//release point
                  || strBasicSymbolID == ("G*G*GPPK--****X")//check point
                  || strBasicSymbolID == ("G*G*GPPS--****X"))//start point
                {
                    blRetVal = true;
                }
                return blRetVal;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsCheckPoint

        /**
         * @name IsCriticalPoint
         *
         * @desc Returns true if the symbolID is a critical point.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is a
         * critical point
         * @return True if the graphic is a critical point, false otherwise.
         */
        public static Boolean isCriticalPoint(String strSymbolID)
        {
            try
            {
                String strBasicSymbolID = getBasicSymbolIDStrict(strSymbolID);
                Boolean blRetVal = false;
                if (isTacticalGraphic(strBasicSymbolID))
                {
                    String[] arr = new String[] { "G*M*BDD---****X",
                                                "G*M*BDE---****X",
                                                "G*M*BDI---****X",
                                                "G*R*CN----****X",
                                                "G*R*CP----****X",
                                                "G*R*FD----****X",
                                                "G*R*FR----****X",
                                                "G*R*PCC---****X",
                                                "G*R*PCO---****X",
                                                "G*R*PDC---****X",
                                                "G*R*PHP---****X",
                                                "G*R*PMC---****X",
                                                "G*R*PO----****X",
                                                "G*R*PPO---****X",
                                                "G*R*PTO---****X",
                                                "G*R*RLGC--****X",
                                                "G*R*SG----****X",
                                                "G*R*SSC---****X",
                                                "G*R*SC----****X",
                                                "G*R*TN----****X",
                                                "G*R*UP----****X" };
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] == (strBasicSymbolID))
                        {
                            blRetVal = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (strBasicSymbolID == ("O*E*AL---------")
                        || strBasicSymbolID == ("O*E*AM---------")
                        || strBasicSymbolID == ("S*G*IMNB-------"))
                    {
                        blRetVal = true;
                    }
                }
                return blRetVal;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        }

        /**
         * @name IsRoute
         *
         * @desc Returns true if the symbolID is a route.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is a route
         * @return True if the graphic is a route, false otherwise.
         */
        public static Boolean isRoute(String strSymbolID)
        {
            try
            {
                String strBasicSymbolID = getBasicSymbolIDStrict(strSymbolID);
                Boolean blRetVal = false;
                if (strBasicSymbolID == ("G*S*LRA---****X") || strBasicSymbolID == ("G*S*LRM---****X"))
                {
                    blRetVal = true;
                }
                return blRetVal;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsRoute



        /**
         * @name IsMOOTW
         *
         * @desc Returns true if the symbolID is a MOOTW symbol.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is a MOOTW
         * graphic
         * @return True if the graphic is a MOOTW symbol in the MIL-STD 2525B or
         * STBOPS in 2525C, false otherwise.
         */
        public static Boolean isMOOTW(String strSymbolID)
        {
            try
            {
                if (strSymbolID.Substring(0, 1) == ("O"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsMOOTW

        /**
       * @name isSTBOPS
       *
       * @desc Returns true if the symbolID is a Stability Operations (STBOPS) symbol.
       *
       * @param strSymbolID - IN - Symbol Id we are checking to see if it is a 
       * isStabilityOperations graphic
       * @return True if the graphic is a MOOTW symbol in the MIL-STD 2525B or
       * STBOPS in 2525C, false otherwise.
       */
        public static Boolean isSTBOPS(String strSymbolID)
        {
            try
            {
                if (strSymbolID.Substring(0, 1) == ("O"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception t)
            {
                Debug.WriteLine(t);
            }
            return false;
        } // End isStabilityOperations



        /**
         * @name isHQ
         *
         * @desc Determines if the symbol id passed in contains a flag for one of the
         * various HQ options Pos 11 of the symbol code
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is a HQ
         * @return True if the graphic is a HQ symbol in the MIL-STD 2525B, false
         * otherwise.
         */
        public static Boolean isHQ(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID.Substring(10, 1)==("A"))
                || (strSymbolID.Substring(10, 1)==("B"))
                || (strSymbolID.Substring(10, 1)==("C")) || (strSymbolID.Substring(10, 1)==("D")));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isHQ

        /**
         * @name isTaskForce
         *
         * @desc Returns whether or not the given symbol id contains task force.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it contains
         * task force
         * @return Returns true if the symbol id contains task force, false otherwise.
         */
       
        public static Boolean isTaskForce(String strSymbolID)
        {
            try
            {
              // Return whether or not task force is included in the symbol id.
              Boolean blRetVal = ((strSymbolID.Substring(10, 1)==("B"))
                || (strSymbolID.Substring(10, 1)==("D"))
                || (strSymbolID.Substring(10, 1)==("E")) || (strSymbolID.Substring(10, 1)==("G")));
              return blRetVal;
            }
            catch(Exception exc)
            {
              Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsTaskForce

        /**
         * @name isFeintDummy
         *
         * @desc Returns whether or not the given symbol id contains FeintDummy.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it contains
         * feint dummy
         * @return Returns true if the symbol id contains FeintDummy, false otherwise.
         */
        public static Boolean isFeintDummy(String strSymbolID)
        {
            try
            {
                // Return whether or not feintdummy is included in the symbol id.
                Boolean blRetVal = ((strSymbolID.Substring(10, 1)==("C"))
                || (strSymbolID.Substring(10, 1)==("D"))
                || (strSymbolID.Substring(10, 1)==("F")) || (strSymbolID.Substring(10, 1)==("G")));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsFeintDummy


        /**
         * Symbol has a mobility modifier
         * @param strSymbolID
         * @return
         */
        public static Boolean isMobility(String strSymbolID)
        {
            Boolean mobilityIsOn = false;
            try
            {

                //if(isEquipment(strSymbolID))
                //{
                    if(strSymbolID.Substring(10, 2)==("MO") ||
                            strSymbolID.Substring(10, 2)==("MP") ||
                            strSymbolID.Substring(10, 2)==("MQ") ||
                            strSymbolID.Substring(10, 2)==("MR") ||
                            strSymbolID.Substring(10, 2)==("MS") ||
                            strSymbolID.Substring(10, 2)==("MT") ||
                            strSymbolID.Substring(10, 2)==("MU") ||
                            strSymbolID.Substring(10, 2)==("MV") ||
                            strSymbolID.Substring(10, 2)==("MW") ||
                            strSymbolID.Substring(10, 2)==("MX") ||
                            strSymbolID.Substring(10, 2)==("MY") ||
                            strSymbolID.Substring(10, 2)==("NS") ||
                            strSymbolID.Substring(10, 2)==("NL"))
                    {
                        mobilityIsOn = true;
                    }
                //}
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            // Return whether or not the mobility wheeled modifier is on.
            return mobilityIsOn;
        }

        /**
         * @name isObstacle
         *
         * @desc Returns true if the symbol id passed in is an Obstacle symbol code.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is an
         * Obstacle
         * @return True if the graphic is an Obstacle in the MIL-STD 2525B, false
         * otherwise.
         */

        /**
      * Returns true if Symbol is a Target
      * @param strSymbolID
      * @return 
      */
        public static Boolean isTarget(String strSymbolID)
        {
            String basicID = SymbolUtilities.getBasicSymbolID(strSymbolID);
            if (basicID.Substring(0, 6) == ("G*F*PT") ||//fire support/point/point target
                    basicID.Substring(0, 6) == ("G*F*LT") ||//fire support/lines/linear target
                    basicID.Substring(0, 6) == ("G*F*AT"))//fire support/area/area target
            {
                return true;
            }
            else
            {
                return false;
            }
        }
 
      
        /**
          * Returns true if Symbol is an Air Track
          * @param strSymbolID
          * @return 
          */
        public static Boolean isAirTrack(String strSymbolID)
        {
            if (strSymbolID.ElementAt(0) == 'S' &&
                    strSymbolID.ElementAt(2) == 'A')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        public static Boolean isObstacle(String strSymbolID)
        {
            try
            {
                // An Obstacle is denoted by the symbol code "G*M*O"
                // So see if it is a tactical graphic then check to see
                // if we have the M and then the O in the correct position.
                Boolean blRetVal = ((isTacticalGraphic(strSymbolID)) && ((strSymbolID.Substring(2, 1)==("M")) && (strSymbolID.Substring(4, 5)==("O"))));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isObstacle

        /**
         * @name isDeconPoint
         *
         * @desc Returns true if the symbol id is a DECON (NBC graphic) point symbol.
         *
         * @param strSymbolID - IN - Symbol Id we are checking to see if it is a Decon
         * Point
         * @return True if the graphic is a Decon Point in the MIL-STD 2525B, false
         * otherwise.
         */
        public static Boolean isDeconPoint(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((isNBC(strSymbolID)) && (strSymbolID.Substring(4, 2)==("ND")));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isDeconPoint


        /**
       * Reads the Symbol ID string and returns the text that represents the echelon
         * code.
       * @param echelon
       * @return
       */
        public static String getEchelonText(String echelon)
        {
            char[] dots = new char[3];
            dots[0] = (char)8226;
            dots[1] = (char)8226;
            dots[2] = (char)8226;
            String dot = new String(dots);
            String text = null;
            if (echelon==("A"))
            {
                text = "0";
            }
            else if (echelon==("B"))
            {
                text = dot.Substring(0, 1);
            }
            else if (echelon == ("C"))
            {
                text = dot.Substring(0, 2);
            }
            else if (echelon == ("D"))
            {
                text = dot;
            }
            else if (echelon == ("E"))
            {
                text = "|";
            }
            else if (echelon == ("F"))
            {
                text = "| |";
            }
            else if (echelon == ("G"))
            {
                text = "| | |";
            }
            else if (echelon == ("H"))
            {
                text = "X";
            }
            else if (echelon == ("I"))
            {
                text = "XX";
            }
            else if (echelon == ("J"))
            {
                text = "XXX";
            }
            else if (echelon == ("K"))
            {
                text = "XXXX";
            }
            else if (echelon == ("L"))
            {
                text = "XXXXX";
            }
            else if (echelon == ("M"))
            {
                text = "XXXXXX";
            }
            else if (echelon == ("N"))
            {
                text = "+ +";
            }
            return text;
        }

        /**
         * @name isUnit
         *
         * @desc Returns true if the symbolID is a unit.
         *
         * @param strSymbolID - IN - SymbolID we are checking on
         * @return True if the graphic is a unit in the MIL-STD 2525B or is a special
         * operation forces unit, false otherwise.
         */
        public static Boolean isUnit(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID.Substring(0, 1)==("S")) && 
                                    (strSymbolID.Substring(2, 1)==("G")) &&
                                    (strSymbolID.Substring(4,1)==("U")));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isUnit

        /**
         * @name isNBC
         *
         * @desc Returns true if the symbol id passed in is a NBC symbol code.
         *
         * @param strSymbolID - IN - SymbolID we are checking on
         * @return True if the graphic is a NBC in the MIL-STD 2525B, false otherwise.
         */
        public static Boolean isNBC(String strSymbolID)
        {
            try
            {
                String temp = getBasicSymbolID(strSymbolID);
                Boolean blRetVal = ((isTacticalGraphic(strSymbolID)) && (temp.Substring(0, 5)==("G*M*N")));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isNBC

        /**
         * returns true if the symbol code
         * represents a symbol that has control points
         */
        public static Boolean isTGWithControlPoints(String strSymbolID, int symStd)
        {
            String temp = getBasicSymbolID(strSymbolID);
            SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(temp, symStd);

            if (sd != null && sd.getDrawCategory() == SymbolDef.DRAW_CATEGORY_ROUTE)
                return true;
            else
                return false;//blRetVal;
        }

        /**
         * There's a handful of single point tactical graphics with unique
         * modifier positions.
         * @param strSymbolID
         * @return
         */
        public static Boolean isTGSPWithSpecialModifierLayout(String strSymbolID)
        {
            String temp = getBasicSymbolIDStrict(strSymbolID);

            Boolean blRetVal = (temp.Equals("G*G*GPH---****X"))//Harbor(General) - center
                    || (temp.Equals("G*G*GPPC--****X")) //Contact Point - center
                    || (temp.Equals("G*G*GPPD--****X"))//Decisions Point - center
                    || (temp.Equals("G*G*GPPW--****X")) //Waypoint - right of center
                    || (temp.Equals("G*G*APP---****X"))//ACP - circle, just below center
                    || (temp.Equals("G*G*APC---****X"))//CCP - circle, just below center
                    || (temp.Equals("G*G*DPT---****X")) //Target Reference - target special
                    || (temp.Equals("G*F*PTS---****X"))//Point/Single Target - target special
                    || (temp.Equals("G*F*PTN---****X"))//Nuclear Target - target special
                    || (temp.Equals("G*F*PCF---****X")) //Fire Support Station - right of center
                    || (temp.Equals("G*M*NZ----****X")) //NUCLEAR DETINATIONS GROUND ZERO
                    || (temp.Equals("G*M*NEB---****X"))//BIOLOGICAL
                    || (temp.Equals("G*M*NEC---****X"))//CHEMICAL
                    || (temp.Equals("G*G*GPRI--****X"))//Point of Interest
                    || (temp.Equals("G*M*OFS---****X"));//Minefield
            return blRetVal;//blRetVal;
        }

        /**
         * Is a single point tactical graphic that has integral text (like the NBC
         * single points)
         * @param strSymbolID
         * @return
         */
        public static Boolean isTGSPWithIntegralText(String strSymbolID)
        {
            String temp = getBasicSymbolID(strSymbolID);

            Boolean blRetVal = (temp.Equals("G*G*GPRD--****X"))//DLRP (D)
                || (temp.Equals("G*G*APU---****X")) //pull-up point (PUP)
                || (temp.Equals("G*M*NZ----****X")) //Nuclear Detonation Ground Zero (N)
                || (temp.Equals("G*M*NF----****X"))//Fallout Producing (N)
                || (temp.Equals("G*M*NEB---****X"))//Release Events Chemical (BIO, B)
                || (temp.Equals("G*M*NEC---****X"));//Release Events Chemical (CML, C)

            return blRetVal;//blRetVal;
        }


        /**
         * Is tactical graphic with fill
         * @param strSymbolID
         * @return
         */
        public static Boolean isTGSPWithFill(String strSymbolID)
        {
            String temp = getBasicSymbolID(strSymbolID);
            Boolean blRetVal = isDeconPoint(temp)//Decon Points
                || temp.StartsWith("G*S*P")//TG/combat service support/points
                || (temp.Equals("G*G*GPP---****X"))//Action points (general)
                || (temp.Equals("G*G*GPPK--****X"))//Check Point
                || (temp.Equals("G*G*GPPL--****X"))//Linkup Point
                || (temp.Equals("G*G*GPPP--****X"))//Passage Point
                || (temp.Equals("G*G*GPPR--****X"))//Rally Point
                || (temp.Equals("G*G*GPPE--****X"))//Release Point
                || (temp.Equals("G*G*GPPS--****X"))//Start Point
                || (temp.Equals("G*G*GPPA--****X"))//Amnesty Point
                || (temp.Equals("G*G*GPPN--****X"))//Entry Control Point
                || (temp.Equals("G*G*APD---****X"))//Down Aircrew Pickup Point
                || (temp.Equals("G*G*OPP---****X"))//Point of Departure
                || (temp.Equals("G*F*PCS---****X"))//Survey Control Point
                || (temp.Equals("G*F*PCB---****X"))//Firing Point
                || (temp.Equals("G*F*PCR---****X"))//Reload Point
                || (temp.Equals("G*F*PCH---****X"))//Hide Point
                || (temp.Equals("G*F*PCL---****X"))//Launch Point
                || (temp.Equals("G*M*BCP---****X"))//Engineer Regulating Point
                || (temp.Equals("G*O*ES----****X"))//Emergency Distress Call

                //star
                || (temp.StartsWith("G*G*GPPD-"))//Decision Point    

                //circle
                || (temp.Equals("G*G*GPPO--****X"))//Coordination Point
                || (temp.Equals("G*G*APP---****X"))//ACP
                || (temp.Equals("G*G*APC---****X"))//CCP
                || (temp.Equals("G*G*APU---****X"))//PUP

                //circle with squiggly
                || (temp.StartsWith("G*G*GPUY"))//SONOBUOY and those that fall under it
                
                //reference point
                || ((temp.StartsWith("G*G*GPR") && temp[7] != 'I'))
                //NBC
                || (temp.Equals("G*M*NEB---****X"))//BIO
                || (temp.Equals("G*M*NEC---****X")) //CHEM
                || (temp.Equals("G*M*NF----****X")) //fallout producing
                || (temp.Equals("G*M*NZ----****X"));//NUC

            return blRetVal;
        }


        public static Boolean hasDefaultFill(String strSymbolID)
        {
            if (SymbolUtilities.isTacticalGraphic(strSymbolID))
            {
                String temp = SymbolUtilities.getBasicSymbolID(strSymbolID);
                //SymbolDef sd = SymbolDefTable.getInstance().getSymbolDef(temp);
                if ((temp.Equals("G*M*NEB---****X"))//BIO
                    || (temp.Equals("G*M*NEC---****X")) //CHEM
                    // || (temp.equals("G*M*NF----****X")) //fallout producing
                    || (temp.Equals("G*M*NZ----****X")))//NUC)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }


        /**
         *
         * @param strSymbolID
         * @return
         */
        public static String getTGFillSymbolCode(String strSymbolID)
        {
            String temp = getBasicSymbolID(strSymbolID);
            if (temp.Equals("G*M*NEB---****X"))
            {
                return "NBCBIOFILL****X";
            }
            if (temp.Equals("G*M*NEC---****X"))
            {
                return "NBCCMLFILL****X";
            }
            if (temp.Equals("G*M*NZ----****X") || temp.Equals("G*M*NF----****X"))
            {
                return "NBCNUCFILL****X";
            }
            if (temp.StartsWith("G*G*GPUY"))
            {
                return "SONOBYFILL****X";
            }
            if ((temp.Equals("G*G*GPPO--****X"))//Coordination Point
                    || (temp.Equals("G*G*APP---****X"))//ACP
                    || (temp.Equals("G*G*APC---****X"))//CCP
                    || (temp.Equals("G*G*APU---****X")))//PUP)
            {
                return "CPOINTFILL****X";
            }
            if (isDeconPoint(temp)//Decon Points
                    || temp.StartsWith("G*S*P")//TG/combat service support/points
                    || (temp.Equals("G*G*GPP---****X"))//Action points (general)
                    || (temp.Equals("G*G*GPPK--****X"))//Check Point
                    || (temp.Equals("G*G*GPPL--****X"))//Linkup Point
                    || (temp.Equals("G*G*GPPP--****X"))//Passage Point
                    || (temp.Equals("G*G*GPPR--****X"))//Rally Point
                    || (temp.Equals("G*G*GPPE--****X"))//Release Point
                    || (temp.Equals("G*G*GPPS--****X"))//Start Point
                    || (temp.Equals("G*G*GPPA--****X"))//Amnesty Point
                    || (temp.Equals("G*G*APD---****X"))//Down Aircrew Pickup Point
                    || (temp.Equals("G*G*OPP---****X"))//Point of Departure
                    || (temp.Equals("G*F*PCS---****X"))//Survey Control Point
                    || (temp.Equals("G*F*PCB---****X"))//Firing Point
                    || (temp.Equals("G*F*PCR---****X"))//Reload Point
                    || (temp.Equals("G*F*PCH---****X"))//Hide Point
                    || (temp.Equals("G*F*PCL---****X"))//Launch Point
                    || (temp.Equals("G*G*GPPN--****X"))//Entry Control Point
                    || (temp.Equals("G*O*ES----****X"))//Emergency Distress Call
                    || (temp.Equals("G*M*BCP---****X")))//Engineer Regulating Point
            {
                return "CHKPNTFILL****X";
            }
            if (temp.StartsWith("G*G*GPR") && temp[7] != 'I')
            {
                return "REFPNTFILL****X";
            }
            if (temp.StartsWith("G*G*GPPD"))
            {
                return "DECPNTFILL****X";
            }


            return null;
        }

        public static Boolean isWeatherSPWithFill(String symbolID)
        {
            if (symbolID.Equals("WOS-HPM-R-P----") ||//landing ring - brown 148,48,0
                symbolID.Equals("WOS-HPD---P----") ||//dolphin facilities - brown
                symbolID.Equals("WOS-HABB--P----") ||//buoy default - 255,0,255
                symbolID.Equals("WOS-HHRS--P----") ||//rock submerged - 0,204,255
                symbolID.Equals("WOS-HHDS--P----") ||//snags/stumps - 0,204,255
                symbolID.Equals("WOS-HHDWB-P----") ||//wreck - 0,204,255
                symbolID.Equals("WOS-TCCTG-P----"))//tide gauge - 210, 176, 106
                return true;
            else
                return false;
        }


        /**
         * @name isSOF
         *
         * @desc Returns true if the symbolID is an SOF (special operations forces)
         * graphic
         *
         * @param strSymbolID - IN - SymbolID we are checking on
         * @return True if the graphic is a SOF in the MIL-STD 2525B, false otherwise.
         */
        public static Boolean isSOF(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID[0]==('S')) && (strSymbolID[2]=='F'));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isSOF

        /**
        * @desc Returns true if the symbol id is a Sonobuoy point symbol.
        *
        * @param strSymbolID - IN - Symbol Id we are checking to see if it is a
        * Sonobuoy Point
        * @return True if the graphic is a Decon Point in the MIL-STD 2525B, false
        * otherwise.
        */
        public static Boolean isSonobuoy(String strSymbolID)
        {

            String basic = getBasicSymbolID(strSymbolID);
            Boolean blRetVal = (basic.Substring(0, 8) == "G*G*GPUY");
            return blRetVal;

        } // End isSOF

        /**
        * @name isSeaSurface
        *
        * @desc Returns true if the symbolID is an warfighting/seasurface graphic
        *
        * @param strSymbolID - IN - SymbolID we are checking on
        * @return True if the graphic is a seasurface in the MIL-STD 2525B, false
        * otherwise.
        */
        public static Boolean isSeaSurface(String strSymbolID)
        {
            
                Boolean blRetVal = ((strSymbolID[0] == 'S') && (strSymbolID[2] == 'S'));
                return blRetVal;
           
        } // End isSOF

        /**
       * @name isSubSurface
       *
       * @desc Returns true if the symbolID is an warfighting/subsurface graphic
       *
       * @param strSymbolID - IN - SymbolID we are checking on
       * @return True if the graphic is a subsurface in the MIL-STD 2525B, false otherwise.
       */
        public static Boolean isSubSurface(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID[0]==('S')) && (strSymbolID[2]=='U'));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End isSOF




        /**
         * @name isEquipment
         *
         * @desc Returns true if the symbol id is an Equipment Id (S*G*E).
         *
         * @param strSymbolID - IN - A MilStd2525B symbolID
         * @return True if symbol is Equipment, false otherwise.
         */
        public static Boolean isEquipment(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID[0]=='S') && 
                        (strSymbolID[2]=='G') &&
                        (strSymbolID[4]=='E'));
                    // || isEMSEquipment(strSymbolID); //uncomment when supporting 2525C
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsEquipment

        /**
         * determines if an EMS symbol (a symbol code that starts with 'E')
         * Is an equipment type.  There is no logical pattern to EMS equipment
         * symbol codes so all we can do is check against a list of codes.
         * @param strSymbolID
         * @return
         */
        public static Boolean isEMSEquipment(String strSymbolID)
        {
            String basicCode = getBasicSymbolID(strSymbolID);
            Boolean blRetVal = false;
            try
            {
                if(strSymbolID.StartsWith("E"))
                {
                    if (basicCode.Equals("E*O*AB----*****") || //equipment
                        basicCode.Equals("E*O*AE----*****") ||//ambulance
                        basicCode.Equals("E*O*AF----*****") ||//medivac helicopter
                        basicCode.Equals("E*O*BB----*****") ||//emergency operation equipment
                        basicCode.Equals("E*O*CB----*****") ||//fire fighting operation equipment
                        basicCode.Equals("E*O*CC----*****") ||//fire hydrant
                        basicCode.Equals("E*O*DB----*****") ||//law enforcement operation equipment
                        //equipment for different service departments
                        (basicCode.StartsWith("E*O*D") && basicCode.EndsWith("B---*****"))
                        || //different sensor types
                        (basicCode.StartsWith("E*O*E") && basicCode.EndsWith("----*****"))
                        || basicCode.Equals("E*F*BA----*****") ||//ATM
                        basicCode.Equals("E*F*LF----*****") ||//Heli Landing site
                        basicCode.Equals("E*F*MA----*****") ||//control valve
                        basicCode.Equals("E*F*MC----*****"))// ||//discharge outfall
                    {
                        blRetVal = true;
                    }
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return blRetVal;
        } // End IsEquipment

        /**
        * determines if an symbol code represents an EMS (Emergency Management
        * Symbol).  Returns true only for those that start with 'E'
        * @return
        */
        public static Boolean isEMS(String strSymbolID)
        {
            //String basicCode = getBasicSymbolID(strSymbolID);
            Boolean blRetVal = false;
            try
            {
                if(strSymbolID.StartsWith("E"))
                {
                    blRetVal = true;
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return blRetVal;
        } // End IsEquipment




        /**
         * Determines if a symbol is an EMS Natural Event
         * @param strSymbolID
         * @return 
         */
       
        public static Boolean isEMSNaturalEvent(String strSymbolID)
        {
            Boolean blRetVal = false;
            try
            {
                if(strSymbolID.ElementAt(0)=='E' && strSymbolID.ElementAt(2)=='N')
                {
                    blRetVal = true;
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc);
            }
            return blRetVal;
         }

        /**
         * Determines if a symbol is an EMS Incident
         * @param strSymbolID
         * @return 
         */
        public static Boolean isEMSIncident(String strSymbolID)
        {
            Boolean blRetVal = false;
            try
            {
                if(strSymbolID.ElementAt(0)=='E' && strSymbolID.ElementAt(2)=='I')
                {
                    blRetVal = true;
                }
            }
            catch(Exception exc)
            {
                Debug.Write(exc);
            }
            return blRetVal;
        }


        /**
         * @name isInstallation
         *
         * @desc Returns true if the symbol id is an installation (S*G*I).
         *
         * @param strSymbolID - IN - A MilStd2525B symbolID
         * @return True if symbol is an Installation, false otherwise.
         */
        public static Boolean isInstallation(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = ((strSymbolID[0]=='S') && (strSymbolID[2]=='G') && (strSymbolID[4]=='I'));
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsInstallation

        /**
       * @name isSIGINT
       *
       * @desc Returns true if the symbol id is Signals Intelligence (SIGINT) (starts with 'I').
       *
       * @param strSymbolID - IN - A MilStd2525B symbolID
       * @return True if symbol is a Signals Intelligence, false otherwise.
       */
        public static Boolean isSIGINT(String strSymbolID)
        {
            try
            {
                Boolean blRetVal = (strSymbolID[0]=='I');
                return blRetVal;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return false;
        } // End IsInstallation

        /**
         * @name isFeintDummyInstallation
         *
         * @desc Returns true if the symbol id has a feint dummy installation modifier
         *
         * @param strSymbolID - IN - A MilStd2525B symbolID
         * @return True if symbol has a feint dummy installation modifier, false
         * otherwise.
         */
        public static Boolean isFeintDummyInstallation(String strSymbolID)
        {
            Boolean feintDummyInstallationIsOn = false;
            try
            {
                // See if the feint dummy installation is on.
                feintDummyInstallationIsOn = (strSymbolID.Substring(10, 1)==("H") && strSymbolID.Substring(11, 1)==("B"));
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            // Return whether or not the feint dummy installation is on.
            return feintDummyInstallationIsOn;
        }

        /**
         * has an 'H' in the 11th position
         * Any symbol can have this character added to make it an installation.
         * @param strSymbolID
         * @return
         */
        public static Boolean hasInstallationModifier(String strSymbolID)
        {
            Boolean hasInstallationModifier = false;
            try
            {
                // See if the feint dummy installation is on.
                hasInstallationModifier = (strSymbolID[10]=='H');
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            // Return whether or not the feint dummy installation is on.
            return hasInstallationModifier;
        }

        /**
         * @name getAffiliation
         *
         * @desc This operation will return the affiliation enumeration for the given
         * symbol id. If the symbol has an unknown or offbeat affiliation, the
         * affiliation of "U" will be returned.
         *
         * @param strSymbolID - IN - Symbol Id we want the affiliation of
         * @return The affiliation of the Symbol Id that was passed in.
         */
        public static char getAffiliation(String strSymbolID)
        {
            try
            {
                char strAffiliation = strSymbolID[1];
                return strAffiliation;
            } // End try
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return 'U';
        } // End GetAffiliation

        /**
         * @name setAffiliation
         *
         * @desc Sets the affiliation for a Mil-Std 2525B symbol ID.
         *
         * @param strSymbolID - IN - A 15 character symbol ID
         * @param strSymbolID - IN - The affiliation we want to change the ID to.
         * @return A string with the affiliation changed to affiliationID
         */
        public static String setAffiliation(String strSymbolID, String strAffiliationID)
        {

            String returnVal = null;
            if (strSymbolID != null && strSymbolID.Length == 15)// &&
                                                                // !IsDrawingPrimitive(strSymbolID))
            {
                returnVal = strSymbolID.Substring(0, 1) + strAffiliationID + strSymbolID.Substring(2, 13);
                if (hasValidAffiliation(returnVal))
                    return returnVal;
                else
                    return strSymbolID;
            }

            return strSymbolID;
        } // End SetAffiliation }

        /**
         * @name getStatus
         *
         * @desc Returns the status (present / planned) for the symbol id provided. If
         * the symbol contains some other status than planned or present, present is
         * returned by default (no unknown available).
         *
         * @param strSymbolID - IN - 15 char symbol code.
         * @return The status of the Symbol Id that was passed in.
         */

        public static String getStatus(String strSymbolID)
        {
            try
            {
                String strStatus = strSymbolID.Substring(3, 1);
                return strStatus;
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return "P";
        } // End getStatus

        /**
         * @name setStatus
         *
         * @desc Sets the status for a Mil-Std 2525B symbol ID.
         *
         * @param strSymbolID - IN - A 15 character symbol ID
         * @param strStatusID - IN - The status we want to change the ID to.
         * "present", "planned", "anticipated", "plannedanticipated"
         * @return A string with the status changed to statusID
         * @deprecated 
         */
        public static String setStatus(String strSymbolID, String strStatusID)
        {
            // PlannedAnticipated, //A
            // Present //P
            String changedID = strSymbolID;

            if ((strSymbolID != null && strSymbolID.Length == 15)
                && (!isWeather(strSymbolID)))
            {
                changedID = strSymbolID.Substring(0, 3) + strStatusID + strSymbolID.Substring(4, 11);

            }

            return changedID;
        } // End SetStatus

        /**
         * @name getEchelon
         *
         * @desc Returns the echelon enumeration for the symbol id provided. Note;
         * this works only with the sub-set of echelon codes tracked in the SymbolID
         * class. 2525 contains more codes than are tracked here. The 11th char of the
         * symbol id is used to determine the echelon. If we are unable to determine
         * the echelon, we return "NULL".
         *
         * @param strSymbolID - IN - 15 char symbol code.
         * @return The echelon of the Symbol Id that was passed in.
         */
        public static String getEchelon(String strSymbolID)
        {
            try
            {
                String strSubEch = strSymbolID.Substring(11, 1);
                return strSubEch;
            } // End try
            catch(Exception exc)
            {
                Debug.WriteLine(exc.StackTrace);
            }
            return "-";
        } // End getEchelon


        /**
         * checks symbol code to see if graphic has a DOM (Q) modifier
         * @param symbolID
         * @return
         */

        public static String getUnitAffiliationModifier(String symbolID, int symStd)
        {
            String textChar = null;
            char affiliation;

            try
            {
                affiliation = symbolID.ElementAt(1);

                if (affiliation == ('F') ||
                    affiliation == ('H') ||
                    affiliation == ('U') ||
                    affiliation == ('N') ||
                    affiliation == ('P'))
                    textChar = null;
                else if (affiliation == ('A') ||
                        affiliation == ('S'))
                {
                    if (symStd == MilStd.Symbology_2525Bch2_USAS_13_14)
                        textChar = "?";
                    else
                        textChar = null;
                }
                else if (affiliation == ('J'))
                    textChar = "J";
                else if (affiliation == ('K'))
                    textChar = "K";
                else if (affiliation == ('D') ||
                        affiliation == ('L') ||
                        affiliation == ('G') ||
                        affiliation == ('W'))
                    textChar = "X";
                else if (affiliation == ('M'))
                {
                    if (symStd == MilStd.Symbology_2525Bch2_USAS_13_14)
                        textChar = "X?";
                    else
                        textChar = "X";
                }

                //check sea mine symbols
                if (symStd == MilStd.Symbology_2525C)
                {
                    if (symbolID.ElementAt(0) == 'S' && symbolID.IndexOf("WM") == 4)
                    {//variuos sea mine exercises
                        if (symbolID.IndexOf("GX") == 6 ||
                                symbolID.IndexOf("MX") == 6 ||
                                symbolID.IndexOf("FX") == 6 ||
                                symbolID.IndexOf("X") == 6 ||
                                symbolID.IndexOf("SX") == 6)
                            textChar = "X";
                        else
                            textChar = null;
                    }
                }
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolUtilities",
                        "getUnitAffiliationModifier", exc);// Level.WARNING);
                return null;
            }
            return textChar;
        }



        public static Boolean hasAMmodifierWidth(String symbolID, int symStd)
        {
            SymbolDef sd = null;
            Boolean returnVal = false;
            String basic = SymbolUtilities.getBasicSymbolID(symbolID);

            sd = SymbolDefTable.getInstance().getSymbolDef(basic, symStd);
            if (sd != null)
            {
                int dc = sd.getDrawCategory();

                switch (dc)
                {
                    case SymbolDef.DRAW_CATEGORY_RECTANGULAR_PARAMETERED_AUTOSHAPE://width
                    case SymbolDef.DRAW_CATEGORY_SECTOR_PARAMETERED_AUTOSHAPE:
                    case SymbolDef.DRAW_CATEGORY_TWO_POINT_RECT_PARAMETERED_AUTOSHAPE:
                        returnVal = true;
                        break;
                    default:
                        returnVal = false;
                        break;
                }
            }

            return returnVal;
        }

        public static Boolean hasANmodifier(String symbolID, int symStd)
        {
            SymbolDef sd = null;
            Boolean returnVal = false;
            String basic = SymbolUtilities.getBasicSymbolID(symbolID);

            sd = SymbolDefTable.getInstance().getSymbolDef(basic, symStd);
            if (sd != null)
            {
                int dc = sd.getDrawCategory();

                switch (dc)
                {
                    case SymbolDef.DRAW_CATEGORY_RECTANGULAR_PARAMETERED_AUTOSHAPE://width
                    case SymbolDef.DRAW_CATEGORY_SECTOR_PARAMETERED_AUTOSHAPE:
                        returnVal = true;
                        break;
                    default:
                        returnVal = false;
                        break;
                }
            }

            return returnVal;
        }


        public static Boolean hasAMmodifierRadius(String symbolID, int symStd)
        {
            SymbolDef sd = null;
            Boolean returnVal = false;
            String basic = SymbolUtilities.getBasicSymbolID(symbolID);

            sd = SymbolDefTable.getInstance().getSymbolDef(basic, symStd);
            if (sd != null)
            {
                int dc = sd.getDrawCategory();

                switch (dc)
                {
                    case SymbolDef.DRAW_CATEGORY_CIRCULAR_PARAMETERED_AUTOSHAPE://radius
                    case SymbolDef.DRAW_CATEGORY_CIRCULAR_RANGEFAN_AUTOSHAPE:
                        returnVal = true;
                        break;
                    default:
                        returnVal = false;
                        break;
                }
            }

            return returnVal;
        }

        /**
 * Gets line color used if no line color has been set. The color is specified based on the affiliation of
 * the symbol and whether it is a unit or not.
 * @param symbolID
 * @return
 */
        public static Color getLineColorOfAffiliation(String symbolID)
        {
            Color retColor = Color.Empty;
            String basicSymbolID = getBasicSymbolID(symbolID);
            try
            {
                // We can't get the fill color if there is no symbol id, since that also means there is no affiliation
                if ((symbolID == null) || (symbolID.Equals("")))
                {
                    return retColor;
                }

                if (SymbolUtilities.isTacticalGraphic(symbolID))// && !SymbolUtilities.isTGWithFill(symbolID))
                {
                    if (SymbolUtilities.isWeather(symbolID))
                    {
                        retColor = getLineColorOfWeather(symbolID);
                    }
                    else if (SymbolUtilities.isObstacle(symbolID))
                    {
                        retColor = Color.Green;	// Green
                    }
                    else if ((SymbolUtilities.isNBC(symbolID))
                            && (basicSymbolID.Equals("G*M*NR----****X") == true || //Radioactive Area
                            basicSymbolID.Equals("G*M*NC----****X") == true || //Chemically Contaminated Area
                            basicSymbolID.Equals("G*M*NB----****X") == true)) //Biologically Contaminated Area
                    {
                        retColor = Color.Black;//0xffff00;
                    }
                    else if (SymbolUtilities.isEMSNaturalEvent(symbolID))
                    {
                        retColor = Color.Black;//0xffff00;
                    }
                    else
                    {
                        char switchChar = symbolID[1];
                        if (switchChar == 'F'
                                || switchChar == 'A'
                                || switchChar == 'D'
                                || switchChar == 'M')
                        {
                            retColor = Color.Black;//0x000000;	// Black
                        }
                        else if (switchChar == 'H'
                                || switchChar == 'S'
                                || switchChar == 'J'
                                || switchChar == 'K')
                        {

                            if (SymbolUtilities.getBasicSymbolID(symbolID).Equals("G*G*GLC---****X")) // Line of Contact
                            {
                                retColor = Color.Black;//0x000000;	// Black
                            }
                            else
                            {
                                retColor = Color.Red;//0xff0000;	// Red
                            }

                        }
                        else if (switchChar.Equals("N")
                                || switchChar.Equals("L")) // Neutral:
                        {
                            retColor = Color.Green;//0x00ff00;	// Green

                        }
                        else if (switchChar.Equals("U")
                                || switchChar.Equals("P")
                                || switchChar.Equals("O")
                                || switchChar.Equals("G")
                                || switchChar.Equals("W"))
                        {
                            if (symbolID.Substring(0, 8).Equals("WOS-HDS-"))
                            {
                                retColor = Color.Gray;//0x808080;	// Gray
                            }
                            else
                            {
                                retColor = Color.Yellow;//0xffff00;	// Yellow
                            }

                        }
                        else
                        {
                            retColor = Color.Black;//null;//0;//Color.Empty;

                        }	// End default

                    }	// End else
                }// End if (SymbolUtilities.IsTacticalGraphic(this.SymbolID))
                else
                {
                    //stopped doing check because all warfighting
                    //should have black for line color.
                    retColor = Color.Black;

                }	// End else
            } // End try
            catch (Exception e)
            {
                // Log Error
                ErrorLogger.LogException("SymbolUtilties", "getLineColorOfAffiliation", e);
                //throw e;
            }	// End catch
            return retColor;
        }   // End get LineColorOfAffiliation


        /**
         * Is the fill color used if no fill color has been set. The color is specified based on the affiliation
          of the symbol and whether it is a unit or not.
         * @param symbolID
         * @return
         */

        public static Color getFillColorOfAffiliation(String symbolID)
        {
            Color retColor = Color.Empty;
            String basicSymbolID = getBasicSymbolID(symbolID);

            try
            {
                char switchChar;
                // We can't get the fill color if there is no symbol id, since that also means there is no affiliation
                if ((symbolID == null) || (symbolID.Equals("")))
                {
                    return retColor;
                }

                if (basicSymbolID.Equals("G*M*NZ----****X") ||//ground zero
                                                              //basicSymbolID.Equals("G*M*NF----****X") || //fallout producing
                        basicSymbolID.Equals("G*M*NEB---****X") ||//biological
                        basicSymbolID.Equals("G*M*NEC---****X"))//chemical
                {
                    retColor = AffiliationColors.UnknownUnitFillColor;//  Color.yellow;
                }
                else if (SymbolUtilities.isTacticalGraphic(symbolID) && !SymbolUtilities.isTGSPWithFill(symbolID))
                {
                    if (basicSymbolID.Equals("G*M*NZ----****X") ||//ground zero
                                                                  //basicSymbolID.Equals("G*M*NF----****X") || //fallout producing
                            basicSymbolID.Equals("G*M*NEB---****X") ||//biological
                            basicSymbolID.Equals("G*M*NEC---****X"))//chemical
                    {
                        retColor = Color.Yellow;
                    }
                    else
                    {
                        switchChar = symbolID[1];
                        if (switchChar == 'F'
                                || switchChar == 'A'
                                || switchChar == 'D'
                                || switchChar == 'M')
                        {
                            retColor = AffiliationColors.FriendlyGraphicFillColor;//0x00ffff;	// Cyan

                        }
                        else if (switchChar == 'H'
                                || switchChar == 'S'
                                || switchChar == 'J'
                                || switchChar == 'K')
                        {
                            retColor = AffiliationColors.HostileGraphicFillColor;//0xfa8072;	// Salmon

                        }
                        else if (switchChar == 'N'
                                || switchChar == 'L')
                        {
                            retColor = AffiliationColors.NeutralGraphicFillColor;//0x7fff00;	// Light Green

                        }
                        else if (switchChar == 'U'
                                || switchChar == 'P'
                                || switchChar == 'O'
                                || switchChar == 'G'
                                || switchChar == 'W')
                        {
                            retColor = Color.FromArgb(255, 250, 205); //0xfffacd;	// LemonChiffon 255 250 205
                        }
                        else
                        {
                            retColor = Color.Empty;
                        }
                    }
                } // End if(SymbolUtilities.IsTacticalGraphic(this._strSymbolID))
                else
                {
                    switchChar = symbolID[1];
                    if (switchChar == 'F'
                            || switchChar == 'A'
                            || switchChar == 'D'
                            || switchChar == 'M')
                    {
                        retColor = AffiliationColors.FriendlyUnitFillColor;//0x00ffff;	// Cyan

                    }
                    else if (switchChar == 'H'
                            || switchChar == 'S'
                            || switchChar == 'J'
                            || switchChar == 'K')
                    {
                        retColor = AffiliationColors.HostileUnitFillColor;//0xfa8072;	// Salmon

                    }
                    else if (switchChar == 'N'
                            || switchChar == 'L')
                    {
                        retColor = AffiliationColors.NeutralUnitFillColor;//0x7fff00;	// Light Green

                    }
                    else if (switchChar == 'U'
                            || switchChar == 'P'
                            || switchChar == 'O'
                            || switchChar == 'G'
                            || switchChar == 'W')
                    {
                        retColor = AffiliationColors.UnknownUnitFillColor;//new Color(255,250, 205); //0xfffacd;	// LemonChiffon 255 250 205
                    }
                    else
                    {
                        retColor = AffiliationColors.UnknownUnitFillColor;//null;
                    }

                }	// End else
            } // End try
            catch (Exception e)
            {
                // Log Error
                ErrorLogger.LogException("SymbolUtilties", "getFillColorOfAffiliation", e);
                //throw e;
            }	// End catch

            return retColor;
        }	// End FillColorOfAffiliation


        public static Color getLineColorOfWeather(String symbolID)
        {
            Color retColor = Color.Black;
            // Get the basic id
            //String symbolID = SymbolUtilities.getBasicSymbolID(symbolID);

            //if(symbolID.Equals(get))
            if (symbolID.Equals("WAS-WSGRL-P----") || // Hail - Light not Associated With Thunder
                symbolID.Equals("WAS-WSGRMHP----") || // Hail - Moderate/Heavy not Associated with Thunder
                symbolID.Equals("WAS-PL----P----") || // Low Pressure Center - Pressure Systems
                symbolID.Equals("WAS-PC----P----") || // Cyclone Center - Pressure Systems
                symbolID.Equals("WAS-WSIC--P----") || // Ice Crystals (Diamond Dust)
                symbolID.Equals("WAS-WSPLL-P----") || // Ice Pellets - Light
                symbolID.Equals("WAS-WSPLM-P----") || // Ice Pellets - Moderate
                symbolID.Equals("WAS-WSPLH-P----") || // Ice Pellets - Heavy
                symbolID.Equals("WAS-WST-NPP----") || // Thunderstorm - No Precipication
                symbolID.Equals("WAS-WSTMR-P----") || // Thunderstorm Light to Moderate with Rain/Snow - No Hail
                symbolID.Equals("WAS-WSTHR-P----") || // Thunderstorm Heavy with Rain/Snow - No Hail
                symbolID.Equals("WAS-WSTMH-P----") || // Thunderstorm Light to Moderate - With Hail
                symbolID.Equals("WAS-WSTHH-P----") || // Thunderstorm Heavy - With Hail
                symbolID.Equals("WAS-WST-FCP----") || // Funnel Cloud (Tornado/Waterspout)
                symbolID.Equals("WAS-WST-SQP----") || // Squall
                symbolID.Equals("WAS-WST-LGP----") || // Lightning
                symbolID.Equals("WAS-WSFGFVP----") || // Fog - Freezing, Sky Visible
                symbolID.Equals("WAS-WSFGFOP----") || // Fog - Freezing, Sky not Visible
                symbolID.Equals("WAS-WSTSD-P----") || // Tropical Depression
                symbolID.Equals("WAS-WSTSS-P----") || // Tropical Storm
                symbolID.Equals("WAS-WSTSH-P----") || // Hurricane/Typhoon
                symbolID.Equals("WAS-WSRFL-P----") || // Freezing Rain - Light
                symbolID.Equals("WAS-WSRFMHP----") || // Freezing Rain - Moderate/Heavy
                symbolID.Equals("WAS-WSDFL-P----") || // Freezing Drizzle - Light
                symbolID.Equals("WAS-WSDFMHP----") || // Freezing Drizzle - Moderate/Heavy
                symbolID.Equals("WOS-HHDMDBP----") || //mine-naval (doubtful)
                symbolID.Equals("WOS-HHDMDFP----") || // mine-naval (definited)
                symbolID.Substring(0, 7).Equals("WA-DPFW") || //warm front
                                                              //symbolID.substring(0,7).Equals("WA-DPFS")//stationary front (actually, it's red & blue)
                symbolID.Equals("WA-DBAIF----A--") || // INSTRUMENT FLIGHT RULE (IFR)
                symbolID.Equals("WA-DBAFP----A--") || // 
                symbolID.Equals("WA-DBAT-----A--") || // 
                symbolID.Equals("WA-DIPIS---L---") || // 
                symbolID.Equals("WA-DIPTH---L---") || // 
                symbolID.Equals("WA-DWJ-----L---") || // Jet Stream  
                symbolID.Equals("WO-DGMSB----A--") || //
                symbolID.Equals("WO-DGMRR----A--") ||
                symbolID.Equals("WO-DGMCH----A--") ||
                symbolID.Equals("WO-DGMIBE---A--") ||
                symbolID.Equals("WO-DGMBCC---A--") ||
                symbolID.Equals("WO-DOBVI----A--"))
            {
                retColor = Color.Red;//0xff0000;	// Red
            }
            else if (symbolID.Equals("WAS-PH----P----") || // High Pressure Center - Pressure Systems
                    symbolID.Equals("WAS-PA----P----") || // Anticyclone Center - Pressure Systems
                    symbolID.Equals("WA-DBAMV----A--") || // MARGINAL VISUAL FLIGHT RULE (MVFR)
                    symbolID.Equals("WA-DBATB----A--") || // BOUNDED AREAS OF WEATHER / TURBULENCE
                    symbolID.Substring(0, 5).Equals("WAS-T") || // Turbulence
                    symbolID.Substring(0, 7).Equals("WA-DPFC") || //cold front
                    symbolID.Equals("WO-DGMIBA---A--"))
            {
                retColor = Color.Blue;
            }
            else if (
            symbolID.Equals("WAS-WSFGPSP----") || // Fog - Shallow Patches
            symbolID.Equals("WAS-WSFGCSP----") || // Fog - Shallow Continuous
            symbolID.Equals("WAS-WSFGP-P----") || // Fog - Patchy
            symbolID.Equals("WAS-WSFGSVP----") || // Fog - Sky Visible
            symbolID.Equals("WAS-WSFGSOP----") || // Fog - Sky Obscured
            symbolID.Equals("WA-DBAFG----A--") || // Fog
            symbolID.Equals("WO-DGMRM----A--") ||
            symbolID.Equals("WO-DGMCM----A--") ||
            symbolID.Equals("WO-DGMIBC---A--") ||
            symbolID.Equals("WO-DGMBCB---A--") ||
            symbolID.Equals("WO-DGMBTE---A--") ||
            symbolID.Equals("WAS-WSBR--P----")) // Mist
            {
                retColor = Color.Yellow;//0xffff00;	// Yellow
            }
            else if (
            symbolID.Equals("WAS-WSFU--P----") || // Smoke
            symbolID.Equals("WAS-WSHZ--P----") || // Haze
            symbolID.Equals("WAS-WSDSLMP----") || // Dust/Sand Storm - Light to Moderate
            symbolID.Equals("WAS-WSDSS-P----") || // Dust/Sand Storm - Severe
            symbolID.Equals("WAS-WSDD--P----") || // Dust Devil
            symbolID.Equals("WA-DBAD-----A--") || // Dust or Sand
            symbolID.Equals("WAS-WSBD--P----")) // Blowing Dust or Sand
            {
                retColor = Color.FromArgb(165, 42, 42);  //165 42 42 //0xa52a2a;	// Brown
            }
            else if (
            symbolID.Equals("WA-DBALPNC--A--") || // 
            symbolID.Equals("WA-DBALPC---A--") || // 
            symbolID.Equals("WA-DIPID---L---") || // 
            symbolID.Equals("WO-DHCF----L---") || // 
            symbolID.Equals("WO-DHCF-----A--") || // 
            symbolID.Equals("WO-DGMSIM---A--") || //
            symbolID.Equals("WO-DGMRS----A--") ||
            symbolID.Equals("WO-DGMCL----A--") ||
            symbolID.Equals("WO-DGMIBB---A--") ||
            symbolID.Equals("WO-DGMBCA---A--") ||
            symbolID.Equals("WAS-WSR-LIP----") || // Rain - Intermittent Light
            symbolID.Equals("WAS-WSR-LCP----") || // Rain - Continuous Light
            symbolID.Equals("WAS-WSR-MIP----") || // Rain - Intermittent Moderate
            symbolID.Equals("WAS-WSR-MCP----") || // Rain - Continuous Moderate
            symbolID.Equals("WAS-WSR-HIP----") || // Rain - Intermittent Heavy
            symbolID.Equals("WAS-WSR-HCP----") || // Rain - Continuous Heavy
            symbolID.Equals("WAS-WSRSL-P----") || // Rain Showers - Light
            symbolID.Equals("WAS-WSRSMHP----") || // Rain Showers - Moderate/Heavy
            symbolID.Equals("WAS-WSRST-P----") || // Rain Showers - Torrential
            symbolID.Equals("WAS-WSD-LIP----") || // Drizzle - Intermittent Light
            symbolID.Equals("WAS-WSD-LCP----") || // Drizzle - Continuous Light
            symbolID.Equals("WAS-WSD-MIP----") || // Drizzle - Intermittent Moderate
            symbolID.Equals("WAS-WSD-MCP----") || // Drizzle - Continuous Moderate
            symbolID.Equals("WAS-WSD-HIP----") || // Drizzle - Intermittent Heavy
            symbolID.Equals("WAS-WSD-HCP----") || // Drizzle - Continuous Heavy
            symbolID.Equals("WAS-WSM-L-P----") || // Rain or Drizzle and Snow - Light
            symbolID.Equals("WAS-WSM-MHP----") || // Rain or Drizzle and Snow - Moderate/Heavy
            symbolID.Equals("WAS-WSMSL-P----") || // Rain and Snow Showers - Light
            symbolID.Equals("WAS-WSMSMHP----") || // Rain and Snow Showers - Moderate/Heavy
            symbolID.Equals("WAS-WSUKP-P----") || // Precipitation of unknown type & intensity
            symbolID.Equals("WAS-WSS-LIP----") || // Snow - Intermittent Light
            symbolID.Equals("WAS-WSS-LCP----") || // Snow - Continuous Light
            symbolID.Equals("WAS-WSS-MIP----") || // Snow - Intermittent Moderate
            symbolID.Equals("WAS-WSS-MCP----") || // Snow - Continuous Moderate
            symbolID.Equals("WAS-WSS-HIP----") || // Snow - Intermittent Heavy
            symbolID.Equals("WAS-WSS-HCP----") || // Snow - Continuous Heavy
            symbolID.Equals("WAS-WSSBLMP----") || // Blowing Snow - Light/Moderate
            symbolID.Equals("WAS-WSSBH-P----") || // Blowing Snow - Heavy
            symbolID.Equals("WAS-WSSG--P----") || // Snow Grains
            symbolID.Equals("WAS-WSSSL-P----") || // Snow Showers - Light
            symbolID.Equals("WAS-WSSSMHP----")) // Snow Showers - Moderate/Heavy
            {
                retColor = Color.Green;// 0x00ff00;	// Green
            }
            else if (symbolID.StartsWith("WAS-IC") || // Clear Icing
                                        symbolID.StartsWith("WAS-IR") || // Rime Icing
                                        symbolID.StartsWith("WAS-IM")) // Mixed Icing
            {
                retColor = Color.FromArgb(128, 96, 16);
            }
            else if (symbolID.Equals("WOS-HDS---P----") || // Soundings
                symbolID.Equals("WOS-HHDF--P----") ||//foul ground
                symbolID.Equals("WO-DHHDF----A--") ||//foul ground
                symbolID.Equals("WOS-HPFS--P----") ||//fish stakes/traps/weirs
                symbolID.Equals("WOS-HPFS---L---") ||//fish stakes
                symbolID.Equals("WOS-HPFF----A--") ||//fish stakes/traps/weirs
                symbolID.Equals("WO-DHDDL---L---") ||//depth curve
                symbolID.Equals("WO-DHDDC---L---") ||//depth contour
                symbolID.Equals("WO-DHCC----L---") ||//coastline
                symbolID.Equals("WO-DHPBP---L---") ||//ports
                symbolID.Equals("WO-DHPMO---L---") ||//offshore loading
                symbolID.Equals("WO-DHPSPA--L---") ||//sp above water
                symbolID.Equals("WO-DHPSPB--L---") ||//sp below water
                symbolID.Equals("WO-DHPSPS--L---") ||//sp sea wall
                symbolID.Equals("WO-DHHDK--P----") ||//kelp seaweed
                symbolID.Equals("WO-DHHDK----A--") ||//kelp seaweed
                symbolID.Equals("WO-DHHDB---L---") ||//breakers
                symbolID.Equals("WO-DTCCCFE-L---") ||//current flow - ebb
                symbolID.Equals("WO-DTCCCFF-L---") ||//current flow - flood
                symbolID.Equals("WOS-TCCTD-P----") ||//tide data point    
                symbolID.Equals("WO-DHCW-----A--") ||
                symbolID.Equals("WO-DMOA-----A--") ||
                symbolID.Equals("WO-DMPA----L---"))//water
                retColor = Color.Gray;//0x808080;	// Gray
            else if (
                symbolID.Equals("WO-DBSM-----A--") ||
                symbolID.Equals("WO-DBSF-----A--") ||
                symbolID.Equals("WO-DGMN-----A--")) // 
            {
                retColor = Color.FromArgb(230, 230, 230);//230,230,230;	// light gray
            }
            else if (
                symbolID.Equals("WO-DBSG-----A--")) // 
            {
                retColor = Color.FromArgb(169, 169, 169);//169,169,169;	// dark gray
            }
            else if (
            symbolID.Equals("WAS-WSVE--P----") || // Volcanic Eruption
            symbolID.Equals("WAS-WSVA--P----") || // Volcanic Ash
            symbolID.Equals("WAS-WST-LVP----") || // Tropopause Level
            symbolID.Equals("WAS-WSF-LVP----")) // Freezing Level
            {
                retColor = Color.Black;//0x000000;	// Black
            }
            else if (
            symbolID.Equals("WOS-HPBA--P----") || // anchorage
            symbolID.Equals("WOS-HPBA---L---") || // anchorage
            symbolID.Equals("WOS-HPBA----A--") || // anchorage
            symbolID.Equals("WOS-HPCP--P----") || // call in point
            symbolID.Equals("WOS-HPFH--P----") || // fishing harbor
            symbolID.Equals("WOS-HPM-FC-L---") || //ferry crossing
            symbolID.Equals("WOS-HABM--P----") || //marker
            symbolID.Equals("WOS-HAL---P----") || //light
            symbolID.Equals("WA-DIPIT---L---") || //ISOTACH
            symbolID.Equals("WOS-TCCTG-P----") || // Tide gauge
            symbolID.Equals("WO-DL-ML---L---") ||
            symbolID.Equals("WOS-HPM-FC-L---") ||
            symbolID.Equals("WO-DL-RA---L---") ||
            symbolID.Equals("WO-DHPBA---L---") ||
            symbolID.Equals("WO-DMCA----L---") ||
            symbolID.Equals("WO-DHPBA----A--") ||
            symbolID.Equals("WO-DL-MA----A--") ||
            symbolID.Equals("WO-DL-SA----A--") ||
            symbolID.Equals("WO-DL-TA----A--") ||
            symbolID.Equals("WO-DGMSR----A--"))
            {
                retColor = Color.FromArgb(255, 0, 255);//magenta
            }
            else if (symbolID.Substring(0, 7).Equals("WA-DPFO")//occluded front
            )
            {
                retColor = Color.FromArgb(226, 159, 255);//light purple
            }
            else if (
            symbolID.Equals("WA-DPXITCZ-L---") || // inter-tropical convergance zone oragne?
            symbolID.Equals("WO-DL-O-----A--") ||
            symbolID.Equals("WA-DPXCV---L---")) // 
            {
                retColor = Color.FromArgb(255, 165, 0);//orange
            }
            else if (
            symbolID.Equals("WA-DBAI-----A--") || //BOUNDED AREAS OF WEATHER / ICING
            symbolID.StartsWith("WAS-IC") || // clear icing
            symbolID.StartsWith("WAS-IR") || // rime icing
            symbolID.StartsWith("WAS-IM")) // mixed icing
            {
                retColor = Color.FromArgb(128, 96, 16);//mud?
            }
            else if (
            symbolID.Equals("WO-DHCI-----A--") || //Island
            symbolID.Equals("WO-DHCB-----A--") || //Beach
            symbolID.Equals("WO-DHPMO---L---") ||//offshore loading
            symbolID.Equals("WO-DHCI-----A--")) // mixed icing
            {
                retColor = Color.FromArgb(210, 176, 106);//light/soft brown
            }
            else if (symbolID.Substring(0, 7).Equals("WO-DOBVA----A--")
            )
            {
                retColor = Color.FromArgb(26, 153, 77);//dark green
            }
            else if (symbolID.Substring(0, 7).Equals("WO-DGMBTI---A--")
            )
            {
                retColor = Color.FromArgb(255, 48, 0);//orange red
            }
            else if (symbolID.Substring(0, 7).Equals("WO-DGMBTH---A--")
            )
            {
                retColor = Color.FromArgb(255, 80, 0);//dark orange
            }
            //255,127,0
            //WO-DGMBTG---A--
            else if (symbolID.Equals("WO-DGMBTG---A--"))
            {
                retColor = Color.FromArgb(255, 127, 0);
            }
            //255,207,0
            //WO-DGMBTF---A--
            else if (symbolID.Equals("WO-DGMBTF---A--"))
            {
                retColor = Color.FromArgb(255, 207, 0);
            }
            //048,255,0
            //WO-DGMBTA---A--
            else if (symbolID.Equals("WO-DGMBTA---A--"))
            {
                retColor = Color.FromArgb(48, 255, 0);
            }
            //220,220,220
            //WO-DGML-----A--
            else if (symbolID.Equals("WO-DGML-----A--"))
            {
                retColor = Color.FromArgb(220, 220, 220);
            }
            //255,220,220
            //WO-DGMS-SH--A--
            else if (symbolID.Equals("WO-DGMS-SH--A--"))
            {
                retColor = Color.FromArgb(255, 220, 220);
            }
            //255,190,190
            //WO-DGMS-PH--A--
            else if (symbolID.Equals("WO-DGMS-PH--A--"))
            {
                retColor = Color.FromArgb(255, 190, 190);
            }
            //lime green 128,255,51
            //WO-DOBVC----A--
            else if (symbolID.Equals("WO-DOBVC----A--"))
            {
                retColor = Color.FromArgb(128, 255, 51);
            }
            //255,255,0
            //WO-DOBVE----A--
            else if (symbolID.Equals("WO-DOBVE----A--"))
            {
                retColor = Color.FromArgb(255, 255, 0);
            }
            //255,150,150
            //WO-DGMS-CO--A--
            else if (symbolID.Equals("WO-DGMS-CO--A--"))
            {
                retColor = Color.FromArgb(255, 150, 150);
            }
            //175,255,0
            //WO-DGMBTC---A--
            else if (symbolID.Equals("WO-DGMBTC---A--"))
            {
                retColor = Color.FromArgb(175, 255, 0);
            }
            //207,255,0
            //WO-DGMBTD---A--
            else if (symbolID.Equals("WO-DGMBTD---A--"))
            {
                retColor = Color.FromArgb(207, 255, 0);
            }
            //127,255,0
            //WO-DGMBTB---A--
            else if (symbolID.Equals("WO-DGMBTB---A--"))
            {
                retColor = Color.FromArgb(127, 255, 0);
            }
            //255,127,0
            //WO-DGMIBD---A--
            else if (symbolID.Equals("WO-DGMIBD---A--"))
            {
                retColor = Color.FromArgb(255, 127, 0);
            }
            else if (symbolID.Equals("WO-DGMSIF---A--"))
            {
                retColor = Color.FromArgb(25, 255, 230);
            }
            //0,215,255
            //WO-DGMSIVF--A--
            else if (symbolID.Equals("WO-DGMSIVF--A--"))
            {
                retColor = Color.FromArgb(0, 215, 255);
            }
            //255,255,220
            //WO-DGMSSVF--A--
            else if (symbolID.Equals("WO-DGMSSVF--A--"))
            {
                retColor = Color.FromArgb(255, 255, 220);
            }
            //255,255,140
            //WO-DGMSSF---A--
            else if (symbolID.Equals("WO-DGMSSF---A--"))
            {
                retColor = Color.FromArgb(255, 255, 140);
            }
            //255,235,0
            //WO-DGMSSM---A--
            else if (symbolID.Equals("WO-DGMSSM---A--"))
            {
                retColor = Color.FromArgb(255, 235, 0);
            }
            //255,215,0
            //WO-DGMSSC---A--
            else if (symbolID.Equals("WO-DGMSSC---A--"))
            {
                retColor = Color.FromArgb(255, 215, 0);
            }
            //255,180,0
            //WO-DGMSSVS--A--
            else if (symbolID.Equals("WO-DGMSSVS--A--"))
            {
                retColor = Color.FromArgb(255, 180, 0);
            }
            //200,255,105
            //WO-DGMSIC---A--
            else if (symbolID.Equals("WO-DGMSIC---A--"))
            {
                retColor = Color.FromArgb(200, 255, 105);
            }
            //100,130,255
            //WO-DGMSC----A--
            else if (symbolID.Equals("WO-DGMSC----A--"))
            {
                retColor = Color.FromArgb(100, 130, 255);
            }
            //255,77,0
            //WO-DOBVH----A--
            else if (symbolID.Equals("WO-DOBVH----A--"))
            {
                retColor = Color.FromArgb(255, 77, 0);
            }
            //255,128,0
            //WO-DOBVG----A--
            else if (symbolID.Equals("WO-DOBVG----A--"))
            {
                retColor = Color.FromArgb(255, 128, 0);
            }
            //255,204,0
            //WO-DOBVF----A--
            else if (symbolID.Equals("WO-DOBVF----A--"))
            {
                retColor = Color.FromArgb(255, 204, 0);
            }
            //204,255,26
            //WO-DOBVD----A--
            else if (symbolID.Equals("WO-DOBVD----A--"))
            {
                retColor = Color.FromArgb(204, 255, 26);
            }
            else
            {
                retColor = Color.Black;//0x000000;	// Black
            }

            return retColor;
        }


        /**
           * Only for single points at the moment
           * @param symbolID
           * @return 
           */
        public static Color getFillColorOfWeather(String symbolID)
        {
            if (symbolID.Equals("WOS-HPM-R-P----"))//landing ring - brown 148,48,0
                return Color.FromArgb(148, 48, 0);
            else if (symbolID.Equals("WOS-HPD---P----"))//dolphin facilities - brown
                return Color.FromArgb(148, 48, 0);
            else if (symbolID.Equals("WO-DHCB-----A--"))//
                return Color.FromArgb(249, 243, 241);
            else if (symbolID.Equals("WOS-HABB--P----"))//buoy default - 255,0,255
                return Color.FromArgb(255, 0, 255);//magenta
            else if (symbolID.Equals("WOS-HHRS--P----"))//rock submerged - 0,204,255
                return Color.FromArgb(0, 204, 255);//a type of blue
            else if (symbolID.Equals("WOS-HHDS--P----"))//snags/stumps - 0,204,255
                return Color.FromArgb(0, 204, 255);
            else if (symbolID.Equals("WOS-HHDWB-P----"))//wreck - 0,204,255
                return Color.FromArgb(0, 204, 255);
            else if (symbolID.Equals("WOS-TCCTG-P----"))//tide gauge - 210, 176, 106
                return Color.FromArgb(210, 176, 106);
            else if (symbolID.Equals("WO-DHCW-----A--"))//water
                return Color.FromArgb(255, 255, 255);
            else if (symbolID.Equals("WO-DHABP----A--") ||
                symbolID.Equals("WO-DHHD-----A--") ||
                symbolID.Equals("WO-DHHDD----A--") ||
                symbolID.Equals("WO-DMCC-----A--"))
            {
                return Color.FromArgb(0, 0, 255);
            }
            else if (symbolID.Equals("WO-DHPMD----A--"))//drydock
                return Color.FromArgb(188, 153, 58);
            else return Color.Empty;
        }

        /**
        *
        * @param hexValue - String representing hex value
        * (formatted "0xRRGGBB" i.e. "0xFFFFFF")
        * OR
        * formatted "0xAARRGGBB" i.e. "0x00FFFFFF" for a color with an alpha value
        * I will also put up with "RRGGBB" and "AARRGGBB" without the starting "0x"
        * @return
        */
        public static Color getColorFromHexString(String hexValue)
        {
            try
            {
                String hexOriginal = hexValue;

                String hexAlphabet = "0123456789ABCDEF";


                if (hexValue[0] == '#')
                    hexValue = hexValue.Substring(1);
                if (hexValue.Substring(0, 2) == ("0x") || hexValue.Substring(0, 2) == ("0X"))
                    hexValue = hexValue.Substring(2);

                hexValue = hexValue.ToUpper();

                int count = hexValue.Length;
                int[] value = null;
                int k = 0;
                int int1 = 0;
                int int2 = 0;

                if (count == 8 || count == 6)
                {
                    value = new int[(count / 2)];
                    for (int i = 0; i < count; i += 2)
                    {
                        int1 = hexAlphabet.IndexOf(hexValue[i]);
                        int2 = hexAlphabet.IndexOf(hexValue[i + 1]);
                        value[k] = (int1 * 16) + int2;
                        k++;
                    }

                    if (count == 8)
                    {

                        return Color.FromArgb(value[1], value[2], value[3], value[0]);
                    }
                    else if (count == 6)
                    {
                        return Color.FromArgb(value[0], value[1], value[2]);
                    }
                }
                else
                {
                    ErrorLogger.LogMessage("SymbolUtilties", "getColorFromHexString", "Bad hex value: " + hexOriginal);
                }
                return Color.Transparent;
            }
            catch (Exception exc)
            {
                ErrorLogger.LogException("SymbolUtilities", "GetColorFromHexString", exc);
            }
            return Color.Transparent;
        }


    }
}
