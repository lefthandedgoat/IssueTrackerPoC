module common

let boolToBit boool = if boool then 1 else 0

let createConnectionString connection database =
    let connection = Printf.StringFormat<string->string>(connection)
    sprintf connection database