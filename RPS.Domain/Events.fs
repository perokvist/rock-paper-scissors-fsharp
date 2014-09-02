module RPS.Events
open System
open Commands
open Common
open Treefort

type MoveMadeEvent=
    {
    playerName:string
    move:Move
    gameId:Guid
    mutable correlationId:Guid
    } 
    interface Events.IEvent with
        member x.SourceId = x.gameId
        member x.CorrelationId with get() = x.correlationId
        member x.CorrelationId with set(v) = x.correlationId <- v

type GameCreatedEvent=
   {
    name:string
    playerName: string
    gameId:Guid
    mutable correlationId:Guid
   }
   interface Events.IEvent with
    member x.SourceId = x.gameId
    member x.CorrelationId = x.correlationId
    member x.CorrelationId with set(v) = x.correlationId <- v

type GameEndedEvent = 
    { 
    result: GameResult; 
    players : string * string
    gameId:Guid
    mutable correlationId:Guid 
    } 
        interface Events.IEvent with
            member x.SourceId = x.gameId
            member x.CorrelationId = x.correlationId
            member x.CorrelationId with set(v) = x.correlationId <- v


