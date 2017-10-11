using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
    class Character
    {
        public enum Direction { Up, Down, Left, Right }
        public Direction Movement { get; set; }
        protected int Speed { get; set; }
        public Position Pos { get; set; }
        public string CurrentState { get; set; } 
        public Character(Position l,int speed)
        {
            Pos = l;
            Speed = speed;
            CurrentState = "NotMoving";
        }
        public Position K(Position n) 
        {
            switch (Movement)
            {
                case Direction.Right:
                    n.X++;
                    break;
                case Direction.Left:
                    n.X--;
                    break;
                case Direction.Down:
                    n.Y++;
                    break;
                case Direction.Up:
                    n.Y--;
                    break;
            }
            return new Position(n.X,n.Y);
        }
        public void Move()
        {
            switch (CurrentState)
            {
                case "MovingLeft":

                    if (this.Pos.X > 0)
                    {
                        this.Pos.X -= this.Speed;
                    }
                    break;
                case "MovingRight":
                    if (this.Pos.X<this.Playground.Width)
                    {
                        this.Pos.X += this.Speed;
                    }
                        break;
                case "MovingUp":
                    if (this.Pos.Y>0)
                    {
                        this.Pos.Y -= this.Speed;
                    }
                    break;
                case "Movingdown":
                    if (this.Pos.Y < this.Playground.Height)
                    {
                        this.Pos.Y += this.Speed;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Position(int x,int y)
        {
            X = x;
            Y = y;
        }
    }
    class Maze
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Maze(Canvas canvas1)
        {
            Width = canvas1.ActualWidth;
            Height = canvas1.Height;
        }
        public Maze(Canvas canvas1, string imagePath)
        {
            Width = canvas1.ActualWidth;
            Height = canvas1.Height;
            //Image stuff
        }
    public class MainCharacter:Character
    {
        
    }   
}
