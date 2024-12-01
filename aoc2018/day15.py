from collections import deque
from functools import reduce

# --- Day 15: Beverage Bandits ---


class Unit:
    hp: int = 200
    attack: int = 3
    symbol: str = 'G'
    pos: tuple = (0, 0)

    def __init__(self, pos, symbol, attack=3):
        self.hp = 200
        self.attack = attack
        self.pos = pos
        self.symbol = symbol

    def __repr__(self):
        return f'{self.symbol}({self.hp})@{self.pos}'


enemyof = {'G': 'E', 'E': 'G'}


def read_input():
    with open("day15.txt", 'r') as input:
        map2d = [list(list(line.strip())) for line in input.readlines()]
    return map2d


def step(m, start):
    if start is None:
        return None
    sx = len(m[0])
    nx = start[1] + 1
    return (start[0] + (nx//sx), nx % sx)


def neighbors(m, pos):
    return [(pos[0]+x[0], pos[1]+x[1]) for x in [(-1, 0), (0, -1), (0, 1), (1, 0)]]


def firstmatch(m, match, start=(1, 1)):
    x0 = start[1]
    for yi in range(start[0], len(m)-1):
        for xi in range(x0, len(m[0])-1):
            x0 = 0
            if m[yi][xi] in match:
                return [(yi, xi), m[yi][xi]]
    return None


def allmatches(m, match):
    matches = []
    for yi in range(1, len(m)-1):
        for xi in range(1, len(m[0])-1):
            if m[yi][xi] in match:
                matches.append([(yi, xi), m[yi][xi]])
    return matches


def closestpath(m, start, end):
    if start == end:
        return [start]

    queue = deque([(start, [start])])
    visited = set()
    visited.add(start)

    while queue:
        (current, path) = queue.popleft()

        for dy, dx in [(-1, 0), (0, -1), (0, 1), (1, 0)]:
            neighbor = (current[0] + dy, current[1] + dx)

            if neighbor == end:
                return path + [neighbor]

            if (0 <= neighbor[0] < len(m) and
                0 <= neighbor[1] < len(m[1]) and
                neighbor not in visited and
                    m[neighbor[0]][neighbor[1]] == '.'):

                queue.append((neighbor, path + [neighbor]))
                visited.add(neighbor)

    return None


def victim(m, units, unit, enemies):
    victim = None
    for enemy in sorted(enemies, key=lambda u: u[0]):
        if enemy[0] in neighbors(m, unit[0]):
            u = unitat(units, enemy[0])
            if victim is None or u.hp < victim.hp:
                victim = u
    return victim


def adjacentto(m, unit, enemies):
    for enemy in enemies:
        if enemy[0] in neighbors(m, unit[0]):
            return enemy
    return None


def dump(m):
    for row in m:
        print(''.join(row))


def unitat(units, pos):
    return next((u for u in units if u.pos == pos), None)


def tryattack(m, units, unit, enemies):
    # am I adjacent to an enemy?
    adj = victim(m, units, unit, enemies)  # adjacentto(m, unit, enemies)
    if adj is not None:
        # attack
        atk = unitat(units, unit[0])
        trg = adj  # unitat(units, adj[0])
        trg.hp -= atk.attack
        if trg.hp <= 0:
            m[trg.pos[0]][trg.pos[1]] = '.'
            units.remove(trg)
        return True
    return False


def move(m, units, unit, enemies):
    closest = None
    for enemy in enemies:  # find the closest enemy
        path = closestpath(m, unit[0], enemy[0])
        if path is not None and (closest is None or len(path) < len(closest)):
            closest = path

    if closest is not None:  # move towards closest enemy
        u = unitat(units, unit[0])
        m[unit[0][0]][unit[0][1]] = '.'
        m[closest[1][0]][closest[1][1]] = unit[1]
        unit = (closest[1], unit[1])
        u.pos = unit[0]

    return unit


def battle(m, units):
    round = 0
    while len(set([u.symbol for u in units])) > 1:  # while there still are are goblins and elves (2 different types)
        round += 1
        for u in sorted(units, key=lambda u: u.pos):
            if u not in units:
                continue  # unit was killed before its turn
            unit = (u.pos, u.symbol)

            enemies = allmatches(m, enemyof[unit[1]])  # find all enemies for this unit
            if len(enemies) == 0:
                continue

            if tryattack(m, units, unit, enemies):  # attack adjacent enemy if possible
                continue

            unit = move(m, units, unit, enemies)  # move towards closest enemy

            if tryattack(m, units, unit, enemies):  # try attacking again after move
                continue

    return round


def task1():
    m = read_input()
    units = [Unit(pos=u[0], symbol=u[1]) for u in allmatches(m, 'GE')]
    rounds = battle(m, units)
    return reduce(lambda x, y: x + y.hp, units, 0) * (rounds - 1)


def task2():
    ehit = 3  # elf attack points
    while True:
        m = read_input()
        units = [Unit(pos=u[0], symbol=u[1], attack=3 if u[1] == 'G' else ehit) for u in allmatches(m, 'GE')]
        nelves = len([u for u in units if u.symbol == 'E'])
        rounds = battle(m, units)
        if len([u for u in units if u.symbol == 'E']) == nelves:
            break
        ehit += 1

    return reduce(lambda x, y: x + y.hp, units, 0) * (rounds - 1)


print('Task#1 outcome:', task1())
print('Task#2 outcome:', task2())
