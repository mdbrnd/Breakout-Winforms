using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g = Breakout_WinForms.Globals;

namespace Breakout_WinForms
{
    internal class Ball
    {
        public Color Color = Color.White;
        public Rectangle Rect { get; set; }
        public bool IsGoingLeft { get; set; } // Whether the ball is going left
        public bool IsGoingUp { get; set; } // Whether the ball is going up
        public Ball(Rectangle rect, bool isleft = false, bool isup = true)
        {
            Rect = rect;
            IsGoingLeft = isleft;
            IsGoingUp = isup;
        }

        public void Draw(Graphics g)
        {
            if (Rect.IsEmpty)
            {
                throw new Exception("Ball Rect cannot be empty");
            }
            SolidBrush b = new SolidBrush(Color);
            g.FillRectangle(b, Rect);
        }

        public void Move(int velocity)
        {
            if (velocity >= g.BALL_SIZE)
            {
                throw new Exception("Velocity cannot be greater than ball size due to pixel skipping");
            }
            else if (velocity >= g.BRICK_HEIGHT)
            {
                throw new Exception("Velocity cannot be greater than brick height as otherwise it can teleport through a brick");
            }
            else
            {
                int x = Rect.X;
                int y = Rect.Y;
                if (IsGoingUp && IsGoingLeft)
                {
                    x -= velocity;
                    y -= velocity;
                }
                else if (IsGoingUp && !IsGoingLeft)
                {
                    x += velocity;
                    y -= velocity;
                }
                else if (!IsGoingUp && IsGoingLeft)
                {
                    x -= velocity;
                    y += velocity;
                }
                else if (!IsGoingUp && !IsGoingLeft)
                {
                    x += velocity;
                    y += velocity;
                }
                Rect = new Rectangle(x, y, Rect.Width, Rect.Height);
            }
        }

        public void OnCollision(Brick collisionBrick)
        {
            Rectangle brickRect = collisionBrick.Rect;

            Point middleBall = new Point(Rect.X + (Rect.Width / 2), Rect.Y + (Rect.Height / 2));
            Point middleBrick = new Point(brickRect.X + (brickRect.Width / 2), brickRect.Y + (brickRect.Height / 2));

            int diffX = middleBrick.X - middleBall.X;
            int diffY = middleBrick.Y - middleBall.Y;

            if (Math.Abs(diffX) > Math.Abs(diffY * g.BrickRatio))
            {
                if (diffX > 0)
                {
                    // from right
                    IsGoingLeft = true;
                }
                else
                {
                    // from left
                    IsGoingLeft = false;
                }
            }
            else
            {
                if (diffY > 0)
                {
                    // from top
                    IsGoingUp = true;
                }
                else
                {
                    // from bottom
                    IsGoingUp = false;
                }
            }
        }


        public void CheckForWallCollision()
        {
            if (Rect.Top <= 0)
            {
                IsGoingUp = false;
            }
            else if (Rect.Top >= g.ScreenHeight)
            {
                // Game over if reaches bottom
                //Application.Exit();
                IsGoingUp = true;
            }
            else if (Rect.Left <= 0)
            {
                IsGoingLeft = false;
            }
            else if (Rect.Left >= g.ScreenWidth)
            {
                IsGoingLeft = true;
            }
        }
    }
}
