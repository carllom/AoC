from scipy.optimize import fsolve
from math import floor,ceil

def solver(racedata):
    s=1
    for race in racedata:
        fn = lambda x,r=race: (r[0]-x) * x - r[1]
        slv = fsolve(fn,[1,race[0]-1])
        s*= floor(slv[1])-ceil(slv[0])+1
    return s

print('Day6-1',solver([(59,597),(79,1234),(65,1032),(75,1328)]))
print('Day6-2',solver([(59796575,597123410321328)]))