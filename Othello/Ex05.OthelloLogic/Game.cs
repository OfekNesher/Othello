namespace Ex05.OthelloLogic
{
    public delegate void GameOverEventHandler();

    public class Game
    {
        private const bool v_ComputerPlayer = true;
        private const string k_ComputerName = "Computer";
        private GameTypeEnum.eGameType m_GameType;
        private Board m_GameBoard;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private Player m_NextPlayer;

        public event GameOverEventHandler m_GameOver;

        public static bool IsValidGameType(GameTypeEnum.eGameType i_GameType)
        {
            return (i_GameType == GameTypeEnum.eGameType.PlayerVsPlayer) || (i_GameType == GameTypeEnum.eGameType.PlayerVsComputer);
        }

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        public Player CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }

            set
            {
                m_CurrentPlayer = value;
            }
        }

        public Player NextPlayer
        {
            get
            {
                return m_NextPlayer;
            }

            set
            {
                m_NextPlayer = value;
            }
        }

        public Game(string i_NameOfPlayer1, string i_NameOfPlayer2, GameTypeEnum.eGameType i_GameType, int i_GameBoardSize)
        {
            m_GameType = i_GameType;
            m_GameBoard = new Board(i_GameBoardSize);
            m_Player1 = new Player(i_NameOfPlayer1, !v_ComputerPlayer, CoinStatusEnum.eCoinStatus.Red, m_GameBoard);

            if (i_GameType == GameTypeEnum.eGameType.PlayerVsPlayer)
            {
                m_Player2 = new Player(i_NameOfPlayer2, !v_ComputerPlayer, CoinStatusEnum.eCoinStatus.Yellow, m_GameBoard);
            }
            else
            {
                m_Player2 = new Player(i_NameOfPlayer2, v_ComputerPlayer, CoinStatusEnum.eCoinStatus.Yellow, m_GameBoard);
            }

            m_CurrentPlayer = m_Player1;
            m_NextPlayer = m_Player2;
        }

        public void PlayOnePlayerTurn(Square i_Destination)
        {
            m_CurrentPlayer.MakeMove(i_Destination);
            updateAvailableMoves();
            swapActivePlayer(ref m_CurrentPlayer, ref m_NextPlayer);
            if (IsGameOver())
            {
                m_GameOver.Invoke();
            }
        }

        public void PlayOneComputerTurn()
        {
            m_CurrentPlayer.MakeComputerMove();
            updateAvailableMoves();
            swapActivePlayer(ref m_CurrentPlayer, ref m_NextPlayer);
            if (IsGameOver())
            {
                m_GameOver.Invoke();
            }
        }

        public void SwapPlayers()
        {
            swapActivePlayer(ref m_CurrentPlayer, ref m_NextPlayer);
        }

        private void swapActivePlayer(ref Player io_ActivePlayer, ref Player io_NextPlayer)
        {
            Player tempPlayer;
            tempPlayer = io_ActivePlayer;
            io_ActivePlayer = io_NextPlayer;
            io_NextPlayer = tempPlayer;
        }

        private void updateAvailableMoves()
        {
            m_Player1.UpdateAvailableMoves();
            m_Player2.UpdateAvailableMoves();
        }

        public void PlayAnotherRound()
        {
            m_GameBoard.InitializeBoard();
            m_Player1.InitializeAvailableMoves();
            m_Player2.InitializeAvailableMoves();
            m_Player1.Score = 0;
            m_Player2.Score = 0;
            m_CurrentPlayer = m_Player1;
            m_NextPlayer = m_Player2;
        }

        public bool IsADraw()
        {
            return m_Player1.Score == m_Player2.Score;
        }

        public void GetWinnerAndLoser(out Player o_Winner, out Player o_Loser)
        {
            if (m_Player1.Score > m_Player2.Score)
            {
                o_Winner = m_Player1;
                o_Loser = m_Player2;
            }
            else
            {
                o_Winner = m_Player2;
                o_Loser = m_Player1;
            }
        }

        private bool IsGameOver()
        {
            return (!CurrentPlayer.HasAvailableMove()) && (!NextPlayer.HasAvailableMove());
        }
    }
}
