// Auteur: Thomas Lucking
// Creation: 10/03/2025
// Date de Modification: 2/5/2025 
// Description : La déclaration de l'échiquier,les position de les pièces placées à l'intérieur de l'échiquier et du système d'échec et mat.
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Net.NetworkInformation;
using System.Linq;

namespace Chess
{
    // The game state enum to track whether the piece is in check, checkmate, or Stalemate. 
    public enum GameState
    {
        Normal,
        Check,
        CheckMate,
        Stalemate
    }
    public partial class Form1 : Form
    {
        Chessboard Mychessboard = new Chessboard(); // Declaration of the chessboard class.
        List<Chesspieces> pieces = new List<Chesspieces>();
        List<int[]> piecemovementpossibilities = new List<int[]>();

        public System.Windows.Forms.Label[,] labels;

        Chesspieces chesspieceClicked = null;
        // Properties to track what the playerturn is currently and to track the gamestate and If the king can move to resolve the check.
        private string currentPlayerTurn = "White";
        private GameState gamestate = GameState.Normal;
        private bool IsMovingToResolveCheck = false;
       
        private Chesspieces whiteKing;
        private Chesspieces blackKing;


        public Form1()
        {

            // Initialize the 2D array of labels
            InitializeComponent();
            labels = Mychessboard.InitializeChessboard();
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    this.Controls.Add(labels[row, col]);
                    labels[row, col].Click += new EventHandler(ChessCase_Click);
                }
            }

            #region Pawn   
            for (int i = 0; i < 8; i++)
            {
                piecemovementpossibilities.Add(new int[] {0, -2});           
                piecemovementpossibilities.Add(new int[] {0, -1});
                piecemovementpossibilities.Add(new int[] {1, -1});
                piecemovementpossibilities.Add(new int[] {-1, -1});

                Chesspieces pawn = new Chesspieces(Properties.Resources.pawn, 6, i, "white", piecemovementpossibilities, "Pawn");
                pawn.PlacePiece(pawn.PositionX, pawn.PositionY, labels);
                pieces.Add(pawn);

                piecemovementpossibilities.Clear();
            }

            for(int i = 0; i < 8; i++)
            {
                piecemovementpossibilities.Add(new int[] { 0, 2 });
                piecemovementpossibilities.Add(new int[] { 0, 1 });
                piecemovementpossibilities.Add(new int[] { 1, 1 }); 
                piecemovementpossibilities.Add(new int[] { -1, 1 });

                Chesspieces blackpawn = new Chesspieces(Properties.Resources.blackpawn, 1, i, "black", piecemovementpossibilities, "Pawn");
                blackpawn.PlacePiece(blackpawn.PositionX, blackpawn.PositionY, labels);
                pieces.Add(blackpawn);

                piecemovementpossibilities.Clear();
            }
            #endregion
            #region Knight


            // Move 2 squares up, 1 square right
            piecemovementpossibilities.Add(new int[] {1, -2});
            // Move 2 squares up, 1 square left
            piecemovementpossibilities.Add(new int[] { -1, -2 });
            // Move 2 squares down, 1 square right
            piecemovementpossibilities.Add(new int[] { 1, 2 });
            // Move 2 squares down, 1 square left
            piecemovementpossibilities.Add(new int[] { -1, 2 });
            // Move 1 square up, 2 squares right
            piecemovementpossibilities.Add(new int[] { 2, -1});
            // Move 1 square up, 2 squares left
            piecemovementpossibilities.Add(new int[] { -2, -1 });
            // Move 1 square down, 2 squares right
            piecemovementpossibilities.Add(new int[] { 2, 1 });
            // Move 1 square down, 2 squares left
            piecemovementpossibilities.Add(new int[] { -2, 1});

            Chesspieces knight = new Chesspieces(Properties.Resources.knight, 7, 6, "white", piecemovementpossibilities, "Knight"  );
            Chesspieces knight2 = new Chesspieces(Properties.Resources.knight, 7, 1, "white", piecemovementpossibilities, "Knight");
            knight.PlacePiece(knight.PositionX, knight.PositionY, labels);
            knight2.PlacePiece(knight2.PositionX, knight2.PositionY, labels);

