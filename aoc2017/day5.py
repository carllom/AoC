prog = [int(off) for off in open('day5.txt').readlines()]
pc = 0
steps = 0
while pc in range(len(prog)):
    offset = prog[pc]
    prog[pc] += 1
    pc += offset
    steps += 1
print('Outcome task#1:', steps)

prog = [int(off) for off in open('day5.txt').readlines()]
pc = 0
steps = 0
while pc in range(len(prog)):
    offset = prog[pc]
    prog[pc] += 1 if offset < 3 else -1
    pc += offset
    steps += 1
print('Outcome task#2:', steps)
