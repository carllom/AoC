data = open('day5-1.data').read().splitlines()

orders = []
goodupdates = []
badupdates = []
first_section = True


def breaksorder(u, orders):  # return the first order that is broken
    for k in range(len(orders)-1):
        i0 = u.index(orders[k][0]) if orders[k][0] in u else -1
        i1 = u.index(orders[k][1]) if orders[k][1] in u else -1
        if i0 != -1 and i1 != -1:
            if i0 > i1:
                return orders[k]
    return None


for i in range(len(data)):
    r = data[i]
    if not data[i]:  # sections divided by empty line
        first_section = False
        continue
    if first_section:  # section 1 - order rules
        r = data[i].split('|')
        orders.append((int(r[0]), int(r[1])))
    else:  # section 2 - updates
        u = list(map(int, data[i].split(',')))
        res = breaksorder(u, orders)
        if res is None:
            goodupdates.append(u)
        else:
            badupdates.append(u)

print('Day5-1', sum([g[len(g)//2] for g in goodupdates]))  # sum of middle elements

mid = 0
for b in badupdates:
    res = 'start'
    while res is not None:
        res = breaksorder(b, orders)
        if res is None:
            mid += b[len(b)//2]
            continue
        else:  # swap elements
            t = b[b.index(res[0])]
            b[b.index(res[0])] = b[b.index(res[1])]
            b[b.index(res[1])] = t

print('Day5-2', mid)
