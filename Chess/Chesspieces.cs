// Auteur: Thomas Lucking
// Creation: 10/03/2025
// Date de Modification: 8/5/2025 
// Description: Description : Toutes les fonctionnalités nécessaires au jeu d'échecs comme les possibilités de mouvement, la méthode de capture des pièces. La méthode de déplacement. 

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


        Image image;


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

        // public bool Hasmoved { get; set; } = false;

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



        public void GetMovePossibilities(Label[,] labels)
        {
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
                        int newPosX = PositionX + x;
                        int newPosY = PositionY + y;

                        // Check if in bounds
                        if (newPosX >= 0 && newPosX < 8 && newPosY >= 0 && newPosY < 8)
                        {
                            // Forward movement requires empty space
                            if (labels[newPosY, newPosX].Image == null)
                            {
                                // Double move only if path is clear
                                if (Math.Abs(y) == 2)
                                {
                                    // For white pawn moving up (y = -2)
                                    if (color == "white" && PositionY == 6)
                                    {
                                        // Check if the square in between is empty
                                        if (labels[PositionY - 1, PositionX].Image == null)
                                        {
                                            labels[newPosY, newPosX].Image = Properties.Resources.dot;
                                            labels[newPosY, newPosX].Tag += "/Canmove";
                                        }
                                    }
                                    // For black pawn moving down (y = 2)
                                    else if (color == "black" && PositionY == 1)
                                    {
                                        // Check if the square in between is empty
                                        if (labels[PositionY + 1, PositionX].Image == null)
                                        {
                                            labels[newPosY, newPosX].Image = Properties.Resources.dot;
                                            labels[newPosY, newPosX].Tag += "/Canmove";
                                        }
                                    }
                                }
                                else // Single square move
                                {
                                    labels[newPosY, newPosX].Image = Properties.Resources.dot;
                                    labels[newPosY, newPosX].Tag += "/Canmove";
                                }
                            }
                        }
                    }
                    // Diagonal movement for captures
                    else if ((x == -1 || x == 1) && ((color == "white" && y == -1) || (color == "black" && y == 1)))
                    {
                        int newPosX = PositionX + x;
                        int newPosY = PositionY + y;

                        // Check if in bounds
                        if (newPosX >= 0 && newPosX < 8 && newPosY >= 0 && newPosY < 8)
                        {
                            // Check if there's a piece to capture
                            if (labels[newPosY, newPosX].Image != null && labels[newPosY, newPosX].Image != Properties.Resources.dot)
                            {
                                // Get target piece color
                                string targetPieceColor = "";
                                if (labels[newPosY, newPosX].Tag != null && labels[newPosY, newPosX].Tag.ToString().Contains("/"))
                                {
                                    string[] tagParts = labels[newPosY, newPosX].Tag.ToString().Split('/');
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
                                    labels[newPosY, newPosX].Tag += "/Cantake";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // Original code for other pieces
                var directions = new Dictionary<string, List<int[]>>();

                // Example for organizing directions:
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
                string currentPieceColor = "";
                if (labels[PositionY, PositionX].Tag != null && labels[PositionY, PositionX].Tag.ToString().Contains("/"))
                {
                    string[] tagParts = labels[PositionY, PositionX].Tag.ToString().Split('/');
                    if (tagParts.Length > 1)
                    {
                        string[] pieceParts = tagParts[1].Split('-');
                        if (pieceParts.Length > 0)
                        {
                            currentPieceColor = pieceParts[0];
                        }
                    }
                }

                // Process each direction separately
                foreach (var direction in directions.Values)
                {
                    // Sort the moves from closest to farthest
                    var sortedMoves = direction.OrderBy(m => Math.Abs(m[0]) + Math.Abs(m[1])).ToList();

                    foreach (var move in sortedMoves)
                    {
                        int newPosX = PositionX + move[0];
                        int newPosY = PositionY + move[1];

                        // Check if in bounds
                        if (newPosX >= 0 && newPosX < 8 && newPosY >= 0 && newPosY < 8)
                        {
                            // Check if square is empty
                            if (labels[newPosY, newPosX].Image == null)
                            {
                                // Place a dot
                                labels[newPosY, newPosX].Image = Properties.Resources.dot;
                                labels[newPosY, newPosX].Tag += "/Canmove";
                            }
                            else if (labels[newPosY, newPosX].Image != Properties.Resources.dot)
                            {
                                // Check if it's an opponent's piece
                                string targetPieceColor = "";
                                if (labels[newPosY, newPosX].Tag != null && labels[newPosY, newPosX].Tag.ToString().Contains("/"))
                                {
                                    string[] tagParts = labels[newPosY, newPosX].Tag.ToString().Split('/');
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
                                    labels[newPosY, newPosX].Tag += "/Cantake";
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


        public void MovePiece(string movex, string movey, Label[,] labels)
        {
            // Convert string to integer for movement
            int newX = Convert.ToInt32(movex);
            int newY = Convert.ToInt32(movey);

            if (newX >= 0 && newX < labels.GetLength(1) && newY >= 0 && newY < labels.GetLength(0))
            {
                bool isCapture = false;

                // Check if this is a capture move
                if (labels[newY, newX].Tag != null && labels[newY, newX].Tag.ToString().Contains("/Cantake"))
                {
                    isCapture = true;
                    Console.WriteLine("Capturing piece at " + newX + "," + newY);
                }

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

    } 

    }


