from collections import deque

data = list(map(lambda l:(l.split()[0], list(map(int,l.split()[1].split(',')))),open('day12-1.data').read().splitlines()))

def sw1st(a, c): return list(map(lambda x: x.replace('?', c, 1),a))

s=0
for l in data: # generate all combinations
  a = [l[0]]
  while a[-1].find('?') > -1:
    a = sw1st(a,'.') + sw1st(a,'#') # switch combination for '.' and '#'
  
  m = 0
  for c in a:
    r = list(map(len,c.replace('.',' ').split()))
    if r==l[1]: m+=1
    
  s+=m
print ('Day12-1', s)

def match(s:str, p:str) -> bool:
   if len(s) != len(p): return False
   return all([x[0] == '?' or x[0] == x[1] for x in zip(s,p)])

def slide(record:str, begin:int, l:list) -> list:
  record += '.' # pad record so we can match pattern in last position
  cl = []
  p = '#'*l[0] + '.' #  pattern template
  sr = sum(l) + len(l)-2 # minimal size occupied by remaining patterns is sum of all sizes + separating dots (len(l)-1) minus one more because first element can be in the original position

  for i in range(begin,len(record)-sr): # 
    if i>0 and record[i-1] == '#': continue # we cannot begin immediately after a '#'
    rs = match(record[i:i+len(p)],p) 
    if rs: cl.append(i)
  return cl

def filter(s:str, l:list): # remove invalid pattern positions
  for i in range(len(l)):
    if i == 0:
      while '#' in s[0:l[i][1][-1]]: l[i][1].pop() # we cannot have a gap with '#' before first pattern
    if i == len(l)-1:
      while '#' in s[l[i][1][0]+l[i][0]:]: l[i][1].popleft() # we cannot have a gap with '#' after last pattern
    if i < len(l)-1: # dont do this for last item (as we check next)
      while l[i][1][-1]+l[i][0] >= l[i+1][1][-1]: l[i][1].pop() # filter those locations where pattern end touches or passes last index of next pattern
      while '#' in s[l[i][1][0]+l[i][0]:l[i+1][1][0]]: l[i][1].popleft() # we cannot have a gap with '#' between first pattern in current and first pattern in next
    if i > 0: # dont do this for first item (as we check previous)
      while '#' in s[l[i-1][1][-1]+l[i-1][0]:l[i][1][-1]]: l[i][1].pop() # we cannot have a gap with '#' between last pattern in previous and last pattern in current 
  return l

def slideall(record:str, l:list) -> list:
  rl = []
  lo = 0 # earliest possible start
  for i in range(len(l)):
    sld = slide(record, lo, l[i:]) # get possible positions by sliding the pattern over the record
    rl.append((l[i], deque(sld)))
    lo = rl[-1][1][0]+rl[-1][0]+1 # next pattern cannot start until after the first possible position for current pattern
  return rl

def dump(s:str, ptnsizes:list, ptnlist:list): # this was _very_ helpful when debugging
  print(s,ptnsizes)
  for ptn in ptnlist:
    for i in ptn[1]:
      print(''.join([' '*i,'#'*ptn[0],'.']))
  print()

res = 0
for l in data:
  expanded = '?'.join([l[0]]*5)
  explist = l[1]*5
  arr = slideall(expanded, explist) # calculate matching positions in the record for all patterns

  flc = -1
  while True: # repeat filtering until list is unchanged
    fl = filter(expanded, arr) # remove impossible and trivial combinations
    nflc = sum([len(p[1]) for p in fl])
    if nflc == flc: break # list was unchanged after filtering
    flc = nflc

  # dump(expanded,explist,fl)
  fl.reverse() # iterate patterns backwards (right to left)
  
  if not [x for x in fl if len(x[1]) != 1]:
    res += 1 # only one valid combination - trivial calculation
    continue
  tl = [[x,1] for x in fl[0][1]] # combinations for last segment
  for i in range(1,len(fl)): # build combination tree
    tn = []
    for j in fl[i][1]:
      tn.append([j,sum([z[1] for z in tl if z[0] > j + fl[i][0] and '#' not in expanded[j + fl[i][0]:z[0]] ])]) #sum up for non-overlapping *AND* without '#' in gap
    tl=tn
  # print('Combinations:', sum([x[1] for x in tl]), fl)
  res += sum([x[1] for x in tl])

print('Day12-2',res)
# 1st try 18313994344847 too high (do not stop search on matching '#'-slot)
# 2nd try 18297462722164 too high (iterated filter loop until no change)
# 3rd try 6512849198636 OK! (removed combinations with '#' in gap)