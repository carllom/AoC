from collections import deque
import itertools
import cProfile

data = list(map(lambda l:(l.split()[0], list(map(int,l.split()[1].split(',')))),open('day12-1.data').read().splitlines()))

def sw1st(a, c): return list(map(lambda x: x.replace('?', c, 1),a))

def part1():
    s=0
    for l in data:
        # generate all combinations
        a = [l[0]]
        while a[-1].find('?') > -1:
            a = sw1st(a,'.') + sw1st(a,'#')

        m = 0
        for c in a:
            r = list(map(len,c.replace('.',' ').split()))
            if r==l[1]: m+=1
        
        s+=m
    print ('Day12-1:', s)

#part1()

#
# Part 2
#

def match(s:str, p:str) -> bool:
   if len(s) != len(p): return False
   return all([x[0] == '?' or x[0] == x[1] for x in zip(s,p)])

def slide(s:str, begin:int, l:list) -> list:
  s += '.'
  cl = []
  p = '#'*l[0] + '.'
  sr = sum(l) + len(l)-2 # min size taken by list is sum of all sizes + separating dots (len(l)-1) minus one more because first element can be in the original position

  for i in range(begin,len(s)-sr):
    if i>0 and s[i-1] == '#': continue # we cannot begin immediately after a '#'
    rs = match(s[i:i+len(p)],p)
    # print(i,s[i:i+len(p)],p,rs)
    if rs:
      cl.append(i)
      # if s[i:i+len(p)-1] == p[:-1]:
      #   break # we found an exact(all '#') matching slot - this means there is no point in moving further
  return cl

def filter(s:str, l:list):
  for i in range(len(l)):
    # print('Before',l[i])
    if i == 0:
      while '#' in s[0:l[i][1][-1]]: # we cannot have a gap with '#' before first pattern
        l[i][1].pop()
      # print('After<',l[i])
    if i == len(l)-1:
      while '#' in s[l[i][1][0]+l[i][0]:]: # we cannot have a gap with '#' after last pattern
        l[i][1].popleft()
      # print('After>',l[i])
    if i < len(l)-1: # dont do this for last item (as we check next)
      while l[i][1][-1]+l[i][0] >= l[i+1][1][-1]: # filter those locations where pattern end touches or passes last index of next pattern
        l[i][1].pop()
      # print('After1',l[i])
      while '#' in s[l[i][1][0]+l[i][0]:l[i+1][1][0]]: # we cannot have a gap with '#' between first pattern in current and first pattern in next
        l[i][1].popleft()
      # print('After2',l[i])
    if i > 0: # dont do this for first item (as we check previous)
      while '#' in s[l[i-1][1][-1]+l[i-1][0]:l[i][1][-1]]: # we cannot have a gap with '#' between last pattern in previous and last pattern in current 
        l[i][1].pop()
      # print('After3',l[i])
    # print()
  return l #[x for x in l if len(x[1]) != 1] # remove all patterns with just one possible location

def slideall(s:str, l:list) -> list:
  # print(s,l)
  rl = []
  lo = 0
  for i in range(len(l)):
    # print(lo)
    sld = slide(s, lo, l[i:])
    rl.append((l[i], deque(sld)))
    lo = rl[-1][1][0]+rl[-1][0]+1
  return rl

def dump(s:str, ptnsizes:list, ptnlist:list):
  print(s,ptnsizes)
  for ptn in ptnlist:
    for i in ptn[1]:
      print(''.join([' '*i,'#'*ptn[0],'.']))

res = 0
for l in data:
  expanded = '?'.join([l[0]]*5)
  explist = l[1]*5

  arr = slideall(expanded, explist)

  flc = -1
  lap = 0
  while True:
    lap += 1
    fl = filter(expanded, arr) # remove impossible and trivial combinations
    nflc = sum([len(p[1]) for p in fl])
    if nflc == flc: break
    flc = nflc
    if lap > 1: print('LAP:',lap)

  dump(expanded,explist,fl)  
  fl.reverse() # iterate patterns backwards (right to left)
  
  if not [x for x in fl if len(x[1]) != 1]:
    res += 1 # only one valid combination  
    continue

  tl = [[x,1] for x in fl[0][1]] # combinations for last segment
  for i in range(1,len(fl)):
    tn = []
    for j in fl[i][1]:
      tn.append([j,sum([z[1] for z in tl if z[0] > j + fl[i][0] and '#' not in expanded[j + fl[i][0]:z[0]] ])]) #sum up for non-overlapping *AND* without '#' in gap
    tl=tn

  res += sum([x[1] for x in tl])

  print('Comb:', sum([x[1] for x in tl]), fl)
  print('='*40)
  # res += srch

print('Day12-2',res)

# 1st try 18313994344847 too high
# 2nd try 18297462722164 too high (iterated filter loop until no change)
# 3rd try 6512849198636 OK! (removed combinations with '#' in gap)
