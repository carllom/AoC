import re
from itertools import chain
from math import prod

data = open('day19-1.data').read().splitlines()
i=0
workflows = {}
parts = []
accepted = []

while data[i]: # parse workflows
  m = re.match(r'(\w+){([^}]+)',data[i])
  workflows[m.group(1)] = m.group(2).split(',')
  i+=1
i+=1 # empty line
while i<len(data): # parse parts
  parts.append(dict(chain([[x.split('=')[0],int(x.split('=')[1])] for x in data[i][1:-1].split(',')])))
  i+=1

def runwf(workflow, part) -> str|None:
  for step in workflow:
    cond = re.match(r'(\w+)([<>])(\w+):(\w+)',step) # try to match a conditional expression
    if cond:
      g = cond.groups()
      if g[1] == '<':
        if part[g[0]] >= int(g[2]): continue
        else: step = g[3]
      else: # '>'
        if part[g[0]] <= int(g[2]): continue
        else: step = g[3]
    if step == 'A':
      accepted.append(part)
      return None
    if step == 'R': return None
    return step
  return None

for part in parts:
  wf = workflows['in']
  while True:
    newwf = runwf(wf, part)
    if newwf == None: break
    wf = workflows[newwf]

print('Day19-1',sum([sum(a.values()) for a in accepted]))

def rangefor(part:dict, attr:str, op:str, val:int) -> tuple: # (match,nomatch) 
  r = part[attr]
  if op == '<':
    if r[0] >= val: return (None,part) # no matches
    if r[1] < val: return (part,None) # all matches
    p2 = part.copy()
    p3 = part.copy()
    p2[attr] = [p2[attr][0],val-1]
    p3[attr] = [val,p3[attr][1]]
    return (p2,p3)
  else: # '>'
    if r[1] <= val: return (None,part) # no matches
    if r[0] > val: return (part,None) # all matches
    p2 = part.copy()
    p3 = part.copy()
    p2[attr] = [val+1,p2[attr][1]]
    p3[attr] = [p3[attr][0],val]
    return (p2,p3)

def evalstep(part:dict, wf:list, stpidx:int = 0):
  if not part or stpidx >= len(wf): return 0 # no condi
  step = wf[stpidx]
  if step == 'A': return prod([(x[1]-x[0]+1) for x in part.values()]) # product of all attributes
  if step == 'R': return 0 # reject all
  cond = re.match(r'(\w+)([<>])(\w+):(\w+)',wf[stpidx]) # try to match a conditional expression
  if cond:
    attr,op,cval,trg  = cond.groups()
    newrng = rangefor(part,attr,op,int(cval))
    return evalstep(newrng[0],workflows[trg]) + evalstep(newrng[1],wf,stpidx+1) # true part => trg, false part => next
  else:
    return evalstep(part,workflows[step]) # unconditional jump to another workflow

workflows['A'] = ['A'] # add special accept and reject workflows to simplify evalstep logic
workflows['R'] = ['R']

print('Day19-2',evalstep({'x':[1,4000], 'm':[1,4000], 'a':[1,4000], 's':[1,4000]}, workflows['in']))