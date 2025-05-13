// Auteur: Thomas Lucking
// Creation: 10/03/2025
// Date de Modification: 2/5/2025 
// Description : Chessboard

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

        public Chessboard()
        {

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
            
            return labels;

        }

    }
}
