module RPS.Game
open System
open Commands
open Events
open Common 
open Treefort

type GameState=
    | NotStarted
    | Created 
    | Started
    | Ended

type State={
    gameState:GameState
    creatorName: string
    creatorMove: Move
}


let wins playerOneMove playerTwoMove=
    match playerOneMove,playerTwoMove with 
    | Common.Move.Rock,Move.Paper -> GameResult.PlayerTwoWin
    | Move.Scissors,Move.Rock -> GameResult.PlayerTwoWin
    | Move.Paper,Move.Scissors -> GameResult.PlayerTwoWin
    | x,y when x=y -> GameResult.Tie
    | _ -> GameResult.PlayerOneWin
    
let createGame (command:CreateGameCommand) state : list<Events.IEvent> =
   match state.gameState with
    | GameState.NotStarted ->
        [{ GameCreatedEvent.name = command.name; playerName = command.playerName; gameId = command.id; correlationId = command.correlationId};
         { MoveMadeEvent.move = command.firstMove; playerName = command.playerName; gameId = command.id; correlationId = command.correlationId } ]
    | _ -> List.empty

     
let isValidPlayer playerName state =
    state.creatorName <> playerName

let makeMove (command:MakeMoveCommand) state : list<Events.IEvent> =
    match state.gameState with
    | GameState.Started when isValidPlayer command.playerName state ->
        let result = wins state.creatorMove command.move
        [{ MoveMadeEvent.playerName = command.playerName; move = command.move; gameId = command.id; correlationId = command.correlationId  };
         { GameEndedEvent.result = result; players = (state.creatorName, command.playerName); gameId = command.id; correlationId = command.correlationId } ]
    |_ -> List.empty 

let applyEvent (state:State) (evt:Events.IEvent) = 
    match evt with
        | :? GameCreatedEvent as e ->
                { gameState = GameState.Started; creatorName = e.playerName; creatorMove = state.creatorMove }
        | :? MoveMadeEvent as e when e.playerName = state.creatorName ->
                { gameState = state.gameState; creatorName = state.creatorName; creatorMove = e.move }
        | :? GameEndedEvent as e -> { state with gameState = GameState.Ended }
        |_ -> state
