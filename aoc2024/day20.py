import heapq

maze = [list(l) for l in open('day20.data').read().splitlines()]
dir = [(0, -1), (-1, 0), (0, 1), (1, 0)]  # N, W, S, E
gx, gy, sx, sy = 0, 0, 0, 0
for y in range(len(maze)):  # find start and end points
    for x in range(len(maze[y])):
        if maze[y][x] == 'E':
            gx, gy = x, y
        elif maze[y][x] == 'S':
            sx, sy = x, y


def bfsx(maze, rx, ry):  # in retrospect, totally overkill for a maze with a single solution...
    q = []
    heapq.heappush(q, (0, rx, ry, []))
    visited = set()
    while q:
        c, x, y, p = heapq.heappop(q)
        if (x, y) in visited:  # we have already been here - implicitcly with a lower or equal cost because of the priority queue
            continue
        visited.add((x, y))
        if x == gx and y == gy:  # at the end tile
            return c, p
        for i in range(4):  # try all directions
            nx, ny = x+dir[i][0], y+dir[i][1]
            if not (nx, ny) in visited and maze[ny][nx] != '#':  # not visited before and not a wall
                heapq.heappush(q, (c+1, nx, ny, p + [(x, y)]))
    return -1, p


def cheatpoints(maze, path: list, x, y, si):
    count = 0
    for d in dir:
        p0 = (x+d[0], y+d[1])
        if maze[p0[1]][p0[0]] != '#':  # Optimization: you must cross a wall to take a shortcut, at least for 2 steps max
            continue
        for e in dir:
            p1 = (p0[0]+e[0], p0[1]+e[1])
            if (0 < p1[0] < len(maze[0]) and 0 < p1[1] < len(maze)) and maze[p1[1]][p1[0]] != '#' and p1 != (x, y) and path.index(p1)-si >= 102:
                count += 1  # within bounds, not on a wall tile or the starting point and net time gain is equal to or greater than 100ns
    return count


def cheatpoints_mh(path: list, start_idx, maxdist=20, mingain=100):
    start = path[start_idx]
    count = 0
    for trg_idx in range(len(path)-1, start_idx + mingain, -1):  # work backwards from the end point to the start point + 100 (minimum required gain)
        target = path[trg_idx]
        dist = abs(target[0]-start[0])+abs(target[1]-start[1])
        if dist <= maxdist and (trg_idx-start_idx-dist) >= mingain:
            count += 1  # we can reach the target in 20ns and the net time gain is equal to or greater than 100ns
    return count


r1 = bfsx(maze, sx, sy)  # solve the maze using a totally overkill bfs (since it is a maze with a single solution)
r1p: list = r1[1] + [(gx, gy)]  # add end point (for shortcuts directly to end)
# sum all cheatpoints for start points 100 or more steps from the end (the minimum required gain)
print('Day20-1', sum([cheatpoints(maze, r1p, *r1p[i], i) for i in range(len(r1p)-100)]))
print('Day20-2', sum([cheatpoints_mh(r1p, i) for i in range(len(r1p)-100)]))
