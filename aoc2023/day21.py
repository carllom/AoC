from itertools import starmap

orig = [list(l) for l in open('day21-1.data').read().splitlines()]
yrng = range(len(orig))
xrng = range(len(orig[0]))
ymax = yrng.stop-1
xmax = xrng.stop-1

def clone(m:list): return [l[:] for l in m]
def neigh(coord:tuple, nlist=[(0,-1),(1,0),(-1,0),(0,1)]): return list(starmap(lambda a,b,c=coord: (c[0]+a, c[1]+b),nlist)) # news
def nfilt(ns): return [n for n in ns if n[0] in xrng and n[1] in yrng]
def whereis(c:str,m:list) -> list:
    for y,l in enumerate(m):
        for x,s in enumerate(l):
            if s == c: return [x,y]
def evalstep(m:list) -> list:
    r = clone(orig)
    sc = whereis('S',orig)
    r[sc[0]][sc[1]] = '.'
    for x in xrng:
        for y in yrng:
            if m[y][x] != 'O': continue
            for n in nfilt(neigh((x,y))):
                if r[n[1]][n[0]] in ['.','S','O']: r[n[1]][n[0]] = 'O'
    return r

m = clone(orig)
sc = whereis('S',m)

m[sc[0]][sc[1]] = 'O'
for i in range(64):
    m = evalstep(m)

print('Day21-1',sum([''.join(l).count('O') for l in m]))

def evalplots(steps:int, start:list) -> int:
    m = clone(orig)
    sc = whereis('S',m)
    m[sc[0]][sc[1]] = '.'
    m[start[1]][start[0]] = 'O'
    for _ in range(steps):
        m = evalstep(m)
    return sum([''.join(l).count('O') for l in m])
    
nsteps = 26501365

plt_full_even = 7770 # plot count at nsteps for tiles starting on even steps 
plt_full_odd = 7627 # plot count at nsteps for tiles starting on odd steps

numtiles = (nsteps - (65)) // xrng.stop # how far out has the diamond corners stretched in terms of tiles
newsteps = (nsteps - (65)) % xrng.stop # partial tile? (no - mod is 0)

eventiles = 4 * numtiles//2 # number of up/down/left/right tiles starting on an even step
oddtiles = 4 * (numtiles//2 -1) # number of up/down/left/right tiles starting on an odd step. last tile is not completely full (-1)

centertileplots = plt_full_odd # odd number of steps

# full u/d/l/r tiles
oddtileplots = oddtiles * plt_full_odd
eventileplots = eventiles * plt_full_even

#diagonal tiles
d_oddtiles = 0
d_eventiles = 0
for o in range(numtiles-2,0,-2):
    d_oddtiles += o-1
    d_eventiles += o
d_oddtiles *= 4
d_eventiles *= 4

# full diagonal tiles
d_oddtileplots = d_oddtiles * plt_full_odd
d_eventileplots = d_eventiles * plt_full_even

fullplots = centertileplots + oddtileplots + eventileplots + d_oddtileplots + d_eventileplots

totalplots = fullplots

# calculate edge (partially filled tiles)
sc = whereis('S',orig)
steps = 131-1 # -1 because we begin with a single start plot => one step into this tile
left = evalplots(steps,[0,sc[1]]) # left corner
right = evalplots(steps,[xmax,sc[1]]) # right corner
lower = evalplots(steps,[sc[1],0]) # lower corner
upper = evalplots(steps,[sc[0],ymax]) # upper
steps-= 66 # outer edge diagonal tiles are 66 steps behind
dtest0 = evalplots(steps,[0,0]) # upper left
dtest1 = evalplots(steps,[xmax,0]) # upper right
dtest2 = evalplots(steps,[0,ymax]) # lower left
dtest3 = evalplots(steps,[xmax,ymax]) # lower right
steps+= xrng.stop # inner edge diagonal tiles are 131 tiles ahead of outer
dtest02 = evalplots(steps,[0,0]) # upper left
dtest12 = evalplots(steps,[xmax,0]) # upper right
dtest22 = evalplots(steps,[0,ymax]) # lower left
dtest32 = evalplots(steps,[xmax,ymax]) # lower right
dedgeo = numtiles * (dtest0+dtest1+dtest2+dtest3) # number of outer diagonal tiles for edge of the diamond 
dedgei = (numtiles-1) * (dtest02+dtest12+dtest22+dtest32) # number of inner diagonal tiles for one edge of the diamond 

edge = left+right+lower+upper+dedgei+dedgeo

totalplots += edge

print('Day21-2:',totalplots)

# Debugging
#
# m = clone(orig)
# sc = whereis('S',m)
# m[sc[0]][sc[1]] = '.'
# #m[sc[0]][0] = 'O'
# m[0][0] = 'O'
# # m[ymax][xmax] = 'O'
# for i in range(65*5):
#     m = evalstep(m)
#     pl = sum([''.join(l).count('O') for l in m])
#     os.system('cls')
#     for l in m: print(''.join(l))
#     print('iter:',i+2,', plots:',pl,', diag:',i+1-65)
#     if (i+1) % 65 == 0: time.sleep(3.0)
#     if (65*4) - (i+1) <= 0: time.sleep(5)
#     else: time.sleep(0.1)
