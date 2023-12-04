from itertools import product, starmap

data = open('day3-1.data').read().splitlines()

numn = []
# turn the problem on its head - find all digits that are neighbors to a symbol
for y,line in enumerate(data):
    for x,c in enumerate(line):
        if not (c.isdigit() or c == '.'): # find symbol
            neighb = list(starmap(lambda a,b,xl=x: (xl+a, y+b), product((0,-1,+1), (0,-1,+1))))[1:] # generate neighbor coords
            for nx,ny in neighb:
                if nx<0 or ny<0 or nx >= len(line) or ny >= len(data): continue
                elif data[ny][nx] in '0123456789':
                    numn += [(ny,nx)]
s=0
# scan map for part numbers and add if neighbor to a symbol
for y,line in enumerate(data):
    d = 0
    atsymb = False
    for x,c in enumerate(line):
        if c.isdigit(): # digit in part number
            d = d*10 + int(c)
            if (y,x) in numn:
                atsymb=True
        else: # end of part number
            if d>0 and atsymb:
                s+=d
            d=0
            atsymb=False
    if d>0 and atsymb: # also terminate number at end of line
        s+=d
print('Day3-1:',s)

gears = [] # gear 'objects'
# find all digits neighboring to a gear(*)
for y,line in enumerate(data):
    for x,c in enumerate(line):
        if c == '*': # symbol is a gear
            neighb = list(starmap(lambda a,b,xl=x: (xl+a, y+b), product((0,-1,+1), (0,-1,+1))))[1:] # generate neighbor coords
            numn = []
            for nx,ny in neighb:
                if nx<0 or ny<0 or nx >= len(line) or ny >= len(data): continue
                elif data[ny][nx] in '0123456789':
                    numn += [(ny,nx)]
            gears += [((y,x), numn, [])] # gear coord, neighbor cells(with digits), product numbers(used in number scan)

# scan map for part numbers and add if neighbor to a gear
for y,line in enumerate(data):
    d=0
    g={} # neighboring gears
    for x,c in enumerate(line):
        if c.isdigit(): # digit in part number
            d = d*10 + int(c)
            for gear in gears:
                if (y,x) in gear[1] and gear[0] not in g: # add gear if we neighbor it
                    g[gear[0]]=gear
        else: # end of part number
            if d>0 and len(g)>0:
                for g0 in g.values():
                    g0[2].append(d) # add product number to gear object
            d=0
            g={}
    if d>0 and len(g)>0: # also terminate number at end of line
        for g0 in g.values():
            g0[2].append(d)

s = sum(starmap(lambda x, y:x*y, filter(lambda g: len(g)==2,map(lambda g: g[2], gears))))
print('Day3-2:',s)