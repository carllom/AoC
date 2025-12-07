data = [x.rstrip('\n') for x in open('day7-1.data', encoding='utf-8')]
beams = [0] * len(data[0])
beams[data[0].index('S')] = 1
splits = 0

for row in data:
    new_beams = [0] * len(row)
    for i, (c, b) in enumerate(zip(row, beams)):
        if b and c == '^':
            new_beams[i - 1] += b
            new_beams[i + 1] += b
            splits += 1
        else:
            new_beams[i] += b
    beams = new_beams

print('Day7-1:', splits)
print('Day7-2:', sum(beams))
