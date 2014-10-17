module users

open canopy
open runner
open common
open users_page

//tests
let all () =
    context "users tests"

    "when you try to go home and you are not logged in you are redirected to login page" &&& fun _ ->
        url root
        on redirect

    "attempt to login with a username that does not exist and it tells you invalid username/password" &&& fun _ ->
        username << "bad username"
        password << "does not matter"
        click "Login"

        on login
        message == "Invalid username or password"
        //retains username
        username == "bad username"

    "attempt to login with a bad password tells you invalid username/password" &&& fun _ ->
        username << "admin"
        password << "adsfafsaffsadf"
        click "Login"

        on login
        message == "Invalid username or password"
        //retains username
        username == "admin"

    "logging in with valid username and password takes you to home/index" &&& fun _ ->
        username << "admin"
        password << "test1234"
        click "Login"

        on home_page.index  

    "once logged in you can log out" &&& fun _ ->
        click "Logout"
        on login