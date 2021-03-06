﻿using System;
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
using Microsoft.Win32;

namespace creating_custom_control_1
{
    /// <summary>
    /// Interaction logic for ImageOverlayer.xaml
    /// </summary>
    public partial class ImageOverlayer : UserControl
    {
        // hidden internal 
        private Point startDrag;

        // Font properties
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ImageOverlayer), new PropertyMetadata("Placeholder"));
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static DependencyProperty TextFontColorProperty = DependencyProperty.Register("TextFontColor", typeof(SolidColorBrush), typeof(ImageOverlayer), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public SolidColorBrush TextFontColor {
            get { return (SolidColorBrush)GetValue(TextFontColorProperty); }
            set { this.SetValue(TextFontColorProperty, value); }
        }

        public static DependencyProperty TextFontSizeProperty = DependencyProperty.Register("TextFontSize", typeof(double), typeof(ImageOverlayer), new PropertyMetadata(20.00));
        public double TextFontSize {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { this.SetValue(TextFontSizeProperty, value); }
        }

        public static DependencyProperty TextFontFamilyProperty = DependencyProperty.Register("TextFontFamily", typeof(FontFamily), typeof(ImageOverlayer), new PropertyMetadata(new FontFamily("Century Gothic")) );
        public FontFamily TextFontFamily {
            get { return (FontFamily)GetValue(TextFontFamilyProperty); }
            set { this.SetValue(TextFontFamilyProperty, value); }
        }


        //  Image properties
        public static  DependencyProperty BackgroundImageProperty = DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(ImageOverlayer), new PropertyMetadata());
        public ImageSource BackgroundImage {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { this.SetValue(BackgroundImageProperty,value); } 
        }


        // SelectionArea properties
        public static DependencyProperty MarchingAntsColorProperty = DependencyProperty.Register("MarchingAntsColor", typeof(SolidColorBrush), typeof(ImageOverlayer), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public SolidColorBrush MarchingAntsColor
        {
            get { return (SolidColorBrush)GetValue(MarchingAntsColorProperty); }
            set { this.SetValue(MarchingAntsColorProperty, value); }
        }
        
        public static DependencyProperty MarchingAntsWeightProperty = DependencyProperty.Register("MarchingAntsWeight", typeof(double), typeof(ImageOverlayer), new PropertyMetadata(1.00));
        public double MarchingAntsWeight
        {
            get { return (double)GetValue(MarchingAntsWeightProperty); }
            set { this.SetValue(MarchingAntsWeightProperty, value); }
        }

        public static DependencyProperty SelectionAreaBcgColorProperty = DependencyProperty.Register("SelectionAreaBcgColor", typeof(SolidColorBrush), typeof(ImageOverlayer), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(40,0,0,0))));
        public SolidColorBrush SelectionAreaBcgColor
        {
            get { return (SolidColorBrush)GetValue(SelectionAreaBcgColorProperty); }
            set { this.SetValue(SelectionAreaBcgColorProperty, value); }
        }

        // Control property
        public static DependencyProperty InSelectionModeProperty = DependencyProperty.Register("InSelectioNMode", typeof(bool), typeof(ImageOverlayer), new PropertyMetadata(true));
        public bool InSelectionMode
        {
            get { return (bool)GetValue(InSelectionModeProperty); }
            set { this.SetValue(InSelectionModeProperty, value); }
        }

        /// <summary>
        /// Initializes a new instance of the ImageOverlayerControl.
        /// </summary>
        public ImageOverlayer()
        {
            InitializeComponent();

            // add canvas control handlers
            addCanvasControlHandlers();
        }

        public void addCanvasControlHandlers() {
            CanvasControl.MouseDown += CanvasControl_MouseDown;
            CanvasControl.MouseUp +=CanvasControl_MouseUp;
            CanvasControl.MouseMove += CanvasControl_MouseMove;
        }
        public void removeCanvasControlHandlers()
        {
            CanvasControl.MouseDown -= CanvasControl_MouseDown;
            CanvasControl.MouseUp -= CanvasControl_MouseUp;
            CanvasControl.MouseMove -= CanvasControl_MouseMove;
        }

        private void CanvasControl_MouseDown(object sender, MouseButtonEventArgs e) {
            if (!InSelectionMode) return;

            //Set the start point
            startDrag = e.GetPosition(CanvasControl);
            //Move the selection marquee on top of all other objects in canvas
            Canvas.SetZIndex(RectangleControl, CanvasControl.Children.Count);
            Canvas.SetZIndex(TextContainer, CanvasControl.Children.Count - 1);
            Canvas.SetZIndex(ImageControl, 0);

            //Capture the mouse
            if (!CanvasControl.IsMouseCaptured)
                CanvasControl.CaptureMouse();
            CanvasControl.Cursor = Cursors.Cross;
        }

        public void updateSelectionArea() {
            //Release the mouse
            if (CanvasControl.IsMouseCaptured)
                CanvasControl.ReleaseMouseCapture();
            CanvasControl.Cursor = Cursors.Arrow;

            // canvas.UpdateLayout();
            var left = RectangleControl.RenderTransform.Value.OffsetX;
            var top = RectangleControl.RenderTransform.Value.OffsetY;

            var rw = RectangleControl.Width;
            var rh = RectangleControl.Height;

            // var xCenter = left + rw / 2;
            // var yCenter = top + rh / 2;

            TextContainer.Width = rw;
            TextContainer.Height = rh;


            textTest.TextWrapping = TextWrapping.Wrap;
            textTest.TextAlignment = TextAlignment.Center;
            textTest.HorizontalAlignment = HorizontalAlignment.Stretch;
            textTest.VerticalAlignment = VerticalAlignment.Center;
            // textTest.Width = rw;
            // textTest.Height = rh;
            textTest.FontWeight = FontWeight.FromOpenTypeWeight(700);

            TextContainer.RenderTransform = new TranslateTransform(left, top);
        }

        private void CanvasControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!InSelectionMode) return;
            updateSelectionArea();
        }

        private void CanvasControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!InSelectionMode) return;

            if (CanvasControl.IsMouseCaptured)
            {
                Point currentPoint = e.GetPosition(CanvasControl);

                //Calculate the top left corner of the rectangle 
                //regardless of drag direction
                double x = startDrag.X < currentPoint.X ? startDrag.X : currentPoint.X;
                double y = startDrag.Y < currentPoint.Y ? startDrag.Y : currentPoint.Y;

                if (RectangleControl.Visibility == Visibility.Hidden)
                    RectangleControl.Visibility = Visibility.Visible;

                //Move the rectangle to proper place
                RectangleControl.RenderTransform = new TranslateTransform(x, y);
                //Set its size
                RectangleControl.Width = Math.Abs(e.GetPosition(CanvasControl).X - startDrag.X);
                RectangleControl.Height = Math.Abs(e.GetPosition(CanvasControl).Y - startDrag.Y);
            }
        }
    }

}
