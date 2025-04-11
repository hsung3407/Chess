using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Square
{
    public int Rank;
    public int File;
}

public class ChessBoard
{
    private List<Piece> _pieces = new List<Piece>();
    public Piece[,] Board { get; private set; } = new Piece[8, 8];
    
    public bool WhiteTurn { get; private set; }

    public void MovePiece(Piece piece, Square location)
    {
        Board[location.File, location.Rank] = piece;
        Board[piece.Location.File, piece.Location.Rank] = null;
        piece.Location = location;

        WhiteTurn = !WhiteTurn;
        
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        foreach (var piece in _pieces) { piece.Updated = false; }
        
        foreach (var piece in _pieces) { piece.UpdatePath(this); }
    }
}

public abstract class Piece
{
    public bool White { get; private set; }

    public Square Location;
    public HashSet<Square> Path;
    public bool Updated;

    public void UpdatePath(ChessBoard chessBoard)
    {
        if(Updated) { return; }
        Updated = true;
    }

    protected abstract void UpdatePersonalPath(ChessBoard chessBoard);

    public void PinedPathUpdate(ChessBoard chessBoard, List<Square> pinedPath)
    {
        if(!Updated) UpdatePath(chessBoard);
        Path = Path.Where(pinedPath.Contains).ToHashSet();
    }
}