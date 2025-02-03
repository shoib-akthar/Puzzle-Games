using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe_GameManager : MonoBehaviour
{
    public Button[] gridButtons;          // Array of grid buttons
    public TextMeshProUGUI endGameText;   // Text to display end game message
    public Button restartButton;          // Restart button

    private string playerSide;            // Player side ("X" is player, "O" is AI)

    public Sprite imageActive;
    public Sprite imageDisabled;
    public Sprite imageplayerX;
    public Sprite imageplayerO;

    private string[] board;               // Board state
    int[] winningCombination = new int[]
    {
        0, 1, 2,  3, 4, 5,  6, 7, 8,  // Rows
        0, 3, 6,  1, 4, 7,  2, 5, 8,  // Columns
        0, 4, 8,  2, 4, 6              // Diagonals
    };

    private void Start()
    {
        InitializeGame();
        restartButton.onClick.AddListener(RestartGame);
    }

    private void InitializeGame()
    {
        playerSide = "X";
        endGameText.text = playerSide + " Turn";
        restartButton.gameObject.SetActive(false);
        board = new string[9]; // Initialize empty board
        SetGridButtonsInteractable(true);
    }

    private void SetGridButtonsInteractable(bool isInteractable)
    {
        for (int i = 0; i < gridButtons.Length; i++)
        {
            int index = i; // Closure-safe variable
            gridButtons[i].onClick.RemoveAllListeners();
            gridButtons[i].onClick.AddListener(() => OnGridButtonClicked(index));
            UpdateUI(gridButtons[i], "Reset");
            board[i] = ""; // Reset the board
        }
    }

    private void OnGridButtonClicked(int index)
    {
        if (!string.IsNullOrEmpty(board[index])) return;

        // Player's move
        board[index] = playerSide;
        UpdateUI(gridButtons[index], playerSide);

        if (CheckWinner(playerSide))
        {
            endGameText.text = "Player X Wins!";
            restartButton.gameObject.SetActive(true);
            return;
        }

        if (IsBoardFull())
        {
            endGameText.text = "It's a Draw!";
            restartButton.gameObject.SetActive(true);
            return;
        }

        // AI's turn
        SwitchPlayerSide();
        AI_Move();

        if (CheckWinner(playerSide))
        {
            endGameText.text = "Player O Wins!";
            restartButton.gameObject.SetActive(true);
            return;
        }

        if (IsBoardFull())
        {
            endGameText.text = "It's a Draw!";
            restartButton.gameObject.SetActive(true);
            return;
        }

        // Switch back to player's turn
        SwitchPlayerSide();
    }

    private void AI_Move()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        // Evaluate all possible moves
        for (int i = 0; i < board.Length; i++)
        {
            if (string.IsNullOrEmpty(board[i]))
            {
                board[i] = playerSide; // Simulate AI move
                int score = Minimax(board, 0, false);
                board[i] = ""; // Undo move

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        // Make the best move
        if (bestMove != -1)
        {
            board[bestMove] = playerSide;
            UpdateUI(gridButtons[bestMove], playerSide);
        }
    }

    private int Minimax(string[] boardState, int depth, bool isMaximizing)
    {
        if (CheckWinner("O")) return 10 - depth; // AI win
        if (CheckWinner("X")) return depth - 10; // Player win
        if (IsBoardFull()) return 0; // Draw

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < boardState.Length; i++)
            {
                if (string.IsNullOrEmpty(boardState[i]))
                {
                    boardState[i] = "O"; // AI's move
                    int score = Minimax(boardState, depth + 1, false);
                    boardState[i] = ""; // Undo move
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < boardState.Length; i++)
            {
                if (string.IsNullOrEmpty(boardState[i]))
                {
                    boardState[i] = "X"; // Player's move
                    int score = Minimax(boardState, depth + 1, true);
                    boardState[i] = ""; // Undo move
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    private void UpdateUI(Button button, string playerSide)
    {
        if (playerSide == "X")
        {
            button.GetComponent<Image>().sprite = imageplayerX;
            button.interactable = false;
        }
        else if (playerSide == "O")
        {
            button.GetComponent<Image>().sprite = imageplayerO;
            button.interactable = false;
        }
        else if (playerSide == "Reset")
        {
            button.GetComponent<Image>().sprite = imageActive;
            button.interactable = true;
        }
    }

    private bool CheckWinner(string side)
    {
        for (int i = 0; i < winningCombination.Length; i += 3)
        {
            if (board[winningCombination[i]] == side &&
                board[winningCombination[i + 1]] == side &&
                board[winningCombination[i + 2]] == side)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBoardFull()
    {
        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell)) return false;
        }
        return true;
    }

    public void RestartGame()
    {
        InitializeGame();
    }

    private void SwitchPlayerSide()
    {
        playerSide = playerSide == "X" ? "O" : "X";
        endGameText.text = playerSide + " Turn";
    }
}
