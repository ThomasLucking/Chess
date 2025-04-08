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
            // Group movements by direction (up, down, left, right, diagonals)
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

            // Now process each direction separately
            foreach (var direction in directions.Values)
            {
                // Sort the moves from closest to farthest using their Manhattan distance from the origin (0,0).
                var sortedMoves = direction.OrderBy(m => Math.Abs(m[0]) + Math.Abs(m[1])).ToList();
                // This code removes the two-square move option for pawns that aren't in their starting position.
                if (piecename == "Pawn")
                {
                    // if(color == "white")
                    if (sortedMoves.Count > 1)
                    {
                        if (PositionY != 6 && color == "white")
                        {
                            sortedMoves.RemoveAt(1);
                            Console.WriteLine("white R");
                        }
                        if (PositionY != 1 && color == "black")
                        {
                            sortedMoves.RemoveAt(1);
                            Console.WriteLine("black R");
                        }
                    }

                }

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
                                    {   // tagpartst[1] is equal to for example " 4-6/black-knight" then piece parts is "black" which is the targetpiececolor. 
                                        targetPieceColor = pieceParts[0];
                                    }
                                }
                            }

                            // If it's an opponent's piece, mark it as capturable
                            if (!string.IsNullOrEmpty(targetPieceColor) && targetPieceColor != currentPieceColor)
                            {
                                
                                // Store the original image t
                                Image originalImage = labels[newPosY, newPosX].Image;

                                
                                // Adds the tag "/Cantake"
                                labels[newPosY, newPosX].Tag += "/Cantake";

                                Console.WriteLine($"Enemy piece at ({newPosX}, {newPosY}) can be captured.");
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
                    if (tagStr.Contains("/Cantake"))
                    {
                        // We don't need to clear the image here as we'll set the new image directly
                        string[] tag = tagStr.Split('/');
                        if (tag.Length > 0)
                        {
                            item.Tag = tag[0]; // Remove all tags after the first "/"
                        }
                    }
                }
            }
        }










    }


    }

