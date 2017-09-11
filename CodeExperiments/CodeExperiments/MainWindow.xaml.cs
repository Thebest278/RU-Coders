using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeExperiments
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            /* Hypothesis: A break statement in a while-loop has no effect.
             * Design: Make an infinite while-loop.  Now the only way to stop the loop is with a break statement.
             * Expected if true: The program will hang, because the while-loop will not end.
             * Expected if false: The message "Hypothesis is false" is displayed.
             */
            while (true)
            {
                break;
            }
            MessageBox.Show("Hypothesis is gogoogaga");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            /* Hypothesis: a switch statement executes all cases up to and including the matching case.
             * Design: Have a case before the one that we know will match.  That case should not be executed.  If it is executed, we will know because a message will be displayed.
             * Expected if true: The message "2" (or "0") is displayed.
             * Expected if false: The message "1" is displayed.
             */
            int counter = 0;
            switch (8)
            {
                case 6:
                    counter = counter + 1;
                    break;
                case 8:
                    counter = counter + 1;
                    break;
            }
            MessageBox.Show(Convert.ToString(counter));
        }
    }
}
