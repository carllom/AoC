def spin(n, progs):
    return progs[-n:] + progs[:-n]


def exchange(a, b, progs):
    progs[a], progs[b] = progs[b], progs[a]
    return progs


def partner(a, b, progs):
    a = progs.index(a)
    b = progs.index(b)
    return exchange(a, b, progs)


def dance(moves, progs):
    for move in moves:
        if move[0] == 's':
            progs = spin(int(move[1:]), progs)
        elif move[0] == 'x':
            a, b = map(int, move[1:].split('/'))
            progs = exchange(a, b, progs)
        elif move[0] == 'p':
            a, b = move[1:].split('/')
            progs = partner(a, b, progs)
    return progs


prog0 = list('abcdefghijklmnop')
moves = open('day16.txt').read().split(',')
progs = prog0.copy()

progs = dance(moves, progs)
print('Outcome task#1', ''.join(progs))

progs = prog0.copy()
mod = 0
for i in range(1000000000):
    progs = dance(moves, progs)
    if (progs == prog0):
        mod = 1000000000 % (i+1)  # we found a cycle - skip all full cycles and just calculate the remainder
        break
progs = list('abcdefghijklmnop')
for i in range(mod):
    progs = dance(moves, progs)

print('Outcome task#2', ''.join(progs))
