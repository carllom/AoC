def neighbors(x, y, outer=False):  # outer = do not take map boundaries into account
    return [c for c in [(x, y-1), (x, y+1), (x-1, y), (x+1, y)] if outer or 0 <= c[0] < len(data[0]) and 0 <= c[1] < len(data)]


def flood(x, y, data):
    c, q, area, peri, memb = data[y][x], set([(x, y)]), 0, 0, set([(x, y)])
    while (len(q) > 0):
        x, y = q.pop()
        if (data[y][x] != c):
            continue
        data[y][x] = c.lower()  # mark as visited
        area += 1
        neigh = list(filter(lambda n: data[n[1]][n[0]] in [c, c.lower()], neighbors(x, y)))  # visited as well
        cand = list(filter(lambda n: data[n[1]][n[0]] == c, neigh))  # not visited
        peri += 4 - len(neigh)  # 4: also count outer edges (outside of map)
        q.update(cand)
        memb.update(cand)
    return area, peri, memb  # memb is for part 2


def addp(a, b):
    return (a[0]+b[0], a[1]+b[1])


def findshell(shell, p):
    return next((s for s in shell if s[0] == p[0] and s[1] == p[1]), None)  # find shell cell with this position


def visitshell(shell: set, p):
    shn = findshell(shell, p)
    if not shn:
        return None
    shell.remove(shn)
    if shn[2] > 1:
        shn = (shn[0], shn[1], shn[2]-1)
        shell.add(shn)
    return shn


data = [[c for c in l] for l in open('day12.data').read().splitlines()]
plots = list()

for y in range(len(data)):
    for x in range(len(data[y])):
        if (not data[y][x].isupper()):
            continue
        plots.append(flood(x, y, data))

print('Day12-1', sum([n[0]*n[1] for n in plots]))

front = [(0, -1), (1, 0), (0, 1), (-1, 0)]  # N, E, S, W
rdiag = [(1, -1), (1, 1), (-1, 1), (-1, -1)]  # NE, SE, SW, NW
tot = 0
for p in plots:
    shell = set()  # generate shell - the contour of the plot
    for c in p[2]:  # the shell is all plot neighbors that are not plot cells
        for n in filter(lambda x: x not in p[2], neighbors(c[0], c[1], True)):
            shell.add((n[0], n[1], sum(x in p[2] for x in neighbors(n[0], n[1]))))  # last value is how many plot cells the shell cell connects to

    while len(shell) > 0:  # walk a shell(contour) edge. We will walk the shell in a clockwise direction with the plot on our right side
        x, y, count = shell.pop()  # select an initial cell
        shell.add((x, y, count))  # always push back initial cell. Dont reduce/remove, we will revisit it later

        dir = next((r for r in range(4) if addp((x, y), front[(r+1) % 4]) in p[2]), 4)  # find initial direction
        x0, y0, dir0 = x, y, dir
        initial_cell = True
        edges = 0
        while len(shell) > 0:
            if not initial_cell and (x, y, dir) == (x0, y0, dir0):
                break  # we are back where we started - edge walk is done
            initial_cell = False

            n = addp((x, y), front[dir])  # get cell ahead of current cell
            # if it is a shell cell and there is a plot cell to the right of it we are on the same edge
            if findshell(shell, n) and addp(n, front[(dir+1) % 4]) in p[2]:
                x, y, count = visitshell(shell, n)  # move to the cell and continue
                continue

            if n in p[2]:  # if next cell is a plot cell we have found an inner corner.
                edges += 1  # Turn counter clockwise and follow a new edge
                x, y, count = visitshell(shell, (x, y))
                dir = (dir + 3) % 4
                continue

            frd = addp((x, y), rdiag[dir])
            if findshell(shell, frd):  # check if we have reached an outer corner (front right is a shell cell)
                edges += 1  # Turn clockwise and follow a new edge
                x, y, count = visitshell(shell, frd)
                dir = (dir + 1) % 4
                continue
        tot += edges * p[0]  # add current loop to edge cost

print('Day12-2', tot)
