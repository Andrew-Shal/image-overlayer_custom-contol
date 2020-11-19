using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace creating_custom_control_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //var converter = new ImageSourceConverter();
            // ImageControl.Source = (ImageSource)converter.ConvertFromInvariantString("nametag.jpg");

            FontSizeCombo.ItemsSource = Enumerable.Range(1, 8).Select(x => x * x);
        }

        #region unused
        /*
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            
            // add circles to each corner of element
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(testRectangleBox);
            try
            {
                var t = new SimpleCircleAdorner(testRectangleBox);
                myAdornerLayer.Add(t);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }    
        
        public static void SaveCanvasToFile(Canvas surface, string filename)
        {
            Size size = new Size(surface.Width, surface.Height);

            surface.Measure(size);
            surface.Arrange(new Rect(size));

            // Create a render bitmap and push the surface to it
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(surface);

            // Create a file stream for saving image
            using (FileStream outStream = new FileStream(filename, FileMode.Create))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }
        }
        public void ExportToPng(Uri path, Canvas surface)
        {
            if (path == null) return;

            try
            {
                // Save current canvas transform
                Transform transform = surface.LayoutTransform;
                // reset current transform (in case it is scaled or rotated)
                surface.LayoutTransform = null;

                // Get the size of canvas

                // INSTEAD OF CANVAS SIZE, WE CAN GET THE IMAGE SIZE
                // Size size = new Size(surface.ActualWidth, surface.ActualHeight);
                Size size = new Size(imageTest.ActualWidth, imageTest.ActualHeight);

                // Measure and arrange the surface
                // VERY IMPORTANT
                surface.Measure(size);
                surface.Arrange(new Rect(size));

                // Create a render bitmap and push the surface to it
                RenderTargetBitmap renderBitmap =
                  new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderBitmap.Render(surface);

                // Create a file stream for saving image
                using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
                {
                    // Use png encoder for our data
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(outStream);
                }

                // Restore previously saved layout
                surface.LayoutTransform = transform;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }

    public class SimpleCircleAdorner : Adorner
    {
        public SimpleCircleAdorner(UIElement adornedElement) : base(adornedElement) { 

        }

        // implement an adorner's rendering behavior, overriding the OnRender
        // method, which is called by the layout system as part of a rendering pass.
        protected override void OnRender(DrawingContext drawingContext)
        {
            this.AdornedElement.MouseEnter += AdornedElement_MouseEnter;
            Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

            SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
            renderBrush.Opacity = 0.2;

            Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy),1.5);
            double renderRadius = 5.0;

            // get the center of the rect
            var xCenter =  adornedElementRect.Width / 2;
            var yCenter = adornedElementRect.Height / 2;

            // draw circle at each corner
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, new Point(xCenter,yCenter) , renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
            base.OnRender(drawingContext);
        }

        private void AdornedElement_MouseEnter(object sender, MouseEventArgs e)
        {
            // Retrieve the coordinate of the mouse position.
            Point pt = e.GetPosition((UIElement)sender);
            
            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(this.AdornedElement,pt);

            if (result != null)
            {


                this.Cursor = Cursors.SizeWE;
                // Perform action on hit visual object.
            }

            var elem = sender as UIElement;
            Rect adornedElementRect = new Rect(elem.RenderSize);
            var e1 = new EllipseGeometry(adornedElementRect.TopRight, 5.0,5.0);
            VisualTreeHelper.HitTest(elem, null, new HitTestResultCallback(HitTestCallback), new GeometryHitTestParameters(e1));
        }
        public HitTestResultBehavior HitTestCallback(HitTestResult htrResult)
        {
            IntersectionDetail idDetail = ((GeometryHitTestResult)htrResult).IntersectionDetail;

            switch (idDetail)
            {
                case IntersectionDetail.FullyContains:
                    MessageBox.Show("fully containeed!");
                    this.Cursor = Cursors.SizeNESW;
                    return HitTestResultBehavior.Continue;
                case IntersectionDetail.Intersects:
                    this.Cursor = Cursors.SizeNESW;
                    return HitTestResultBehavior.Continue;
                case IntersectionDetail.FullyInside:
                    this.Cursor = Cursors.SizeNESW;
                    return HitTestResultBehavior.Continue;
                default:
                    return HitTestResultBehavior.Stop;
            }
        }

        private void AdornedElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
    */
        #endregion

        private void NameTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox elem = (TextBox)sender;
            overlayer.Text = elem.Text;
            overlayer.CanvasControl.UpdateLayout();
            overlayer.updateSelectionArea();
        }

        private void FontFamilyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox elem = (ComboBox)sender;
            overlayer.TextFontFamily = new FontFamily(elem.SelectedItem.ToString());
            overlayer.CanvasControl.UpdateLayout();
            overlayer.updateSelectionArea();
        }

        private void FontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox elem = (ComboBox)sender;

            try
            {
                if (elem.SelectedItem != null)
                {
                    double val;

                    if (double.TryParse(elem.SelectedItem.ToString(), out val))
                    {
                        overlayer.TextFontSize = val;
                        overlayer.CanvasControl.UpdateLayout();
                        overlayer.updateSelectionArea();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("value entered is not valid.");
            }          
        }

        private void InSelectionModeCbx_Click(object sender, RoutedEventArgs e)
        {
            CheckBox elem = (CheckBox)sender;

            overlayer.InSelectionMode = !(bool)elem.IsChecked;
            overlayer.CanvasControl.UpdateLayout();
            overlayer.updateSelectionArea();
        }
    }
}
