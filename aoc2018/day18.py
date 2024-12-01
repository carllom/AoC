def read_input():
    with open("day18.txt", 'r') as input:
        map2d = [list(list(line.strip())) for line in input.readlines()]
    return map2d


def neigh(map2d, x, y):
    count = {'#': 0, '.': 0, '|': 0}
    for dy in range(-1, 2):
        for dx in range(-1, 2):
            if dx == 0 and dy == 0:
                continue
            if x + dx < 0 or x + dx >= len(map2d[0]):
                continue
            if y + dy < 0 or y + dy >= len(map2d):
                continue
            count[map2d[y + dy][x + dx]] += 1
    return count


def countall(map2d):
    count = {'#': 0, '.': 0, '|': 0}
    for row in map2d:
        for c in row:
            count[c] += 1
    return count


def iter(map2d):
    next = [l.copy() for l in map2d]
    for y in range(len(map2d)):
        for x in range(len(map2d[y])):
            n = neigh(map2d, x, y)
            if map2d[y][x] == '.' and n['|'] >= 3:
                next[y][x] = '|'
            if map2d[y][x] == '|' and n['#'] >= 3:
                next[y][x] = '#'
            if map2d[y][x] == '#' and (n['#'] == 0 or n['|'] == 0):
                next[y][x] = '.'
    return next


def task1():  # calculate worth after 10 minutes
    map2d = read_input()
    for i in range(10):  # 10 minutes
        map2d = iter(map2d)
    count = countall(map2d)
    return count['#'] * count['|']


def task2():  # find cyclic behaviour and then calculate the worth after 1000000000 minutes
    map2d = read_input()

    for i in range(500):
        map2d = iter(map2d)  # prime for the cycle

    cycle = countall(map2d)
    size = 1
    while True:  # find cycle size
        map2d = iter(map2d)
        newcount = countall(map2d)
        if newcount == cycle:
            break
        size += 1

    for i in range((1000000000-500) % size):  # find cycle offset of final iteration
        map2d = iter(map2d)

    count = countall(map2d)
    return count['#'] * count['|']


print('Task#1 outcome', task1())
print('Task#2 outcome', task2())
