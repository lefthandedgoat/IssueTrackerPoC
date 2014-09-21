namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web.Security
open helpers

[<AllowAnonymous>]
type UserController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Login () = 
        let isAuthed = (System.Web.HttpContext.Current.User <> null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated
        this.ViewData?IsAuthed <- isAuthed
        this.View()

    [<HttpPost>]
    member this.Login (username, password : string) = 
        let user = auth.getUser username        
        if user.IsSome && (auth.passwordCorrect password user.Value.Password user.Value.Salt) then
            FormsAuthentication.SetAuthCookie(username, false)
            this.RedirectToAction("Index", "Home") :> ActionResult
        else
            this.View() :> ActionResult            

    [<HttpGet>]
    member this.Logout () = 
        FormsAuthentication.SignOut()
        this.RedirectToAction("Login", "User")