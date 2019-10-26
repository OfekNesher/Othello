namespace Ex05.OthelloLogic
{
    public delegate void FlipEventHandler(int row, int column, CoinStatusEnum.eCoinStatus flipTo);

    public class Board
    {
        private const int k_SmallSize = 6;
        private const int k_LargeSize = 8;
        private int m_Size;
        private CoinStatusEnum.eCoinStatus[,] m_Board;

        public event FlipEventHandler m_CoinFliped;

        public static bool IsValidBoardSize(int i_BoardSize)
        {
            return (i_BoardSize == k_SmallSize) || (i_BoardSize == k_LargeSize);
        }

        public int Size
        {
            get
            {
                return m_Size;
            }
        }

        public Board(int i_BoardSize)
        {
            m_Size = i_BoardSize;
            m_Board = new CoinStatusEnum.eCoinStatus[i_BoardSize, i_BoardSize];

            InitializeBoard();
        }

        public void InitializeBoard()
        {
            int halfSizeOfBoard = m_Size / 2;

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    m_Board[i, j] = CoinStatusEnum.eCoinStatus.Uninitialized;
                }
            }

            m_Board[halfSizeOfBoard - 1, halfSizeOfBoard - 1] = CoinStatusEnum.eCoinStatus.Yellow;
            m_Board[halfSizeOfBoard, halfSizeOfBoard - 1] = CoinStatusEnum.eCoinStatus.Red;
            m_Board[halfSizeOfBoard - 1, halfSizeOfBoard] = CoinStatusEnum.eCoinStatus.Red;
            m_Board[halfSizeOfBoard, halfSizeOfBoard] = CoinStatusEnum.eCoinStatus.Yellow;
        }

        public CoinStatusEnum.eCoinStatus GetSquareStatus(int i_Row, int i_Column)
        {
            return m_Board[i_Row, i_Column];
        }

        public void UpdateBoardAfterMove(Move i_MoveThatWasMade, CoinStatusEnum.eCoinStatus i_PlayerCoinColor)
        {
            int i;
            int j;

            switch (i_MoveThatWasMade.Direction)
            {
                case MoveDirectionEnum.eMoveDirection.Up:
                    for (i = i_MoveThatWasMade.From.Row - 1; i >= i_MoveThatWasMade.To.Row; i--)
                    {
                        m_Board[i, i_MoveThatWasMade.From.Column] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, i_MoveThatWasMade.From.Column, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.Right:
                    for (j = i_MoveThatWasMade.From.Column + 1; j <= i_MoveThatWasMade.To.Column; j++)
                    {
                        m_Board[i_MoveThatWasMade.From.Row, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i_MoveThatWasMade.From.Row, j, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.Left:
                    for (j = i_MoveThatWasMade.From.Column - 1; j >= i_MoveThatWasMade.To.Column; j--)
                    {
                        m_Board[i_MoveThatWasMade.From.Row, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i_MoveThatWasMade.From.Row, j, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.Down:
                    for (i = i_MoveThatWasMade.From.Row + 1; i <= i_MoveThatWasMade.To.Row; i++)
                    {
                        m_Board[i, i_MoveThatWasMade.From.Column] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, i_MoveThatWasMade.From.Column, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.DiagonalUpRight:
                    for (i = i_MoveThatWasMade.From.Row - 1, j = i_MoveThatWasMade.From.Column + 1; i >= i_MoveThatWasMade.To.Row && j <= i_MoveThatWasMade.To.Column; i--, j++)
                    {
                        m_Board[i, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, j, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.DiagonalUpLeft:
                    for (i = i_MoveThatWasMade.From.Row - 1, j = i_MoveThatWasMade.From.Column - 1; i >= i_MoveThatWasMade.To.Row && j >= i_MoveThatWasMade.To.Column; i--, j--)
                    {
                        m_Board[i, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, j, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.DiagonalDownRight:
                    for (i = i_MoveThatWasMade.From.Row + 1, j = i_MoveThatWasMade.From.Column + 1; i <= i_MoveThatWasMade.To.Row && j <= i_MoveThatWasMade.To.Column; i++, j++)
                    {
                        m_Board[i, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, j, i_PlayerCoinColor);
                    }

                    break;
                case MoveDirectionEnum.eMoveDirection.DiagonalDownLeft:
                    for (i = i_MoveThatWasMade.From.Row + 1, j = i_MoveThatWasMade.From.Column - 1; i <= i_MoveThatWasMade.To.Row && j >= i_MoveThatWasMade.To.Column; i++, j--)
                    {
                        m_Board[i, j] = i_PlayerCoinColor;
                        m_CoinFliped.Invoke(i, j, i_PlayerCoinColor);
                    }

                    break;
            }
        }
    }
}
