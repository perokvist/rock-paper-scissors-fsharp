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
    
//let gameId = Guid.NewGuid();
//let eventStore =
//    Map.empty
//        .Add(gameId, [{GameCreatedEvent.playerName="per";name="stanley cup"; gameId = gameId; correlationId = Guid.Empty } :> Events.IEvent;{MoveMadeEvent.playerName="per";MoveMadeEvent.move=Move.Rock; gameId = gameId; correlationId = Guid.Empty} :> Events.IEvent ])
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