#r "paket: groupref FakeBuild //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.IO
open Fake.IO.Globbing.Operators //enables !! and globbing
open Fake.DotNet
open Fake.Core

// Properties
let buildDir = "./build/"


// *** Define Targets ***
Target.create "Clean" (fun _ ->
  Shell.cleanDir buildDir
)

Target.create "Build" (fun _ ->
  !! "src/MongoChains.sln"
    |> MSBuild.runRelease (fun x -> { x with Targets = ["Clean; Restore; Build"]}) buildDir "Build"
    |> Trace.logItems "AppBuild-Output: "
)

Target.create "Package" (fun _ ->
  !! "src/MongoChains.sln"
    |> MSBuild.runRelease (fun x -> { x with Targets = ["Pack"]}) buildDir "Build"
    |> Trace.logItems "AppBuild-Output: "
)

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"Clean"
  ==> "Build"
  ==> "Package"

// *** Start Build ***
Target.runOrDefault "Package"