using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SpectreExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToSpectreColor(this System.Drawing.Color color)
        {
            return new(color.R, color.G, color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IRenderable RenderBreakdown(this Color color)
        {
            int totalValue = color.R + color.G + color.B;
            return new BreakdownChart()
                .ShowPercentage()
                .FullSize()
                .AddItem("Red", color.R is not 0 ? (((double)color.R / (double)totalValue) * 100) : 0, Color.Red)
                .AddItem("Green", color.G is not 0 ? (((double)color.G / (double)totalValue) * 100) : 0, Color.Green)
                .AddItem("Blue", color.B is not 0 ? (((double)color.B / (double)totalValue) * 100) : 0, Color.Blue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IRenderable RenderCodes(this Color color)
        {
            string hexValue = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            uint uintValue = Convert.ToUInt32(hexValue, 16);
            return new Table()
                .HideHeaders()
                .NoBorder()
                .BorderColor(Color.Grey)
                .AddColumn("Type")
                .AddColumn("Value")
                .AddRow(
                    new Markup("RGB"),
                    new Markup($"{color.R}, {color.G}, {color.B}"))
                .AddRow(
                    new Markup("Hex"),
                    new Markup($"#{color.R:X2}{color.G:X2}{color.B:X2}"))
                .AddRow(
                    new Markup("UInt32"),
                    new Markup($"{uintValue}"));
        }
    }
}
