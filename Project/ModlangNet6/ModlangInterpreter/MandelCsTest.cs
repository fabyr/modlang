using System;

namespace MandelCsTest
{
    public static class MandelCsTest
    {
		public static double Map(double x, double in_min, double in_max, double out_min, double out_max)
		{
			return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		public static void MANDEL_PROGRAM()
        {
			const int maxIter = 30;

			int Mandel(double x, double y)
			{
				double zre = 0, zim = 0, zretmp;
				int i;
				for (i = 0; i < maxIter; i++)
				{
					zretmp = zre;
					zre = (zre * zre) - (zim * zim);
					zim = 2 * zretmp * zim;

					zre += x;
					zim += y;

					if (zre * zre + zim * zim > 4)
					{
						break;
					}
				}
				if (i == maxIter)
					return -1;
				return i;
			}

			void MANDEL()
			{
				double x = -2.1, y = 1, w = 2.6, h = -2;
				double xm, ym;
				const int cw = 70, ch = 24;
				for (int cy = 0; cy < ch; cy++)
				{
					for (int cx = 0; cx < cw; cx++)
					{
						xm = Map(cx, 0, cw, x, x + w);
						ym = Map(cy, 0, ch, y, y + h);

						int r = Mandel(xm, ym);
						if (r == -1)
						{
							Console.Write("$");
						}
						else
						{
                            Console.Write(".");
						}
					}
                    Console.WriteLine();
				}
			}

			MANDEL();
		}
    }
}
