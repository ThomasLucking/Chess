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
            // We need to organize pieceMovementPossibilities by direction
            // This assumes pieceMovementPossibilities contains all possible moves for the piece
            // Group movements by direction (up, down, left, right, diagonals)
            var directions = new Dictionary<string, List<int[]>>();
            // For a queen/bishop/rook, we need to organize directions properly
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

            // Now process each direction separately
            foreach (var direction in directions.Values)
            {
                // Sort the moves from closest to farthest
                var sortedMoves = direction.OrderBy(m => Math.Abs(m[0]) + Math.Abs(m[1])).ToList();
                // Process moves in this direction until blocked
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
                            Console.WriteLine($"Position ({newPosX}, {newPosY}) is empty.");
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
                            if (!string.IsNullOrEmpty(targetPieceColor) && !string.IsNullOrEmpty(currentPieceColor) &&
                            targetPieceColor != currentPieceColor)
                            {
                                // If this is showing possible moves (marking as "Cantake")
                                if (!Convert.ToString(labels[newPosY, newPosX].Tag).Contains("/Cantake"))
                                {
                                    // Store original image before adding visual indicator
                                    Image originalImage = labels[newPosY, newPosX].Image;

                                    // Add a visual indicator for "Cantake" (e.g., red border, highlight, etc.)
                                    // For example, you might create a new combined image with a red border
                                    // This is a placeholder - implement your visual indicator logic here
                                    // labels[newPosY, newPosX].BackColor = Color.Red; // Simple example

                                    // Add the Cantake tag
                                    labels[newPosY, newPosX].Tag += "/Cantake";
                                    Console.WriteLine($"Position ({newPosX}, {newPosY}) has an opponent's piece - can take.");
                                }
                                // If player is actually making the move to a "Cantake" position
                                else if (Convert.ToString(labels[newPosY, newPosX].Tag).Contains("/Cantake"))
                                {
                                    // Capture the piece at the destination
                                    labels[newPosY, newPosX].Image = labels[newPosY, newPosX].Image;
                                    labels[newPosY, newPosX].Tag = labels[newPosY, newPosX].Tag.ToString().Replace("/Cantake", "");

                                    // Clear the original position
                                    labels[newPosY, newPosX].Image = null;
                                    labels[newPosY, newPosX].Tag = null;

                                    Console.WriteLine($"Moved piece from ({newPosY}, {newPosX}) to ({newPosY}, {newPosX}) and captured opponent's piece.");

                                    // After the move, clear all "Cantake" tags from the board
                                    ClearAllCantakeTags();
                                }
                            }

                            // Found a piece in this direction, stop here
                            Console.WriteLine($"Position ({newPosX}, {newPosY}) has a piece - stopping in this direction.");
                            break; // Stop processing this direction
                        }
                    }
                    else
                    {
                        // Out of bounds
                        Console.WriteLine($"Position ({newPosX}, {newPosY}) is out of bounds.");
                        break; // Stop processing this direction
                    }
                }
            }
        }













        public void MovePiece(string movex, string movey, Label[,] labels)
        {
            
            // Convert string to integer for movement
            int newX = Convert.ToInt32(movex);
            int newY = Convert.ToInt32(movey);



            // Check if there is a piece in the way (using the Tag or Image property)
            

            // Check if the new position is within bounds
            if (newX >= 0 && newX < labels.GetLength(1) && newY >= 0 && newY < labels.GetLength(0))
            {
                // If no pieces block the move, proceed with moving the piece
                labels[positionY, positionX].Image = null;
                labels[positionY, positionX].Tag = positionX + "-" + positionY;

                // Update the piece's position
                positionX = newX;
                positionY = newY;

                // Set the image at the new position
                labels[positionY, positionX].Image = image;
                labels[positionY, positionX].Tag = positionX + "-" + positionY + "/" + Colorpiece + "-" + piecename;  // Update the pieces name and color and position
                



            }



            // Cleanup potential move indicators ("/Canmove" tags) after the move
            foreach (var item in labels)
            {
                // Handle Canmove tags
                if (Convert.ToString(item.Tag).Contains("/Canmove"))
                {
                    item.Image = null;
                    string[] tag = Convert.ToString(item.Tag).Split('/');
                    if (tag.Length > 1)
                    {
                        if (tag[1] == "Canmove")
                        {
                            item.Tag = tag[0]; // Remove "/Canmove" from the tag
                        }
                    }
                }
               
            }


        }
        private void ClearAllCantakeTags()
        {
            foreach (var item in labels)
            {
                if (item != null && item.Tag != null && Convert.ToString(item.Tag).Contains("/Cantake"))
                {
                    item.Tag = Convert.ToString(item.Tag).Replace("/Cantake", "");
                }
            }
        }


    }

}