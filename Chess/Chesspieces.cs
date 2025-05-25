// Auteur: Thomas Lucking
// Creation: 10/03/2025
// Date de Modification: 8/5/2025 
// Description: Updated Chesspieces class with proper rotation support

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Chess
{
    internal class Chesspieces
    {
        public Label[,] labels = new Label[8, 8]; // 2D array for the chessboard labels

        private List<int[]> pieceMovementPossibilities;
        public List<int[]> PieceMovementPossibilities
        {
            get { return pieceMovementPossibilities; }
            set { pieceMovementPossibilities = value; }
        }

        public Image image; // Made public so Form1 can access it

        private int positionX = 0;
        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        int positionY = 0;
        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        private string Colorpiece;
        public string color
        {
            get { return Colorpiece; }
            set { Colorpiece = value; }
        }

        private string Piecename;
        public string piecename
        {
            get { return Piecename; }
            set { Piecename = value; }
        }

        // Define the board array (this will hold references to the labels of the pieces)
        private Label[,] board = new Label[8, 8];

        public Chesspieces(Image _image, int _posY, int _posX, string _Colorpiece, List<int[]> _piecemovementpossibilities, string _Piecename)
        {
            image = _image;
            positionX = _posX;
            positionY = _posY;
            Colorpiece = _Colorpiece;
            Piecename = _Piecename;

            pieceMovementPossibilities = new List<int[]>();
            foreach (var movement in _piecemovementpossibilities)
            {
                pieceMovementPossibilities.Add((int[])movement.Clone()); // Cloner chaque tableau pour éviter la référence partagée
            }
        }

        public void PlacePiece(int x, int y, Label[,] labels)
        {
            // Place the image of the piece on the given label (x, y)
            labels[y, x].Image = image;
            labels[y, x].ImageAlign = ContentAlignment.MiddleCenter;
            labels[y, x].Tag += "/" + Colorpiece + "-" + piecename;
        }

        // Helper methods for coordinate conversion
        private (int visualX, int visualY) LogicalToVisual(int logicalX, int logicalY, bool isBoardRotated)
        {
            if (isBoardRotated)
            {
                return (7 - logicalX, 7 - logicalY);
            }
            return (logicalX, logicalY);
        }

        private (int logicalX, int logicalY) VisualToLogical(int visualX, int visualY, bool isBoardRotated)
        {
            if (isBoardRotated)
            {
                return (7 - visualX, 7 - visualY);
            }
            return (visualX, visualY);
        }

        public void GetMovePossibilitiesWithRotation(Label[,] labels, bool isBoardRotated)
        {
            // Clear any existing move indicators first
            ClearMoveIndicators(labels);

            // Special handling for Pawns
            if (piecename == "Pawn")
            {
                // Handle forward movements first
                foreach (var move in pieceMovementPossibilities)
                {
                    int x = move[0];
                    int y = move[1];

                    // Forward movement (not diagonal)
                    if (x == 0)
                    {
                        int newLogicalX = PositionX + x;
                        int newLogicalY = PositionY + y;

                        // Check if in bounds
                        if (newLogicalX >= 0 && newLogicalX < 8 && newLogicalY >= 0 && newLogicalY < 8)
                        {
                            // Convert to visual coordinates
                            var (visualX, visualY) = LogicalToVisual(newLogicalX, newLogicalY, isBoardRotated);

                            // Forward movement requires empty space
                            if (labels[visualY, visualX].Image == null)
                            {
                                // Double move only if path is clear
                                if (Math.Abs(y) == 2)
                                {
                                    // For white pawn moving up (y = -2)
                                    if (color == "white" && PositionY == 6)
                                    {
                                        // Check if the square in between is empty
                                        var (betweenVisualX, betweenVisualY) = LogicalToVisual(PositionX, PositionY - 1, isBoardRotated);
                                        if (labels[betweenVisualY, betweenVisualX].Image == null)
                                        {
                                            labels[visualY, visualX].Image = Properties.Resources.dot;
                                            labels[visualY, visualX].Tag += "/Canmove";
                                        }
                                    }
                                    // For black pawn moving down (y = 2)
                                    else if (color == "black" && PositionY == 1)
                                    {
                                        // Check if the square in between is empty
                                        var (betweenVisualX, betweenVisualY) = LogicalToVisual(PositionX, PositionY + 1, isBoardRotated);
                                        if (labels[betweenVisualY, betweenVisualX].Image == null)
                                        {
                                            labels[visualY, visualX].Image = Properties.Resources.dot;
                                            labels[visualY, visualX].Tag += "/Canmove";
                                        }
                                    }
                                }
                                else // Single square move
                                {
                                    labels[visualY, visualX].Image = Properties.Resources.dot;
                                    labels[visualY, visualX].Tag += "/Canmove";
                                }
                            }
                        }
                    }
                    // Diagonal movement for captures
                    else if ((x == -1 || x == 1) && ((color == "white" && y == -1) || (color == "black" && y == 1)))
                    {
                        int newLogicalX = PositionX + x;
                        int newLogicalY = PositionY + y;

                        // Check if in bounds
                        if (newLogicalX >= 0 && newLogicalX < 8 && newLogicalY >= 0 && newLogicalY < 8)
                        {
                            // Convert to visual coordinates
                            var (visualX, visualY) = LogicalToVisual(newLogicalX, newLogicalY, isBoardRotated);

                            // Check if there's a piece to capture
                            if (labels[visualY, visualX].Image != null && labels[visualY, visualX].Image != Properties.Resources.dot)
                            {
                                // Get target piece color
                                string targetPieceColor = "";
                                if (labels[visualY, visualX].Tag != null && labels[visualY, visualX].Tag.ToString().Contains("/"))
                                {
                                    string[] tagParts = labels[visualY, visualX].Tag.ToString().Split('/');
                                    if (tagParts.Length > 1)
                                    {
                                        string[] pieceParts = tagParts[1].Split('-');
                                        if (pieceParts.Length > 0)
                                        {
                                            targetPieceColor = pieceParts[0];
                                        }
                                    }
                                }

                                // Only allow diagonal capture of opponent pieces
                                if (targetPieceColor != color)
                                {
                                    labels[visualY, visualX].Tag += "/Cantake";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Original code for other pieces with rotation support
                var directions = new Dictionary<string, List<int[]>>();

                directions["up"] = new List<int[]>();
                directions["down"] = new List<int[]>();
                directions["left"] = new List<int[]>();
                directions["right"] = new List<int[]>();
                directions["upLeft"] = new List<int[]>();
                directions["upRight"] = new List<int[]>();
                directions["downLeft"] = new List<int[]>();
                directions["downRight"] = new List<int[]>();

                // Populate each direction with appropriate movements
                foreach (var move in pieceMovementPossibilities)
                {
                    int x = move[0];
                    int y = move[1];
                    // Determine which direction this move belongs to
                    if (x == 0 && y < 0) directions["up"].Add(move);
                    else if (x == 0 && y > 0) directions["down"].Add(move);
                    else if (x < 0 && y == 0) directions["left"].Add(move);
                    else if (x > 0 && y == 0) directions["right"].Add(move);
                    else if (x < 0 && y < 0) directions["upLeft"].Add(move);
                    else if (x > 0 && y < 0) directions["upRight"].Add(move);
                    else if (x < 0 && y > 0) directions["downLeft"].Add(move);
                    else if (x > 0 && y > 0) directions["downRight"].Add(move);
                }

                // Get the current piece's color
                string currentPieceColor = color;

                // Process each direction separately
                foreach (var direction in directions.Values)
                {
                    // Sort the moves from closest to farthest
                    var sortedMoves = direction.OrderBy(m => Math.Abs(m[0]) + Math.Abs(m[1])).ToList();

                    foreach (var move in sortedMoves)
                    {
                        int newLogicalX = PositionX + move[0];
                        int newLogicalY = PositionY + move[1];

                        // Check if in bounds
                        if (newLogicalX >= 0 && newLogicalX < 8 && newLogicalY >= 0 && newLogicalY < 8)
                        {
                            // Convert to visual coordinates
                            var (visualX, visualY) = LogicalToVisual(newLogicalX, newLogicalY, isBoardRotated);

                            // Check if square is empty
                            if (labels[visualY, visualX].Image == null)
                            {
                                // Place a dot
                                labels[visualY, visualX].Image = Properties.Resources.dot;
                                labels[visualY, visualX].Tag += "/Canmove";
                            }
                            else if (labels[visualY, visualX].Image != Properties.Resources.dot)
                            {
                                // Check if it's an opponent's piece
                                string targetPieceColor = "";
                                if (labels[visualY, visualX].Tag != null && labels[visualY, visualX].Tag.ToString().Contains("/"))
                                {
                                    string[] tagParts = labels[visualY, visualX].Tag.ToString().Split('/');
                                    if (tagParts.Length > 1)
                                    {
                                        string[] pieceParts = tagParts[1].Split('-');
                                        if (pieceParts.Length > 0)
                                        {
                                            targetPieceColor = pieceParts[0];
                                        }
                                    }
                                }

                                // If it's an opponent's piece, mark it as capturable
                                if (!string.IsNullOrEmpty(targetPieceColor) && targetPieceColor != currentPieceColor)
                                {
                                    labels[visualY, visualX].Tag += "/Cantake";
                                }

                                // Found a piece in this direction, stop here
                                break; // Stop processing this direction
                            }
                        }
                        else
                        {
                            // Out of bounds
                            break; // Stop processing this direction
                        }
                    }
                }
            }
        }

        // Keep the original method for backward compatibility
        public void GetMovePossibilities(Label[,] labels)
        {
            GetMovePossibilitiesWithRotation(labels, false);
        }

        public void MovePieceWithRotation(int newLogicalX, int newLogicalY, Label[,] labels, bool isBoardRotated)
        {
            if (newLogicalX >= 0 && newLogicalX < 8 && newLogicalY >= 0 && newLogicalY < 8)
            {
                // Convert current logical position to visual position
                var (currentVisualX, currentVisualY) = LogicalToVisual(positionX, positionY, isBoardRotated);

                // Remove the piece from its current visual position
                labels[currentVisualY, currentVisualX].Image = null;
                labels[currentVisualY, currentVisualX].Tag = currentVisualX + "-" + currentVisualY;

                // Update the piece's logical position
                positionX = newLogicalX;
                positionY = newLogicalY;

                // Convert new logical position to visual position
                var (newVisualX, newVisualY) = LogicalToVisual(newLogicalX, newLogicalY, isBoardRotated);

                // Place the piece at its new visual position
                labels[newVisualY, newVisualX].Image = image;
                labels[newVisualY, newVisualX].Tag = newVisualX + "-" + newVisualY + "/" + Colorpiece + "-" + piecename;
            }

            // Cleanup move indicators
            foreach (var item in labels)
            {
                if (item.Tag != null)
                {
                    string tagStr = Convert.ToString(item.Tag);

                    if (tagStr.Contains("/Canmove") || tagStr.Contains("/Cantake"))
                    {
                        string[] mainParts = tagStr.Split('/');
                        string positionPart = mainParts[0];

                        string pieceInfo = "";
                        if (mainParts.Length > 1 && mainParts[1].Contains("-"))
                        {
                            for (int i = 1; i < mainParts.Length; i++)
                            {
                                if (mainParts[i].Contains("-") &&
                                    (mainParts[i].StartsWith("white") || mainParts[i].StartsWith("black")))
                                {
                                    pieceInfo = mainParts[i];
                                    break;
                                }
                            }
                        }

                        if (tagStr.Contains("/Canmove"))
                        {
                            item.Image = null;
                        }

                        if (!string.IsNullOrEmpty(pieceInfo))
                        {
                            item.Tag = positionPart + "/" + pieceInfo;
                        }
                        else
                        {
                            item.Tag = positionPart;
                        }
                    }
                }
            }
        }

        // Keep the original method for backward compatibility
        public void MovePiece(string movex, string movey, Label[,] labels)
        {
            // Convert string to integer for movement
            int newX = Convert.ToInt32(movex);
            int newY = Convert.ToInt32(movey);

            if (newX >= 0 && newX < labels.GetLength(1) && newY >= 0 && newY < labels.GetLength(0))
            {
                // Remove the piece from its current position
                labels[positionY, positionX].Image = null;
                labels[positionY, positionX].Tag = positionX + "-" + positionY;

                // Update the piece's position
                positionX = newX;
                positionY = newY;

                // Set the image at the new position and update tag
                labels[positionY, positionX].Image = image;
                labels[positionY, positionX].Tag = positionX + "-" + positionY + "/" + Colorpiece + "-" + piecename;
            }

            // Cleanup potential move indicators ("/Canmove" and "/Cantake" tags) after the move
            foreach (var item in labels)
            {
                if (item.Tag != null)
                {
                    string tagStr = Convert.ToString(item.Tag);

                    if (tagStr.Contains("/Canmove") || tagStr.Contains("/Cantake"))
                    {
                        // Extract the position part and any piece information
                        string[] mainParts = tagStr.Split('/');
                        string positionPart = mainParts[0]; // The position part (e.g. "5-3")

                        string pieceInfo = "";
                        // Check if there's piece information after position
                        if (mainParts.Length > 1 && mainParts[1].Contains("-"))
                        {
                            // Find where the color-piecename part is
                            for (int i = 1; i < mainParts.Length; i++)
                            {
                                if (mainParts[i].Contains("-") &&
                                    (mainParts[i].StartsWith("white") || mainParts[i].StartsWith("black")))
                                {
                                    pieceInfo = mainParts[i];
                                    break;
                                }
                            }
                        }

                        // If it's a movable square (has the dot), clear the image
                        if (tagStr.Contains("/Canmove"))
                        {
                            item.Image = null;
                        }

                        // Reset the tag, preserving piece information if it exists
                        if (!string.IsNullOrEmpty(pieceInfo))
                        {
                            item.Tag = positionPart + "/" + pieceInfo;
                        }
                        else
                        {
                            item.Tag = positionPart;
                        }
                    }
                }
            }
        }

        public void MovePieceLogical(int newX, int newY, Label[,] labels)
        {
            if (newX >= 0 && newX < 8 && newY >= 0 && newY < 8)
            {
                // Remove the piece from its current position
                labels[positionY, positionX].Image = null;
                labels[positionY, positionX].Tag = positionX + "-" + positionY;

                // Update the piece's logical position
                positionX = newX;
                positionY = newY;

                // Place the piece at its new position
                labels[positionY, positionX].Image = image;
                labels[positionY, positionX].Tag = positionX + "-" + positionY + "/" + Colorpiece + "-" + piecename;
            }

            // Cleanup move indicators
            foreach (var item in labels)
            {
                if (item.Tag != null)
                {
                    string tagStr = Convert.ToString(item.Tag);

                    if (tagStr.Contains("/Canmove") || tagStr.Contains("/Cantake"))
                    {
                        string[] mainParts = tagStr.Split('/');
                        string positionPart = mainParts[0];

                        string pieceInfo = "";
                        if (mainParts.Length > 1 && mainParts[1].Contains("-"))
                        {
                            for (int i = 1; i < mainParts.Length; i++)
                            {
                                if (mainParts[i].Contains("-") &&
                                    (mainParts[i].StartsWith("white") || mainParts[i].StartsWith("black")))
                                {
                                    pieceInfo = mainParts[i];
                                    break;
                                }
                            }
                        }

                        if (tagStr.Contains("/Canmove"))
                        {
                            item.Image = null;
                        }

                        if (!string.IsNullOrEmpty(pieceInfo))
                        {
                            item.Tag = positionPart + "/" + pieceInfo;
                        }
                        else
                        {
                            item.Tag = positionPart;
                        }
                    }
                }
            }
        }

        private void ClearMoveIndicators(Label[,] labels)
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
    }
}