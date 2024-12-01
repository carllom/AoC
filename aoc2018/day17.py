import re


def read_input():
    with open('day17-0.txt', 'r') as input:
        return [[t for t in re.split(r'[,= \n]', l) if t] for l in input.readlines()]


def print_grid(grid):
    for l in grid:
        print(''.join(l))


def parse(ldef):
    coord = [0, 0, 0, 0]
    for i in [0, 2]:
        if '.' in ldef[i+1]:
            s = ldef[i+1].split('..')
            coord[0 if ldef[i] == 'x' else 2] = int(s[0])
            coord[1 if ldef[i] == 'x' else 3] = int(s[1])
        else:
            coord[0 if ldef[i] == 'x' else 2] = int(ldef[i+1])
            coord[1 if ldef[i] == 'x' else 3] = int(ldef[i+1])
    return coord


def pour(grid, x0, y0):
    stream = [(y0, x0)]
    while stream:
        y, x = stream[-1]  # peek last element
        if grid[y+1][x] == '.':  # falling
            grid[y][x] = '|'
            stream.append((y+1, x))
            continue
        if grid[y+1][x] == '|':  # already falling
            stream.pop()
            continue
        if grid[y+1][x] == '#':  # hit the ground
            # pour left
            if grid[y][x-1] == '.':
                grid[y][x-1] = '|'
                stream.append((y, x-1))
                continue
            # pour right
            elif grid[y][x+1] == '.':
                grid[y][x+1] = '|'
                stream.append((y, x+1))
                continue
            else:
                stream.pop()
                continue
        continue


def pour2(grid, y, x, y0):
    if y >= len(grid) or grid[y][x] == '#':
        return
    if grid[y][x] == '.':  # falling
        grid[y][x] = '|'
        pour(grid, y+1, x, y0)
    elif grid[y][x] == '|':
        return
    if grid[y+1][x] == '#':
        # pour left
        x1 = x
        while grid[y][x1] != '#':
            x1 -= 1
            if grid[y+1][x1] == '.':
                pour(grid, y, x1, y0)
                break
        # pour right
        x1 = x
        while grid[y][x1] != '#':
            x1 += 1
            if grid[y+1][x1] == '.':
                pour(grid, y, x1, y0)
                break
        # fill the row
        x1 = x
        while grid[y][x1] != '#':
            grid[y][x1] = '~'
            x1 -= 1
        x1 = x
        while grid[y][x1] != '#':
            grid[y][x1] = '~'
            x1 += 1


def pour3(grid, stream):
    y, x = stream[-1]

    # try falling first if possible
    if y+1 < len(grid) and grid[y+1][x] == '.':
        grid[y+1][x] = '|'
        pour3(grid, stream.copy().append((y+1, x)))
    if x+1 < len(grid[0]) and grid[y][x+1] == '.':  # then try right
        grid[y][x+1] = '|'
        pour3(grid, stream.copy().append((y, x+1)))
    if x-1 >= 0 and grid[y][x-1] == '.':
        grid[y][x-1] = '|'
        pour3(grid, stream.copy().append((y, x-1)))


def task1():
    input = read_input()
    parsed = [parse(l) for l in input]
    i0 = parsed[0].copy()  # dimension of the grid
    # print(input[0], i0)
    for i1 in parsed[1:]:
        # print(i0, i1)
        i0[0] = min(i0[0], i1[0])
        i0[1] = max(i0[1], i1[1])
        i0[2] = min(i0[2], i1[2])
        i0[3] = max(i0[3], i1[3])

    grid = [['.' for _ in range(i0[0]-1, i0[1]+1+1)] for _ in range(i0[2]-1, i0[3]+1+1)]
    for i in parsed:
        print(i)
        for y in range(i[2]-i0[2]+1, i[3]-i0[2]+2):
            for x in range(i[0]-i0[0]+1, i[1]-i0[0]+2):
                grid[y][x] = '#'  # clay
    grid[0][500-i0[0]+1] = '+'
    print_grid(grid)

    pour(grid, 500-i0[0]+1, 1)
    print_grid(grid)

    return input


print('Task#1 outcome:', task1())
