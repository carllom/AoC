from collections import defaultdict
import heapq
import math

data = open('day21.data').read().splitlines()
nrobots = 25
directions2 = {(0, -1): '^', (0, 1): 'v', (-1, 0): '<', (1, 0): '>'}
dirpad = {
    ' ': (0,0), '^': (1,0), 'A': (2,0),
    '<': (0,1), 'v': (1,1), '>': (2,1),
}
numpad = {
    '7': (0, 0), '8': (1, 0), '9': (2, 0),
    '4': (0, 1), '5': (1, 1), '6': (2, 1),
    '1': (0, 2), '2': (1, 2), '3': (2, 2),
    ' ': (0, 3), '0': (1, 3), 'A': (2, 3)
}

def find_all_legal_paths(pad:dict, w, h, start, end): # find all legal moves from start key to end key for the given pad
    p = []
    start_pos = pad[start]
    end_pos = pad[end]
    space = pad[' ']
    xstep = math.copysign(1, end_pos[0] - start_pos[0]) if end_pos[0] != start_pos[0] else 0
    ystep = math.copysign(1, end_pos[1] - start_pos[1]) if end_pos[1] != start_pos[1] else 0
    q = [(0, start_pos, '')]
    while q:
        steps, pos, path = heapq.heappop(q)
        if pos == end_pos:
            p.append(path)
            continue
        if xstep != 0:
            xnext = pos[0] + xstep
            if -1 < xnext < w and space != (xnext,pos[1]):
                heapq.heappush(q, (steps+1, (xnext, pos[1]), path + directions2[(xstep, 0)]))
        if ystep != 0:
            ynext = pos[1] + ystep
            if -1 < ynext < h and space != (pos[0],ynext):
                heapq.heappush(q,(steps+1, (pos[0], ynext), path + directions2[(0, ystep)]))
    return p

def expand_sequence(pad, w, h, curr, seq):
    expand = ['']
    for i in range(len(seq)): # each key in sequence is followed by 'A' to activate it
        expand = [q+'A' for q in [x+p for x in expand for p in find_all_legal_paths(pad, w, h, curr, seq[i])]]
        curr = seq[i]
    return expand


levelcache = [defaultdict(int) for _ in range(nrobots)]
def minimized_sequence(inp:str, xpdict:dict, iter:int):
    if iter < 0:
        return len(inp) # keypresses for final level
    
    inp = 'A'+inp # add the start condition (where the robot is)
    size = 0
    for i in range(len(inp)-1):
        exp = inp[i:i+2]
        if exp in levelcache[iter]: # use cached result
            size += levelcache[iter][exp]
            continue
        nexp = xpdict[exp]
        if len(nexp) == 1: # repetition only expands into a repeated keypress which expands into itself
            size += 1
            continue # we can stop here as expansion does not add any keypresses
        l = minimized_sequence(nexp, xpdict, iter-1)
        levelcache[iter][exp] = l
        size += l
    return size

xpdict = {
    'AA': 'A', 'A<': 'v<<A', 'A>': 'vA', 'A^': '<A', 'Av': '<vA',
    '<A': '>>^A', '<<': 'A', '<>': '>>A', '<^': '>^A', '<v': '>A',
    '>A': '^A', '><': '<<A', '>>': 'A', '>^': '<^A', '>v': '<A',
    '^A': '>A', '^<': 'v<A', '^>': 'v>A', '^^': 'A', '^v': 'vA',  
    'vA': '^>A', 'v<': '<A', 'v>': '>A', 'v^': '^A', 'vv': 'A'
}

def run_sequence(nrobots):
    tot = 0
    for code in data:
        npmoves = expand_sequence(numpad, 3, 4, 'A', code) # the dirpad moves needed to control the numpad robot to enter the code
        numkp = []
        for i in range(len(npmoves)): # try for all possible numpad expansions - not necessarily the shortest...
            numkp.append(minimized_sequence(npmoves[i], xpdict, nrobots-1))
        tot += min(numkp) * int(code[:-1]) # calc complexity
    return tot
print('Day21-1', run_sequence(2))
print('Day21-2', run_sequence(25))
