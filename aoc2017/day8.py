prog = [l.split() for l in open('day8.txt', 'r').readlines()]
regs = dict()
maxany = 0
for i in prog:
    if i[4] not in regs.keys():
        regs[i[4]] = 0
    if i[0] not in regs.keys():
        regs[i[0]] = 0
    if eval(f'{regs[i[4]]} {i[5]} {i[6]}'):
        if i[1] == 'inc':
            regs[i[0]] += int(i[2])
        else:
            regs[i[0]] -= int(i[2])
    maxany = max(maxany, max(regs.values()))

print('Outcome task#1', max(regs.values()))
print('Outcome task#2', maxany)
