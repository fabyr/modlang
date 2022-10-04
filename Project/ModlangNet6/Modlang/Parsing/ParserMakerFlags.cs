using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Parsing
{
    public enum ParserMakerFlags : uint
    {
        NONE           = 0,
        STATIC_FLAG    =   0b00000001,
        CONST_FLAG     =   0b00000010,
        OPERATOR_FLAG  =   0b00000100,
        PREFIX_FLAG    =   0b00001000,
        POSTFIX_FLAG   =   0b00010000,
        IMPLICIT_FLAG  =   0b00100000,
        EXPLICIT_FLAG  =   0b01000000,
        PRIMITIVE_FLAG =   0b10000000,
        GET_FLAG       =  0b100000000,
        SET_FLAG       = 0b1000000000
    }
}
