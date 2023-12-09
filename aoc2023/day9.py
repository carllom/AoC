def calc(l):
    while any(l[-1]): # while not all 0:s
        l.append([x1-x2 for (x1,x2) in zip(l[-1][1:], l[-1][:-1])]) # generate diff line
    l[-1].append(0) # extend last 0
    for x in reversed(range(len(l)-1)):
        l[x] += [l[x+1][-1] + l[x][-1]] # extend upwards
    return l[0][-1]

s=0
with open('day9-1.data') as input:
    for line in input:
        l = [list(map(int,line.split()))]
        s+= calc(l)
print('Day9-1:', s)

s=0
with open('day9-1.data') as input:
    for line in input:
        l = [list(map(int,reversed(line.split())))] # just flip the input to add to the 'beginning'
        s+=calc(l)
print('Day9-2:',s)
