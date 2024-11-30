
discs = open('day7.txt', 'r').readlines()
discs = [[d[0], int(d[1][1:-1]), [da[:-1] for da in d[3:-1]] + [d[len(d)-1]] if len(d) > 2 else []] for d in [disc.split() for disc in discs]]
# print(discs)
root = [d for d in discs if d[0] not in set(sum([d[2] for d in discs], []))][0][0]
print('Outcome task#1', root)
ddict = dict([(d[0], d) for d in discs])
print(ddict)


def weight(disc):
    return ddict[disc][1] + sum([weight(d) for d in ddict[disc][2]])


def balanced(disc):
    if len(ddict[disc][2]) == 0:
        return True
    return len(set([weight(d) for d in ddict[disc][2]])) == 1


def balance(node):
    if not balanced(node):
        wts = [(balance(d), d) for d in ddict[node][2]]
        cw = [w for w, d in wts]
        for w, d in wts:
            if cw.count(w) == 1:
                ddict[d][1] = ddict[d][1] + (set([x[0] for x in wts if x[0] != w]).pop() - w)
                print('Outcome task#2', ddict[d][1])
                break
    return weight(node)


balance(root)
