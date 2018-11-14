/**
 * Represents a mark type board
 */
class Board{
    /**
     * Builds a new board
     */
    constructor(){
        this._board=[];
    }

    /**
     * Marks a certain position with a mark
     * @param {Mark} mark Mark with the mark being marked on the board
     * @param {Number} position Number with the board position index
     */
    mark(mark,position){
        if(!this._board[position])
            this._board[position]=mark;
    }

    /**
     * Checks if the board has a mark on certain position
     * @param {Mark} mark Mark with the mark being checked on a certain position
     * @param {Number} positon Number with the board position
     */
    hasMark(mark,positon){
        return this._board[positon]==(mark);
    }

    /**
     * Returns the current board
     */
    getBoard(){return this._board.slice();}

}

/**
 * Exports Board module
 */
module.exports=Board;