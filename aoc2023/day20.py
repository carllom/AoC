from collections import deque
from math import lcm

data = {} # module: name -> [receivers], type, state
pulses = deque() # pulse: receiver, state, sender

def setup():
    global data
    data = dict([(l.split('->')[0].strip()[1:],[[x.strip() for x in l.split('->')[1].split(',')],l.split('->')[0].strip()[0],False]) for l in open('day20-1.data').read().splitlines()]) # name -> dest(0),type(1),state(2)
    for rcv in [m for m in data.items() if m[1][1] == '&']: # fix conjunction (&) memory - all inputs need to be prepopulated
        asd = [(m[0],False) for m in data.items() if rcv[0] in m[1][0]]
        data[rcv[0]][2] = dict(asd)
    pulses.clear()

def simulate(pul:dict) -> dict:
    while pulses:
        sig = pulses.popleft()
        if sig[0] not in data.keys(): continue
        m = data[sig[0]]
        out = None
        match m[1]:
            case 'b': # broadcaster
                m[2] = sig[1] # state
                out = m[2]
            case '%': # flip-flop
                if sig[1]: continue
                if not sig[1]: m[2] = not m[2]
                out = m[2]
            case '&': # conjunction (and)
                m[2][sig[2]] = sig[1] # set input memory
                out = not all(m[2].values())
        for b in m[0]: pulses.append([b, out, sig[0]]) # propagate pulse
        pul[out] += len(m[0]) # count pulses
    return pul

def pushbutton(pul:dict) -> dict:
    pulses.append(['roadcaster',False,'button'])
    pul[False] +=1
    return simulate(pul)

setup()
pulsecounter = dict([(False,0),(True,0)])
for _ in range(1000): pulsecounter = pushbutton(pulsecounter)

print('Day20-1',pulsecounter[False]*pulsecounter[True])

# Part 2 was solved analytically, see day20-2.md for description. Circuit reverse-engineering FTW!
print('Day20-2',lcm(*[3847,3823,3877,4001])) # Counter reset values
