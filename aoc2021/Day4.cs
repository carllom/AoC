namespace aoc2021
{
    internal class Day4 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);
            var numbers = indata[0].Split(',').Select(int.Parse).ToList();
            var boards = ReadBoards(indata, 2);

            foreach (var item in numbers)
            {
                foreach (var board in boards)
                {
                    var idx = Array.IndexOf(board, item);
                    if (idx == -1) continue;
                    board[idx] = -1;
                }

                foreach (var board in boards)
                {
                    if (IsWinner(board))
                    {
                        var sum = board.Where(n => n >= 0).Sum();
                        return sum * item;
                    }
                }
            }
            return 0;
        }

        private List<int[]> ReadBoards(string[] indata, int startLine)
        {
            List<int[]> boards = new List<int[]>();
            while (startLine < indata.Length)
            {
                var board = new List<int>();
                for (int j = 0; j<5; j++)
                {
                    board.AddRange(indata[startLine+j].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
                }
                boards.Add(board.ToArray());
                startLine+=6;
            }
            return boards;
        }

        private bool IsWinner(int[] board)
        {
            for (int r = 0; r < 5; r++)
            {
                if (IsWinRow(board, r)) return true;
                if (IsWinColumn(board, r)) return true;
            }
            return false;
        }

        private bool IsWinRow(int[] board, int row)
        {
            for (int col = 0; col < 5; col++)
            {
                if (board[row*5+col] >= 0) return false;
            }
            return true;
        }

        private bool IsWinColumn(int[] board, int col)
        {
            for (int row = 0; row < 5; row++)
            {
                if (board[row*5+col] >= 0) return false;
            }
            return true;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile);
            var numbers = indata[0].Split(',').Select(int.Parse).ToList();
            var boards = ReadBoards(indata, 2);

            foreach (var item in numbers)
            {
                foreach (var board in boards)
                {
                    var idx = Array.IndexOf(board, item);
                    if (idx == -1) continue;
                    board[idx] = -1;
                }

                for (int b = 0; b<boards.Count; b++)
                {
                    if (IsWinner(boards[b]))
                    {
                        if (boards.Count == 1)
                        {
                            var sum = boards[b].Where(n => n >= 0).Sum();
                            return sum * item;
                        }

                        boards[b] = null; // Null promotes distrust...
                    }
                }
                boards = boards.Where(n => n != null).ToList(); // ..but we remove them immediately, promise!
            }
            return 0;
        }
    }
}
