from itertools import groupby
rot_e = list(zip(*[x.split() for x in open('day6-1.data')][::-1]))  # rotate the grid element-wise
print('Day6-1:', sum(eval(row[0].join(row[1:])) for row in rot_e))  # dirty trick.. create string expression and eval

rot_c = [''.join(c).strip() for c in zip(*open('day6-1.data'))]  # rotate grid character-wise
# group by empty string and eval each group similar to 6-1, but first element has op as last char
print('Day6-2:', sum(eval(g[0][-1].join([g[0][:-1]] + g[1:])) for k, grp in groupby(rot_c, bool) if k for g in [[*grp]]))
