from re import findall
robots = [list(map(int, findall(r'(-?\d+)', l))) for l in open('day14.data').read().splitlines()]
w, h = 101, 103
f, g = w//2, h//2
for s in range(100):
    for i in range(len(robots)):
        robots[i] = [(w + robots[i][0] + robots[i][2]) % w, (h + robots[i][1] + robots[i][3]) % h, robots[i][2], robots[i][3]]
q1 = len(list(filter(lambda r: r[0] < f and r[1] < g, robots)))
q2 = len(list(filter(lambda r: r[0] > f and r[1] < g, robots)))
q3 = len(list(filter(lambda r: r[0] < f and r[1] > g, robots)))
q4 = len(list(filter(lambda r: r[0] > f and r[1] > g, robots)))
print('Day14-1', q1*q2*q3*q4)


def part_of_line(s, r):
    for x in range(4):  # find a horizontal line of at least 9 robots in a row - this is the smallest number that gives a correct answer...
        if (r[0] + x, r[1]) not in s or (r[0] - x, r[1]) not in s:
            return False
    return True


robots = [list(map(int, findall(r'(-?\d+)', l))) for l in open('day14.data').read().splitlines()]
time = 0
while True:
    s = set([(r[0], r[1]) for r in robots])
    line = False
    for r in robots:
        if part_of_line(s, r):  # check if there is a line of robots - such a dumb solution...
            line = True
            break
    if line:
        break

    for i in range(len(robots)):
        robots[i] = [(w + robots[i][0] + robots[i][2]) % w, (h + robots[i][1] + robots[i][3]) % h, robots[i][2], robots[i][3]]
    time += 1

m = [['.']*w for _ in range(h)]  # print the map
for r in robots:
    m[r[1]][r[0]] = '#'
for i in range(h):
    print(''.join(m[i]))

print('Day14-2', time)
