import React, { useState } from "react";
import Chessboard from "chessboardjsx";
import { Chess } from "chess.js";

const ChessGame = () => {
  const [chess] = useState(new Chess());
  const [fen, setFen] = useState("start");
  const [clickedSquare, setClickedSquare] = useState(null);
  const [possibleMoves, setPossibleMoves] = useState({});
  const [lastMove, setLastMove] = useState<{ from: string; to: string } | null>(
    null
  );
  const [gameOver, setGameOver] = useState(false);
  const [winner, setWinner] = useState("");

  

  const onDrop = ({ sourceSquare, targetSquare } : { sourceSquare: any , targetSquare: any}) => {
    // Get all legal moves from the source square
    const legalMoves = chess.moves({ square: sourceSquare, verbose: true });

    // Check if the target square is a valid destination for the piece
    const isLegalMove = legalMoves.some((move) => move.to === targetSquare);

    if (!isLegalMove) {
      return;
    }

    let move = chess.move({
      from: sourceSquare,
      to: targetSquare,
      promotion: "q", // always promote to a queen for simplicity
    });

    if (move !== null) {
      setLastMove({ from: sourceSquare, to: targetSquare });
    }

    if (move === null) {
      return;
    }

    if (chess.isStalemate()) {
      setWinner("Draw");
      setGameOver(true);
    }

    if (chess.isCheckmate()) {
      const winnerColor = chess.turn() === "b" ? "White" : "Black";
      setWinner(winnerColor);
      setGameOver(true);
    }

    setFen(chess.fen());
    setClickedSquare(null); // Clear the clicked square after a successful move
    setPossibleMoves({}); // Clear possible moves after a successful move
  };

  const onSquareClick = (square:any) => {
    setClickedSquare(square);
  };

  const onMouseOverSquare = (square:any) => {
    const moves = chess.moves({
      square: square,
      verbose: true,
    });

    if (moves.length > 0) {
      const newPossibleMoves = {};
      moves.forEach((move) => {
        newPossibleMoves[move.to] = { backgroundColor: "rgba(0, 255, 0, 0.2)" };
      });
      setPossibleMoves(newPossibleMoves);
    }
  };

  const onMouseOutSquare = () => {
    setPossibleMoves({});
  };

  const highlightSquareStyle = {
    backgroundColor: "rgba(255, 255, 0, 0.2)",
  };

  const squareStyles = {
    ...possibleMoves,
    ...(clickedSquare ? { [clickedSquare]: highlightSquareStyle } : {}),
    ...(lastMove
      ? {
          [lastMove.from]: { backgroundColor: "rgba(255, 255, 0, 0.5)" },
          [lastMove.to]: { backgroundColor: "rgba(255, 255, 0, 0.5)" },
        }
      : {}),
  };

  return (
    <div className="flex flex-col items-center">
      <Chessboard
        position={fen}
        onDrop={onDrop}
        onSquareClick={onSquareClick}
        onMouseOverSquare={onMouseOverSquare}
        onMouseOutSquare={onMouseOutSquare}
        squareStyles={squareStyles}
        draggable={true}
      />
      {gameOver && winner !== "Draw" && (
        <div className="absolute inset-0 w-full h-full flex items-center justify-center bg-opacity-50 bg-black text-white text-lg">
          {winner} Wins!
        </div>
      )}
      {gameOver && winner == "Draw" && (
        <div className="absolute inset-0 w-full h-full flex items-center justify-center bg-opacity-50 bg-black text-white text-lg">
          {winner}!
        </div>
      )}
    </div>
  );
};

export default ChessGame;
