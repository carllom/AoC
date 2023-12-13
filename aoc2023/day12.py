import itertools
import cProfile

data = list(map(lambda l:(l.split()[0], list(map(int,l.split()[1].split(',')))),open('day12-0.data').read().splitlines()))

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
            # r = list(map(len,re.findall('\.?(#+)\.', c+'.')))
            r = list(map(len,c.replace('.',' ').split()))
            if r==l[1]: m+=1
        
        s+=m
    print ('Day12-1:', s)

#
# Part 2
#

def goodsofar(s:str, l:list) -> bool: # check if s matches l until first '?'
  sofar0 = list(map(len, s.split('?', maxsplit=1)[0].replace('.',' ').split())) # list of hash group lengths up until first '?'
  # print ('mapped:', sofar, s.split('?', maxsplit=1)[0], s ) 
  
  # alternative sofar impl without split and replace - still slow
  sofar = []
  end = s.find('?')
  if end<0: end=len(s)
  hs = s.find('#')
  he = 0
  while hs >= 0 and hs < end:
    he = s.find('.',hs)
    if he < 0: he = end
    sofar.append(min(he,end)-hs)
    hs = s.find('#',he+1)
  
  if sofar != sofar0:
     print('Error!',sofar0, sofar)
  # end alternative sofar

  lt = False
  for z in itertools.zip_longest(sofar,l, fillvalue=0):
    #   if z[0] is None: continue
    # print(z)
    if z[0] > z[1]: return False # hash group is bigger than list
    if z[0] > 0 and z[0] < z[1]:
      if lt: return False # hash group is smaller and is not last in list
      else: 
        lt = True # We are at the first less than
        continue

#   for i, s in enumerate(sofar): 
#      if l[i] > s or (l[i] == s 
#      print('sofar',i,s)
  return True

def good(s:str, l:list) -> bool:
   return l == list(map(len, s.replace('.',' ').split())) # list of hash group lengths up until first '?'


def search(s:str, l:list):
  if s.find('?') < 0: return good(s,l)
    #  print('validcomb:',s,l)
    #  return 1 # no more ? means this combination works

  dot = s.replace('?','.',1)
  hash = s.replace('?','#',1)
  tot = 0

  dotok = goodsofar(dot, l)
#   print(dot, l, dotok)
  if dotok:
    tot += search(dot, l)
  hashok = goodsofar(hash, l)
#   print(hash, l, hashok)
  if hashok:
    tot+= search(hash, l)

  return tot

def match(s:str, p:str) -> bool:
   if len(s) != len(p): return False
   return all([x[0] == '?' or x[0] == x[1] for x in zip(s,p)])

def slide(s:str, begin:int, l:list) -> list:
  s += '.'
  cl = []
  p = '#'*l[0] + '.'
  sr = sum(l) + len(l)-2 # min size taken by list is sum of all sizes + separating dots (len(l)-1) minus one more because first element can be in the original position

  for i in range(begin,len(s)-sr):
    rs = match(s[i:i+len(p)],p)
    print(i,s[i:i+len(p)],p,rs)
    if rs: cl.append(i)
    if s[i:i+len(p)-1] == p[:-1]: 
       break # we found an exact(all '#') matching slot - this means there is no point in moving further
  return cl

def slideall(s:str, l:list) -> list:
  print(s,l)
  rl = []
  lo = 0
  for i in range(len(l)):
    print(lo)
    rl.append((l[i], slide(s, lo, l[i:])))
    lo = rl[-1][1][0]+l[i]+1
  return rl

res = 0
for l in data:
  expanded = '?'.join([l[0]]*5)
  explist = l[1]*5
  x = '''
  cProfile.run('search(expanded, explist)')
  srch = search(l[0], l[1])
  arr = slideall(l[0], l[1])
  '''
  srch = search(expanded, explist)
  arr = slideall(expanded, explist)

  print('Comb:', srch, l, arr)
  res += srch

print(res)

