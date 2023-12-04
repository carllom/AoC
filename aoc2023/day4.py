import math

s=0
with open('day4-1.data') as input:
    for line in input:
        card = line.split(':')
        bar = card[1].index('|')
        wins = set(map(int,card[1][:bar].split()))
        nums = set(map(int,card[1][bar+1:].split()))
        s+=math.floor(2 ** (len(wins & nums)-1)) # use set union to find matches
print('Day4-1:',s)

cards = {}
with open('day4-1.data') as input:
    for line in input:
        card = line.split(':')
        bar = card[1].index('|')
        wins = set(map(int,card[1][:bar].split()))
        nums = set(map(int,card[1][bar+1:].split()))
        cards[int(card[0].split()[1])] = [len(wins & nums),1] 

for cnum in cards:
    for win in range(cnum+1,cnum+1+cards[cnum][0]):
        cards[win][1] += 1 * cards[cnum][1]

print('Day4-2:', sum(map(lambda c: c[1], cards.values())))
