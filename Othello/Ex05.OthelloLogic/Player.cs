using System;
using System.Collections.Generic;

namespace Ex05.OthelloLogic
{
    public class Player
    {
        private const int k_MaxNameLength = 20;
        private readonly CoinStatusEnum.eCoinStatus r_CoinColor;
        private string m_Name;
        private bool m_IsComputer;
        private int m_Score = 0;
        private Board m_BoardData;
        private List<Move> m_AvailableMoves = null;
        private int m_NumOfWins = 0;

        public static bool IsValidPlayerName(string i_PlayerName)
        {
            return (i_PlayerName != string.Empty && !i_PlayerName.Contains(" ")) && (i_PlayerName.Length <= k_MaxNameLength);
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public CoinStatusEnum.eCoinStatus CoinColor
        {
            get
            {
                return r_CoinColor;
            }
        }

        public List<Move> AvailableMoves
        {
            get
            {
                return m_AvailableMoves;
            }
        }

        public int NumOfWins
        {
            get
            {
                return m_NumOfWins;
            }

            set
            {
                m_NumOfWins = value;
            }
        }

        public Player(string i_PlayerName, bool i_IsComputer, CoinStatusEnum.eCoinStatus i_CoinColor, Board i_BoardData)
        {
            m_Name = i_PlayerName;
            m_IsComputer = i_IsComputer;
            r_CoinColor = i_CoinColor;
            m_BoardData = i_BoardData;
            m_AvailableMoves = new List<Move>();

            InitializeAvailableMoves();
        }

        public void InitializeAvailableMoves()
        {
            int halfSizeOfBoard = m_BoardData.Size / 2;

            if (r_CoinColor == CoinStatusEnum.eCoinStatus.Red)
            {
                Square UpperBlacKCoin = new Square(halfSizeOfBoard - 1, halfSizeOfBoard);
                Square LowerBlacKCoin = new Square(halfSizeOfBoard, halfSizeOfBoard - 1);

                m_AvailableMoves.Add(new Move(LowerBlacKCoin, new Square(halfSizeOfBoard - 2, halfSizeOfBoard - 1), MoveDirectionEnum.eMoveDirection.Up));
                m_AvailableMoves.Add(new Move(UpperBlacKCoin, new Square(halfSizeOfBoard - 1, halfSizeOfBoard - 2), MoveDirectionEnum.eMoveDirection.Left));
                m_AvailableMoves.Add(new Move(LowerBlacKCoin, new Square(halfSizeOfBoard, halfSizeOfBoard + 1), MoveDirectionEnum.eMoveDirection.Right));
                m_AvailableMoves.Add(new Move(UpperBlacKCoin, new Square(halfSizeOfBoard + 1, halfSizeOfBoard), MoveDirectionEnum.eMoveDirection.Down));
            }
            else
            {
                Square UpperWhiteCoin = new Square(halfSizeOfBoard - 1, halfSizeOfBoard - 1);
                Square LowerWhiteCoin = new Square(halfSizeOfBoard, halfSizeOfBoard);

                m_AvailableMoves.Add(new Move(LowerWhiteCoin, new Square(halfSizeOfBoard - 2, halfSizeOfBoard), MoveDirectionEnum.eMoveDirection.Up));
                m_AvailableMoves.Add(new Move(UpperWhiteCoin, new Square(halfSizeOfBoard - 1, halfSizeOfBoard + 1), MoveDirectionEnum.eMoveDirection.Right));
                m_AvailableMoves.Add(new Move(UpperWhiteCoin, new Square(halfSizeOfBoard + 1, halfSizeOfBoard - 1), MoveDirectionEnum.eMoveDirection.Down));
                m_AvailableMoves.Add(new Move(LowerWhiteCoin, new Square(halfSizeOfBoard, halfSizeOfBoard), MoveDirectionEnum.eMoveDirection.Left));
            }
        }

        public void MakeMove(Square i_MoveDestination)
        {
            foreach (Move availableMove in m_AvailableMoves)
            {
                if (Square.AreEqualSquares(availableMove.To, i_MoveDestination))
                {
                    m_BoardData.UpdateBoardAfterMove(availableMove, r_CoinColor);
                }
            }
        }

        public void UpdateAvailableMoves()
        {
            m_AvailableMoves.Clear();
            for (int i = 0; i < m_BoardData.Size; i++)
            {
                for (int j = 0; j < m_BoardData.Size; j++)
                {
                    if (m_BoardData.GetSquareStatus(i, j) == CoinStatusEnum.eCoinStatus.Uninitialized)
                    {
                        UpdateAvailableMovesInGivenRow(i, j);
                        UpdateAvailableMovesInGivenColumn(i, j);
                        UpdateAvailableMovesInMainDiagonal(i, j);
                        UpdateAvailableMovesInSecondaryDiagonal(i, j);
                    }
                }
            }
        }

        public void UpdateAvailableMovesInGivenRow(int i_Row, int i_Column)
        {
            int j;
            int numCoinsToFlip = 0;

            for (j = i_Column + 1; j < m_BoardData.Size && m_BoardData.GetSquareStatus(i_Row, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i_Row, j) != r_CoinColor; j++)
            {
                numCoinsToFlip++;
            }

            if (j < m_BoardData.Size)
            {
                if (m_BoardData.GetSquareStatus(i_Row, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i_Row, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.Left));
                }
            }

            numCoinsToFlip = 0;
            for (j = i_Column - 1; j >= 0 && m_BoardData.GetSquareStatus(i_Row, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i_Row, j) != r_CoinColor; j--)
            {
                numCoinsToFlip++;
            }

            if (j >= 0)
            {
                if (m_BoardData.GetSquareStatus(i_Row, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i_Row, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.Right));
                }
            }
        }

