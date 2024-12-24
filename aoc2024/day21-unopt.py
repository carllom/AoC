from collections import defaultdict
import heapq
from re import findall
import math
import os

data = open('day21.data').read().splitlines()

#  ^A
# <v>
#
# 789
# 456
# 123
#  0A

nrobots = 25

directions = {'^': (0, -1), 'v': (0, 1), '<': (-1, 0), '>': (1, 0)}
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

legalpaths = defaultdict(list)

# find all legal moves from start key to end key for the given pad
def find_all_legal_paths(pad:dict, w, h, start, end):
    
    p = []
    if pad == dirpad:
        p = legalpaths[start+end]
    if p:
        return p # return cached result

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
    legalpaths[start+end] = p
    return p

def expand_sequence(pad, w, h, curr, seq):
    expand = []
    for i in range(len(seq)):
        expand.append(find_all_legal_paths(pad, w, h, curr, seq[i]))
        expand.append(['A'])  # each key in sequence is followed by 'A' to activate it
        curr = seq[i]

    # expand = normalize_moves(expand)
    # return curr, expand
    # Compress consecutive single-element arrays into a single string
    compressed_expand = []
    temp_str = ""
    for item in expand:
        if len(item) == 1:
            temp_str += item[0]
        else:
            if temp_str:
                compressed_expand.append([temp_str])
                temp_str = ""
            compressed_expand.append(item)
    if temp_str:
        compressed_expand.append([temp_str])
    return curr, compressed_expand

def expand_sequence2(pad, w, h, curr, seq):
    expand = ['']
    for i in range(len(seq)):
        # for p in find_all_legal_paths(pad, w, h, curr, seq[i]):
        expand = [x+p for x in expand for p in find_all_legal_paths(pad, w, h, curr, seq[i])]
        expand = [x+'A' for x in expand]  # each key in sequence is followed by 'A' to activate it
        curr = seq[i]

    return curr, expand

def expand_sequence3(pad, w, h, curr, seq): # input seq must be normalizd (i.e no options in the sequence)
    expand = []
    build = '' # build sequence of keypresses as long as they are unconditional
    for i in range(len(seq)):
        paths = find_all_legal_paths(pad, w, h, curr, seq[i])
        if len(paths) == 1:
            build += paths[0] + 'A'
        else:
            if build:
                expand.append(build)
            build = 'A' # trailing keypress for the options begins next sequence
            expand.append(paths)
        curr = seq[i]
    if build:
        expand.append(build)
    return curr, expand


def encode_sequence(pad, w, h, seq):
    pos = pad['A']
    encoded = ''
    for i in range(len(seq)):
        if seq[i] == 'A':
            for k, v in pad.items():
                if v == pos:
                    encoded += k
        else:
            off = directions[seq[i]]
            pos = (pos[0] + off[0], pos[1] + off[1])
    return encoded

def expand_moves(pad, w, h, curr, moves):
    expand = []
    for options in moves: # moves are 'mandatory' so we can just expand them
        xpoptions = []
        for option in options: # we need to expand all options, but store them separately
            curr, xpmoves = expand_sequence(pad, w, h, curr, option)
            if len(options) == 1:
                expand.extend(xpmoves)
            else:
                xpoptions.append(xpmoves)
        if len(xpoptions) > 0:
            expand.append(xpoptions)
    return curr, expand

def normalize_moves(moves):
    # Normalize the moves by compressing consecutive single-element arrays into a single string
    compressed_moves = []
    temp_str = ""
    for item in moves:
        if isinstance(item, list):
            compressed_moves.append(normalize_moves(item))
        elif len(item) == 1:
            temp_str += item[0]
        else:
            if temp_str:
                compressed_moves.append([temp_str])
                temp_str = ""
            compressed_moves.append(item)
    if temp_str:
        compressed_moves.append([temp_str])
    return compressed_moves

def assemble_moves(assembled:list, moves:list):
    # Assemble the moves 

    for item in moves:
        if len(item) == 1:
            assembled = [p+item[0] for p in assembled] if assembled else [item[0]]
        else:
            new_ass =[]
            for it in item:
                new_ass.extend([p+it for p in assembled] if assembled else [it])
            assembled = new_ass
    return assembled

def minimized_sequences(pad, w, h, inputs):
    minl = []
    minl_len = len(inputs[0])*1000
    for inp in inputs:
        _, moves = expand_sequence2(pad, w,h, 'A', inp) # the dirpad moves needed to control the first dirpad robot
        dplen = len(moves[0]) # the keypresses always have the same length for the same input
        if dplen < minl_len:
            minl.clear() # we have new minimum sequences, clear all the old
        if dplen <= minl_len:
            minl.extend(moves)
            minl_len = dplen
    return minl

