namespace IssueTracker.Controllers

open System.Web.Mvc
open System.Web
open helpers
open issues

//[<Authorize>]
type HomeController() =
    inherit Controller()
    member this.Index () =         
        //this assume usernames are unique across the system
//        let name = HttpContext.Current.User.Identity.Name
//        let user = user.getUserByName name
        this.ViewData?Issues <- getIssuesByClientId 1 //user.ClientId
        this.View()