            Chesspieces blacknight = new Chesspieces(Properties.Resources.blackknight, 0, 6, "black", piecemovementpossibilities, "Knight");
            Chesspieces blacknight2 = new Chesspieces(Properties.Resources.blackknight, 0, 1, "black", piecemovementpossibilities, "Knight");
            blacknight.PlacePiece(blacknight.PositionX, blacknight.PositionY, labels);
            blacknight2.PlacePiece(blacknight2.PositionX, blacknight2.PositionY, labels);

            piecemovementpossibilities.Clear();

            #endregion
            #region Rock


            for (int i = -7; i < 7; i++)
            {
                if(i != 0)
                {
                    piecemovementpossibilities.Add(new int[] { 0, i });
                }
            }            
            
            for (int i = -7; i < 7; i++)
            {
                if(i != 0)
                {
                    piecemovementpossibilities.Add(new int[] { i, 0 });
                }
            }

            Chesspieces rook = new Chesspieces(Properties.Resources.rock, 7, 7, "white", piecemovementpossibilities, "Rook");
            Chesspieces rook2 = new Chesspieces(Properties.Resources.rock, 7, 0, "white", piecemovementpossibilities, "Rook");
            rook.PlacePiece(rook.PositionX, rook.PositionY, labels);
            rook2.PlacePiece(rook2.PositionX, rook2.PositionY, labels);

            Chesspieces blackrook = new Chesspieces(Properties.Resources.blackrook, 0, 0, "black", piecemovementpossibilities, "Rook");
            Chesspieces blackrook2 = new Chesspieces(Properties.Resources.blackrook, 0, 7, "black", piecemovementpossibilities, "Rook");
            blackrook.PlacePiece(blackrook.PositionX, blackrook.PositionY, labels);
            blackrook2.PlacePiece(blackrook2.PositionX, blackrook2.PositionY, labels);

            piecemovementpossibilities.Clear();

            #endregion
            #region Bishop

            int[] bishopRange = new int[2];

            for (int i = 1; i < 7; i++)
            {
                // ++
                piecemovementpossibilities.Add(new int[] { i, i });

                // +-
                piecemovementpossibilities.Add(new int[] { i, i * -1 });

                // -+
                piecemovementpossibilities.Add(new int[] { i * -1, i });

                // --
                piecemovementpossibilities.Add(new int[] {  i * -1, i * -1 });


            }

            Chesspieces bishop = new Chesspieces(Properties.Resources.bishop,7,5 , "white", piecemovementpossibilities, "Bishop");
            Chesspieces bishop2 = new Chesspieces(Properties.Resources.bishop, 7, 2, "white", piecemovementpossibilities, "Bishop");
            bishop.PlacePiece(bishop.PositionX, bishop.PositionY, labels);
            bishop2.PlacePiece(bishop2.PositionX, bishop2.PositionY, labels);

            Chesspieces blackbishop = new Chesspieces(Properties.Resources.blackbishop, 0, 5, "black", piecemovementpossibilities, "Bishop");
            Chesspieces blackbishop2 = new Chesspieces(Properties.Resources.blackbishop, 0, 2, "black", piecemovementpossibilities, "Bishop");
            blackbishop.PlacePiece(blackbishop.PositionX, blackbishop.PositionY, labels);
            blackbishop2.PlacePiece(blackbishop2.PositionX, blackbishop2.PositionY, labels);

            piecemovementpossibilities.Clear();

            #endregion
            #region queen
            for (int i = 1; i < 7; i++)
            {
                // ++
                piecemovementpossibilities.Add(new int[] { i, i });

                // +-

                piecemovementpossibilities.Add(new int[] { i, i * -1 });

                // -+

                piecemovementpossibilities.Add(new int[] { i * -1, i });
                // --

                piecemovementpossibilities.Add(new int[] { i * -1,  i * -1 });
            }

            for (int i = -7; i < 7; i++)
            {
                if(i != 0)
                {

                    piecemovementpossibilities.Add(new int[] { 0, i });

                    piecemovementpossibilities.Add(new int[] { i, 0 });
                }
            }
            Chesspieces queen = new Chesspieces(Properties.Resources.queen, 7, 3, "white", piecemovementpossibilities, "Queen");
            Chesspieces blackqueen = new Chesspieces(Properties.Resources.blackqueen, 0, 3, "black", piecemovementpossibilities, "Queen");
            queen.PlacePiece(queen.PositionX, queen.PositionY, labels);
            blackqueen.PlacePiece(blackqueen.PositionX, blackqueen.PositionY, labels);


            piecemovementpossibilities.Clear();
            #endregion
            #region king
            int[] kingrange = new int[2];

           
            piecemovementpossibilities.Add(new int[] { 0, 1 });

