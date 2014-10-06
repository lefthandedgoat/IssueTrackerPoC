open canopy
open runner
open common

if unit() then
    //todo write some!
    ()

if ui() then
    canopy.configuration.failFast := true
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