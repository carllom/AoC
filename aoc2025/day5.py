fresh_rng, fruits = [], []
for d in (x.strip() for x in open('day5-1.data', encoding='utf-8') if x.strip()):
    if (i := d.find('-')) == -1:  # no dash == single fruit
        fruits.append(int(d))
    else:  # range of fruits
        fresh_rng.append(range(int(d[:i]), int(d[i+1:])+1))  # +1 to make the range inclusive

print('Day5-1:', len([x for x in fruits if any(x in r for r in fresh_rng)]))

merged = []  # merged ranges
for r in sorted(fresh_rng, key=lambda x: x.start):  # order ranges by start value
    if not merged or r.start > merged[-1].stop:
        merged.append(r)  # new disjoint range
    else:
        merged[-1] = range(merged[-1].start, max(merged[-1].stop, r.stop))  # overlap: extend the last range
print('Day5-2:', sum(r.stop - r.start for r in merged))
