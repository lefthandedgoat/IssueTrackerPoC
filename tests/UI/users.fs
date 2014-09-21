module users

open canopy
open runner
open common

//helpers
let home = root + "/User/Login"
let redirect = root + "/User/Login?ReturnUrl=%2f"

//selectors
let username = "input[name='username']"
let password = "input[name='password']"

//tests
let all () =
    "when you try to go home and you are not logged in you are redirected to login page" &&& fun _ ->
        url root
        on redirect

    "attempt to login with a username that does not exist and it tells you bad username/password" &&& fun _ ->
        username << "bad username"
        password << "does not matter"
        click "Login"

        on home