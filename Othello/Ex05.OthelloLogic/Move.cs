namespace Ex05.OthelloLogic
{
    public class Move
    {
        private Square m_Source;
        private Square m_Destination;
        private MoveDirectionEnum.eMoveDirection m_Direction;

        public Square From
        {
            get
            {
                return m_Source;
            }
        }

        public Square To
        {
            get
            {
                return m_Destination;
            }
        }

        public MoveDirectionEnum.eMoveDirection Direction
        {
            get
            {
                return m_Direction;
            }
        }

        public Move(Square i_From, Square i_To, MoveDirectionEnum.eMoveDirection i_Direction)
        {
            m_Source = i_From;
            m_Destination = i_To;
            m_Direction = i_Direction;
        }
    }
}
