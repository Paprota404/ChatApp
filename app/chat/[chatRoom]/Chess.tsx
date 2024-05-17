'use client'
import React, { useState, useEffect, useRef } from "react";
import { Chess } from "chess.js";
import Chessboard from "chessboardjsx";

const ChessGame = () => {
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const [fen, setFen] = useState("start"); // FEN (Forsyth-Edwards Notation) represents the game state
 // Create a new chess game instance

 const chess = new Chess()
 chess.move('e4')
 chess.move('e5')
 chess.move('f4')
 chess.move('exf4')
 
 console.log(chess.history());
 console.log(chess.fen());

  // useEffect(() => {
  //   const handleResize = () => {
  //     setWindowWidth(window.innerWidth);
  //   };

  //   window.addEventListener("resize", handleResize);

  //   // Clean up the event listener on component unmount
  //   return () => window.removeEventListener("resize", handleResize);
  // }, []);

  // const chessboardWidth = Math.min(windowWidth / 1.3, 500);

 

  return (
    <Chessboard 
      position={fen} 
      // width={chessboardWidth} 
      
    />
  );
};

export default ChessGame;
