import sys

data = open('day5-1.data')

seeds = list(map(int,data.readline().split()[1:]))
data.readline() # read empty line

ranges = []
mapnames = []
maps = [] # dst,src,len
with data as input:
    for line in input:
        if len(line.strip()) == 0:
            maps += [ranges]
            ranges = []
            continue
        toks = line.split()
        if line.strip().endswith(':'):
            mapnames += [toks[0]]
        else:
           ranges += [list(map(int,toks))] 
maps += [ranges]

mins = sys.maxsize
for s in seeds:
    for i,m in enumerate(maps):
        for c in m: # conversion
            seeds_nxt = s-c[1]
            if seeds_nxt >= 0 and seeds_nxt < c[2]:
                s = seeds_nxt+c[0]
                break
    mins = min(mins,s)
print('Day5-1:',mins)

def range_overlap(a, b):
    return range(max(a[0], b[0]), min(a[-1], b[-1]) + 1)

from operator import add

# seed range map into next range(s) and repeat
seeds2 = list(map(lambda x:range(x[0],x[1]),zip(seeds[0::2], map(add,seeds[0::2], seeds[1::2])))) # dst,src,len
for i,m in enumerate(maps): # map
    seeds_nxt = []
    for s2 in seeds2:
        sconv = False
        for c in m: # conversion
            cr = range(c[1],c[1]+c[2])
            ol = range_overlap(s2, cr)
            if ol.start < ol.stop: # valid range
                if ol.start > s2.start:
                    seeds2.append(range(s2.start,ol.start)) # add 'cutoffs' to seed list
                if ol.stop < s2.stop:
                    seeds2.append(range(ol.stop,s2.stop)) # add 'cutoffs' to seed list

                off = c[0]-c[1]
                seeds_nxt.append(range(off+ol.start, off+ol.stop)) # add remapped range
                sconv = True
        if not sconv:
            seeds_nxt.append(s2) # range was not converted (identity map)
    seeds2 = seeds_nxt
print('Day5-2:', sorted(seeds2, key=lambda r: r.start)[0].start)