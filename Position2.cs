using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// cleanup/refactored version of position.cs
namespace epdTester
{
    public class Position2
    {
        /*const definitions*/
        public const string SanPiece = "PNBRQKpnbrqk";
        public const string SanCols = "abcdefgh";
        public const int W_KS = 1;
        public const int W_QS = 2;
        public const int B_KS = 4;
        public const int B_QS = 8;
        public const int WHITE = 0;
        public const int BLACK = 1;
        public const int COLOR_NONE = 2;
        public static string StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private List<List<Info>> Positions = new List<List<Info>>(); // stores position history [idx][position info]
        private Info info = new Info();
        public bool valid = false;

        public static string[] SanSquares =
            { "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1",
              "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
              "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
              "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
              "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
              "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
              "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
              "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8" };
        public enum Squares
        {
            A1 = 0, B1 = 1, C1 = 2, D1 = 3, E1 = 4, F1 = 5, G1 = 6, H1 = 7,
            A2 = 8, B2 = 9, C2 = 10, D2 = 11, E2 = 12, F2 = 13, G2 = 14, H2 = 15,
            A3 = 16, B3 = 17, C3 = 18, D3 = 19, E3 = 20, F3 = 21, G3 = 22, H3 = 23,
            A4 = 24, B4 = 25, C4 = 26, D4 = 27, E4 = 28, F4 = 29, G4 = 30, H4 = 31,
            A5 = 32, B5 = 33, C5 = 34, D5 = 35, E5 = 36, F5 = 37, G5 = 38, H5 = 39,
            A6 = 40, B6 = 41, C6 = 42, D6 = 43, E6 = 44, F6 = 45, G6 = 46, H6 = 47,
            A7 = 48, B7 = 49, C7 = 50, D7 = 51, E7 = 52, F7 = 53, G7 = 54, H7 = 55,
            A8 = 56, B8 = 57, C8 = 58, D8 = 59, E8 = 60, F8 = 61, G8 = 62, H8 = 63, SQ_NONE = 64
        };
        public enum Pieces
        {
            W_PAWN = 0, W_KNIGHT = 1, W_BISHOP = 2,
            W_ROOK = 3, W_QUEEN = 4, W_KING = 5,
            B_PAWN = 6, B_KNIGHT = 7, B_BISHOP = 8,
            B_ROOK = 9, B_QUEEN = 10, B_KING = 11
        };
        public enum Piece
        {
            PAWN = 0, KNIGHT = 1, BISHOP = 2, ROOK = 3, QUEEN = 4, KING = 5, PIECE_NONE = 6
        };
        public enum MoveType
        {
            MOVE_NONE = 0, PROMOTION = 1, PROMOTION_CAP = 5, CASTLE_KS = 9, CASTLE_QS = 10, QUIET = 11, CAPTURE = 12, EP = 13
        };
        /* move encoding */
        // bits 0-5        from
        // bits 6-11       to
        // bits 12-15      move data        

        /*upper 4 bits (move data) values*/
        // 0 - error            8  - pc(quen) 
        // 1 - pp(nite)         9  - castle ks
        // 2 - pp(bish)         10 - castle qs
        // 3 - pp(rook)         11 - quiet move
        // 4 - pp(quen)         12 - capture move
        // 5 - pc(nite)         13 - 
        // 6 - pc(bish)         14 - 
        // 7 - pc(rook)         15 - 

