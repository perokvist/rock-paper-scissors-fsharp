module RPS.GameHandler

open Microsoft.FSharp.Collections
open Game
open Commands
open Events
open Common
open System
open Treefort

// Dummy mutable event store, to be replaced with real event store :D
let mutable eventStore = Map.empty

let load store aggregateId = 
    if eventStore.ContainsKey aggregateId
    then eventStore.[aggregateId]
    else List.empty

let append store aggregateId events = 
    let oldList = load eventStore aggregateId
    let newList = List.append oldList events
    eventStore <- Map.add aggregateId newList eventStore
    
let rehydrate events =
    List.fold applyEvent {State.creatorName="";State.creatorMove=Move.Rock;State.gameState=NotStarted } events 

let persist store id f =
    load store id |> rehydrate |> f |> append store id

let apply = persist eventStore

let handle (command:obj) = 
    match command with
    | :? MakeMoveCommand as c -> apply c.id (makeMove c)
    | :? CreateGameCommand as c -> apply c.id (createGame c) 
    | _ -> ()