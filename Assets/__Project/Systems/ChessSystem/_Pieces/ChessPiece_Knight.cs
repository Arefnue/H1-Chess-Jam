using UnityEngine;

namespace __Project.Systems.ChessSystem._Pieces
{
    public class ChessPiece_Knight : ChessPieceBase
    {
        public override void Move(Vector3Int newPosition)
        {
            // Knights move in an L-shape: two squares in one direction and then one square perpendicular
            Vector3Int[] possibleMoves = new Vector3Int[]
            {
                new Vector3Int(CurrentPosition.x + 2, CurrentPosition.y + 1),
                new Vector3Int(CurrentPosition.x + 2, CurrentPosition.y - 1),
                new Vector3Int(CurrentPosition.x - 2, CurrentPosition.y + 1),
                new Vector3Int(CurrentPosition.x - 2, CurrentPosition.y - 1),
                new Vector3Int(CurrentPosition.x + 1, CurrentPosition.y + 2),
                new Vector3Int(CurrentPosition.x + 1, CurrentPosition.y - 2),
                new Vector3Int(CurrentPosition.x - 1, CurrentPosition.y + 2),
                new Vector3Int(CurrentPosition.x - 1, CurrentPosition.y - 2)
            };

            foreach (var move in possibleMoves)
            {
                if (move.x == newPosition.x && move.y == newPosition.y)
                {
                    ChangePosition(newPosition);
                    return;
                }
            }

            Debug.LogError("Invalid move for Knight");
        }
    }
}