module Game
open System

type Event = 
    interface
    end
    
type Move=
  | Rock
  | Paper
  | Scissors

type GameResult =
  | PlayerOneWin
  | PlayerTwoWin
  | Tie

let wins playerOneMove playerTwoMove=
    match playerOneMove,playerTwoMove with 
    | Move.Rock,Move.Paper -> GameResult.PlayerTwoWin
    | Move.Scissors,Move.Rock -> GameResult.PlayerTwoWin
    | Move.Paper,Move.Scissors -> GameResult.PlayerTwoWin
    | x,y when x=y -> GameResult.Tie
    | _ -> GameResult.PlayerOneWin
    
type CreateGameCommand={
    playerName: string
    firstMove: Move
    name:string
    id:Guid
}

type GameState=
    | NotStarted
    | Created 
    | Started
    | Ended

type Player={
    name:string
    moves:seq<Move>
}

type State={
    gameState:GameState
    creatorName: string
    creatorMove: Move
}


type MoveMadeEvent=
    {
    playerName:string
    move:Move
    } 
    interface Event

type GameCreatedEvent=
   {
    name:string
    playerName: string
   }
   interface Event

type GameEndedEvent = { result: GameResult; players : string * string } interface Event



type EventEntity=
    | Event of GameCreatedEvent
    | Events of seq<GameCreatedEvent>

let createGame (command:CreateGameCommand) state : list<Event> =
   match state.gameState with
    | GameState.NotStarted ->
        [{ name = command.name; playerName = command.playerName};
         { move = command.firstMove; playerName = command.playerName } ]
    | _ -> List.empty

type MakeMoveCommand = {
    move:Move
    playerName:string
    id:Guid
}

let isValidPlayer playerName state =
    state.creatorName <> playerName



let makeMove (command:MakeMoveCommand) state : list<Event> =
    match state.gameState with
    | GameState.Started when isValidPlayer command.playerName state ->
        let result = wins state.creatorMove command.move
        [{ MoveMadeEvent.playerName = command.playerName; move = command.move };
         { GameEndedEvent.result = result; players = (state.creatorName, command.playerName) } ]
    |_ -> List.empty 

let restoreState state (events:list<Event>) :State=
    let step (evt:Event) (state:State) =
        match evt with
        | :? GameCreatedEvent as e ->
                { gameState = GameState.Started; creatorName = e.playerName; creatorMove = state.creatorMove }
        | :? MoveMadeEvent as e when e.playerName = state.creatorName ->
                { gameState = state.gameState; creatorName = state.creatorName; creatorMove = e.move }
        | :? GameEndedEvent as e -> { state with gameState = GameState.Ended }
        |_ -> state

    List.foldBack step events state


