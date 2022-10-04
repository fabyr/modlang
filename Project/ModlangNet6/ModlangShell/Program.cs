using Modlang.Abstract;
using Modlang.Exceptions;
using Modlang.Lexing;
using Modlang.Parsing;
using Modlang.Runtime;
using Modlang.Runtime.CoreLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModlangShell
{
    class Program
    {
        private static StringBuilder _stringBuffer = new StringBuilder();
        private static List<Token> _lr = new List<Token>();
        private static Modlang.Runtime.Environment _env;

        private static int _clBrCounter = 0;

        private static bool _printAll = false;

        static void ErrWriteLine(string value = "")
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Error.WriteLine(value);

            Console.ForegroundColor = originalColor;
        }

        static void Main(string[] args)
        {
            bool nat = false;
#if A_ILC
            nat = true;
#endif

            const string dbgStr =
#if DEBUG
                    "Debug"
#else
                    "Release"
#endif
                    ;
#if A_ACPU
            const string platform = "Any CPU";
#elif A_X64
            const string platform = "x64";
#else
            const string platform = "Undefined";
#endif

            _printAll = args.Length > 0 && args[0] == "--printall"; // TODO: Use a proper argument parser

            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.WriteLine("---");
            Console.WriteLine($"--- Modlang Shell ({dbgStr}, {platform}{(nat ? ", ILCompiled (AOT)" : string.Empty)})");
            Console.WriteLine("--- Modlang v1 by fabyr");
            Console.WriteLine("---");
            Console.WriteLine("---");
            if(_printAll)
                Console.WriteLine("--- printall is enabled");
            else
                Console.WriteLine("--- Use '--printall' to print the returned value of every expression");
            Console.WriteLine("---");
            Console.WriteLine();

            {
                _env = new Modlang.Runtime.Environment();
                _env.Init(EnvironmentInitializer.GetDefault(@"core.mdl"));
                _env.ExecutionOptions.PassOutMlNull = true;
                _env.ExecutionOptions.InitializeArraysWithMlNull = true;
            }

            string input;
            bool exit;
            do
            {
                Console.Write(">>> ");
                Console.Write("".PadLeft(_clBrCounter, '\t'));
                input = Console.ReadLine();
                if(!(exit = input == "exit"))
                {
                    if (Execute(input + "\r\n"))
                        Console.WriteLine();    
                }
            } while (!exit);
        }

        private static bool Execute(string input)
        {
            const string conInFn = "__ConsoleIn";

            LexerResult lr;
            try
            {
                Lexer lx = new Lexer(input, Encoding.UTF8)
                {
                    FileName = conInFn
                };
                lr = lx.Lex();
                _lr.AddRange(lr);
                _stringBuffer.Append(input);

                foreach (Token t in lr)
                {
                    if (t.Kind == TokenType.CURLY_LBRACE)
                        _clBrCounter++;
                    if (t.Kind == TokenType.CURLY_RBRACE)
                        _clBrCounter--;
                }
            } catch (LexerException ex) 
            {
                ErrWriteLine($"Error: {ex.Message}");
            }

            TokenType lastKind = _lr.Count == 0 ? TokenType.UNDEFINED : _lr[_lr.Count - 1].Kind;
            if (_clBrCounter == 0/* && (lastKind == TokenType.EOI || lastKind == TokenType.CURLY_RBRACE)*/)
            {
                try
                {
                    // if the user forgets the ;
                    // we can just add it since we are in shell
                    if (lastKind != TokenType.EOI)
                        _lr.Add(new Token("", 0, 0, 0, 0, "", TokenType.EOI));
                    Parser px = new Parser(_lr);
                    ParseResult pr = px.Parse();
                    //Console.WriteLine(pr.BuildDisplayString());
                    //if(false)
                    foreach(Expression exp in pr)
                    {
                        MlObject mlo = _env.SingleExecute(exp);
                        if (_printAll && mlo != null)
                        {
                            if (mlo.UnderlyingType.TypeStr == Constants.TYPE_NULL)
                            {
                                Console.WriteLine("<NULL>");
                            }
                            else
                            {
                                _env.EnterClassContext(mlo);
                                MlObjectCallable moc = _env.ResolveObjectPath(new IdentifierExpression(Modlang.Runtime.CoreLib.Constants.OBJ_FUNC_TOSTRING)).Value as MlObjectCallable;
                                MlObject str = Execution.Call(moc, _env);
                                _env.FindFunction(DefaultEnvironmentInitializer.FUNC_N_PRINT, true).Call(_env, str);
                                _env.ExitClassContext();
                                Console.WriteLine();
                            }
                        }
                    }
                } catch(ModlangException ex)
                {
                    ErrWriteLine($"\tError: {ex.Message}");
                    string site = "<File not found/inaccessible>";
                    if (ex.Origin == null || ex.Origin.Filename == conInFn)
                        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(_stringBuffer.ToString())))
                        using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
                            site = ex.Origin.GetSiteFromStream(sr);
                    else
                        try
                        {
                            ErrWriteLine($"\tIn file \"{ex.Origin.Filename}\" at line {ex.Origin.Line}");
                            using (FileStream fs = new FileStream(ex.Origin.Filename, FileMode.Open, FileAccess.Read))
                            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                                site = ex.Origin.GetSiteFromStream(sr);
                        } catch { }
                    ErrWriteLine();
                    ErrWriteLine("\t" + site.Replace("\n", "\n\t"));
                } catch (Exception ex)
                {
                    ErrWriteLine($"\tFatal Error: {ex.ToString().Replace("\n", "\n\n")}");
                } finally
                {
                    try
                    {
                        _env.ReturnToGlobalScope();
                    } catch { }
                }

                _lr.Clear();
                _stringBuffer.Clear();
                return true;
            }
            return false;
        }
    }
}
