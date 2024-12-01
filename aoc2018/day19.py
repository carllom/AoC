# --- Day 16: Chronal Classification ---

opcodes = {
    'addr': lambda r, a, b: r[a] + r[b],
    'addi': lambda r, a, b: r[a] + b,
    'mulr': lambda r, a, b: r[a] * r[b],
    'muli': lambda r, a, b: r[a] * b,
    'banr': lambda r, a, b: r[a] & r[b],
    'bani': lambda r, a, b: r[a] & b,
    'borr': lambda r, a, b: r[a] | r[b],
    'bori': lambda r, a, b: r[a] | b,
    'setr': lambda r, a, b: r[a],
    'seti': lambda r, a, b: a,
    'gtir': lambda r, a, b: 1 if a > r[b] else 0,
    'gtri': lambda r, a, b: 1 if r[a] > b else 0,
    'gtrr': lambda r, a, b: 1 if r[a] > r[b] else 0,
    'eqir': lambda r, a, b: 1 if a == r[b] else 0,
    'eqri': lambda r, a, b: 1 if r[a] == b else 0,
    'eqrr': lambda r, a, b: 1 if r[a] == r[b] else 0,
}


def execute_instruction(registers, instruction):
    opcode, a, b, c = instruction
    registers[c] = opcodes[opcode](registers, a, b)
    return registers


def task1():
    with open("day19.txt", 'r') as input:
        ipreg = int(input.readline().split()[1])
        instructions = []
        for line in input.readlines():
            line = line.strip().split()
            instructions.append([line[0]]+list(map(int, line[1:])))

        ip = 0
        regs = [0, 0, 0, 0, 0, 0]
        while True:
            regs[ipreg] = ip
            regs = execute_instruction(regs, instructions[regs[ipreg]])
            ip = regs[ipreg] + 1
            if ip >= len(instructions):
                break
        return regs[0]


def task2():
    with open("day19.txt", 'r') as input:
        ipreg = int(input.readline().split()[1])
        instructions = []
        for line in input.readlines():
            line = line.strip().split()
            instructions.append([line[0]]+list(map(int, line[1:])))

        ip = 0
        regs = [0, 0, 0, 0, 0, 0]
        # rnames = ['r0', 'r1', 'r2', 'r3', 'r4', 'r5']
        # rnames[ipreg] = 'ip'
        while True:
            regs[ipreg] = ip

            # instr = instructions[regs[ipreg]]
            # print(f'{ipreg:2}:', rnames[instr[1]], rnames[instr[2]], rnames[instr[3]])
            # if (ip == 9):  # patch it to speed up
            #     regs[1] = regs[2]+1  # 09: gtrr 1 2 4 waits for r1>r2
            # if (ip == 13):  # patch it to speed up
            #     regs[3] = regs[2]+1  # 13: addi 3 1 3 waits for r3>r2

            if ip == 35:
                print('@35')

            rbef = regs.copy()

            # if regs[3] > 870:
            regs = execute_instruction(regs, instructions[regs[ipreg]])

            # if regs[0] != rbef[0]:
            # print(regs[ipreg], instructions[regs[ipreg]], rbef, regs)

            ip = regs[ipreg] + 1
            if ip >= len(instructions):
                break
        return regs[0]


print('Task#1 outcome:', task1())
print('Task#2 outcome:', task2())