def seqlen(seq): # total length of sequence (can include option lists)
    l = 0
    for s in seq:
        if isinstance(s, list):
            l += len(s[0])
        else:
            l += len(s)
    return l

# are we sure about the minimums?

# expanding to < or v makes a difference in the next iteration, since < is farther away from A than v
# the priority when choosing between otherwise equal length expansions would be
# 1: ^ >, 2: v, 3: <
# perhaps an extra step is needed to weed out the longer expansions
# compare all the lowest length expansions and score on key distances to A where they differ

kd = {'^': 1, '>': 2, 'v': 3, '<': 4} # distance to A for the different directions
def seq_order(seq):
    return 0
    # for s in seq:
    #     if isinstance(s, list):

    #     return kd[seq[0][0]]
    # return kd[seq[0]]

def minimized_sequences3(pad, w, h, input): # input can have options
    # the purpose of this is to expand the input, but also to choose the minimal length expansion out of the possible input combinations
    # all expansions for a single input carries the same length, so we only have to do a single test for each input combination
    # minl=[]
    # minl_len = seqlen(input) * 1000
    output = []
    curr = 'A'
    for i in range(len(input)):
        if isinstance(input[i], list): # input has options, find the minimal length expansion
            expopt = []
            heapq.heapify(expopt)
            xpseq = [(*expand_sequence3(pad, w, h, curr, o),o) for o in input[i]]
            for xp in [(seqlen(e[1]), seq_order(e[1]), e[1], e[2]) for e in xpseq]:
                heapq.heappush(expopt, xp)
            best = heapq.heappop(expopt)
            output.extend(best[2])
            curr = best[3][-1]
        else:
            _, lx = expand_sequence3(pad, 3, 2, curr, input[i])
            output.extend(lx) # expand the unconditional sequence
            curr = input[i][-1] # the last key in the sequence is the current key

    i=1
    while i < len(output):
        if isinstance(output[i],str) and isinstance(output[i-1],str):
            output[i-1] += output[i]
            del output[i]
        else:
            i += 1
    return output

# new try for expansion and minimization
# what if we recurse the first element down to max robots
# elements are not dependent on the following elements for minimization since no matter what they are
# you still have to do the keypresses for the current element
# are we dependent on the previous elements for minimization?
# what about the transition between elemets, specifically for options?
# options are dependent on the previous element, and affect the next element due to the current key position
#
# what are options? options are paths between two fixed keys
# what paths are preferable when considering expansion?
# there is a limited number of paths between two keys and there must always be a best choice considering expansion, right?

# 5 keys in total gives a total of 5*4 20 start/end combinations.
# only an handful of them have distance > 1 which is required for having options

# so do a dictionary of all key combinations and their paths
# expand them and take the shortest combination - this is the preferred option - always(?)

def generate_expansions(pad, w, h):
    keys = ['A','<','>','^','v']
    xpdict = defaultdict(str)
    for sk in keys:
        for ek in keys:
            legal = find_all_legal_paths(pad, w, h, sk, ek)
            opdict = defaultdict(str)
            for x in legal:
                if x == '':
                    opdict[x] = 0 # trivial case - start and end are the same
                    continue
                _,seq = expand_sequence2(pad, w, h, sk, x)
                opdict[x] = len(seq[0])

            # if these expansions are shorter than the current
            # then x (from legal) is the preferred option
            # find the key for the smallest value in opdict
            smop = min(opdict, key=opdict.get)
            xpdict[sk+ek] = smop + 'A'
    return xpdict

def minimized_sequence4(input, xpdict): # must be normalized as per usual
    sk = 'A'
    output = ''
    for i in range(len(input)):
        output += xpdict[sk+input[i]]
        sk = input[i]
    return output

def minimized_sequence45(input, xpdict): # must be normalized as per usual
    output = ''
    for i in range(1, len(input)):
        output += xpdict[input]
        sk = input[i]
    return output



levelcache = [defaultdict(int) for _ in range(nrobots+1)]
levelcache2 = [defaultdict(str) for _ in range(nrobots+1)]
first = [True for _ in range(nrobots+1)]
atpos = ['A' for _ in range(nrobots+1)]

