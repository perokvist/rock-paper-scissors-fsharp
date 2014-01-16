module RPS.GameHandler

open Microsoft.FSharp.Collections
open RPS.FSharp

let eventStore = [("a",[{GameCreatedEvent.playerName="per";name="stanley cup"} :> Event;{MoveMadeEvent.playerName="per";MoveMadeEvent.move=Move.Rock} :> Event])] |> Map.ofList

let load store id = 
    eventStore.[id]

let save store commandId events =
    ()
    
let rehydrate events =
    restoreState {creatorName="";creatorMove=Rock;gameState=NotStarted } events

let persist store id f =
    load store id |> rehydrate |> f |> save store id

let apply = persist eventStore

let handle (command:obj) = 
    match command with
    | :? MakeMoveCommand as c -> apply c.id (makeMove c)
    | :? CreateGameCommand as c -> apply c.id (createGame c) 
    | _ -> ()