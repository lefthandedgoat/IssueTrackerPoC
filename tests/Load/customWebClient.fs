module CustomWebClient

open System.Net
open System

type CustomWebClient() =
    inherit WebClient()
        member val CookieContainer : CookieContainer = new CookieContainer() with get, set
        
        member x.AddCookie name value path =        
            let cookie = new Cookie(name, value)
            x.CookieContainer.Add(new Uri(path), cookie)

    override x.GetWebRequest address =        
        let request = base.GetWebRequest(address) :?> HttpWebRequest
        request.CookieContainer <- x.CookieContainer;
        request.AutomaticDecompression <- DecompressionMethods.Deflate ||| DecompressionMethods.GZip;

        request :> WebRequest