'''
New idea - use patterns for each list item

1 = #.
2 = ##.
..and so on

slide these across the string and record all possible starting points

you will end up with a list of starting points for each item
[
(1, [0,3,4,5,6,12])
(1, [0,3,4,5,6,12])
(3, [5])
]

then you can probably sort and cut down the starting points to a subset
and do a product of the length of all starting point lists

Comb: 10 ('?###????????', [3, 2, 1])

[(3, [1, 2, 3, 4, 5]),
 (2, [5, 6, 7, 8, 9]),
 (1, [8, 9, 10, 11])]

 [(3, [1]),
  (2, [5, 6, 7, 8, 9]),
  (1, [8, 9, 10, 11])]
 
 1 => 1+(3+1)=5 => 5+(2+1)=8 => 8..11 = 4
                   6+(2+1)=9 => 9..11 = 3
                   7+(2+1)=10 => 10..11 = 2
                   8+(2+1)=11 => 11.11 = 1

Comb: 16384 ('.??..??...?##.', [1, 1, 3])
[(1, [1, 2, 5, 6]),
 (1, [5, 6]),
 (3, [10, 25, 40]),
 (1, [14, 16, 17, 20, 21]),
 (1, [16, 17, 20, 21]),
 (3, [25, 40]),
 (1, [29, 31, 32, 35, 36]),
 (1, [31, 32, 35, 36]),
 (3, [40, 55]),
 (1, [44, 46, 47, 50, 51]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62, 65, 66]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 remove impossible (last element >= last element in next row):
 [(1, [1, 2, 5]),
 (1, [5, 6]),
 (3, [10]),
 (1, [14, 16, 17, 20]),
 (1, [16, 17, 20, 21]),
 (3, [25]),
 (1, [29, 31, 32, 35]),
 (1, [31, 32, 35, 36]),
 (3, [40]),
 (1, [44, 46, 47, 50]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62, 65]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 remove impossible (last element will overlap(including length) with last element in next row):
 [(1, [1, 2]),
 (1, [5, 6]),
 (3, [10]),
 (1, [14, 16, 17]),
 (1, [16, 17, 20, 21]),
 (3, [25]),
 (1, [29, 31, 32]),
 (1, [31, 32, 35, 36]),
 (3, [40]),
 (1, [44, 46, 47]),
 (1, [46, 47, 50, 51]),
 (3, [55]),
 (1, [59, 61, 62]),
 (1, [61, 62, 65, 66]),
 (3, [70])]

 4*3 - all less or equal to item - segment length
 59-61 62 65 66
 61-65 66
 62-65 66

 8st

1
[(1, [1]),
 (3, [3]),
 (1, [7]),
 (6, [9, 10]),
 (1, [17]),
 (3, [19]),
 (1, [23]),
 (6, [25, 26]),
 (1, [33]),
 (3, [35]),
 (1, [39]),
 (6, [41, 42]),
 (1, [49]),
 (3, [51]),
 (1, [55]),
 (6, [57, 58]),
 (1, [65]),
 (3, [67]),
 (1, [71]),
 (6, [73])]
 
Comb: 16 ('????.#...#...', [4, 1, 1]) [(4, [0, 13, 14]), (1, [5]), (1, [9]), (4, [13, 14, 27, 28]), (1, [19]), (1, 
[23]), (4, [27, 28, 41, 42]), (1, [33]), (1, [37]), (4, [41, 42]), (1, [47]), (1, [51]), (4, [55, 56]), (1, [61]), 
(1, [65])]

Comb: 2500 ('????.######..#####.', [1, 6, 5]) [(1, [0, 1, 2, 3]), (6, [5]), (5, [13]), (1, [19, 20, 21, 22, 23]), (6, [25]), (5, [33]), (1, [39, 40, 41, 42, 43]), (6, [45]), (5, [53]), (1, [59, 60, 61, 62, 63]), (6, [65]), (5, [73]), (1, [79, 80, 81, 82, 83]), (6, [85]), (5, [93])] 

almost there - we still cannot skip a place with a hash sign. Those combinations are illegal

'''