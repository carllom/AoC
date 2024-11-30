def gen(x, f): return (x * f) % 2147483647


g0 = 703  # 65
g1 = 516  # 8921
jcount = 0
for i in range(40000000):
    g0 = gen(g0, 16807)
    g1 = gen(g1, 48271)
    if g0 & 0xFFFF == g1 & 0xFFFF:
        jcount += 1
print('Outcome task#1', jcount)

g0 = 703  # 65
g1 = 516  # 8921
g0res = []
g1res = []
i = 0
while True:
    g0 = gen(g0, 16807)
    g1 = gen(g1, 48271)
    if g0 % 4 == 0:
        g0res.append(g0)
    if g1 % 8 == 0:
        g1res.append(g1)
    if len(g0res) >= 5000000 and len(g1res) >= 5000000:
        break
    i += 1

jcount = 0
for i in range(5000000):
    if g0res[i] & 0xFFFF == g1res[i] & 0xFFFF:
        jcount += 1

print('Outcome task#2', jcount)
