def colat(m, i):
    return ''.join([l[i] for l in m])

def univexp(m, xpand):
    offy = [0]
    offx = [0]

    # expand vert
    for l in m[:-1]: offy.append(offy[-1] + (xpand if l.count('#') == 0 else 1))

    # expand horz
    for i in range(len(m[0])-1): offx.append(offx[-1] + (xpand if colat(m,i).count('#') == 0 else 1))

    # get (offset) star locations
    stars = []
    for y,l in enumerate(m):
        for x,c in enumerate(l):
            if c=='#': stars.append((offx[x],offy[y]))

    # manh dist over all pairs
    sumd = 0
    for i in range(len(stars)):
        for j in range(i+1,len(stars)):
            sumd += abs(stars[i][0]-stars[j][0]) + abs(stars[i][1]-stars[j][1])
    return sumd

m = list(map(list,open('day11-1.data').read().splitlines()))
print ('Day11-1:', univexp(m, 2))
print ('Day11-2:', univexp(m, 1000000))