import heapq
maze = [list(l) for l in open('day16.data').read().splitlines()]
dir = [(0, -1), (-1, 0), (0, 1), (1, 0)]  # N, W, S, E
tcost = [0, 1000, 2000, 1000]  # turn costs abs(current-next) (index 3 is clockwise from N(0) to E(3)
rx, ry, rdir = 1, len(maze)-2, 3  # cheat a bit, we know the raindeer is in the bottom left corner facing east


def bfs(maze, rx, ry, rdir):  # implement a breadth first search - rotations cost 1000 per 90 degree turn, moving cost 1. Find the lowest cost path to the cell containing 'E'
    q = []
    heapq.heappush(q, (0, rx, ry, rdir))
    visited = set()
    while q:
        c, x, y, d = heapq.heappop(q)
        if (x, y, d) in visited:  # we have already been here - implicitcly with a lower or equal cost because of the priority queue
            continue
        visited.add((x, y, d))
        if maze[y][x] == 'E':  # at the end tile
            return c
        for i in range(4):  # try all directions
            nx, ny = x+dir[i][0], y+dir[i][1]
            if maze[ny][nx] != '#' and (abs(i-d) != 2 or c == 0):  # not a wall and not turning around, unless it is the first move
                heapq.heappush(q, (c+1 if i == d else c+1+tcost[abs(i-d)], nx, ny, i))
    return -1


print('Day16-1', bfs(maze, rx, ry, rdir))


def bfs2(maze, rx, ry, rdir):  # in order to get all paths with the same best cost, we need to modify the bfs function a bit.
    q = []  # queue with robot position, direction and cost
    heapq.heappush(q, (0, rx, ry, rdir, set()))  # cost, x, y, direction, path, visited
    visited = set()
    best_visited = set()
    while q:
        c, x, y, d, p = heapq.heappop(q)
        if len({v for v in visited if v[0] == x and v[1] == y and v[2] == d and v[3] < c}) > 0:  # We have included cost in the visited set.
            continue  # The tile has already been visited with a _lower_ cost, we do not need to visit it again
        visited.add((x, y, d, c))  # The tile is unvisited or has been visited with a _higher or equal_ cost, we can visit it again to find another best path
        p.add((x, y))
        if maze[y][x] == 'E':  # at the end tile
            best_visited.update(p)  # add visited positions to the result
            continue
        for i in range(4):  # try all directions
            nx, ny = x+dir[i][0], y+dir[i][1]
            if maze[ny][nx] != '#' and (abs(i-d) != 2 or c == 0):  # not a wall and not turning around, unless it is the first move
                heapq.heappush(q, (c+1+tcost[abs(i-d)], nx, ny, i, p.copy()))
    return best_visited


print('Day16-2', len(bfs2(maze, rx, ry, rdir)))  # number of best solutions, number of unique tiles
