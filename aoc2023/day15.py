def totup(x:str) -> tuple:
    i=x.find('=')
    if i >= 0:
        return (x[:i],int(x[i+1:]))
    else:
        i=x.find('-')
        return (x[:i],0)

data = open('day15-1.data').read().split(',')

def hash(d:str) -> int:
    cv = 0
    for c in d: cv = ((cv + ord(c))*17) % 256
    return cv

print('Day15-1',sum(map(hash,data)))


data = list(map(totup,open('day15-1.data').read().split(',')))

def idxof(l,d):
    try:
        return list(map(lambda x:x[0],l)).index(d)
    except ValueError:
        return -1

hmap = dict()
for d in data:
    h = hash(d[0])
    if h not in hmap: hmap[h] = []
    l = hmap[h]
    i = idxof(l,d[0])

    if not d[1]:
        if (i >= 0):
            del l[i]
    else:
        if (i >= 0):
            hmap[h][i] = d
        else:
            hmap[h].append(d)

s = 0
for k in sorted(list(hmap.keys())):
    for i,x in enumerate(hmap[k]):
        s += (k+1) * (i+1) * x[1]

print('Day15-2',s)
