using __Project.Systems.ChessSystem._Pieces;
using _NueCore.Common.ReactiveUtils;

namespace __Project.Systems.ChessSystem
{
    public abstract class ChessREvents
    {
        public class PieceMovedREvent : REvent
        {
            public ChessPieceBase Piece { get; private set; }
            public PieceMovedREvent(ChessPieceBase piece)
            {
                Piece = piece;
            }
        }
    }
}