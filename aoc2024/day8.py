import itertools
from collections import defaultdict


def antinode(antinodes, a, b):
    n = (2*b[0] - a[0], 2*b[1] - a[1])  # extrapolate new node a->b->n with b as midpoint
    if 0 <= n[0] < len(data[0]) and 0 <= n[1] < len(data):  # is n within bounds?
        antinodes.add(n)


def resonance(antinodes, a, b):
    while 0 <= b[0] < len(data[0]) and 0 <= b[1] < len(data):  # while b is within bounds
        antinodes.add(b)  # initial condition is to add b input! this represents the node at distance 0
        n = (2*b[0] - a[0], 2*b[1] - a[1])  # extrapolate new node a->b->n with b as midpoint
        a = b
        b = n


data = open('day8-1.data').read().splitlines()
adict = defaultdict(set)
for i in [(data[y][x], (x, y)) for y in range(len(data))for x in range(len(data[y])) if data[y][x] != '.']:
    adict[i[0]].add(i[1])

anti = set()
for k, v in adict.items():
    for comb in list(itertools.combinations(v, 2)):
        antinode(anti, comb[0], comb[1])
        antinode(anti, comb[1], comb[0])

print('Day8-1', len(anti))

for k, v in adict.items():
    for comb in list(itertools.combinations(v, 2)):
        resonance(anti, comb[0], comb[1])
        resonance(anti, comb[1], comb[0])

print('Day8-2', len(anti))
