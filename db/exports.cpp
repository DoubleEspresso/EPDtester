#include "definitions.h"
#include "pgnio.h"
#include "board.h"
#include "log.h"
#include "globals.h"
#include "zobrist.h"
#include "magic.h"

/*chess db exports*/
extern "C"
{
	// purpose: returns a handle the pgnio class 
	DllExport pgn_io * DECL PGNIO_instance(char * dbname)
	{
		return new pgn_io(dbname);
	}

	// purpose: input a (possibly large) .txt file of pgn games and store a binary file
	// of the hashed game data
	DllExport bool DECL PGN_parse(char * pgn_in_fname, char * db_fname, size_t size_mb)
	{
		Log::init(L"\\M.Glatzmaier\\EPD Tester\\1.0.0.0\\Log-unmanaged"); // creates a log every time pgn-parse is called .. !?
			
		/*initialization*/
		if (!Globals::init())
		{
			Log::write("..global arrays initialized failed\n");
			return false;
		}

		Magic::init();
		if (!Magic::check_magics())
		{
			Log::write("..!!ERROR : attack hash failed to initialize properly, abort!\n");
			std::cin.get();
			return false;
		}

		// fill the zobrist arrays for transposition table hashing
		if (!Zobrist::init())
		{
			Log::write("..!!ERROR : failed to initialize zobrist keys, abort!\n");
			std::cin.get();
			return false;
		}

		// parse pgn files
		Board b;
		std::istringstream fen(START_FEN);
		b.from_fen(fen);
		
		pgn_io pgn(pgn_in_fname, db_fname, size_mb);
		if (!pgn.parse(b))
		{
			// TODO : carefully free all allocs
			Log::write("..[pgn] ERROR: failed to parse pgn-file(%s) correctly\n", pgn_in_fname);
			return false;
		}
		return true;
	}


	// purpose: find move/score data from binary file, given a fen position
	DllExport void DECL PGN_find(char * res, char * dbname, const char* fen)
	{
		pgn_io pgn(dbname); // todo : fix so we don't need to instantiate the pgn_io class each time..
		sprintf(res, "%s", pgn.find(fen).c_str());
	}
}