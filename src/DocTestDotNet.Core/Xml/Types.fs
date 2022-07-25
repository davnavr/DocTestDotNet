namespace DocTestDotNet.Xml

open System.Runtime.CompilerServices

[<RequireQualifiedAccess; IsReadOnly; Struct; NoComparison; StructuralEquality>]
type SourceLanguage =
    | CSharp
    | FSharp

type ParsedTest =
    { Language: SourceLanguage
      Name: string
      Code: string }

type ParserOutput =
    { AssemblyName: string
      Tests: System.Collections.Generic.IReadOnlyCollectio<ParsedTest> }
