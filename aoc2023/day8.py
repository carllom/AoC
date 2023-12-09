from math import lcm

data = open('day8-1.data').read().splitlines()

g = {}
for node in data[2:]:
    g[node[:3]] = (node[7:10],node[12:15]) # parse line into dict

cn = 'AAA'
steps = 0
while True:
    for s in data[0]:
        cn = g[cn][0 if s=='L' else 1]
        steps += 1
        if cn == 'ZZZ': break
    if cn == 'ZZZ': break

print('Day8-1:',steps)

cn = [[x,x,[],{}] for x in g.keys() if x[2]=='A'] # 'A'-terminated nodes

for c in cn:
    steps = 0
    nextpath = False
    while not nextpath:
        for i,s in enumerate(data[0]):
            c[0] = g[c[0]][0 if s=='L' else 1]
            steps += 1
            if c[0][2] == 'Z': 
                if c[0] in c[3]: # we have been @ node before
                    if c[3][c[0]][1] == i:
                        nextpath = True
                        break # cycle
                else:
                    c[3][c[0]] = (steps, i) # add z-node w. index

            if c[0] == c[1]: # back at start point
                break 

mul = [x[3][next(iter(x[3]))][0] for x in cn]
print('Day8-2:',lcm(*mul))