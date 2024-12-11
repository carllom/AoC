from collections import defaultdict
data = list(map(int, open('day11.data').read().split(' ')))
ns = []
for i in range(25):
    for s in range(len(data)):
        if data[s] == 0:
            ns.append(1)  # data[s] = 1
        elif len(str(data[s])) % 2 == 0:
            l = len(str(data[s]))
            ns.append(int(str(data[s])[:l//2]))
            ns.append(int(str(data[s])[-l//2:]))
        else:
            ns.append(data[s]*2024)
    data = ns
    ns = []

print('Day11-1', len(data))

d = defaultdict(int, map(lambda x: (x, 1), list(map(int, open('day11.data').read().split(' ')))))
nd = defaultdict(int)  # use a dictionary approach to store the number of stones per number - assuming to get many stones with the same value
for i in range(75):
    for k in d.keys():
        if k == 0:
            nd[1] += d[k]
        elif len(str(k)) % 2 == 0:
            l = len(str(k))
            nd[int(str(k)[:l//2])] += d[k]
            nd[int(str(k)[-l//2:])] += d[k]
        else:
            nd[k*2024] += d[k]
    tmp = d  # swap dictionaries and reuse the old one
    d = nd
    nd = tmp
    nd.clear()

print('Day11-2', sum(d.values()))
