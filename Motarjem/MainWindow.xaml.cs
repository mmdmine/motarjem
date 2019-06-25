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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            output.Blocks.Clear();
            var eng = new Paragraph();
            var fa = new Paragraph { FlowDirection = FlowDirection.RightToLeft };
            output.Blocks.Add(eng);
            output.Blocks.Add(fa);
            EnglishDump.SetOutput(eng.Inlines);
            PersianDump.SetOutput(fa.Inlines);
            try
            {
                var sentences = Parser.ParseEnglish(Token.Tokenize(input.Text));
                foreach (var s in sentences)
                {
                    PersianDump.DumpSentence(Translator.Translate(s));
                    EnglishDump.DumpSentence(s);
                }
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
