/**
 * Represents all possible combinations that can lead to a win on a Tic-Tac-Toe game
 * Winning possibilities are represented by the board indexes
 */
const WINNING_COMBINATIONS=[
    [0,1,2],
    [3,4,5],
    [6,7,8],
    [0,3,6],
    [1,4,7],
    [2,5,8],
    [0,4,8],
    [2,4,6]
];

/**
 * Checks if a player won the game
 * @param {Player} player Player with the player being checked
 * @param {Array} board Array with the Tic-Tac-Toe board
 */
function won(player,board){
    let currentBoardIndex=0;
    //Rows
    let boardRows=Math.sqrt(board.length);
    for(let i=0;i<boardRows;i++){
        let playedPositions=[];
        for(let j=0;j<boardRows;j++){
            if(board[currentBoardIndex]===player){
                playedPositions.push(currentBoardIndex);
            }
            currentBoardIndex++;
        }
        console.log("->>>>>>>>>>>>> "+playedPositions);
        if(isWinningPlay(playedPositions)){
            return true;
        }
    }
    currentBoardIndex=0;

    //Columns
    for(let i=0;i<boardRows;i++){
        let playedPositions=[];
        for(let j=0;j<boardRows;j++){
            if(board[i+j+boardRows]===player){
                playedPositions.push(i+j+boardRows);
            }
            currentBoardIndex++;
        }
        if(isWinningPlay(playedPositions)){
            console.log("????????");
            return true;
        }
    }

    if(board[0]===player && board[4]===player && board[8]===player)
        return true;

    if(board[2]===player && board[4]===player && board[6]===player)
        return true;

    return false;
}

/**
 * Checks if a certain play is a winning play
 * @param {Array} play Array with the play being checked
 */
function isWinningPlay(play){
    if(play.length!=3)return false;
    for(let i=0;i<WINNING_COMBINATIONS.length;i++){
        let winningPlay=true;
        for(let j=0;j<play.length;j++){
            winningPlay&=(WINNING_COMBINATIONS[i][j]===play[j]);
        }
        if(winningPlay)return true;
    }
    return false;
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
 * Exports TicTacToeUtils module functions
 */
module.exports={WINNING_COMBINATIONS,won,isWinningPlay,getAvailablePositions};