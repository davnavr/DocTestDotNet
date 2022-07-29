[<RequireQualifiedAccess>]
module DocTestDotNet.Runner.TestRunner

open System.Collections.Generic

type Failure =
    { Message: string
      ExitCode: int }

val execute : options: Options -> tests: seq<string> -> Async<IReadOnlyCollection<Failure>>
