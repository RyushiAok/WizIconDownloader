open FsHttp

let SAVE_PATH = $"{__SOURCE_DIRECTORY__}/../../tmp"

let downloadIcon url filename =
    http { GET url }
    |> Request.send
    |> Response.saveFile ($"{SAVE_PATH}/{filename}.svg")

let getDefaultIconURL name =
    $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/wght200/24px.svg"

let getBoldIconURL name =
    $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/default/24px.svg"

let getFilledIconURL name =
    $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/fill1/24px.svg"

open Argu

type IconStyle =
    | Default
    | Bold
    | Filled

type Args =
    | Name of string
    | Style of IconStyle

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Name _ -> "specify icon name"
            | Style _ -> "specify icon style"

[<EntryPoint>]
let main argv =
    let parser = ArgumentParser.Create<Args>(programName = "ls")
    let parseResults = parser.ParseCommandLine argv
    let name = parseResults.GetResult Name
    let style = parseResults.TryGetResult Style |> Option.defaultValue Default
    name |> printfn "Icon Name: %A"
    style |> printfn "Icon Style: %A"

    let url, filename =
        match style with
        | Default -> getDefaultIconURL name, name
        | Bold -> getBoldIconURL name, $"{name}_bold"
        | Filled -> getFilledIconURL name, $"{name}_filled"

    url |> printfn "Icon URL: %A"
    filename |> printfn "Icon Filename: %A"
    downloadIcon url filename
    0


// default: weight 200 rounded 24px
// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/search/wght200/24px.svg
// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/home/wght200/24px.svg

// bold; weight 400 rounded 24px
// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/search/default/24px.svg

// filled
// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/favorite/fill1/24px.svg
// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/check_circle/fill1/24px.svg
