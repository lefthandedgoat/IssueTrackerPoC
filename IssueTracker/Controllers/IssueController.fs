namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web.Security
open helpers
open issues
open comments

[<AllowAnonymous>]
type IssueController() =
    inherit Controller()
    
    [<HttpGet>]
    member this.Index (id : int) =
        this.ViewData?Issue <- getIssueById id 1 //user.ClientId
        this.ViewData?Comments <- getCommentsByIssueId id
        this.View()           