namespace Ex05.OthelloLogic
{
    public class Square
    {
        private int m_Row;
        private int m_Column;

        public static bool AreEqualSquares(Square i_Square1, Square i_Square2)
        {
            return (i_Square1.Row == i_Square2.Row) && (i_Square1.Column == i_Square2.Column);
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }

        public int Column
        {
            get
            {
                return m_Column;
            }
        }

        public Square(int i_Row, int i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column;
        }

        public Square(int i_Row, char i_Column)
        {
            m_Row = i_Row;
            m_Column = i_Column - 'A' + 1;
        }
    }
}