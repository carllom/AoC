from functools import cmp_to_key

def pc(l):
    c = l.split()
    return [c[0],int(c[1]),0]

hands = list(map(pc, open('day7-1.data').read().splitlines()))

def rank(hand:str):
    q = [0,0,0,0,0]
    grp = [[y for y in hand if y==x] for x in set(hand)]
    for x in grp:
        q[len(x)-1] +=1

    if q[4] > 0: return 7
    elif q[3] > 0: return 6
    elif q[2] == 1 and q[1] == 1: return 5
    elif q[2] == 1: return 4
    elif q[1] == 2: return 3
    elif q[1] == 1: return 2
    else: return 1


for hand in hands:
    hand[2] = rank(hand[0])

suit = '23456789TJQKA'
def cmp(c1, c2):
    if c1[2] == c2[2]:
        for i in range(0,len(c1[0])):
            r = suit.index(c1[0][i]) - suit.index(c2[0][i])
            if r != 0:
                return r
    return c1[2] - c2[2]

hands.sort(key=cmp_to_key(cmp))

s=0
for i,h in enumerate(hands):
   s+= (i+1)*h[1]

print('Day7-1',s)

suit = 'J23456789TQKA'
for hand in hands:
    if hand[0].find('J') < 0:
        hand[2] = rank(hand[0])
    else:
        saveh = hand[0]
        maxr = 0
        maxh = ''
        for r in suit[1:]:
            saveh = hand[0].replace('J',r)
            maxr = max(maxr,rank(saveh))
        hand[2] = maxr

hands.sort(key=cmp_to_key(cmp))

s=0
for i,h in enumerate(hands):
   s+= (i+1)*h[1]

print('Day7-2',s)