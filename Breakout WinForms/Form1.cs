using g = Breakout_WinForms.Globals;

namespace Breakout_WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true; // This is so that the screen doesn't flicker each time onPaint is called
            this.BackColor = Color.Black;

            #region Var Init
            g.ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            g.ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

            this.Height = g.ScreenHeight;
            this.Width = g.ScreenWidth;

            g.BrickRatio = g.BRICK_WIDTH / g.BRICK_HEIGHT;

            // Get all System.Drawing Colors
            KnownColor[] values = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor kc in values)
            {
                var color = Color.FromKnownColor(kc);
                if (color.R != BackColor.R && color.G != BackColor.G && color.B != BackColor.B && color.ToArgb != BackColor.ToArgb)
                {
                    g.colors.Add(Color.FromKnownColor(kc));
                }
            }

            // Set gameTimers (fps timer) tick rate
            gameTimer.Interval = 1000 / g.fps;

            // Set ball position
            //g.ball = new Ball(new Rectangle(g.ScreenWidth / 2, g.ScreenHeight - 100, g.BALL_SIZE, g.BALL_SIZE), true, true);
            g.ball = new Ball(new Rectangle(g.ScreenWidth / 2, 100, g.BALL_SIZE, g.BALL_SIZE), true, true);

            // Resize brickPanel to fit screen size
            g.brickPanel.Width = g.ScreenWidth - (2 * g.BRICK_PANEL_PADDING);
            g.brickPanel.Height = g.ScreenHeight - g.BRICK_PANEL_PADDING - g.BRICK_PANEL_PADDING_BOTTOM;
            g.brickPanel.Location = new Point(g.BRICK_PANEL_PADDING, g.BRICK_PANEL_PADDING);

            // Fill brick list
            Random rand = new Random();
            Rectangle brickRect = new Rectangle();
            brickRect.Width = g.BRICK_WIDTH;
            brickRect.Height = g.BRICK_HEIGHT;

            // TODO: fill from 2d array for custom levels
            for (int column = 0; column <= g.brickPanel.Height / g.BRICK_HEIGHT; column++)
            {
                for (int row = 0; row < g.brickPanel.Width / g.BRICK_WIDTH; row++)
                {
                    brickRect.X = row * g.BRICK_WIDTH + g.BRICK_PANEL_PADDING;
                    brickRect.Y = column * g.BRICK_HEIGHT + g.BRICK_PANEL_PADDING;

                    Brick b = new Brick(brickRect, g.colors[rand.Next(0, g.colors.Count)], true);
                    g.bricks.Add(b);
                }
            }

            // Enable timer (and also collision check) only at the end 
            gameTimer.Enabled = true;
            #endregion
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // If there is an image and it has a location, 
            // paint it when the Form is repainted.
            base.OnPaint(e);

            // Draw Ball
            g.ball.Draw(e.Graphics);


            // Draw all the bricks
            foreach (var brick in g.bricks)
            {
                brick.Draw(e.Graphics);
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            this.Text = $"Breakout: ~{1000 / gameTimer.Interval}FPS";

            g.ball.CheckForWallCollision();

            //Collision, Parallel means that the load is spread on all the cpu cores meaning that when you have 100000 bricks for example, it can still hold up
            Parallel.ForEach(g.bricks, brick =>
            {
                if (g.ball.Rect.IntersectsWith(brick.Rect))
                {
                    g.bricksToRemove.Add(brick);
                }
            });

            if (g.bricksToRemove.Count > 0)
            {
                if (g.IgnoreBrickCollision == false)
                {
                    if (g.bricksToRemove[0].HasCollision)
                    {
                        g.ball.OnCollision(g.bricksToRemove[0]);
                    }
                }
            }

            // Remove collided bricks
            foreach (var brickToRemove in g.bricksToRemove)
            {
                g.bricks.Remove(brickToRemove);
            }
            g.bricksToRemove.Clear();

            // Move ball 
            g.ball.Move(g.velocity);

            // Repaint screen
            Invalidate();

            if (g.bricks.Count == 0)
            {
                Application.Exit();
            }
        }
    }
}