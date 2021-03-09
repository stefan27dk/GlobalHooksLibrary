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
using WpfGlobalHooksLibrary;

namespace GlobalHooksLibrary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
       public MainWindow()
        {
            InitializeComponent();
            MouseHook.Start();  // Mouse Hook
            MouseHook.MouseAction += new EventHandler(Global_Mouse_Event);
            MouseHook.MouseLeftButtonClick += new EventHandler(Global_Mouse_Left_Click_Event);
        }

     


        // Mouse Hook Global Event
        private void Global_Mouse_Event(object sender, EventArgs e)
        {
            
        }



        // Mouse Hook Global Mouse - Left - Click Event
        private void Global_Mouse_Left_Click_Event(object sender, EventArgs e)
        {
            CountGlobalMouseClicks();
        }





        // Mouse
        private void CountGlobalMouseClicks()
        {
            myGrid.Background = Brushes.Lime;
            textbox_MouseClicks_Count.Text = (Int32.Parse(textbox_MouseClicks_Count.Text) + 1).ToString();
        }
    }
}
