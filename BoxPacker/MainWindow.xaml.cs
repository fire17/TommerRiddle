using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using BoxPacker.Core;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoxPacker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Packer _packer = new Packer();

        private readonly ObservableCollection<Box> _boxes = new ObservableCollection<Box>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            BoxesItemsControl.ItemsSource = _boxes;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // await GenerateCanvas();
        }

        private async Task GenerateCanvas(bool mock = true)
        {
            var boxes = mock ? MockBoxesGenerator.Generate() : _boxes.ToList();

            if (!boxes.Any())
            {
                MessageBox.Show(this, "No boxes");
                return;
            }

            Canvas.Children.Clear();

            var potPack = _packer.Pack(boxes.ToList());

            var canvasWidth = potPack.Width;
            var canvasHeight = potPack.Height;

            Canvas.Width = canvasWidth;
            Canvas.Height = canvasHeight;
            Canvas.LayoutTransform = new ScaleTransform
            {
                ScaleX = 2,
                ScaleY = 2
            };

            foreach (var box in boxes)
            {
                var grid = new Grid();

                var rectangle = new Rectangle
                {
                    Width = box.Width,
                    Height = box.Height,
                    Fill = (SolidColorBrush) new BrushConverter().ConvertFrom(box.ColorHex)
                };

                var textBlock = new TextBlock
                {
                    Text = box.Id.ToString(),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    FontSize = GetIdFontSize(box),
                    Foreground = Brushes.White
                };

                grid.Children.Add(rectangle);
                grid.Children.Add(textBlock);

                Canvas.Children.Add(grid);
                Canvas.SetTop(grid, box.Y);
                Canvas.SetLeft(grid, box.X);

                if (!box.Equals(boxes.Last()))
                {
                    await Task.Delay(10);
                }
            }
        }

        private double GetIdFontSize(Box box)
        {
            if (box.Width < 20 || box.Height < 20)
            {
                return 4;
            }

            if (box.Width < 40 || box.Height < 40)
            {
                return 6;
            }

            if (box.Width < 80 || box.Height < 80)
            {
                return 8;
            }

            return 12;
        }

        private void AddRectangleClicked(object sender, RoutedEventArgs e)
        {
            var rectangleHeightValueTextBox = RectangleHeightValueTextBox.Text;
            var rectangleWidthValueTextBox = RectangleWidthValueTextBox.Text;

            if (string.IsNullOrWhiteSpace(rectangleHeightValueTextBox) ||
                string.IsNullOrWhiteSpace(rectangleWidthValueTextBox))
            {
                return;
            }
            
            _boxes.Add(new Box
            {
                Height = double.Parse(rectangleHeightValueTextBox),
                Width = double.Parse(rectangleWidthValueTextBox),
                ColorHex = RandomGenerator.GetRandomColorHex(new Random())
            });
        }

        private void ImportJsonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var fileContent = File.ReadAllText(openFileDialog.FileName);

                    List<Box> boxes = new List<Box>();
                    var jArray = JArray.Parse(fileContent);

                    var random = new Random();
                    foreach (var jObject in jArray.Children())
                    {
                        boxes.Add(new Box
                        {
                            Width = double.Parse(jObject["width"]?.ToString() ?? "0"),
                            Height = double.Parse(jObject["height"]?.ToString() ?? "0"),
                            Id = int.Parse(jObject["id"]?.ToString() ?? "0"),
                            ColorHex = RandomGenerator.GetRandomColorHex(random)
                        });
                    }

                    _boxes.Clear();
                    boxes.ForEach(box => _boxes.Add(box));
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this,
                        "The JSON is not in the correct format. Make sure it's a JSON array that contains valid properties of id, width & height.");
                }
            }
        }

        private void ClearClicked(object sender, RoutedEventArgs e)
        {
            _boxes.Clear();
            Canvas.Children.Clear();
            RectangleWidthValueTextBox.Text = "";
            RectangleHeightValueTextBox.Text = "";
            Canvas.Width = 0;
            Canvas.Height = 0;
        }

        private async void GenerateClicked(object sender, RoutedEventArgs e)
        {
            await GenerateCanvas(false);
        }

        private async void GenerateRandomClicked(object sender, RoutedEventArgs e)
        {
            await GenerateCanvas(true);
        }

        private void RectangleInputTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}