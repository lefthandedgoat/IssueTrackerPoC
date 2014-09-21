namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web.Security
open helpers

[<AllowAnonymous>]
type AccountController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Login () = 
        let isAuthed = (System.Web.HttpContext.Current.User <> null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated
        this.ViewData?IsAuthed <- isAuthed
        this.View()

    [<HttpPost>]
    member this.Login (username, password) = 
        if true then
            let a: string = password
            FormsAuthentication.SetAuthCookie(username, false)
            this.RedirectToAction("Index", "Home") :> ActionResult
        else
            this.View() :> ActionResult

    [<HttpGet>]
    member this.Logout () = 
        FormsAuthentication.SignOut()
        this.RedirectToAction("Login", "Account")