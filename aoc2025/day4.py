data = [list(x.strip()) for x in open('day4-1.data', encoding='utf-8').readlines()]
NEIGH8 = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1),  (0, 1),  (1, 1)]


def getgrid(grid, x, y):  # save grid value getter with out-of-bounds check
    return grid[y][x] if 0 <= x < len(grid[0]) and 0 <= y < len(grid) else '.'  # return '.' if out of bounds


def numempty(grid, x, y):  # number of cells being '.' in the 8 adjacent cells
    return [getgrid(grid, x + dx, y + dy) for dx, dy in NEIGH8].count('.')  # count '.' in neighbors


def pickable_rolls(grid):  # find all pickable rolls and return position
    for y, g_row in enumerate(grid):
        for x, c in enumerate(g_row):
            if c == '.':
                continue
            if numempty(grid, x, y) > 4:
                yield (x, y)


print('Day4-1:', len(list(pickable_rolls([row[:] for row in data]))))

pickable = {(-1, -1)}  # dummy initial value
n_rolls = 0
while len(pickable) > 0:
    pickable = list(pickable_rolls(data))
    n_rolls += len(pickable)
    for px, py in pickable:
        data[py][px] = '.'  # remove picked roll
print('Day4-2:', n_rolls)
