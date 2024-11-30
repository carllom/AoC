steps = open('day11.txt').read().strip().split(',')
# \ q axis
# / r axis
cube_directions = {
    'n':  (1, -1),
    'ne': (1, 0),
    'se': (0, 1),
    's':  (-1, 1),
    'sw': (-1, 0),
    'nw': (0, -1),
}


def axial_to_cube(hex):
    x = hex[0]
    z = hex[1]
    y = -x-z
    return x, y, z


def cube_distance(a, b):
    return max(abs(a[0] - b[0]), abs(a[1] - b[1]), abs(a[2] - b[2]))


def hex_distance(a, b):
    ac = axial_to_cube(a)
    bc = axial_to_cube(b)
    return cube_distance(ac, bc)


pos = (0, 0)
maxdist = 0

for step in steps:
    x, y = cube_directions[step]
    pos = pos[0] + x, pos[1] + y
    dist = hex_distance((0, 0), pos)
    if dist > maxdist:
        maxdist = dist

print('Outcome task#1', hex_distance((0, 0), pos))
print('Outcome task#2', maxdist)
