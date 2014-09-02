module RPS.Common
open System

type Move=
    | Rock
    | Paper
    | Scissors

type GameResult =
  | PlayerOneWin
  | PlayerTwoWin
  | Tie
