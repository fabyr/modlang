using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang
{
    public static class Constants
    {
        public const string IdentifierChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_"; // Start of identifier musn't include digits
        public const string IdentifierCenterChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789"; // After the start there can be digits

        public const string HexCharset = "0123456789abcdefABCDEF";
        public const string NumberCenterCharset = HexCharset + "bBxX.ulULfdFDeE";
        public const string NumberCharset = "0123456789";

        public const string NonOperatorCharset = PeriodOpStr + AssignmentOperatorStr + LexNonOperatorCharset;

        public const string LexNonOperatorCharset = CommaOpStr + IdentifierCenterChars + HexCharset + "bBxXulULfdFDeE"
                                                    + "()[]{}\\" + EOISTR + ControlCharSet  + "\"\'";

        public const int UnicodeEscapeSquenceLength = 4;
        public const int UnicodeEscapeSquenceMaxLengthVariable = 4;
        public const int UnicodeEscapeSquenceLength32 = 8;

        public const char EOI = ';';
        public const string EOISTR = ";"; // So that i can use it in another constant value

        public const char AssignmentOperator = '=';
        public const string AssignmentOperatorStr = "=";

        public const char ColonOperator = ':';
        public const string ColonOperatorStr = ":";

        public const char CommaOp = ',';
        public const string CommaOpStr = ",";

        public const char PeriodOp = '.';
        public const string PeriodOpStr = ".";

        // This is used in the NonOperatorCharsets so that no operator can be any of those (ASCII Control Characters)
        public const string ControlCharSet = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x20";

        public const int MaxPrecedence = 16; // Has to be manually adjusted if new operators are "pre-defined" in Modlang.Util.cs; "GetPrecedence"-Method
        public const int CastPrecedence = 2;
    }
}
