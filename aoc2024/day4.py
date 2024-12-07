data = open('day4-1.data').read().splitlines()
count = 0

for line in data:  # use standard string count for horizontal
    count += line.count('XMAS')
    count += line.count('SAMX')

match = ('XMAS', 'SAMX')
for y in range(3, len(data)):
    for x in range(0, len(data[y])):
        v = ''.join([data[y-3][x], data[y-2][x], data[y-1][x], data[y][x]])  # vertical
        if v in match:
            count += 1
        if x > 2:
            x1 = ''.join([data[y][x], data[y-1][x-1], data[y-2][x-2], data[y-3][x-3]])  # diagonal \
            if x1 in match:
                count += 1
            x2 = ''.join([data[y-3][x], data[y-2][x-1], data[y-1][x-2], data[y][x-3]])  # diagonal /
            if x2 in match:
                count += 1

print('Day4-1', count)

count = 0
match = ('MAS', 'SAM')
for y in range(2, len(data)):
    for x in range(2, len(data[y])):
        x1 = ''.join([data[y][x], data[y-1][x-1], data[y-2][x-2]])  # diagonal \
        x2 = ''.join([data[y][x-2], data[y-1][x-1], data[y-2][x]])  # diagonal /
        if x1 in match and x2 in match:
            count += 1

print('Day4-2', count)
