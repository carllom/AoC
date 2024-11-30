banks = [int(x) for x in open('day6.txt').read().split()]
visited = []
while banks not in visited:
    visited.append(list(banks))
    max_val = max(banks)
    max_index = banks.index(max_val)
    banks[max_index] = 0
    for i in range(max_val):
        banks[(max_index + i + 1) % len(banks)] += 1
print('Outcome task#1:', len(visited))
match = visited.index(banks)
print('Outcome task#2:', len(visited)-match)
