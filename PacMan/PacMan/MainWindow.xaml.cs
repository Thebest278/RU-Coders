using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkLib;
using System.Windows.Threading;
using PacMan;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class Ghost
    {
        public Image pic { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        public Canvas can { get; set; }
        public enum Direction { Left, Right, Up, Down };
        Direction facing { get; set; }

        public Ghost(Image p, char colour, Canvas canv)
        {
            pic = p;
            X = 8;
            Y = 8;
            can = canv;
            Random r = new Random();

            int pink = r.Next(0, 4);
            int red = r.Next(0, 4);
            int yellow = r.Next(0, 4);

            switch (colour)
            {
                case 'p': { facing = (Direction)pink; break; }
                case 'y': { facing = (Direction)yellow; break; }
                case 'r': { facing = (Direction)red; break; }
            }
        }
        public void Move()
        {
            double next;
            Walls();
            switch (facing)
            {
                case Direction.Left: { next = (Canvas.GetLeft(pic) - X); Canvas.SetLeft(pic, next); break; }
                case Direction.Right: { next = (Canvas.GetLeft(pic) + X); Canvas.SetLeft(pic, next); break; }
                case Direction.Up: { next = (Canvas.GetTop(pic) - Y); Canvas.SetTop(pic, next); break; }
                case Direction.Down: { next = (Canvas.GetTop(pic) + Y); Canvas.SetTop(pic, next); break; }
            } 
        }
        private void Walls()
        {
            
            Random r = new Random();
            int d;
            switch (facing)
            {
                case Direction.Down:
                    {
                        
                        if (Canvas.GetTop(pic) + pic.ActualHeight + Y >= can.ActualHeight) {d = r.Next(0,3); facing = (Direction)d; }
                        break;
                    }
                case Direction.Up:
                    {
                        
                        if (Canvas.GetTop(pic)- Y <= 0) { d = 2;  do { d = r.Next(0, 4); } while ( d == 2 );  facing = (Direction)d; }
                        break;
                    }
                case Direction.Left:
                    {
                        
                        if (Canvas.GetLeft(pic) - X <= 0) { d = r.Next(1, 4); facing = (Direction)d; }
                        break;
                    }
                case Direction.Right:
                    {
                        
                        if (Canvas.GetLeft(pic) + pic.ActualWidth + X >= can.ActualWidth) { d = 1; do { d = r.Next(0, 4); } while (d == 1); facing = (Direction)d; }
                        break;
                    }
            }
        }
    }

    public class Player
    {
        
        public enum Moving { Up, Down, Left, Right, Stopped }
        public List<BitmapImage> Pic { get; set; }
        public Image CurrentPic { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        public Canvas can { get; set; }
        Moving facing { get; set; }
        public Player(List<BitmapImage> x, Canvas c, Image d)
        {
            X = 9;
            Y = 9;
            CurrentPic = d;
            Pic = x;
            facing = Moving.Down;
            ChangeDirection();
            can = c;
            
        }
        public void Move(char pressed)
        {
            
            switch (pressed)
            {
                case 'l':
                    {
                        facing = Moving.Left ;
                        ChangeDirection();
                        if (Canvas.GetLeft(CurrentPic) > 0)
                        {
                            Canvas.SetLeft(CurrentPic, (Canvas.GetLeft(CurrentPic) - X));
                        }
                        else facing = Moving.Stopped;
                        break;
                    }
                case 'r':
                    {
                        facing = Moving.Right;
                        ChangeDirection();
                        if (Canvas.GetLeft(CurrentPic) + CurrentPic.ActualWidth + X < can.ActualWidth)
                        {
                            Canvas.SetLeft(CurrentPic, (Canvas.GetLeft(CurrentPic) + X));
                        }
                        else facing = Moving.Stopped;
                        break;
                    }
                case 'd':
                    {
                        facing = Moving.Down;
                        ChangeDirection();
                        if (Canvas.GetTop(CurrentPic) + CurrentPic.ActualHeight + Y < can.ActualHeight)
                        {
                            Canvas.SetTop(CurrentPic, (Canvas.GetTop(CurrentPic) + Y));
                        }
                        else facing = Moving.Stopped;
                        break;
                    }
                case 'u':
                    {
                        facing = Moving.Up;
                        ChangeDirection();
                        if (Canvas.GetTop(CurrentPic) > 0)
                        {
                            Canvas.SetTop(CurrentPic, (Canvas.GetTop(CurrentPic) - Y));
                        }
                        else facing = Moving.Stopped;
                        break;
                    }
            }
        }
        private void ChangeDirection()
        {
            switch (facing)
            {

                case Moving.Up: { CurrentPic.Source = Pic[0]; break; }
                case Moving.Down: { CurrentPic.Source = Pic[1]; break; }
                case Moving.Left: { CurrentPic.Source = Pic[2]; break; }
                case Moving.Right: { CurrentPic.Source = Pic[3]; break; }

            }

        }
    }

    public partial class MainWindow : Window
    {
        DispatcherTimer theTimer;
        Ghost YellowGhost;
        Ghost RedGhost;
        Ghost PinkGhost;
        Player One;
        char Move;
        public MainWindow()
        {
            InitializeComponent();

            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(100);
            theTimer.IsEnabled = true;
            theTimer.Tick += DispatcherTimer_Tick;
            YellowGhost = new Ghost(Yellow, 'y', Background);
            RedGhost = new Ghost(Red, 'r', Background);
            PinkGhost = new Ghost(Pink, 'p', Background);
            List<BitmapImage> PacPics = new List<BitmapImage> { new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Up.gif", UriKind.Absolute)), new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/down.gif", UriKind.Absolute)),
                 new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Left.gif", UriKind.Absolute)),new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Right.gif", UriKind.Absolute)) };

            One = new Player(PacPics, Background, MsPac);
            

        }






        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            YellowGhost.Move();
            PinkGhost.Move();
            RedGhost.Move();
            One.Move(Move);
            //Each Time a the timer ticks the ghost will call its Move() method to 
            //make it move in the appropriate direction
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            // this is the controller for the paddle
            switch (e.Key)
            {
                case Key.Left:
                    Move = 'l'; MessageBox.Show("ad");
                    break;
                case Key.Right:
                    Move ='r';
                    break;
                case Key.Up:
                    Move = 'u'; break;
                case Key.Down:
                    Move = 'd';
                    break;
                // case Key.Add: velY++; break;
                // case Key.Subtract: velY--; break;
                default: Move = 's';  break;
            }
            

        }

        private void Background_KeyDown(object sender, KeyEventArgs e)
        {
            
            // this is the controller for the paddle
            switch (e.Key)
            {
                case Key.Home:
                    Move = 'l'; MessageBox.Show("ad");
                    break;
                case Key.End:
                    Move = 'r';
                    break;
                case Key.PageUp:
                    Move = 'u'; break;
                case Key.PageDown:
                    Move = 'd';
                    break;
                // case Key.Add: velY++; break;
                // case Key.Subtract: velY--; break;
                default: Move = 's'; break;
            }
            
        }
    }
}
