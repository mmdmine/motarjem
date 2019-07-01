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
        }

        private void Translate(object sender, RoutedEventArgs e)
        {
            output.Blocks.Clear();
            var en = new Paragraph();
            var fa = new Paragraph { FlowDirection = FlowDirection.RightToLeft };
            var en_display = new Display(en.Inlines);
            var fa_display = new Display(fa.Inlines);
            try
            {
                var sentences = Sentence.ParseEnglish(input.Text);
                foreach (var s in sentences)
                {
                    s.Display(en_display);
                    s.Translate().Display(fa_display);
                }
                output.Blocks.Add(en);
                output.Blocks.Add(fa);
            }
            catch (MotarjemException ex)
            {
                var err = new Paragraph
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0x00, 0x20))
                };
                err.Inlines.Add(new Run { Text = "Error: " });
                err.Inlines.Add(new Run { FontStyle = FontStyles.Italic, Text = ex.GetType().FullName });
                err.Inlines.Add(new LineBreak());
                err.Inlines.Add(new Run { Text = ex.Message });
                err.Inlines.Add(new LineBreak());
                err.Inlines.Add(new Run { Text = ex.MessageFa, FlowDirection = FlowDirection.RightToLeft });
                output.Blocks.Add(err);
            }
#if !DEBUG
            catch (Exception ex)
            {
                var err = new Paragraph
                {
                    Foreground = new SolidColorBrush(Color.FromRgb(0xB0, 0x00, 0x20))
                };
                err.Inlines.Add(new Run { Text = "Internal Error: " });
                err.Inlines.Add(new Run { FontStyle = FontStyles.Italic,  Text = ex.GetType().FullName });
                output.Blocks.Add(err);
            }
#endif
        }
    }
}
