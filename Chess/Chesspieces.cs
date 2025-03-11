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
            // Iterate through the list of movement possibilities (piecemovementpossibilities)
            foreach (var move in pieceMovementPossibilities)
            {
                int newPosX = PositionX + move[0]; // Calculate the new X position based on current position and movement
                int newPosY = PositionY + move[1]; // Calculate the new Y position based on current position and movement

                // Ensure that the new position is within bounds
                if (newPosX >= 0 && newPosX < 8 && newPosY >= 0 && newPosY < 8)
                {
                    // Ensure that the target square is empty (no piece already present)
                    if (labels[newPosY, newPosX].Image == null)
                    {
                        // If empty, display a dot and tag it as a valid move
                        labels[newPosY, newPosX].Image = Properties.Resources.dot; // Assuming dot is a small image representing a possible move
                        labels[newPosY, newPosX].Tag += "/Canmove"; // Tag it as a valid move

                        Console.WriteLine($"Position ({newPosX}, {newPosY}) is empty.");
                    }
                }
                else
                {
                    // Out of bounds (invalid position)
                    Console.WriteLine($"Position ({newPosX}, {newPosY}) is out of bounds.");
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


    }

}