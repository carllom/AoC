from itertools import groupby
from math import prod
data = [[tuple(map(int, x.split(','))), 0] for x in open('day8-1.data', encoding='utf-8')]


def eucl_dist(a, b):
    return ((a[0]-b[0])**2 + (a[1]-b[1])**2 + (a[2]-b[2])**2)**0.5


def connect(c1, c2, ncirc) -> int:
    if c2[1] == 0 and c1[1] == 0:  # neither is connected - create new circuit
        c1[1] = ncirc
        c2[1] = ncirc
        ncirc += 1
    elif c1[1] == c2[1]:  # same circuit already
        pass
    elif c1[1] != 0 and c2[1] == 0:  # add c2 to c1's circuit
        c2[1] = c1[1]
    elif c2[1] != 0 and c1[1] == 0:  # add c1 to c2's circuit
        c1[1] = c2[1]
    else:  # both connected to different circuits - merge
        for crd in [c for c in data if c[1] == c2[1]]:
            crd[1] = c1[1]
    return ncirc


distlist = {}
for i, coord in enumerate(data[:-1]):  # build distance map
    for other in data[(i+1):]:
        d = eucl_dist(coord[0], other[0])
        distlist[d] = (coord, other)

ncirc, nconn, maxconn = 1, 0, 1000
for d, (c1, c2) in sorted(distlist.items(), key=lambda x: x[0]):
    if nconn >= maxconn:
        break
    nconn += 1
    ncirc = connect(c1, c2, ncirc)

glist = [len(list(g)) for k, g in groupby(sorted(data, key=lambda x: x[1]), key=lambda x: x[1]) if k != 0]  # count the number of members in each group
print('Day8-1:', prod(sorted(glist, reverse=True)[:3]))

for d in data:
    d[1] = 0  # reset connections
ncirc, nconn = 1, 0
for d, (c1, c2) in sorted(distlist.items(), key=lambda x: x[0]):
    nconn += 1
    ncirc = connect(c1, c2, ncirc)
    if len({crd[1] for crd in data}) == 1 and nconn >= maxconn:
        print('Day8-2:', c1[0][0]*c2[0][0])
        break
