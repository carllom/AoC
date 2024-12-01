l, r = [], []
for a in [b.split() for b in open('day1-1.data').read().splitlines()]:
    l.append(int(a[0]))
    r.append(int(a[1]))
l.sort()
r.sort()
print('Day1-1:', sum([abs(l[i] - r[i]) for i in range(len(l))]))  # distanes (abs l-r)

print('Day1-2:', sum([l[i] * r.count(l[i]) for i in range(len(l))]))  # 2nd part "similarity score"
