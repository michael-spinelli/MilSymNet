using System;
using System.Collections.Generic;

namespace MilSymNetUtilities
{
    public class ModifiersTG
    {
        //public const int SYMBOL_ID = "Symbol ID";
        //public const int SOURCE = "Source";
        //public const int EDITOR_CLASS_TYPE = "Editor Class Type";
        //public const int URN = "URN";
        //public const int UIC = "UIC";
        //public const int ANGLE_OF_ROTATION = "Angle of Rotation";
        /**
         * The innermost part of a symbol that represents a warfighting object
         * Here for completeness, not actually used as this comes from the
         * symbol code.
         * SIDC positions 3, 5-104
         * TG: P,L,A,BL,N,B/C
         * Length: G
         */
        public const int A_SYMBOL_ICON = 0;
        /**
         * The basic graphic (see 5.5.1).
         * We feed off of the symbol code so this isn't used
         * SIDC positions 11 and 12
         * TG: L,A,BL
         * Length: G
         */
        public const int B_ECHELON = 1;
        /**
         * A graphic modifier in a boundary graphic that
         * identifies command level (see 5.5.2.2, table V, and
         * figures 10 and 12).
         * TG: N
         * Length: 6
         */
        public const int C_QUANTITY = 2;
        /**
         * A text modifier for tactical graphics; content is
         * implementation specific.
         * TG: P,L,A,N,B/C
         * Length: 20
         */
        public const int H_ADDITIONAL_INFO_1 = 3;
        /**
         * A text modifier for tactical graphics; content is
         * implementation specific.
         * TG: P,L,A,N,B/C
         * Length: 20
         */
        public const int H1_ADDITIONAL_INFO_2 = 4;
        /**
         * A text modifier for tactical graphics; content is
         * implementation specific.
         * TG: P,L,A,N,B/C
         * Length: 20
         */
        public const int H2_ADDITIONAL_INFO_3 = 5;
        /**
         * A text modifier for tactical graphics; letters "ENY" denote hostile symbols.
         * TG: P,L,A,BL,N,B/C
         * Length: 3
         */
        public const int N_HOSTILE = 6;
        /**
         * A graphic modifier for CBRN events that
         * identifies the direction of movement (see 5.5.2.1
         * and figure 11).
         * TG: N,B/C
         * Length: G
         */
        public const int Q_DIRECTION_OF_MOVEMENT = 7;
        /**
         * A graphic modifier for points and CBRN events
         * used when placing an object away from its actual
         * location (see 5.5.2.3 and figures 10, 11, and 12).
         * TG: P,N,B/C
         * Length: G
         */
        public const int S_OFFSET_INDICATOR = 8;
        /**
         * A text modifier that uniquely identifies a particular
         * tactical graphic; track number.
         * Nuclear: delivery unit (missile, aircraft, satellite,
         * etc.)
         * TG:P,L,A,BL,N,B/C
         * Length: 15 (35 for BL)
         */
        public const int T_UNIQUE_DESIGNATION_1 = 9;
        /**
         * A text modifier that uniquely identifies a particular
         * tactical graphic; track number.
         * Nuclear: delivery unit (missile, aircraft, satellite,
         * etc.)
         * TG:P,L,A,BL,N,B/C
         * Length: 15 (35 for BL)
         */
        public const int T1_UNIQUE_DESIGNATION_2 = 10;
        /**
         * A text modifier that indicates nuclear weapon type.
         * TG: N
         * Length: 20
         */
        public const int V_EQUIP_TYPE = 11;
        /**
         * A text modifier for units, equipment, and installations that displays DTG format:
         * DDHHMMSSZMONYYYY or â€œO/Oâ€� for on order (see 5.5.2.6).
         * TG:P,L,A,N,B/C
         * Length: 16
         */
        public const int W_DTG_1 = 12;
        /**
         * A text modifier for units, equipment, and installations that displays DTG format:
         * DDHHMMSSZMONYYYY or â€œO/Oâ€� for on order (see 5.5.2.6).
         * TG:P,L,A,N,B/C
         * Length: 16
         */
        public const int W1_DTG_2 = 13;
        /**
         * A text modifier that displays the minimum,
         * maximum, and/or specific altitude (in feet or
         * meters in relation to a reference datum), flight
         * level, or depth (for submerged objects in feet
         * below sea level). See 5.5.2.5 for content.
         * TG:P,L,A,N,B/C
         * Length: 14
         */
        public const int X_ALTITUDE_DEPTH = 14;
        /**
         * A text modifier that displays a graphicâ€™s location
         * in degrees, minutes, and seconds (or in UTM or
         * other applicable display format).
         *  Conforms to decimal
         *  degrees format:
         *  xx.dddddhyyy.dddddh
         *  where
         *  xx = degrees latitude
         *  yyy = degrees longitude
         *  .ddddd = decimal degrees
         *  h = direction (N, E, S, W)
         * TG:P,L,A,BL,N,B/C
         * Length: 19
         */
        public const int Y_LOCATION = 15;

