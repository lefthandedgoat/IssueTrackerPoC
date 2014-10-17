module home_page

open common

//urls
let index = root + ""

//selectors
let links = ".navbar-nav li a"
let headers = ".tablesorter-header-inner"
let rows = ".issue-row"

//helpers

let issue id = sprintf "tr[data-issue-id='%i']" id