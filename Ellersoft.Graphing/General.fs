module EBrown.Graphing.General
open System.Drawing

let drawLabel (g : Graphics) (bounds : Rectangle) font brush str (ptLoc : PointF) =
    let measurements = g.MeasureString(str, font)
    g.DrawString(
        str,
        font,
        brush,
        PointF(
            min (max (ptLoc.X) (bounds.Left |> float32)) ((bounds.Right |> float32) - measurements.Width),
            max (min (ptLoc.Y) ((bounds.Bottom |> float32) - measurements.Height)) (bounds.Top |> float32)))

let drawLabelCentered (g : Graphics) (bounds : Rectangle) font brush str (ptLoc : PointF) =
    let measurements = g.MeasureString(str, font)
    drawLabel g bounds font brush str (PointF(ptLoc.X - measurements.Width * 0.5f, ptLoc.Y))
    