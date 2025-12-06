from itertools import accumulate
from functools import reduce
moves = list(map(int, open('day1-1.data', encoding='utf-8').read().replace('L', '-').replace('R', '+').splitlines()))


def zero_xing(last, rot):
    dn = (last[0] + (rot % 100)) % 100
    c = abs(rot) // 100  # full circles
    if dn == 0 or (last[0] != 0 and ((rot < 0) == (dn > last[0]))):  # landed on 0 or crossed zero (XNOR)
        c += 1
    return (dn, last[1] + c)


print('Day1-1:', list(accumulate(moves, lambda last, rot: (last + rot) % 100, initial=50)).count(0))
print('Day1-2:', reduce(zero_xing, moves, (50, 0))[1])  # accumulated is currpos, 0-cross sum
