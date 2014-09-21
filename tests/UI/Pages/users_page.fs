module users_page

open common

//urls
let login = root + "/User/Login"
let redirect = root + "/User/Login?ReturnUrl=%2f"

//selectors
let username = "input[name='username']"
let password = "input[name='password']"
let message = "#message"