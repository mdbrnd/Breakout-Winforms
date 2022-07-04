using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Breakout_WinForms
{
    internal class Brick
    {
        public Color Color;
        public Rectangle Rect;
        public bool HasCollision;

        public Brick(Rectangle rect, Color color, bool collEnabled = true)
        {
            Rect = rect;
            Color = color;
            HasCollision = collEnabled;
        }

        public void Draw(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color);
            g.FillRectangle(b, Rect);
        }
    }
}
