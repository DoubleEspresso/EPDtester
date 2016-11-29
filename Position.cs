using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{

    public class Position
    {
        private List<List<int>> wPieceSquares = null; // [piece][square]
        private List<List<int>> bPieceSquares = null;
        private List<List<String>> FenPositions = null;
        public String SanPiece = "PNBRQKpnbrqk";
        public String SanCols = "abcdefgh";
        private int[] priv_colorOn = null;
        private int[] pieceOn = null;
        private int[] kingSqs = null;
        private int[] pieceDiffs = null;
        bool gameFinished = false; // for mate/draw global flag
        public int W_KS = 1;
        public int W_QS = 2;
        public int B_KS = 4;
        public int B_QS = 8;
        public static int WHITE = 0;
        public static int BLACK = 1;
        public static int COLOR_NONE = 2;
        public int stm = WHITE;
        public int crights = 0;
        public int EP_SQ = 0;
        public int Move50 = 0;
        public int HalfMvs = 0;
        public int capturedPiece = -1;
        private int promotedPiece = -1;
        private bool moveIsEP = false;
        private bool moveIsCapture = false;
        private bool moveIsPromotion = false;
        private bool moveIsPromotionCapture = false;
        private bool moveIsCastle = false;
        private int displayedMove = 0;
        public static String StartFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public static String[] SanSquares =
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
        public Position() { Clear(); }
        public Position(string fen)
        {
            if (!Load(fen)) Log.WriteLine("..[position] Warning failed to load position {0}", fen);
        }
        public bool isPromotion() { return (moveIsPromotion); }
        public bool isCapture() { return (moveIsCapture || moveIsEP); }
        public static int[] FromTo(string move)
        {
            int ifrom = -1; int ito = -1;
            string from = move.Substring(0, 2);
            string to = move.Substring(2, 2);
            for (int j = 0; j < SanSquares.Length; ++j)
            {
                if (from == SanSquares[j]) ifrom = j;
                if (to == SanSquares[j]) ito = j;
            }
            return new int[] { ifrom, ito };
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
                    if (isEmpty(s)) { ++empties; continue; }

                    if (empties > 0)
                    {
                        fen += empties; empties = 0;
                    }
                    fen += SanPiece[(priv_colorOn[s] == BLACK ? pieceOn[s] + 6 : pieceOn[s])];
                }
                if (empties > 0)
                {
                    fen += empties;
                }
                if (r > 0) fen += "/";
            }
            fen += (stm == WHITE ? " w" : " b");

            // castle rights
            String castleRights = "";
            if ((crights & W_KS) == W_KS) castleRights += "K";
            if ((crights & W_QS) == W_QS) castleRights += "Q";
            if ((crights & B_KS) == B_KS) castleRights += "k";
            if ((crights & B_QS) == B_QS) castleRights += "q";
            fen += (castleRights == "" ? " -" : " " + castleRights);

            // ep-square
            String epSq = "";
            if (EP_SQ != 0)
            {
                epSq += SanCols[ColOf(EP_SQ)] + Convert.ToString(RowOf(EP_SQ) + 1);
            }
            fen += (epSq == "" ? " -" : " " + epSq);
            // move50
            // half-mvs
            return fen;
        }
        public bool Load(string fen)
        {
            Log.WriteLine("..[position] loading fen {0} ", fen);
            Clear();
            int s = (int)Squares.A8;
            String[] split_fen = fen.Split(' ');
            if (split_fen.Length <= 0)
                return false;

            for (int i = 0; i < split_fen[0].Length; ++i)
            {
                char c = fen[i];
                bool isDigit = (c >= '0' && c <= '9');
                if (isDigit)
                    s += (c - '0');
                else
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
            else stm = (split_fen[1][0] == 'w' ? WHITE : BLACK);

            // castle rights
            if (split_fen.Length <= 2) return true;
            else
            {
                for (int i = 0; i < split_fen[2].Length; ++i)
                {
                    char c = split_fen[2][i];
                    switch (c)
                    {
                        case 'K': crights |= W_KS; break;
                        case 'Q': crights |= W_QS; break;
                        case 'k': crights |= B_KS; break;
                        case 'q': crights |= B_QS; break;
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
                    EP_SQ = 8 * row + col;
                }
                else
                    EP_SQ = 0;
            }
            // the half-moves since last pawn move/capture
            if (split_fen.Length <= 4)
                return true;
            else
            {
                if (!String.IsNullOrWhiteSpace(split_fen[4])) Move50 = (int)Convert.ToInt32(split_fen[4]);
            }
            // the move counter
            if (split_fen.Length <= 5) return true;
            else HalfMvs = (int)Convert.ToInt32(split_fen[5]);

            // add this position to the position tracking
            if (FenPositions == null) FenPositions = new List<List<string>>();
            FenPositions.Add(new List<String>());
            int idx = FenPositions.Count - 1;
            FenPositions[idx].Add(toFen());
            setDisplayedMoveIdx(idx);

            return true;
        }
        public void Clear()
        {
            stm = WHITE;
            crights = 0;
            EP_SQ = 0;
            Move50 = 0;
            HalfMvs = 0;
            displayedMove = 0;
            gameFinished = false; // for mate/draw global flag
            capturedPiece = -1;
            promotedPiece = -1;
            moveIsEP = false;
            moveIsCapture = false;
            moveIsPromotion = false;
            moveIsPromotionCapture = false;
            moveIsCastle = false;

            if (wPieceSquares == null) wPieceSquares = new List<List<int>>();

            wPieceSquares.Clear();
            for (int i = 0; i < 6; ++i) wPieceSquares.Add(new List<int>()); // pawn, knight, bishop, rook queen, king
            if (bPieceSquares == null) bPieceSquares = new List<List<int>>();

            bPieceSquares.Clear();
            for (int i = 0; i < 6; ++i) bPieceSquares.Add(new List<int>()); // pawn, knight, bishop, roo queen, king

            if (priv_colorOn == null) priv_colorOn = new int[64];
            for (int i = 0; i < 64; ++i) priv_colorOn[i] = COLOR_NONE;

            if (pieceOn == null) pieceOn = new int[64];
            for (int i = 0; i < 64; ++i) pieceOn[i] = (int)(int)Piece.PIECE_NONE;

            if (kingSqs == null) kingSqs = new int[2];
            for (int i = 0; i < 2; ++i) kingSqs[i] = (int)Squares.SQ_NONE;

            if (pieceDiffs == null) pieceDiffs = new int[6];
            for (int i = 0; i < 6; ++i) pieceDiffs[i] = 0;

            if (FenPositions == null)
            {
                FenPositions = new List<List<String>>();
                FenPositions.Clear();
            }
        }
        public bool isEmpty(int s)
        {
            return pieceOn[s] == (int)(int)Piece.PIECE_NONE;
        }
        public List<int> PieceSquares(int c, int p)
        {
            return (c == WHITE ? wPieceSquares[p] : bPieceSquares[p]);
        }
        public int PieceOn(int s)
        {
            return pieceOn[s];
        }
        public int colorOn(int s)
        {
            if (!onBoard(s)) return (int)(int)Piece.PIECE_NONE;
            return priv_colorOn[s];
        }
        private void SetPiece(char c, int s)
        {
            for (int p = (int)Pieces.W_PAWN; p <= (int)Pieces.B_KING; ++p)
            {
                if (c == SanPiece[p])
                {
                    int color = (p < 6 ? WHITE : BLACK);
                    int piece = (p > (int)Pieces.W_KING ? p - 6 : p);

                    if (color == WHITE) wPieceSquares[piece].Add(s);
                    else bPieceSquares[piece].Add(s);

                    priv_colorOn[s] = color;
                    pieceOn[s] = piece;
                    if (p == (int)Pieces.W_KING || p == (int)Pieces.B_KING) kingSqs[color] = s;
                    else
                    {
                        if (color == WHITE) pieceDiffs[piece]++;
                        else pieceDiffs[piece]--;
                    }
                }
            }
        }
        public bool isLegal(int from, int to, int piece, int color, bool update)
        {
            if (color != stm || !onBoard(from) || !onBoard(to)) return false;
            if (PieceOn(from) == (int)Piece.PIECE_NONE) return false;
            if (isCastle(from, to, piece, color))
            {
                if (!isLegalCastle(from, to, piece, color)) return false;
                clearAllCastleRights(color);
                return true;
            }
            else if (!isPseudoLegal(from, to, piece, color)) return false;
            doMove(from, to, piece, color);
            int tmp_capturedPiece = capturedPiece;
            int tmp_promotedPiece = promotedPiece;
            bool tmp_moveIsCapture = moveIsCapture;
            bool tmp_moveIsPromotion = moveIsPromotion;
            bool tmp_moveIsPromotionCapture = moveIsPromotionCapture;
            bool tmp_moveIsCastle = moveIsCastle;
            bool tmp_moveIsEP = moveIsEP;
            bool inCheck = kingInCheck(color);

            // restore move state
            capturedPiece = tmp_capturedPiece;
            promotedPiece = tmp_promotedPiece;
            moveIsCapture = tmp_moveIsCapture;
            moveIsPromotion = tmp_moveIsPromotion;
            moveIsPromotionCapture = tmp_moveIsPromotionCapture;
            moveIsCastle = tmp_moveIsCastle;
            moveIsEP = tmp_moveIsEP;

            if (inCheck)
            {
                undoMove(to, from, piece, color);
                return false;
            }

            // move is legal -- update position data
            if (update)
            {
                if (pieceOn[to] == (int)(int)Piece.KING) clearAllCastleRights(color); // in case king has moved
                if (pieceOn[to] == (int)(int)Piece.ROOK && from == (int)Squares.A1 && color == WHITE) clearCastleRights(W_QS);
                if (pieceOn[to] == (int)(int)Piece.ROOK && from == (int)Squares.H1 && color == WHITE) clearCastleRights(W_KS);
                if (pieceOn[to] == (int)(int)Piece.ROOK && from == (int)Squares.A8 && color == BLACK) clearCastleRights(B_QS);
                if (pieceOn[to] == (int)(int)Piece.ROOK && from == (int)Squares.H8 && color == BLACK) clearCastleRights(B_KS);
            }

            // update EP square
            EP_SQ = 0;
            if (color == WHITE)
            {
                if (piece == (int)(int)Piece.PAWN && to - from == 16)
                {
                    EP_SQ = to - 8;
                }
            }
            else
            {
                if (piece == (int)(int)Piece.PAWN && from - to == 16)
                {
                    EP_SQ = to + 8;
                }
            }
            if (update && !moveIsPromotion) // handle promotion moves separately
            {
                FenPositions.Add(new List<String>());
                int idx = FenPositions.Count - 1;
                FenPositions[idx].Add(toFen());
                setDisplayedMoveIdx(idx);
            }
            undoMove(to, from, piece, color);
            return true;
        }
        public string getPosition(int idx, int mvidx)
        {
            if (idx < 0 || idx > FenPositions.Count - 1) return "";
            return FenPositions[idx][mvidx];
        }
        public int displayedMoveIdx() { return displayedMove; }
        public void setDisplayedMoveIdx(int idx) { displayedMove = idx; }
        public int maxDisplayedMoveIdx() { return FenPositions.Count - 1; }
        public bool setPositionFromFenStrings(int idx, int mvidx)
        {
            if (idx < 0 || idx > FenPositions.Count - 1) return false;
            if (mvidx < 0 || mvidx > FenPositions[idx].Count - 1) return false;
            if (!Load(FenPositions[idx][mvidx])) return false;
            displayedMove = idx;
            return true;
        }
        public string toSan(string move)
        {
            //clearMoveData();
            int[] fromto = FromTo(move);
            int f = fromto[0]; int t = fromto[1];
            int p = PieceOn(f);
            bool isCapture = PieceOn(t) != (int)Piece.PIECE_NONE;
            if (p == (int)Piece.PIECE_NONE) return "";
            string sanMove = ""; string sanFrom = SanSquares[f]; string sanTo = SanSquares[t];
            List<int> toSquares = new List<int>();
            List<int> pieces = PieceSquares(stm, p); // more than one ?
            sanMove += (p == (int)Piece.PAWN ? "" : Convert.ToString(SanPiece[p]));
            List<int> legalFromSqs = new List<int>();
            string promotionPiece = "";

            // special cases (castle moves, promotions)
            if (isCastle(f, t, p, stm))
            {
                int left = (stm == WHITE ? (int)Squares.C1 : (int)Squares.C8);
                int right = (stm == WHITE ? (int)Squares.G1 : (int)Squares.G8);
                if (t == right) return "O-O";
                else if (t == left) return "O-O-O";
                else return "";
            }
            else if (p == (int)Piece.PAWN && isPseudoLegal(f, t, p, stm) && (moveIsPromotion || moveIsPromotionCapture))
            {
                promotionPiece = move.Substring(5, 1);
                if (String.IsNullOrWhiteSpace(promotionPiece)) return "";
            }
            else if (moveIsEP)
            {
                sanMove += SanCols[ColOf(f)];
                isCapture = true;
            }

            foreach (int fromsq in pieces)
            {
                switch (p)
                {
                    case (int)Piece.PAWN: if (PawnMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.KNIGHT: if (KnightMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.BISHOP: if (BishopMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.ROOK: if (RookMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.QUEEN: if (QueenMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    case (int)Piece.KING: if (KingMoves(fromsq, stm).Contains(t)) legalFromSqs.Add(fromsq); break;
                    default: return "";
                }
            }
            if (legalFromSqs.Count <= 0) return "";
            else if (legalFromSqs.Count == 2)
            {
                if (RowOf(legalFromSqs[0]) == RowOf(legalFromSqs[1])) sanMove += SanCols[ColOf(f)];
                else if (ColOf(legalFromSqs[0]) == ColOf(legalFromSqs[1])) sanMove += Convert.ToString(RowOf(f));
            }
            if (isCapture || moveIsPromotionCapture)
            {
                if (p == (int)Piece.PAWN) sanMove += SanCols[ColOf(f)];
                sanMove += "x";
            }
            sanMove += SanSquares[t];
            if (moveIsPromotion || moveIsPromotionCapture) sanMove += promotionPiece;

            // does it mate the enemy king
            bool ismate = isMate(f, t, p, (stm == WHITE ? BLACK : WHITE));
            if (ismate) { sanMove += "#"; return sanMove; }

            // does it check the king?
            clearMoveData();
            doMove(f, t, p, stm);
            int tmp_capturedPiece = capturedPiece;
            int tmp_promotedPiece = promotedPiece;
            bool tmp_moveIsCapture = moveIsCapture;
            bool tmp_moveIsPromotion = moveIsPromotion;
            bool tmp_moveIsPromotionCapture = moveIsPromotionCapture;
            bool tmp_moveIsCastle = moveIsCastle;
            bool tmp_moveIsEP = moveIsEP;

            bool givesCheck = kingInCheck(stm);
            // restore move state
            capturedPiece = tmp_capturedPiece;
            promotedPiece = tmp_promotedPiece;
            moveIsCapture = tmp_moveIsCapture;
            moveIsPromotion = tmp_moveIsPromotion;
            moveIsPromotionCapture = tmp_moveIsPromotionCapture;
            moveIsCastle = tmp_moveIsCastle;
            moveIsEP = tmp_moveIsEP;

            undoMove(t, f, p, stm == WHITE ? BLACK : WHITE);
            if (givesCheck) sanMove += "+";

            return sanMove;
        }
        public void clearMoveData()
        {
            capturedPiece = -1;
            promotedPiece = -1;
            moveIsEP = false;
            moveIsCapture = false;
            moveIsPromotion = false;
            moveIsPromotionCapture = false;
            moveIsCastle = false;
        }
        public bool isPseudoLegal(int from, int to, int piece, int color)
        {
            if (colorOn(to) == color) return false;
            switch (piece)
            {
                case 0: // pawn
                    return pseudoLegalPawnMove(from, to, color);
                case 1: // knight
                    return pseudoLegalKnightMove(from, to, color);
                case 2: // bishop
                    return pseudoLegalBishopMove(from, to, color);
                case 3: // rook
                    return pseudoLegalRookMove(from, to, color);
                case 4: // queen
                    return pseudoLegalQueenMove(from, to, color);
                case 5: // king
                    return pseudoLegalKingMove(from, to, color);
            }
            return false;
        }
        public bool isCastle(int from, int to, int piece, int color)
        {
            if (piece != (int)(int)Piece.KING) return false;
            if (PieceOn(from) != (int)(int)Piece.KING) return false;
            if (from != (color == WHITE ? (int)Squares.E1 : (int)Squares.E8)) return false;
            if (color == WHITE)
            {
                if (to != (int)Squares.G1 && to != (int)Squares.C1) return false;
            }
            else
            {
                if (to != (int)Squares.G8 && to != (int)Squares.C8) return false;
            }
            return true;
        }
        public bool isLegalCastle(int from, int to, int piece, int color)
        {
            if (!hasCastleRights(to, color)) return false;
            int sqLeft1 = (color == WHITE ? (int)Squares.D1 : (int)Squares.D8);
            int sqLeft2 = (color == WHITE ? (int)Squares.C1 : (int)Squares.C8);
            int sqRight1 = (color == WHITE ? (int)Squares.F1 : (int)Squares.F8);
            int sqRight2 = (color == WHITE ? (int)Squares.G1 : (int)Squares.G8);
            if (to == sqRight2)
            {
                if (!isEmpty(sqRight1) && !isEmpty(sqRight2)) return false;
                if (isAttacked(sqRight1, color) || isAttacked(sqRight2, color)) return false;
                if (kingInCheck(color)) return false;
            }
            else
            {
                if (!isEmpty(sqLeft1) && !isEmpty(sqLeft2)) return false;
                if (isAttacked(sqLeft1, color) || isAttacked(sqLeft2, color)) return false;
                if (kingInCheck(color)) return false;
            }
            moveIsCastle = true;
            return true;
        }
        public bool hasCastleRights(int to, int color)
        {
            int ks = (color == WHITE ? (crights & 1) : (crights & 4));
            int qs = (color == WHITE ? (crights & 2) : (crights & 8));
            if (color == WHITE)
            {
                if (to == (int)Squares.G1 && (ks == 1)) return true;
                else if (to == (int)Squares.C1 && (qs == 2)) return true;
            }
            else
            {
                if (to == (int)Squares.G8 && (ks == 4)) return true;
                else if (to == (int)Squares.C8 && (qs == 8)) return true;
            }
            return false;
        }
        public void clearAllCastleRights(int color)
        {
            if (color == WHITE)
            {
                crights = (crights & 12);
            }
            else crights = (crights & 3);
        }
        public void clearCastleRights(int side)
        {
            crights = (crights & (~side));
            //Log.WriteLine("all cr after: " + crights);
        }
        int RowOf(int from)
        {
            return (from >> 3);
        }
        int ColOf(int from)
        {
            return (from & 7);
        }
        bool onBoard(int to)
        {
            return (to >= 0 && to <= 63);
        }
        bool enemyOn(int s, int color)
        {
            return (priv_colorOn[s] == color && pieceOn[s] != (int)Piece.PIECE_NONE);
        }
        int colDiff(int s1, int s2)
        {
            return Math.Abs(ColOf(s1) - ColOf(s2));
        }
        int rowDiff(int s1, int s2)
        {
            return Math.Abs(RowOf(s1) - RowOf(s2));
        }
        bool onCol(int s1, int s2)
        {
            return colDiff(s1, s2) == 0;
        }
        bool onRow(int s1, int s2)
        {
            return rowDiff(s1, s2) == 0;
        }
        bool onDiag(int s1, int s2)
        {
            return colDiff(s1, s2) == rowDiff(s1, s2);
        }
        private bool pseudoLegalPawnMove(int from, int to, int color)
        {
            int enemy = (color == WHITE ? BLACK : WHITE);
            int forward1 = (color == WHITE ? 8 : -8);
            int forward2 = (color == WHITE ? 16 : -16);
            int capRight = (color == WHITE ? 7 : -7);
            int capLeft = (color == WHITE ? 9 : -9);
            bool on4 = (color == WHITE ? (RowOf(from) == 4) : (RowOf(from) == 3));
            bool on7 = (color == WHITE ? (RowOf(from) == 6) : (RowOf(from) == 1));
            bool on2 = (color == WHITE ? (RowOf(from) == 1) : (RowOf(from) == 6));

            if (on2)
            {
                if ((from + forward1) == to && isEmpty(to)) return true;
                else if ((from + forward2) == to && isEmpty(to) && isEmpty(from + forward1)) return true;
                else if ((from + capRight) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsCapture = true;
                    return true;
                }
                else if ((from + capLeft) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsCapture = true;
                    return true;
                }
            }
            else if (on4 && EP_SQ == to) // EP move?
            {
                int epTo = (color == WHITE ? to - 8 : to + 8);
                if ((from + capRight) == to && enemyOn(epTo, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsEP = true;
                    return true;
                }
                else if ((from + capLeft) == to && enemyOn(epTo, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsEP = true;
                    return true;
                }
            }
            else if (on7)
            {
                if ((from + forward1) == to && isEmpty(to))
                {
                    moveIsPromotion = true;
                    return true;
                }
                else if ((from + capRight) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsPromotionCapture = true;
                    //moveIsCapture = true; moveIsPromotion = true;
                    return true;
                }
                else if ((from + capLeft) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsPromotionCapture = true;
                    //moveIsCapture = true; moveIsPromotion = true;
                    return true;
                }
            }
            else
            {
                if ((from + forward1) == to && isEmpty(to))
                    return true;
                else if ((from + capRight) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsCapture = true;
                    return true;
                }
                else if ((from + capLeft) == to && enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to))
                {
                    moveIsCapture = true;
                    return true;
                }
            }
            // TODO: handle promotions
            return false;
        }
        private bool pseudoLegalKnightMove(int from, int to, int color)
        {
            int[] deltas = { 16 - 1, 16 + 1, 2 + 8, 2 - 8, -16 + 1, -16 - 1, -2 - 8, -2 + 8 };
            int enemy = (color == WHITE ? BLACK : WHITE);

            for (int j = 0; j < deltas.Length; ++j)
            {
                int t = deltas[j] + from;
                if (t != to || (colDiff(from, to) != 2 && colDiff(from, to) != 1))
                    continue;
                else if (!onBoard(t) || (!isEmpty(to) && !enemyOn(to, enemy)))
                    return false;
                else if (onBoard(t) && isEmpty(to) && !enemyOn(to, enemy))
                    return true;
                else if (onBoard(t) && !isEmpty(to) && enemyOn(to, enemy))
                {
                    moveIsCapture = true;
                    return true;
                }
            }
            return false;
        }
        private bool pseudoLegalBishopMove(int from, int to, int color)
        {
            int[] deltas = { 7, 9, -7, -9 };
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;

                while (onBoard(t) && onDiag(from, t) && (isEmpty(t) || enemyOn(t, enemy)))
                {
                    if (!isEmpty(t) && t != to)
                        break;
                    else if (isEmpty(t) && t == to) return true;
                    else if (enemyOn(t, enemy) && t == to)
                    {
                        moveIsCapture = true;
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
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && (onRow(from, t) || onCol(from, t)) && (isEmpty(t) || enemyOn(t, enemy)))
                {
                    if (!isEmpty(t) && t != to)
                        break;
                    else if (t == to && isEmpty(t))
                        return true;
                    else if (enemyOn(t, enemy) && t == to)
                    {
                        moveIsCapture = true;
                        return true;
                    }
                    t = from + d * (++c);
                }
            }
            return false;
        }
        private bool pseudoLegalQueenMove(int from, int to, int color)
        {
            if (onDiag(from, to)) return pseudoLegalBishopMove(from, to, color);
            else if (onRow(from, to) || onCol(from, to)) return pseudoLegalRookMove(from, to, color);
            return false;
        }
        private bool pseudoLegalKingMove(int from, int to, int color)
        {
            int[] deltas = { 1, -1, 7, 9, 8, -8, -7, -9 };
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int t = from + d;
                if (onBoard(t) && (isEmpty(t) || enemyOn(t, enemy)))
                {
                    if ((!isEmpty(t) && !enemyOn(t, enemy)))
                        return false;
                    else if ((rowDiff(from, to) > 1 || colDiff(from, to) > 1)) return false;
                    else if (isEmpty(t) && t == to)
                        return true;
                    else if (enemyOn(t, enemy) && t == to)
                    {
                        moveIsCapture = true;
                        return true;
                    }
                }
            }
            return false;
        }
        private List<int> PawnMoves(int from, int color)
        {
            List<int> to_sqs = new List<int>();
            int enemy = (color == WHITE ? BLACK : WHITE);
            int forward1 = (color == WHITE ? 8 : -8);
            int forward2 = (color == WHITE ? 16 : -16);
            int capRight = (color == WHITE ? 7 : -7);
            int capLeft = (color == WHITE ? 9 : -9);
            int to = -1;
            bool on4 = (color == WHITE ? (RowOf(from) == 4) : (RowOf(from) == 3));
            bool on2 = (color == WHITE ? (RowOf(from) == 1) : (RowOf(from) == 6));

            if (on2)
            {
                to = (from + forward1);
                if (isEmpty(to)) to_sqs.Add(to);

                to = (from + forward2);
                if (to_sqs.Count > 0 && isEmpty(to)) to_sqs.Add(to);

                to = (from + capRight);
                if (enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            else if (on4 && EP_SQ == to) // EP move?
            {
                int epTo = (color == WHITE ? to - 8 : to + 8);
                to = (from + capRight);
                if (enemyOn(epTo, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (enemyOn(epTo, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            else
            { // same condition for normal pawn moves or promotion moves - this method is meant to check if a pawn move exists, it doesn't 
              // return the exact number of pawn moves.
                to = (from + forward1);
                if (isEmpty(to)) to_sqs.Add(to);

                to = (from + capRight);
                if (enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);

                to = (from + capLeft);
                if (enemyOn(to, enemy) && colDiff(from, to) == 1 && onBoard(to)) to_sqs.Add(to);
            }
            return to_sqs;
        }
        private List<int> KnightMoves(int from, int color)
        {
            List<int> to_sqs = new List<int>();
            int[] deltas = { 16 - 1, 16 + 1, 2 + 8, 2 - 8, -16 + 1, -16 - 1, -2 - 8, -2 + 8 };
            int enemy = (color == WHITE ? BLACK : WHITE);

            for (int j = 0; j < deltas.Length; ++j)
            {
                int t = deltas[j] + from;

                if (!onBoard(t) || (!isEmpty(t) && !enemyOn(t, enemy))) continue;
                else if (onBoard(t) && isEmpty(t) && !enemyOn(t, enemy)) to_sqs.Add(t);
                else if (onBoard(t) && !isEmpty(t) && enemyOn(t, enemy)) to_sqs.Add(t);
            }
            return to_sqs;
        }
        private List<int> BishopMoves(int from, int color)
        {
            List<int> to_sqs = new List<int>();
            int[] deltas = { 7, 9, -7, -9 };
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && onDiag(from, t) && (isEmpty(t) || enemyOn(t, enemy)))
                {
                    if (!isEmpty(t) && !enemyOn(t, enemy))
                        break;
                    else if (isEmpty(t))
                    {
                        to_sqs.Add(t);
                    }
                    else if (enemyOn(t, enemy))
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
            List<int> to_sqs = new List<int>();
            int[] deltas = { 1, -1, 8, -8 };
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int d = deltas[j];
                int c = 1;
                int t = from + d * c;
                while (onBoard(t) && (onRow(from, t) || onCol(from, t)) && (isEmpty(t) || enemyOn(t, enemy)))
                {
                    if (!isEmpty(t) && !enemyOn(t, enemy))
                        break;
                    else if (isEmpty(t))
                    {
                        to_sqs.Add(t);
                    }
                    else if (enemyOn(t, enemy))
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
            int enemy = (color == WHITE ? BLACK : WHITE);
            for (int j = 0; j < deltas.Length; ++j)
            {
                int t = deltas[j] + from;
                if (!onBoard(t) || (!isEmpty(t) && !enemyOn(t, enemy))) continue;
                else if (onBoard(t) && isEmpty(t) && !enemyOn(t, enemy)) to_sqs.Add(t);
                else if (onBoard(t) && !isEmpty(t) && enemyOn(t, enemy)) to_sqs.Add(t);
            }
            return to_sqs;
        }
        public bool doMove(int from, int to, int piece, int color)
        {
            // todo : refactor to not call remove/add during iteration 
            if (color == WHITE)
            {
                List<int> wsquares = PieceSquares(WHITE, PieceOn(from));
                for (int j = 0; j < wsquares.Count; ++j)
                    if (from == wsquares[j])
                    {
                        wPieceSquares[piece].RemoveAt(j);
                        wPieceSquares[piece].Add(to);
                        break;
                    }
            }
            else
            {
                List<int> bsquares = PieceSquares(BLACK, PieceOn(from));
                for (int j = 0; j < bsquares.Count; ++j)
                    if (from == bsquares[j])
                    {
                        bPieceSquares[piece].RemoveAt(j);
                        bPieceSquares[piece].Add(to);
                        break;
                    }
            }
            if (moveIsCapture)
            {
                capturedPiece = pieceOn[to];
                if (color == WHITE) // remove black piece
                {
                    List<int> bsquares = PieceSquares(BLACK, PieceOn(to));
                    for (int j = 0; j < bsquares.Count; ++j)
                        if (to == bsquares[j])
                        {
                            bPieceSquares[capturedPiece].RemoveAt(j); // remove piece @ to sq
                            break;
                        }
                }
                else
                {
                    List<int> wsquares = PieceSquares(WHITE, PieceOn(to));
                    for (int j = 0; j < wsquares.Count; ++j)
                    {
                        if (to == wsquares[j])
                        {
                            wPieceSquares[capturedPiece].RemoveAt(j); // remove piece @ to sq
                            break;
                        }
                    }
                }
            }
            else if (moveIsEP)
            {
                int epTo = (color == WHITE ? to - 8 : to + 8);
                capturedPiece = pieceOn[epTo];
                if (color == WHITE) // remove black piece
                {
                    List<int> bsquares = PieceSquares(BLACK, PieceOn(epTo));
                    for (int j = 0; j < bsquares.Count; ++j)
                        if (epTo == bsquares[j])
                        {
                            bPieceSquares[capturedPiece].RemoveAt(j); // remove piece @ to sq
                            break;
                        }
                }
                else
                {
                    List<int> wsquares = PieceSquares(WHITE, PieceOn(epTo));
                    for (int j = 0; j < wsquares.Count; ++j)
                        if (epTo == wsquares[j])
                        {
                            wPieceSquares[capturedPiece].RemoveAt(j); // remove piece @ to sq
                            break;
                        }
                }
                pieceOn[epTo] = (int)(int)Piece.PIECE_NONE;
                priv_colorOn[epTo] = COLOR_NONE;
            }
            else if (moveIsCastle)
            {
                if (color == WHITE)
                {
                    int rookFrom = (to == (int)Squares.G1 ? (int)Squares.H1 : (int)Squares.A1);
                    int rookto = (to == (int)Squares.G1 ? (int)Squares.F1 : (int)Squares.D1);
                    List<int> wsquares = PieceSquares(WHITE, PieceOn(rookFrom));
                    for (int j = 0; j < wsquares.Count; ++j)
                        if (rookFrom == wsquares[j])
                        {
                            wPieceSquares[(int)(int)Piece.ROOK].RemoveAt(j); // remove from sq
                            wPieceSquares[(int)(int)Piece.ROOK].Add(rookto); // add to sq
                            break;
                        }
                    pieceOn[rookFrom] = (int)(int)Piece.PIECE_NONE;
                    pieceOn[rookto] = (int)(int)Piece.ROOK;
                    priv_colorOn[rookFrom] = COLOR_NONE;
                    priv_colorOn[rookto] = color;
                }
                else
                {
                    int rookFrom = (to == (int)Squares.G8 ? (int)Squares.H8 : (int)Squares.A8);
                    int rookto = (to == (int)Squares.G8 ? (int)Squares.F8 : (int)Squares.D8);
                    List<int> bsquares = PieceSquares(BLACK, PieceOn(rookFrom));
                    for (int j = 0; j < bsquares.Count; ++j)
                        if (rookFrom == bsquares[j])
                        {
                            bPieceSquares[(int)(int)Piece.ROOK].RemoveAt(j); // remove from sq
                            bPieceSquares[(int)(int)Piece.ROOK].Add(rookto); // add to sq
                            break;
                        }
                    pieceOn[rookFrom] = (int)(int)Piece.PIECE_NONE;
                    pieceOn[rookto] = (int)(int)Piece.ROOK;
                    priv_colorOn[rookFrom] = COLOR_NONE;
                    priv_colorOn[rookto] = color;
                }
            }
            pieceOn[from] = (int)(int)Piece.PIECE_NONE;
            pieceOn[to] = piece;

            priv_colorOn[from] = COLOR_NONE;
            priv_colorOn[to] = color;

            stm = (stm == WHITE ? BLACK : WHITE);
            if (moveIsCastle)
            {
                FenPositions.Add(new List<String>());
                int idx = FenPositions.Count - 1;
                FenPositions[idx].Add(toFen());
                setDisplayedMoveIdx(idx);
            }
            return true;
        }
        // note the from sq is no longer used -- we checked legality and *did* the pawn
        // move, and handled captures already, just remove pawn at *to* sq and replace it
        // with promoted piece at *to* sq.
        public void doPromotionMove(int from, int to, int promotedPiece, int color)
        {
            if (color == WHITE)
            {
                List<int> wsquares = PieceSquares(WHITE, PieceOn(to));
                for (int j = 0; j < wsquares.Count; ++j)
                    if (to == wsquares[j])
                    {
                        wPieceSquares[(int)(int)Piece.PAWN].RemoveAt(j); // remove from sq
                        wPieceSquares[promotedPiece].Add(to); // add to sq
                        break;
                    }
            }
            else
            {
                List<int> bsquares = PieceSquares(BLACK, PieceOn(to));
                for (int j = 0; j < bsquares.Count; ++j)
                    if (to == bsquares[j])
                    {
                        bPieceSquares[(int)(int)Piece.PAWN].RemoveAt(j);
                        bPieceSquares[promotedPiece].Add(to);
                        break;
                        //BoardWindow.addTexture(promotedPiece, to, BLACK);
                    }
            }
            pieceOn[from] = (int)(int)Piece.PIECE_NONE;
            pieceOn[to] = promotedPiece;

            priv_colorOn[from] = COLOR_NONE;
            priv_colorOn[to] = color;

            FenPositions.Add(new List<String>());
            int idx = FenPositions.Count - 1;
            FenPositions[idx].Add(toFen());
            setDisplayedMoveIdx(idx);

            // check mate/stalemate
            //if (isMate(from, to, promotedPiece, color)) Log.WriteLine("..game over, mate");
            //else if (isStaleMate()) Log.WriteLine("..game over, stalemate");
            //else if (isRepetitionDraw()) Log.WriteLine("..game over, 3-fold repetition");
        }
        public bool undoMove(int from, int to, int piece, int color)
        {
            if (color == WHITE)
            {
                List<int> wsquares = PieceSquares(WHITE, PieceOn(from));
                for (int j = 0; j < wsquares.Count(); ++j)
                    if (from == wsquares[j])
                    {
                        wPieceSquares[piece].RemoveAt(j);
                        wPieceSquares[piece].Add(to);
                        break;
                    }
            }
            else
            {
                List<int> bsquares = PieceSquares(BLACK, PieceOn(from));
                for (int j = 0; j < bsquares.Count; ++j)
                    if (from == bsquares[j])
                    {
                        bPieceSquares[piece].RemoveAt(j);
                        bPieceSquares[piece].Add(to);
                        break;
                    }
            }
            if (moveIsCapture)
            {
                if (color == WHITE) bPieceSquares[capturedPiece].Add(from);
                else wPieceSquares[capturedPiece].Add(from);
            }
            else if (moveIsEP)
            {
                int epTo = (color == WHITE ? from - 8 : from + 8);
                if (color == WHITE) bPieceSquares[capturedPiece].Add(epTo);
                else wPieceSquares[capturedPiece].Add(epTo);
                pieceOn[epTo] = (int)(int)Piece.PAWN;
                priv_colorOn[epTo] = (color == WHITE ? BLACK : WHITE);
            }
            else if (moveIsCastle)
            {
                if (color == WHITE) // remove black piece
                {
                    int rookFrom = (from == (int)Squares.G1 ? (int)Squares.F1 : (int)Squares.D1);
                    int rookto = (from == (int)Squares.F1 ? (int)Squares.H1 : (int)Squares.A1);
                    List<int> wsquares = PieceSquares(WHITE, PieceOn(rookFrom));
                    for (int j = 0; j < wsquares.Count; ++j)
                        if (rookFrom == wsquares[j])
                        {
                            wPieceSquares[(int)(int)Piece.ROOK].RemoveAt(j); // remove from sq
                            wPieceSquares[(int)(int)Piece.ROOK].Add(rookto); // add to sq
                            break;
                        }
                    pieceOn[rookFrom] = (int)(int)Piece.PIECE_NONE;
                    pieceOn[rookto] = (int)(int)Piece.ROOK;
                    priv_colorOn[rookFrom] = COLOR_NONE;
                    priv_colorOn[rookto] = color;
                }
                else
                {
                    int rookFrom = (to == (int)Squares.G8 ? (int)Squares.F8 : (int)Squares.D8);
                    int rookto = (to == (int)Squares.F8 ? (int)Squares.H8 : (int)Squares.A8);
                    List<int> bsquares = PieceSquares(BLACK, PieceOn(rookFrom));
                    for (int j = 0; j < bsquares.Count; ++j)
                        if (rookFrom == bsquares[j])
                        {
                            bPieceSquares[(int)(int)Piece.ROOK].RemoveAt(j); // remove from sq
                            bPieceSquares[(int)(int)Piece.ROOK].Add(rookto); // add to sq
                            break;
                        }
                    pieceOn[rookFrom] = (int)(int)Piece.PIECE_NONE;
                    pieceOn[rookto] = (int)(int)Piece.ROOK;
                    priv_colorOn[rookFrom] = COLOR_NONE;
                    priv_colorOn[rookto] = color;
                }
            }
            pieceOn[from] = (moveIsCapture ? capturedPiece : (int)Piece.PIECE_NONE);
            pieceOn[to] = piece;
            priv_colorOn[from] = (moveIsCapture ? stm : COLOR_NONE);
            priv_colorOn[to] = color;
            stm = (stm == WHITE ? BLACK : WHITE);
            return true;
        }
        // c denotes the color of the king "in check" .. it is only called after a
        // "do-move"
        // and do-move updates the current side to move, so if white just made a
        // move, stm=black, and we want
        // to check if white's king is in check (so c == white).
        public bool kingInCheck(int c)
        {
            int ks = PieceSquares(c, (int)Piece.KING)[0];
            int enemy = (c == WHITE ? BLACK : WHITE);

            // pawn checks
            List<int> psquares = PieceSquares(enemy, (int)Piece.PAWN);
            for (int j = 0; j < psquares.Count; ++j)
            {
                int to = psquares[j];
                if (pseudoLegalPawnMove(ks, to, c))
                {
                    return true;
                }
            }

            // knight checks
            List<int> nsquares = PieceSquares(enemy, (int)Piece.KNIGHT);
            for (int j = 0; j < nsquares.Count; ++j)
            {
                int to = nsquares[j];
                if (pseudoLegalKnightMove(ks, to, c))
                {
                    return true;
                }
            }

            // bishop checks .. return a list of "to" squares being the enemy
            // bishops
            List<int> bsquares = PieceSquares(enemy, (int)Piece.BISHOP);
            for (int j = 0; j < bsquares.Count; ++j)
            {
                int to = bsquares[j];
                if (pseudoLegalBishopMove(ks, to, c))
                {
                    return true;
                }
            }

            // rook checks
            List<int> rsquares = PieceSquares(enemy, (int)Piece.ROOK);
            for (int j = 0; j < rsquares.Count; ++j)
            {
                int to = rsquares[j];
                if (pseudoLegalRookMove(ks, to, c))
                {
                    return true;
                }
            }
            // queen checks
            List<int> qsquares = PieceSquares(enemy, (int)Piece.QUEEN);
            for (int j = 0; j < qsquares.Count; ++j)
            {
                int to = qsquares[j];
                if (pseudoLegalQueenMove(ks, to, c))
                {
                    return true;
                }
            }
            return false;
        }
        public List<int> squaresBetween(int s1, int s2)
        {
            List<int> squares = new List<int>();
            if (onRow(s1, s2))
            {
                int delta = 1;
                int from = (ColOf(s1) < ColOf(s2)) ? s1 : s2;
                int to = (from == s1) ? s2 : s1;
                int s = from + delta;
                while (s < to)
                {
                    squares.Add(s); s += delta;
                }

            }
            else if (onCol(s1, s2))
            {
                int delta = 8;
                int from = (RowOf(s1) < RowOf(s2)) ? s1 : s2;
                int to = (from == s1) ? s2 : s1;
                int s = from + delta;
                while (s < to)
                {
                    squares.Add(s); s += delta;
                }
            }
            else if (onDiag(s1, s2))
            {

                int delta = 0; int from = s1; int to = s2;
                if (RowOf(from) < RowOf(to))
                {
                    if (ColOf(from) < ColOf(to)) delta = 9;
                    else delta = 7;
                }
                else
                {
                    if (ColOf(from) < ColOf(to)) delta = -7;
                    else delta = -9;
                }
                int s = from + delta;
                while (s != to)
                {
                    if (s != to) squares.Add(s); s += delta;
                }
            }
            return squares;
        }
        // check if current stm is in mate (from,to,piece) are enemy
        public bool isMate(int from, int to, int piece, int enemy)
        {
            clearMoveData();
            int ks = PieceSquares(stm, (int)Piece.KING)[0];
            if (!isAttacked(ks, stm))
            {
                clearMoveData();
                return false;
            }
            clearMoveData();

            int[] tosqs = { ks + 1, ks - 1, ks + 8, ks - 8, ks + 7, ks + 9, ks - 7, ks - 9 };
            for (int j = 0; j < tosqs.Length; ++j)
            {
                //Log.WriteLine("from = " + ks + " to = " + tosqs[j]);
                if (isLegal(ks, tosqs[j], (int)Piece.KING, stm, false))
                {
                    clearMoveData();
                    return false;
                }
                clearMoveData();
            }
            // is this a discovered check (means we need to adjust the attacking sq)
            int dscTo = findDiscoveredChecks(ks, stm);

            bool isDiscovered = (dscTo != -1 && dscTo != to);
            clearMoveData();

            // king cannot capture/escape on its own, can we capture the checking piece legally?
            if (canCapture(to, true))
            {
                clearMoveData();
                return false;
            }
            clearMoveData();

            if (isDiscovered)
            {
                if (canCapture(dscTo, true))
                {
                    clearMoveData();
                    return false; // only *LEGAL* captures so double checks will fail this
                }
                clearMoveData();
            }
            // cannot capture checking piece legally, can we block the check?
            if (piece != (int)Piece.BISHOP && piece != (int)Piece.ROOK && piece != (int)Piece.QUEEN && !isDiscovered) return true; // cannot block a stepping piece
            List<int> bSqs = squaresBetween(ks, to); bool canBlock = false;
            //Log.WriteLine("..squaresBetween = " + bSqs.Count);
            for (int j = 0; j < bSqs.Count; ++j)
            {
                int s = bSqs[j];
                //Log.WriteLine("..check block @ sq = " + s);

                if (!isEmpty(s)) break;
                if (canCapture(s, false)) { canBlock = true; break; } // these are *LEGAL* blocking moves			
                clearMoveData();
            }
            clearMoveData();
            bool canBlockd = false;
            if (isDiscovered)
            {
                List<int> bSqsd = squaresBetween(ks, dscTo);
                for (int j = 0; j < bSqsd.Count; ++j)
                {
                    int s = bSqsd[j];
                    //Log.WriteLine("..check block @ sq = " + s);

                    if (!isEmpty(s)) break;
                    if (canCapture(s, false)) { canBlockd = true; break; }
                    clearMoveData();
                }
                clearMoveData();
            }
            return !canBlock && !canBlockd;
        }
        public bool isStaleMate()
        {
            //Log.WriteLine("----------------start stalemate routine------------------------");
            // king moves
            List<int> ksquares = PieceSquares(stm, (int)Piece.KING);
            for (int j = 0; j < ksquares.Count; ++j)
            {
                int from = ksquares[j];
                List<int> to_sqs = KingMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {

                    int to = to_sqs[k];
                    //Log.WriteLine("..to = " + to + " empty? = " + isEmpty(to));
                    if (isLegal(from, to, (int)Piece.KING, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                    //Log.WriteLine("..to = " + to + " NOT LEGAL ");
                }
            }

            // pawn moves 
            List<int> psquares = PieceSquares(stm, (int)Piece.PAWN);
            for (int j = 0; j < psquares.Count; ++j)
            {
                int from = psquares[j];
                List<int> to_sqs = PawnMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {
                    int to = to_sqs[k];
                    if (isLegal(from, to, (int)Piece.PAWN, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                }
            }

            // knight moves 
            List<int> nsquares = PieceSquares(stm, (int)Piece.KNIGHT);
            for (int j = 0; j < nsquares.Count; ++j)
            {
                int from = nsquares[j];
                List<int> to_sqs = KnightMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {
                    int to = to_sqs[k];
                    if (isLegal(from, to, (int)Piece.KNIGHT, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                }
            }

            // bishop moves 
            List<int> bsquares = PieceSquares(stm, (int)Piece.BISHOP);
            for (int j = 0; j < bsquares.Count; ++j)
            {
                int from = bsquares[j];
                List<int> to_sqs = BishopMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {
                    int to = to_sqs[k];
                    if (isLegal(from, to, (int)Piece.BISHOP, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                }
            }
            // rook moves
            List<int> rsquares = PieceSquares(stm, (int)Piece.ROOK);
            for (int j = 0; j < rsquares.Count; ++j)
            {
                int from = rsquares[j];
                List<int> to_sqs = RookMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {
                    int to = to_sqs[k];
                    if (isLegal(from, to, (int)Piece.ROOK, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                }
            }

            // queen moves
            List<int> qsquares = PieceSquares(stm, (int)Piece.QUEEN);
            for (int j = 0; j < qsquares.Count; ++j)
            {
                int from = qsquares[j];
                List<int> to_sqs = QueenMoves(from, stm); // no move data was updated
                for (int k = 0; k < to_sqs.Count; ++k)
                {
                    int to = to_sqs[k];
                    if (isLegal(from, to, (int)Piece.QUEEN, stm, false))
                    {
                        clearMoveData();
                        return false;
                    }
                    clearMoveData();
                }
            }

            return true;
        }
        int findDiscoveredChecks(int to, int stm)
        {
            int enemy = (stm == WHITE ? BLACK : WHITE);

            // bishop checks .. return a list of "to" squares being the enemy
            // bishops
            List<int> bsquares = PieceSquares(enemy, (int)Piece.BISHOP); //Log.WriteLine(bsquares);
            for (int j = 0; j < bsquares.Count; ++j)
            {
                int from = bsquares[j];
                if (pseudoLegalBishopMove(from, to, enemy))
                    return from;
            }

            // rook checks
            List<int> rsquares = PieceSquares(enemy, (int)Piece.ROOK); //Log.WriteLine(rsquares);
            for (int j = 0; j < rsquares.Count; ++j)
            {
                int from = rsquares[j];
                if (pseudoLegalRookMove(from, to, enemy))
                    return from;
            }

            // queen checks
            List<int> qsquares = PieceSquares(enemy, (int)Piece.QUEEN); //Log.WriteLine(qsquares);
            for (int j = 0; j < qsquares.Count; ++j)
            {
                int from = qsquares[j];
                if (pseudoLegalQueenMove(from, to, enemy))
                    return from;
            }

            return -1;
        }
        public bool isRepetitionDraw()
        {
            if (FenPositions.Count < 6) return false;
            String fen = FenPositions[FenPositions.Count - 1][0].Split(' ')[0];
            int count = 0;
            for (int j = FenPositions.Count - 2; j >= 0; --j)
            {
                if (fen == FenPositions[j][0].Split(' ')[0]) ++count;
                if (count > 2) return true;
            }
            return false;
        }
        public bool canCapture(int to, bool checkOccupied)
        {
            if (checkOccupied)
            {
                if (isEmpty(to)) return false;
            }

            // pawn attacks 
            List<int> psquares = PieceSquares(stm, (int)Piece.PAWN);
            for (int j = 0; j < psquares.Count; ++j)
            {
                int from = psquares[j];
                if (isLegal(from, to, (int)Piece.PAWN, stm, false)) return true;
                clearMoveData();
                if (EP_SQ != 0 && isLegal(from, EP_SQ, (int)Piece.PAWN, stm, false)) return true;
            }

            // knight attacks
            List<int> nsquares = PieceSquares(stm, (int)Piece.KNIGHT);
            for (int j = 0; j < nsquares.Count; ++j)
            {
                int from = nsquares[j];
                if (isLegal(from, to, (int)Piece.KNIGHT, stm, false)) return true;
                clearMoveData();
            }

            // knight attacks
            List<int> bsquares = PieceSquares(stm, (int)Piece.BISHOP);
            for (int j = 0; j < bsquares.Count; ++j)
            {
                int from = bsquares[j];
                if (isLegal(from, to, (int)Piece.BISHOP, stm, false)) return true;
                clearMoveData();
            }

            // rook attacks
            List<int> rsquares = PieceSquares(stm, (int)Piece.ROOK);
            for (int j = 0; j < rsquares.Count; ++j)
            {
                int from = rsquares[j];
                if (isLegal(from, to, (int)Piece.ROOK, stm, false)) return true;
                clearMoveData();
            }

            // queen attacks
            List<int> qsquares = PieceSquares(stm, (int)Piece.QUEEN);
            for (int j = 0; j < qsquares.Count; ++j)
            {
                int from = qsquares[j];
                if (isLegal(from, to, (int)Piece.QUEEN, stm, false)) return true;
                clearMoveData();
            }

            return false;
        }
        public bool isAttacked(int from, int c)
        {
            int enemy = (c == WHITE ? BLACK : WHITE);

            // pawn checks
            List<int> psquares = PieceSquares(enemy, (int)Piece.PAWN);
            for (int j = 0; j < psquares.Count; ++j)
            {
                int to = psquares[j];
                if (pseudoLegalPawnMove(from, to, c))
                    return true;
            }

            // knight checks
            List<int> nsquares = PieceSquares(enemy, (int)Piece.KNIGHT);
            for (int j = 0; j < nsquares.Count; ++j)
            {
                int to = nsquares[j];
                if (pseudoLegalKnightMove(from, to, c))
                    return true;
            }

            // bishop checks .. return a list of "to" squares being the enemy
            // bishops
            List<int> bsquares = PieceSquares(enemy, (int)Piece.BISHOP);
            for (int j = 0; j < bsquares.Count; ++j)
            {
                int to = bsquares[j];
                if (pseudoLegalBishopMove(from, to, c))
                    return true;
            }

            // rook checks
            List<int> rsquares = PieceSquares(enemy, (int)Piece.ROOK);
            for (int j = 0; j < rsquares.Count; ++j)
            {
                int to = rsquares[j];
                if (pseudoLegalRookMove(from, to, c))
                    return true;
            }

            // queen checks
            List<int> qsquares = PieceSquares(enemy, (int)Piece.QUEEN);
            for (int j = 0; j < qsquares.Count; ++j)
            {
                int to = qsquares[j];
                if (pseudoLegalQueenMove(from, to, c))
                    return true;
            }
            return false;
        }
    }

}
