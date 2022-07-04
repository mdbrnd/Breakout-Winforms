using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout_WinForms
{
    internal static class Globals
    {
        public static List<Brick> bricks = new List<Brick>();
        public static List<Brick> bricksToRemove = new List<Brick>(); // Bricks that are removed on collision
        public static List<Color> colors = new List<Color>();
        public static Rectangle brickPanel;
        public static Ball ball = new Ball(Rectangle.Empty, true, true);
        public static Random rand = new Random();
        public static bool IgnoreBrickCollision = false;
        public static int BALL_SIZE = 256;
        public static int BRICK_WIDTH = 32 * 6;
        public static int BRICK_HEIGHT = 16 * 6;
        public static int BRICK_PANEL_PADDING = 0; // Padding for sides and top
        public static int BRICK_PANEL_PADDING_BOTTOM = 700;
        public static int BrickRatio;
        public static int velocity = 64;
        public static int fps = 1000;
        public static int ScreenWidth;
        public static int ScreenHeight;
    }
}
