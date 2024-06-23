namespace TriangTriang.Models
{
    /// <summary>
    /// This will hold the char
    /// </summary>
    public class Piece
    {
        private char symbol;

        public Piece(char symbol)
        {
            this.symbol = symbol;
        }

        public override string ToString()
        {
            return symbol.ToString();
        }
    }
}
