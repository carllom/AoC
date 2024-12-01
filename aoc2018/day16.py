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


def matchopcodes(before, instruction, after):
    matches = set()
    _, a, b, c = instruction
    for k, v in opcodes.items():
        insres = before.copy()
        insres[c] = v(insres, a, b)
        if insres == after:
            matches.add(k)
    return matches


def task1():
    with open("day16.txt", 'r') as input:
        count = 0
        while True:
            before = input.readline().strip()
            if not before:
                break
            instruction = input.readline().strip()
            after = input.readline().strip()
            input.readline()
            count += 1 if len(matchopcodes(eval(before[8:]), list(map(int, instruction.split())), eval(after[8:]))) >= 3 else 0
        return count


def task2():
    candidates = {}
    with open("day16.txt", 'r') as input:
        baseops = set(opcodes.keys())
        while True:
            before = input.readline().strip()
            if not before:
                break  # end of trace
            instruction = input.readline().strip()
            after = input.readline().strip()
            input.readline()
            instruction = list(map(int, instruction.split()))
            if not instruction[0] in candidates.keys():
                candidates[instruction[0]] = baseops.copy()  # all opcodes are candidates to begin with
            c = candidates[instruction[0]] & matchopcodes(eval(before[8:]), instruction, eval(after[8:]))
            if len(c) == 1:
                baseops -= c  # remove identified opcode from the list of base candidates
                for k in candidates.keys():
                    if k != instruction[0]:
                        candidates[k] -= c  # remove identified opcode from other candidates
                    else:
                        candidates[k] = c  # set identified opcode as the only candidate
            else:
                candidates[instruction[0]] = c

        regs = [0, 0, 0, 0]
        for line in input.readlines():
            if not line.strip():
                continue  # skip empty lines
            instr = list(map(int, line.split()))
            instr[0] = list(candidates[instr[0]])[0]
            regs = execute_instruction(regs, instr)
    return regs[0]


print('Task#1 outcome:', task1())
print('Task#2 outcome:', task2())
