module comments

open common

let comment issueId value = sprintf "INSERT INTO dbo.Comments VALUES ('%i', '%s')" issueId value

let add connection database =
    let connection = createConnectionString connection database
    
    let sqls =
        [| 1 .. (clients.number * issues.number) |]
        |> Array.map (fun issueId ->
            [|
                comment issueId "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam finibus feugiat nibh, sed finibus ligula euismod ut. Pellentesque urna tortor, pharetra a turpis non, placerat laoreet massa. Sed malesuada tortor metus, eu sodales odio cursus vitae. Nunc odio sem, faucibus eu metus scelerisque, fringilla accumsan augue."
                comment issueId "Aliquam feugiat elit elementum enim porttitor, ac fringilla mauris ullamcorper. Quisque pharetra purus ut erat euismod luctus."
                comment issueId "Vivamus quis lorem sit amet magna pharetra fermentum in at dui. Integer scelerisque est ac lacus sollicitudin, nec maximus nisi pretium. Sed faucibus nisi vitae enim pellentesque scelerisque. Morbi semper ullamcorper rhoncus. Nam lacinia, ipsum in commodo tincidunt, metus enim volutpat tortor, id tempus sapien felis quis ipsum. Vivamus eget risus velit. Nulla malesuada lectus convallis quam ornare fermentum. Pellentesque accumsan, leo at commodo rhoncus, elit est iaculis enim, ut pretium mauris massa eu est. Donec nec dapibus eros."
                comment issueId "Nam nec felis mollis, volutpat arcu ac, porta felis. Donec malesuada magna ac dictum eleifend. Nullam tristique magna turpis, id tempor metus eleifend eu. Aenean at egestas metus."
                comment issueId "Fusce molestie fermentum lectus, ut condimentum orci vehicula vitae."
            |])
        |> Array.concat

    sqls
    |> migration.batch 5000
    |> List.iter (fun sql -> migration.executeNonQuery connection sql)