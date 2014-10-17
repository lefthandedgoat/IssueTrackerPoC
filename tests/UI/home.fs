module home

open canopy
open runner
open common
open users_page
open home_page

let all () =

    context "home tests"

    once (fun _ -> loginAs Admin)

    "Home and Logout links exists" &&& fun _ ->
        links *= "Home"
        links *= "Logout"

    "Table has Description, Id, More, and Create Date headers" &&& fun _ ->
        headers *~ "DESCRIPTION"
        headers *~ "ID"
        headers *~ "MORE"
        headers *~ "CREATE DATE"
        
    "There are 100 issues loaded" &&& fun _ ->
        count rows 100        
    
    "When I click issue #75 it takes me to the details of that record" &&& fun _ ->
        click <| issue 75
        on <| root + "/Issue/Index/75"