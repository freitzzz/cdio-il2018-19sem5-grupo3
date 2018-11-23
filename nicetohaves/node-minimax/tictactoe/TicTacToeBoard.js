/**
 * Requires Board module as the base board logic
 */
const Board=require('../board/Board');

/**
 * Represents a Tic-Tac-Toe type board
 */
class TicTacToeBoard extends Board{
    /**
     * Builds a new classic Tic-Tac-Toe board (3x3)
     */
    constructor(){
        super();
        for(let i=0;i<9;i++)this._board.push(null);
    }

}

/**
 * Exports TicTacToeBoard module
 */
module.exports=TicTacToeBoard;