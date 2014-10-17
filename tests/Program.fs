open canopy
open runner
open common

if unit() then
    //todo write some!
    ()

if ui() then
    //turn failFast on so that the first time a test fails, it stops running tests
    canopy.configuration.failFast := true
    //make canopy try assertions etc for a shorter period of time.  Default is 10 seconds
    canopy.configuration.compareTimeout <- 3.0
    canopy.configuration.elementTimeout <- 3.0
    canopy.configuration.pageTimeout <- 3.0

    start chrome //requires chrome driver so download and put it in c:/ if you are trying to run this
                 //version as of 9/21/2014 is 2.10 http://chromedriver.storage.googleapis.com/index.hml
                 //OR switch to 'start firefox'
    if one() then uitests.one()
    else uitests.many()
    run()

    System.Console.ReadKey() |> ignore
    quit()

if load() then
    loadScript.run()