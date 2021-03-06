﻿module EBrown.Graphing.Gauge.Linear
open System.Drawing
open System.Drawing.Drawing2D
open EBrown.Graphing
open EBrown.Graphing.Gauge.Configuration
open System

let defaultConfig = { Default with GaugeWidth = 32; Height = 50 }

let generate<'a> config (toFloat : 'a -> float32) (formatter : 'a -> string) (upper : 'a) (lower : 'a) (value : 'a) =
    use defaultFont = new Font("Arial", 12.f, FontStyle.Regular)
    let font = config.Font |> Option.defaultValue defaultFont
    let valueFont = config.ValueFont |> Option.defaultValue font

    let imageWidth = config.Width - config.Padding.Top - config.Padding.Bottom
    let imageHeight = config.Height - config.Padding.Left - config.Padding.Right
    let length = ((imageWidth |> float32) * (((value |> toFloat) - (lower |> toFloat)) / ((upper |> toFloat) - (lower |> toFloat)))) |> int

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
    let filledGauge = Rectangle(config.Padding.Left, config.Padding.Top, length, config.GaugeWidth)

    let externalGauge = Rectangle(config.Padding.Left, config.Padding.Top, imageWidth, config.GaugeWidth)
    
    g.FillRectangle(emptyFillBrush, externalGauge)
    g.FillRectangle(gaugeFillBrush, filledGauge)
    g.DrawRectangle(externalPen, externalGauge)
    
    let drawLabel = General.drawLabel g (Rectangle(0, 0, image.Width, image.Height)) font fontBrush
    let drawLabelCentered = General.drawLabelCentered g (Rectangle(0, 0, image.Width, image.Height)) valueFont valueFontBrush

    let gaugeLabelOffsetX = (config.Padding.Left |> float32)
    let imageMidX = (imageWidth |> float32) * 0.5f + (config.Padding.Left |> float32)
    drawLabelCentered (value |> formatter) (PointF(imageMidX, (config.Padding.Top |> float32) + (config.GaugeWidth |> float32) + 5.f - (config.ValueVerticalTranslation |> Option.defaultValue 0.f)))
    drawLabel (lower |> formatter) (PointF(gaugeLabelOffsetX, (config.Padding.Top |> float32) + (config.GaugeWidth |> float32) + 5.f))
    drawLabel (upper |> formatter) (PointF((image.Width |> float32) - gaugeLabelOffsetX - g.MeasureString(upper |> formatter, font).Width, (config.Padding.Top |> float32) + (config.GaugeWidth |> float32) + 5.f))
    image
    
let generateDelegate<'a> configuration (toFloat : Func<'a, float32>) (formatter : Func<'a, string>) (upper : 'a) (lower : 'a) (value : 'a) =
    generate configuration toFloat.Invoke formatter.Invoke upper lower value
