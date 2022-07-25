namespace DocTestDotNet

open System.Runtime.CompilerServices

[<RequireQualifiedAccess; IsReadOnly; Struct; NoComparison; StructuralEquality>]
type SourceLanguage =
    | CSharp
    | FSharp

type DocumentationTest =
    { Language: SourceLanguage
      Name: string
      Code: string }
