# modlang
This is just a fun project of mine where I create my own programming language.

The syntax and look is completely stolen from C#...
I'm sorry i have no imagination 😢

It is by no means polished or finished and there are a lot of errors and edge cases.
The code is at some points also really dirty. I did not really do any sort of planning.

I just put it on github to have easy access to it. Private repo is unideal in my case.

## Brief summary
It is a C#-looking interpreted scripting language. It is statically typed and follows the Object Oriented Paradigm.
It is not purely interpreted, rather the entire code is lexed and parsed first into easier to handle memory structures.

It is written in C# itself. Since recently the project was switched from .NET Core 3.1 to .NET 6

The Solution-Folder is at [/Project/ModlangNet6](/Project/ModlangNet6/).

There are 3 executable programs:
- ModlangShell
  * Gives you a shell interface like most scripting languages have
- ModlangInterpreter
  * Interprets a file
- ModlangRuntimeSerializer
  * Allows you to store the Abstract Syntax Tree in a binary format.

The other projects are the core of functionality. They are also split up:
- Modlang
  * Contains the code responsible for Lexing and Parsing.
- Modlang.CommunUtilities
  * Minor shared utilities
- Modlang.Runtime
  * Contains the code responsible for executing any parsed program.
- Modlang.Runtime.Serialization
  * Backbone for the ModlangRuntimeSerializer

## Name
Why "modlang"? I originally wanted it to be extremely modifyable from within itself. I.e. syntax customization and modification. The only thing that has turned out to be customizable are operators.
Inside a class you can define an operator method with any character which is not alphanumeric or reserved. They can be postfix-unary, prefix-unary or binary operators.

This made for some pretty complicated and sloppish parsing code. 
The normal operators (such as +, -, *, /) have all predefined precedences.
The user defined operators always have the lowest precedence and come last.

### Standard library
I have created a small standard library in my programming language.
It contains the basic types and core functions such as:
- console output and input
- math operations
- array operations
- base64
- big integer arithmetic (It was directly ported from a C# project of somebody else)
  * [CodeProject Link](https://www.codeproject.com/Articles/60108/BigInteger-Library-2)
- unfinished file operations and bytestreams

## Some code examples
Mandelbrot renderer
```
const int maxIter = 30;

public int Mandel(double x, double y)
{
	double zre = 0, zim = 0, zretmp;
	int i;
	for(i = 0; i < maxIter; i ++)
	{
		zretmp = zre;
		zre = (zre * zre) - (zim * zim);
		zim = 2 * zretmp * zim;
		
		zre += x;
		zim += y;
		
		if(zre * zre + zim * zim > 4)
		{
			break;
		}
	}
	if(i == maxIter)
		return -1;
	return i;
}

public double Map(double x, double in_min, double in_max, double out_min, double out_max)
{
  return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}

void MANDEL()
{
	double x = -2.1, y = 1, w = 2.6, h = -2;
	double xm, ym;
	
	const int cw = 70, ch = 24;
	for(int cy = 0; cy < ch; cy ++)
	{
		for(int cx = 0; cx < cw; cx ++)
		{
			xm = Map(cx, 0, cw, x, x + w);
			ym = Map(cy, 0, ch, y, y + h);
			
			int r = Mandel(xm, ym);
			if(r == -1)
			{
				print("$");
			}
			else
			{
				print(".");
			}
		}
		println();
	}
}

MANDEL();
```

Byte Stream Test
```
bytestream s = new bytestream();

for(long i = 0; i < 120; i++)
{
	byte[] buf = new byte[10];
	for(int j = 0; j < buf.length; j++)
		buf[j] = (byte)('0' + (i % 10));
	s.write(buf, 0, buf.length);
	println("pos: " + s.position);
	println(new string((char[])buf));
}
println();
println("REWIND");
println();
s.position = 0;

for(long i = 0; i < 220; i++)
{
	byte[] buf = new byte[7];
	long r = s.read(buf, 0, buf.length);
	println("r: " + r + " pos: " + s.position);
	println(new string((char[])buf));
}
println();
println("DONE");
```