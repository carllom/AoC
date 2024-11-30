from functools import reduce

lengths = [int(i) for i in open('day10.txt', 'r').read().split(',')]


def rearrange(l, p0, skip):
    global lengths
    for length in lengths:
        l = list(reversed(l[:length])) + l[length:]  # flip portion
        pos = (length + skip) % len(l)
        p0 += length + skip
        l = l[pos:] + l[:pos]  # realign new pos to 0 (avoids overlapping reversal)
        skip += 1
    return l, p0, skip


lq, p0, _ = rearrange(list(range(256)), 0, 0)
print('Outcome task#1', lq[-(p0 % len(lq))] * lq[-((p0 - 1) % len(lq))])

lengths = [ord(i) for i in open('day10.txt', 'r').read()] + [17, 31, 73, 47, 23]
l = list(range(256))
p0 = 0
skip = 0

for r in range(64):
    l, p0, skip = rearrange(l, p0, skip)
l = l[-(p0 % len(l)):] + l[:-(p0 % len(l))]  # realign to 0
h16 = [reduce(lambda a, b: a ^ b, l[i:i + 16], 0) for i in range(0, len(l), 16)]  # xor hash groups

print('Outcome task#2', ''.join([hex(i)[2:].zfill(2) for i in h16]))
