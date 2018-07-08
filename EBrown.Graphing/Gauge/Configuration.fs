module EBrown.Graphing.Gauge.Configuration
open System.Drawing

type Configuration =
    { Height : int
      Width : int
      Padding : Rectangle
      GaugeWidth : int
      Font : Font option
      EmptyGaugeColor : Color
      FillGaugeColor : Color
      FontColor : Color
      OutlineColor : Color
      BackgroundColor : Color
      OutlineThickness : float32 }
    with
        // The following members exist to make life easier on the other .NET
        // languages, this allows for one to chain calls on a config object:
        // config.SetHeight(value).SetWidth(value)...
        // Since the record type is immutable, this allows C#/VB.NET consumers
        // to start with the `Default` configuration, and modify it to their
        // liking.
        member this.SetHeight value = { this with Height = value }
        member this.SetWidth value = { this with Width = value }
        member this.SetPadding value = { this with Padding = value }
        member this.SetGaugeWidth value = { this with GaugeWidth = value }
        member this.SetFont value = { this with Font = value }
        member this.SetEmptyGaugeColor value = { this with EmptyGaugeColor = value }
        member this.SetFillGaugeColor value = { this with FillGaugeColor = value }
        member this.SetFontColor value = { this with FontColor = value }
        member this.SetOutlineColor value = { this with OutlineColor = value }
        member this.SetBackgroundColor value = { this with BackgroundColor = value }
        member this.SetOutlineThickness value = { this with OutlineThickness = value }
      
let Default =
    { Height = 200
      Width = 400
      Padding = Rectangle()
      GaugeWidth = 64
      Font = None
      EmptyGaugeColor = Color.FromArgb(255, 192, 192, 192)
      FillGaugeColor = Color.FromArgb(255, 32, 96, 192)
      FontColor = Color.FromArgb(255, 0, 0, 0)
      OutlineColor = Color.FromArgb(255, 32, 32, 32)
      BackgroundColor = Color.White
      OutlineThickness = 1.25f }
