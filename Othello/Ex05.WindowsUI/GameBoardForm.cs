using System;
using System.Drawing;
using System.Windows.Forms;
using Ex05.OthelloLogic;

namespace Ex05.WindowsUI
{
    public partial class GameBoardForm : Form
    {
        private const int k_RibLengthOfSquare = 50;
        private const int k_ClientSizeOffset = 24;
        private const int k_PictureBoxOffset = 12;
        private readonly PictureBox[,] r_PictureBoxes;
        private readonly Game r_Game; 

        public GameBoardForm(int i_BoardSize, GameTypeEnum.eGameType i_GameType)
        {
            InitializeComponent();
            ClientSize = new Size((k_RibLengthOfSquare * i_BoardSize) + k_ClientSizeOffset, (k_RibLengthOfSquare * i_BoardSize) + k_ClientSizeOffset);
            r_Game = new Game("Red", "Yellow", i_GameType, i_BoardSize);
            r_PictureBoxes = new PictureBox[r_Game.GameBoard.Size, r_Game.GameBoard.Size];
            initializeForm();
            registerToLogicEvents();
        }

        private void registerToLogicEvents()
        {
            r_Game.GameBoard.m_CoinFliped += onFlip;
            r_Game.m_GameOver += onGameOver;
        }

        private void initializeForm()
        {
            for (int i = 0; i < r_Game.GameBoard.Size; i++)
            {
                for(int j = 0; j < r_Game.GameBoard.Size; j++)
                {
                    r_PictureBoxes[i, j] = new PictureBox();
                    r_PictureBoxes[i, j].Location = new Point(k_PictureBoxOffset + (i * k_RibLengthOfSquare), k_PictureBoxOffset + (j * k_RibLengthOfSquare));
                    r_PictureBoxes[i, j].Name = string.Format("pictureBox{0}{1}", i, j);
                    r_PictureBoxes[i, j].Size = new Size(k_RibLengthOfSquare, k_RibLengthOfSquare);
                    r_PictureBoxes[i, j].BorderStyle = BorderStyle.Fixed3D;
                    r_PictureBoxes[i, j].Click += pictureBox_Click;
                    Controls.Add(r_PictureBoxes[i, j]);
                }
            }

            resetFormForNewGame();
        }

        private void resetFormForNewGame()
        {
            for (int i = 0; i < r_Game.GameBoard.Size; i++)
            {
                for (int j = 0; j < r_Game.GameBoard.Size; j++)
                {
                    Square square = new Square(i, j);
                    r_PictureBoxes[i, j].Enabled = false;
                    r_PictureBoxes[i, j].Image = null;

                    if (r_Game.CurrentPlayer.IsValidMoveDestination(square))
                    {
                        r_PictureBoxes[i, j].BackColor = Color.Green;
                        r_PictureBoxes[i, j].Enabled = true;
                    }
                    else if (r_Game.GameBoard.GetSquareStatus(i, j) == CoinStatusEnum.eCoinStatus.Yellow)
                    {
                        r_PictureBoxes[i, j].Image = Properties.Resources.CoinYellow;
                        r_PictureBoxes[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else if (r_Game.GameBoard.GetSquareStatus(i, j) == CoinStatusEnum.eCoinStatus.Red)
                    {
                        r_PictureBoxes[i, j].Image = Properties.Resources.CoinRed;
                        r_PictureBoxes[i, j].SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox squareThatWasClicked = sender as PictureBox;

            r_Game.PlayOnePlayerTurn(getSquareLocation(squareThatWasClicked));
            if (r_Game.CurrentPlayer.HasAvailableMove())
            {
                Text = string.Format("Othello - {0}'s Turn", r_Game.CurrentPlayer.CoinColor);
                if (r_Game.CurrentPlayer.IsComputer)
                {
                    clearAvailableMovesOnForm();
                    System.Threading.Thread.Sleep(2000);
                    r_Game.PlayOneComputerTurn();
                    while (!r_Game.CurrentPlayer.HasAvailableMove() && r_Game.NextPlayer.HasAvailableMove())
                    {
                        r_Game.SwapPlayers();
                        clearAvailableMovesOnForm();
                        System.Threading.Thread.Sleep(2000);
                        r_Game.PlayOneComputerTurn();
                    }
                }
            }
            else
            {
                r_Game.SwapPlayers();
            }

            Text = string.Format("Othello - {0}'s Turn", r_Game.CurrentPlayer.CoinColor);
            showAvailableMovesOnForm();
        }

        private Square getSquareLocation(PictureBox i_SquareThatWasClicked)
        {
            int row = (i_SquareThatWasClicked.Location.X - k_PictureBoxOffset) / k_RibLengthOfSquare;
            int column = (i_SquareThatWasClicked.Location.Y - k_PictureBoxOffset) / k_RibLengthOfSquare;

            return new Square(row, column);
        }

        private void clearAvailableMovesOnForm()
        {
            foreach (PictureBox pictureBox in r_PictureBoxes)
            {
                if (pictureBox.BackColor == Color.Green)
                {
                    pictureBox.BackColor = Color.Empty;
                    pictureBox.Enabled = false;
                }
            }
        }

        private void showAvailableMovesOnForm()
        {
            clearAvailableMovesOnForm();

            foreach (Move availableMove in r_Game.CurrentPlayer.AvailableMoves)
            {
                int row = availableMove.To.Row;
                int column = availableMove.To.Column;

                r_PictureBoxes[row, column].BackColor = Color.Green;
                r_PictureBoxes[row, column].Enabled = true;
            }
        }

        private void onFlip(int i_Row, int i_Column, CoinStatusEnum.eCoinStatus i_FlipTo)
        {
            switch(i_FlipTo)
            {
                case CoinStatusEnum.eCoinStatus.Red:
                    r_PictureBoxes[i_Row, i_Column].Image = Properties.Resources.CoinRed;
                    r_PictureBoxes[i_Row, i_Column].SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case CoinStatusEnum.eCoinStatus.Yellow:
                    r_PictureBoxes[i_Row, i_Column].Image = Properties.Resources.CoinYellow;
                    r_PictureBoxes[i_Row, i_Column].SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
            }

            r_PictureBoxes[i_Row, i_Column].Refresh();
        }

        private void onGameOver()
        {
            Player winner;
            Player loser;
            string gameOverMessage;

            clearAvailableMovesOnForm();
            r_Game.Player1.UpdateScore();
            r_Game.Player2.UpdateScore();
            if (r_Game.IsADraw())
            {
                gameOverMessage = string.Format("Draw!!! ({0}/{1}) ({2}/{3}){4}Would you like another round?", r_Game.Player1.Score, r_Game.Player1.Score, r_Game.Player1.NumOfWins, r_Game.Player2.NumOfWins, Environment.NewLine);
            }
            else
            {
                r_Game.GetWinnerAndLoser(out winner, out loser);
                winner.NumOfWins += 1;
                gameOverMessage = string.Format("{0} Won!!! ({1}/{2}) ({3}/{4}){5}Would you like another round?", winner.CoinColor, winner.Score, loser.Score, winner.NumOfWins, loser.NumOfWins, Environment.NewLine);
            }

            DialogResult dialogResult = MessageBox.Show(gameOverMessage, "Othello", MessageBoxButtons.YesNo);
            switch (dialogResult)
            {
                case DialogResult.Yes:
                    r_Game.PlayAnotherRound();
                    resetFormForNewGame();
                    break;
                case DialogResult.No:
                    Close();
                    break;
            }
        }
    }
}
