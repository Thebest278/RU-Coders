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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThinkLib;
using System.Windows.Threading;

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

                        if (Canvas.GetTop(pic) + pic.ActualHeight + Y >= can.ActualHeight) { d = r.Next(0, 3); facing = (Direction)d; }
                        break;
                    }
                case Direction.Up:
                    {

                        if (Canvas.GetTop(pic) - Y <= 0) { d = 2; do { d = r.Next(0, 4); } while (d == 2); facing = (Direction)d; }
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
        public double X { get; set; }
        public double Y { get; set; }
        public Canvas can { get; set; }
        public Moving facing { get; set; }
        public string State { get; set; }
        public Player(List<BitmapImage> x, Canvas c, Image d)
        {
            X = 10;
            Y = 10;
            CurrentPic = d;
            Pic = x;
            State = null;
            facing = Moving.Down;
            ChangeDirection();
            can = c;

        }
        //public void Move(char pressed)
        //{

        //    switch (pressed)
        //    {
        //        case 'l':
        //            {
        //                facing = Moving.Left;
        //                ChangeDirection();
        //                if (Canvas.GetLeft(CurrentPic) > 0)
        //                {
        //                    Canvas.SetLeft(CurrentPic, (Canvas.GetLeft(CurrentPic) - X));
        //                }
        //                else facing = Moving.Stopped;
        //                break;
        //            }
        //        case 'r':
        //            {
        //                facing = Moving.Right;
        //                ChangeDirection();
        //                if (Canvas.GetLeft(CurrentPic) + CurrentPic.ActualWidth + X < can.ActualWidth)
        //                {
        //                    Canvas.SetLeft(CurrentPic, (Canvas.GetLeft(CurrentPic) + X));
        //                }
        //                else facing = Moving.Stopped;
        //                break;
        //            }
        //        case 'd':
        //            {
        //                facing = Moving.Down;
        //                ChangeDirection();
        //                if (Canvas.GetTop(CurrentPic) + CurrentPic.ActualHeight + Y < can.ActualHeight)
        //                {
        //                    Canvas.SetTop(CurrentPic, (Canvas.GetTop(CurrentPic) + Y));
        //                }
        //                else facing = Moving.Stopped;
        //                break;
        //            }
        //        case 'u':
        //            {
        //                facing = Moving.Up;
        //                ChangeDirection();
        //                if (Canvas.GetTop(CurrentPic) > 0)
        //                {
        //                    Canvas.SetTop(CurrentPic, (Canvas.GetTop(CurrentPic) - Y));
        //                }
        //                else facing = Moving.Stopped;
        //                break;
        //            }
        //        default:
        //            if (Canvas.GetTop(CurrentPic) > 0)
        //            {
        //                Canvas.SetTop(CurrentPic, (Canvas.GetTop(CurrentPic) - Y));
        //            }
        //            break;
        //    }
        //}
        public void ChangeDirection()
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
        List<UIElement> Img;
        double PacSpeed;
        public MainWindow()
        {
            InitializeComponent();

            Img = new List<UIElement>();
            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(100);
            theTimer.IsEnabled = true;
            theTimer.Tick += DispatcherTimer_Tick;
            YellowGhost = new Ghost(Yellow, 'y', Background);
            RedGhost = new Ghost(Red, 'r', Background);
            PinkGhost = new Ghost(Pink, 'p', Background);
            List<BitmapImage> PacPics = new List<BitmapImage> { new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Up.gif", UriKind.Absolute)), new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/down.gif", UriKind.Absolute)),
                 new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Left.gif", UriKind.Absolute)),new BitmapImage(new Uri(@"pack://application:,,,/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "/Resources/Right.gif", UriKind.Absolute)) };
            PacSpeed = 10;
            One = new Player(PacPics, Background, MsPac);
            One.State = "Moving";
            Add();
           // MessageBox.Show(@"Welcome..."+Convert.ToString(Img.Count),  "UserGuide");
        }
        private void Remove()
        {
            foreach(UIElement o in Img)
            {
                double tr=Canvas.GetLeft(o);
                double d = Canvas.GetTop(o);

               
                if (Canvas.GetTop(MsPac)-MsPac.ActualHeight<d)
                {
                    Background.Children.Remove(o);
                     MessageBox.Show($"{Canvas.GetTop(MsPac)}");
                }
            }
        }
        public void Trail()
        {
            if(Canvas.GetTop(MsPac) - MsPac.ActualHeight <= Canvas.GetTop(i1) - i1.ActualHeight)
            {
                MessageBox.Show("");
            }
        }
        public double distanceBetween(double x1,double x2,double y1,double y2)
        {
            double dis = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            return dis;
        }
        List<UIElement> Add()
        {
            foreach(UIElement i in Background.Children)
            {
                if ( i.Uid.StartsWith("l"))
                {
                    Img.Add(i);
                }
            }
            return Img;
        }
     
        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            YellowGhost.Move();
            PinkGhost.Move();
            RedGhost.Move();
            PacMove();
            // One.Move(Move);
            //Each Time a the timer ticks the ghost will call its Move() method to 
            //make it move in the appropriate direction
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            // One.facing = Player.Moving.Stopped;
        }
        public void PacMove()
        {
            One.X = Canvas.GetLeft(MsPac) + PacSpeed;
            One.Y = Canvas.GetTop(MsPac) + PacSpeed;
            double r = Canvas.GetTop(MsPac) - PacSpeed;
            double r1 = Canvas.GetLeft(MsPac) - PacSpeed;
            if (One.State == "Moving")
            {
                switch (One.facing)
                {
                    case Player.Moving.Right:
                        if (One.X < Background.ActualWidth - MsPac.ActualWidth)  
                            Canvas.SetLeft(MsPac, One.X); break; 
                    case Player.Moving.Down:
                        if (One.Y < Background.ActualHeight - MsPac.ActualHeight)
                            Canvas.SetTop(MsPac, One.Y); break;
                    case Player.Moving.Up:
                        if (r > 0) 
                            Canvas.SetTop(MsPac, r); break;
                    case Player.Moving.Left:
                        if (r1 > 0)
                            Canvas.SetLeft(MsPac, r1); break;
                }

            }

        }
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: One.facing = Player.Moving.Left; One.ChangeDirection(); Trail(); break;
                case Key.Right: One.facing = Player.Moving.Right; One.ChangeDirection();/* Remove(); */break;
                case Key.Up: One.facing = Player.Moving.Up; One.ChangeDirection(); /* Remove(); */ break;
                case Key.Down: One.facing = Player.Moving.Down; One.ChangeDirection();/* Remove(); */break;
                default: One.facing = Player.Moving.Stopped;/* Remove(); */  break;
            }
        }
        bool RedGhostCollide()
        {
            return  (Canvas.GetLeft(Red) <= Canvas.GetLeft(MsPac) + MsPac.ActualWidth) && (Canvas.GetTop(Red) >= Canvas.GetTop(MsPac)) && ((Canvas.GetTop(Red) <= Canvas.GetTop(MsPac) + MsPac.ActualHeight));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Background.Visibility = Visibility.Visible;
            lac.Visibility = Visibility.Hidden;
            //One.State = "Moving";
        }
    }
}
