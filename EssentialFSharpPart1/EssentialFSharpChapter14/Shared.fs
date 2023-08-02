module GiraffeExample.Shared

open Giraffe.ViewEngine

// string -> XmlNode list -> XmlNode
let masterPage msg content =
    html [] [
        head [] [
            title [] [ str msg ]
            link [ _rel "stylesheet"; _href "css/main.css" ]
        ]
        body [] content
    ]