namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web.Security
open helpers

[<AllowAnonymous>]
type UserController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Login () =         
        this.View()
            
    member private this.invalid username =
            this.ViewData?UserName <- username
            this.ViewData?Message <- "Invalid username or password"
            this.View() :> ActionResult

    [<HttpPost>]
    member this.Login (username, password : string) = 
        let user = auth.getUser username  
        if user.IsNone then
            this.invalid username
        else if auth.passwordCorrect password user.Value.Password user.Value.Salt then
            FormsAuthentication.SetAuthCookie(username, false)
            this.RedirectToAction("Index", "Home") :> ActionResult
        else
            this.invalid username

    [<HttpGet>]
    member this.Logout () = 
        FormsAuthentication.SignOut()
        this.RedirectToAction("Login", "User")