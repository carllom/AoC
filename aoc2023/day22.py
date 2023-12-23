import heapq

bricks = [[list(map(int,d.split(','))) for d in c] for c in (l.split('~') for l in open('day22-1.data').read().splitlines())] # x,y horiz, z vert
bdict = dict() # id => brick lookup
for i,b in enumerate(bricks): 
    bricks[i] = b + [i,set(),set()] # brick id, supports, supportedby
    bdict[i] = bricks[i]

def rngovlp(a:range, b:range): return range(max(a[0], b[0]), min(a[-1], b[-1])+1)

def overlapxy(a:list,b:list):
    xo = rngovlp(range(a[0][0],a[1][0]+1), range(b[0][0],b[1][0]+1))
    yo = rngovlp(range(a[0][1],a[1][1]+1), range(b[0][1],b[1][1]+1))
    return [len(xo)>0 and len(yo)>0, xo,yo]

def bfor(s:set) -> list: return [bdict[i] for i in s] # get bricks from id list

xrng = range(max([max(b[0][0],b[1][0]) for b in bricks])+1)
yrng = range(max([max(b[0][1],b[1][1]) for b in bricks])+1)
dbuf = [[(0,-1)]*xrng.stop for _ in yrng] # z-buffer containing (zmax,brickid) 

def setdbuf(b:list):
    for y in range(b[0][1],b[1][1]+1):
        for x in range(b[0][0],b[1][0]+1):
            dbuf[y][x] = (b[1][2],b[2]) # zmax,id

def dbufmax(b:list): return max([dbuf[y][x][0] for y in range(b[0][1],b[1][1]+1) for x in range(b[0][0],b[1][0]+1)])

bricks.sort(key = lambda b: b[0][2]) # order by z

for b in bricks: # collapse structure using z-buffer
    zmin = dbufmax(b) + 1 # +1 because we have to be on top on the current max
    b[1][2] = zmin + b[1][2]-b[0][2]
    b[0][2] = zmin
    setdbuf(b)

bricks.sort(key = lambda b: b[0][2]) # order by z again

for b in bricks: # create support map (from,to,id,supports,supportedby)
    for b2 in [x for x in bricks if b[0][2] == x[1][2]+1]: # b is supported by these
        if overlapxy(b,b2)[0]:
            b[-1].add(b2[2]) # supportedby
            b2[-2].add(b[2]) # supports

removecand = set() # candidates for removal
for b in (x for x in bricks if len(x[-1]) > 1): # find all bricks supported by at least 2 other bricks
    for sb in bfor(b[-1]): # iterate over bricks supporting (b)
        if any([len(s[-1]) < 2 for s in bfor(sb[-2])]): continue # supporting brick (sb) is the sole support for at least one brick - we cannot remove it
        removecand.add(sb[2])

for ns in (x[2] for x in bricks if len(x[-2]) == 0): removecand.add(ns) # add bricks that do not support other bricks

print('Day22-1',len(removecand))

chainr = []
for b in bricks:
    rs = set([b[2]]) # brick id of all moving/disintegrated bricks
    qadd = set() # bricks already added to heapq
    q = list(map(lambda x: (x[0][2],x),bfor(b[-2]))) # enqueue bricks supported by (b)
    heapq.heapify(q) # bricks affected by chain reaction, process lowest first
    while q: 
        sb = heapq.heappop(q)[1]
        if all([x in rs for x in sb[-1]]): # have all supports moved?
            rs.add(sb[2]) # this will also move
            for supp in bfor(sb[-2]):
                if supp[2] not in qadd: # avoid duplicate add to heapq
                    heapq.heappush(q, (supp[0][2], supp))
                    qadd.add(supp[2])
    chainr.append((b[2],len(rs)-1))

print('Day22-2',sum([x[1] for x in chainr]))
