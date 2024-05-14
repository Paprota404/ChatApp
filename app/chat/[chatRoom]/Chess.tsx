import React, { useState } from 'react';
import Chess from 'chess.js';
import Chessboard from 'chessboardjsx';

const ChessGame = () => {
 

  return (
    <Chessboard showNotation={true} sparePieces={true} position="start" width={500} />
  );
};

export default ChessGame;