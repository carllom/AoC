import re

m = list(map(lambda l:(l.split()[0], list(map(int,l.split()[1].split(',')))),open('day12-1.data').read().splitlines()))

def sw1st(a, c): return list(map(lambda x: x.replace('?', c, 1),a))

s=0
for l in m:
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