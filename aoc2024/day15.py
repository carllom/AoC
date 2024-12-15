def compress(ray: list[str]):
    if not '.' in ray:  # no empty space - cannot compress
        return (ray, -1)
    wi = ray.index('#')  # first wall
    cp = ray.index('.')  # compression point (first empty space)
    if cp < wi:  # compression point must be before wall
        ray = ray[:cp] + ray[cp+1:]  # compress
    return (ray, cp)


def recursive_compress(wh, rx, ry, d, visited: set):
    dx, dy = dirs[d]
    rayx, rayy = rx, ry
    ray = []  # get sequence of positions in movement direction
    while 0 <= rayx < len(wh[0]) and 0 <= rayy < len(wh):
        ray.append(wh[rayy][rayx])
        rayx, rayy = rayx+dx, rayy+dy

    cray, cp = compress(ray)
    if cray == ray:
        return False  # could not compress

    for i in range(cp-1, -1, -1):  # iteerate over compressed(moved) positions
        visited.add((rx+(dx*i), ry+(dy * i)))  # mark positions to update
        if d in ['^', 'v'] and cray[i] == '[':  # Task#2 - try compress right half of box
            if (rx+1, ry+(dy * i)) in visited:  # We have already checked this position
                continue
            if not recursive_compress(wh, rx+1, ry+(dy * i), d, visited):
                return False
        if d in ['^', 'v'] and cray[i] == ']':  # Task#2 - try compress left half of box
            if (rx-1, ry+(dy * i)) in visited:  # We have already checked this position
                continue
            if not recursive_compress(wh, rx-1, ry+(dy * i), d, visited):
                return False
    return True


def do_moves(wh, moves, rx, ry):
    for m in moves:
        dx, dy = dirs[m]  # direction delta
        nx, ny = rx+dx, ry+dy  # next position
        if wh[ny][nx] == '#':
            nx, ny = rx, ry
        elif wh[ny][nx] in ['O', '[', ']']:  # there is a box in next position
            visited = set()
            if not recursive_compress(wh, nx, ny, m, visited):
                nx, ny = rx, ry  # no compression
            else:
                for (x, y) in sorted(visited, key=lambda pos: pos[0 if m in ['<', '>'] else 1], reverse=m in [
                        '>', 'v']):  # sort update order so we do not overwrite when moving boxes
                    wh[y+dy][x+dx] = wh[y][x]
                    wh[y][x] = '.'
        wh[ny][nx] = '.'  # clear robot position
        rx, ry = nx, ny
    return wh


def calc_gps(wh):
    return sum([100*y+x for y in range(len(wh)) for x in range(len(wh[y])) if wh[y][x] in ['O', '[']])


dirs = {  # movement directions
    '^': (0, -1),
    'v': (0, 1),
    '<': (-1, 0),
    '>': (1, 0)
}
moves = ''
x0, y0 = -1, -1
wh = []
section = 0
for l in open('day15.data').read().splitlines():
    if (l == ''):
        section = 1  # empty line - end of warehouse map
    if section == 0 and l != '':
        if '@' in l:
            x0, y0 = l.index('@'), len(wh)  # robot start position
        wh.append(list(l))
        continue
    moves = moves + l
wh[y0][x0] = '.'  # clear robot symbol
print('Day15-1', calc_gps(do_moves(wh, moves, x0, y0)))

wh = []  # reset warehouse map
for l in open('day15.data').read().splitlines():
    if l == '':
        break  # just read first section - moves are the same for both tasks
    wh.append(list(l.replace('#', '##').replace('.', '..').replace('@', '@.').replace('O', '[]')))  # double width
wh[y0][x0*2] = '.'  # clear robot symbol
print('Day15-2', calc_gps(do_moves(wh, moves, x0*2, y0)))
