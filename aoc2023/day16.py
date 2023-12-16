from collections import deque
from itertools import chain

data = open('day16-1.data').read().splitlines()
xrng = range(len(data[0])) # mirrormap bounds
yrng = range(len(data))
dirs = { 0:(0,-1),1:(1, 0),2:(0,1),3:(-1,0) }  # maps direction to movement (nesw) => (xd,yd)
slash = { 0:1,1:0,2:3,3:2 } # direction change maps per symbol
bslash = { 0:3,1:2,2:1,3:0 }
vpipe = { 0:[0],1:[0,2],2:[2],3:[0,2] }
hpipe = { 0:[1,3],1:[1],2:[1,3],3:[3] }

def calcdir(dir,sym):
    match sym:
        case '.': return [dir]
        case '/': return [slash[dir]]
        case '\\': return [bslash[dir]]
        case '|': return vpipe[dir]
        case '-': return hpipe[dir]

def move(beam,dir):
    d = dirs[dir]
    return [beam[0]+d[0],beam[1]+d[1],dir]

def step(beams, m, nrg):
    b = beams.popleft() # pop next beam
    for ndir in calcdir(b[2],m[b[1]][b[0]]): # calc new direction(s if split)
        bn = move(b,ndir) # move beam to next pos
        if bn[0] in xrng and bn[1] in yrng: # beam still inside map?
            if not nrg[bn[1]][bn[0]][0]:
                nrg[bn[1]][bn[0]] = [True,[ndir]] # energize tile
            elif ndir not in nrg[bn[1]][bn[0]][1]:
                nrg[bn[1]][bn[0]][1].append(ndir)
            else:
                bn = None # we have been here before with the same direction - do not re-add
            if bn: beams.append(bn) # re-add beam

def calc(beams, m):
    nrg = list(map(lambda x:[[False,[]]]*len(m[0]),m)) # map for visited locations
    nrg[beams[0][1]][beams[0][0]]=[True,[beams[0][2]]] # energize entry point
    while beams: step(beams, m, nrg)
    return sum(map(lambda x: x[0],chain.from_iterable(nrg)))

beams = deque([[0,0,1]]) # x,y,dir
print('Day16-1',calc(beams,data))

print('Day16-2', max(chain(
    map(lambda y:calc(deque([[0,y,1]]),data),yrng),
    map(lambda y:calc(deque([[max(xrng),y,3]]),data),yrng),
    map(lambda x:calc(deque([[x,0,2]]),data),xrng),
    map(lambda x:calc(deque([[x,max(yrng),0]]),data),xrng)
)))