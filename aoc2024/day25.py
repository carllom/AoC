import numpy as np
import itertools

schem = open('day25.data').read().splitlines()
kl = [[],[]] # 0:keys, 1:locks
for i in range(0,len(schem),8): # iterate over schematic blocks (8 lines each)
    m = np.sum([np.array([1 if k=='#' else 0 for k in schem[i+j]]) for j in range(1,6)],0) # generate array of column heights
    kl[1 if schem[i] == '#####' else 0].append(m) # append to keys or locks depending on first line
print('Day25-1', np.count_nonzero(list(map(lambda e: np.all(e[0]+e[1]<6),itertools.product(*kl))))) # count all key/lock combinations that do not overlap (key+lock column sum < 6)
