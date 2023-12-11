from itertools import starmap
import numpy as np

pseg = { # segment endpoint directions
    '|': [(0,-1), (0,1)], # n s
    '-': [(-1,0), (1,0)], # w e
    'F': [ (0,1), (1,0)], # s e 
    '7': [ (0,1),(-1,0)], # s w
    'L': [(0,-1), (1,0)], # n e
    'J': [(0,-1),(-1,0)]  # n w
}

def get_m(map, c): return map[c[1]][c[0]] # get map value

def set_m(map, c,v): map[c[1]][c[0]] = v # set map value

def goesto(c0): return neigh(c0, pseg[get_m(m, c0)]) # 

def goesnext(csrc, cdest): return list(filter(lambda d:d!=csrc,goesto(cdest)))[0]

def neigh(coord:tuple, nlist=[(0,-1),(1,0),(-1,0),(0,1)]): 
    return list(starmap(lambda a,b,c=coord: (c[0]+a, c[1]+b),nlist)) # news

def findseg(cell):
    for y,row in enumerate(m):
        try: x = row.index(cell)
        except: x = -1
        if x>=0: return (x,y)
    return (-1,-1)

m = list(map(list,open('day10-1.data').read().splitlines()))
coord = findseg('S')
sn = list(filter(lambda n: coord in goesto(n), neigh(coord))) # determine start segment endpoint neighbors

s1 = coord
s2 = coord
d1 = sn[0]
d2 = sn[1]
steps = 1
set_m(m, coord,'#') # draw path for part 2 - start segment
while d1!= d2:
    d1n = goesnext(s1,d1)
    set_m(m, d1,'#') # draw path for part 2 - ccw seg
    d2n = goesnext(s2,d2)
    set_m(m, d2,'#') # draw path for part 2 - cw seg
    steps +=1
    if d1n == d2n:
        set_m(m, d1n,'#') # draw path for part 2 - distant seg
        break
    s1 = d1
    s2 = d2
    d1 = d1n
    d2 = d2n

print('Day10-1:', steps)

def nfilt(ns, map=m): 
    return list(filter(lambda n:n[0]>=0 and n[1]>=0 and n[0] < len(map[0]) and n[1] < len(map) , ns))

def bflood(c, map, val): # breadth-first flood fill
    ns = set([c])
    # flood fill outside
    while len(ns) > 0:
        c = ns.pop()
        v = get_m(map, c)
        if v not in [val,'#']: set_m(map,c,val)
        nn = list(filter(lambda n: get_m(map, n) not in [val,'#'], nfilt(neigh(c), map)))
        ns.update(nn)

# clear all non-path-segments
for y,l in enumerate(m):
    for x,c in enumerate(l):
        if c != '#': m[y][x]=' '

mp = m # mp = processed map
m = list(map(list,open('day10-1.data').read().splitlines())) # original map

s = coord
d = sn[0] # choose direction
while d != coord:
    dn = goesnext(s,d)
    delt =  tuple(np.subtract(dn, d))
    ccw = (delt[1],-delt[0]) # rotate ccw
    ccwn = tuple(np.add(d,ccw)) # ccw neighbor
    if ccwn !=s and mp[ccwn[1]][ccwn[0]] == ' ': bflood(ccwn, mp,'@') # flood fill empty cell to the left
    if ccwn != s and get_m(m,d) in '7JFL': # outer corner: rotate twice
        ccw = (ccw[1],-ccw[0])
        ccwn = tuple(np.add(d,ccw))
        if mp[ccwn[1]][ccwn[0]] == ' ': bflood(ccwn, mp,'@') # flood fill empty cell behind
    s = d
    d = dn

print('Day10-2:',sum([l.count('@') for l in mp])) # count inside(@) cells
