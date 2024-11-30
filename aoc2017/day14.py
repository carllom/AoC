from functools import reduce


def rearrange(lengths, l, p0, skip):
    for length in lengths:
        l = list(reversed(l[:length])) + l[length:]  # flip portion
        pos = (length + skip) % len(l)
        p0 += length + skip
        l = l[pos:] + l[:pos]  # realign new pos to 0 (avoids overlapping reversal)
        skip += 1
    return l, p0, skip


def knothash(key):
    lengths = [ord(i) for i in key] + [17, 31, 73, 47, 23]
    l = list(range(256))
    p0 = 0
    skip = 0

    for r in range(64):
        l, p0, skip = rearrange(lengths, l, p0, skip)
    l = l[-(p0 % len(l)):] + l[:-(p0 % len(l))]  # realign to 0
    h16 = [reduce(lambda a, b: a ^ b, l[i:i + 16], 0) for i in range(0, len(l), 16)]  # xor hash groups
    return ''.join([hex(i)[2:].zfill(2) for i in h16])


def flood(grid, row, col, group):
    if row < 0 or row >= 128 or col < 0 or col >= 128:
        return
    if grid[row][col] == '1':
        grid[row][col] = group
        flood(grid, row+1, col, group)
        flood(grid, row-1, col, group)
        flood(grid, row, col+1, group)
        flood(grid, row, col-1, group)


def bininfo(num):
    b = bin(num)[2:].zfill(4)
    return (sum([1 for bit in b if bit == '1']), b)


input = 'jzgqcdpd'
ones = dict([(hex(i)[2:], bininfo(i)) for i in range(16)])  # precompute binary info (#ones and binary string) for 0-f
total = 0
hashes = []
for row in range(128):
    hashes.append(knothash(f'{input}-{row}'))
    total += sum([ones[h][0] for h in hashes[row]])
print('Outcome part#1', total)

grid = [[' ' for _ in range(128)] for _ in range(128)]
for row in range(128):
    for col in range(128):
        grid[row][col] = ones[hashes[row][col//4]][1][col % 4]
numgroups = 0
for row in range(128):
    for col in range(128):
        if grid[row][col] == '1':
            flood(grid, row, col, '.')
            numgroups += 1
print('Outcome part#2', numgroups)
