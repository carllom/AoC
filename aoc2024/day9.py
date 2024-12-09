src = [int(x) for x in open('day9-1.data').read()]
dst = []
file_index = 0
i = 0
while i < len(src):
    if len(src) % 2 == 0:  # ends with space section - remove it
        src.pop()
        continue

    if i % 2 == 0:  # file blocks
        dst += [file_index]*src[i]  # append file blocks
        file_index += 1
    else:  # space blocks
        free_space = src[i]  # number of spaces
        while free_space > 0:
            tk = min(free_space, src[-1])  # number of spaces to take
            free_space -= tk  # adjust remaining spaces
            src[-1] -= tk  # adjust remaining file size
            dst += [(len(src) - 1) // 2]*tk  # append moved file blocks
            if src[-1] == 0:
                src = src[:-2]  # remove file and trailing space
                if i >= len(src):  # bounds check since we shrunk the source
                    break  # finished
    i += 1

print('Day9-1', sum([i*dst[i] for i in range(len(dst))]))

dst = [(-1 if i % 2 != 0 else i//2, int(b)) for i, b in enumerate(open('day9-1.data').read())]  # prepopulate dst with file/space segments
file_moved = True
last_file_index = len(dst)-1
while file_moved:
    file_moved = False
    for fi in range(last_file_index, 0, -1):  # search files rtl
        if dst[fi][0] < 0:  # skip space segments
            continue

        ftm = dst[fi]
        for i in range(1, fi):  # search destination space ltr
            if dst[i][0] == -1 and dst[i][1] >= ftm[1]:  # enough space
                dst[i] = (dst[i][0], dst[i][1]-ftm[1])  # adjust remaining space
                dst[fi] = (-1, ftm[1])  # clear old file segment (replace with space)
                dst.insert(i, ftm)  # insert file segment at new location
                file_moved = True
                break

        last_file_index = fi
        if file_moved:
            break

idx = 0
tot = 0
for block in dst:
    if block[0] > -1:
        tot += sum(block[0]*(list(range(idx, idx+block[1]))))
    idx += block[1]

print('Day9-2', tot)
