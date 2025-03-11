using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    internal class Chessboard
    {
        // Declare labels as a class member
        public Label[,] labels;
        private Image pawnImage;
        private Image knightImage;
        private Image rookImage;
        private Image queenImage;
        private Image kingImage;
        private Image bishopImage;
        private Image blackpawnImage;
        private Image blackknightImage;
        private Image blackrookImage;
        private Image blackqueenImage;
        private Image blackkingImage;
        private Image blackbishopImage;




        public Chessboard()
        {
            // Load pawn image from resources or file

            


        } 
        public Label[,] InitializeChessboard()
        {
            labels = new Label[8, 8];
            int labelWidth = 75;
            int labelHeight = 75;
            int margin = 5;

            // Create and place labels in a grid
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    labels[row, col] = new Label();
                    labels[row, col].Size = new Size(labelWidth, labelHeight);
                    labels[row, col].Location = new Point(col * (labelWidth + margin), row * (labelHeight + margin));
                    labels[row, col].TextAlign = ContentAlignment.MiddleCenter;
                    labels[row, col].Tag = Convert.ToString(col) + "-" + Convert.ToString(row);


                    // Set the background color to alternate between black and white
                    if ((row + col) % 2 == 0)
                    {
                        labels[row, col].BackColor = Color.White; // Light square
                    }
                    else
                    {
                        labels[row, col].BackColor = Color.DarkGray; // Dark square
                    }
                    labels[row, col].BorderStyle = BorderStyle.FixedSingle;
                }
            }
            // Placing down the white pieces
            
            return labels;

        }

        private void PlacePawn(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = pawnImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }  // Attach the click event for selecting the pawn
        }

        // Place a black pawn on a specific row and column
        
        public void PlaceRook(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = rookImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceKnight(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = knightImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBishop(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = bishopImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceQueen(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = queenImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceKing(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = kingImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackpawn(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackpawnImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackrook(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackrookImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackbishop(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackbishopImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackknight(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackknightImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackqueen(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackqueenImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
        public void PlaceBlackking(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8)
            {

                labels[row, col].Image = blackkingImage;
                labels[row, col].ImageAlign = ContentAlignment.MiddleCenter;
            }

        }
    }
}
