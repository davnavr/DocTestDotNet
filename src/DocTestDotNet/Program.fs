module private DocTestDotNet.Program

open System.IO

open Argu

type Options =
    | [<Mandatory; AltCommandLine("-i")>] Include of ``doc.xml``: string
    | [<Unique; AltCommandLine("-o")>] Output_Directory of directory: string
    | [<Unique; AltCommandLine("-f")>] Target_Framework of string
    | Reference_Assembly of ``lib.dll``: string
    | [<Unique; AltCommandLine("-v")>] Verbosity of level: Runner.Verbosity
    | [<Unique>] Dotnet_Cli_Path of string
    | Build_Only
    | [<Unique>] Test_Timeout of milliseconds: int
    | Launch_Self_Debugger

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Include _ -> "specifies an XML documentation file to generate tests for"
            | Output_Directory _ -> "directory that the generated tests are written to, defaults to the current directory"
            | Target_Framework _ -> "the target framework of generated test projects"
            | Reference_Assembly _ -> "includes the specified assembly as a dependency for generated tests"
            | Verbosity _ -> "the verbosity used when executing the generated tests"
            | Dotnet_Cli_Path _ -> "explicitly specifies the path to the .NET CLI executable"
            | Build_Only -> "if set, does not execute the generated tests"
            | Test_Timeout _ -> "specifies the maximum amount of time a test is allowed to execute, in milliseconds"
            | Launch_Self_Debugger -> "launches the debugger used to debug the test generator, NOT the tests themselves"

[<EntryPoint>]
let main args =
    try
        let options = ArgumentParser<Options>.Create().ParseCommandLine(inputs = args, raiseOnUsage = true)

        if options.Contains <@ Launch_Self_Debugger @> then System.Diagnostics.Debugger.Launch() |> ignore

        let documentation =
            use xmlDocumentationSource = new StreamReader(options.GetResult <@ Include @>)
            Xml.DocumentationParser.parseTestsFromReader(System.Xml.XmlReader.Create xmlDocumentationSource)

        let tests =
            let testProjectReferences =
                List.map Writer.ProjectReference.Dll (options.GetResults <@ Reference_Assembly @>)

            Writer.TestWriter.writeTestsToPath
                (options.GetResult(<@ Output_Directory @>, defaultValue = System.Environment.CurrentDirectory))
                testProjectReferences
                (options.GetResult(<@ Target_Framework @>, defaultValue = "net6.0"))
                Writer.TestWriter.defaultWriterLookup
                documentation

        if options.Contains <@ Build_Only @> then
            0
        else
            let failures =
                let testRunnerOptions = Runner.Options()
                testRunnerOptions.Verbosity <-  options.GetResult(<@ Verbosity @>, defaultValue = Runner.Verbosity.Default)
                testRunnerOptions.DotNetPath <-
                    options.TryGetResult <@ Dotnet_Cli_Path @>
                    |> Option.map Runner.DotNetPath.Explicit
                    |> Option.defaultValue Runner.DotNetPath.Inferred

                let testRunnerTimeout = options.GetResult(<@ Test_Timeout @>, defaultValue = 10000)

                Async.RunSynchronously(Runner.TestRunner.execute testRunnerOptions tests, timeout = testRunnerTimeout)

            if failures.Count = 0 then
                0
            else
                for error in failures do
                    eprintfn "Test exited with code %i:" error.ExitCode
                    stderr.WriteLine error.Message
                -1
    with
    | :? ArguException as ex ->
        stderr.WriteLine ex.Message
        -1
