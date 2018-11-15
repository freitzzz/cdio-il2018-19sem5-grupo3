//Solution based on @diegocasmo Tic-Tac-Toe minimax implementation https://github.com/diegocasmo/tic-tac-toe-minimax

const TIC_TAC_TOE_UTILS=require('./TicTacToeUtils');

function evaluate(board,depth,humanPlayer,computerPlayer){
    let score=checkWinner(board,humanPlayer,computerPlayer);
    switch(score){
        case 1:
            return 0;
        case 2:
            return depth-10;
        case 3:
            return 10-depth;
    }
}

function checkWinner(board,humanPlayer,computerPlayer){
    /* if(TIC_TAC_TOE_UTILS.won(humanPlayer,board)){
        console.log("Human");
        return 2;
    }else if(TIC_TAC_TOE_UTILS.won(computerPlayer,board)){
        console.log("Computer");
        return 3;
    }else{
        for(let i=0;i<9;i++){
            if(board[i]!==humanPlayer&&board[i]!==computerPlayer){
                return 0;
            }
        }
    }
    return 1; */
    // Check for horizontal wins
    for (i = 0; i <= 6; i += 3)
    {
        if (board[i] === humanPlayer && board[i + 1] === humanPlayer && board[i + 2] === humanPlayer)
            return 2;
        if (board[i] === computerPlayer && board[i + 1] === computerPlayer && board[i + 2] === computerPlayer)
            return 3;
    }

    // Check for vertical wins
    for (i = 0; i <= 2; i++)
    {
        if (board[i] === humanPlayer && board[i + 3] === humanPlayer && board[i + 6] === humanPlayer)
            return 2;
        if (board[i] === computerPlayer && board[i + 3] === computerPlayer && board[i + 6] === computerPlayer)
            return 3;
    }

    // Check for diagonal wins
    if ((board[0] === humanPlayer && board[4] === humanPlayer && board[8] === humanPlayer) ||
            (board[2] === humanPlayer && board[4] === humanPlayer && board[6] === humanPlayer))
        return 2;

    if ((board[0] === computerPlayer && board[4] === computerPlayer && board[8] === computerPlayer) ||
            (board[2] === computerPlayer && board[4] === computerPlayer && board[6] === computerPlayer))
        return 3;

    // Check for tie
    for (i = 0; i < board.length; i++)
    {
        if (board[i] !== humanPlayer && board[i] !== computerPlayer)
            return 0;
    }
    return 1;
}

var choice=2;

function minimax(board,depth,computerTurn,humanPlayer,computerPlayer){
    minimax2(board,depth,computerTurn,humanPlayer,computerPlayer);
    return choice;
}

/**
 * Applies the minimax algorithm to choose the next position to play
 * @param {Array} board Array with the game board
 * @param {Number} depth Number with the minimax depth
 * @param {Boolean} computerTurn Boolean verifying if the current turn is for the computer
 * @param {Player} humanPlayer Player with the human player
 * @param {Player} computerPlayer Player with the computer player
 */
function minimax2(board,depth,computerTurn,humanPlayer,computerPlayer){
    if(checkWinner(board,humanPlayer,computerPlayer)!==0){
        return evaluate(board,depth,humanPlayer,computerPlayer);
    }
    depth+=1;
    let scores=[];
    let moves=[];
    let availableMoves=getAvailablePositions(board);
    let move,possibleGame;
    for(let i=0;i<availableMoves.length;i++){
        move=availableMoves[i];
        board=mark(board,move,computerTurn,humanPlayer,computerPlayer);
        computerTurn=!computerTurn;
        scores.push(minimax2(board,depth,computerTurn,humanPlayer,computerPlayer));
        moves.push(move);
        board=undoMove(board,move);
        computerTurn=!computerTurn;
    }
    let min_score,max_score,min_score_index,max_score_index;
    if(computerTurn){
        max_score=Math.max.apply(Math,scores);
        max_score_index=scores.indexOf(max_score);
        choice=moves[max_score_index];
        return scores[max_score_index];
    }else{
        min_score=Math.min.apply(Math,scores);
        min_score_index=scores.indexOf(min_score);
        choice=moves[min_score_index];
        return scores[min_score_index];
    }
}

/**
 * Undo's a move
 * @param {Array} board Array with the Tic-Tac-Toe game board
 * @param {Number} position Number with the position where the move will be undo'ed
 */
function undoMove(board,position){
    board[position]=null;
    return board;
}

/**
 * Marks a position on the board
 * @param {Array} board Array with the game board
 * @param {Number} position Number with the position being marked
 * @param {Boolean} computerTurn Boolean verifying if the current turn is for the computer
 * @param {Player} humanPlayer Player with the human player
 * @param {Player} computerPlayer Player with the computer player
 */
function mark(board,position,computerTurn,humanPlayer,computerPlayer){
    let playerToMark=computerTurn ? computerPlayer :humanPlayer;
    board[position]=playerToMark;
    return board;
}

/**
 * Returns all available positions that can be played on the Tic-Tac-Toe board
 * @param {Array} board Array with the current Tic-Tac-Toe board
 */
function getAvailablePositions(board){
    let availablePositions=[];
    for(let i=0;i<9;i++)
        if(!board[i])
            availablePositions.push(i);
    return availablePositions;
}

/**
 * Exports Minimax utility functions
 */
module.exports={evaluate,checkWinner,minimax,mark,getAvailablePositions};