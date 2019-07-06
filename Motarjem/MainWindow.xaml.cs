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
using Motarjem.Core;

namespace Motarjem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            en_display = new FlowDocumentDisplay(en.Inlines);
            fa_display = new FlowDocumentDisplay(fa.Inlines);
        }

        private void Translate(object sender, RoutedEventArgs e)
        {
            try
            {
                en_display.Clear();
                fa_display.Clear();
                foreach (var s in Parser.Parse(input.Text))
                {
                    s.Display(en_display);
                    s.Translate().Display(fa_display);
                }
            }
            catch (MotarjemException ex)
            {
                en_display.Clear();
                en_display.Print("Error: " + ex.Message, FontColor.Red);
                fa_display.Clear();
                fa_display.Print("خطا: " + ex.MessageFa, FontColor.Red);
            }
#if !DEBUG
            catch (Exception ex)
            {
                en_display.Clear();
                fa_display.Clear();
                en_display.Print("Internal Error: " + ex, FontColor.Red);
            }
#endif
        }

        private FlowDocumentDisplay en_display;
        private FlowDocumentDisplay fa_display;
    }
}
