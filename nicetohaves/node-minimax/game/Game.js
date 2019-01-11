/**
 * Requires Board module
 */
const Board=require('../board/Board');

/**
 * Represents a mark type board game
 */
class Game{
    /**
     * Builds a mark type board game
     * @param {Player} playerX Player with the first player
     * @param {Player} playerY Player with the second player
     */
    constructor(playerX,playerY){
        this._board=new Board();
        this._playerX=playerX;
        this._playerY=playerY;
    }

    /**
     * Let's a player mark a board position
     * @param {Player} player Player with the player playing the game
     * @param {Number} position Number with the position being marked as played
     */
    play(player,position){
        this._grantPlayerIsAtGame(player);
        this._board.mark(player,position);
        this.won(player);
    }

    /**
     * Checks if a player won the game
     * @param {Player} player Player with the player being checked
     */
    won(player){}

    /**
     * Grants that a player is an active player on the current game
     * @param {Player} player Player with the player being checked 
     */
    _grantPlayerIsAtGame(player){
        if(this._playerX!==player&&this._playerY!==player){
            throw `Player $player is not on the current game`;
        }
    }
}

/**
 * Exports Game module
 */
module.exports=Game;