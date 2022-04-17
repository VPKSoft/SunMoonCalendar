using System;
using System.Drawing;

namespace SunMoonCalendar
{
    // based on the article https://www.daniweb.com/programming/software-development/code/454979/draw-phases-of-the-moon-on-a-form
    // No license terms were given.
    public class Moon
    {
        private const float ArcLength = 180; //from north to south pole
        private float penthickness = 2f;    //2 to get a better overlap of pixels

        public int DrawSize { get; set; }

        public Point DrawPosition { get; set; }

        public Moon()
        {
            DrawSize = 200; //default values
            DrawPosition = new Point(0, 0);
        }

        public Moon(int drawsize)
        {
            DrawSize = drawsize;
            if (drawsize < 50) penthickness = 1f;
            else penthickness = 2f;
            DrawPosition = new Point(0, 0); //drawsize / 2, drawsize / 2);
        }

        public void DrawMoonPhase(Graphics g, float angle, double phase, Bitmap moonImage = null)
        {
            bool blackcoloring = true;
            Brush br;
            Pen pen = new Pen(Color.Black, penthickness);

            if (moonImage != null)
            {
                g.DrawImage(moonImage, new Rectangle(new Point(0, 0), new Size(DrawSize, DrawSize)));
            }

            int moonphase = GetMoonPhasenumber(phase, out blackcoloring);
            if (blackcoloring) // reverse color depending on what phase we are in
            {
                if (moonImage != null)
                {
                    br = Brushes.Black; pen.Color = Color.Transparent;
                }
                else
                {
                    br = Brushes.Black; pen.Color = Color.Wheat;
                }
            }
            else
            {
                if (moonImage != null)
                {
                    br = Brushes.Transparent; pen.Color = Color.Black;
                }
                else
                {
                    br = Brushes.Wheat; pen.Color = Color.Black;
                }
            }
            g.FillEllipse(br, DrawPosition.X, DrawPosition.Y, DrawSize, DrawSize);
            if (moonphase <= DrawSize / 2)
            {
                DrawRightMoonPart(g, angle, pen, moonphase);
            }
            else
            {
                DrawRightMoonPart(g, angle, pen, DrawSize / 2);
                DrawLeftMoonPart(g, angle, pen, moonphase - DrawSize / 2);
            }
        }

        private void DrawLeftMoonPart(Graphics g, float angle, Pen P, int moonphase)
        {
            for (int i = 0; i < moonphase; i++)
            {
                // Increase rect size to draw arc in
                g.DrawArc(P,
                    new RectangleF(DrawPosition.X + DrawSize / 2 - i, DrawPosition.Y, 0.1f + 2 * i, DrawSize), angle + ArcLength, ArcLength);
            }
        }

        private void DrawRightMoonPart(Graphics g, float angle, Pen P, int moonphase)
        {
            for (int i = 0; i < moonphase; i++)
            {
                // Decrease rect size to draw arc in
                g.DrawArc(P, 
                    new RectangleF(DrawPosition.X + i, DrawPosition.Y, DrawSize - 2 * i, DrawSize), angle, ArcLength);
            }
        }

        //return the number of pixels, to indicate sunlight on the moon
        private int GetMoonPhasenumber(double phase, out Boolean colorblack)
        {
            colorblack = phase <= 0.5;
            if (phase >= 0.5)
            {
                phase -= 0.5;            
            }
            phase *= 2.0;
            return (int)((double)DrawSize * phase);
        }
    }
}
