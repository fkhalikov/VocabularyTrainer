using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace VocabularyTrainer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public List<Word> Words { get; set; } = new List<Word>();

    public Word CurrentWord { get; set; }

    public MainWindow()
    {
      InitializeComponent();

      var lines = File.ReadAllLines("./Dictionary.txt");

      foreach(var line in lines)
      {
        var data = line.Split('>');

        Words.Add(new Word { German = data[0], English = data[1] });
      }

      GetRandomWord();

      txtbTranslation.KeyUp += TxtbTranslation_KeyUp;
    }

    private void TxtbTranslation_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key == System.Windows.Input.Key.Enter)
      {
        if (txtbTranslation.Text == CurrentWord.German)
        {
          lblState.Content = "";
          txtbTranslation.Background = Brushes.Lime;
          Task.Run(() =>
          {
            Task.Delay(1000).ContinueWith((tsk) =>
            {
              Dispatcher.Invoke(() => { 
                txtbTranslation.Background = Brushes.Black;
                txtbTranslation.Text = "";
                GetRandomWord();
              });
            });
          });
          lblState.Background = SystemColors.WindowBrush;
        }
        else
        {
          lblState.Background = SystemColors.AppWorkspaceBrush;
          lblState.Content = "Try again";
        }
      }
    }

    private void GetRandomWord()
    {
      int rnd = (new Random()).Next(0, Words.Count - 1);
      CurrentWord = Words[rnd];
      txtbBlock.Text = CurrentWord.English;
    }

    private void btnShow_Click(object sender, RoutedEventArgs e)
    {
      this.txtbTranslation.Text = CurrentWord.German;
    }
  }

  public class Word
  {
    public string English { get; set; }

    public string German { get; set; }
  }
}