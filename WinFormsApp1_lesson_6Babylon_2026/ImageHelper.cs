using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1_lesson_6Babylon_2026;

public static class ImageHelper
{
    public static Image GetDefaultImage(string label = "NA")
    {
        Bitmap bmp = new Bitmap(100, 100);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.Clear(Color.FromArgb(30, 30, 30));

            using (Pen copperPen = new Pen(Color.FromArgb(184, 115, 51), 4))
            {
                g.DrawRectangle(copperPen, 5, 5, 90, 90);
            }

            using (Font font = new Font("Consolas", 14, FontStyle.Bold))
            {
                string text = label.Length > 4 ? label.Substring(0, 4) : label;

                SizeF textSize = g.MeasureString(text, font);
                float x = (100 - textSize.Width) / 2;
                float y = (100 - textSize.Height) / 2;

                g.DrawString(text, font, Brushes.Gray, x, y);
            }
        }

        return bmp;
    }
}

