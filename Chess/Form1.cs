﻿// Auteur: Thomas Lucking
// Creation: 10/03/2025
// Date de Modification: 8/5/2025 
// Description: Fixed chess game with proper board rotation functionality

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Net.NetworkInformation;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Chess
{
    // The game state enum to track whether the piece is in check, checkmate.
    public enum GameState
    {
        Normal,
        Check,
        CheckMate,
    }
    public partial class Form1 : Form
    {
        Chessboard Mychessboard = new Chessboard(); // Declaration of the chessboard class.
        List<Chesspieces> pieces = new List<Chesspieces>();
        List<int[]> piecemovementpossibilities = new List<int[]>();

        public System.Windows.Forms.Label[,] labels;

        Chesspieces chesspieceClicked = null;
        // Properties to track what the playerturn is currently and to track the gamestate and If the king can move to resolve the check.
        private string currentPlayerTurn = "white";
        private GameState gamestate = GameState.Normal;

        private Chesspieces whiteKing;
        private Chesspieces blackKing;

        private bool isBoardRotated = false;

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
                piecemovementpossibilities.Add(new int[] { 0, -2 });
                piecemovementpossibilities.Add(new int[] { 0, -1 });
                piecemovementpossibilities.Add(new int[] { 1, -1 });
                piecemovementpossibilities.Add(new int[] { -1, -1 });

                Chesspieces pawn = new Chesspieces(Properties.Resources.pawn, 6, i, "white", piecemovementpossibilities, "Pawn");
                pawn.PlacePiece(pawn.PositionX, pawn.PositionY, labels);
                pieces.Add(pawn);

                piecemovementpossibilities.Clear();
            }

            for (int i = 0; i < 8; i++)
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

            // ... (Keep all other piece initialization code exactly the same)
            #region Knight
            // Move 2 squares up, 1 square right
            piecemovementpossibilities.Add(new int[] { 1, -2 });
            // Move 2 squares up, 1 square left
            piecemovementpossibilities.Add(new int[] { -1, -2 });
            // Move 2 squares down, 1 square right
            piecemovementpossibilities.Add(new int[] { 1, 2 });
            // Move 2 squares down, 1 square left
            piecemovementpossibilities.Add(new int[] { -1, 2 });
            // Move 1 square up, 2 squares right
            piecemovementpossibilities.Add(new int[] { 2, -1 });
            // Move 1 square up, 2 squares left
            piecemovementpossibilities.Add(new int[] { -2, -1 });
            // Move 1 square down, 2 squares right
            piecemovementpossibilities.Add(new int[] { 2, 1 });
            // Move 1 square down, 2 squares left
            piecemovementpossibilities.Add(new int[] { -2, 1 });

            Chesspieces knight = new Chesspieces(Properties.Resources.knight, 7, 6, "white", piecemovementpossibilities, "Knight");
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
                if (i != 0)
                {
                    piecemovementpossibilities.Add(new int[] { 0, i });
                }
            }

            for (int i = -7; i < 7; i++)
            {
                if (i != 0)
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
                piecemovementpossibilities.Add(new int[] { i * -1, i * -1 });
            }

            Chesspieces bishop = new Chesspieces(Properties.Resources.bishop, 7, 5, "white", piecemovementpossibilities, "Bishop");
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
                piecemovementpossibilities.Add(new int[] { i * -1, i * -1 });
            }

            for (int i = -7; i < 7; i++)
            {
                if (i != 0)
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

            Chesspieces blacking = new Chesspieces(Properties.Resources.blackking, 0, 4, "black", piecemovementpossibilities, "King");
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

            // Initialize the king reference
            InitializeKingReferences();

            // Set the initial turn
            currentPlayerTurn = "white";

            // Set the initial game state
            gamestate = GameState.Normal;
        }

        // NEW: Helper methods for coordinate conversion
        private (int visualX, int visualY) LogicalToVisual(int logicalX, int logicalY)
        {
            if (isBoardRotated)
            {
                return (7 - logicalX, 7 - logicalY);
            }
            return (logicalX, logicalY);
        }

        private (int logicalX, int logicalY) VisualToLogical(int visualX, int visualY)
        {
            if (isBoardRotated)
            {
                return (7 - visualX, 7 - visualY);
            }
            return (visualX, visualY);
        }

        // NEW: Method to properly rotate the board display
        private void RotateBoard()
        {
            isBoardRotated = !isBoardRotated;

            // Clear the visual board
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    labels[row, col].Image = null;
                    labels[row, col].Tag = col + "-" + row; // Reset to basic position tag
                }
            }

            // Re-place all pieces at their new visual positions
            foreach (var piece in pieces)
            {
                var (visualX, visualY) = LogicalToVisual(piece.PositionX, piece.PositionY);
                labels[visualY, visualX].Image = piece.image;
                labels[visualY, visualX].Tag = visualX + "-" + visualY + "/" + piece.color + "-" + piece.piecename;
            }
        }

        private void InitializeKingReferences()
        {
            foreach (var piece in pieces)
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

            // For debugging
            MessageBox.Show(Convert.ToString(clicked_label.Tag));

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
                // Extract X and Y positions from the tag
                string[] tagParts = coordinates.Split('/');
                if (tagParts.Length > 0)
                {
                    string[] posParts = tagParts[0].Split('-');
                    if (posParts.Length >= 2)
                    {
                        int clickedVisualX = Convert.ToInt32(posParts[0]);
                        int clickedVisualY = Convert.ToInt32(posParts[1]);

                        // Convert visual coordinates to logical coordinates
                        var (logicalX, logicalY) = VisualToLogical(clickedVisualX, clickedVisualY);

                        // Find the piece at the logical position
                        foreach (var item in pieces)
                        {
                            if (item.PositionX == logicalX && item.PositionY == logicalY && item.color == currentPlayerTurn)
                            {
                                if (!CanPieceMoveInCheck(item))
                                {
                                    MessageBox.Show($"Your king is in check! Only your king can move.");
                                    return;
                                }
                                // Use the rotation-aware method
                                item.GetMovePossibilitiesWithRotation(labels, isBoardRotated);
                                // FilterMovesLeadingToCheck(item);
                                chesspieceClicked = item;
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
                        int clickedVisualX = Convert.ToInt32(posParts[0]);
                        int clickedVisualY = Convert.ToInt32(posParts[1]);

                        // Convert visual coordinates to logical coordinates
                        var (logicalX, logicalY) = VisualToLogical(clickedVisualX, clickedVisualY);

                        // If this is a capture, remove the captured piece from the pieces list
                        if (Convert.ToString(clicked_label.Tag).Contains("/Cantake"))
                        {
                            // Find and remove the captured piece from the pieces list
                            Chesspieces capturedPiece = null;
                            foreach (var piece in pieces)
                            {
                                if (piece.PositionX == logicalX && piece.PositionY == logicalY)
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

                        // Move the piece to the new logical position
                        chesspieceClicked.MovePieceWithRotation(logicalX, logicalY, labels, isBoardRotated);
                        currentPlayerTurn = (currentPlayerTurn == "white") ? "black" : "white";

                        // Rotate the board after the turn changes
                        RotateBoard();

                        MessageBox.Show("it's " + currentPlayerTurn + 's' + " turn");
                        CheckForCheckAndCheckmate();
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
                        // Convert visual coordinates to logical coordinates
                        var (logicalX, logicalY) = VisualToLogical(x, y);

                        // Save the original position.
                        int originalX = piece.PositionX;
                        int originalY = piece.PositionY;

                        // Save captured piece if any
                        Chesspieces capturedPiece = null;
                        foreach (var p in pieces.ToList()) // Use ToList() to avoid modification during iteration
                        {
                            if (p.PositionX == logicalX && p.PositionY == logicalY && p != piece)
                            {
                                capturedPiece = p;
                                pieces.Remove(capturedPiece);
                                break;
                            }
                        }

                        // Temporarily make the move
                        piece.PositionX = logicalX;
                        piece.PositionY = logicalY;

                        // Check if this move would put our king in check.
                        bool wouldbeInCheck = IsKingInCheck(piece.color);

                        // Restore the original position BEFORE checking other moves
                        piece.PositionX = originalX;
                        piece.PositionY = originalY;

                        // Restore captured piece if any
                        if (capturedPiece != null)
                        {
                            pieces.Add(capturedPiece);
                        }

                        // if it would put us in check, add to invalid moves (use visual coordinates)
                        if (wouldbeInCheck)
                        {
                            invalidMoves.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
            }

            // Now remove the invalid moves
            foreach (var invalidMove in invalidMoves)
            {
                int x = invalidMove.Item1;
                int y = invalidMove.Item2;

                if (labels[y, x].Tag != null)
                {
                    string tagstr = labels[y, x].Tag.ToString();

                    if (tagstr.Contains("/Canmove"))
                    {
                        labels[y, x].Image = null;
                        // Keep only the position part of the tag
                        string[] parts = tagstr.Split('/');
                        labels[y, x].Tag = parts[0]; // Keep position, remove /Canmove
                    }
                    else if (tagstr.Contains("/Cantake"))
                    {
                        // keep the piece image but remove the /Cantake tag
                        string[] parts = tagstr.Split('/');
                        labels[y, x].Tag = parts[0]; // Keep position, remove /Cantake
                    }
                }
            }
        }

        private bool IsKingInCheck(string kingColor)
        {
            // Find the king's position
            Chesspieces king = (kingColor == "white") ? whiteKing : blackKing;
            int kingX = king.PositionX;
            int kingY = king.PositionY;

            // Check if an opponent piece can attack the king's position
            string opponentColor = (kingColor == "white") ? "black" : "white";

            // Clear any existing move indicators before starting
            ClearMoveIndicators();

            foreach (var piece in pieces)
            {
                if (piece.color == opponentColor)
                {
                    // Get the movement possibilities of the opponent piece
                    piece.GetMovePossibilitiesWithRotation(labels, isBoardRotated);

                    // Convert king's logical position to visual position for checking
                    var (kingVisualX, kingVisualY) = LogicalToVisual(kingX, kingY);

                    if (labels[kingVisualY, kingVisualX].Tag != null)
                    {
                        string kingPositionTag = labels[kingVisualY, kingVisualX].Tag.ToString();

                        if (kingPositionTag.Contains("/Canmove") || kingPositionTag.Contains("/Cantake"))
                        {
                            // Clean up before returning
                            ClearMoveIndicators();
                            return true;
                        }
                    }

                    // Clear move indicators after checking each piece
                    ClearMoveIndicators();
                }
            }
            return false;
        }

        private void CheckForCheckAndCheckmate()
        {
            // Determine which king to check (the one whose turn it is now)
            string kingColorToCheck = currentPlayerTurn;

            Debug.WriteLine($"Checking if {kingColorToCheck} king is in check");

            bool isInCheck = IsKingInCheck(kingColorToCheck);
            bool isCheckmate = false;

            if (isInCheck)
            {
                Debug.WriteLine($"{kingColorToCheck} king is in check!");
                // Now that we know it's check, THEN check for checkmate
                isCheckmate = IsCheckmate(kingColorToCheck);
                Debug.WriteLine($"Is it checkmate? {isCheckmate}");

                if (isCheckmate)
                {
                    gamestate = GameState.CheckMate;
                    MessageBox.Show($"Checkmate! {(kingColorToCheck == "white" ? "Black" : "White")} wins!");
                    ResetGame();
                    return;
                }
                else
                {
                    gamestate = GameState.Check;
                    Debug.WriteLine("Setting game state to Check");

                    string kingColorInCheck = currentPlayerTurn;
                    Debug.WriteLine($"{kingColorInCheck} king is in check - only king can move until check is resolved");

                    MessageBox.Show($"Check! {char.ToUpper(kingColorInCheck[0]) + kingColorInCheck.Substring(1)} king is in check. Only the king can move.");
                }
            }
            else
            {
                gamestate = GameState.Normal;
                Debug.WriteLine("Setting game state to Normal");
            }
        }

        private bool IsCheckmate(string kingColor)
        {
            // First check if the king is even in check
            if (!IsKingInCheck(kingColor))
            {
                return false; // Not in check, so definitely not checkmate
            }

            // Get all pieces of the given color
            var piecesToCheck = pieces.Where(p => p.color == kingColor).ToList();

            // Try all possible moves for all pieces of the given color
            foreach (var piece in piecesToCheck)
            {
                // Save the original position
                int originalX = piece.PositionX;
                int originalY = piece.PositionY;

                // Clear any previous move indicators
                ClearMoveIndicators();

                // Get all possible moves for this piece
                piece.GetMovePossibilitiesWithRotation(labels, isBoardRotated);

                // Check each possible move
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        if (labels[y, x].Tag != null &&
                           (labels[y, x].Tag.ToString().Contains("/Canmove") ||
                            labels[y, x].Tag.ToString().Contains("/Cantake")))
                        {
                            // Found a potential move - convert to logical coordinates
                            var (logicalX, logicalY) = VisualToLogical(x, y);

                            // Keep track of all the pieces on the board before the move
                            var removedPiece = pieces.FirstOrDefault(p => p.PositionX == logicalX && p.PositionY == logicalY && p != piece);
                            bool hadRemovedPiece = removedPiece != null;

                            // Temporarily remove any piece at the destination square
                            if (hadRemovedPiece)
                            {
                                pieces.Remove(removedPiece);
                            }

                            // Temporarily move the piece
                            piece.PositionX = logicalX;
                            piece.PositionY = logicalY;

                            // Check if this move resolves the check
                            bool stillInCheck = IsKingInCheck(kingColor);

                            // Restore the original position
                            piece.PositionX = originalX;
                            piece.PositionY = originalY;

                            // Restore any removed piece
                            if (hadRemovedPiece && removedPiece != null)
                            {
                                pieces.Add(removedPiece);
                            }

                            // If this move resolves the check, it's not checkmate
                            if (!stillInCheck)
                            {
                                ClearMoveIndicators();
                                return false;
                            }
                        }
                    }
                }

                // Clean up 
                ClearMoveIndicators();
            }

            // If we've checked all pieces and found no moves that resolve the check, it's checkmate
            return true;
        }

        private bool CanPieceMoveInCheck(Chesspieces piece)
        {
            // If we're not in check, any piece can move
            if (gamestate != GameState.Check)
            {
                return true;
            }

            // If we're in check, only the current player's king can move
            if (piece.color == currentPlayerTurn && piece.piecename == "King")
            {
                Debug.WriteLine($"Allowing {currentPlayerTurn} king to move while in check");
                return true;
            }

            // If it's a piece of the player who is NOT in check, they can move freely
            if (piece.color != currentPlayerTurn)
            {
                Debug.WriteLine($"Allowing {piece.color} piece to move as their king is not in check");
                return true;
            }

            // All other pieces belonging to the player in check cannot move
            Debug.WriteLine($"Blocking {piece.color} {piece.piecename} from moving while king is in check");
            return false;
        }

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
                        // Preserve piece information if it exists
                        string[] parts = tagStr.Split('/');
                        string pieceInfo = "";

                        // Look for color-piecename format
                        for (int i = 1; i < parts.Length; i++)
                        {
                            if (parts[i].Contains("-") &&
                                (parts[i].StartsWith("white") || parts[i].StartsWith("black")))
                            {
                                pieceInfo = parts[i];
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(pieceInfo))
                            item.Tag = $"{parts[0]}/{pieceInfo}";
                        else
                            item.Tag = parts[0]; // Keep only the position
                    }
                    // Handle Cantake tags
                    else if (tagStr.Contains("/Cantake"))
                    {
                        string[] parts = tagStr.Split('/');
                        string pieceInfo = "";

                        // Look for color-piecename format
                        for (int i = 1; i < parts.Length; i++)
                        {
                            if (parts[i].Contains("-") &&
                                (parts[i].StartsWith("white") || parts[i].StartsWith("black")))
                            {
                                pieceInfo = parts[i];
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(pieceInfo))
                            item.Tag = $"{parts[0]}/{pieceInfo}";
                        else
                            item.Tag = parts[0]; // Keep only the position
                    }
                }
            }
        }

        // method to restart the game once someone get checkmated
        private void ResetGame()
        {
            Application.Restart();
        }
    }
}