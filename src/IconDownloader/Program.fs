open FsHttp
open System.IO

let downloadIcon url (filePath: string) =
    http { GET url }
    |> Request.send
    |> Response.toStringAsync None
    |> Async.RunSynchronously
    |> fun svg ->
        let dir = Path.GetDirectoryName(filePath)

        if not <| Directory.Exists(dir) then
            Directory.CreateDirectory(dir) |> ignore

        File.WriteAllText(filePath, svg)


/// default
/// - weight 200 rounded 24px
/// - ex. https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/search/wght200/24px.svg
///
/// bold
/// - weight 400 rounded 24px
/// - ex. https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/search/default/24px.svg
///
/// filled
/// - ex. https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/favorite/fill1/24px.svg
type IconStyle =
    | Default
    | Bold
    | Filled

let getIconURL style name =
    match style with
    | Default -> $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/wght200/24px.svg"
    | Bold -> $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/default/24px.svg"
    | Filled -> $"https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsrounded/{name}/fill1/24px.svg"

open Argu

type Args =
    | Name of string
    | Style of IconStyle
    | Out of string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Name _ -> "specify icon name"
            | Style _ -> "specify icon style"
            | Out _ -> "specify output path"

[<EntryPoint>]
let main argv =

    let parseResults =
        let parser = ArgumentParser.Create<Args>(programName = "icon downloader")
        parser.ParseCommandLine argv

    let url, filePath =
        let name = parseResults.GetResult Name
        let style = parseResults.TryGetResult Style |> Option.defaultValue Default

        let outPath =
            parseResults.TryGetResult Out
            |> Option.defaultValue $"{__SOURCE_DIRECTORY__}/../../tmp"


        printfn $"name: {name}, style: {style}"

        let fileName =
            match style with
            | Default -> name
            | Bold -> $"{name}_bold"
            | Filled -> $"{name}_filled"

        let filePath = $"{outPath}/{fileName}.svg"

        let url = getIconURL style name
        printfn $"url: {url}"
        printfn $"filePath: {filePath}"
        url, filePath

    downloadIcon url filePath

    0
