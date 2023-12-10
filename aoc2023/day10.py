from itertools import starmap
import numpy as np

s=0

# news
pseg = {
    '|': [(0,-1), (0,1)], # n s
    '-': [(-1,0), (1,0)], # w e
    'F': [ (0,1), (1,0)], # s e 
    '7': [ (0,1),(-1,0)], # s w
    'L': [(0,-1), (1,0)], # n e
    'J': [(0,-1),(-1,0)],  # n w
    '.': []
}

next = {
    (0,-1): ['F','|','7'],
    (0,1): ['L','|','J'],
    (-1,0): ['F','-','L'],
    (1,0): ['7','-','J']
}

def mat(map, c): return map[c[1]][c[0]]

def mset(map, c,v):
    map[c[1]][c[0]] = v

def goesto(c0): return neigh(c0, pseg[mat(m, c0)])

def goesnext(csrc, cdest): return list(filter(lambda d:d!=csrc,goesto(cdest)))[0]

def neigh(coord:tuple, nlist=[(0,-1),(1,0),(-1,0),(0,1)]): 
    return list(starmap(lambda a,b,c=coord: (c[0]+a, c[1]+b),nlist)) # news

def findseg(cell):
    for y,row in enumerate(m):
        try:
            x = row.index(cell)
        except:
            x = -1
        if x>=0: return (x,y)
    return (-1,-1)

m = list(map(list,open('day10-1.data').read().splitlines()))
coord = findseg('S')
sn = list(filter(lambda n: coord in goesto(n), neigh(coord)))

s1 = coord
s2 = coord
d1 = sn[0]
d2 = sn[1]
steps = 1
mset(m, coord,'#') # for part 2
while d1!= d2:
    # print(s1, mat(s1),d1, mat(d1),s2, mat(s2),d2,mat(d2),steps)
    d1n = goesnext(s1,d1)
    mset(m, d1,'#') # for part 2
    d2n = goesnext(s2,d2)
    mset(m, d2,'#') # for part 2
    steps +=1
    if d1n == d2n:
        mset(m, d1n,'#') # for part 2
        break
    s1 = d1
    s2 = d2
    d1 = d1n
    d2 = d2n

print('Day10-1:', steps)


def nfilt(ns, map=m):
    return list(filter(lambda n:n[0]>=0 and n[1]>=0 and n[0] < len(map[0]) and n[1] < len(map) , ns))

def bflood(c, map=m, val='_'):
    ns = set([c])
    # flood fill outside
    while len(ns) > 0:
        c = ns.pop()
        v = mat(map, c)
        if v not in [val,'#']:
            mset(map,c,val)
        nn = list(filter(lambda n: mat(map, n) not in [val,'#'], nfilt(neigh(c), map)))
        ns.update(nn)









# clear all non-path-segments
for y,l in enumerate(m):
    for x,c in enumerate(l):
        if c != '#': m[y][x]='_'

for l in m: print(''.join(l))

mp = m
m = list(map(list,open('day10-1.data').read().splitlines())) # original map

coord = findseg('S')
sn = list(filter(lambda n: coord in goesto(n), neigh(coord)))[0]

s = coord
d = sn
while d1!= d2:
    dn = goesnext(s,d)
    delt =  tuple(np.subtract(dn, d))
    ccw = (delt[1],-delt[0])
    ccwn = tuple(np.add(d,ccw))
    # print(s, mat(s),d, mat(d), dn, mat(dn), delt, ccw)
    if mp[ccwn[1]][ccwn[0]] == '_':
        bflood(ccwn, mp,'@')

    if mat(mp,d) in '7JFL':
        ccw = (ccw[1],-ccw[0])
        ccwn = tuple(np.add(d,ccw))
        if mp[ccwn[1]][ccwn[0]] == '_':
            bflood(ccwn, mp,'@')

    if dn == coord:
        break
    s = d
    d = dn

for lp in mp: print(''.join(lp))

s=0
for l in mp: s += l.count('@')

print('Day10-2:',s)

# def nfilt(ns, map=m):
#     return list(filter(lambda n:n[0]>=0 and n[1]>=0 and n[0] < len(map[0]) and n[1] < len(map) , ns))

# -1 0 => 0 1   x=y y=-x
#  0 1 => 1 0   x=y y=-x
#  1 0 => 0 -1  x=y y=-1

# def bflood(c, map=m, val='_'):
#     ns = set([c])
#     # flood fill outside
#     while len(ns) > 0:
#         c = ns.pop()
#         v = mat(c)
#         if v not in [val,'#']:
#             mset(c,val)
#         nn = list(filter(lambda n: mat(n) not in [val,'#'], nfilt(neigh(c), map)))
#         ns.update(nn)

def flood(c, map=m):
    v = mat(map, c)
    if v == '_' or v == '#': return
    mset(map,c,'_')
    for nc in nfilt(neigh(c), map):
        flood(nc)

bflood((0,0), m)

m0 = list(map(list,open('day10-1.data').read().splitlines())) # original map
notogl = {
    '|': '',
    '-': '-7J',
    'F': '-7L',
    '7': '',
    'L': '-7J',
    'J': '',
    'S': ''
}

cin = 0 
for y,l in enumerate(m):
    # print(''.join(l))
    cp = '_'
    c0p = '_'
    inside = False
    for x,c in enumerate(l):
        c0 = m0[y][x]
        if c == '_': inside = False
        elif c == '#':
            if cp != '#' or c0 not in notogl[c0p]: inside = not inside
            m0[y][x] = 'i' if inside else 'o'
        elif not inside:
            l[x] = '_'
        elif inside:
            l[x] = '@'
            cin += 1
        cp = c
        c0p = c0
    # print(''.join(l))

# for l in m0: print(''.join(l))

print('Day10-2:', cin)

# 321 too large?
