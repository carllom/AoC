s=0
with open('day1-1.data') as input:
    for line in input:
        l = list(map(int,filter(lambda c: c.isdigit(), line)))
        s+=l[0]*10+l[-1]
print('Day1-1:', s)

toks = {
    '0': 0, 'zero': 0,
    '1': 1, 'one': 1,
    '2': 2, 'two': 2,
    '3': 3, 'three': 3,
    '4': 4, 'four': 4,
    '5': 5, 'five': 5,
    '6': 6, 'six': 6,
    '7': 7, 'seven': 7,
    '8': 8, 'eight': 8,
    '9': 9, 'nine': 9
}

import itertools
s=0
with open('day1-1.data') as input:
    for line in input:
        y = sorted(list(set(itertools.chain(
            filter(lambda x: x[0]>-1,map(lambda x, q=line:(q.rfind(x),x) , toks.keys())),
            filter(lambda x: x[0]>-1,map(lambda x, q=line:(q.find(x),x) , toks.keys()))
            ))))
        s+= toks[y[0][1]]*10 + toks[y[-1][1]]
print('Day1-2:',s)