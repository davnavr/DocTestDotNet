namespace DocTestDotNet

[<Struct>]
type SourceLanguage = CSharp | FSharp

type DocumentationTest = { Language: SourceLanguage; Name: string; Code: string }
