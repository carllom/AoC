def doops(ptn, terms):
    t = terms[0]
    for i in range(len(ptn)):
        if ptn[i] == '0':  # add
            t += terms[i+1]
        elif ptn[i] == '1':  # multiply
            t *= terms[i+1]
        else:  # concatenate
            t = int(str(t) + str(terms[i+1]))
    return t


def trinary(n):  # base 3 pattern used in part 2
    res = ''
    while n > 0:
        res = str(n % 3) + res
        n = n // 3
    return res


eqs = [(int(l.split(':')[0]), [int(t) for t in l.split(':')[1].split()]) for l in open('day7-1.data').read().splitlines()]
tot = 0
unsolved = set(range(len(eqs)))  # optimization for part 2 - remember which equations are already solved
for i, eq in enumerate(eqs):
    for c in range(2**(len(eq[1])-1)):  # try all possible combinations of operations
        ptn = bin(c)[2:].zfill(len(eq[1])-1)  # two possible operations -> binary pattern
        t = doops(ptn, eq[1])
        if t == eq[0]:
            unsolved.discard(i)
            tot += t
            break

print('Day7-1', tot)

for eq in [eqs[u] for u in unsolved]:  # only try for the still unsolved equations
    for c in range(3**(len(eq[1])-1)):
        ptn = trinary(c).zfill(len(eq[1])-1)  # three possible operations -> trinary pattern
        t = doops(ptn, eq[1])
        if t == eq[0]:
            tot += t
            break

print('Day7-2', tot)