            piecemovementpossibilities.Add(new int[] { 1, 0 });

            piecemovementpossibilities.Add(new int[] { -1, 0 });
        
            piecemovementpossibilities.Add(new int[] { 0, -1 });

            piecemovementpossibilities.Add(new int[] { 1, 1 });

            piecemovementpossibilities.Add(new int[] { -1, -1 });

            piecemovementpossibilities.Add(new int[] { -1, 1 });

            piecemovementpossibilities.Add(new int[] { 1, -1 });

            Chesspieces blacking = new Chesspieces(Properties.Resources.blackking,0,4 , "black", piecemovementpossibilities, "King");
            Chesspieces king = new Chesspieces(Properties.Resources.king, 7, 4, "white", piecemovementpossibilities, "King");
            blacking.PlacePiece(blacking.PositionX, blacking.PositionY, labels);
            king.PlacePiece(king.PositionX, king.PositionY, labels);


            piecemovementpossibilities.Clear();
            #endregion



            pieces.Add(rook2);
            pieces.Add(knight);
            pieces.Add(knight2);
            pieces.Add(rook);
            pieces.Add(queen);
            pieces.Add(bishop);
            pieces.Add(bishop2);
            pieces.Add(king);
            pieces.Add(blacknight);
            pieces.Add(blacknight2);
            pieces.Add(blackqueen);
            pieces.Add(blacking);
            pieces.Add(blackbishop);
            pieces.Add(blackbishop2);
            pieces.Add(blackrook);
            pieces.Add(blackrook2);
            // Initialize the king refenrence
            InitializeKingReferences();

            // Set the initial turn
            currentPlayerTurn = "white";

