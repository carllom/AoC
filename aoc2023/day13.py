maps = []
m = []
for line in open('day13-1.data').read().splitlines():
  if not len(line):
    maps.append(m)
    m = []
  else: m.append(line)
maps.append(m)

def cwrot(m:list) -> list:
  rm = []
  for i in range(len(m[0])):
    rml = ''
    for j in range(len(m)-1,-1,-1):
      rml += m[j][i]
    rm.append(rml)
  return rm

def trymatch(m:list, exclude=-1) -> int:
  for i in range(len(m)-1):
    match = True
    for j in range(min(i+1,len(m)-(i+1))):
      if m[i-j] != m[i+j+1]:
        match = False
        break
    if match and i != exclude:
      return i+1
  return 0

def findref(m, excl=-1, exclcw=-1):
  i = trymatch(m,excl)
  return 100*i if i else trymatch(cwrot(m),exclcw)

s = 0
for m in maps:
  s += findref(m)

print ('Day13-1',s)

def swp(s,i): return ''.join([s[:i],'#' if s[i] == '.' else '.',s[i+1:]]) # toggle ash/rock

s = 0
for m in maps:
  om = findref(m) # rerun old match to know what to exclude
  ex = -1 if om < 100 else (om//100-1)
  excw = om-1 if om < 100 else -1
  nm = 0
  found = False
  for y in range(len(m)):
    for x in range(len(m[y])):
      ol = m[y]
      m[y] = swp(ol,x)
      nm = findref(m,ex,excw)
      m[y] = ol # swap back
      if nm > 0 and nm != om:
        found = True
        break
    if found: break
  if not found: print('ERROR: No new match found') # we have a bug
  s += nm

print ('Day13-2',s)