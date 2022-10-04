using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modlang.Lexing
{
    public enum TokenType
    {
        UNDEFINED = -1,
        KEYWORD_NEW,
        KEYWORD_STATIC,
        KEYWORD_CLASS,
        KEYWORD_THIS,

        KEYWORD_CONST,

        KEYWORD_IF,
        KEYWORD_WHILE,
        KEYWORD_DO,
        KEYWORD_FOR,
        KEYWORD_SWITCH,
        KEYWORD_CASE,
        KEYWORD_DEFAULT,
        KEYWORD_ELSE,

        KEYWORD_BREAK,
        KEYWORD_CONTINUE,

        KEYWORD_RETURN,

        KEYWORD_OPERATOR,
        KEYWORD_PREFIX,
        KEYWORD_POSTFIX,

        KEYWORD_PUBLIC,
        KEYWORD_PRIVATE,
        KEYWORD_PROTECTED,

        OPERATOR,
        OP_ASSIGN,
        OP_COMMA,
        OP_PERIOD,
        OP_COLON,

        LBRACE,
        RBRACE,
        CURLY_LBRACE,
        CURLY_RBRACE,
        SQUARE_LBRACE,
        SQUARE_RBRACE,

        IDENTIFIER,
        LITERAL_STRING,
        LITERAL_NUMBER,
        LITERAL_CHAR,

        KEYWORD_TRUE,
        KEYWORD_FALSE,

        KEYWORD_PRIMITIVE,

        KEYWORD_IMPLICIT,
        KEYWORD_EXPLICIT,

        KEYWORD_GET,
        KEYWORD_SET,

        EOI, // End of Instruction, ';'
    }
}
