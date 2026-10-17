using EventFlowAPI.Logic.Helpers.JpgOptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace EventFlowAPI.Logic.Services.OtherServices.Services.Configuration.HallConfiguration
{
    public class WatermarkConfiguration(
        int canvasWidth, 
        float canvasBorderThickness,
        int paddingFromCanvasBorder,
        int footerPadding,
        float legendY,
        int legendTextHeight,
        int legendTextPadding
        )
    {
        private const double logoScaleRatio = 0.35;
        private const float logoWatermarkOpacity = 0.7f;
        private Image? resizedLogo;
        public Image? ResizedLogo => resizedLogo;
        public double LogoScaleRatio => logoScaleRatio;
        public ImagePrintingOptions GetWaterMarkPrintingOptions(Image logo)
        {
            var newLogoWidth = logo.Width * logoScaleRatio;
            var newLogoHeight = logo.Height * logoScaleRatio;
            var logoPointX = canvasWidth - canvasBorderThickness - paddingFromCanvasBorder - footerPadding - newLogoWidth;
            var logoPointY = (legendY + legendTextHeight + legendTextPadding) - newLogoHeight;
            resizedLogo = logo.Clone(ctx => ctx.Resize(new Size((int)newLogoWidth, (int)newLogoHeight)));
            return new ImagePrintingOptions
            {
                Location = new Point((int)logoPointX, (int)logoPointY),
                Opacity = logoWatermarkOpacity
            };
        }
    }
}
