module users_page

open common
open canopy

//urls
let login = root + "/User/Login"
let redirect = root + "/User/Login?ReturnUrl=%2f"

//selectors
let username = "input[name='username']"
let password = "input[name='password']"
let message = "#message"

//helpers
let loginAs user =
    let loginAs username_ password_ =
        url login
        username << username_
        password << password_
        click "Login"
    
    match user with
    | Admin -> loginAs "admin" "test1234"
    | Admin2 -> loginAs "admin2" "test1234"