            // Set the initial game state
            gamestate = GameState.Normal;

        }
        private void InitializeKingReferences()
        {
            foreach(var piece in pieces)
            {
                if (piece.piecename == "King" && piece.color == "white")
                {
                    whiteKing = piece;
                }
                else if (piece.piecename == "King" && piece.color == "black")
                {
                    blackKing = piece;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void ChessCase_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Label clicked_label = sender as System.Windows.Forms.Label;
            string coordinates = Convert.ToString(clicked_label.Tag);

            // If there's already a piece selected and we click somewhere that's not a valid move
            if (chesspieceClicked != null &&
                !Convert.ToString(clicked_label.Tag).Contains("/Canmove") &&
                !Convert.ToString(clicked_label.Tag).Contains("/Cantake"))
            {
                // Clear all move indicators
                ClearMoveIndicators();
                chesspieceClicked = null;
                return;
            }

            // If we clicked on a piece to select it
            if (chesspieceClicked == null && clicked_label.Image != null && clicked_label.Image != Properties.Resources.dot)
            {
                foreach (var item in pieces)
                {
                    // Extract X and Y positions from the tag
                    string[] tagParts = coordinates.Split('/');
                    if (tagParts.Length > 0)
                    {
                        string[] posParts = tagParts[0].Split('-');
                        if (posParts.Length >= 2)
                        {
                            int clickedX = Convert.ToInt32(posParts[0]);
                            int clickedY = Convert.ToInt32(posParts[1]);

                            // If this is the piece at the clicked position and checks if it's the player's turn
                            if (item.PositionX == clickedX && item.PositionY == clickedY && item.color.ToLower() == currentPlayerTurn.ToLower())
                            {
                                // If in check, only allow moves that resolve the check
                                if (gamestate == GameState.Check && item.color.ToLower() == currentPlayerTurn.ToLower())
                                {
                                    IsMovingToResolveCheck = true;
                                    item.GetMovePossibilities(labels);
                                    FilterMovesLeadingToCheck(item);

                                    // Check if there are any valid moves after filtering
                                    bool hasValidMoves = false;
                                    for (int y = 0; y < 8; y++)
                                    {
                                        for (int x = 0; x < 8; x++)
                                        {
                                            if (labels[y, x].Tag != null &&
                                               (labels[y, x].Tag.ToString().Contains("/Canmove") ||
                                                labels[y, x].Tag.ToString().Contains("/Cantake")))
                                            {
                                                hasValidMoves = true;
                                                break;
                                            }
                                        }
                                        if (hasValidMoves) break;
                                    }

                                    if (hasValidMoves)
                                    {
                                        chesspieceClicked = item;
                                    }
                                    else
                                    {
                                        // No valid moves for this piece, clear indicators
                                        ClearMoveIndicators();
                                    }
                                }
                                else
                                {
                                    // Normal move selection
                                    item.GetMovePossibilities(labels);
                                    FilterMovesLeadingToCheck(item);
                                    chesspieceClicked = item;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            // If we clicked on a valid move or capture
            else if (chesspieceClicked != null &&
                    (Convert.ToString(clicked_label.Tag).Contains("/Canmove") ||
                     Convert.ToString(clicked_label.Tag).Contains("/Cantake")))
            {
                // Extract X and Y positions from the tag
                string[] tagParts = coordinates.Split('/');
                if (tagParts.Length > 0)
                {
                    string[] posParts = tagParts[0].Split('-');
                    if (posParts.Length >= 2)
                    {
                        int clickedX = Convert.ToInt32(posParts[0]);
                        int clickedY = Convert.ToInt32(posParts[1]);

                        // If this is a capture, remove the captured piece from the pieces list
                        if (Convert.ToString(clicked_label.Tag).Contains("/Cantake"))
                        {
                            // Find and remove the captured piece from the pieces list
                            Chesspieces capturedPiece = null;
                            foreach (var piece in pieces)
                            {
                                if (piece.PositionX == clickedX && piece.PositionY == clickedY)
                                {
                                    capturedPiece = piece;
                                    break;
                                }
                            }

                            if (capturedPiece != null)
                            {
                                pieces.Remove(capturedPiece);
                            }
                        }

                        // Move the piece to the new position
                        chesspieceClicked.MovePiece(Convert.ToString(clickedX), Convert.ToString(clickedY), labels);

                        // Reset move resolution flag
                        IsMovingToResolveCheck = false;

                        // Switch turns
                        currentPlayerTurn = (currentPlayerTurn.ToLower() == "white") ? "black" : "white";

                        // Check for check/checkmate in the new position
                        CheckForCheckAndCheckmate();

                        // Clear selection
                        chesspieceClicked = null;
                    }
                }
            }
        }

        private void FilterMovesLeadingToCheck(Chesspieces piece)
        {
            List<Tuple<int, int>> invalidMoves = new List<Tuple<int, int>>();

            // iterate through all the squares with the "/CanMove" or "Cantake" tags
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (labels[y, x].Tag != null &&
                       (labels[y, x].Tag.ToString().Contains("/Canmove") ||
                        labels[y, x].Tag.ToString().Contains("/Cantake")))
                    {
                        // Save the original position.
                        int originalX = piece.PositionX;
                        int originalY = piece.PositionY;

                        // Save captured piece if any
                        Chesspieces capturedPiece = null;
                        foreach (var p in pieces)
                        {
                            if (p.PositionX == x && p.PositionY == y)
                            {
                                capturedPiece = p;
                                break;
                            }
                        }

                        // Temporarily remove the captured piece if any
                        if (capturedPiece != null)
                        {
                            pieces.Remove(capturedPiece);
                        }

                        // Temporarily make the move
                        piece.PositionX = x;
                        piece.PositionY = y;

                        // Check if this move would put our king in check.
                        bool wouldBeInCheck = IsKingInCheck(piece.color);

                        // if it would put us in check, add to invalid moves
                        if (wouldBeInCheck)
                        {
                            invalidMoves.Add(new Tuple<int, int>(x, y));
                        }

                        // Restore the original position
                        piece.PositionX = originalX;
                        piece.PositionY = originalY;

                        // Restore the captured piece if any
                        if (capturedPiece != null)
                        {
                            pieces.Add(capturedPiece);
                        }
                    }
                }
            }

            // Remove the invalid moves
            foreach (var invalidMove in invalidMoves)
            {
                int x = invalidMove.Item1;
                int y = invalidMove.Item2;

                if (labels[y, x].Tag != null)
                {
                    string tagStr = labels[y, x].Tag.ToString();

                    if (tagStr.Contains("/Canmove"))
                    {
                        labels[y, x].Image = null;
                        // Keep only the position part of the tag
                        labels[y, x].Tag = tagStr.Split('/')[0];
                    }
                    else if (tagStr.Contains("/Cantake"))
                    {
                        // Keep the piece image but remove the Cantake tag
                        string[] parts = tagStr.Split('/');
                        if (parts.Length >= 3)
                        {
                            // Keep position and piece info
                            labels[y, x].Tag = parts[0] + "/" + parts[2];
                        }
                        else if (parts.Length > 1)
                        {
                            labels[y, x].Tag = parts[0];
                        }
                    }
                }
            }
        }
        private bool IsKingInCheck(string kingColor)
        {
            // Find the king's position
            Chesspieces king = (kingColor == "white") ? whiteKing : blackKing;

            if (king == null)
            {
                InitializeKingReferences(); // Make sure kings are referenced
                king = (kingColor == "white") ? whiteKing : blackKing;

                if (king == null)
                {
                    MessageBox.Show("Error: King reference is null!");
                    return false;
                }
            }

            int kingX = king.PositionX;
            int kingY = king.PositionY;

            // Check if an opponent piece can attack the king's position
            string opponentColor = (kingColor == "white") ? "black" : "white";

            foreach (var piece in pieces)
            {
                if (piece.color == opponentColor)
                {
                    // Clear any existing move indicators
                    ClearMoveIndicators();

                    // Get the movement possibilities of the opponent piece
                    piece.GetMovePossibilities(labels);

                    // Check if the king's position is marked as capturable
                    if (labels[kingY, kingX].Tag != null &&
                        labels[kingY, kingX].Tag.ToString().Contains("/Cantake"))
                    {
                        ClearMoveIndicators();
                        return true;
                    }
                }
            }

            // Clear any move indicators that were added
            ClearMoveIndicators();
            return false;
        }



        private void CheckForCheckAndCheckmate()
        {
            // Determine which king t check (the one whose turn it is now)
            string kingColorToCheck = currentPlayerTurn;

            //Check if the king is in check.
            bool isInCheck = IsKingInCheck(kingColorToCheck);

            if (isInCheck)
            {
                // Check the Game state
                gamestate = GameState.Check;

                // Check if it's checkmate by seeing if any move can get out of the check
                if (IsCheckmate(kingColorToCheck))
                {
                    gamestate = GameState.CheckMate;
                    MessageBox.Show($"Checkmate! {(kingColorToCheck == "white" ? "Black": "White")} wins!");
                    // To do Restart the form when checkmate
                }
                else
                {
                    MessageBox.Show($"{char.ToUpper(kingColorToCheck[0]) + kingColorToCheck.Substring(1)} is in check!");
                }
            }
            else
            {
                gamestate = GameState.Normal;
            }

        }
        private bool IsCheckmate(string kingColor)
        {
            // Try all possible moves for all pieces of the given color
            foreach (var piece in pieces.Where(p => p.color == kingColor))
            {
                // Clear any previous move indicators
                ClearMoveIndicators();

                // Get all possible moves for this piece
                piece.GetMovePossibilities(labels);

                // Filter moves that would still leave the king in check
                FilterMovesLeadingToCheck(piece);

                // Check if there are any valid moves left 
                bool hasValidMove = false;
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (labels[y, x].Tag != null &&
                           (labels[y, x].Tag.ToString().Contains("/Canmove") ||
                            labels[y, x].Tag.ToString().Contains("/Cantake")))
                        {
                            // There's at least one valid move, so it's not checkmate
                            ClearMoveIndicators();
                            return false;
                        }
                    }
                }

                // Clean up 
                ClearMoveIndicators();
            }

            // If we've checked all pieces and found no valid moves, it's checkmate
            return true;
        }



        // Helper method to clear all move indicators
        private void ClearMoveIndicators()
        {
            foreach (var item in labels)
            {
                if (item.Tag != null)
                {
                    string tagStr = Convert.ToString(item.Tag);

                    // Handle Canmove tags
                    if (tagStr.Contains("/Canmove"))
                    {
                        item.Image = null;
                        string[] tag = tagStr.Split('/');
                        if (tag.Length > 0)
                        {
                            item.Tag = tag[0]; // Remove all tags after the first "/"
                        }
                    }

                    // Handle Cantake tags
                    else if (tagStr.Contains("/Cantake"))
                    {
                        // Parse the tag to keep the piece information
                        string[] tag = tagStr.Split('/');

                        // Check if there's any piece information (color/type) to preserve
                        if (tag.Length >= 3)
                        {
                            // Keep position and the piece info (color/type)
                            item.Tag = tag[0] + "/" + tag[2];
                        }
                        else if (tag.Length > 0)
                        {
                            // If there's no piece info, just keep the position
                            item.Tag = tag[0];
                        }
                    }
                }
            }
        }

    }
}
