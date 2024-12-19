from collections import defaultdict
indata = open('day19.data').read().splitlines()
patterns = set([p.strip() for p in indata[0].split(',')])
designs = indata[2:]
maxlen = max(map(len, patterns))  # maximum pattern length


def trymatch(dsgn, offsets):
    noffsets = set()
    for o in offsets:  # for each starting offset in the design
        for l in range(maxlen):  # try all possible pattern lengths
            if dsgn[o: o+l] in patterns:
                noffsets.add(o+l)  # found a matching pattern, add new offset
    if len(noffsets) == 0:
        return 0  # dead end (no new offsets found)
    if len(dsgn) in noffsets:
        return 1  # found a match (we reached the end offset of the design)
    return trymatch(dsgn, noffsets)


def trymatch2(dsgn, offsets, matches):
    noffsets = defaultdict(int)  # also keep score of number of matches for each offset
    for o in offsets:
        for l in range(min(len(dsgn)-o, maxlen), 0, -1):  # try largest first, but keep within design bounds
            if dsgn[o:o+l] in patterns:
                if (o+l < len(dsgn)):
                    noffsets[o+l] += offsets[o]  # found a matching pattern, add new offset and propagate score
                else:
                    matches += offsets[o]  # add score to total matches
    if len(noffsets) == 0:
        return matches  # dead end (no new offsets found)
    return trymatch2(dsgn, noffsets, matches)


print('Day19-1', sum(trymatch(d, set([0])) for d in designs))
print('Day19-2', sum(trymatch2(d, {0: 1}, 0) for d in designs))
