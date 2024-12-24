from collections import defaultdict

wires = defaultdict(None)
gates = defaultdict(list)
data = open('day24.data').read().splitlines()
stage=0
for l in [d.split() for d in data]:
    if len(l) == 0:
        stage += 1
    elif stage==0:
        wires[l[0][:-1]] = l[-1].strip() == '1'
    else:
        gates[l[-1]] = l[:-2]

def calcop(wrs, a, op, b):
    if op == 'AND':
        return getop(wrs, a) & getop(wrs, b)
    elif op == 'OR':
        return getop(wrs, a) | getop(wrs, b)
    elif op == 'XOR':
        return getop(wrs, a) ^ getop(wrs, b)

def getop(wrs, w):
    if w not in wrs or wrs[w] is None:
        wrs[w] = calcop(wrs, *gates[w])
    return wrs[w]

zs = sorted(list(filter(lambda x:x.startswith('z'),gates.keys())))
bits = [getop(wires, z) for z in zs]
print('Day24-1', int(''.join(['1' if bit else '0' for bit in reversed(bits)]), 2))

validationdict = defaultdict(str)

def setwires(wrs, x, y):
    for k in wrs.keys():
        if k.startswith('x'):
            wrs[k] = x >> int(k[1:]) & 1
        elif k.startswith('y'):
            wrs[k] = y >> int(k[1:]) & 1
        else:
            wrs[k] = None

def find_gate(a, op, b):
    for z in gates.keys():
        if set([gates[z][0],gates[z][2]]) == set([a,b]) and gates[z][1] == op:
            return z
    return None

def find_gate2(a, op):
    for z in gates.keys():
        if gates[z][1] == op and (gates[z][0] == a or gates[z][2] == a):
            return z, gates[z][0] if gates[z][0] != a else gates[z][2] # return other part of the gate
    return None, None

def check_expected_layout(z):
    bit = int(z[1:])
    hs_xor = find_gate(f'x{z[1:]}', 'XOR', f'y{z[1:]}') # low bit add
    hs_and = find_gate(f'x{z[1:]}', 'AND', f'y{z[1:]}') # top bit add
    c_hs_and, xx_hs_xor1 = find_gate2(validationdict[f'c{bit-1:02}'], 'AND') # last carry and half sum (top bit carry add)
    c_hs_xor, xx_hs_xor2 = find_gate2(validationdict[f'c{bit-1:02}'], 'XOR') # last carry xor half sum (low bit carry add)
    if c_hs_xor != z: # last carry xor half sum is expected to be the sum bit
        return c_hs_xor, z
    if xx_hs_xor1 != hs_xor or xx_hs_xor2 != hs_xor: # half sum of last carry is expected to be with half sum xor
        return hs_xor, xx_hs_xor1
    c = find_gate(c_hs_and, 'OR', hs_and) # top bit carry add
    return None,None # no swap needed


def translate_expected_layout(bit):
    hs_xor = find_gate(f'x{bit:02}', 'XOR', f'y{bit:02}') # low bit add
    validationdict[f'hsx{bit:02}'] = hs_xor
    hs_and = find_gate(f'x{bit:02}', 'AND', f'y{bit:02}') # top bit add
    validationdict[f'hsa{bit:02}'] = hs_and
    if bit == 0: # special case for bit 0 - we have no carry in so carry is directly from the half sum
        validationdict[f'c{bit:02}'] = hs_and
        return
    lc = validationdict[f'c{bit-1:02}']
    c_hs_and = find_gate(hs_xor, 'AND', lc) # last carry and half sum (top bit carry add)
    validationdict[f'csa{bit:02}'] = c_hs_and
    validationdict[f's{bit:02}'] = find_gate(lc, 'XOR', hs_xor) # last carry xor half sum (low bit carry add)
    validationdict[f'c{bit:02}'] = find_gate(c_hs_and, 'OR', hs_and) # top bit carry add

def testcircuit():
    swapped = set()
    zs.sort()
    zi = 0
    while zi < len(zs)-1: # skip the top bit since it is a special case taking no xn or yn
        z = zs[zi]
        for i in range(4):
            one = 2**zi
            setwires(wires, one if i%2==1 else 0, one if i//2==1 else 0)
            zn = getop(wires, z)
            if zn != i%2 ^ i//2:
                sw1, sw2 = check_expected_layout(z)
                if sw1:
                    gates[sw1], gates[sw2] = gates[sw2], gates[sw1] # swap gates
                    zi = 0 # restart validation just in case we mess up
                    swapped.update([sw1,sw2]) # keep track of swapped gates
                    break
        translate_expected_layout(zi)
        zi += 1
    return swapped

print('Day24-2', ','.join(sorted(testcircuit())))