        public void UpdateAvailableMovesInGivenColumn(int i_Row, int i_Column)
        {
            int i;
            int numCoinsToFlip = 0;

            for (i = i_Row + 1; i < m_BoardData.Size && m_BoardData.GetSquareStatus(i, i_Column) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, i_Column) != r_CoinColor; i++)
            {
                numCoinsToFlip++;
            }

            if (i < m_BoardData.Size)
            {
                if (m_BoardData.GetSquareStatus(i, i_Column) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, i_Column), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.Up));
                }
            }

            numCoinsToFlip = 0;
            for (i = i_Row - 1; i >= 0 && m_BoardData.GetSquareStatus(i, i_Column) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, i_Column) != r_CoinColor; i--)
            {
                numCoinsToFlip++;
            }

            if (i >= 0)
            {
                if (m_BoardData.GetSquareStatus(i, i_Column) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, i_Column), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.Down));
                }
            }
        }

        public void UpdateAvailableMovesInMainDiagonal(int i_Row, int i_Column)
        {
            int i;
            int j;
            int numCoinsToFlip = 0;

            for (i = i_Row + 1, j = i_Column + 1; i < m_BoardData.Size && j < m_BoardData.Size && m_BoardData.GetSquareStatus(i, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, j) != r_CoinColor; i++, j++)
            {
                numCoinsToFlip++;
            }

            if (i < m_BoardData.Size && j < m_BoardData.Size)
            {
                if (m_BoardData.GetSquareStatus(i, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.DiagonalUpLeft));
                }
            }

            numCoinsToFlip = 0;
            for (i = i_Row - 1, j = i_Column - 1; i >= 0 && j >= 0 && m_BoardData.GetSquareStatus(i, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, j) != r_CoinColor; i--, j--)
            {
                numCoinsToFlip++;
            }

            if (i >= 0 && j >= 0)
            {
                if (m_BoardData.GetSquareStatus(i, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.DiagonalDownRight));
                }
            }
        }

        public void UpdateAvailableMovesInSecondaryDiagonal(int i_Row, int i_Column)
        {
            int i;
            int j;
            int numCoinsToFlip = 0;

            for (i = i_Row - 1, j = i_Column + 1; i >= 0 && j < m_BoardData.Size && m_BoardData.GetSquareStatus(i, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, j) != r_CoinColor; i--, j++)
            {
                numCoinsToFlip++;
            }

            if (i >= 0 && j < m_BoardData.Size)
            {
                if (m_BoardData.GetSquareStatus(i, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.DiagonalDownLeft));
                }
            }

            numCoinsToFlip = 0;
            for (i = i_Row + 1, j = i_Column - 1; i < m_BoardData.Size && j >= 0 && m_BoardData.GetSquareStatus(i, j) != CoinStatusEnum.eCoinStatus.Uninitialized && m_BoardData.GetSquareStatus(i, j) != r_CoinColor; i++, j--)
            {
                numCoinsToFlip++;
            }

            if (i < m_BoardData.Size && j >= 0)
            {
                if (m_BoardData.GetSquareStatus(i, j) == r_CoinColor && numCoinsToFlip > 0)
                {
                    m_AvailableMoves.Add(new Move(new Square(i, j), new Square(i_Row, i_Column), MoveDirectionEnum.eMoveDirection.DiagonalUpRight));
                }
            }
        }

        public bool IsValidMoveDestination(Square i_MoveDestination)
        {
            bool isValidMoveDestination = false;

            foreach (Move availableMove in m_AvailableMoves)
            {
                if (Square.AreEqualSquares(availableMove.To, i_MoveDestination))
                {
                    isValidMoveDestination = true;
                    break;
                }
            }

            return isValidMoveDestination;
        }

        public void UpdateScore()
        {
            for (int rows = 0; rows < m_BoardData.Size; rows++)
            {
                for (int columns = 0; columns < m_BoardData.Size; columns++)
                {
                    if (m_BoardData.GetSquareStatus(rows, columns) == r_CoinColor)
                    {
                        m_Score++;
                    }
                }
            }
        }

        public void MakeComputerMove()
        {
            Random rnd_Number = new Random();
            int rnd_index = rnd_Number.Next(m_AvailableMoves.Count);
            Move newComputerMove = m_AvailableMoves[rnd_index];

            MakeMove(newComputerMove.To);
        }

        public bool HasAvailableMove()
        {
            return m_AvailableMoves.Count != 0;
        }
    }
}
