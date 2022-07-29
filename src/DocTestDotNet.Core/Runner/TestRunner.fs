module DocTestDotNet.Runner.TestRunner

open System.Collections.Immutable
open System.Diagnostics
open System.Threading

type Failure = { Message: string; ExitCode: int }

let beginTestExecution (options: Options) (tests: seq<string>) =
    let verbosity = "--verbosity" + options.Verbosity.ToString()
    
    tests
    |> Seq.map (fun project -> async {
        let config = ProcessStartInfo(options.DotNetPath.ToString())
        config.ArgumentList.Add "run"
        config.ArgumentList.Add "--project"
        config.ArgumentList.Add project
        config.ArgumentList.Add verbosity
        config.UseShellExecute <- false
        config.RedirectStandardError <- true

        let dotnet = Process.Start config
        let! errors = Async.AwaitTask(dotnet.StandardError.ReadToEndAsync())
        let! token = Async.CancellationToken
        let! () = Async.AwaitTask(dotnet.WaitForExitAsync token)
        return
            if dotnet.ExitCode = 0
            then None
            else Some { Message = errors; ExitCode = dotnet.ExitCode }
    })
    |> Async.Parallel

let execute options tests =
    async {
        let! results = beginTestExecution options tests
        let errors = ImmutableArray.CreateBuilder results.Length
        for r in results do
            match r with
            | Some failure -> errors.Add failure
            | None -> ()
        return errors.ToImmutable()
    }
