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

res = 0
for l in data:
  expanded = '?'.join([l[0]]*5)
  explist = l[1]*5
  cProfile.run('search(expanded, explist)')

  srch = search(expanded, explist)
#   srch = search(l[0], l[1])
  print('Comb:', srch, l)
  res += srch
  # dot = l[0].replace('?','.',1)
  # hash = l[0].replace('?','#',1)
print(res)
