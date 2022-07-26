module DocTestDotNet.Tests.Program

open DocTestDotNet.Xml

open Expecto

[<EntryPoint>]
let main argv =
    testList "tests" [
        testCase "simple" <| fun _ ->
            let output =
                System.Text.StringBuilder()
                    .AppendLine("""<?xml version="1.0" encoding="utf-8"?>""")
                    .AppendLine("""<doc>""")
                    .AppendLine("""<assembly><name>MyAssemblyName</name></assembly>""")
                    .AppendLine("""<members>""")
                    .AppendLine("""<member name="T:MyNamespace.MyType">""")
                    .AppendLine("""</member>""")
                    .AppendLine("""</members>""")
                    .AppendLine("""</doc>""")
                    .ToString()
                |> DocumentationParser.parseTestsFromString

            Expect.equal output.Assembly "MyAssemblyName" "assembly name is incorrect"
    ]
    |> runTestsWithCLIArgs List.empty argv
    |> exit
