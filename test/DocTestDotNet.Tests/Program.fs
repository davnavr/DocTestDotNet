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
                    .AppendLine("""<example>""")
                    .AppendLine("""<code testName="HelloExample" testLanguage="csharp">""")
                    .AppendLine("""System.Console.WriteLine("Hello There!");""")
                    .AppendLine("""</code>""")
                    .AppendLine("""</example>""")
                    .AppendLine("""</member>""")
                    .AppendLine("""</members>""")
                    .AppendLine("""</doc>""")
                    .ToString()
                |> DocumentationParser.parseTestsFromString

            Expect.equal output.Assembly "MyAssemblyName" "assembly name is incorrect"
            Expect.equal output.Members[0].Member "T:MyNamespace.MyType" "member name is incorrect"
            Expect.equal output.Members[0].Tests[0].Name "HelloExample" "test name is incorrect"
            Expect.equal output.Members[0].Tests[0].Language SourceLanguage.CSharp "test code is incorrect"
    ]
    |> runTestsWithCLIArgs List.empty argv
    |> exit