        /**
         * For Tactical Graphics
         * A numeric modifier that displays a minimum,
         * maximum, or a specific distance (range, radius,
         * width, length, etc.), in meters.
         * 0 - 999,999 meters
         * TG: P.L.A
         * Length: 6
         */
        public const int AM_DISTANCE = 16;
        /**
         * For Tactical Graphics
         * A numeric modifier that displays an angle
         * measured from true north to any other line in
         * degrees.
         * 0 - 359 degrees
         * TG: P.L.A
         * Length: 3
         */
        public const int AN_AZIMUTH = 17;

        public const int SYMBOL_FILL_IDS = 90;


        public const int LENGTH = 30;
        public const int WIDTH = 31;
        public const int RADIUS = 32;
        public const int ANGLE = 33;
        //public const int SEGMENT_DATA = "Segment Data";



        /**
         * Returns an Arraylist of the modifer names for tactical graphics
         * @return
         */
        public static List<int> GetModifierList()
        {
            List<int> list = new List<int>();

            //list.Add(ModifierType.A_SYMBOL_ICON);//graphical, feeds off of symbol code
            //list.Add(ModifierType.B_ECHELON);//graphical, feeds off of symbol code
            list.Add(C_QUANTITY);
            list.Add(H_ADDITIONAL_INFO_1);
            list.Add(H1_ADDITIONAL_INFO_2);
            list.Add(H2_ADDITIONAL_INFO_3);
            list.Add(N_HOSTILE);
            list.Add(Q_DIRECTION_OF_MOVEMENT);
            list.Add(T_UNIQUE_DESIGNATION_1);
            list.Add(T1_UNIQUE_DESIGNATION_2);
            list.Add(V_EQUIP_TYPE);
            list.Add(W_DTG_1);
            list.Add(W1_DTG_2);
            list.Add(X_ALTITUDE_DEPTH);
            list.Add(Y_LOCATION);

            list.Add(AM_DISTANCE);//2525C
            //list.Add(AM1_DISTANCE);//2525C
            list.Add(AN_AZIMUTH);//2525C
            //list.Add(AN1_AZIMUTH);//2525C

            //back compat
            list.Add(LENGTH);
            list.Add(WIDTH);
            list.Add(RADIUS);
            list.Add(ANGLE);



            return list;
        }

        /**
         *
         * @param modifier like ModifiersTG.C_QUANTITY
         * @return modifier name based on mofidier constants
         */
        public static String getModifierName(int modifier)
        {
            switch (modifier)
            {
                //case A_SYMBOL_ICON:
                //    return "Symbol Icon";
                case B_ECHELON:
                    return "Echelon";
                case C_QUANTITY:
                    return "Quantity";
                case H_ADDITIONAL_INFO_1:
                    return "Additional Info 1";
                case H1_ADDITIONAL_INFO_2:
                    return "Additional Info 2";
                case H2_ADDITIONAL_INFO_3:
                    return "Additional Info 3";
                case N_HOSTILE:
                    return "Hostile";
                case Q_DIRECTION_OF_MOVEMENT:
                    return "Direction of Movement";
                //case S_OFFSET_INDICATOR:
                //    return "Offset Indicator";
                case T_UNIQUE_DESIGNATION_1:
                    return "Unique Designation 1";
                case T1_UNIQUE_DESIGNATION_2:
                    return "Unique Designation 2";
                case V_EQUIP_TYPE:
                    return "Equipment Type";
                case W_DTG_1:
                    return "Date Time Group 1";
                case W1_DTG_2:
                    return "Date Time Group 2";
                case X_ALTITUDE_DEPTH:
                    return "Altitude Depth";
                case Y_LOCATION:
                    return "Location";
                case AM_DISTANCE:
                    return "Distance";
                case AN_AZIMUTH:
                    return "Azimuth";
                default:
                    return "";

            }
        }

        /**
 *
 * @param modifier like ModifiersTG.C_QUANTITY
 * @return modifier name based on mofidier constants
 */
        public static String getModifierLetterCode(int modifier)
        {
            switch (modifier)
            {
                //case A_SYMBOL_ICON:
                //    return "Symbol Icon";
                case B_ECHELON:
                    return "B";
                case C_QUANTITY:
                    return "C";
                case H_ADDITIONAL_INFO_1:
                    return "H";
                case H1_ADDITIONAL_INFO_2:
                    return "H1";
                case H2_ADDITIONAL_INFO_3:
                    return "H2";
                case N_HOSTILE:
                    return "N";
                case Q_DIRECTION_OF_MOVEMENT:
                    return "Q";
                //case S_OFFSET_INDICATOR:
                //    return "Offset Indicator";
                case T_UNIQUE_DESIGNATION_1:
                    return "T";
                case T1_UNIQUE_DESIGNATION_2:
                    return "T1";
                case V_EQUIP_TYPE:
                    return "V";
                case W_DTG_1:
                    return "W";
                case W1_DTG_2:
                    return "W1";
                case X_ALTITUDE_DEPTH:
                    return "X";
                case Y_LOCATION:
                    return "Y";
                case AM_DISTANCE:
                    return "AM";
                case AN_AZIMUTH:
                    return "AN";
                default:
                    return "";

            }
        }
    }
}
