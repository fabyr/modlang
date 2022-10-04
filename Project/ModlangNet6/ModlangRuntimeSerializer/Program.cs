using Modlang.Abstract;
using Modlang.Exceptions;
using Modlang.Lexing;
using Modlang.Parsing;
using Modlang.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModlangRuntimeSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            Console.WriteLine("---");
            Console.WriteLine($"--- Modlang Serializer for Expression-Files (mdls)");
            Console.WriteLine("--- Modlang v1 by fabyr");
            Console.WriteLine("---");
            Console.WriteLine();
#if !DEBUG
            if(args.Length != 3)
            {
                Console.WriteLine("Invalid argument count.");
                Console.WriteLine("Syntax: {0} s|d <InFile> <OutFile>", Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));
            } else
#endif
            {
                string mode, inFile, outFile;

#if DEBUG
                mode = "s";
                inFile = "bigint.mdl";
                outFile = "bigint.mdls";
#else
                mode = args[0];
                inFile = args[1];
                outFile = args[2];
#endif

                switch (mode)
                {
                    case "s":
                        {
                            try
                            {
                                using (FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                                {
                                    try
                                    {
                                        Lexer lex = new Lexer(fs);
                                        LexerResult lr = lex.Lex();
                                        Parser par = new Parser(lr);
                                        ParseResult pr = par.Parse();
                                        Expression[] expressions = pr.ToArray();
                                        MdlsFile.Write(outFile, expressions);
                                        Console.WriteLine($"Sucessfully serialized to {outFile}");
                                    } catch (ModlangException ex)
                                    {
                                        Console.WriteLine($"Error: {ex}");
                                        string site;
                                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                                            site = ex.Origin.GetSiteFromStream(sr);
                                        Console.WriteLine();
                                        Console.WriteLine("\t" + site.Replace("\n", "\n\t"));
                                    }
                                }
                            } catch(IOException ex)
                            {
                                Console.WriteLine($"File Error: {ex}");
                            } catch(Exception ex)
                            {
                                Console.WriteLine($"Fatal Error: {ex}");
                            }
                        } break;
                    case "d": 
                        {
                            Expression[] expressions = MdlsFile.Read(inFile);
                            ParseResult pr = new ParseResult();
                            foreach (Expression exp in expressions)
                                pr.Add(exp);
                            string str = pr.BuildCodeString();
                            File.WriteAllText(outFile, str);
                            Console.WriteLine($"Sucessfully deserialized to {outFile}");
                            //Console.WriteLine("Deserialization to Code is not yet supported. :/");
                        } break;
                    default:
                        Console.WriteLine($"Invalid mode '{mode}'.");
                        Console.WriteLine("Use 's' for serialization and 'd' for deserialization.");
                        break;
                }
            }
            Console.WriteLine();
            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
