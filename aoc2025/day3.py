data = [x.strip() for x in open('day3-1.data', encoding='utf-8').readlines()]


def max_batt(line, nbatt):
    dl, vi = '', 0
    while len(dl) < nbatt:
        lseg = line[vi:-(nbatt-len(dl)-1)] if nbatt-len(dl)-1 > 0 else line[vi:]
        dl += max(lseg)
        vi += lseg.index(dl[-1]) + 1
    return int(dl)


print('Day3-1:', sum(max_batt(line, 2) for line in data))
print('Day3-2:', sum(max_batt(line, 12) for line in data))
