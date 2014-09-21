namespace IssueTracker.Controllers

open System.Web.Mvc

[<Authorize>]
type HomeController() =
    inherit Controller()
    member this.Index () = this.View()


