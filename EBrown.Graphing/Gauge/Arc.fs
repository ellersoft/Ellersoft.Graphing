module EBrown.Graphing.Gauge.Arc
open System.Drawing
open System.Drawing.Drawing2D
open EBrown.Graphing
open EBrown.Graphing.Gauge.Configuration
open System

let defaultConfig = { Default with ValueVerticalTranslation = Some 10.f }

let generate<'a> config (toFloat : 'a -> float32) (formatter : 'a -> string) (upper : 'a) (lower : 'a) (value : 'a) =
    use defaultFont = new Font("Arial", 12.f, FontStyle.Regular)
    let font = config.Font |> Option.defaultValue defaultFont
    let valueFont = config.ValueFont |> Option.defaultValue font

    let imageWidth = config.Width - config.Padding.Top - config.Padding.Bottom
    let imageHeight = config.Height - config.Padding.Left - config.Padding.Right
    let gaugeSweep = 180.0f
    let angle = gaugeSweep * (((value |> toFloat) - (lower |> toFloat)) / ((upper |> toFloat) - (lower |> toFloat)))
    let startAngle = 180.0f + (180.0f - gaugeSweep) / 2.0f

    let image = new Bitmap(imageWidth + config.Padding.Left + config.Padding.Right, imageHeight + config.Padding.Top + config.Padding.Bottom)
    use g = Graphics.FromImage(image)
    g.SmoothingMode <- SmoothingMode.AntiAlias
    g.InterpolationMode <- InterpolationMode.HighQualityBicubic

    use backgroundBrush = new SolidBrush(config.BackgroundColor)
    use emptyFillBrush = new SolidBrush(config.EmptyGaugeColor)
    use gaugeFillBrush = new SolidBrush(config.FillGaugeColor)
    use externalPen = new Pen(config.OutlineColor, config.OutlineThickness)
    use fontBrush = new SolidBrush(config.FontColor)
    use valueFontBrush = new SolidBrush(config.ValueFontColor |> Option.defaultValue config.FontColor)

    g.FillRectangle(backgroundBrush, Rectangle(0, 0, imageWidth + config.Padding.Left + config.Padding.Right, imageHeight + config.Padding.Top + config.Padding.Bottom))
    use path = new GraphicsPath()
    path.AddArc(Rectangle(config.Padding.Left, config.Padding.Top, imageWidth, imageWidth), startAngle, angle)
    path.Reverse()
    path.AddArc(Rectangle(config.Padding.Left + config.GaugeWidth, config.Padding.Top + config.GaugeWidth, imageWidth - config.GaugeWidth * 2, imageWidth - config.GaugeWidth * 2), startAngle, angle)
    path.CloseFigure()

    use externalPath = new GraphicsPath()
    externalPath.AddArc(Rectangle(config.Padding.Left, config.Padding.Top, imageWidth, imageWidth), startAngle, gaugeSweep)
    externalPath.Reverse()
    externalPath.AddArc(Rectangle(config.Padding.Left + config.GaugeWidth, config.Padding.Top + config.GaugeWidth, imageWidth - config.GaugeWidth * 2, imageWidth - config.GaugeWidth * 2), startAngle, gaugeSweep)
    externalPath.CloseFigure()
    
    g.FillPath(emptyFillBrush, externalPath)
    g.FillPath(gaugeFillBrush, path)
    g.DrawPath(externalPen, externalPath)

    let drawLabel = General.drawLabelCentered g (Rectangle(0, 0, image.Width, image.Height)) font fontBrush
    let drawLabelValue = General.drawLabelCentered g (Rectangle(0, 0, image.Width, image.Height)) valueFont valueFontBrush

    let gaugeLabelOffsetX = (config.Padding.Left |> float32) + (config.GaugeWidth / 2 |> float32)
    let imageMidX = (imageWidth |> float32) * 0.5f + (config.Padding.Left |> float32)
    drawLabelValue (value |> formatter) (PointF(imageMidX, (imageWidth |> float32) * 0.5f + (config.Padding.Top |> float32) - (config.ValueVerticalTranslation |> Option.defaultValue 10.f)))
    drawLabel (lower |> formatter) (PointF(gaugeLabelOffsetX, (imageWidth |> float32) * 0.5f + 5.f + (config.Padding.Top |> float32)))
    drawLabel (upper |> formatter) (PointF((image.Width |> float32) - gaugeLabelOffsetX, (imageWidth |> float32) * 0.5f + 5.f + (config.Padding.Top |> float32)))
    image
    
let generateDelegate<'a> configuration (toFloat : Func<'a, float32>) (formatter : Func<'a, string>) (upper : 'a) (lower : 'a) (value : 'a) =
    generate configuration toFloat.Invoke formatter.Invoke upper lower value
