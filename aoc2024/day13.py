from re import findall
data = open('day13.data').read().splitlines()
acost, bcost, machines = 3, 1, []

for i in range(0, len(data), 4):
    abtn = list(map(int, findall(r'\+(\d+)', data[i])))
    bbtn = list(map(int, findall(r'\+(\d+)', data[i+1])))
    prize = list(map(int, findall(r'\=(\d+)', data[i+2])))
    machines.append([abtn, bbtn, prize])  # [a-btn-move, b-btn-move, prize-position]

tot = 0
for m in machines:
    for i in range(100):  # test max 100 buttonpresses
        a_tot = m[0][0] * i, m[0][1]*i  # calc position for pressing button a i times
        if (m[2][0] - a_tot[0]) % m[1][0] == 0 and (m[2][1] - a_tot[1]) % m[1][1] == 0:  # is the distance left to the target divisible by b (for both x and y)
            bc_x = (m[2][0] - a_tot[0]) // m[1][0]  # calculate the number of b buttonpresses to reach target x
            if (bc_x == (m[2][1] - a_tot[1]) // m[1][1]):  # must be the same number of presses for y
                tot += i * acost + bc_x * bcost  # add the token costs for button a and b
                break
print('Day13-1', tot)

tot = 0
for m in machines:
    m[2] = [m[2][0] + 10000000000000, m[2][1] + 10000000000000]  # increase the target position by a lot
    ax, ay, bx, by, tx, ty = m[0][0], m[0][1], m[1][0], m[1][1], m[2][0], m[2][1]  # rename for easier reading
    bc = (ay*tx - ax*ty) / (ay*bx - ax*by)  # equation for bc (see explanation below)
    ac = (ty - by * bc) / ay  # equation for ac (see explanation below)
    if (ac % 1 == 0 and bc % 1 == 0):  # the buttonpresses must be integers, otherwise it is not a solution
        tot += round(ac) * acost + round(bc) * bcost

print('Day13-2', tot)

'''
We can formulate the b-problem as 2 equations:
ax * ac + bx * bc = totx
ay * ac + by * bc = toty

where ac and bc are the number of button presses for a and b respectively.
ax, ay, bx, by are the distance moved in x and y for a and b respectively.
totx and toty is the target position.
We have 2 unknowns and 2 equations, so we can solve this by substitution.
First we solve for ac in the first equation:
ac = (totx - bx * bc) / ax

We then substitute it into the second equation:
ay * ((totx - bx * bc) / ax) + by * bc = toty

expand equation to become:
ay*totx/ax - ay*bx*bc/ax + by*bc = toty

extract bc:
ay*totx/ax + bc*(by - ay*bx/ax) = toty
bc = (toty - ay*totx/ax) / (by - ay*bx/ax)

simplify equation by multiplying with ax: (Simplification is vital to avoid floating point errors!! This cost me a lot of time)
bc = (ax*toty - ay*totx) / (ax*by - ay*bx)

we can now calculate bc and ac is the same equaion as before:
ac = (totx - bx * bc) / ax
'''
