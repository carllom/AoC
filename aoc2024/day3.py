import re


def domuls(data):
    return sum([int(x[0])*int(x[1]) for x in re.findall(r'mul\((\d{1,3}),(\d{1,3})\)', data)])


data = open('day3-1.data').read()
print('Day3-1', domuls(data))

active = True
activeat = 0
match = []

for m in re.finditer(r'don\'t|do', data):
    if m.group() == 'don\'t' and active:  # store "active" block at on-off transition
        match.append(data[activeat:m.start()])
    elif m.group() == 'do' and not active:  # register off-on transition position
        activeat = m.end()
    active = m.group() == 'do'

if active:  # store tail block
    match.append(data[activeat:])

print('Day3-2', sum([domuls(m) for m in match]))
