using Modlang;
using Modlang.Abstract;
using Modlang.Abstract.ControlStructures;
using Modlang.CommonUtilities;
using Modlang.Exceptions;
using Modlang.Lexing;
using Modlang.Parsing;
using Modlang.Runtime.CoreLib;
using Modlang.Runtime.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModlangInterpreter
{
    class Program
    {
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
            Console.WriteLine($"Build: {dbgStr}, {platform}{(nat ? ", ILCompiled (AOT)" : string.Empty)}");
            Console.WriteLine("Press ENTER to start...");
            //Console.ReadLine();
            //MandelCsTest.MandelCsTest.MANDEL_PROGRAM();
            Console.ReadLine();
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            //long start = 0, end = 0;
            const string filename = @"Programs/million.mdl";
            Modlang.Runtime.Environment env = new Modlang.Runtime.Environment();
            env.Init(EnvironmentInitializer.GetDefault(@"core.mdl"));
            //env.ExecutionOptions.PassOutMlNull = false;
            //env.ExecutionOptions.NoTypeAssert = true;
            //env.ExecutionOptions.InitializeArraysWithMlNull = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                sw.Stop();
                Console.WriteLine($"File Opening... ({sw.Elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond}ms)");
                try
                {
                    sw.Restart();
                    Lexer lex = new Lexer(fs);
                    lex.LineBreakMode = Lexer.NewlineMode.LF; // LINUX
                    LexerResult lr = lex.Lex();
                    Parser par = new Parser(lr);
                    ParseResult pr = par.Parse();
                    sw.Stop();
                    Console.WriteLine($"Lexing & Parsing... ({sw.Elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond}ms)");
                    sw.Restart();
                    env.Execute(pr);
                    sw.Stop();
                } catch (ModlangException ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    string site;
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        site = ex.Origin.GetSiteFromStream(sr);
                    Console.WriteLine();
                    Console.WriteLine("\t" + site.Replace("\n", "\n\t"));
                } catch (Exception ex)
                {
                    Console.WriteLine($"Fatal Error: {ex}");
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Done... ({sw.Elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond}ms)");
            Console.ReadLine();
        }
    }
}
