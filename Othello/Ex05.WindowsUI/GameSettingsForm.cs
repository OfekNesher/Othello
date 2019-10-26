using System;
using System.Windows.Forms;
using Ex05.OthelloLogic;

namespace Ex05.WindowsUI
{
    public partial class GameSettingsForm : Form
    {
        private const int k_MinBoardSize = 6;
        private const int k_MaxBoardSize = 12;
        private int m_BoardSize = k_MinBoardSize;
        private GameTypeEnum.eGameType m_GameType;

        public int BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }

        public GameTypeEnum.eGameType GameType
        {
            get
            {
                return m_GameType;
            }
        }

        public GameSettingsForm()
        {
            InitializeComponent();
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            if (m_BoardSize < k_MaxBoardSize)
            {
                m_BoardSize += 2;
            }
            else
            {
                m_BoardSize = k_MinBoardSize;
            }

            buttonBoardSize.Text = string.Format("Board Size: {0}x{0} (click to increase)", m_BoardSize);
        }

        private void buttonPlayVsComputer_Click(object sender, EventArgs e)
        {
            m_GameType = GameTypeEnum.eGameType.PlayerVsComputer;
            GameBoardForm gameBoardForm = new GameBoardForm(m_BoardSize, m_GameType);
            Hide();
            gameBoardForm.ShowDialog();
        }

        private void buttonPlayVsFriend_Click(object sender, EventArgs e)
        {
            m_GameType = GameTypeEnum.eGameType.PlayerVsPlayer;
            GameBoardForm gameBoardForm = new GameBoardForm(m_BoardSize, m_GameType);
            Hide();
            gameBoardForm.ShowDialog();
        }
    }
}
