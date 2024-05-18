import React, { useState } from "react";
import Chessboard from "chessboardjsx";
import { Chess } from "chess.js";

const ChessGame = () => {
  const [chess] = useState(new Chess());
  const [fen, setFen] = useState("start");
  const [clickedSquare, setClickedSquare] = useState(null);
  const [possibleMoves, setPossibleMoves] = useState({});

  const onDrop = ({ sourceSquare, targetSquare }) => {
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

    if (move === null) {
      return;
    }

   
    setFen(chess.fen());
    setClickedSquare(null); // Clear the clicked square after a successful move
    setPossibleMoves({}); // Clear possible moves after a successful move
  };

  const onSquareClick = (square) => {
    setClickedSquare(square);
  };

  const onMouseOverSquare = (square) => {
    const moves = chess.moves({
      square: square,
      verbose: true,
    });

    if (moves.length > 0) {
      const newPossibleMoves = {};
      moves.forEach((move) => {
        newPossibleMoves[move.to] = { backgroundColor: "rgba(0, 255, 0, 0.4)" };
      });
      setPossibleMoves(newPossibleMoves);
    }
  };

  const onMouseOutSquare = () => {
    setPossibleMoves({});
  };

  const highlightSquareStyle = {
    backgroundColor: "rgba(255, 255, 0, 0.4)",
  };

  const squareStyles = {
    ...possibleMoves,
    ...(clickedSquare ? { [clickedSquare]: highlightSquareStyle } : {}),
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
    </div>
  );
};

export default ChessGame;
