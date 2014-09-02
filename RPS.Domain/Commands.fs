namespace RPS
module Commands =
    open System
    open Common
    open Treefort

    type CreateGameCommand =
        {playerName: string
         firstMove: Move
         name:string
         id:System.Guid
         correlationId:System.Guid} 
            interface Commanding.ICommand with
                  member x.AggregateId = x.id
                  member x.CorrelationId = x.correlationId
   
    type MakeMoveCommand = 
       {move:Move
        playerName:string
        id:Guid
        correlationId:Guid }
        interface Treefort.Commanding.ICommand with
            member x.AggregateId = x.id
            member x.CorrelationId = Guid.NewGuid()



            