from collections import deque
from itertools import starmap, chain

data = [[y[0],int(y[1]),y[2]] for y in 
        [x.split() for x in 
         open('day18-1.data').readlines()]]

expmap = deque() # we are going to expand this as we go
expmap.append(deque())
expmap[0].append('#') # starting point "0,0"
pos = [0,0]

def buildedge(m:deque, pos:list, edge:list): # m=dig plan, pos=current position, edge=edge data(direction,amount,color)
    np=pos[:]
    dmov = ()
    match edge[0]:
        case 'U':
            np[1]-=edge[1] # y-
            dmov = (0,-1)
        case 'R':
            np[0]+=edge[1] # x+
            dmov = (1,0)
        case 'D':
            np[1]+=edge[1] # y+
            dmov = (0,1)
        case 'L':
            np[0]-=edge[1] # x-
            dmov = (-1,0)
    if np[0] not in range(len(m[0])) or np[1] not in range(len(m)): # do we need to expand dig plan?
        m = expand(m, np)
    # adjust current pos if we have expanded up/left
    if np[0] < 0:
        pos[0] += abs(np[0])
        np[0] = 0
    if np[1] < 0:
        pos[1] += abs(np[1])
        np[1] = 0
    while pos != np:
        pos[0] += dmov[0]
        pos[1] += dmov[1]
        m[pos[1]][pos[0]]='#'
    return m

def expand(m:deque, pos:list):
    if pos[0] < 0: m = deque(map(lambda x:deque(['.']*abs(pos[0])) + x,m)) # expand left
    elif pos[0] >= len(m[0]): m = deque(map(lambda x:x + deque(['.']*abs(pos[0]-len(m[0])+1)),m)) # expand right
    if pos[1] < 0: m = deque([deque(['.']*len(m[0])) for _ in range(abs(pos[1]))]) + m # expand up
    elif pos[1] >= len(m): m = m + deque([deque(['.']*len(m[0])) for _ in range(abs(pos[1]-len(m)+1))]) # expand down
    return m

# breadth-first flood fill (from day 10)
def neigh(coord:tuple, nlist=[(0,-1),(1,0),(-1,0),(0,1)]): return list(starmap(lambda a,b,c=coord: (c[0]+a, c[1]+b),nlist)) # news
def nfilt(ns, m:deque): return list(filter(lambda n:n[0]>=0 and n[1]>=0 and n[0] < len(m[0]) and n[1] < len(m) , ns))
def bflood(c, m, val): 
    ns = set([c])
    while len(ns) > 0:
        c = ns.pop()
        v = m[c[1]][c[0]]
        if v not in [val,'#']: m[c[1]][c[0]] = '#'
        nn = list(filter(lambda n: m[n[1]][n[0]] not in [val,'#'], nfilt(neigh(c), m)))
        ns.update(nn)
    return m

for d in data: expmap = buildedge(expmap, pos, d)
expmap = bflood((len(expmap[0])//2,len(expmap)//2), expmap, '#') # just do naive select of center for now
res = ''.join(chain(*expmap)).count('#')
print('Day18-1',res,res==31171)

# part 2 - entirely different strategy (grr..) Find area of polygon (use shoelace alg.)
# Grow polygon by 0.5 in order to get outer area. This is done by adjusting distances depending on whether start/end corner is outer/inner
area = 0
lturn = 1 # last turn (outer or inner corner)
p1 = (0,0)
for i in range(len(data)):
    edge = data[i]
    nturn = 1 if (int(edge[2][-2]) + 1)%4 == int(data[(i+1)%len(data)][2][-2]) else 0 # calculate next corner (inner/outer)
    dist = int(edge[2][2:-2],16) + lturn + nturn - 1 # increase length if both corners are outer, decrease if both are inner
    match int(edge[2][-2]):
        case 0: p2 = (p1[0]+dist,p1[1]) # x+
        case 1: p2 = (p1[0],p1[1]+dist) # y+
        case 2: p2 = (p1[0]-dist,p1[1]) # x-
        case 3: p2 = (p1[0],p1[1]-dist) # y-
    area += p1[0]*p2[1]-p1[1]*p2[0] # x1*y2 - y1*x2
    p1 = p2
    lturn = nturn

print('Day18-2',abs(area)//2, abs(area)//2 == 131431655002266)