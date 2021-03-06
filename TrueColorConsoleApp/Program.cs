﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TrueColorConsole;
using TrueColorConsole.Styled;

namespace TrueColorConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
			if (!VTConsole.IsSupported)
				throw new NotSupportedException();

			VTConsole.Enable();

			Example4();

            Console.ReadKey();
        }
        private static void Example4() {
            var st = new StyleSheet(Color.Orange);
            st.AddStyle(new FormatRule(new Regex("hope.+SWEET"), new []{VTFormat.Underline }, new [] {VTFormat.NoUnderline }));
            st.AddStyle(new StyleRule(new Regex("are .+"), Color.Transparent, null, Color.Red));
            st.AddStyle(new StyleRule(new Regex(@"Happy.+going"),Color.Lime,(s)=>"Zanay how BOW how howdy"));
            st.AddStyle(new StyleRule(new Regex("how"), Color.Blue));
            VTConsole.WriteLineStyled("I hope you are Happy with how its going???? yah **SWEET**",st);

		}
        private static int Example1()
        {
            var width = 80;
            var height = 25;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);

            var cx = Console.WindowWidth;
            var cy = Console.WindowHeight;
            for (var y = 0; y < cy; y++)
            for (var x = 0; x < cx; x++)
            {
                var r = (int) ((float) x / cx * 255);
                var g = (int) ((float) y / cy * 255);
                var b = (int) (1.0f * 255);
                var value = $"{(y * cx + x) % 10}";
                VTConsole.Write(value, Color.Black, Color.FromArgb(r, g, b));
            }
            return cy;
        }

        private static void Example2()
        {
            for (var i = 0; i < Console.WindowHeight / 4; i++)
            {
                Sleep(50);
                VTConsole.ScrollUp();
            }

            VTConsole.WriteLine("Disabling cursor blinking", Color.White, Color.Red);
            VTConsole.CursorSetBlinking(false);
            Sleep();

            VTConsole.WriteLine("Enabling cursor blinking", Color.White, Color.Green);
            VTConsole.CursorSetBlinking(true);
            Sleep();

            VTConsole.SetColorBackground(Color.White);
            VTConsole.WriteLine("Hiding cursor", Color.DeepPink);
            VTConsole.CursorSetVisibility(false);
            Sleep();

            VTConsole.WriteLine("Showing cursor", Color.DeepSkyBlue);
            VTConsole.CursorSetVisibility(true);
            Sleep();

            VTConsole.WriteLine();
            VTConsole.SetFormat(VTFormat.Underline, VTFormat.Negative);
            VTConsole.WriteLine("Press a key to exit !!!", Color.White, Color.Red);
        }

        private static void Example3()
        {
            var plasma = new Plasma(256, 256);
            Console.Title = "Plasma !";
            Console.CursorVisible = false;

            var width = Console.WindowWidth;
            var height = Console.WindowHeight;
            var builder = new StringBuilder(width * height * 22);
            var resetEvent = new AutoResetEvent(true);

            using(new Timer(x =>{resetEvent.Set();}, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1.0/20*1000)))
            for (var frame = 0; ; frame++)
            {
                if (width != Console.WindowWidth || height != Console.WindowHeight)
                {
                    width = Console.WindowWidth;
                    height = Console.WindowHeight;
                    Console.WriteLine();
                    builder = new StringBuilder(width * height * 22);
                }
                else
                {
                    builder.Clear();
                }
                plasma.PlasmaFrame(frame);
                
                resetEvent.WaitOne();
                
                for (var i = 0; i < width * height; i++)
                {
                    var x1 = i % width;
                    var y1 = i / width;
                    var i1 = y1 * plasma.SizeX + x1;
                    var b = plasma.Screen[i1];

                    var cr = plasma.ColR[b] >> 4;
                    var cg = plasma.ColG[b] >> 4;
                    var cb = plasma.ColB[b] >> 4;
                    var str = VTConsole.GetColorBackgroundString(cr, cg, cb);
                    builder.Append(str);
                    builder.Append(' ');
                }
                var bytes = Encoding.ASCII.GetBytes(builder.ToString());
                VTConsole.WriteFast(bytes);
            }
        }
        
        private static void Sleep(int millisecondsTimeout = 2000)
        {
            Thread.Sleep(millisecondsTimeout);
        }
    }
}