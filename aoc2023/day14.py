dish = list(map(list,open('day14-1.data').read().splitlines()))

def cost(d): return sum([(len(d)-ri) * d[ri].count('O') for ri in range(len(d))])

def tiltn(d):
  for ri in range(1,len(d)):
    for ci in range(len(d[ri])):
      if d[ri][ci] == 'O':
        di = ri-1
        while di >= 0 and d[di][ci] == '.':
          di-=1
        d[ri][ci] = '.'
        d[di+1][ci] = 'O'
  return d

print('Day14-1',cost(tiltn(dish)))

def tilts(d):
  for ri in range(len(d)-2,-1,-1):
    for ci in range(len(d[ri])):
      if d[ri][ci] == 'O':
        di = ri+1
        while di < len(d) and d[di][ci] == '.':
          di+=1
        d[ri][ci] = '.'
        d[di-1][ci] = 'O'
  return d
def tiltw(d):
  for ci in range(1,len(d[0])):
    for ri in range(len(d)):
      if d[ri][ci] == 'O':
        di = ci-1
        while di >= 0 and d[ri][di] == '.':
          di-=1
        d[ri][ci] = '.'
        d[ri][di+1] = 'O'
  return d
def tilte(d):
  for ci in range(len(d[0])-2,-1,-1):
    for ri in range(len(d)):
      if d[ri][ci] == 'O':
        di = ci+1
        while di < len(d[0]) and d[ri][di] == '.':
          di+=1
        d[ri][ci] = '.'
        d[ri][di-1] = 'O'
  return d

c = 0
cdict = dict()

for i in range(1000):
  tiltn(dish) # cycle
  tiltw(dish)
  tilts(dish)
  tilte(dish)
  cn = cost(dish)

  if cn not in cdict:
    cdict[cn] = ([i],[0],[i%78])   # visual inspection of cdict shows continuous repeat of 78 cycles
  else:
    v:tuple = cdict[cn]
    v[2].append(i%78)
    v[1].append(i-v[0][-1])
    v[0].append(i)

mod = (1000000000-1) % 78 # find repeat offset, but compensate that index 0 corresponds to cycle 1
y = list(filter(lambda y: mod in y[1][2], map(lambda k: (k,cdict[k]), filter(lambda k: len(cdict[k][1]) > 10, cdict.keys()))))
# previous line first filters all entries with 10+ occurrances and then finds the one with matching offset
print ('Day14-2',y[0][0])
