from collections import defaultdict

isecret = list(map(int,open('day22.data').read().splitlines()))

def next_num(n):
    nn = ((n<<6) ^ n) % 16777216
    nn= ((nn>>5) ^ nn) % 16777216 
    nn= ((nn<<11)^nn) % 16777216
    return nn

for i in range(len(isecret)):
    s0 = isecret[i]
    for j in range(2000):
        isecret[i] = next_num(isecret[i])

print('Day22-1', sum(isecret), 18941802053)

isecret = list(map(int,open('day22.data').read().splitlines()))
pricetotals = defaultdict(int)
for i in range(len(isecret)):
    secl = [(isecret[i]%10, None)]
    s0 = isecret[i]
    already = set() # sequences we have already used for this buyer
    for j in range(2000):
        isecret[i] = next_num(isecret[i])
        bcost = isecret[i]%10
        bdelt = bcost-secl[j][0]
        secl.append((bcost, bdelt, chr(65+9+bdelt)))
        if len(secl) > 4:
            ptn = ''.join([g[2] for g in secl[-4:]])
            if ptn in already:
                continue # we have already matched this pattern
            already.add(ptn)
            pricetotals[ptn] += bcost
    isecret[i] = (s0, isecret[i], secl)

# find the maximum value in dictionary pricetotals
maxval = max(pricetotals.values())

# bonus: find the key for the max value in pricetotals
# maxkey = max(pricetotals, key=pricetotals.get)
# decoded_key = [ord(c)-65-9 for c in maxkey]

print('Day22-2', maxval, 2218)
