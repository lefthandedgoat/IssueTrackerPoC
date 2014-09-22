namespace IssueTracker.Controllers

open System.Web.Mvc
open helpers
open issues

[<Authorize>]
type HomeController() =
    inherit Controller()
    member this.Index () = 
        //todo determine which client the user is for and only get their items, unless their client is admin then access to all
        this.ViewData?Issues <- getIssuesByClientId 1
        this.View()