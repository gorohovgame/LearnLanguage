using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn
{
    public class FontWizard
    {
        public static Font FlexFont(Graphics g, float minFontSize, float maxFontSize, Size layoutSize, string s, Font f, out SizeF extent)
        {
            if (maxFontSize == minFontSize)
                f = new Font(f.FontFamily, minFontSize, f.Style);

            extent = g.MeasureString(s, f);

            if (maxFontSize <= minFontSize)
                return f;

            float hRatio = layoutSize.Height / extent.Height;
            float wRatio = layoutSize.Width / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            float newSize = f.Size * ratio;

            if (newSize < minFontSize)
                newSize = minFontSize;
            else if (newSize > maxFontSize)
                newSize = maxFontSize;

            f = new Font(f.FontFamily, newSize, f.Style);
            extent = g.MeasureString(s, f);

            return f;
        }

        public static void OnPaint(object sender, PaintEventArgs e, string text, Brush brush, Boolean bold = true)
        {
            var control = sender as Control;
            if (control == null)
                return;

            control.Text = string.Empty;    //delete old stuff
            var rectangle = control.ClientRectangle;

            using (Font f = new System.Drawing.Font("Bookman Old Style", 20.25f, bold ? FontStyle.Bold : FontStyle.Regular))
            {
                SizeF size;
                using (Font f2 = FontWizard.FlexFont(e.Graphics, 5, 50, rectangle.Size, text, f, out size))
                {
                    e.Graphics.Clear(SystemColors.Control);
                    PointF p = new PointF((rectangle.Width - size.Width) / 2, (rectangle.Height - size.Height) / 2);
                    e.Graphics.DrawString(text, f2, brush, p);
                }
            }
        }
    }
}
