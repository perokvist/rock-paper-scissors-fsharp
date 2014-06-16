module RPS.GameHandler

open Microsoft.FSharp.Collections
open Game

let mutable eventStore = Map.empty

let load store aggregateId = 
    if eventStore.ContainsKey aggregateId
    then eventStore.[aggregateId]
    else List.empty

let save store aggregateId events = 
    eventStore <- Map.add aggregateId events eventStore
    
let rehydrate events =
    List.foldBack applyEvent events {State.creatorName="";State.creatorMove=Rock;State.gameState=NotStarted }


let persist store id f =
    load store id |> rehydrate |> f |> save store id

let apply = persist eventStore

let handle (command:obj) = 
    match command with
    | :? MakeMoveCommand as c -> apply c.id (makeMove c)
    | :? CreateGameCommand as c -> apply c.id (createGame c) 
    | _ -> ()