[<RequireQualifiedAccess>]
module DocTestDotNet.Runner.TestRunner

open System.Collections.Immutable

type Failure =
    { Message: string
      ExitCode: int }

val execute : options: Options -> tests: seq<string> -> Async<ImmutableArray<Failure>>
