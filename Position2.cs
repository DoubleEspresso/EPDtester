using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        public ChessGame Game = null;
        private Info info = new Info();
        public bool valid = false;
        public int displayIdx = 0;
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
        // 5 - pc(nite)         13 - ep capture
        // 6 - pc(bish)         14 - 
        // 7 - pc(rook)         15 - 

        /*necessary position state information*/
        public class Info
        {
            //public string fen_tmp = "";
            public int stm = WHITE; // side to move
            public int cr = 0; // castle rights encoding
            public int ep_sq = 0;
            public int halfmvs = 0;
            public int move50 = 0;
            public UInt16 move = 0; // encoded move
            public int captured_piece = -1;
            public int promoted_piece = -1;
            public int moving_piece = -1;
            public int[] color_on = new int[64];
            public int[] piece_on = new int[64];
            public int[] king_sqs = new int[64];
            public int[] piece_diffs = new int[64];
            public List<List<List<int>>> PieceSquares = null; // [color][piece][square]
            public Info() { }
            public Info(Info src)
            {
                //fen_tmp = src.fen_tmp;
                stm = src.stm;
                cr = src.cr;
                ep_sq = src.ep_sq;
                halfmvs = src.halfmvs;
                move50 = src.move50;
                move = src.move;
                captured_piece = src.captured_piece;
                promoted_piece = src.promoted_piece;
                moving_piece = src.moving_piece;
                for (int i=0; i<64; ++i)
                {
                    color_on[i] = src.color_on[i]; piece_on[i] = src.piece_on[i];
                    king_sqs[i] = src.king_sqs[i]; piece_diffs[i] = src.piece_diffs[i];
                }
                if (PieceSquares == null) PieceSquares = new List<List<List<int>>>();
                PieceSquares.Clear();
                for (int c = 0; c < 2; ++c)
                {
                    PieceSquares.Add(new List<List<int>>());
                    for (int p = 0; p < 6; ++p)
                    {
                        PieceSquares[c].Add(new List<int>());
                        for (int s = 0; s < src.PieceSquares[c][p].Count; ++s)
                        {
                            PieceSquares[c][p].Add(src.PieceSquares[c][p][s]);
                        }
                        
                    }
                }
            }
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
            public bool isEP()
            {
                return moveType() == (int)MoveType.EP;
            }
            public bool isCastle()
            {
                int mt = moveType();
                return (mt == (int)MoveType.CASTLE_KS || mt == (int)MoveType.CASTLE_QS);
            }
            public bool update()
            {
                ++halfmvs;
                if (moving_piece != (int)Piece.PAWN && !isCapture()) ++move50;
                /*update castle rights*/
                if (isCastle()) clearAllCastleRights(stm);
                if (moving_piece == (int)Piece.KING) clearAllCastleRights(stm);
                if (moving_piece == (int)Piece.ROOK && From() == (int)Squares.A1 && stm == WHITE) clearCastleRights(W_QS);
                if (moving_piece == (int)Piece.ROOK && From() == (int)Squares.H1 && stm == WHITE) clearCastleRights(W_KS);
                if (moving_piece == (int)Piece.ROOK && From() == (int)Squares.A8 && stm == BLACK) clearCastleRights(B_QS);
                if (moving_piece == (int)Piece.ROOK && From() == (int)Squares.H8 && stm == BLACK) clearCastleRights(B_KS);
                // set ep square
                if (moving_piece == (int)Piece.PAWN && Math.Abs(From() - To()) == 16) ep_sq = (stm == WHITE ? To() - 8 : To() + 8);
                stm ^= 1;
                return true;
            }
            public void clearAllCastleRights(int color)
            {
                if (color == WHITE) cr &= 12;
                else cr &= 3;
            }
            public void clearCastleRights(int type)
            {
                cr = (cr & (~type));
            }
        }
        /*node/chessgame classes to track move histories/variations*/
        public class Node
        {
            public Node parent = null;
            Position2.Info pos = null;
            string san_move = "";
            public bool hasSiblings = false;
            public List<Node> children = null;
            public Node() { }
            public Node(Info i, string san) // for inserting nodes into existing list
            {
                parent = new Node(); 
                pos = i;
                san_move = san;
                if (children == null) children = new List<Node>();
                children.Clear();
            }
            public bool hasChildren() { return children != null && children.Count > 0; }
            public bool hasParent() { return parent != null; }
            public bool hasParentPosition() { return hasParent() && parent.pos != null; }
            public bool hasPosition() { return pos != null; }
            public bool isValid()
            {
                return (parent != null && pos != null && children != null);
            }
            public string SanMove() { return san_move; }
            public Info position() { return pos; }
        }
        public class ChessGame
        {
            Node current = null;
            public ChessGame(Position2.Info startpos)
            {
                current = new Node(startpos, ""); 
            }
            public void insert(Node n)
            {
                // note : two cases to consider
                // 1. we have added a variation to an existing position
                // 2. this is a new move
                n.parent = current;
                current.children.Add(n);
                if (hasChildren()) foreach (Node c in current.children) c.hasSiblings = true;
                current = n; // if this is a new move, it is made current by default
            }
            public void next()
            {
                if (current.children == null || current.children.Count == 0) return;
                current = current.children[0]; // default
            }
            public void previous()
            {
                if (current.parent == null || !current.parent.hasPosition()) return;
                current = current.parent;
            }
            public void selectSibling(int idx)
            {
                if (idx < 0 || current.parent == null ||
                        current.parent.children == null ||
                        idx > current.parent.children.Count)
                    return;
                current = current.parent.children[idx];
            }
            public int MoveIndex()
            {
                int count = 0;
                Node dummy = current;
                while (dummy != null && dummy.hasParent())
                {
                    dummy = dummy.parent; ++count;
                }
                return (int) Math.Floor((double)count/2);
            }
            public string SanMove()
            {
                return current.SanMove();
            }
            public bool hasChildren()
            {
                return current.children != null && current.children.Count > 1;
            }
            public bool hasSiblings()
            {
                return current.parent.children != null && current.parent.children.Count > 1;
            }
            public Info position() { return current.position(); }
            public string Moves()
            {
                int count = 0; string g_mvs = "";
                Node dummy = (current.hasChildren() ? current.children[0] : current);
                while (dummy != null && dummy.parent.parent != null)
                {
                    dummy = dummy.parent; ++count;
                }
                for (int j = 0; j < count; ++j)
                {
                    dummy = (dummy.children.Count > 1 ? dummy.children[1] : dummy.children[0]); // default selection for now
                    g_mvs += (j % 2 == 0 ? " " + Convert.ToString((int)Math.Floor((double)(j + 1) / 2) + 1) + "." : " ");
                    g_mvs += (dummy.hasSiblings ? "[" + dummy.SanMove() + "]" : dummy.SanMove());
                }
                return g_mvs;
            }
        }
        public Position2() { init(); }
        public Position2(string fen)
        {
            valid = Load(fen); // note : load calls udpateHistory()
        }
        private void init()
        {
            if (info == null) info = new Info();
            info.clear();
        }
        public int ToMove() { return info.stm; }

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
            return UpdatePosition(); 
        }
        public bool UpdatePosition()
        {
            if (info == null) return false;
            if (Game == null)
            {
                Game = new ChessGame(new Info(info));
                return true;
            }
            string san = toSan(SanSquares[info.From()] + SanSquares[info.To()]);
            Game.insert(new Node(new Info(info), san)); // new move
            return true;
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
        public List<int> PieceSquares(int color, int piece)
        {
            return info.PieceSquares[color][piece];
        }
        public int EnemyColor()
        {
            return (info != null ? (info.stm == WHITE ? BLACK : WHITE) : COLOR_NONE);
        }
        public bool Empty(int s)
        {
            return PieceOn(s) == (int)Piece.PIECE_NONE;
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
        public bool isPromotion() { return info.isPromotion(); }
        public int MaxDisplayIdx() { return Positions.Count - 1; }
        private List<int> PawnMoves(int from, int color)
        {
            List<int> to_sqs = new List<int>();
            int forward1 = (color == WHITE ? 8 : -8);
            int forward2 = (color == WHITE ? 16 : -16);
            int capRight = (color == WHITE ? 7 : -7);
            int capLeft = (color == WHITE ? 9 : -9);
            int to = -1; int enemy = color ^ 1;
            bool on4 = (color == WHITE ? (RowOf(from) == 4) : (RowOf(from) == 3));
            bool on2 = (color == WHITE ? (RowOf(from) == 1) : (RowOf(from) == 6));
            if (on2)
            {
                to = (from + forward1);
                if (Empty(to)) to_sqs.Add(to);

                to = (from + forward2);
                if (to_sqs.Count > 0 && Empty(to)) to_sqs.Add(to);

                to = (from + capRight);
                if (ColorOn(to) == enemy && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (ColorOn(to) == enemy && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            else if (on4 && info.ep_sq == to) // EP move?
            {
                int epTo = (color == WHITE ? to - 8 : to + 8);
                to = (from + capRight);
                if (enemy == ColorOn(epTo) && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (enemy == ColorOn(epTo) && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            else
            {
                // same condition for normal pawn moves or promotion moves - this method is meant to check if a pawn move exists, it doesn't 
                // return the exact number of pawn moves.
                to = (from + forward1);
                if (Empty(to)) to_sqs.Add(to);

                to = (from + capRight);
                if (enemy == ColorOn(to) && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (enemy == ColorOn(to) && ColDist(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            return to_sqs;
        }
        private List<int> KnightMoves(int from, int color)
        {
            int enemy = color ^ 1;
            List<int> to_sqs = new List<int>();
            int[] deltas = { 16 - 1, 16 + 1, 2 + 8, 2 - 8, -16 + 1, -16 - 1, -2 - 8, -2 + 8 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int t = deltas[j] + from; bool enemyon = (ColorOn(t) == enemy);
                if (!onBoard(t) || (!Empty(t) && !enemyon)) continue;
                else if (onBoard(t) && Empty(t) && !enemyon) to_sqs.Add(t);
                else if (onBoard(t) && !Empty(t) && enemyon) to_sqs.Add(t);
            }
            return to_sqs;
        }
        private List<int> BishopMoves(int from, int color)
        {
            int enemy = color ^ 1;
            List<int> to_sqs = new List<int>();
            int[] deltas = { 7, 9, -7, -9 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && SameDiag(from, t) && (Empty(t) || ColorOn(t) == enemy))
                {
                    if (!Empty(t) && ColorOn(t) != enemy) break;
                    else if (Empty(t)) to_sqs.Add(t);
                    else if (ColorOn(t) == enemy)
                    {
                        to_sqs.Add(t);
                        break;
                    }
                    t = from + d * (++c);
                }
            }
            return to_sqs;
        }
        private List<int> RookMoves(int from, int color)
        {
            int enemy = color ^ 1;
            List<int> to_sqs = new List<int>();
            int[] deltas = { 1, -1, 8, -8 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && (SameRow(from, t) || SameCol(from, t)) && (Empty(t) || ColorOn(t) == enemy))
                {
                    if (Empty(t)) to_sqs.Add(t);
                    else if (ColorOn(t) == enemy) // todo : enemyon needs a color parameter :( since we return moves for both colors in multiple legality conditions :(
                    {
                        to_sqs.Add(t);
                        break;
                    }
                    t = from + d * (++c);
                }
            }
            return to_sqs;
        }
        private List<int> QueenMoves(int from, int color)
        {
            List<int> to_sqs_b = new List<int>();
            List<int> to_sqs_r = new List<int>();
            List<int> to_sqs_q = new List<int>();
            to_sqs_b = BishopMoves(from, color);
            to_sqs_r = RookMoves(from, color);
            for (int j = 0; j < to_sqs_b.Count; ++j) to_sqs_q.Add(to_sqs_b[j]);
            for (int j = 0; j < to_sqs_r.Count; ++j) to_sqs_q.Add(to_sqs_r[j]);
            return to_sqs_q;
        }
        private List<int> KingMoves(int from, int color)
        {
            List<int> to_sqs = new List<int>();
            int[] deltas = { 1, -1, 7, 9, 8, -8, -7, -9 };
            for (int j = 0; j < deltas.Length; ++j)
            {
                int t = deltas[j] + from;
                if (!onBoard(t) || (!Empty(t) && !EnemyOn(t))) continue;
                else if (onBoard(t) && Empty(t) && !EnemyOn(t)) to_sqs.Add(t);
                else if (onBoard(t) && !Empty(t) && EnemyOn(t)) to_sqs.Add(t);
            }
            return to_sqs;
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
                        info.EncodeMove(from, to, MoveType.CAPTURE);
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
            if (ColDist(from, to) != 1 && RowDist(from, to) != 1) return false;
            if ((!Empty(to) && !EnemyOn(to))) return false;
            if (!onBoard(to)) return false;
            if (Empty(to)) { info.EncodeMove(from, to, MoveType.QUIET); return true; }
            else if (EnemyOn(to)) { info.EncodeMove(from, to, MoveType.CAPTURE); return true; }
            return false;
        }
        private bool isCastle(int from, int to, int piece, int color)
        {
            if (piece != (int)Piece.KING) return false;
            if (PieceOn(from) != (int)Piece.KING) return false;
            if (from != (color == WHITE ? (int)Squares.E1 : (int)Squares.E8)) return false;
            if (color == WHITE && (to != (int)Squares.G1 && to != (int)Squares.C1)) return false;
            if (color == BLACK && (to != (int)Squares.G8 && to != (int)Squares.C8)) return false;
            return true;
        }
        public bool hasCastleRights(int to, int color)
        {
            bool ks_ok = (color == WHITE ? ((info.cr & 1) == 1) : ((info.cr & 4) == 4));
            bool qs_ok = (color == WHITE ? (info.cr & 2) == 2 : (info.cr & 8) == 8);
            if (color == WHITE)
            {
                if (to == (int)Squares.G1 && ks_ok) return true;
                else if (to == (int)Squares.C1 && qs_ok) return true;
            }
            else
            {
                if (to == (int)Squares.G8 && ks_ok) return true;
                else if (to == (int)Squares.C8 && qs_ok) return true;
            }
            return false;
        }
        private bool isCastleLegal(int from, int to, int piece, int color)
        {
            if (!hasCastleRights(to, color)) return false;
            int sqLeft1 = (color == WHITE ? (int)Squares.D1 : (int)Squares.D8);
            int sqLeft2 = (color == WHITE ? (int)Squares.C1 : (int)Squares.C8);
            int sqRight1 = (color == WHITE ? (int)Squares.F1 : (int)Squares.F8);
            int sqRight2 = (color == WHITE ? (int)Squares.G1 : (int)Squares.G8);
            if (to == sqRight2)
            {
                if (!Empty(sqRight1) && !Empty(sqRight2)) return false;
                if (isAttacked(sqRight1, color) || isAttacked(sqRight2, color)) return false;
            }
            else
            {
                if (!Empty(sqLeft1) && !Empty(sqLeft2)) return false;
                if (isAttacked(sqLeft1, color) || isAttacked(sqLeft2, color)) return false;
            }
            return !kingInCheck(color);
        }
        private bool isPseudoLegal(int from, int to, int piece, int color)
        {
            if (ColorOn(to) == color) return false;
            switch (piece)
            {
                case 0: return pseudoLegalPawnMove(from, to, color);
                case 1: return pseudoLegalKnightMove(from, to, color);
                case 2: return pseudoLegalBishopMove(from, to, color);
                case 3: return pseudoLegalRookMove(from, to, color);
                case 4: return pseudoLegalQueenMove(from, to, color);
                case 5: return pseudoLegalKingMove(from, to, color);
            }
            return false;
        }
        public bool isLegal(int from, int to, int piece, int color)
        {
            /*basic sanity checks*/
            if (color != info.stm || !onBoard(from) || !onBoard(to)) return false;
            if (PieceOn(from) == (int)Piece.PIECE_NONE) return false;
            if (isCastle(from, to, piece, color)) // check if castle move before pseudo-legal (king is moving two sqs for castling).
            {
                MoveType mt = (to == (int)Squares.G1 || to == (int)Squares.G8) ? MoveType.CASTLE_KS : MoveType.CASTLE_QS;
                info.EncodeMove(from, to, mt);
                return isCastleLegal(from, to, piece, color);
            }
            /* note: move is now encoded & stored in info class */
            if (!isPseudoLegal(from, to, piece, color)) return false;
            /*does the move leave the king in check*/
            // note : we have to "make" a pseudo-move and see if the king is attacked
            // then unmake the pseudo-move (without editing any state variables)
            movePiece(from, to, piece, color);
            bool incheck = kingInCheck(color);
            movePiece(to, from, piece, color);
            return !incheck;
        }
        public string toFen()
        {
            String fen = "";
            for (int r = 7; r >= 0; --r)
            {
                int empties = 0;
                for (int c = 0; c < 8; ++c)
                {
                    int s = r * 8 + c;
                    if (Empty(s)) { ++empties; continue; }
                    if (empties > 0)
                    {
                        fen += empties; empties = 0;
                    }
                    fen += SanPiece[(info.color_on[s] == BLACK ? info.piece_on[s] + 6 : info.piece_on[s])];
                }
                if (empties > 0)
                {
                    fen += empties;
                }
                if (r > 0) fen += "/";
            }
            fen += (info.stm == WHITE ? " w" : " b");

            string castleRights = "";
            if ((info.cr & W_KS) == W_KS) castleRights += "K";
            if ((info.cr & W_QS) == W_QS) castleRights += "Q";
            if ((info.cr & B_KS) == B_KS) castleRights += "k";
            if ((info.cr & B_QS) == B_QS) castleRights += "q";
            fen += (castleRights == "" ? " -" : " " + castleRights);

            // ep-square
            string epSq = "";
            if (info.ep_sq != 0)
            {
                epSq += SanCols[ColOf(info.ep_sq)] + Convert.ToString(RowOf(info.ep_sq) + 1);
            }
            fen += (epSq == "" ? " -" : " " + epSq);
            string mv_str = Convert.ToString(info.move50);
            fen +=  (mv_str == "" ? " -" : " " + mv_str);
            mv_str = Convert.ToString(info.halfmvs);
            fen += (mv_str == "" ? " -" : " " + mv_str);
            return fen;
        }
        private void movePiece(int from, int to, int piece, int color) // todo : fixme (captures are breaking checks)
        {
            /*update tracking info for piece*/
            if (info.PieceSquares[color][piece].Contains(from))
            {
                info.PieceSquares[color][piece].Remove(from);
                info.PieceSquares[color][piece].Add(to);
            }
            int enemy = color ^ 1;
            if (info.isCapture())
            {
                if (info.isEP()) to = (color == WHITE ? to - 8 : to + 8);
                if (info.color_on[to] == enemy) info.captured_piece = info.piece_on[to];
                if (info.PieceSquares[enemy][info.captured_piece].Contains(to)) // e.g. white captures black
                {
                    info.PieceSquares[enemy][info.captured_piece].Remove(to);
                    info.color_on[from] = COLOR_NONE;
                    info.piece_on[from] = (int)Piece.PIECE_NONE;
                }
                else
                {
                    info.PieceSquares[enemy][info.captured_piece].Add(from); // add captured piece back in this case
                    info.color_on[from] = enemy;
                    info.piece_on[from] = info.captured_piece;
                }
                if (info.isEP()) to = (color == WHITE ? to + 8 : to - 8); // reset to-square
            }
            else // quiet moves
            {
                info.color_on[from] = COLOR_NONE;
                info.piece_on[from] = (int)Piece.PIECE_NONE;
            }
            info.color_on[to] = color;
            info.piece_on[to] = piece;
            // note : castle moves are handled here as normal quiet moves of the king only
        }
        public bool isAttacked(int to, int c)
        {
            int enemy = (c == WHITE ? BLACK : WHITE);
            for (int p = 0; p <= (int)Piece.KING; ++p)
            {
                foreach (int from in info.PieceSquares[enemy][p])
                {
                    List<int> tos = new List<int>();
                    switch (p)
                    {
                        case (int)Piece.PAWN: tos = PawnMoves(from, enemy); break;
                        case (int)Piece.KNIGHT: tos = KnightMoves(from, enemy); break;
                        case (int)Piece.BISHOP: tos = BishopMoves(from, enemy); break;
                        case (int)Piece.ROOK: tos = RookMoves(from, enemy); break;
                        case (int)Piece.QUEEN: tos = QueenMoves(from, enemy); break;
                        case (int)Piece.KING: tos = KingMoves(from, enemy); break;
                        default: break;
                    }
                    if (tos.Count > 0 && tos.Contains(to)) return true;
                }
            }
            return false;
        }
        public bool kingInCheck(int c)
        {
            return isAttacked(info.PieceSquares[c][(int)Piece.KING][0], c);
        }
        public bool doMove(int from, int to, int piece, int color)
        {
            if (!isLegal(from, to, piece, color))
            {
                info.move = 0; // illegal
                return false;
            }
            // note : movePiece moves only the king for castle moves
            // and only the promoting pawn forward for promotion moves.
            movePiece(from, to, piece, color);

            // finish moving special move types
            // 1. castle - move rook only (movePiece handles king)
            // 2. promotion/promotion capture - add promoted piece to board & remove pawn
            if (info.isCastle())
            {
                if (info.moveType() == (int) MoveType.CASTLE_KS)
                {
                    int rookFrom = (to == (int)Squares.G1 ? (int)Squares.H1 : (int)Squares.H8);
                    int rookto = (to == (int)Squares.G1 ? (int)Squares.F1 : (int)Squares.F8);
                    movePiece(rookFrom, rookto, (int)Piece.ROOK, color);
                }
                else
                {
                    int rookFrom = (to == (int)Squares.C1 ? (int)Squares.A1 : (int)Squares.A8);
                    int rookto = (to == (int)Squares.C1 ? (int)Squares.D1 : (int)Squares.D8);
                    movePiece(rookFrom, rookto, (int)Piece.ROOK, color);
                }
            }
            return info.update();
        }
        public bool isMate()
        {
            return kingInCheck(info.stm) && !hasLegalMoves();
        }
        public bool setPositionFromDisplay(int relCount)
        {
            if (relCount > 0)
                for (int i = 0; i < relCount; ++i) Game.next();
            else
                for (int i = 0; i < Math.Abs(relCount); ++i) Game.previous();
            info = Game.position();
            return true;
        }
        public bool isStaleMate()
        {
            return !kingInCheck(info.stm) && !hasLegalMoves();
        }
        List<int> AllCheckersOf(int color)
        {
            // note : color is the victim color
            int enemy = info.stm ^ 1;
            int to = info.PieceSquares[color][(int)Piece.KING][0];
            List<int> checkers = new List<int>();
            for (int p = 0; p < 6; ++p)
            {
                List<int> sqs = PieceSquares(enemy, p);
                foreach (int s in sqs)
                {
                    switch (p)
                    {
                        case 0: if (PawnMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        case 1: if (KnightMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        case 2: if (BishopMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        case 3: if (RookMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        case 4: if (QueenMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        case 5: if (KingMoves(s, enemy).Contains(to)) checkers.Add(s); break;
                        default: break;
                    }
                }
            }
            return checkers;
        }
        bool hasLegalMoves() // for mate detection only
        {
            for (int p = 0; p < 6; ++p)
            {
                int [] sqs = PieceSquares(info.stm, p).ToArray(); // make a copy since PieceSquares is edited during iteration.
                foreach (int from in sqs)
                {
                    switch (p)
                    {
                        case 0:
                            List<int> tos = PawnMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        case 1:
                            tos = KnightMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        case 2:
                            tos = BishopMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        case 3:
                            tos = RookMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        case 4:
                            tos = QueenMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        case 5:
                            tos = KingMoves(from, info.stm);
                            foreach (int to in tos) if (isLegal(from, to, p, info.stm)) return true; break;
                        default: break;
                    }
                }
            }
            return false;
        }
        public string toSan(string move)
        {
            /*sanity checks*/
            if (move.Length != 4 && move.Length != 5) return "";
            string fstring = move.Substring(0, 2);
            string tstring = move.Substring(2, 2);
            int f = -1; int t = -1;
            for (int j = 0; j < 64; ++j)
            {
                if (fstring == SanSquares[j]) f = j;
                if (tstring == SanSquares[j]) t = j;
            }
            if (!onBoard(f) || !onBoard(t)) return "";
            int p = PieceOn(t); int color = ColorOn(t);
            if (p == (int)Piece.PIECE_NONE || color == COLOR_NONE) return "";
            string sanMove = (p == (int)Piece.PAWN ? "" : Convert.ToString(SanPiece[p]));
            string promotionPiece = "";

            // special cases (castle moves, promotions)
            if (info.isCastle())
            {
                int left = (color == WHITE ? (int)Squares.C1 : (int)Squares.C8);
                int right = (color == WHITE ? (int)Squares.G1 : (int)Squares.G8);
                if (t == right) return "O-O";
                else if (t == left) return "O-O-O";
                else return "";
            }
            // note : we do not call isPseudoLegal(f, t, p, color) here
            // since toSan() is called after a doMove call and legality
            // has been checked already, all state variables for the move
            // are set correctly.
            else if (p == (int)Piece.PAWN && info.isPromotion())
            {
                promotionPiece = move.Substring(5, 1);
                if (String.IsNullOrWhiteSpace(promotionPiece)) return "";
            }
            else if (info.isEP()) sanMove += SanCols[ColOf(f)];

            List<int> pieces = info.PieceSquares[color][p];
            List<int> legalFromSqs = new List<int>(); // for those moves where more than one piece attacks the *to* square
            movePiece(t, f, p, color); // remove piece so the *to* sq is flagged as empty
            foreach (int fromsq in pieces)
            {
                if (fromsq == f) continue; // note : we have already moved the piece when we get here .. skip the piece sitting at *to* sq
                switch (p)
                {
                    case (int)Piece.PAWN: if (PawnMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.KNIGHT: if (KnightMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.BISHOP: if (BishopMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.ROOK: if (RookMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.QUEEN: if (QueenMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.KING: if (KingMoves(fromsq, color).Contains(t)) legalFromSqs.Add(fromsq); break;
                    default: return "";
                }
            }
            movePiece(f, t, p, color); // reset the piece, we are done checking all those pieces that attack the *to* square.
            if (legalFromSqs.Count > 0) // note : we are checking if multiple pieces could have been placed on the *to* square
            {
                if (RowOf(legalFromSqs[0]) == RowOf(f)) sanMove += SanCols[ColOf(f)];
                else if (ColOf(legalFromSqs[0]) == ColOf(f)) sanMove += Convert.ToString(RowOf(f));
                else sanMove += SanCols[ColOf(f)];
            }
            if (info.isCapture())
            {
                if (p == (int)Piece.PAWN) sanMove += SanCols[ColOf(f)];
                sanMove += "x";
            }
            sanMove += SanSquares[t];
            if (info.isPromotion()) sanMove += promotionPiece;
            // note : toSan() is called *after* a doMove call -- which means if
            // white moved, info.stm = black here, and we check if black is in mate
            // or checked
            if (isMate()) { sanMove += "#"; return sanMove; }
            if (kingInCheck(info.stm)) sanMove += "+";
            return sanMove;
        }
    }
}
