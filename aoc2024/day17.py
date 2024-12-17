data = [l.split(':') for l in open('day17.data').read().splitlines()]
a0, b0, c0 = (int(data[0][1]), int(data[1][1]), int(data[2][1]))
prog = list(map(int, data[4][1].split(',')))


def op(op, a, b, c):
    if op < 4:
        return op
    elif op == 4:
        return a
    elif op == 5:
        return b
    elif op == 6:
        return c


def runprog(a, b, c):
    output = []
    pc = 0
    while pc < len(prog):
        ins, lit, cbo = prog[pc], prog[pc+1], op(prog[pc+1], a, b, c)
        match ins:
            case 0:
                a = a // (2**cbo)  # shift right?
            case 1:
                b = b ^ lit  # xor with literal
            case 2:
                b = cbo % 8  # mod
            case 3:
                if a > 0:
                    pc = lit  # jump
                    continue
            case 4:
                b = b ^ c  # xor
            case 5:
                output.append(cbo % 8)  # print
            case 6:
                b = a // (2**cbo)  # shift right?
            case 7:
                c = a // (2**cbo)  # shift right?
        pc += 2
    return output


print('Day17-1', ','.join(map(str, runprog(a0, b0, c0))))


def findmatch(i, okvals, path):
    if i == 0:
        return path
    for okv2 in okvals[i-1]:
        if (okv2 >> 3) & 0b111111 == path[-1] & 0b111111:  # The 6-bit overlap for the register value matches
            p = findmatch(i-1, okvals, path + [okv2])
            if p:
                return p
    return None


okvals = []  # build the list of possible values for each program step
if len(okvals) == 0:
    for pc in range(len(prog)):
        okvals.append(list())
        for i in range(2**11):  # the range that can possibly affect the current step (see discussion below)
            o = runprog(i, 0, 0)
            if o[0] == prog[pc]:
                okvals[pc].append(i)

success = []
for okv in okvals[-1]:
    success = findmatch(len(okvals)-1, okvals, [okv])
    if success:
        break

a0 = success[0] << 3  # build the initial value
for s in success[1:]:
    a0 = (a0 + (s & 0b111)) << 3  # a is shifted 3 steps for each output value

print('Day17-2 Register A:', a0 >> 3, 'Program:', runprog(a0, b0, c0))

'''
Captain RevEng to the rescue!

2,4 b = a % 8 (bst)   b = lowest 3 bits of a
1,1 b = b xor 1 (bxl) toggle b bit 0
7,5 c = a >> b (cdv)  c is a shifted right by b
0,3 a = a >> 3 (adv)  a is a shifted right by 3
1,4 b = b xor 4 (bxl) toggle b bit 2
4,4 b = b xor c (bxc) b = b xor c
5,5 out(b) (out)
3,0 jnz 0 (jnz)

Ee seem to have a bit of bit shuffling going on
output cannot be more than 7, so we know that only the lowest 3 bits are used
b = a[0:2] ^ 1
c = a >> b
discard a lowest 3 bits
b = a[0:2] ^ 4
out (a[a[0:2] ^ 1]:] ^ a[a[0:2] ^ 5])

What are the possible vales to end up with 2 for eample (the first number)?
The bits of a are named ..gfedcbannn where nnn are the lowest 3 bits
 
  a      b      c      b     b^c
(a&7)  (a^1)  (a>>b) (b^4)
 000 -> 001 -> a00 -> 101 -> A01 (cannot be match for 2)
 001 -> 000 -> 001 -> 100 -> 101 (cannot be match for 2)
 010 -> 011 -> cba -> 111 -> CBA matches for xxxx101010
 011 -> 010 -> ba0 -> 110 -> BA0 matches for xxxxx10011
 100 -> 101 -> edc -> 001 -> edC matches for xx011xx100
 101 -> 100 -> dcb -> 000 -> dcb matches for xxx010x101
 110 -> 111 -> gfe -> 011 -> gFE matches for 001xxxx110
 111 -> 110 -> fed -> 010 -> fEd matches for x000xxx111

So in order to output 2, a can be any of the values in the last column.
x bits are don't care bits, so all possible combination of these bits are valid

The first output value is dependent on bit [0..9] bits of a, the next on bit [3..12] and so on.
We can build a table of possible values for each step of the program
'''
