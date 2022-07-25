module DocTestDotNet.Tests.Program

open DocTestDotNet.Xml

open Expecto

[<EntryPoint>]
let main argv =
    testList "tests" [
        testCase "simple" <| fun _ ->
            let output =
                """
                <?xml version="1.0" encoding="utf-8"?>
                <doc>
                <assembly><name>DocTestDotNet.Core</name></assembly>
                </doc>
                """
                |> Parser.parseTestsFromString

            Expect.equal output.AssemblyName "MyAssemblyName" "assembly name is incorrect"
    ]
    |> runTestsWithCLIArgs List.empty argv
    |> exit
