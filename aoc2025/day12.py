data = [x.rstrip('\n') for x in open('day12-1.data', encoding='utf-8')]
pieces, puzzles, i = [], [], 0
while i < len(data):
    line = data[i]
    if 'x' in line and ':' in line:  # Puzzle is 6x7: 0 1 2 3 4 5
        parts = line.split(':')
        dims = parts[0].strip().split('x')
        width, height = int(dims[0]), int(dims[1])
        counts = list(map(int, parts[1].strip().split()))
        puzzles.append((width, height, counts))
        i += 1
        continue
    piece = []
    while i < len(data) and data[i] and ('#' in data[i] or '.' in data[i]):  # Piece line consists of # and .
        piece.append(data[i])
        i += 1
    if piece:  # Have we read a piece?
        pieces.append(piece)
    i += 1


def piece_area(piece):  # count occupied cells
    return sum(row.count('#') for row in piece)


# I had a *lot* of code here before: Keywords backtracking, DLX, dancing links, exact cover etc.
# Tried ready made DLX implementations, but they could not manage the size of the problem, nor the quantity of puzzles.
# Checked the area constraint early on to prune puzzles, but did not try it as an answer...
# After a while I realized that the puzzles themselves are incredibly hard to solve even by best practice algorithms.
# So I gave up and tried the area constraint as the answer, expecting it to be too high and it worked.

# Do the pieces fit on the board based on area?
fits_in_area = [sum(piece_area(p) * c for p, c in zip(pieces, counts)) <= width * height for width, height, counts in puzzles]
print('Day12-1:', sum(fits_in_area))  # Fuuuuuuuuuck it was the answer
