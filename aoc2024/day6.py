def move_guard(map, x, y, d):
    while True:
        nx = x + dir[d][0]
        ny = y + dir[d][1]
        if nx < 0 or ny < 0 or ny >= len(map) or nx >= len(map[0]):
            return None
        if map[ny][nx] in "#O":  # obstacles
            return x, y, (d + 1) % 4  # turn right
        return nx, ny, d


def patrol(map, x, y, d):
    visited = set()
    while not (x, y, d) in visited:
        visited.add((x, y, d))
        np = move_guard(map, x, y, d)
        if np is None:
            return (False, visited)  # exited map
        x, y, d = np
    return (True, visited)  # looped back on itself


data = [list(l) for l in open('day6-1.data').read().splitlines()]
dir = [(0, -1), (1, 0), (0, 1), (-1, 0)]
x0, y0 = 0, 0
for i, line in enumerate(data):  # find starting point
    if '^' in line:
        y0 = i
        x0 = line.index('^')
        break

gpath = set(map(lambda p: (p[0], p[1]), patrol(data, x0, y0, 0)[1]))  # filter to position only - path is used in part 2 to place obstacles
print('Day6-1', len(gpath))

loops = 0
for p in gpath:
    data[p[1]][p[0]] = 'O'
    res = patrol(data, x0, y0, 0)
    if res[0]:
        loops += 1
    data[p[1]][p[0]] = '.'

print('Day6-2', loops)
