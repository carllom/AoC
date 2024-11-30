stream = open('day9.txt', 'r').read()
gscore = 0
gcount = 0


def process(d, s):
    global gscore, gcount
    garb = False
    while len(s) > 0:
        if garb:  # process garbage
            if s[0] == '>':
                garb = False
            elif s[0] == '!':
                s = s[1:]
            else:
                gcount += 1
            s = s[1:]
            continue

        if s[0] == '<':  # start garbage
            s = s[1:]
            garb = True
        elif s[0] == '{':  # start group
            s = process(d + 1, s[1:])
        elif s[0] == '}':  # end group
            gscore += d
            return s[1:]
        else:  # next element (,)
            s = s[1:]
    return s


process(0, stream)
print('Outcome task#1', gscore)
print('Outcome task#2', gcount)
