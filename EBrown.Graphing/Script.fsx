#load "General.fs"
#load "ArcGauge.fs"
#load "LinearGauge.fs"
open EBrown.Graphing
open System.Drawing
open System.Drawing.Imaging
open System.Diagnostics
open System.IO

let basePath = @"C:\Users\ebrown\Desktop\"

let values = (150., 45., 101.325)
let g1() =
    let savePath = Path.Combine(basePath, "TestGauge.png")
    let padding = Rectangle.FromLTRB(10, 10, 10, 30)
    let config =
        { ArcGauge.Configuration.Height = 200
          ArcGauge.Configuration.Width = 400
          ArcGauge.Configuration.Padding = padding
          ArcGauge.Configuration.Font = None
          ArcGauge.Configuration.GaugeWidth = 64
          ArcGauge.Configuration.EmptyGaugeColor = Color.FromArgb(255, 192, 192, 192) |> Some
          ArcGauge.Configuration.FillGaugeColor = Color.FromArgb(255, 96, 16, 16) |> Some
          ArcGauge.Configuration.FontColor = Color.FromArgb(255, 32, 0, 0) |> Some
          ArcGauge.Configuration.OutlineColor = Color.FromArgb(255, 64, 64, 64) |> Some
          ArcGauge.Configuration.BackgroundColor = Color.White |> Some
          ArcGauge.Configuration.OutlineThickness = 1.25f |> Some }
    let image = values|||> ArcGauge.generate config float32 (fun f -> f.ToString("0 KPa"))
    image.Save(savePath, ImageFormat.Png)
    Process.Start savePath |> ignore

let g2() =
    let savePath = Path.Combine(basePath, "TestGauge.png")
    let padding = Rectangle.FromLTRB(10, 10, 10, 30)
    let config =
        { LinearGauge.Configuration.Height = 50
          LinearGauge.Configuration.Width = 400
          LinearGauge.Configuration.Padding = padding
          LinearGauge.Configuration.Font = None
          LinearGauge.Configuration.GaugeWidth = 32
          LinearGauge.Configuration.EmptyGaugeColor = Color.FromArgb(255, 192, 192, 192) |> Some
          LinearGauge.Configuration.FillGaugeColor = Color.FromArgb(255, 96, 16, 16) |> Some
          LinearGauge.Configuration.FontColor = Color.FromArgb(255, 32, 0, 0) |> Some
          LinearGauge.Configuration.OutlineColor = Color.FromArgb(255, 64, 64, 64) |> Some
          LinearGauge.Configuration.BackgroundColor = Color.White |> Some
          LinearGauge.Configuration.OutlineThickness = 1.25f |> Some }
    let image = values|||> LinearGauge.generate config float32 (fun f -> f.ToString("0 KPa"))
    image.Save(savePath, ImageFormat.Png)
    Process.Start savePath |> ignore

//() |> g1
//() |> g2
