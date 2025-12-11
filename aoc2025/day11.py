# Graph is a DAG. I checked with networkx. So no cycles.
graph = {m[0][:-1]: m[1:] for m in [x.split() for x in open('day11-1.data', encoding='utf-8')]}
cache = {}  # path cache for count efficiency


def num_paths(g, start, end):
    if (start, end) in cache:
        return cache[(start, end)]
    if start == end:
        return 1
    if start == 'out':  # out has no outgoing edges, if it is not end, no path
        return 0
    np = sum(num_paths(g, next, end) for next in g[start])
    cache[(start, end)] = np
    return np


print('Day11-1:', num_paths(graph, 'you', 'out'))

# The whole path is start->probA->probB->end. Solve each separately and multiply the results.
# There are two possible orders to visit the problematic nodes: dac->fft or fft->dac
# but as the graph is a DAG only one will work, otherwise the graph would have cycles
tot = num_paths(graph, 'dac', 'fft')
if tot > 0:  # check if dac -> fft works
    tot *= num_paths(graph, 'svr', 'dac') * num_paths(graph, 'fft', 'out')
else:
    tot = num_paths(graph, 'fft', 'dac')  # this must be > 0 if dac->fft was 0
    tot *= num_paths(graph, 'svr', 'fft') * num_paths(graph, 'dac', 'out')
print('Day11-2:', tot)
