using __Project.Systems.ChessSystem._Pieces;
using _NueCore.Common.ReactiveUtils;

namespace __Project.Systems.ChessSystem
{
    public abstract class ChessREvents
    {
        public class PieceMoveStartedREvent : REvent
        {
            public ChessPieceBase Piece { get; private set; }
            public PieceMoveStartedREvent(ChessPieceBase piece)
            {
                Piece = piece;
            }
        }
        public class PieceMoveFinishedREvent : REvent
        {
            public ChessPieceBase Piece { get; private set; }
            public PieceMoveFinishedREvent(ChessPieceBase piece)
            {
                Piece = piece;
            }
        }
        
        public class AllPiecesFinishedREvent : REvent
        {
            
        }
    }
}