        /*necessary position state information*/
        public class Info
        {   
            public int stm = WHITE; // side to move
            public int cr = 0; // castle rights encoding
            public int ep_sq = 0;
            public int halfmvs = 0;
            public int move50 = 0;
            public UInt16 move = 0; // encoded move
            public int captured_piece = -1;
            public int promoted_piece = -1;
            public int[] color_on = null;
            public int[] piece_on = null;
            public int[] king_sqs = null;
            public int[] piece_diffs = null;
            public List<List<List<int>>> PieceSquares = null; // [color][piece][square]
            public void clear()
            {
                stm = WHITE;
                cr = move50 = ep_sq = halfmvs = move = 0;
                captured_piece = promoted_piece = -1;
                if (PieceSquares == null)
                {
                    PieceSquares = new List<List<List<int>>>();
                    PieceSquares.Add(new List<List<int>>()); // white pieces
                    PieceSquares.Add(new List<List<int>>()); // black pieces
                    for (int p = 0; p < 6; ++p)
                    {
                        PieceSquares[WHITE].Add(new List<int>()); // sqs for each piecetype
                        PieceSquares[BLACK].Add(new List<int>());
                    }
                }
                if (color_on == null) color_on = new int[64];
                if (piece_on == null) piece_on = new int[64];
                if (king_sqs == null) king_sqs = new int[64];
                if (piece_diffs == null) piece_diffs = new int[64];
                for (int i = 0; i < 64; ++i)
                {
                    color_on[i] = COLOR_NONE;
                    piece_on[i] = (int)Piece.PIECE_NONE;
                    king_sqs[i] = (int)Squares.SQ_NONE;
                }
                for (int i = 0; i < 6; ++i) piece_diffs[i] = 0;
            }
            public void EncodeMove(int from, int to, MoveType mt)
            {
                move = (UInt16)(from | (to << 6) | ((int)mt << 12));
            }
            public int moveType()
            {
                return (move & 0xf000) >> 12;
            }
            public int From()
            {
                return move & 0x3f;
            }
            public int To()
            {
                return (move & 0xfc0) >> 6;
            }
            public bool isCapture()
            {
                int mt = moveType();
                return mt == (int)MoveType.CAPTURE || 
                    (mt >= (int)MoveType.PROMOTION_CAP && mt < (int)MoveType.CASTLE_KS) || mt == (int)MoveType.EP;
            }
            public bool isPromotion()
            {
                int mt = moveType();
                return (mt >= (int)MoveType.PROMOTION_CAP && mt < (int)MoveType.CASTLE_KS) ||
                    (mt >= (int)MoveType.PROMOTION && mt < (int)MoveType.PROMOTION_CAP);
            }
        }
        public Position2() { init(); }
        public Position2(string fen)
        {
            valid = Load(fen);
        }
        private void init()
        {
            if (info == null) info = new Info();
            info.clear();
        }
        public bool Load(string fen)
        {
            init();
            int s = (int)Squares.A8;
            string[] split_fen = fen.Split(' ');
            if (split_fen.Length <= 0) return false;
            for (int i = 0; i < split_fen[0].Length; ++i)
            {
                char c = fen[i];
                if ((c >= '0' && c <= '9')) s += (c - '0');
                else // c is not a digit ..
                {
                    switch (c)
                    {
                        case '/':
                            s -= 16;
                            break;
                        default:
                            SetPiece(c, s);
                            ++s;
                            break;
                    }
                }
            }
            // side to move
            if (split_fen.Length <= 1) return true;
            else info.stm = (split_fen[1][0] == 'w' ? WHITE : BLACK);

            // castle rights
            if (split_fen.Length <= 2) return true;
            else
            {
                for (int i = 0; i < split_fen[2].Length; ++i)
                {
                    char c = split_fen[2][i];
                    switch (c)
                    {
                        case 'K': info.cr |= W_KS; break;
                        case 'Q': info.cr |= W_QS; break;
                        case 'k': info.cr |= B_KS; break;
                        case 'q': info.cr |= B_QS; break;
                    }
                }
            }
            // en-passant square
            if (split_fen.Length <= 3) return true;
            else
            {
                int col = 0;
                int row = 0;
                char c = split_fen[3][0];
                if (c != '-')
                {
                    for (int j = 0; j < SanCols.Length; ++j)
                    {
                        if (SanCols[j] == c) col = j;
                    }
                    c = (split_fen[3][1]);
                    row = (c - '0') - 1;
                    info.ep_sq = 8 * row + col;
                }
                else info.ep_sq = 0;
            }
            // the half-moves since last pawn move/capture
            if (split_fen.Length <= 4) return true;
            else if (!String.IsNullOrWhiteSpace(split_fen[4])) info.move50 = (int)Convert.ToInt32(split_fen[4]);
            
            // the move counter
            if (split_fen.Length <= 5) return true;
            else info.halfmvs = (int)Convert.ToInt32(split_fen[5]);
            return UpdateHistory(); 
        }
        private bool UpdateHistory()
        {
            if (info == null) return false;
            // add this position to the position tracking
            if (Positions == null) Positions = new List<List<Info>>();
            Positions.Add(new List<Info>());
            Positions[Positions.Count - 1].Add(info);
            return true;
        }
        public void ClearHistory()
        {
            Positions.Clear();
        }
        private bool onBoard(int s)
        {
            return s >= 0 && s < 64;
        }
        private void SetPiece(char c, int s)
        {
            for (int p = (int)Pieces.W_PAWN; p <= (int)Pieces.B_KING; ++p)
            {
                if (c != SanPiece[p]) continue;
                int color = (p < 6 ? WHITE : BLACK);
                int piece = (p > (int)Pieces.W_KING ? p - 6 : p);
                info.PieceSquares[color][piece].Add(s);
                info.color_on[s] = color;
                info.piece_on[s] = piece;
                if (p == (int)Pieces.W_KING || p == (int)Pieces.B_KING) info.king_sqs[color] = s;
                else info.piece_diffs[piece] += (color == WHITE ? 1 : -1);
            }
        }
        public int PieceOn(int s)
        {
            if (!onBoard(s) || info == null) return (int)Piece.PIECE_NONE;
            return info.piece_on[s];
        }
        public int ColorOn(int s)
        {
            if (!onBoard(s) || info == null) return (int)COLOR_NONE;
            return info.color_on[s];
        }
        public int EnemyColor()
        {
            return (info != null ? (info.stm == WHITE ? BLACK : WHITE) : COLOR_NONE);
        }
        public bool Empty(int s)
        {
            return PieceOn(s) != (int)Piece.PIECE_NONE;
        }
        int RowOf(int from)
        {
            return (from >> 3);
        }
        int ColOf(int from)
        {
            return (from & 7);
        }
        bool EnemyOn(int s)
        {
            return ColorOn(s) == EnemyColor();
        }
        int ColDist(int s1, int s2)
        {
            return Math.Abs(ColOf(s1) - ColOf(s2));
        }
        int RowDist(int s1, int s2)
        {
            return Math.Abs(RowOf(s1) - RowOf(s2));
        }
        bool SameCol(int s1, int s2)
        {
            return ColDist(s1, s2) == 0;
        }
        bool SameRow(int s1, int s2)
        {
            return RowDist(s1, s2) == 0;
        }
        bool SameDiag(int s1, int s2)
        {
            return ColDist(s1, s2) == RowDist(s1, s2);
        }
        private bool pseudoLegalPawnMove(int from, int to, int color)
        {
            int enemy = EnemyColor();
            int forward1 = (color == WHITE ? 8 : -8);
            int forward2 = (color == WHITE ? 16 : -16);
            int capRight = (color == WHITE ? 7 : -7);
            int capLeft = (color == WHITE ? 9 : -9);
            bool isCapture = ((from + capRight) == to || (from + capLeft) == to) && EnemyOn(to);
            bool on4 = (color == WHITE ? (RowOf(from) == 4) : (RowOf(from) == 3));
            bool on7 = (color == WHITE ? (RowOf(from) == 6) : (RowOf(from) == 1));
            bool on2 = (color == WHITE ? (RowOf(from) == 1) : (RowOf(from) == 6));
            bool isOK = false;
            MoveType mt = MoveType.MOVE_NONE;
            if (on2)
            {
                if ((from + forward1) == to && Empty(to)) { isOK = true; mt = MoveType.QUIET; }
                else if ((from + forward2) == to && Empty(to) && Empty(from + forward1)) { isOK = true; mt = MoveType.QUIET; }
                else if (isCapture && ColDist(from, to) == 1 && onBoard(to)) { isOK = true; mt = MoveType.CAPTURE; }
            }
            else if (on4 && info.ep_sq == to) // EP move?
            {
                int epTo = (color == WHITE ? to - 8 : to + 8);
                if (((from + capRight) == to  || (from + capLeft) == to) &&  EnemyOn(epTo) &&
                    ColDist(from, to) == 1 && onBoard(to))  { isOK = true; mt = MoveType.CAPTURE; }
            }
            else if (on7)
            {
                if ((from + forward1) == to && Empty(to)) { isOK = true; mt = MoveType.PROMOTION; }
                else if (isCapture && ColDist(from, to) == 1 && onBoard(to)) { isOK = true; mt = MoveType.PROMOTION; }
            }
            else
            {
                if ((from + forward1) == to && Empty(to)) { isOK = true; mt = MoveType.QUIET; } 
                else if (isCapture && ColDist(from, to) == 1 && onBoard(to)) { isOK = true; mt = MoveType.CAPTURE; }
            }
            info.EncodeMove(from, to, mt); // encode move no matter what?
            return isOK;
        }
        private bool pseudoLegalKnightMove(int from, int to, int color)
        {
            if ((!Empty(to) && !EnemyOn(to)) || !onBoard(to)) return false;
            if ((ColDist(from, to) != 2 && ColDist(from, to) != 1)) return false;
            if ((RowDist(from, to) != 2 && RowDist(from, to) != 1)) return false;
            if (Empty(to) && !EnemyOn(to)) { info.EncodeMove(from, to, MoveType.QUIET); return true; }
            else if (!Empty(to) && EnemyOn(to)) { info.EncodeMove(from, to, MoveType.CAPTURE); return true; }
            return false;
        }
        private bool pseudoLegalBishopMove(int from, int to, int color)
        {
            int[] deltas = { 7, 9, -7, -9 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && SameDiag(from, t) && (Empty(t) || EnemyOn(t)))
                {
                    if (!Empty(t) && t != to) break;
                    else if (Empty(t) && t == to)
                    {
                        info.EncodeMove(from, to, MoveType.QUIET);
                        return true;
                    }
                    else if (EnemyOn(t) && t == to)
                    {
                        info.EncodeMove(from, to, MoveType.CAPTURE);
                        return true;
                    }
                    t = from + d * (++c);
                }
            }
            return false;
        }
        private bool pseudoLegalRookMove(int from, int to, int color)
        {
            int[] deltas = { 1, -1, 8, -8 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && (SameRow(from, t) || SameCol(from, t)) && (Empty(t) || EnemyOn(t)))
                {
                    if (!Empty(t) && t != to) break;
                    else if (t == to && Empty(t)) { info.EncodeMove(from, to, MoveType.QUIET); return true; }
                    else if (EnemyOn(t) && t == to)
                    {
                        info.EncodeMove(from, to, MoveType.QUIET);
                        return true;
                    }
                    t = from + d * (++c);
                }
            }
            return false;
        }
        private bool pseudoLegalQueenMove(int from, int to, int color)
        {
            if (SameDiag(from, to)) return pseudoLegalBishopMove(from, to, color);
            else if (SameRow(from, to) || SameCol(from, to)) return pseudoLegalRookMove(from, to, color);
            return false;
        }
        private bool pseudoLegalKingMove(int from, int to, int color)
        {
            if (ColDist(from, to) != 1 || ColDist(from, to) != 1) return false;
            if ((!Empty(to) && !EnemyOn(to))) return false;
            if (!onBoard(to)) return false;
            if (Empty(to)) { info.EncodeMove(from, to, MoveType.QUIET); return true; }
            else if (EnemyOn(to)) { info.EncodeMove(from, to, MoveType.CAPTURE); return true; }
            return false;
        }
    }

}
