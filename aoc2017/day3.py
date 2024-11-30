dir = [(1, 0), (0, 1), (-1, 0), (0, -1)]
x = 0
y = 0
turn = 0
seglen = 1
dist = 0
target = 312051


def nextpos(x, y, turn):
    return x + dir[turn][0], y + dir[turn][1]


while dist < target:
    for i in range(seglen):
        dist += 1
        if dist == target:
            print('Outcome task#1', abs(x) + abs(y))
        x += dir[turn][0]
        y += dir[turn][1]
    turn = (turn + 1) % 4
    if turn % 2 == 0:
        seglen += 1

ack = 0
visited = {(0, 0): 1}
x = 0
y = 0
turn = 0
seglen = 1
dist = 0
target = 312051
while ack <= target:
    for i in range(seglen):
        dist += 1
        x, y = nextpos(x, y, turn)
        neighs = [visited.get((x+i, y+j), 0) for i in range(-1, 2) for j in range(-1, 2)]
        ack = sum(neighs)
        visited[(x, y)] = ack
        if ack > target:
            print('Outcome task#2:', visited[(x, y)])
            break
    turn = (turn + 1) % 4
    if turn % 2 == 0:
        seglen += 1
