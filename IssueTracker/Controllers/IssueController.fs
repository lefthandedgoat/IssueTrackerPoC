namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web
open System.Web.Security
open helpers
open issues
open comments

[<AllowAnonymous>]
type IssueController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Index (id : int) =
        let name = HttpContext.Current.User.Identity.Name
        let user = user.getUserByName name
        this.ViewData?Issue <- getIssueById id user.ClientId
        this.ViewData?Comments <- getCommentsByIssueId id
        this.View()           