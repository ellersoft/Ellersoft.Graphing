module EBrown.Graphing.ArcGauge
open System.Drawing
open System.Drawing.Drawing2D

type Configuration =
    { Height : int
      Width : int
      Padding : Rectangle
      GaugeWidth : int
      Font : Font option
      EmptyGaugeColor : Color option
      FillGaugeColor : Color option
      FontColor : Color option
      OutlineColor : Color option
      BackgroundColor : Color option
      OutlineThickness : float32 option }

let generate<'a> configuration (toFloat : 'a -> float32) (formatter : 'a -> string) (max : 'a) (min : 'a) (value : 'a) =
    use defaultFont = new Font("Arial", 12.f, FontStyle.Regular)
    let font = configuration.Font |> Option.defaultValue defaultFont

    let imageWidth = configuration.Width - configuration.Padding.Top - configuration.Padding.Bottom
    let imageHeight = configuration.Height - configuration.Padding.Left - configuration.Padding.Right
    let gaugeSweep = 180.0f
    let angle = gaugeSweep * (((value |> toFloat) - (min |> toFloat)) / ((max |> toFloat) - (min |> toFloat)))
    let gaugeWidth = configuration.GaugeWidth
    let startAngle = 180.0f + (180.0f - gaugeSweep) / 2.0f

    let image = new Bitmap(imageWidth + configuration.Padding.Left + configuration.Padding.Right, imageHeight + configuration.Padding.Top + configuration.Padding.Bottom)
    use g = Graphics.FromImage(image)
    g.SmoothingMode <- SmoothingMode.AntiAlias
    g.InterpolationMode <- InterpolationMode.HighQualityBicubic

    use backgroundBrush = new SolidBrush(configuration.BackgroundColor |> Option.defaultValue Color.White)
    use emptyFillBrush = new SolidBrush(configuration.EmptyGaugeColor |> Option.defaultValue (Color.FromArgb(255, 192, 192, 192)))
    use gaugeFillBrush = new SolidBrush(configuration.FillGaugeColor |> Option.defaultValue (Color.FromArgb(255, 192, 64, 64)))
    use externalPen = new Pen(configuration.OutlineColor |> Option.defaultValue (Color.FromArgb(255, 96, 96, 96)), configuration.OutlineThickness |> Option.defaultValue 1.5f)
    use fontBrush = new SolidBrush(configuration.FontColor |> Option.defaultValue Color.Black)

    g.FillRectangle(backgroundBrush, Rectangle(0, 0, imageWidth + configuration.Padding.Left + configuration.Padding.Right, imageHeight + configuration.Padding.Top + configuration.Padding.Bottom))
    use path = new GraphicsPath()
    path.AddArc(Rectangle(configuration.Padding.Left, configuration.Padding.Top, imageWidth, imageWidth), startAngle, angle)
    path.Reverse()
    path.AddArc(Rectangle(configuration.Padding.Left + gaugeWidth, configuration.Padding.Top + gaugeWidth, imageWidth - gaugeWidth * 2, imageWidth - gaugeWidth * 2), startAngle, angle)
    path.CloseFigure()

    use externalPath = new GraphicsPath()
    externalPath.AddArc(Rectangle(configuration.Padding.Left, configuration.Padding.Top, imageWidth, imageWidth), startAngle, gaugeSweep)
    externalPath.Reverse()
    externalPath.AddArc(Rectangle(configuration.Padding.Left + gaugeWidth, configuration.Padding.Top + gaugeWidth, imageWidth - gaugeWidth * 2, imageWidth - gaugeWidth * 2), startAngle, gaugeSweep)
    externalPath.CloseFigure()
    
    g.FillPath(emptyFillBrush, externalPath)
    g.FillPath(gaugeFillBrush, path)
    g.DrawPath(externalPen, externalPath)

    let drawLabel = General.drawLabelCentered g (Rectangle(0, 0, image.Width, image.Height)) font fontBrush

    let gaugeLabelOffsetX = (configuration.Padding.Left |> float32) + (gaugeWidth / 2 |> float32)
    let imageMidX = (imageWidth |> float32) * 0.5f + (configuration.Padding.Left |> float32)
    drawLabel (value |> formatter) (PointF(imageMidX, (imageWidth |> float32) * 0.5f + (configuration.Padding.Top |> float32) - 10.f))
    drawLabel (min |> formatter) (PointF(gaugeLabelOffsetX, (imageWidth |> float32) * 0.5f + 5.f + (configuration.Padding.Top |> float32)))
    drawLabel (max |> formatter) (PointF((image.Width |> float32) - gaugeLabelOffsetX, (imageWidth |> float32) * 0.5f + 5.f + (configuration.Padding.Top |> float32)))
    image
    