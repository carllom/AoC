def neigh(m, x, y):  # get orthogonal neighbors of x,y (within bounds)
    result = []
    for dp in [(-1, 0), (1, 0), (0, -1), (0, 1)]:
        xn = x + dp[0]
        yn = y + dp[1]
        if 0 <= xn < len(m[0]) and 0 <= yn < len(m):
            result.append((xn, yn))
    return result


def step(m, x, y):  # waik all possible trail paths from x,y
    if m[y][x] == '9':
        return [[(x, y)]]  # end of trail
    n = neigh(m, x, y)
    paths = []
    for xn, yn in n:
        if ord(m[yn][xn]) == ord(m[y][x])+1:  # next step? (cheat and use ascii values)
            for path in step(m, xn, yn):
                if len(path) == 0:  # dead end
                    continue
                paths.append(path + [(x, y)])
    return paths  # list of all possible paths from x,y


data = open('day10.data').read().splitlines()
res1 = []
res2 = []
for y, l in enumerate(data):
    for x, c in enumerate(l):
        if c == '0':
            trail = step(data, x, y)
            res1.append(len(set([t[0] for t in trail])))  # task 1 trail score is number of distinct endpoints for each trailhead
            res2.append(len(trail))  # task 2 trail score is just the number of endpoints for each trailhead. easy peasy.

print('Day10-1', sum(res1))
print('Day10-2', sum(res2))
