#load "Option.fs"
#load "General.fs"
#load @"Gauge\Configuration.fs"
#load @"Gauge\Arc.fs"
#load @"Gauge\Linear.fs"
open EBrown.Graphing
open EBrown.Graphing.General
open EBrown.Graphing.Gauge
open EBrown.Graphing.Gauge.Configuration
open System.Drawing
open System.Drawing.Imaging
open System.Diagnostics
open System.IO

let basePath = @"C:\Users\ebrown\Desktop\"

let values = (150., 45., 101.325)
let g1() =
    let savePath = Path.Combine(basePath, "TestGauge.png")
    let padding = Rectangle.FromLTRB(10, 10, 10, 30)
    let config = { Arc.defaultConfig with Padding = padding }
    use image = values|||> Arc.generate config float32 (fun f -> f.ToString("0 KPa"))
    image.Save(savePath, ImageFormat.Png)
    Process.Start savePath |> ignore

let g2() =
    let savePath = Path.Combine(basePath, "TestGauge2.png")
    let padding = Rectangle.FromLTRB(10, 10, 10, 30)
    let config = { Linear.defaultConfig with Padding = padding }
    use image = values|||> Linear.generate config float32 (fun f -> f.ToString("0 KPa"))
    image.Save(savePath, ImageFormat.Png)
    Process.Start savePath |> ignore

//() |> g1
//() |> g2
