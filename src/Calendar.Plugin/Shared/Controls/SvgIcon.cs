using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using Xamarin.Forms;

namespace Xamarin.Plugin.Calendar.Controls
{
    /// <summary>
    /// siehe https://www.pshul.com/2018/01/25/xamarin-forms-using-svg-images-with-skiasharp/
    /// </summary> 
    public class SvgIcon : Frame
    {
        private readonly SKCanvasView _canvasView = new SKCanvasView();

        public SvgIcon()
        {
            Padding = new Thickness(0);

            // Thanks to TheMax for pointing out that on mobile, the icon will have a shadow by default.
            // Also it has a white background, which we might not want.
            HasShadow = false;
            BackgroundColor = Color.Transparent;

            Content = _canvasView;
            _canvasView.PaintSurface += CanvasViewOnPaintSurface;
        }

        public static readonly BindableProperty IconFilePathProperty = BindableProperty.Create(
                        nameof(IconFilePath),
                        typeof(string),
                        typeof(SvgIcon),
                        default(string),
                        propertyChanged: RedrawCanvas);

        public string IconFilePath
        {
            get => (string)GetValue(IconFilePathProperty);
            set => SetValue(IconFilePathProperty, value);
        }

        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
                        nameof(IconColor),
                        typeof(Color),
                        typeof(SvgIcon),
                        Color.Black,
                        propertyChanged: RedrawCanvas);

        public Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            SvgIcon svgIcon = bindable as SvgIcon;
            svgIcon?._canvasView.InvalidateSurface();

        }
        private void CanvasViewOnPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            if (string.IsNullOrEmpty(IconFilePath))
                return;

            using (Stream stream = GetType().Assembly.GetManifestResourceStream(IconFilePath))
            {
                if (stream == null)   // needed for IOS
                    return;
                SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
                svg.Load(stream);

                SKImageInfo info = args.Info;
                canvas.Translate(info.Width / 2f, info.Height / 2f);

                SKRect bounds = svg.ViewBox;
                float xRatio = info.Width / bounds.Width;
                float yRatio = info.Height / bounds.Height;

                float ratio = Math.Min(xRatio, yRatio);

                canvas.Scale(ratio);
                canvas.Translate(-bounds.MidX, -bounds.MidY);
                using (var paint = new SKPaint())
                {
                    paint.ColorFilter = SKColorFilter.CreateBlendMode(
                        IconColor.ToSKColor(),       
                        SKBlendMode.SrcIn); // use the source color

                    canvas.DrawPicture(svg.Picture, paint);
                }

            }
        }
    }
}