def minimized_sequence5(inp, xpdict, iter): # must be normalized as per usual
    if iter==0:
        return len(inp),inp
    
    inp = atpos[iter]+inp # add the start condition if we have never been here before

    sk = inp[0]
    size = 0
    tres = ''
    for i in range(1,len(inp)):
        exp = sk+inp[i]
        if exp in levelcache[iter]:
            size += levelcache[iter][exp]
            # tres += levelcache2[iter][exp]
            sk = inp[i]
            atpos[iter] = sk
            continue
        nexp = xpdict[exp]
        if len(nexp) == 1:
            size += 1
            # tres += nexp
            sk = inp[i]
            atpos[iter] = sk
            print('level', iter, 'iteration',i, 'repeat',exp, '->', xpdict[exp], 'size',1, 'total',size)
            continue
        l,res = minimized_sequence5(nexp, xpdict, iter-1)
        # tres+=res
        # levelcache2[iter][exp] = res
        levelcache[iter][exp] = l
        size += l
        sk = inp[i]
        print('level', iter, 'iteration',i, 'expanding',exp, '->', xpdict[exp], 'size',l, 'total',size)
        atpos[iter] = sk
    return size,tres

def minimized_sequence6(inexp, xpdict, iter): # must be normalized as per usual
    if iter==0:
        return len(inexp)
    t = 0
    exp = xpdict[inexp]
    if len(exp) == 1:
        t += 1 # repeated keypresses do not need to be propagated
    for a in range(len(exp)-1):
        t += minimized_sequence6(exp[a:a+2], xpdict, iter-1)
    return t
# agh, what to do when xpdict returns a single value. While the input is a sequence of the same keypresses
# we need to iterate it somehow. perhaps we need to peek forward and eat all identical keypresses ?
# what happens in an expansion for identical keypresses? it will have an expanded prefix, followed by
# repetitions of 'A'. In the next expansion, those repeated 'A':s will propagate to the next expansion and so on.
# ^^^ results in AAA, resulting in AAA, resulting in AAA.
# this would mean that repetitions could be compressed into a single keypress, and just added to the total score.
# for example: ^^^ above would generate a prefix taking us to ^, followed by an A and then 2 more A:s for the repetitions.
# These 2 A:s can just be added to the total score and skipped in the input.


# it still expands to far, but we should be able to expand the first element all 25 times and then do the next!
# i.e. solve it iteratively

# tot = 0
# for code in data:
#     _, npmoves = expand_sequence2(numpad, 3, 4, 'A', code) # the dirpad moves needed to control the numpad robot to enter the code
#     for i in range(2):
#         npmoves = minimized_sequences(dirpad, 3, 2, npmoves)
    
#     complexity = len(npmoves[0]) * int(code[:-1])
#     tot += complexity
#     print(code, 'has complexity',complexity, 'and minlen', len(npmoves[0]))

# print('Day21-1', tot)

# xpdict = generate_expansions(dirpad, 3, 2) # this is the preferred expansion for all key combinations

xpdict = {
    'AA': 'A',
    'A<': 'v<<A', ## cannot cross empty
    'A>': 'vA',
    'A^': '<A',
    'Av': '<vA', ##
    '<A': '>>^A', ## cannot cross empty
    '<<': 'A',
    '<>': '>>A',
    '<^': '>^A', ## cannot cross empty
    '<v': '>A',
    '>A': '^A',
    '><': '<<A',
    '>>': 'A',
    '>^': '<^A', ##
    '>v': '<A',
    '^A': '>A',
    '^<': 'v<A', ## Cannot cross empty
    '^>': 'v>A', ##
    '^^': 'A',
    '^v': 'vA',  
    'vA': '^>A', ##
    'v<': '<A',
    'v>': '>A',
    'v^': '^A',
    'vv': 'A'
}

tot = 0
for code in data:
    _, npmoves = expand_sequence2(numpad, 3, 4, 'A', code) # the dirpad moves needed to control the numpad robot to enter the code

    bestnum = defaultdict(list)
    for sseq in npmoves:
        ms = minimized_sequence4(sseq, xpdict)
        bestnum[len(ms)].append(sseq)
    
    # find the best option (smallest key) in bestnum
    best = bestnum[min(bestnum.keys())]

    # npmoves = best[0] # these are the keypresses needed to direct the numpad robot to enter the code

    # sk = 'A'
    # numkp = 0
    # for i in range(len(npmoves)):
    #     numkp += minimized_sequence6(sk+npmoves[i], xpdict, 25)
    #     sk = npmoves[i]

    # sexp = npmoves
    nrobots = 25
    # for q in range(nrobots):
    #     sexp = minimized_sequence4(sexp, xpdict)
    # print(len(sexp), sexp)

    # first = [True for _ in range(nrobots+1)]
    numkp = []
    for i in range(len(npmoves)):
        atpos = ['A' for _ in range(nrobots+1)]
        numkp.append(minimized_sequence5(npmoves[i], xpdict, nrobots)[0])
        print('Code',code, 'gives', numkp, 'keypresses')
    
    complexity = min(numkp) * int(code[:-1])
    tot += complexity
    print(code, 'has complexity',complexity, 'and minlen', len(npmoves[0]))
print('Day21-2', tot)
# 363730683118 too low
# 226411735645566 too high