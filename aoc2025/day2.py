from math import ceil
data = [list(map(int, x.split('-'))) for x in open('day2-1.data', encoding='utf-8').read().split(',')]


def repattern(ptnlen, idfrom, idto):
    invalids = set()
    ptn = '1' + '0' * (ptnlen - 1)  # 100...0 as starting pattern
    while len(ptn) <= ptnlen:
        pid = ptn * ceil(len(str(idfrom)) / len(ptn))  # repeat pattern to cover full length
        while len(pid) <= len(str(idto)):
            if idfrom <= int(pid) <= idto and len(pid) > 1:
                invalids.add(pid)
            pid += ptn
        ptn = str(int(ptn) + 1)  # increment the pattern
    return invalids


s = set()
for d in data:
    for i in range(d[0], d[1]+1):  # brute force..bleh
        si = str(i)
        if len(si) % 2 == 0 and si[:len(si)//2] == si[len(si)//2:]:  # first half == second half (even length)
            s.add(i)
print('Day2-1:', sum(s))

for d in data:
    for n in range(1, len(str(d[1]))//2+1):  # pattern length from 1 to half range length (inclusive)
        s.update(map(int, repattern(n, d[0], d[1])))  # find repeated patterns for range
print('Day2-2:', sum(s))
