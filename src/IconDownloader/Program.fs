// https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsoutlined/add/default/24px.svg

open FsHttp

http { GET "https://fonts.gstatic.com/s/i/short-term/release/materialsymbolsoutlined/add/default/24px.svg" }
|> Request.send
|> Response.saveFile ($"{__SOURCE_DIRECTORY__}/add.svg")
|> printfn "%A"
