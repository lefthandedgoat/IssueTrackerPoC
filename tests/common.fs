﻿module common

let unit () = System.Environment.GetCommandLineArgs() |> Array.exists (fun arg -> arg = "unit")
let ui () = System.Environment.GetCommandLineArgs() |> Array.exists (fun arg -> arg = "ui")
let one () = System.Environment.GetCommandLineArgs() |> Array.exists (fun arg -> arg = "one")
let load () = System.Environment.GetCommandLineArgs() |> Array.exists (fun arg -> arg = "load")

let server = "localhost"
let port = ":48214"
let prefix = "http://"
let root = prefix + server + port

type Users =
    | Admin
    | Admin2