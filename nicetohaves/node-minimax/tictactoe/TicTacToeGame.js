/**
 * Requires TicTacToeUtils class for Tic-Tac-Toe game utility functions
 */
const TIC_TAC_TOE_UTILS=require('./TicTacToeUtils');

/**
 * Requires TicTacToeBoard module for Tic-Tac-Toe board logic
 */
const TicTacToeBoard=require('./TicTacToeBoard');

/**
 * Requires Game module for the base game logic
 */
const Game=require('../game/Game');

/**
 * Represents a Tic-Tac-Toe game
 */
class TicTacToeGame extends Game{
    /**
     * Builds a new classic Tic-Tac-Toe game (3x3 Board with 2 players)
     * @param {Player} playerX Player with the first player
     * @param {Player} playerY Player with the second player
     */
    constructor(playerX,playerY){
        super(playerX,playerY);
        this._board=new TicTacToeBoard();
    }
    
    /**
     * Checks if a player won the game
     * @param {Player} player Player with the player being checked
     */
    won(player){
        return TIC_TAC_TOE_UTILS.won(player,this._board.getBoard());
    }
}

/**
 * Exports TicTacToeGame module
 */
module.exports=TicTacToeGame;